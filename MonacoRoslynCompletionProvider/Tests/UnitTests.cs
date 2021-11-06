using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonacoRoslynCompletionProvider;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public async Task HoverTest()
        {
            var code = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, world!"");
            Console.ReadLine();
        }
    }
}";
            var ws = CompletionWorkspace.Create();
            var document = await ws.CreateDocument(code);
            var info = await document.GetHoverInformation(258);
            var info2 = await document.GetHoverInformation(267);
        }
    }
}
