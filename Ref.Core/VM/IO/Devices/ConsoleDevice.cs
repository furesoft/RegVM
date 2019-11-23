using Ref.Core.VM.Core.Ports;
using System;

namespace Ref.Core.VM.IO.Devices
{
    [Port(0xABC, PortAccess.Write)] //Control Port
    public class ConsoleDevice : IPortMappedDevice
    {
        public void HandleRead(int addr, Register reg)
        {
            throw new System.NotImplementedException();
        }

        public void HandleWrite(int addr, int controlVal, Stack stack)
        {
            switch (controlVal)
            {
                case 0:
                    Console.Clear();
                    break;

                case 1:
                    Console.Write((char)stack.Pop());
                    break;
            }
        }
    }
}