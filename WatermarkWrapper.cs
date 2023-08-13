using System.Runtime.CompilerServices;

namespace DayuuMod
{
    internal class WatermarkWrapper
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ActivateWatermark() => AddWatermark.API.ActivateWatermark();
    }
}
