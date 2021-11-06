using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MonacoRoslynCompletionProvider.Api;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MonacoRoslynCompletionProvider
{
    internal class HoverInformationProvider
    {
        public async Task<HoverInfoResult> Provide(Document document, int position, SemanticModel semanticModel)
        {
            //SemanticModel semanticModel = await document.GetSemanticModelAsync();
            //if (semanticModel == null)
            //    return null;

            TypeInfo typeInfo;
            var syntaxRoot = await document.GetSyntaxRootAsync();
            
            var expressionNode = syntaxRoot.FindToken(position).Parent;
            if (expressionNode is VariableDeclaratorSyntax)
            {
                SyntaxNode childNode = expressionNode.ChildNodes()?.FirstOrDefault()?.ChildNodes()?.FirstOrDefault();
                typeInfo = semanticModel.GetTypeInfo(childNode);
            }
            else if (expressionNode is ClassDeclarationSyntax)
            {
                throw new NotImplementedException();
            }
            else if (expressionNode is IdentifierNameSyntax)
            {
               
                var symbolInfo = semanticModel.GetSymbolInfo(expressionNode);
                if (symbolInfo.Symbol != null)
                {
                    var location = expressionNode.GetLocation();
                    return new HoverInfoResult() { Information = symbolInfo.Symbol.ToDisplayString(), OffsetFrom = location.SourceSpan.Start, OffsetTo = location.SourceSpan.End };
                }
                    return null;
                return null;
            }
            else
            {
                typeInfo = semanticModel.GetTypeInfo(expressionNode);
                if (typeInfo.Type == null)
                {
                    expressionNode = expressionNode.Parent;
                    typeInfo = semanticModel.GetTypeInfo(expressionNode);
                }
            }

            return null;
        }
    }
}
