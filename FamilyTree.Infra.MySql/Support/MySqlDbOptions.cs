namespace FamilyTree.Infra.MySql.Support
{
    /// <summary>
    /// Injectable database options.
    /// </summary>
    public class MySqlDbOptions
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }
    }
}
