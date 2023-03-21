using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static partial class MyExtensions
{
    public static string MySubString(this string s, int start, int end)
    {
        return s.Substring(start, end - start);
    }
}
