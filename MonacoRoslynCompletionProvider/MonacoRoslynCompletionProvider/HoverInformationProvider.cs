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
            TypeInfo typeInfo;
            var syntaxRoot = await document.GetSyntaxRootAsync();

            var expressionNode = syntaxRoot.FindToken(position).Parent;
            if (expressionNode is VariableDeclaratorSyntax)
            {
                SyntaxNode childNode = expressionNode.ChildNodes()?.FirstOrDefault()?.ChildNodes()?.FirstOrDefault();
                typeInfo = semanticModel.GetTypeInfo(childNode);
                var location = expressionNode.GetLocation();
                return new HoverInfoResult() { Information = typeInfo.Type.ToString(), OffsetFrom = location.SourceSpan.Start, OffsetTo = location.SourceSpan.End };
            }

            if (expressionNode is ParameterSyntax p)
            {
                var location = expressionNode.GetLocation();
                return new HoverInfoResult() { Information = p.Type.ToString(), OffsetFrom = location.SourceSpan.Start, OffsetTo = location.SourceSpan.End };
            }

            var symbolInfo = semanticModel.GetSymbolInfo(expressionNode);
            if (symbolInfo.Symbol != null)
            {
                var location = expressionNode.GetLocation();
                return new HoverInfoResult()
                {
                    Information = HoverInfoBuilder.Build(symbolInfo),
                    OffsetFrom = location.SourceSpan.Start,
                    OffsetTo = location.SourceSpan.End
                };
            }
            return null;
        }
    }
}
