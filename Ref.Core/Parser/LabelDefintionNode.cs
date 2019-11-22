using System.Collections.Generic;

namespace Ref.Core.Parser
{
    public class LabelDefintionNode
    {
        public List<AsmCommand> Body { get; set; }
        public string Name { get; set; }
    }
}