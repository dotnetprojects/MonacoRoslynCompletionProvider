async function sendRequest(type, request) {
    let endPoint;
    switch (type) {
        case 'complete': endPoint = '/completion/complete'; break;
        case 'signature': endPoint = '/completion/signature'; break;
        case 'hover': endPoint = '/completion/hover'; break;
        case 'codeCheck': endPoint = '/completion/codeCheck'; break;
    }
    return await axios.post(endPoint, JSON.stringify(request))
}

function registerCsharpProvider() {

    var assemblies = null;

    monaco.languages.registerCompletionItemProvider('csharp', {
        triggerCharacters: [".", " "],
        provideCompletionItems: async (model, position) => {
            let suggestions = [];

            let request = {
                Code: model.getValue(),
                Position: model.getOffsetAt(position),
                Assemblies: assemblies
            }

            let resultQ = await sendRequest("complete", request);

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

    monaco.languages.registerSignatureHelpProvider('csharp', {
        signatureHelpTriggerCharacters: ["("],
        signatureHelpRetriggerCharacters: [","],

        provideSignatureHelp: async (model, position, token, context) => {

            let request = {
                Code: model.getValue(),
                Position: model.getOffsetAt(position),
                Assemblies: assemblies
            }

            let resultQ = await sendRequest("signature", request);
            if (!resultQ.data) return;

            let signatures = [];
            for (let signature of resultQ.data.Signatures) {
                let params = [];
                for (let param of signature.Parameters) {
                    params.push({
                        label: param.Label,
                        documentation: param.Documentation ?? ""
                    });
                }

                signatures.push({
                    label: signature.Label,
                    documentation: signature.Documentation ?? "",
                    parameters: params,
                });
            }

            let signatureHelp = {};
            signatureHelp.signatures = signatures;
            signatureHelp.activeParameter = resultQ.data.ActiveParameter;
            signatureHelp.activeSignature = resultQ.data.ActiveSignature;

            return {
                value: signatureHelp,
                dispose: () => { }
            };
        }
    });


    monaco.languages.registerHoverProvider('csharp', {
        provideHover: async function (model, position) {

            let request = {
                Code: model.getValue(),
                Position: model.getOffsetAt(position),
                Assemblies: assemblies
            }

            let resultQ = await sendRequest("hover", request);

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

            let resultQ = await sendRequest("codeCheck", request)

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

    /*monaco.languages.registerInlayHintsProvider('csharp', {
        displayName: 'test',
        provideInlayHints(model, range, token) {
            return {
                hints: [
                    {
                        label: "Test",
                        tooltip: "Tooltip",
                        position: { lineNumber: 3, column: 2},
                        kind: 2
                    }
                ],
                dispose: () => { }
            };
        }

    });*/

    /*monaco.languages.registerCodeActionProvider("csharp", {
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
    });*/

}