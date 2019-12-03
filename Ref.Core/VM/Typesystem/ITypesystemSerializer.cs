namespace Ref.Core.VM.Typesystem
{
    public interface ITypesystemSerializer
    {
        void Deserialize(byte[] raw);

        byte[] Serizalize();
    }
}