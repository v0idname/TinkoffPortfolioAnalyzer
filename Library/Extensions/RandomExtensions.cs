using System;

namespace Library.Extensions
{
    public static class RandomExtensions
    {
        public static T NextItem<T>(this Random rnd, params T[] items)
        {
            return items[rnd.Next(items.Length)];
        }

        public static T NextEnumItem<T>(this Random rnd)
        {
            var enumValues = Enum.GetValues(typeof(T));
            return (T)enumValues.GetValue(rnd.Next(enumValues.Length));
        }
    }
}
