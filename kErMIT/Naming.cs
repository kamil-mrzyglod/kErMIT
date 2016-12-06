namespace kErMIT
{
    internal static class Naming
    {
        internal static string ToMethodName(this string typeName, string methodName)
        {
            return $"__kErMIT_{typeName}_{methodName}";
        }
    }
}