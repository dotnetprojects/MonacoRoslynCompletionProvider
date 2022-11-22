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
                var sb = new StringBuilder().Append("(method) ").Append(symbol.DeclaredAccessibility.ToString().ToLower()).Append(' ');
                if (symbol.IsStatic)
                    sb.Append("static").Append(' ');
                sb.Append(symbol.ToDisplayString()).Append(" : ")
                    .Append(symbol.ReturnType).ToString();
                return sb.ToString();
            }
            if (symbolInfo.Symbol is ILocalSymbol localsymbol)
                return new StringBuilder().Append(localsymbol.Name).Append(" : ").Append(localsymbol.Type).ToString();
            return string.Empty;
        }
    }
}
