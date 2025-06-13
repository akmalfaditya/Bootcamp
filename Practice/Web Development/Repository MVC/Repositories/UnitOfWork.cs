using Microsoft.EntityFrameworkCore.Storage;
using RepositoryMVC.Data;

namespace RepositoryMVC.Repositories
{
    /// <summary>
    /// Unit of Work Implementation - Central Coordinator for Repository Operations
    /// 
    /// This class implements the Unit of Work pattern, providing a single point of control
    /// for all data access operations. It coordinates multiple repositories and ensures
    /// that all changes are committed or rolled back as a single unit.
    /// 
    /// Key Features:
    /// 1. Lazy Loading - Repositories are created only when accessed
    /// 2. Single DbContext - All repositories share the same context for consistency
    /// 3. Transaction Management - Explicit control over database transactions
    /// 4. Resource Management - Proper disposal of database resources
    /// 5. Generic Repository Factory - Can create repositories for any entity type
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _currentTransaction;
        
        // Lazy-loaded repository instances
        private IStudentRepository? _studentRepository;
        private IGradeRepository? _gradeRepository;
        
        // Dictionary to store generic repositories for entities without specialized repositories
        private readonly Dictionary<Type, object> _genericRepositories = new();

        /// <summary>
        /// Constructor - Initialize with Entity Framework DbContext
        /// The DbContext is the foundation that all repositories will share
        /// </summary>
        /// <param name="context">Entity Framework database context</param>
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Student Repository Property - Lazy Loading Implementation
        /// 
        /// This property implements lazy loading - the repository is created only
        /// when first accessed. This improves performance by not creating objects
        /// that might never be used.
        /// </summary>
        public IStudentRepository Students
        {
            get
            {
                _studentRepository ??= new StudentRepository(_context);
                return _studentRepository;
            }
        }

        /// <summary>
        /// Grade Repository Property - Lazy Loading Implementation
        /// 
        /// Similar to Students property, this creates the GradeRepository only
        /// when first accessed. Both repositories share the same DbContext,
        /// ensuring they work with the same data and transaction scope.
        /// </summary>
        public IGradeRepository Grades
        {
            get
            {
                _gradeRepository ??= new GradeRepository(_context);
                return _gradeRepository;
            }
        }

        /// <summary>
        /// Generic Repository Factory - Create Repository for Any Entity Type
        /// 
        /// This method demonstrates the Factory pattern combined with generics.
        /// It creates and caches repositories for entity types that don't have
        /// specialized repositories. This provides flexibility while maintaining performance.
        /// 
        /// The dictionary caching ensures that multiple calls for the same entity type
        /// return the same repository instance, maintaining consistency.
        /// </summary>
        /// <typeparam name="T">Entity type for which to create a repository</typeparam>
        /// <returns>Generic repository for the specified entity type</returns>
        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            var entityType = typeof(T);
            
            if (_genericRepositories.ContainsKey(entityType))
            {
                return (IGenericRepository<T>)_genericRepositories[entityType];
            }

            var repository = new GenericRepository<T>(_context);
            _genericRepositories[entityType] = repository;
            return repository;
        }

        /// <summary>
        /// Save All Changes - The Heart of Unit of Work Pattern
        /// 
        /// This method commits all pending changes from all repositories in a single
        /// database transaction. This is what makes the Unit of Work pattern powerful -
        /// you can make changes through multiple repositories and commit them all at once.
        /// 
        /// If any operation fails, all changes are rolled back, ensuring data consistency.
        /// The return value indicates how many entities were affected by the operation.
        /// </summary>
        /// <returns>Number of entities written to the database</returns>
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch
            {
                // If save fails and we have an active transaction, roll it back
                if (_currentTransaction != null)
                {
                    await RollbackTransactionAsync();
                }
                throw; // Re-throw the exception to let the caller handle it
            }
        }

        /// <summary>
        /// Begin Database Transaction - Explicit Transaction Control
        /// 
        /// This method starts a database transaction for operations that need
        /// explicit transaction boundaries. Use this when you need more control
        /// over when changes are committed or when you need to coordinate
        /// operations across multiple SaveChangesAsync calls.
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Commit Transaction - Finalize All Changes
        /// 
        /// Commits all changes made within the current transaction to the database.
        /// After this call, all changes become permanent and visible to other
        /// database connections.
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No transaction in progress.");
            }

            try
            {
                await _currentTransaction.CommitAsync();
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        /// <summary>
        /// Rollback Transaction - Undo All Changes
        /// 
        /// Undoes all changes made within the current transaction. This is crucial
        /// for error handling - when something goes wrong, you can ensure that
        /// no partial or inconsistent data is saved to the database.
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No transaction in progress.");
            }

            try
            {
                await _currentTransaction.RollbackAsync();
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        /// <summary>
        /// Dispose Pattern Implementation - Clean Up Resources
        /// 
        /// This method ensures that database resources are properly cleaned up
        /// when the Unit of Work is no longer needed. It implements the Dispose
        /// pattern to release both managed and unmanaged resources.
        /// 
        /// The disposed flag prevents multiple disposal calls, which could cause errors.
        /// </summary>
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    _currentTransaction?.Dispose();
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Public Dispose Method - Called by consumers to clean up resources
        /// This is the method that external code calls when done with the Unit of Work
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
