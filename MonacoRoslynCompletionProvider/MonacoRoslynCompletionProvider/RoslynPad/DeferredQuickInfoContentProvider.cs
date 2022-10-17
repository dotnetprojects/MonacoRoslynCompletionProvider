//using Microsoft.CodeAnalysis;
//using RoslynPad.Roslyn.QuickInfo;
//using System.Collections.Generic;
//using System.Linq;

//namespace MonacoRoslynCompletionProvider.RoslynPad
//{
//    internal class DeferredQuickInfoContentProvider : IDeferredQuickInfoContentProvider
//    {
//        public IDeferredQuickInfoContent CreateQuickInfoDisplayDeferredContent(
//            ISymbol symbol,
//            bool showWarningGlyph,
//            bool showSymbolGlyph,
//            IList<TaggedText> mainDescription,
//            IDeferredQuickInfoContent documentation,
//            IList<TaggedText> typeParameterMap,
//            IList<TaggedText> anonymousTypes,
//            IList<TaggedText> usageText,
//            IList<TaggedText> exceptionText)
//        {
//            var text = "";
//            if (mainDescription != null)
//                text += string.Join("", mainDescription.Select(x => x.Text));
//            return new DeferredQuickInfoContent(text);
//        }


//        public IDeferredQuickInfoContent CreateDocumentationCommentDeferredContent(string documentationComment)
//        {
//            return new DeferredQuickInfoContent(documentationComment);
//        }

//        public IDeferredQuickInfoContent CreateClassifiableDeferredContent(IList<TaggedText> content)
//        {
//            if (content.Count > 0)
//                return new DeferredQuickInfoContent(string.Join("", content.Select(x=>x.Text)));
//            return new DeferredQuickInfoContent("");
//        }

//        private class DeferredQuickInfoContent : IDeferredQuickInfoContent
//        {
//            private readonly string _data;

//            public DeferredQuickInfoContent(string data)
//            {
//                _data = data;
//            }
//            public object Create() => _data;
//        }
//    }
//}
