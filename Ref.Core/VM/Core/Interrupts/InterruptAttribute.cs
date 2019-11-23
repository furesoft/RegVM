using System;

namespace Ref.Core.VM.Core.Interrupts
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InterruptAttribute : Attribute
    {
        public int InterruptNumber { get; set; }

        public InterruptAttribute(int interruptNumber)
        {
            InterruptNumber = interruptNumber;
        }
    }
}