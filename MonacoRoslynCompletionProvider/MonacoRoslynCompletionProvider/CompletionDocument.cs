using Microsoft.CodeAnalysis;
using MonacoRoslynCompletionProvider.Api;
using System.Threading.Tasks;

namespace MonacoRoslynCompletionProvider
{
    public class CompletionDocument
    {
        private readonly Document document;
        private readonly SemanticModel semanticModel;

        internal CompletionDocument(Document document, SemanticModel semanticModel)
        {
            this.document = document;
            this.semanticModel = semanticModel;
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
    }
}
