using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using MonacoRoslynCompletionProvider.Api;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonacoRoslynCompletionProvider
{
    internal class SignatureHelpProvider
    {
        public async Task<SignatureHelpResult> Provide(Document document, int position)
        {
            var syntaxRoot = await document.GetSyntaxRootAsync();
            var semanticModel = await document.GetSemanticModelAsync();
            var methods = syntaxRoot.DescendantNodes().OfType<InvocationExpressionSyntax>();

            var allMethodRefs = new List<IEnumerable<ReferencedSymbol>>();

            if (methods != null)
            {
                if (methods.Count() > 0)
                {
                    foreach (var m in methods)
                    {
                        var info = semanticModel.GetSymbolInfo(m);
                        if (info.Symbol != null)
                        {
                            allMethodRefs.Add(await SymbolFinder.FindReferencesAsync(info.Symbol, document.Project.Solution));
                        }
                        else
                        {
                            foreach (var symbol in info.CandidateSymbols)
                            {
                                allMethodRefs.Add(await SymbolFinder.FindReferencesAsync(symbol, document.Project.Solution));
                            }
                        }
                    }
                }
            }

            var signatures = new List<Signatures>();
            foreach (var signatureWrap in allMethodRefs)
            {
                foreach (var signature in signatureWrap)
                {
                    var parameters = new List<Parameter>();
                    if(signature.Definition is IMethodSymbol symbol)
                    {
                        foreach(var par in symbol.Parameters)
                        {
                            parameters.Add(new Parameter()
                            {
                                Label = par.Type + " " + par.Name,
                                Documentation = par.GetDocumentationCommentXml()
                            });
                        }
                    }
                    signatures.Add(new Signatures()
                    {
                        Label = signature.Definition.Name,
                        Documentation = "",
                        Parameters = parameters.ToArray()
                    });
                }
            }
            return new SignatureHelpResult()
            {
                Signatures = signatures.ToArray()
            };
        }
    }
}
