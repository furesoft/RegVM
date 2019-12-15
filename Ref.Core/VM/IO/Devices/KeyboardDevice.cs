using Ref.Core.VM.Core.Ports;
using Ref.Core.VM.IO.MappedIO;

namespace Ref.Core.VM.IO.Devices
{
    [Port(0xBCD, PortAccess.Write)] //Control Port
    [Port(0xBCD1, PortAccess.Read)] // Data Access Port - isInputAvailable
    internal class KeyboardDevice : IPortMappedDevice
    {
        public VirtualMachine VM { get; set; }

        public KeyboardDevice()
        {
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
        }

        public void Dispose()
        {
            _globalKeyboardHook?.Dispose();
        }

        public void HandleRead(int port, Registers reg)
        {
            if (port == 0xBCD1)
            {
                VM.Register[Registers.KDS] = isInputAvailable ? 1 : 0;
                isInputAvailable = false;
            }
        }

        public void HandleWrite(int port, int value)
        {
            throw new System.NotImplementedException();
        }

        private GlobalKeyboardHook _globalKeyboardHook;

        private bool isInputAvailable = false;

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyboardData.VirtualCode != GlobalKeyboardHook.VkSnapshot)
                return;
            isInputAvailable = true;

            VM.Register[Registers.KDR] = e.KeyboardData.VirtualCode;
        }
    }
}