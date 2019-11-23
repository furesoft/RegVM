namespace Ref.Core.VM.Core.Ports
{
    public interface IPortMappedDevice
    {
        void HandleRead(int addr, Register reg);

        void HandleWrite(int addr, int value, Stack stack);
    }
}