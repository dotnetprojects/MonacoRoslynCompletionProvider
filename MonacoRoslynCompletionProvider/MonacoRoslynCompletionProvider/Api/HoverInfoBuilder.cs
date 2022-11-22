using Microsoft.CodeAnalysis;
using System.Text;

namespace MonacoRoslynCompletionProvider.Api
{
    public abstract class HoverInfoBuilder
    {
        public static string Build(SymbolInfo symbolInfo)
        {
            if (symbolInfo.Symbol is IMethodSymbol symbol)
            {
                return new StringBuilder().Append("(method) ").Append(symbol.DeclaredAccessibility.ToString().ToLower()).Append(' ')
                    .Append(symbol.ToDisplayString()).Append(" : ")
                    .Append(symbol.ReturnType).ToString();
            }
            return symbolInfo.Symbol.ToDisplayString();
        }
    }
}
