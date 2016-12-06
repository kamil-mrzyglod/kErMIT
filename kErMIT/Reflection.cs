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
                    emiter.LoadArgument(0);
                    emiter.LoadConstant(i);
                    emiter.LoadElement(typeof(object));

                    AlignParameterType(parameters[i], emiter);
                    
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

        private static void AlignParameterType(Type parameterType, Emit<Func<object[], object>> emiter)
        {
            if (parameterType.IsPrimitive)
            {
                emiter.UnboxAny(parameterType);
            }
            else
            {
                emiter.CastClass(parameterType);
            }
        }
    }
}
