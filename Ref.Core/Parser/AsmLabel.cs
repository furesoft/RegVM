using System.Collections.Generic;

namespace Ref.Core.Parser
{
    public class AsmLabel
    {
        public List<AsmCommand> Commands { get; set; } = new List<AsmCommand>();

        public string Name { get; set; }

        public AsmLabel(string name)
        {
            Name = name;
        }
    }
}