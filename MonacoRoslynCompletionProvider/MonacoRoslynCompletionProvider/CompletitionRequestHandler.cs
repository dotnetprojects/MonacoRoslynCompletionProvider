using MonacoRoslynCompletionProvider.Api;
using System.Threading.Tasks;

namespace MonacoRoslynCompletionProvider
{
    public static class CompletitionRequestHandler
    {
        public async static Task<TabCompletionResult[]> Handle(TabCompletionRequest tabCompletionRequest)
        {
            var workspace = CompletionWorkspace.Create(tabCompletionRequest.Assemblies);
            var document = await workspace.CreateDocument(tabCompletionRequest.Code);
            return await document.GetTabCompletion(tabCompletionRequest.Position);
        }

        public async static Task<HoverInfoResult> Handle(HoverInfoRequest hoverInfoRequest)
        {
            var workspace = CompletionWorkspace.Create(hoverInfoRequest.Assemblies);
            var document = await workspace.CreateDocument(hoverInfoRequest.Code);
            return await document.GetHoverInformation(hoverInfoRequest.Position);
        }
    }
}
