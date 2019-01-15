using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.Save;
using Sitecore.Web.UI.Sheer;
using System;


namespace Sitecore.Support.Pipelines.Save
{
  public class ValidateFields
  {
    public void Process(SaveArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.IsNotNull(args.Items, "args.Items");
      SaveArgs.SaveItem[] items = args.Items;
      foreach (SaveArgs.SaveItem saveItem in items)
      {
        Item item = Client.ContentDatabase.GetItem(saveItem.ID, saveItem.Language);
        if (item != null && !item.Paths.IsMasterPart && !StandardValuesManager.IsStandardValuesHolder(item))
        {
          SaveArgs.SaveField[] fields = saveItem.Fields;
          foreach (SaveArgs.SaveField saveField in fields)
          {
            // 301460
            Item fieldItem = Client.ContentDatabase.GetItem(saveField.ID, saveItem.Language);
            string fieldRegexValidationError = Sitecore.Support.Data.Fields.FieldUtil.GetFieldRegexValidationError(fieldItem, saveField.Value);
            if (!string.IsNullOrEmpty(fieldRegexValidationError))
            {
              if (args.HasSheerUI)
              {
                SheerResponse.Alert(fieldRegexValidationError, Array.Empty<string>());
                SheerResponse.SetReturnValue("failed");
              }
              args.AbortPipeline();
              return;
            }

          }
        }
      }
    }
  }
}