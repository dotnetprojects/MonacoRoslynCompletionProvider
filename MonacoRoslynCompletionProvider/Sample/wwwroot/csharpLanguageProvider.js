function registerCsharpProvider() {

    var assemblies = null;

    monaco.languages.registerCompletionItemProvider('csharp', {
        triggerCharacters: ["."],
        provideCompletionItems: async (model, position) => {
            let suggestions = [];

            let request = {
                Code: model.getValue(),
                Position: model.getOffsetAt(position),
                Assemblies: assemblies
            }

            let resultQ = await axios.post("/completion/complete", JSON.stringify(request))

            for (let elem of resultQ.data) {
                suggestions.push({
                    label: {
                        label: elem.Suggestion,
                        description: elem.Description
                    },
                    kind: monaco.languages.CompletionItemKind.Function,
                    insertText: elem.Suggestion
                });
            }

            return { suggestions: suggestions };
        }
    });

    //@ts-ignore
    monaco.languages.registerSignatureHelpProvider('csharp', {
        signatureHelpTriggerCharacters: ["("],
        signatureHelpRetriggerCharacters: [","],
        //@ts-ignore
        provideSignatureHelp: async (model, position, token, context) => {
            //@ts-ignore
            let signatureHelp= {
                signatures: [
                    {
                        label: 'test',
                        documentation: "ooooo",
                        parameters: [{ label: 'par1', documentation: 'ppp' }]
                    }
                ],
                activeParameter: 0,
                activeSignature: 0
            };

            let request = {
                Code: model.getValue(),
                Position: model.getOffsetAt(position),
                Assemblies: assemblies
            }

            let resultQ = await axios.post("/completion/signature", JSON.stringify(request))

            return { value: signatureHelp };
        }
    });


    monaco.languages.registerHoverProvider('csharp', {
        provideHover: async function (model, position) {

            let request = {
                Code: model.getValue(),
                Position: model.getOffsetAt(position),
                Assemblies: assemblies
            }

            let resultQ = await axios.post("/completion/hover", JSON.stringify(request))

            if (resultQ.data) {
                posStart = model.getPositionAt(resultQ.data.OffsetFrom);
                posEnd = model.getPositionAt(resultQ.data.OffsetTo);

                return {
                    range: new monaco.Range(posStart.lineNumber, posStart.column, posEnd.lineNumber, posEnd.column),
                    contents: [
                        { value: resultQ.data.Information }
                    ]
                };
            }

            return null;
        }
    });

    monaco.editor.onDidCreateModel(function (model) {
        async function validate() {

            let request = {
                Code: model.getValue(),
                Assemblies: assemblies
            }

            let resultQ = await axios.post("/completion/codeCheck", JSON.stringify(request))

            let markers = [];

            for (let elem of resultQ.data) {
                posStart = model.getPositionAt(elem.OffsetFrom);
                posEnd = model.getPositionAt(elem.OffsetTo);
                markers.push({
                    severity: elem.Severity,
                    startLineNumber: posStart.lineNumber,
                    startColumn: posStart.column,
                    endLineNumber: posEnd.lineNumber,
                    endColumn: posEnd.column,
                    message: elem.Message,
                    code: elem.Id
                });
            }

            monaco.editor.setModelMarkers(model, 'csharp', markers);
        }

        var handle = null;
        model.onDidChangeContent(() => {
            monaco.editor.setModelMarkers(model, 'csharp', []);
            clearTimeout(handle);
            handle = setTimeout(() => validate(), 500);
        });
        validate();
    });

    monaco.languages.registerCodeActionProvider("csharp", {
        provideCodeActions: async (model, range, context, token) => {
            const actions = context.markers.map(error => {
                console.log(context, error);
                return {
                    title: `Example quick fix`,
                    diagnostics: [error],
                    kind: "quickfix",
                    edit: {
                        edits: [
                            {
                                resource: model.uri,
                                edits: [
                                    {
                                        range: error,
                                        text: "This text replaces the text with the error"
                                    }
                                ]
                            }
                        ]
                    },
                    isPreferred: true
                };
            });
            return {
                actions: actions,
                dispose: () => { }
            }
        }
    });

}