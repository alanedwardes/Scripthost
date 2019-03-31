# Scripthost
A very light console app to execute arbitrary C# scripts as text at runtime.

## Usage
This tool needs to be published before it can be used, since it relies in several System* packages which only get copied to the output directory on a publish.

**Script.cs**
```csharp
using System;

namespace MyNamespace
{
  public class MyClass
  {
    public static void MyStaticMethod()
    {
      Console.WriteLine("Hello world!");
    }
  }
}
```

**Command Line**
```
./scripthost --script "Path/To/Script.cs" --type "MyNamespace.MyClass" --method "MyStaticMethod"

# Prints "Hello World!"
```
