using Microsoft.Data.SqlClient;
using System;

namespace RepoSP.Net.Interfaces
{
    /// <summary>
    /// Defines a configuration interface for managing stored procedures for a specific entity type.
    /// </summary>
    /// <typeparam name="T">The type of the entity associated with the stored procedures.</typeparam>
    public interface IStoredProcConfiguration<T> where T : class
    {
        /// <summary>
        /// Gets the name of the parameter used to pass the ID in the stored procedure.
        /// This property returns the parameter name without the '@' prefix.
        /// Example: "IdName" instead of "@IdName".
        /// </summary>
        string IdParameterName { get; }

        /// <summary>
        /// Gets the name of the output parameter used to return the ID from the stored procedure.
        /// This property returns the parameter name without the '@' prefix.
        /// Example: "OutputId" instead of "@OutputId".
        /// </summary>
        string Id_Output_ParameterName { get; }

        /// <summary>
        /// Gets the name of the stored procedure used to retrieve an entity by its ID.
        /// </summary>
        string GetByIdProcedure { get; }

        /// <summary>
        /// Gets the name of the stored procedure used to retrieve all entities.
        /// </summary>
        string GetAllProcedure { get; }

        /// <summary>
        /// Gets the name of the stored procedure used to insert a new entity.
        /// </summary>
        string InsertProcedure { get; }

        /// <summary>
        /// Gets the name of the stored procedure used to update an existing entity.
        /// </summary>
        string UpdateProcedure { get; }

        /// <summary>
        /// Gets the name of the stored procedure used to delete an entity.
        /// </summary>
        string DeleteProcedure { get; }

        /// <summary>
        /// Gets the name of the stored procedure used to check if a record exists by ID.
        /// </summary>
        string IsExistsByIdProcedure { get; }

        /// <summary>
        /// Configures the parameters required for the insert stored procedure.
        /// </summary>
        /// <param name="command">The SQL command to configure.</param>
        /// <param name="entity">The entity to insert.</param>
        void SetInsertParameters(SqlCommand command, T entity);

        /// <summary>
        /// Configures the parameters required for the update stored procedure.
        /// </summary>
        /// <param name="command">The SQL command to configure.</param>
        /// <param name="entity">The entity to update.</param>
        void SetUpdateParameters(SqlCommand command, T entity);

        /// <summary>
        /// Maps a database record to an entity.
        /// </summary>
        /// <param name="reader">The SQL data reader containing the record.</param>
        /// <returns>The mapped entity of type <typeparamref name="T"/>.</returns>
        T MapEntity(SqlDataReader reader);

        ///// <summary>
        ///// Maps an entity with a given ID.
        ///// </summary>
        ///// <param name="Id">The ID of the entity to map.</param>
        ///// <param name="entity">The entity to map the ID to.</param>
        ///// <returns>The updated entity with the mapped ID.</returns>
        //T MapEntity(int Id, T entity);
    }
}
