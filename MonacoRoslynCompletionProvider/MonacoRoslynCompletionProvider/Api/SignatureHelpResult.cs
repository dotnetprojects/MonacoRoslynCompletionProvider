namespace MonacoRoslynCompletionProvider.Api
{
    public class SignatureHelpResult : IResponse
    {
        public SignatureHelpResult() { }

        public virtual Signatures[] Signatures { get; set; }
        public virtual int ActiveParameter { get; set; }
        public virtual int ActiveSignature { get; set; }
    }

    public class Signatures
    {
        public virtual string Label { get; set; }

        public virtual string Documentation { get; set; }

        public virtual Parameter[] Parameters { get; set; }
    }

    public class Parameter
    {
        public virtual string Label { get; set; }

        public virtual string Documentation { get; set; }
    }
}
