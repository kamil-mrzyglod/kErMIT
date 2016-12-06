using NUnit.Framework;

namespace kErMIT.Test
{
    internal class ReflectionTest
    {
        [Test]
        public void Foo()
        {
            var foo = typeof(Foo).CreateInstance();

            Assert.That(foo, Is.InstanceOf<Foo>());
        }
    }

    internal class Foo
    {       
    }
}
