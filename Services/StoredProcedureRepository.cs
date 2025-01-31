using RepoSP.Net.Exceptions;
using Microsoft.Data.SqlClient;
using RepoSP.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace RepoSP.Net.Services
{
    /// <summary>
    /// Repository implementation for managing stored procedure-based operations for entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of entity to manage.</typeparam>
    public class StoredProcedureRepository<T> : IRepository<T> where T : class
    {
        private readonly string _connectionString;
        private readonly IStoredProcConfiguration<T> _procConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcedureRepository{T}"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string for the database.</param>
        /// <param name="procConfiguration">The stored procedure configuration for the entity.</param>
        public StoredProcedureRepository(string connectionString, IStoredProcConfiguration<T> procConfiguration)
        {
            _connectionString = connectionString;
            _procConfiguration = procConfiguration;
        }

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        /// <param name="Id">The ID of the entity to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public virtual async Task<bool> Delete(int Id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(_procConfiguration.DeleteProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue($"@{_procConfiguration.IdParameterName}", Id);

                        // Add return value parameter
                        SqlParameter returnValue = new SqlParameter
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        command.Parameters.Add(returnValue);

                        connection.Open();
                        await command.ExecuteNonQueryAsync();

                        return (int)returnValue.Value == 1;

                    }
                }
            }
            catch (SqlException sqlex)
            {
                throw new RepositoryException($"Database error while deleting entity with ID {Id}: {sqlex.Message}", sqlex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Unexpected error while deleting entity with ID {Id}.", ex);
            }
        }

        /// <summary>
        /// Retrieves all entities asynchronously.
        /// </summary>
        /// <returns>A collection of all entities of type <typeparamref name="T"/>.</returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var listEntites = new List<T>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(_procConfiguration.GetAllProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                listEntites.Add(_procConfiguration.MapEntity(reader));
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlex)
            {
                throw new RepositoryException("Database error while retrieving all entities.", sqlex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Unexpected error while retrieving all entities.", ex);
            }
            return listEntites;
        }

        /// <summary>
        /// Retrieves an entity by its ID asynchronously.
        /// </summary>
        /// <param name="Id">The ID of the entity to retrieve.</param>
        /// <returns>The entity of type <typeparamref name="T"/>, or null if not found.</returns>
        public virtual async Task<T?> GetByIdAsync(int Id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(_procConfiguration.GetByIdProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue($"@{_procConfiguration.IdParameterName}", Id);

                        connection.Open();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                                return _procConfiguration.MapEntity(reader);
                        }
                    }
                }
            }
            catch (SqlException sqlex)
            {
                throw new RepositoryException($"Database error while retrieving entity with ID {Id}.", sqlex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Unexpected error while retrieving entity with ID {Id}.", ex);
            }
            return null;
        }

        /// <summary>
        /// Inserts a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>The inserted entity with its assigned ID, or null if the operation failed.</returns>
        public virtual async Task<T?> InsertAsync(T entity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(_procConfiguration.InsertProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        _procConfiguration.SetInsertParameters(command, entity);
                        connection.Open();

                        await command.ExecuteNonQueryAsync();
                        int id = Convert.ToInt32(command.Parameters[$"@{_procConfiguration.Id_Output_ParameterName}"].Value);

                        
                        if (id > 0)
                            return await GetByIdAsync(id);


                    }
                }
            }
            catch (SqlException sqlex)
            {
                throw new RepositoryException("Database error while inserting a new entity.", sqlex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Unexpected error while inserting a new entity.", ex);
            }
            return null;
        }

        /// <summary>
        /// Checks if a record with the specified ID exists in the database.
        /// </summary>
        /// <param name="Id">The ID of the record to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <c>true</c> if the record exists; otherwise, <c>false</c>.</returns>
        /// <exception cref="SqlException">Thrown when a database-related error occurs.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public async Task<bool> IsExistsAsync(int Id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(_procConfiguration.IsExistsByIdProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue($"@{_procConfiguration.IdParameterName}", Id);

                        // Add return value parameter
                        SqlParameter returnValue = new SqlParameter
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        command.Parameters.Add(returnValue);

                        connection.Open();
                        await command.ExecuteNonQueryAsync();

                        return (int)returnValue.Value == 1;
                    }
                }
            }
            catch (SqlException sqlex)
            {
                throw new RepositoryException($"Database error while checking existence of entity with ID {Id}.", sqlex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Unexpected error while checking existence of entity with ID {Id}.", ex);
            }
        }

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public virtual async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(_procConfiguration.UpdateProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        _procConfiguration.SetUpdateParameters(command, entity);

                        // Add return value parameter
                        SqlParameter returnValue = new SqlParameter
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        command.Parameters.Add(returnValue);

                        connection.Open();
                        await command.ExecuteNonQueryAsync();

                        return (int)returnValue.Value == 1;

                    }
                }
            }
            catch (SqlException sqlex)
            {
                throw new RepositoryException("Database error while updating the entity.", sqlex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Unexpected error while updating the entity.", ex);
            }
        }
    }
}
