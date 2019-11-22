using System.Text;

namespace Ref.Core.VM.Core
{
    public static class Extensions
    {
        public static string ToHex(this byte[] raw)
        {
            var sb = new StringBuilder();

            foreach (var item in raw)
            {
                sb.Append(item.ToString("x") + " ");
            }

            return sb.ToString();
        }
    }
}