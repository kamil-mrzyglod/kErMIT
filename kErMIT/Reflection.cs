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
        /// <param name="type">Type to be constructed</param>
        /// <returns>An instance of a given type</returns>
        public static object CreateInstance(this Type type)
        {
            return CreateInstance(type, new Type[0], new object[0]);
        }

        /// <summary>
        /// Creates an instance of a type using matching
        /// constructor
        /// </summary>
        /// <param name="type">Type to be constructed</param>
        /// <param name="parameters">Constructor's parameters types</param>
        /// <param name="values">Values to be passed as parameters</param>
        /// <returns>An instance of a given type</returns>
        public static object CreateInstance(this Type type, Type[] parameters, object[] values)
        {
            var emiter = Emit<Func<object[], object>>.NewDynamicMethod(type.Name.ToMethodName("CreateInstance"));

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
                return del(values);
            }
        }
    }
}
