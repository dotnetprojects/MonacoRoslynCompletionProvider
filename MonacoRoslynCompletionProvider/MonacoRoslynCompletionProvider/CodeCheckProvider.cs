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
                var msg = new CodeCheckResult() { Message = r.GetMessage(), OffsetFrom = r.Location.SourceSpan.Start, OffsetTo = r.Location.SourceSpan.End };
                result.Add(msg);
            }
            return result.ToArray();
        }
    }
}
