using System.Collections.Generic;

namespace Ref.Core.VM
{
    public class Debugger
    {
        public List<int> Breakpoints { get; set; } = new List<int>();

        public void Break(int index)
        {
            Breakpoints.Add(index);
        }

        public bool HasBreakPoint(int index)
        {
            return Breakpoints.Contains(index);
        }

        public void Step()
        {
        }
    }
}