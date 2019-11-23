using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ref.Core.VM.Core.Interrupts
{
    public static class InterruptTable
    {
        public static Dictionary<int, IInterruptHandler> Handlers { get; set; } = new Dictionary<int, IInterruptHandler>();

        public static void Interrupt(int number, VirtualMachine vm)
        {
            if (Handlers.ContainsKey(number))
            {
                Handlers[number].Handle(vm);
            }

            throw new Exception($"Interrupt {number} has no handler registered");
        }

        public static void ScanHandlers()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.GetInterfaces().Contains(typeof(IInterruptHandler)))
                {
                    var handler = (IInterruptHandler)Activator.CreateInstance(type);
                    var attr = type.GetCustomAttribute<InterruptAttribute>();

                    Handlers.Add(attr.InterruptNumber, handler);
                }
            }
        }
    }
}