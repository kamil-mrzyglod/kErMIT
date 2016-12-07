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

            method();

            Assert.That(Foo.StaticBar, Is.EqualTo("CallMeStatic"));
        }
    }

    internal class Foo
    {
        public static string StaticBar = string.Empty;

        public readonly string Bar;
        public readonly int Foobar;

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
    }
}
