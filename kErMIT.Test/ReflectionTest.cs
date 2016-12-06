using NUnit.Framework;

namespace kErMIT.Test
{
    internal class ReflectionTest
    {
        [Test]
        public void ReflectionTest_WhenPassingNoParameterToCreateInstance_ShouldCreateParameterlessCtor()
        {
            var foo = typeof(Foo).CreateInstance();

            Assert.That(foo, Is.InstanceOf<Foo>());
            Assert.That(((Foo)foo)._bar, Is.EqualTo("Parameterless"));
        }

        [Test]
        public void ReflectionTest_WhenPassingParametersToCreateInstance_ShouldCreateParameterlessCtor()
        {
            var foo = typeof(Foo).CreateInstance(typeof(string));

            Assert.That(foo, Is.InstanceOf<Foo>());
            Assert.That(((Foo)foo)._bar, Is.EqualTo(null));
        }
    }

    internal class Foo
    {
        public readonly string _bar;

        public Foo()
        {
            _bar = "Parameterless";
        }

        public Foo(string bar)
        {
            _bar = bar;
        }   
    }
}
