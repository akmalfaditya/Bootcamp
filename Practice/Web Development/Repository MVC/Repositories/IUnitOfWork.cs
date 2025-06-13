using RepositoryMVC.Repositories;

namespace RepositoryMVC.Repositories
{
    /// <summary>
    /// Unit of Work Interface - Orchestrates Repository Operations
    /// 
    /// The Unit of Work pattern ensures that multiple repository operations
    /// can be treated as a single transaction. This is crucial for maintaining
    /// data consistency when operations involve multiple entities.
    /// 
    /// Key Benefits:
    /// 1. Transaction Management - All changes are committed or rolled back together
    /// 2. Single Point of Control - One place to manage all repository operations
    /// 3. Improved Performance - Batches multiple database operations
    /// 4. Data Consistency - Ensures related changes happen atomically
    /// 
    /// Example Scenario:
    /// When creating a new student and their initial grades, you want both
    /// operations to succeed or both to fail. The Unit of Work ensures this atomicity.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Student Repository - Access to all student-related data operations
        /// This property provides access to the specialized StudentRepository
        /// through the IStudentRepository interface
        /// </summary>
        IStudentRepository Students { get; }

        /// <summary>
        /// Grade Repository - Access to all grade-related data operations
        /// This property provides access to the specialized GradeRepository
        /// through the IGradeRepository interface
        /// </summary>
        IGradeRepository Grades { get; }

        /// <summary>
        /// Generic Repository Factory - Create repositories for any entity type
        /// 
        /// This method allows you to get a repository for any entity type that
        /// doesn't have a specialized repository. It's useful for entities that
        /// only need basic CRUD operations.
        /// 
        /// Example: var categoryRepo = GetRepository<Category>();
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <returns>Generic repository for the specified entity type</returns>
        IGenericRepository<T> GetRepository<T>() where T : class;

        /// <summary>
        /// Save all changes to the database
        /// 
        /// This method commits all pending changes from all repositories in a single transaction.
        /// It's the key method that makes the Unit of Work pattern work - all changes are
        /// batched and committed together, ensuring data consistency.
        /// 
        /// Returns the number of affected records, which can be useful for
        /// validation and logging purposes.
        /// </summary>
        /// <returns>Number of entities written to the database</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Begin a database transaction
        /// 
        /// For complex operations that need explicit transaction control,
        /// this method allows you to start a database transaction manually.
        /// Use this when you need more control over when the transaction
        /// is committed or rolled back.
        /// </summary>
        /// <returns>Database transaction object</returns>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commit the current transaction
        /// 
        /// Commits all changes made within the current transaction.
        /// Should be called after BeginTransactionAsync() when all
        /// operations have completed successfully.
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rollback the current transaction
        /// 
        /// Undoes all changes made within the current transaction.
        /// Should be called when an error occurs and you want to
        /// ensure no partial changes are saved to the database.
        /// </summary>
        Task RollbackTransactionAsync();
    }
}
