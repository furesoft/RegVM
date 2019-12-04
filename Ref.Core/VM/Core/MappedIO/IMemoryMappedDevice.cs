namespace Ref.Core.VM.IO.MappedIO
{
    public interface IMemoryMappedDevice
    {
        void HandleMemoryMapped(int address, int value, VirtualMachine vm);
    }
}