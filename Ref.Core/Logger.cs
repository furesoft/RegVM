using System;
using System.IO;

namespace Ref.Core
{
    public static class Logger
    {
        public static void Log(string message)
        {
            File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\vm.txt", $"{DateTime.Now}: {message}\n\r");
        }
    }
}