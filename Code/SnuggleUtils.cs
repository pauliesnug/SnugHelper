using Microsoft.Xna.Framework;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Monocle;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using System;
using System.Reflection;

namespace Celeste.Mod.SnugHelper {
    public static class SnuggleUtils {
        public static ILHook HookCoroutine(string typeName, string methodName, ILContext.Manipulator manipulator) {
            ModuleDefinition celeste = Everest.Relinker.SharedRelinkModuleMap["Celeste.Mod.mm"];

            TypeDefinition type = celeste.GetType(typeName);
            if (type == null) return null;

            foreach (TypeDefinition nest in type.NestedTypes) {
                if (nest.Name.StartsWith("<" + methodName + ">d__")) {
                    MethodDefinition method = nest.FindMethod("System.Boolean MoveNext()");
                    if (method == null) return null;
                    
                    Log(LogLevel.Info, $"Building IL hook for method {method.FullName} in order to mod {typeName}.{methodName}()");
                    Type reflectionType = typeof(Player).Assembly.GetType(typeName);
                    Type reflectionNestedType = reflectionType.GetNestedType(nest.Name, BindingFlags.NonPublic);
                    MethodBase moveNextMethod = reflectionNestedType.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);
                    return new ILHook(moveNextMethod, manipulator);
                }
            }

            return null;
        }
        
        public static FieldReference FindReferenceToThisInCoroutine(ILCursor cursor) {
            // coroutines are cursed and references to "this" are actually references to this.<>4__this
            cursor.GotoNext(instr => instr.OpCode == OpCodes.Ldfld && (instr.Operand as FieldReference).Name == "<>4__this");
            FieldReference refToThis = cursor.Next.Operand as FieldReference;
            cursor.Index = 0;
            return refToThis;
        }
        
        public static void Log(LogLevel logLevel, string str)
            => Logger.Log(logLevel, "SnugHelper", str);

        public static void Log(string str)
            => Log(LogLevel.Debug, str);

        public static bool GetPlayer(out Player player) {
            player = Engine.Scene?.Tracker?.GetEntity<Player>();
            return player != null;
        }

        public static int ToInt(bool b) 
            => b ? 1 : 0;

        public static float Mod(float x, float m)
            => (x % m + m) % m;
        
        public static Vector2 Min(Vector2 a, Vector2 b)
            => new(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));

        public static Vector2 Max(Vector2 a, Vector2 b)
            => new(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
    }
}