using Ref.Core.VM;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ref.Core
{
    public static class ErrorTable
    {
        public static void Add(int errorcode, string explanation)
        {
            if (!_data.ContainsKey(errorcode))
            {
                _data.Add(errorcode, explanation);
            }
        }

        public static string GetExplanation(int errorcode)
        {
            if (_data.ContainsKey(errorcode))
            {
                return _data[errorcode];
            }

            return string.Empty;
        }

        public static void ScanErrors()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var attr = type.GetCustomAttribute<ErrorAttribute>();

                if (attr != null)
                {
                    ErrorTable.Add(attr.ErrorCode, attr.Explanation);
                }
            }
        }

        private static Dictionary<int, string> _data = new Dictionary<int, string>();
    }
}