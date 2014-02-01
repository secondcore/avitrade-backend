using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vimba.AviTrade.Web.Helpers
{
    /// <summary>
    /// This class is copied from some MVC project sample on Microsoft site
    /// </summary>
    public static class ModelStateHelper
    {
        public const string TempDataModelStateKey = "__YesrWeb_ModelStateTemp__";

        public static void ModelStateFromTemp(this Controller controller)
        {
            ModelStateDictionary modelState = controller.ViewData.ModelState;
            TempDataDictionary tempData = controller.TempData;

            if (!tempData.ContainsKey(TempDataModelStateKey))
            {
                return;
            }

            ModelStateDictionary fromTempData = tempData[TempDataModelStateKey]
                                                    as ModelStateDictionary;
            if (fromTempData == null)
            {
                return;
            }

            foreach (KeyValuePair<string, ModelState> pair in fromTempData)
            {
                if (modelState.ContainsKey(pair.Key))
                {
                    modelState[pair.Key].Value = pair.Value.Value;

                    foreach (ModelError error in pair.Value.Errors)
                    {
                        modelState[pair.Key].Errors.Add(error);
                    }
                }
                else
                {
                    modelState.Add(pair.Key, pair.Value);
                }
            }
        }

        public static void ModelStateToTemp(this Controller controller)
        {
            controller.TempData[TempDataModelStateKey] = controller.ViewData.ModelState;
        }
    }
}