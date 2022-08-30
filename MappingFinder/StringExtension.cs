using System;

namespace MappingFinder
{
    static class StringExtension
    {
        public static string[] Split(this string str, string separate)
        {
            return str.Split(new string[] { separate }, StringSplitOptions.None);
        }
    }
}
