using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
    public class CompletionWorkspace
    {
        public static MetadataReference[] DefaultMetadataReferences = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(int).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("netstandard").Location),
                MetadataReference.CreateFromFile(typeof(DescriptionAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Dictionary<,>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(DataSet).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(XmlDocument).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(INotifyPropertyChanged).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Linq.Expressions.Expression).Assembly.Location)
            };

        private Project _project;
        private AdhocWorkspace _workspace;
        private List<MetadataReference> _metadataReferences;

        public static CompletionWorkspace Create(params string[] assemblies) 
        { 
            Assembly[] lst = new[] {
                Assembly.Load("Microsoft.CodeAnalysis.Workspaces"),
                Assembly.Load("Microsoft.CodeAnalysis.CSharp.Workspaces"),
                Assembly.Load("Microsoft.CodeAnalysis.Features"),
                Assembly.Load("Microsoft.CodeAnalysis.CSharp.Features")
            };

            var host = MefHostServices.Create(lst);
            var workspace = new AdhocWorkspace(host);

            var references = DefaultMetadataReferences.ToList();

            if (assemblies != null && assemblies.Length > 0)
            {
                for (int i = 0; i < assemblies.Length; i++)
                {
                    references.Add(MetadataReference.CreateFromFile(assemblies[i]));
                }
            }

            var projectInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(), "TempProject", "TempProject", LanguageNames.CSharp)
                .WithMetadataReferences(references);
            var project = workspace.AddProject(projectInfo);


            return new CompletionWorkspace() { _workspace = workspace, _project = project, _metadataReferences = references };
        }

        public async Task<CompletionDocument> CreateDocument(string code)
        {
            var document = _workspace.AddDocument(_project.Id, "MyFile2.cs", SourceText.From(code));
            var st = await document.GetSyntaxTreeAsync();
            var compilation =
            CSharpCompilation
                .Create("Temp",
                    new[] { st },
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                    references: _metadataReferences
                );

            var result = compilation.Emit("temp");
            var semanticModel = compilation.GetSemanticModel(st, true);

            
            return new CompletionDocument(document, semanticModel, result); 
        }
    }
}
