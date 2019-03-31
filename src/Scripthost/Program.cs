using CommandLine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Scripthost.Options;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Scripthost
{
    class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<RunOptions>(args)
                .MapResult((RunOptions opts) => RunCode(opts), errs => 1);
        }

        private static int RunCode(RunOptions options)
        {
            if (!options.Script.Exists)
            {
                return Fatal($"Could not find script {options.Script.FullName}");
            }

            var assembly = Compile(options.Script);
            if (assembly == null)
            {
                return Fatal($"Could not load assembly.");
            }
            
            var type = assembly.GetType(options.Type);
            if (type == null)
            {
                return Fatal($"Could not load type {options.Type}");
            }

            var method = type.GetMethod(options.Method);
            if (method == null)
            {
                return Fatal($"Could not find method {options.Method}");
            }

            method.Invoke(null, null);
            return 0;
        }

        private static Assembly Compile(FileInfo script)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(script.FullName));

            // Just scoop up all System.* references
            var references = new FileInfo(typeof(Program).Assembly.Location)
                .Directory
                .EnumerateFiles("System.*.dll")
                .Select(x => MetadataReference.CreateFromFile(x.FullName));

            var assmblyName = Path.GetRandomFileName();

            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            var compilation = CSharpCompilation.Create(assmblyName, new[] { syntaxTree }, references, options);

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    var failures = result.Diagnostics.Where(x => x.IsWarningAsError || x.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Error($"{diagnostic.Id}: {diagnostic.GetMessage()}");
                    }

                    return null;
                }

                ms.Seek(0, SeekOrigin.Begin);
                return Assembly.Load(ms.ToArray());
            }
        }

        private static void Error(string error)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Error.WriteLine();
            Console.Error.WriteLine(error);
            Console.Error.WriteLine();
            Console.ResetColor();
        }

        private static int Fatal(string error)
        {
            Error(error);
            return 1;
        }
    }
}
