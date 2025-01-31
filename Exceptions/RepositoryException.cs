using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoSP.Net.Exceptions
{
    /// <summary>
    /// Custom exception class for repository-related errors.
    /// </summary>
    public class RepositoryException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public RepositoryException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
