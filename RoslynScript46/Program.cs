using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynScript46
{
    public class Params
    {
        public double gX;
    }

    //See: https://github.com/dotnet/roslyn/wiki/Scripting-API-Samples
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            //Console.ReadLine();
            Task.Run(async () =>
            {
                try
                {
                    var state1 = await CSharpScript.RunAsync<int>("int a = 10;");
                    state1 = await state1.ContinueWithAsync<int>("int b=20;");
                    state1 = await state1.ContinueWithAsync<int>(
                        @"int c=a+b;
                      c / 5
                    ");
                    Console.WriteLine(state1.ReturnValue); //6 - please check
                    string program2 = @"
                        using System;
                        Math.Sin(gX) * Math.Cos(gX)
                        ";
                    var script2 = CSharpScript.Create<double>(program2, ScriptOptions.Default.WithImports("System.Math"), typeof(Params));
                    script2.Compile();
                    var roslynApi = script2.GetCompilation();
                    Console.WriteLine($"1: {(await script2.RunAsync(new Params { gX = 1 })).ReturnValue}");
                    Console.WriteLine($"2: {(await script2.RunAsync(new Params { gX = 2 })).ReturnValue}");
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }).Wait();

        }
    }
}
