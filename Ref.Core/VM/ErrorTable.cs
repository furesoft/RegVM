using System.Collections.Generic;

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

        private static Dictionary<int, string> _data = new Dictionary<int, string>();
    }
}