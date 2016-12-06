using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Sigil;

namespace kErMIT
{
    public static class Reflection
    {
        /// <summary>
        /// Creates an instance of a type using a contructor
        /// without parameters
        /// </summary>
        /// <param name="type">Type to be constructed</param>
        /// <returns>An instance of a given type</returns>
        public static Func<object[], object> CreateInstance(this Type type)
        {
            return CreateInstance(type, new Type[0]);
        }

        /// <summary>
        /// Creates an instance of a type using matching
        /// constructor
        /// </summary>
        /// <param name="type">Type to be constructed</param>
        /// <param name="parameters">Constructor's parameters types</param>
        /// <param name="values">Values to be passed as parameters</param>
        /// <returns>An instance of a given type</returns>
        public static Func<object[], object> CreateInstance(this Type type, Type[] parameters)
        {
            var methodName = type.Name.ToMethodName("CreateInstance");           
            var emiter = Emit<Func<object[], object>>.NewDynamicMethod(methodName);

            using (var local = emiter.DeclareLocal<object>("__instance"))
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    var param = emiter.DeclareLocal(parameters[i], $"__parameter_{i}");
                    emiter.LoadArgument((ushort)i);
                    emiter.LoadConstant(i);
                    emiter.LoadElement(typeof(object));
                    emiter.CastClass(parameters[i]);
                    emiter.StoreLocal(param);
                    emiter.LoadLocal(param);
                }

                emiter.NewObject(type, parameters);
                emiter.StoreLocal(local);
                emiter.LoadLocal(local);
                emiter.Return();

                var del = emiter.CreateDelegate();

                return del;
            }
        }
    }
}
