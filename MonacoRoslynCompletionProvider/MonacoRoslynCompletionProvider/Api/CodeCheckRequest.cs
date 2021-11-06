namespace MonacoRoslynCompletionProvider.Api
{
    public class CodeCheckRequest
    {
        public CodeCheckRequest()
        { }

        public CodeCheckRequest(string code, string[] assemblies)
        {
            this.Code = code;
            this.Assemblies = assemblies;
        }

        public virtual string Code { get; set; }

        public virtual string[] Assemblies { get; set; }
    }
}
