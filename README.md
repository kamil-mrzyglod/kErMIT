# kErMIT

Emitting made simple

## What is that?

kErMIT is a simple tool, which allows you to take advantage of emitted code without actually writing it on your own. It shares a simple API allowing you to create a delegate, which you can use in your code to avoid old-fashioned reflection.

## Usage

You can use methods of kErMIT as extension methods for `Type`.

Currently kErMIT covers following scenario:
* `new T()` -> `typeof(T).GenerateConstructor()(null);`
* `new T("param1", "param2")` -> `typeof(T).GenerateConstructor()(new []{"param1", "param2"});`
* `T.CallMeStatic()` -> `typeof(T).GenerateMethodCall("CallMeStatic")()`
* `T.CallMeStatic("param1")` -> `typeof(T).GenerateMethodCall("CallMeStatic", new[]{typeof(string)})("param1")`

## Performance

You can run simple benchmarks from `kErMIT.Bench` project. Current results(Intel(R) Code(TM) i7-6820HQ @ 2.70GHz, 16 GB SODIMM 2133MHz, Windows 10 Enterprise, Release build, 1M iterations per each test case, warm up of 10K iterations):

```
# Warming up...
# Creating instances(default) using reflection.
# Elapsed time: 77ms
# Creating instances(default) using kErMIT.
# Elapsed time: 10ms
# Creating instances(1 param) using reflection.
# Elapsed time: 917ms
# Creating instances(1 param) using kErMIT.
# Elapsed time: 18ms
# Calling a static method(no params, void) using reflection.
# Elapsed time: 157ms
# Creating a static method(no params, void) using kErMIT.
# Elapsed time: 6ms
# Calling a static method(1 param, void) using reflection.
# Elapsed time: 231ms
# Creating a static method(1 param, void) using kErMIT.
# Elapsed time: 14ms
```
