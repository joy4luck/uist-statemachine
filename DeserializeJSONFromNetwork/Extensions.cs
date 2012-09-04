using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeserializeJSONFromNetwork
{
    public static class Extensions
    {
        public static string PrintArray<T>(this T[] arr)
        {
            StringBuilder output = new StringBuilder();
            output.Append("[");
            output.Append(String.Join(",", arr));
            output.Append("]");
            return output.ToString();
        }
    }
}
