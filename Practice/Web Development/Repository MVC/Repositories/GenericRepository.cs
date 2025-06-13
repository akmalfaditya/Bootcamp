using Microsoft.EntityFrameworkCore;
using RepositoryMVC.Data;
using System.Linq.Expressions;

namespace RepositoryMVC.Repositories
{
    /// <summary>
    /// Generic Repository Implementation - The Heart of Repository Pattern
    /// 
    /// This class implements the IGenericRepository interface using Entity Framework Core.
    /// It provides concrete implementations for common database operations that work
    /// with any entity type. This eliminates the need to write the same CRUD operations
    /// for every entity in your application.
    /// 
    /// Key Design Patterns Used:
    /// 1. Repository Pattern - Encapsulates data access logic
    /// 2. Generic Programming - Works with any entity type
    /// 3. Dependency Injection - Receives DbContext through constructor
    /// 4. Async/Await - Non-blocking database operations for better performance
    /// </summary>
    /// <typeparam name="T">The entity type this repository will handle</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Constructor - Sets up the repository with database context
        /// We store both the context and the specific DbSet for our entity type
        /// This gives us both flexibility and performance
        /// </summary>
        /// <param name="context">Entity Framework database context</param>
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Get all entities - Simple but powerful
        /// ToListAsync() executes the query and returns all results
        /// Virtual keyword allows derived classes to override this behavior if needed
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Get entity by ID - The most common lookup operation
        /// FindAsync is optimized for primary key lookups
        /// Returns null if entity doesn't exist, which controllers can handle appropriately
        /// </summary>
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Find entities using LINQ expressions - Very powerful for custom queries
        /// Expression trees allow us to write type-safe queries that are translated to SQL
        /// Example usage: FindAsync(s => s.Name.Contains("John") && s.IsActive)
        /// </summary>
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Add a single entity - Stages for insertion
        /// AddAsync is used because some database providers support async key generation
        /// The entity isn't actually saved until SaveChanges is called
        /// </summary>
        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        /// <summary>
        /// Add multiple entities efficiently
        /// Much faster than calling AddAsync in a loop
        /// Useful for bulk data imports or seeding operations
        /// </summary>
        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// Update an entity - Entity Framework handles change tracking
        /// EF Core will detect which properties have changed and generate appropriate SQL
        /// This is more efficient than updating all columns
        /// </summary>
        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        /// <summary>
        /// Remove a single entity - Stages for deletion
        /// The entity must exist in the context for this to work
        /// If you only have an ID, use GetByIdAsync first, then Remove
        /// </summary>
        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Remove multiple entities efficiently
        /// Much faster than calling Remove in a loop
        /// Useful for bulk delete operations
        /// </summary>
        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Check existence without loading the full entity
        /// This is much more efficient than GetById when you only need to check existence
        /// Translates to SELECT 1 WHERE... SQL which is very fast
        /// </summary>
        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        /// <summary>
        /// Count entities efficiently
        /// If no predicate is provided, counts all entities
        /// Much more efficient than loading all entities and counting in memory
        /// </summary>
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.CountAsync();
            }
            return await _dbSet.CountAsync(predicate);
        }
    }
}
