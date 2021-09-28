using Microsoft.Quantum.QsCompiler.SyntaxTokens;
using Microsoft.Quantum.QsCompiler.SyntaxTree;

namespace RewriteQsharpCompiler
{
    public static class QsCallableExtension
    {
		public static QsCallable ToCaseDriveAccessModifier(this QsCallable c)
		{
			var accessModifier = char.IsUpper(c.FullName.Name[0]) ? AccessModifier.DefaultAccess : AccessModifier.Internal;
			if (accessModifier.IsDefaultAccess == c.Modifiers.Access.IsDefaultAccess) return c;

			return new QsCallable(
				c.Kind, c.FullName, c.Attributes, new Modifiers(accessModifier),
				c.SourceFile, c.Location,
				c.Signature, c.ArgumentTuple, c.Specializations,
				c.Documentation, c.Comments);
		}
	}
}
