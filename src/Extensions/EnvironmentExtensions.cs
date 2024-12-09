//-----------------------------------------------------------------------
// <copyright file="EnvironmentExtensions.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>Jonas Schubert</author>
//-----------------------------------------------------------------------

namespace Snowflake.Data.Xt
{
  /// <summary>
  /// The static environment extensions class.
  /// </summary>
  public static class EnvironmentExtensions
  {
    /// <summary>
    /// Gets the snowflake connection string from environment variables.
    /// </summary>
    /// <exception cref="ArgumentNullException">If any environment variable is missing, an ArgumentNullException will be thrown.</exception>
    /// <returns>The snowflake connection string.</returns>
    public static string GetSnowflakeConnectionString()
    {
      var account = Environment.GetEnvironmentVariable("SNOWFLAKE_ACCOUNT");
      if (string.IsNullOrWhiteSpace(account))
      {
        throw new ArgumentNullException("Missing snowflake connection env var SNOWFLAKE_ACCOUNT!");
      }

      var user = Environment.GetEnvironmentVariable("SNOWFLAKE_USER");
      if (string.IsNullOrWhiteSpace(account))
      {
        throw new ArgumentNullException("Missing snowflake connection env var SNOWFLAKE_USER!");
      }

      var privateKeyPassword = Environment.GetEnvironmentVariable("SNOWFLAKE_PRIVATE_KEY_PASSWORD");
      if (string.IsNullOrWhiteSpace(account))
      {
        throw new ArgumentNullException("Missing snowflake connection env var SNOWFLAKE_PRIVATE_KEY_PASSWORD!");
      }

      var authenticator = Environment.GetEnvironmentVariable("SNOWFLAKE_AUTHENTICATOR");
      if (string.IsNullOrWhiteSpace(account))
      {
        throw new ArgumentNullException("Missing snowflake connection env var SNOWFLAKE_AUTHENTICATOR!");
      }

      var privateKeyFile = Environment.GetEnvironmentVariable("SNOWFLAKE_PRIVATE_KEY_FILE");
      if (string.IsNullOrWhiteSpace(account))
      {
        throw new ArgumentNullException("Missing snowflake connection env var SNOWFLAKE_PRIVATE_KEY_FILE!");
      }

      var warehouse = Environment.GetEnvironmentVariable("SNOWFLAKE_WAREHOUSE") ?? string.Empty;

      return $"account={account};user={user};private_key_pwd={privateKeyPassword};authenticator={authenticator};private_key_file={privateKeyFile};warehouse={warehouse}";
    }
  }
}
