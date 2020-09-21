using System;
using System.Text;
using PTMS.Core.Models;

namespace PTMS.Core.Extentions {
    public static class TemplateExtentions {
        public static string ToCacheKey(this Template template) {
            return $"{template.Id}_{template.Version}";
        }

        public static string ToCacheKey(this string id, string version) {
            return $"{id}_{version}";
        }
    }
}
