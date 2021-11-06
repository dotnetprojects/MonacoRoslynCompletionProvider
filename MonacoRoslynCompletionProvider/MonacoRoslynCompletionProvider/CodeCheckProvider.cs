using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using MonacoRoslynCompletionProvider.Api;
using System.Collections.Generic;

namespace MonacoRoslynCompletionProvider
{
    public class CodeCheckProvider
    {
        public CodeCheckProvider()
        {
        }

        public CodeCheckResult[] Provide(EmitResult emitResult)
        {
            var result = new List<CodeCheckResult>();
            foreach(var r in emitResult.Diagnostics)
            {
                var sev = r.Severity == DiagnosticSeverity.Error ? CodeCheckSeverity.Error : r.Severity == DiagnosticSeverity.Warning ? CodeCheckSeverity.Warning : r.Severity == DiagnosticSeverity.Info ? CodeCheckSeverity.Info : CodeCheckSeverity.Hint;
                var msg = new CodeCheckResult() { Message = r.GetMessage(), OffsetFrom = r.Location.SourceSpan.Start, OffsetTo = r.Location.SourceSpan.End, Severity = sev };
                result.Add(msg);
            }
            return result.ToArray();
        }
    }
}
