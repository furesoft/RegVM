using System;

namespace Ref.Core
{
    public struct Register
    {
        public Action<int> OnChange;
        public RegisterAccess Access { get; set; }
        public VM Vm { get; set; }

        [Flags]
        public enum RegisterAccess { Read, Write, Protected }

        public int GetValue()
        {
            if (Access.HasFlag(RegisterAccess.Read))
            {
                return value;
            }

            return 0;
        }

        public void Increment()
        {
            value++;
        }

        public void SetValue(int value)
        {
            if (Access.HasFlag(RegisterAccess.Write))
            {
                if (!Access.HasFlag(RegisterAccess.Protected))
                {
                    this.value = value;
                    //ToDo: call changed event
                    OnChange?.Invoke(value);
                }
                else
                {
                    Vm.Register[Registers.ERR] = 1;
                }
            }
            else
            {
                Vm.Register[Registers.ERR] = 1;
            }
        }

        private int value;
    }
}