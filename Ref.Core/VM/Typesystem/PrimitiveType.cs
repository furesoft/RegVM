namespace Ref.Core.VM.Typesystem
{
    public class PrimitiveType<TName> : VmType
    {
        public PrimitiveType()
        {
            Name = typeof(TName).Name;
        }
    }
}