using System.Collections.Generic;

namespace Ref.Core.Parser
{
    public class AsmFile
    {
        public List<AsmCommand> Commands { get; set; } = new List<AsmCommand>();
        public List<AsmCommand> DataCommands { get; set; } = new List<AsmCommand>();
        public List<AsmLabel> Labels { get; set; } = new List<AsmLabel>();

        public static AsmFile Create()
        {
            return new AsmFile();
        }
    }

    public class AsmLabel
    {
        public List<AsmCommand> Commands { get; set; }
        public string Name { get; set; }
    }
}