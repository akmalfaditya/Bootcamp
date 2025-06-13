using System.Linq.Expressions;

namespace RepositoryMVC.Repositories
{
    /// <summary>
    /// Generic Repository Interface - The Foundation of Repository Pattern
    /// 
    /// This interface defines common database operations that work with any entity type.
    /// The Repository Pattern provides a uniform interface for accessing data,
    /// abstracting the underlying data access technology (Entity Framework, in our case).
    /// 
    /// Benefits of using Generic Repository:
    /// 1. Reduces code duplication - write CRUD operations once, use for all entities
    /// 2. Enforces consistency - all repositories follow the same pattern
    /// 3. Makes testing easier - can easily mock repository operations
    /// 4. Provides abstraction - business logic doesn't depend on specific data access technology
    /// </summary>
    /// <typeparam name="T">The entity type this repository will handle</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Get all entities of type T from the database
        /// This is the most basic read operation
        /// </summary>
        /// <returns>Collection of all entities</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Get a specific entity by its primary key
        /// Most entities have an integer ID as primary key
        /// </summary>
        /// <param name="id">Primary key value</param>
        /// <returns>Entity if found, null otherwise</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Find entities that match specific criteria
        /// This method uses Expression trees to build dynamic queries
        /// Example: Find(s => s.Name.Contains("John"))
        /// </summary>
        /// <param name="predicate">Expression that defines the search criteria</param>
        /// <returns>Collection of matching entities</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Add a new entity to the database
        /// This stages the entity for insertion - you still need to call SaveAsync()
        /// </summary>
        /// <param name="entity">Entity to add</param>
        /// <returns>Task representing the async operation</returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Add multiple entities in a single operation
        /// More efficient than calling AddAsync multiple times
        /// </summary>
        /// <param name="entities">Collection of entities to add</param>
        /// <returns>Task representing the async operation</returns>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Update an existing entity
        /// Entity Framework will track changes and update only modified fields
        /// </summary>
        /// <param name="entity">Entity with updated values</param>
        void Update(T entity);

        /// <summary>
        /// Remove an entity from the database
        /// This stages the entity for deletion - you still need to call SaveAsync()
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        void Remove(T entity);

        /// <summary>
        /// Remove multiple entities in a single operation
        /// More efficient than calling Remove multiple times
        /// </summary>
        /// <param name="entities">Collection of entities to remove</param>
        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Check if any entity matches the given criteria
        /// Useful for validation (e.g., check if email already exists)
        /// More efficient than getting the full entity when you just need to check existence
        /// </summary>
        /// <param name="predicate">Expression that defines the search criteria</param>
        /// <returns>True if at least one entity matches, false otherwise</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get the count of entities that match the criteria
        /// Useful for pagination or statistics
        /// </summary>
        /// <param name="predicate">Expression that defines the search criteria (optional)</param>
        /// <returns>Number of matching entities</returns>
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    }
}
