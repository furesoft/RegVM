using System;

namespace Ref.Core.VM
{
    public class ErrorAttribute : Attribute
    {
        public int ErrorCode { get; set; }

        public string Explanation { get; set; }

        public ErrorAttribute(int errorCode, string explanation)
        {
            ErrorCode = errorCode;
            Explanation = explanation;
        }
    }
}