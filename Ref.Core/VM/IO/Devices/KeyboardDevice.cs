using Ref.Core.VM.Core.Ports;
using Ref.Core.VM.IO.MappedIO;

namespace Ref.Core.VM.IO.Devices
{
    [Port(0xBCD, PortAccess.Write)] //Control Port
    [Port(0xBCD1, PortAccess.Read)] // Data Access Port - isInputAvailable
    internal class KeyboardDevice : IPortMappedDevice
    {
        public KeyboardDevice()
        {
            //ToDo: init GlobalSystemKeyboard hook, when new input is available push it on stack
        }

        public void HandleRead(int port, Registers reg, VirtualMachine vm)
        {
            if (port == 0xBCD1)
            {
                vm.Register[Registers.KDR] = isInputAvailable ? 1 : 0;
            }
        }

        public void HandleWrite(int port, int value, VirtualMachine vm)
        {
            throw new System.NotImplementedException();
        }

        private bool isInputAvailable = false;
    }
}