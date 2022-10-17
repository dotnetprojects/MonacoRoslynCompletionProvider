using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using MonacoRoslynCompletionProvider.Api;
//using MonacoRoslynCompletionProvider.RoslynPad;
//using RoslynPad.Roslyn.QuickInfo;
using System.Threading;
using System.Threading.Tasks;

namespace MonacoRoslynCompletionProvider
{
    public class CompletionDocument
    {
        private readonly Document document;
        private readonly SemanticModel semanticModel;
        private readonly EmitResult emitResult;

        //private QuickInfoProvider quickInfoProvider;

        internal CompletionDocument(Document document, SemanticModel semanticModel, EmitResult emitResult)
        {
            this.document = document;
            this.semanticModel = semanticModel;
            this.emitResult = emitResult;

            //this.quickInfoProvider = new QuickInfoProvider(new DeferredQuickInfoContentProvider());
        }

        public Task<HoverInfoResult> GetHoverInformation(int position, CancellationToken cancellationToken)
        {
            //var info = await quickInfoProvider.GetItemAsync(document, position, cancellationToken);
            //return new HoverInfoResult() { Information = info.Create().ToString() };
            var hoverInformationProvider = new HoverInformationProvider();
            return hoverInformationProvider.Provide(document, position, semanticModel);
        }

        public Task<TabCompletionResult[]> GetTabCompletion(int position, CancellationToken cancellationToken)
        {
            var tabCompletionProvider = new TabCompletionProvider();
            return tabCompletionProvider.Provide(document, position);
        }

        public async Task<CodeCheckResult[]> GetCodeCheckResults(CancellationToken cancellationToken)
        {
            var codeCheckProvider = new CodeCheckProvider();
            return await codeCheckProvider.Provide(emitResult, document, cancellationToken);
        }
    }
}
