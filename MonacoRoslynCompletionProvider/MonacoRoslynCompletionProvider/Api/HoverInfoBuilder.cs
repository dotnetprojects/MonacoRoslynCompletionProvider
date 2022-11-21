using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Elfie.Model.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonacoRoslynCompletionProvider.Api
{
    public abstract class HoverInfoBuilder
    {
        public static string Build(SymbolInfo symbolInfo)
        {
            if (symbolInfo.Symbol.GetType().Name.Equals("MethodSymbol"))
            {
                IMethodSymbol symbol = (IMethodSymbol)symbolInfo.Symbol;
                return new StringBuilder().Append("(method) ").Append(symbol.DeclaredAccessibility.ToString().ToLower()).Append(" ")
                    .Append(symbol.ToDisplayString()).Append(" : ")
                    .Append(symbol.ReturnType).ToString();
            }
            return symbolInfo.Symbol.ToDisplayString();
        }
    }
}
