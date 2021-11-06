namespace MonacoRoslynCompletionProvider.Api
{
    public class TabCompletionResult
    {
        public TabCompletionResult() { }

        public virtual string Suggestion { get; set; }

        public virtual string Description { get; set; }
    }
}
