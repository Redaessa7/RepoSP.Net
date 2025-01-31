using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepoSP.Net.Interfaces
{
    /// <summary>
    /// Generic repository interface for managing entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of entity managed by the repository.</typeparam>
    internal interface IRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves an entity by its ID asynchronously.
        /// </summary>
        /// <param name="Id">The ID of the entity to retrieve.</param>
        /// <returns>
        /// A task representing the asynchronous operation. 
        /// The task result contains the entity of type <typeparamref name="T"/> if found; otherwise, null.
        /// </returns>
        Task<T?> GetByIdAsync(int Id);

        /// <summary>
        /// Retrieves all entities asynchronously.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains a collection of all entities of type <typeparamref name="T"/>.
        /// </returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Inserts a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>
        /// A task representing the asynchronous operation. 
        /// The task result contains the inserted entity of type <typeparamref name="T"/> if successful; otherwise, null.
        /// </returns>
        Task<T?> InsertAsync(T entity);

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains a boolean indicating whether the update was successful.
        /// </returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        /// <param name="Id">The ID of the entity to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        Task<bool> Delete(int Id);

        /// <summary>
        /// Checks if a record with the specified ID exists in the database.
        /// </summary>
        /// <param name="Id">The ID of the record to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <c>true</c> if the record exists; otherwise, <c>false</c>.</returns>
        Task<bool> IsExistsAsync(int Id);
    }
}
