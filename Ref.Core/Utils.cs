using System;
using System.Collections.Generic;
using System.Text;

namespace Ref.Core
{
    public class Utils
    {
        public static void PrintRegisters(RegisterCollection register)
        {
            for (int i = 0; i < register.Length; i++)
            {
                var reg = Enum.GetName(typeof(Registers), i);
                var val = register[i];

                Console.WriteLine("{0,10}{1,10:x4}", reg, val);
            }
        }

        public static string[] Split(string source)
        {
            StringBuilder currentToken = new StringBuilder();
            bool inDelimitedString = false;
            List<string> scannedTokens = new List<string>();
            foreach (char c in source)
            {
                switch (c)
                {
                    case '"':
                        if (inDelimitedString)
                        {
                            if (currentToken.Length > 0)
                            {
                                scannedTokens.Add(currentToken.ToString());
                                currentToken.Clear();
                            }
                        }
                        inDelimitedString = !inDelimitedString;
                        break;

                    case ' ':
                        if (!inDelimitedString)
                        {
                            if (currentToken.Length > 0)
                            {
                                scannedTokens.Add(currentToken.ToString());
                                currentToken.Clear();
                            }
                        }
                        else
                        {
                            currentToken.Append(c);
                        }
                        break;

                    case ',':
                        if (!inDelimitedString)
                        {
                            if (currentToken.Length > 0)
                            {
                                scannedTokens.Add(currentToken.ToString());
                                currentToken.Clear();
                            }
                        }
                        else
                        {
                            currentToken.Append(c);
                        }
                        break;

                    default:
                        currentToken.Append(c);
                        break;
                }
            }
            if (currentToken.Length > 0)
            {
                scannedTokens.Add(currentToken.ToString());
                currentToken.Clear();
            }

            return scannedTokens.ToArray();
        }
    }
}