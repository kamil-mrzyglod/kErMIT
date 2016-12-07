using System;
using System.Diagnostics;

namespace kErMIT.Bench
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("# Warming up...");

            var methodInfo = typeof(DummyClass).GetMethod("CallMeStatic");
            var instance = new DummyClass();
            var sw = new Stopwatch();

            for (var i = 0; i <= 100000; i++)
            {
                var int1 = Activator.CreateInstance<DummyClass>();
                var int2 = typeof(DummyClass).GenerateConstructor()(null);
                var res1 = typeof(DummyClass).GenerateMethodCall("CallMeStatic");
                var res2 = methodInfo.Invoke(instance, new object[0]);
            }

            Console.WriteLine("# Creating instances(default) using reflection.");

            sw.Start();
            for (var i = 0; i <= 1000000; i++)
            {
                var int1 = Activator.CreateInstance(typeof(DummyClass));
            }
            sw.Stop();

            Console.WriteLine($"# Elapsed time: {sw.ElapsedMilliseconds}ms");

            Console.WriteLine("# Creating instances(default) using kErMIT.");
            var del = typeof(DummyClass).GenerateConstructor();

            sw.Reset();
            sw.Start();
            for (var i = 0; i <= 1000000; i++)
            {
                var int2 = del(null);
            }
            sw.Stop();

            Console.WriteLine($"# Elapsed time: {sw.ElapsedMilliseconds}ms");

            Console.WriteLine("# Creating instances(1 param) using reflection.");

            sw.Reset();
            sw.Start();
            for (var i = 0; i <= 1000000; i++)
            {
                var int1 = Activator.CreateInstance(typeof(DummyClass), "foo");
            }
            sw.Stop();

            Console.WriteLine($"# Elapsed time: {sw.ElapsedMilliseconds}ms");

            Console.WriteLine("# Creating instances(1 param) using kErMIT.");

            sw.Reset();
            sw.Start();
            for (var i = 0; i <= 1000000; i++)
            {
                var int2 = del(new []{"foo"});
            }
            sw.Stop();

            Console.WriteLine($"# Elapsed time: {sw.ElapsedMilliseconds}ms");

            Console.WriteLine("# Calling a static method(no params, void) using reflection.");

            sw.Reset();
            sw.Start();
    
            for (var i = 0; i <= 1000000; i++)
            {
                methodInfo.Invoke(instance, new object[0]);
            }
            sw.Stop();

            Console.WriteLine($"# Elapsed time: {sw.ElapsedMilliseconds}ms");

            Console.WriteLine("# Creating a static method(no params, void) using kErMIT.");
            var callDel = typeof(DummyClass).GenerateMethodCall("CallMeStatic");

            sw.Reset();
            sw.Start();
            for (var i = 0; i <= 1000000; i++)
            {
                callDel();
            }
            sw.Stop();

            Console.WriteLine($"# Elapsed time: {sw.ElapsedMilliseconds}ms");
            Console.ReadLine();
        }
    }

    internal class DummyClass
    {
        public static string StaticBar = string.Empty;

        private readonly string _foo;

        public DummyClass()
        {
            _foo = "Dummy";
        }

        public DummyClass(string text)
        {
            _foo = text;
        }

        public static void CallMeStatic()
        {
            StaticBar = "CallMeStatic";
        }
    }
}
