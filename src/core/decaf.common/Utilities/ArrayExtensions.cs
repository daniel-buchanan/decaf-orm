namespace decaf.common.Utilities
{
    public static class ArrayExtensions
    {
        public static bool TryGetValue<T>(this T[] self, int index, out T value)
        {
            value = default(T);
            if (index < 0 ||
               index > self.Length ||
               self.Length == 0)
                return false;

            value = self[index];
            return true;
        }

        public static int LastIndex<T>(this T[] self)
        {
            var length = self.Length;
            if (length == 0) return 0;
            return length - 1;
        }
    }
}