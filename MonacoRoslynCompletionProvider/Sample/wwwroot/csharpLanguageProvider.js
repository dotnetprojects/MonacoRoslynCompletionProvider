function registerCsharpProvider() {

    monaco.languages.registerCompletionItemProvider('csharp', {
        triggerCharacters: [".", "("],
        provideCompletionItems: async (model, position) => {
            let textUntilPosition = model.getValueInRange({ startLineNumber: 1, startColumn: 1, endLineNumber: position.lineNumber, endColumn: position.column });
            let suggestions = [];

            let request = {
                Code: model.getValue(),
                Position: textUntilPosition.length,
                Assemblies: null
            }

            let resultQ = await axios.post("/completion/complete", JSON.stringify(request))

            for (let elem of resultQ.data) {
                suggestions.push({
                    label: elem.Suggestion,
                    kind: monaco.languages.CompletionItemKind.Function,
                    insertText: elem.Suggestion,
                    documentation: elem.Description
                });
            }

            return { suggestions: suggestions };
        }
    });

    monaco.languages.registerHoverProvider('csharp', {
        provideHover: async function (model, position) {

            let request = {
                Code: model.getValue(),
                Position: model.getOffsetAt(position),
                Assemblies: null
            }

            let resultQ = await axios.post("/completion/hover", JSON.stringify(request))

            posStart = model.getPositionAt(resultQ.data.OffsetFrom);
            posEnd = model.getPositionAt(resultQ.data.OffsetTo);

            return {
                range: new monaco.Range(posStart.lineNumber, posStart.column, posEnd.lineNumber, posEnd.column),
                contents: [
                    { value: resultQ.data.Information }
                ]
            };
        }
    });

}