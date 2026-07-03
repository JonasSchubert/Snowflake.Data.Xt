//-----------------------------------------------------------------------
// <copyright file="SnowflakeOptions.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>Jonas Schubert</author>
//-----------------------------------------------------------------------

using Microsoft.Extensions.Configuration;

namespace Snowflake.Data.Xt
{
  /// <summary>
  /// The snowflake options.
  /// </summary>
  public class SnowflakeOptions
  {
    /// <summary>
    /// Gets or sets the snowflake account.
    /// </summary>
    [ConfigurationKeyName("SNOWFLAKE_ACCOUNT")]
    [RequiredWhenEnabled]
    public required string Account { get; set; }

    /// <summary>
    /// Gets or sets the snowflake authenticator.
    /// </summary>
    [ConfigurationKeyName("SNOWFLAKE_AUTHENTICATOR")]
    [RequiredWhenEnabled]
    public required string Authenticator { get; set; }

    /// <summary>
    /// Gets or sets the snowflake database.
    /// </summary>
    [ConfigurationKeyName("SNOWFLAKE_DATABASE")]
    public string? Database { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether snowflake is enabled.
    /// </summary>
    [ConfigurationKeyName("SNOWFLAKE_ENABLED")]
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the snowflake private key file.
    /// </summary>
    [ConfigurationKeyName("SNOWFLAKE_PRIVATE_KEY_FILE")]
    [RequiredWhenEnabled]
    public required string PrivateKeyFile { get; set; }

    /// <summary>
    /// Gets or sets the snowflake private key password.
    /// </summary>
    [ConfigurationKeyName("SNOWFLAKE_PRIVATE_KEY_PASSWORD")]
    [RequiredWhenEnabled]
    public required string PrivateKeyPassword { get; set; }

    /// <summary>
    /// Gets or sets the snowflake schema.
    /// </summary>
    [ConfigurationKeyName("SNOWFLAKE_SCHEMA")]
    public string? Schema { get; set; }

    /// <summary>
    /// Gets or sets the snowflake user.
    /// </summary>
    [ConfigurationKeyName("SNOWFLAKE_USER")]
    [RequiredWhenEnabled]
    public required string User { get; set; }

    /// <summary>
    /// Gets or sets the snowflake warehouse.
    /// </summary>
    [ConfigurationKeyName("SNOWFLAKE_WAREHOUSE")]
    public string? Warehouse { get; set; }
  }
}
