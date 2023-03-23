using System.Collections.Generic;

namespace Extensions
{
    public static class ListExtensions
    {
        public static T GetLast<T>(this List<T> list)
        {
            return list[list.Count-1];
        }
        public static int GetLastIndex<T>(this List<T> list)
        {
            return list.Count-1;
        }
    }
}