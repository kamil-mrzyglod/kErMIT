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
            return CreateInstance(type, new Type[0]);
        }

        public static object CreateInstance(this Type type, params Type[] parameters)
        {
            var emiter = Emit<Func<object>>.NewDynamicMethod(type.Name.ToMethodName("CreateInstance"));

            using (var local = emiter.DeclareLocal<object>("__instance"))
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    var param = emiter.DeclareLocal(parameters[i], $"__parameter_{i}");
                    emiter.LoadLocal(param);
                }

                emiter.NewObject(type, parameters);
                emiter.StoreLocal(local);
                emiter.LoadLocal(local);
                emiter.Return();

                var del = emiter.CreateDelegate();
                return del();
            }
        }
    }
}
