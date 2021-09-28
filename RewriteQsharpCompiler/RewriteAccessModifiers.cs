using Microsoft.Quantum.QsCompiler.SyntaxTree;
using Microsoft.Quantum.QsCompiler.Transformations.Core;
using System;

namespace RewriteQsharpCompiler
{
    public class RewriteAccessModifiers : SyntaxTreeTransformation<EmptyState>
    {
        public RewriteAccessModifiers() : base(new EmptyState())
        {
            Namespaces = new NamespaceTransformation(this);
        }

        private class NamespaceTransformation : NamespaceTransformation<EmptyState>
        {
            public NamespaceTransformation(SyntaxTreeTransformation<EmptyState> parent) : base(parent, TransformationOptions.Default)
            {}

            public override QsCallable OnCallableDeclaration(QsCallable c)
            {
                if (!c.SourceFile.EndsWith(".qs", StringComparison.OrdinalIgnoreCase))
                    return c;
                Console.WriteLine($"Callable: {c.FullName} in {c.SourceFile}");
                var transformed = c.ToCaseDriveAccessModifier();
                Console.WriteLine($" modifier: {c.Modifiers.Access} -> modifier: {transformed.Modifiers.Access}");
                return transformed;
            }
        }
    }

    public class EmptyState
    { }
}