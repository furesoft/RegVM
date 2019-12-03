using System;

namespace Ref.Core.VM.Core.Ports
{
    public enum PortAccess { Read, Write }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class PortAttribute : Attribute
    {
        public PortAccess Access { get; set; }

        public int Address { get; set; }

        public PortAttribute(int address, PortAccess access)
        {
            Address = address;
            Access = access;
        }
    }
}