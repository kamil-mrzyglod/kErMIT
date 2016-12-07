# kErMIT

Emitting made simple

## What is that?

kErMIT is a simple tool, which allows you to take advantage of emitted code without actually writing it on your own. It shares a simple API allowing you to create a delegate, which you can use in your code to avoid old-fashioned reflection.

## Usage

You can use methods of kErMIT as extension methods for `Type`.

Currently kErMIT covers following scenario:
* `Activator.CreateInstance<T>();` -> `typeof(T).CreateInstance()(null);`
* `Activator.CreateInstance(typeof(DummyClass), "param1", "param2");` -> `typeof(T).CreateInstance()(new []{"param1", "param2"});`

## Performance

You can run simple benchmarks from `kErMIT.Bench` project. Current results(Intel(R) Code(TM) i7-6820HQ @ 2.70GHz, 16 GB SODIMM 2133MHz, Windows 10 Enterprise, Release build, 1M iterations per each test case, warm up of 10K iterations):

```
# Creating instances(default) using reflection.
# Elapsed time: 77ms
# Creating instances(default) using kErMIT.
# Elapsed time: 9ms
# Creating instances(1 param) using reflection.
# Elapsed time: 882ms
# Creating instances(1 param) using kErMIT.
# Elapsed time: 17ms
```
