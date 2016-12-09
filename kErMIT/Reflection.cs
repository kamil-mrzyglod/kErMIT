using System;
using System.Collections.Generic;
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
                LoadParametersAsLocals(parameters, emiter);

                emiter.NewObject(type, parameters);
                emiter.StoreLocal(local);
                emiter.LoadLocal(local);
                emiter.Return();

                var del = emiter.CreateDelegate();
                return del;
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

            if (parameters.Length != 0)
            {
                LoadParametersAsLocals(parameters, emiter);
            }

            emiter.Call(type.GetMethod(name, parameters));          
            emiter.Return();

            var del = emiter.CreateDelegate();
            return del;
        }

        public static Action<object[], object> GenerateInstanceMethodCall(this Type type, string name)
        {
            return GenerateInstanceMethodCall(type, name, new Type[0]);
        }

        public static Action<object[], object> GenerateInstanceMethodCall(this Type type, string name, Type[] parameters)
        {
            var methodName = type.Name.ToMethodName("GenerateInstanceMethodCall");
            var emiter = Emit<Action<object[], object>>.NewDynamicMethod(methodName);

            emiter.LoadArgument(1);
            emiter.CastClass(type);

            if (parameters.Length == 0)
            {
                emiter.Call(type.GetMethod(name, parameters));
            }
            else
            {
                LoadParametersAsLocals(parameters, emiter);
                emiter.Call(type.GetMethod(name, parameters));
            }
    
            emiter.Return();

            var del = emiter.CreateDelegate();
            return del;
        }

        private static void LoadParametersAsLocals<T>(IReadOnlyList<Type> parameters, Emit<T> emiter)
        {
            for (var i = 0; i < parameters.Count; i++)
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
        }

        private static void AlignParameterType<T>(Type parameterType, Emit<T> emiter)
        {
            if (parameterType.IsPrimitive || parameterType.IsValueType)
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
