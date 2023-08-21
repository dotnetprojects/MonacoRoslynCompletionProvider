using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonacoRoslynCompletionProvider;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using MonacoRoslynCompletionProvider.Api;

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
            var info = await document.GetHoverInformation(258, CancellationToken.None);
            var info2 = await document.GetHoverInformation(267, CancellationToken.None);
        }

        [TestMethod]
        public async Task DocumentShouldNotContainErrorsWhenUsingTopLevelStatements()
        {
            const string code = @"using System;
Console.WriteLine(""Hello, world!"");
";

            var ws = CompletionWorkspace.Create();
            var document = await ws.CreateDocument(code, OutputKind.ConsoleApplication);
            var codeCheckResults = await document.GetCodeCheckResults(CancellationToken.None);

            Assert.IsTrue(codeCheckResults.All(r => r.Severity != CodeCheckSeverity.Error));
        }
    }
}
