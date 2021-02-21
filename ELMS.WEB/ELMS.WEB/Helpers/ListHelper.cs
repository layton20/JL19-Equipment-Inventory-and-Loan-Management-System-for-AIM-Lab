using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELMS.WEB.Helpers
{
    public static class ListHelper
    {
        public static IEnumerable<SelectListItem> GetEnumSelectList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = x.ToString()
                }).ToList();
        }
    }
}