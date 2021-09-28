using Microsoft.CodeAnalysis;
using Microsoft.Quantum.QsCompiler;
using Microsoft.Quantum.QsCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RewriteQsharpCompiler
{
    public class Case : IRewriteStep
    {
        private readonly List<IRewriteStep.Diagnostic> diagnostics = new List<IRewriteStep.Diagnostic>();
        public string Name => "Case-based Access Modifiers";
        public int Priority => 0;
        public IDictionary<string, string> AssemblyConstants { get; } = new Dictionary<string, string>();
        public IEnumerable<IRewriteStep.Diagnostic> GeneratedDiagnostics => diagnostics;
        public bool ImplementsPreconditionVerification => true;
        public bool ImplementsTransformation => true;
        public bool ImplementsPostconditionVerification => false;

        public bool PreconditionVerification(QsCompilation compilation)
        {
            var invalidAccessibility = compilation.Namespaces.Callables()
                .Where(x => x.SourceFile.EndsWith(".qs"))
                .Where(x => x.Modifiers.Access.IsDefaultAccess && !char.IsUpper(x.FullName.Name[0]) ||
                            x.Access.IsInternal && char.IsUpper(x.FullName.Name[0]));

            foreach (var callable in invalidAccessibility)
            {
                diagnostics.Add(new IRewriteStep.Diagnostic
                {
                    Severity = DiagnosticSeverity.Warning,
                    Message = $@"Callable '{callable.FullName} should be {(callable.Access.IsPublic ? "internal" : "public")}. This will be auto-corrected.'",
                    Stage = IRewriteStep.Stage.PreconditionVerification,
                    Source = callable.SourceFile,
                    Range = callable.Location.IsValue ? callable.Location.Item.Range : null

                });
            }

            return true;
        }

        public bool Transformation(QsCompilation compilation, out QsCompilation transformed)
        {
            var rewriter = new RewriteAccessModifiers();
            transformed = rewriter.OnCompilation(compilation);
            return true;
        }

        public bool PostconditionVerification(QsCompilation compilation)
        {
            throw new NotImplementedException();
        }
    }
}
