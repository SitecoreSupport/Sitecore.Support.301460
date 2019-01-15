namespace Sitecore.Support.Data.Fields
{
  using Sitecore;
  using Sitecore.Configuration;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;
  using System;
  using System.Text.RegularExpressions;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  public static class FieldUtil
  {
    public static readonly string EmailValidationMarker = "EmailValidation";
    // 301460
    public static string GetFieldRegexValidationError(Item fieldItem, string valueToValidate)
    {
      Assert.ArgumentNotNull(fieldItem, "fieldItem");
      Assert.ArgumentNotNull(valueToValidate, "valueToValidate");
      string validation = fieldItem.Fields["Validation"].ToString();
      if (string.IsNullOrEmpty(validation))
      {
        return string.Empty;
      }
      if (validation == EmailValidationMarker)
      {
        validation = FallbackEmailRegexPattern;
      }
      if (Regex.IsMatch(valueToValidate, validation, RegexOptions.Singleline))
      {
        return string.Empty;
      }
      string validationText = fieldItem.Fields["ValidationText"].ToString();
      if (string.IsNullOrEmpty(validationText))
      {
        validationText = "'$Value' is not a valid value.";
      }
      return Translate.Text(validationText).Replace("$Value", valueToValidate);
    }

    public static string FallbackEmailRegexPattern
    {
      get
      {
        string[] values = new string[] { Settings.EmailValidation, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" };
        return StringUtil.GetString(values);
      }
    }
  }
}