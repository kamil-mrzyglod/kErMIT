using System;
using NUnit.Framework;

namespace kErMIT.Test
{
    internal class ReflectionTest
    {
        [Test]
        public void ReflectionTest_WhenPassingNoParameterToCreateInstance_ShouldCreateParameterlessCtor()
        {
            var foo = typeof(Foo).GenerateConstructor()(null);

            Assert.That(foo, Is.InstanceOf<Foo>());
            Assert.That(((Foo)foo).Bar, Is.EqualTo("Parameterless"));
        }

        [Test]
        public void ReflectionTest_WhenPassingParametersToCreateInstance_ShouldCreateCorrectCtor()
        {
            var foo = typeof(Foo).GenerateConstructor(new [] { typeof(string) })(new[] {"Foo"});

            Assert.That(foo, Is.InstanceOf<Foo>());
            Assert.That(((Foo)foo).Bar, Is.EqualTo("Foo"));

            var bar = typeof(Foo).GenerateConstructor(new[] { typeof(string), typeof(int) })(new object[] { "Bar", 1 });

            Assert.That(bar, Is.InstanceOf<Foo>());
            Assert.That(((Foo)bar).Bar, Is.EqualTo("Bar"));
            Assert.That(((Foo)bar).Foobar, Is.EqualTo(1));
        }

        [Test]
        public void ReflectionTest_WhenCallingAStaticMethodWithoutParametersAndVoidReturnType_ItIsCalledCorrectly()
        {
            var method = typeof(Foo).GenerateMethodCall("CallMeStatic");

            method(null);

            Assert.That(Foo.StaticBar, Is.EqualTo("CallMeStatic"));
        }

        [Test]
        public void ReflectionTest_WhenCallingAStaticMethodWithParametersAndVoidReturnType_ItIsCalledCorrectly()
        {
            var method = typeof(Foo).GenerateMethodCall("CallMeStatic", new []{typeof(string)});
            method(new[] {"Foo"});

            Assert.That(Foo.StaticBar, Is.EqualTo("Foo"));

            var method2 = typeof(Foo).GenerateMethodCall("CallMeStatic", new[] { typeof(string), typeof(DateTime) });
            method2(new object[] { "Foo", new DateTime(1990, 1, 1) });

            Assert.That(Foo.StaticBar, Is.EqualTo("Foo"));
            Assert.That(Foo.StaticBarDt, Is.EqualTo(new DateTime(1990, 1, 1)));
        }

        [Test]
        public void ReflectionTest_WhenCallingAnInstanceMethodWithoutParametersAndVoidReturnType_ItIsCalledCorrectly()
        {
            var instance = new Foo();
            var method = typeof(Foo).GenerateInstanceMethodCall("CallMe");

            method(null, instance);

            Assert.That(instance.InstanceBar, Is.EqualTo("CallMe"));
        }

        [Test]
        public void ReflectionTest_WhenCallingAnInstanceMethodWithParametersAndVoidReturnType_ItIsCalledCorrectly()
        {
            var instance = new Foo();
            var method = typeof(Foo).GenerateInstanceMethodCall("CallMe", new []{typeof(string)});
            method(new []{"foo"}, instance);

            Assert.That(instance.InstanceBar, Is.EqualTo("foo"));

            var method2 = typeof(Foo).GenerateInstanceMethodCall("CallMe", new[] { typeof(string), typeof(DateTime) });
            method2(new object[] { "foo", new DateTime(1990, 1, 1) }, instance);

            Assert.That(instance.InstanceBar, Is.EqualTo("foo"));
            Assert.That(instance.InstanceBarDt, Is.EqualTo(new DateTime(1990, 1, 1)));
        }
    }

    internal class Foo
    {
        public static string StaticBar = string.Empty;
        public static DateTime StaticBarDt = DateTime.MinValue;

        public readonly string Bar;
        public readonly int Foobar;

        public string InstanceBar;
        public DateTime InstanceBarDt;

        public Foo()
        {
            Bar = "Parameterless";
        }

        public Foo(string bar)
        {
            Bar = bar;
        }

        public Foo(string bar, int foobar)
            : this(bar)
        {
            Foobar = foobar;
        }

        public static void CallMeStatic()
        {
            StaticBar = "CallMeStatic";
        }

        public void CallMe()
        {
            InstanceBar = "CallMe";
        }

        public static void CallMeStatic(string text)
        {
            StaticBar = text;
        }

        public static void CallMeStatic(string text, DateTime time)
        {
            StaticBar = text;
            StaticBarDt = time;
        }

        public void CallMe(string text)
        {
            InstanceBar = text;
        }

        public void CallMe(string text, DateTime time)
        {
            InstanceBar = text;
            InstanceBarDt = time;
        }
    }
}
