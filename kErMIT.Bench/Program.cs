using System;
using System.Diagnostics;

namespace kErMIT.Bench
{
    internal class Program
    {
        private static void Main()
        {
            var sw = new Stopwatch();
            for (var i = 0; i <= 1000; i++)
            {
                var int1 = Activator.CreateInstance<DummyClass>();
                var int2 = typeof(DummyClass).CreateInstance()(null);
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
            var del = typeof(DummyClass).CreateInstance();

            sw.Reset();
            sw.Start();
            for (var i = 0; i <= 1000000; i++)
            {
                var int2 = del(null);
            }
            sw.Stop();

            Console.WriteLine($"# Elapsed time: {sw.ElapsedMilliseconds}ms");

            Console.WriteLine("# Creating instances(1 param) using reflection.");

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
            Console.ReadLine();
        }
    }

    internal class DummyClass
    {
        private readonly string _foo;

        public DummyClass()
        {
            _foo = "Dummy";
        }

        public DummyClass(string text)
        {
            _foo = text;
        }
    }
}
