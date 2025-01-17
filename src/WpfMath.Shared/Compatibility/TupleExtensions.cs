using System;

namespace WpfMath.Compatibility;

#if NET452 // not needed for .NET Core 3.0+ because there are System.TupleExtensions
// TODO[#338]: Remove after migration to .NET Framework 4.7

internal static class TupleExtensions
{
    public static void Deconstruct<T1, T2>(this Tuple<T1, T2> tuple, out T1 x1, out T2 x2)
    {
        x1 = tuple.Item1;
        x2 = tuple.Item2;
    }
}
#endif
