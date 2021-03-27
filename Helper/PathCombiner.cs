using System;
using System.IO;

namespace TF47_Backend.Helper
{
    public static class PathCombiner
    {
        public static string Combine(params string[] arguments)
        {
            if (arguments.Length == 0) return String.Empty;
            if (arguments.Length == 1) return arguments[0];

            var output = arguments[0];

            for (int i = 1; i < arguments.Length; i++)
            {
                output = Path.Combine(output, arguments[i]);
            }

            if (!OperatingSystem.IsWindows())
                return output.Replace("\\", "/");

            return output;
        }
    }
}