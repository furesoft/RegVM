using CommandLine;

namespace RefVm_Compiler
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "File to compile")]
        public string Input { get; set; }

        [Option('o', "output", Required = true, HelpText = "The output file name")]
        public string Output { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
    }
}