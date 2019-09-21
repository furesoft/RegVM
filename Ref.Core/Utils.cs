using System;

namespace Ref.Core
{
    public class Utils
    {
        public static void PrintRegisters(Register[] register)
        {
            for (int i = 0; i < register.Length; i++)
            {
                var reg = Enum.GetName(typeof(Registers), i);
                var val = register[i];

                Console.WriteLine("{0,10}{1,10:x4}", reg, val.GetValue());
            }
        }
    }
}