using Microsoft.CodeAnalysis;
using System.Text;

namespace MonacoRoslynCompletionProvider.Api
{
    public abstract class HoverInfoBuilder
    {
        public static string Build(SymbolInfo symbolInfo)
        {
            if (symbolInfo.Symbol is IMethodSymbol methodsymbol)
            {
                var sb = new StringBuilder().Append("(method) ").Append(methodsymbol.DeclaredAccessibility.ToString().ToLower()).Append(' ');
                if (methodsymbol.IsStatic)
                    sb.Append("static").Append(' ');
                sb.Append(methodsymbol.Name).Append('(');
                for (var i = 0; i < methodsymbol.Parameters.Length; i++)
                {
                    sb.Append(methodsymbol.Parameters[i].Type).Append(' ').Append(methodsymbol.Parameters[i].Name);
                    if (i < (methodsymbol.Parameters.Length - 1)) sb.Append(", ");
                }
                sb.Append(") : ");
                sb.Append(methodsymbol.ReturnType).ToString();
                return sb.ToString();
            }
            if (symbolInfo.Symbol is ILocalSymbol localsymbol)
            {
                var sb = new StringBuilder().Append(localsymbol.Name).Append(" : ");
                if (localsymbol.IsConst)
                    sb.Append("const").Append(' ');
                sb.Append(localsymbol.Type);
                return sb.ToString();
            }
            if (symbolInfo.Symbol is IFieldSymbol fieldSymbol)
            {
                var sb = new StringBuilder().Append(fieldSymbol.Name).Append(" : ").Append(fieldSymbol.DeclaredAccessibility.ToString().ToLower()).Append(' ');
                if (fieldSymbol.IsStatic)
                    sb.Append("static").Append(' ');
                if (fieldSymbol.IsReadOnly)
                    sb.Append("readonly").Append(' ');
                if (fieldSymbol.IsConst)
                    sb.Append("const").Append(' ');
                sb.Append(fieldSymbol.Type).ToString();
                return sb.ToString();
            }

            return string.Empty;
        }
    }
}
