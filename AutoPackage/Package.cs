using System;

namespace AutoPackage
{
    public static class Package
    {
        public static string Pack(string id, string status, string message)
        {
            string result = $"{id}™{status}™{message}";
            return result;
        }

        public static (string, string, string) Unpack(string str)
        {
            string[] temp = str.Split("™");
            var result = (temp[0], temp[1], temp[2]);
            return result;
        }
    }
}
