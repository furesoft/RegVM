﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ref.Core.VM.Core.Ports
{
    public static class PortMappedDeviceManager
    {
        public static Dictionary<int, IPortMappedDevice> ReadPorts { get; set; } = new Dictionary<int, IPortMappedDevice>();
        public static Dictionary<int, IPortMappedDevice> WritePorts { get; set; } = new Dictionary<int, IPortMappedDevice>();

        public static bool IsRegistered(int out_addr)
        {
            return ReadPorts.ContainsKey(out_addr) || WritePorts.ContainsKey(out_addr);
        }

        public static void Read(int port, Registers reg)
        {
            if (ReadPorts.ContainsKey(port))
            {
                ReadPorts[port].HandleRead(port, reg);
                return;
            }

            throw new Exception($"Unmapped Port 0x{port.ToString("x")}");
        }

        public static void ScanDevices(VirtualMachine vm)
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.GetInterfaces().Contains(typeof(IPortMappedDevice)))
                {
                    var device = (IPortMappedDevice)Activator.CreateInstance(type);
                    var attrs = type.GetCustomAttributes<PortAttribute>();

                    device.VM = vm;

                    foreach (var att in attrs)
                    {
                        if (att.Access == PortAccess.Read)
                        {
                            ReadPorts.Add(att.Address, device);
                        }
                        else
                        {
                            WritePorts.Add(att.Address, device);
                        }
                    }
                }
            }
        }

        public static void Write(int port, int value)
        {
            if (WritePorts.ContainsKey(port))
            {
                WritePorts[port].HandleWrite(port, value);
                return;
            }

            throw new Exception($"Unmapped Port 0x{port.ToString("x")}");
        }
    }
}