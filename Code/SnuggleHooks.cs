using System;

namespace Celeste.Mod.SnugHelper; 

public static class SnuggleHooks {
    public static void Load() {
        On.Celeste.Achievements.Register += Achievements_Register;
    }

    public static void Unload() {
        On.Celeste.Achievements.Register -= Achievements_Register;
    }

    private static void Achievements_Register(On.Celeste.Achievements.orig_Register orig, Achievement achievement) {
        foreach (Achievement ach in (Achievement[]) Enum.GetValues(typeof(Achievement))) {
            orig(ach);
        }
    }
}