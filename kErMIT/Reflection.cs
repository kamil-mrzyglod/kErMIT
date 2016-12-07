using System;
using System.Reflection;
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
        public static Func<object[], object> GenerateConstructor(this Type type)
        {
            return GenerateConstructor(type, new Type[0]);
        }

        /// <summary>
        /// Creates an instance of a type using matching
        /// constructor
        /// </summary>
        /// <param name="type">Type to be constructed</param>
        /// <param name="parameters">Constructor's parameters types</param>
        /// <returns>An instance of a given type</returns>
        public static Func<object[], object> GenerateConstructor(this Type type, Type[] parameters)
        {
            var methodName = type.Name.ToMethodName("GenerateConstructor");           
            var emiter = Emit<Func<object[], object>>.NewDynamicMethod(methodName);

            using (var local = emiter.DeclareLocal<object>("__instance"))
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    using (var param = emiter.DeclareLocal(parameters[i], $"__parameter_{i}"))
                    {
                        emiter.LoadArgument(0);
                        emiter.LoadConstant(i);
                        emiter.LoadElement(typeof(object));

                        AlignParameterType(parameters[i], emiter);

                        emiter.StoreLocal(param);
                        emiter.LoadLocal(param);
                    }                    
                }

                emiter.NewObject(type, parameters);
                emiter.StoreLocal(local);
                emiter.LoadLocal(local);
                emiter.Return();

                var del = emiter.CreateDelegate();
                return del;
            }
        }

        private static void AlignParameterType<T>(Type parameterType, Emit<T> emiter)
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

        /// <summary>
        /// Generates a static method call. The method should
        /// not have parameters and should be marked as 'void'.
        /// </summary>
        /// <param name="type">Type containing this method</param>
        /// <param name="name">The name of the method</param>
        /// <returns>A delegate which calls a method</returns>
        public static Action<object[]> GenerateMethodCall(this Type type, string name)
        {
            return GenerateMethodCall(type, name, new Type[0]);
        }

        /// <summary>
        /// Generates a static method call. The method 
        /// should be marked as 'void'.
        /// </summary>
        /// <param name="type">Type containing this method</param>
        /// <param name="name">The name of the method</param>
        /// <param name="parameters">Types of the parameters</param>
        /// <returns>A delegate which calls a method</returns>
        public static Action<object[]> GenerateMethodCall(this Type type, string name, Type[] parameters)
        {
            var methodName = type.Name.ToMethodName("GenerateMethodCall");
            var emiter = Emit<Action<object[]>>.NewDynamicMethod(methodName);

            if (parameters.Length == 0)
            {
                emiter.Call(type.GetMethod(name, parameters));
            }
            else
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    using (var param = emiter.DeclareLocal(parameters[i], $"__parameter_{i}"))
                    {
                        emiter.LoadArgument(0);
                        emiter.LoadConstant(i);
                        emiter.LoadElement(typeof(object));

                        AlignParameterType(parameters[i], emiter);

                        emiter.StoreLocal(param);
                        emiter.LoadLocal(param);
                    }
                }
              
                emiter.Call(type.GetMethod(name, parameters));
            }
            
            emiter.Return();

            var del = emiter.CreateDelegate();
            return del;
        }
    }
}
