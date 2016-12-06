using System;
using Sigil;

namespace kErMIT
{
    public static class Reflection
    {
        /// <summary>
        /// Creates an instance of a type using a contructor
        /// without parameters
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CreateInstance(this Type type)
        {
            return CreateInstance(type, null);
        }

        public static object CreateInstance(this Type type, params object[] parameters)
        {
            var emiter = Emit<Func<object>>.NewDynamicMethod("__kErMIT_Create_Instance");

            using (var local = emiter.DeclareLocal<object>("__instance"))
            {
                emiter.NewObject(type);
                emiter.StoreLocal(local);
                emiter.LoadLocal(local);
                emiter.Return();

                var del = emiter.CreateDelegate();
                return del();
            }
        }
    }
}
