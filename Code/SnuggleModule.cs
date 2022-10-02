namespace Celeste.Mod.SnugHelper {
    public class SnuggleModule : EverestModule {
        public static SnuggleModule Instance;

        public SnuggleModule() {
            Instance = this;
        }

        public override void Load() {
            SnuggleUtils.Log(LogLevel.Info, "Loading...");
            Logger.SetLogLevel("SnugHelper", LogLevel.Info);
            
            SnuggleHooks.Load();
            SnuggleInterop.Load();
        }

        public override void Unload() {
            SnuggleUtils.Log(LogLevel.Info, "Unloading...");
            
            SnuggleHooks.Unload();
        }
    }
}