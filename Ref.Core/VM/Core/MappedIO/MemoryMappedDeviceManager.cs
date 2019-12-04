using Ref.Core.VM.IO.MappedIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ref.Core.VM.Core.MappedIO
{
    public class MemoryMappedDeviceManager
    {
        public static void ScanDevices()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.GetInterfaces().Contains(typeof(IMemoryMappedDevice)))
                {
                    var device = (IMemoryMappedDevice)Activator.CreateInstance(type);
                    var attr = type.GetCustomAttribute<AddressRangeAttribute>();

                    if (attr != null)
                    {
                        Devices.Add(attr, device);
                    }
                }
            }
        }

        public static void Write(int out_addr, int out_value, VirtualMachine vm)
        {
            foreach (var device in Devices)
            {
                if (out_addr > device.Key.Start && out_addr < device.Key.End)
                {
                    var translated_addr = out_addr - device.Key.Start;
                    device.Value.HandleMemoryMapped(translated_addr, out_value, vm);
                    break;
                }
            }
        }

        private static Dictionary<AddressRangeAttribute, IMemoryMappedDevice> Devices = new Dictionary<AddressRangeAttribute, IMemoryMappedDevice>();
    }
}