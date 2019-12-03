using CommandLine;
using System.Collections.Generic;

namespace Ref_Compiler
{
    public class Options
    {
        public Dictionary<string, object> Tags = new Dictionary<string, object>();

        [Option('i', "input", Required = true, HelpText = "File to compile")]
        public string Input { get; set; }

        [Option('o', "output", Required = true, HelpText = "The output file name")]
        public string Output { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
    }
}