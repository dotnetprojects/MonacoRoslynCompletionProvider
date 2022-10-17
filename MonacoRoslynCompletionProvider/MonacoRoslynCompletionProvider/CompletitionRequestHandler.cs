using MonacoRoslynCompletionProvider.Api;
using System.Threading;
using System.Threading.Tasks;

namespace MonacoRoslynCompletionProvider
{
    public static class CompletitionRequestHandler
    {
        public async static Task<TabCompletionResult[]> Handle(TabCompletionRequest tabCompletionRequest)
        {
            var workspace = CompletionWorkspace.Create(tabCompletionRequest.Assemblies);
            var document = await workspace.CreateDocument(tabCompletionRequest.Code);
            return await document.GetTabCompletion(tabCompletionRequest.Position, CancellationToken.None);
        }

        public async static Task<HoverInfoResult> Handle(HoverInfoRequest hoverInfoRequest)
        {
            var workspace = CompletionWorkspace.Create(hoverInfoRequest.Assemblies);
            var document = await workspace.CreateDocument(hoverInfoRequest.Code);
            return await document.GetHoverInformation(hoverInfoRequest.Position, CancellationToken.None);
        }

        public async static Task<CodeCheckResult[]> Handle(CodeCheckRequest codeCheckRequest)
        {
            var workspace = CompletionWorkspace.Create(codeCheckRequest.Assemblies);
            var document = await workspace.CreateDocument(codeCheckRequest.Code);
            return await document.GetCodeCheckResults(CancellationToken.None);
        }
    }
}
