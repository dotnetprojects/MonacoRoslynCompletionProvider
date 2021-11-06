namespace MonacoRoslynCompletionProvider.Api
{
    public class CodeCheckResult
    {
        public CodeCheckResult() { }

        public virtual string Message { get; set; }

        public virtual int OffsetFrom { get; set; }

        public virtual int OffsetTo { get; set; }
    }
}
