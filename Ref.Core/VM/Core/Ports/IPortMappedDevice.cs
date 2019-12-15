namespace Ref.Core.VM.Core.Ports
{
    public interface IPortMappedDevice
    {
        VirtualMachine VM { get; set; }

        void HandleRead(int port, Registers reg);

        void HandleWrite(int port, int value);
    }
}