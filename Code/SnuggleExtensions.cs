using Celeste.Mod.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Celeste.Mod.SnugHelper {
    public static class SnuggleExtensions {
        public static List<Type> GetSubClasses(this Type type) {
            List<Type> list = new List<Type>();
            foreach (Type type2 in FakeAssembly.GetFakeEntryAssembly().GetTypes()) {
                if (type != type2 && type.IsAssignableFrom(type2)) {
                    list.Add(type2);
                }
            }
            return list;
        }

        public static List<MethodInfo> GetOverrides(this MethodInfo method, bool returnBase) {
            List<MethodInfo> list = new List<MethodInfo>();
            if (returnBase)
                list.Add(method);

            foreach (Type subType in method.DeclaringType.GetSubClasses()) {
                MethodInfo overrideMethod = subType.GetMethod(method.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
                if (overrideMethod != null && overrideMethod.Attributes.HasFlag(MethodAttributes.Virtual) && overrideMethod.GetBaseDefinition() == method)
                    list.Add(overrideMethod);

            }
            return list;
        }

        public static MethodInfo GetMethod(this Type type, string name, BindingFlags bindingAttr, bool throwOnNull = false) {
            var method = type.GetMethod(name, bindingAttr);
            if (throwOnNull && method is null)
                throw new NullReferenceException($"Could not find method {name} in type {type.FullName}");
            return method;
        }

        public static MethodInfo GetMethod(this Type type, string name, Type[] types, bool throwOnNull = false) {
            var method = type.GetMethod(name, types);
            if (throwOnNull && method is null)
                throw new NullReferenceException($"Could not find method {name}({string.Join<Type>(",", types)}) in type {type.FullName}.");
            return method;
        }

        public static string GetFullName(this MethodInfo method)
            => $"{method.DeclaringType}.{method.Name}";
    }
}