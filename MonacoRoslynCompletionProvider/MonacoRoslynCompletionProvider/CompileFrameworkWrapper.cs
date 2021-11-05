using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace MonacoRoslynCompletionProvider
{
    public class CompletionProvider
    {
        // Thanks to https://www.strathweb.com/2018/12/using-roslyn-c-completion-service-programmatically/
        public async static Task<TabCompletionDTO[]> GetCodeCompletionForCode(string code, int position, params string[] assemblies)
        {
            Assembly[] lst = new[] {
                Assembly.Load("Microsoft.CodeAnalysis.Workspaces"),
                Assembly.Load("Microsoft.CodeAnalysis.CSharp.Workspaces"),
                Assembly.Load("Microsoft.CodeAnalysis.Features"),
                Assembly.Load("Microsoft.CodeAnalysis.CSharp.Features")
            };
            
            var host = MefHostServices.Create(lst);
            var workspace = new AdhocWorkspace(host);

            const string systemCollectionsName = "System.Collections";
            var systemCollectionsAssembly = Assembly.Load(systemCollectionsName);
            const string systemRuntimeName = "System.Runtime";
            var systemRuntimeAssembly = Assembly.Load(systemRuntimeName);
            const string netstandardName = "netstandard";
            var netstandardAssembly = Assembly.Load(netstandardName);
            const string systemComponentModelName = "System.ComponentModel";
            var systemComponentModelAssembly = Assembly.Load(systemComponentModelName);

            var references = new List<MetadataReference>()
            {
                MetadataReference.CreateFromFile(systemCollectionsAssembly.Location),
                MetadataReference.CreateFromFile(systemRuntimeAssembly.Location),
                MetadataReference.CreateFromFile(netstandardAssembly.Location),
                MetadataReference.CreateFromFile(systemComponentModelAssembly.Location),
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Dictionary<,>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(DataSet).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(XmlDocument).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(INotifyPropertyChanged).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Linq.Expressions.Expression).Assembly.Location)
            };

            if (assemblies != null && assemblies.Length > 0)
            {
                for (int i = 0; i < assemblies.Length; i++)
                {
                    references.Add(MetadataReference.CreateFromFile(Assembly.Load(assemblies[i]).Location));
                }
            }

            var projectInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(), "MyProject", "MyProject", LanguageNames.CSharp)
                .WithMetadataReferences(references);
            var project = workspace.AddProject(projectInfo);

            var document = workspace.AddDocument(project.Id, "MyFile.cs", SourceText.From(code));
            var res = await PrintCompletionResults(document, position);

            return res;
        }

        private static async Task<TabCompletionDTO[]> PrintCompletionResults(Document document, int position)
        {
            var completionService = CompletionService.GetService(document);
            var results = await completionService.GetCompletionsAsync(document, position);

            var tabCompletionDTOs = new TabCompletionDTO[results.Items.Length];

            if (results != null)
            {
                var suggestions = new string[results.Items.Length];

                for (int i = 0; i < results.Items.Length; i++)
                {
                    var itemDescription = await completionService.GetDescriptionAsync(document, results.Items[i]);

                    var dto = new TabCompletionDTO();
                    dto.Suggestion = results.Items[i].DisplayText;
                    dto.Description = itemDescription.Text;

                    tabCompletionDTOs[i] = dto;
                    suggestions[i] = results.Items[i].DisplayText;
                }

                return tabCompletionDTOs;
            } 
            else
            {
                return Array.Empty<TabCompletionDTO>();
            }
        }
    }
}
