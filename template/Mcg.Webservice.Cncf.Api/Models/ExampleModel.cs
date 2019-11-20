using System;
using Newtonsoft.Json;

namespace Mcg.Webservice.Cncf.Api.Models
{
    /// <summary>
    /// Represents a domain model that is used by the service.
    /// </summary>
    /// <remarks>
    /// This is merely an example implementation.  It can be reused or deleted as needed.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ExampleModel : IEquatable<ExampleModel>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        //[JsonProperty("id"), JsonRequired]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>The email address.</value>
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if the object passed is the same as the <see cref="DefaultModel"/> passed in.
        /// </summary>
        ///<remarks>
        /// All properties must be unique for a ExampleModel.  ID, username, and emailaddress must all be unique.
        ///</remarks>
        /// <param name="other">The value to compare against.</param>
        /// <returns></returns>
        public bool Equals(ExampleModel other)
        {
            if (other == null) { return false; }

            return (
                this.ID == other.ID
                || this.Username.Equals(other.Username, StringComparison.InvariantCultureIgnoreCase)
                || this.EmailAddress.Equals(other.EmailAddress, StringComparison.InvariantCultureIgnoreCase)
            );
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ExampleModel);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
