//-----------------------------------------------------------------------
// <copyright file="RequiredWhenEnabledAttribute.cs" company="Jonas Schubert">
//     Copyright (c) Jonas Schubert. All rights reserved.
// </copyright>
// <author>Jonas Schubert</author>
//-----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace Snowflake.Data.Xt
{
  /// <summary>
  /// The attribute to check whether snowflake is enabled and then require the property.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
  public class RequiredWhenEnabledAttribute : RequiredAttribute
  {
    /// <summary>
    /// Checks whether a value is required.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A validation result.</returns>
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
      var snowflakeOptions = (SnowflakeOptions)validationContext.ObjectInstance;
      if (!snowflakeOptions.IsEnabled)
      {
        return ValidationResult.Success!;
      }

      if (value == null)
      {
        return new ValidationResult("Value is required.");
      }

#pragma warning disable IDE0046 // Convert to conditional expression
      if (!this.AllowEmptyStrings && value is string stringValue && string.IsNullOrWhiteSpace(stringValue))
      {
        return new ValidationResult("Value is required.");
      }
#pragma warning restore IDE0046 // Convert to conditional expression

      return ValidationResult.Success!;
    }
  }
}
