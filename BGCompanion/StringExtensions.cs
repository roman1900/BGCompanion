using System;
using System.Collections.Generic;
using System.Text;

namespace BGCompanion
{
    public static class StringExtensions
    {
        public static string CentreString(this string stringToCenter, int totalLength)
        {
            return stringToCenter.PadLeft(((totalLength - stringToCenter.Length) / 2)
                                + stringToCenter.Length)
                       .PadRight(totalLength);
        }
    }
}
