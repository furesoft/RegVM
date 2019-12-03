namespace Ref.Core.VM.Core.Ports
{
    public interface IPortMappedDevice
    {
        void HandleRead(int port, Registers reg, VirtualMachine vm);

        void HandleWrite(int port, int value, VirtualMachine vm);
    }
}