using MonoMod.ModInterop;

namespace Celeste.Mod.SnugHelper {
    [ModExportName("SnugHelper")]
    public static class SnuggleInterop {
        internal static void Load() {
            typeof(SnuggleInterop).ModInterop();
        }
    }
}