using System.Collections.Generic;
using System.Text;

namespace Ref.Core.VM.Core
{
    public static class Extensions
    {
        public static void PopRegisters(this Stack s, RegisterCollection r)
        {
            var tmpList = new List<int>();

            for (int i = r.Register.Length; i > 0; i--)
            {
                var val = s.Pop();

                tmpList.Add(val);
            }

            for (int i = 0; i < r.Length; i++)
            {
                r.Register[i].value = tmpList[i];
            }
        }

        public static void PushRegisters(this Stack s, RegisterCollection r)
        {
            for (int i = 0; i < r.Register.Length; i++)
            {
                Register register = r.Register[i];

                s.Push(register.value);
            }
        }

        public static string ToHex(this byte[] raw)
        {
            var sb = new StringBuilder();

            foreach (var item in raw)
            {
                if (item == 0)
                {
                    sb.Append("00 ");
                    continue;
                }

                sb.Append(item.ToString("x") + " ");
            }

            return sb.ToString();
        }
    }
}