using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using MonacoRoslynCompletionProvider.Api;
using System.Threading.Tasks;

namespace MonacoRoslynCompletionProvider
{
    public class CompletionDocument
    {
        private readonly Document document;
        private readonly SemanticModel semanticModel;
        private readonly EmitResult emitResult;

        internal CompletionDocument(Document document, SemanticModel semanticModel, EmitResult emitResult)
        {
            this.document = document;
            this.semanticModel = semanticModel;
            this.emitResult = emitResult;
        }

        public Task<HoverInfoResult> GetHoverInformation(int position)
        {
            var hoverInformationProvider = new HoverInformationProvider();
            return hoverInformationProvider.Provide(document, position, semanticModel);
        }

        public Task<TabCompletionResult[]> GetTabCompletion(int position)
        {
            var tabCompletionProvider = new TabCompletionProvider();
            return tabCompletionProvider.Provide(document, position);
        }

        public async Task<CodeCheckResult[]> GetCodeCheckResults()
        {
            var codeCheckProvider = new CodeCheckProvider();
            return codeCheckProvider.Provide(emitResult);
        }
    }
}
