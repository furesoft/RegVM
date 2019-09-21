using System;
using System.Linq;

namespace Ref.Core
{
    public class ReplCommand
    {
        public string[] Args { get; set; }
        public string Name { get; set; }

        public static bool IsCommand(string src)
        {
            return src.Trim().StartsWith(".");
        }

        public static ReplCommand Parse(string src)
        {
            var name = src.Split(' ')[0].Substring(1);
            var argsSrc = src.Substring(name.Length + 1);
            var argsSplt = argsSrc.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();

            return new ReplCommand { Name = name, Args = argsSplt };
        }
    }
}