using System;

namespace Ref.Core
{
    public class RegisterCollection
    {
        public int Length => Enum.GetNames(typeof(Registers)).Length;
        public Register[] Register { get; set; }
        public VirtualMachine Vm { get; private set; }

        public RegisterCollection(VirtualMachine vm)
        {
            this.Vm = vm;

            Register = new Register[Enum.GetNames(typeof(Registers)).Length];
            InitRegisters();
        }

        public int this[Registers reg]
        {
            get
            {
                return Register[(int)reg].GetValue();
            }
            set
            {
                Register[(int)reg].SetValue(value);
            }
        }

        public Register this[int index]
        {
            get
            {
                return Register[index];
            }
        }

        public void ClearRegister(Registers value)
        {
            SetValue(value, 0);
        }

        public int GetValue(Registers reg)
        {
            return Register[BitConverter.ToInt32(new byte[] { (byte)reg, 0, 0, 0 }, 0)].GetValue();
        }

        public void SetValue(Registers value, int v)
        {
            var index = BitConverter.ToInt32(new byte[] { (byte)value, 0, 0, 0 }, 0);
            Register[index].SetValue(v);
        }

        public void Subscribe(Registers reg, Action<int> callback)
        {
            Register[(int)reg].OnChange = callback;
        }

        private void InitRegisters()
        {
            for (int i = 0; i < Register.Length; i++)
            {
                var reg = new Register();
                reg.Vm = Vm;
                reg.Access = Core.Register.RegisterAccess.Read | Core.Register.RegisterAccess.Write;

                Register[i] = reg;
            }

            Register[(int)Registers.F].Access = Register[(int)Registers.F].Access | Core.Register.RegisterAccess.Protected;
        }
    }
}