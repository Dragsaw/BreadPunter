using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using BP.WebUI.Infrastructure.Filters;

namespace BP.WebUI.Models
{
    public class LanguagesViewModel
    {
        public string Current { get; set; }
        public IEnumerable<CultureInfo> Languages { get; set; }

        public LanguagesViewModel()
        {
            Languages = CultureAttribute.Cultures.Select(x =>
                CultureInfo.GetCultureInfo(x));
            Current = CultureInfo.CurrentCulture.NativeName;
        }
    }
}