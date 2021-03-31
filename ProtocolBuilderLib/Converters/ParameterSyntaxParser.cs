using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProtocolBuilder.Converters
{
    partial class BuilderStatic
    {
        /// <summary>
        /// Converts a parameter to Swift
        /// </summary>
        /// <example>ParameterSyntax param = null</example>
        /// <param name="param">The parameter to convert</param>
        /// <returns>The converted Swift parameter</returns>
        [ParsesType(typeof(ParameterSyntax))]
        public static string Parameter(ParameterSyntax param)
        {
            var typeSyntax = param.Type;
            if (param.Type is IdentifierNameSyntax)
            {
                typeSyntax = (IdentifierNameSyntax)param.Type;
            }
            if (param.Modifiers.Any(mod => mod.ToString() == "params"))
            {
                typeSyntax = ((ArrayTypeSyntax)param.Type);
            }

            return Builder.Instance.LanguageDeclaration(
                    false,
                    false,
                    false,
                    false,
                    param.Identifier,
                    param.Type,
                    null,
                    param.Default,
                    null,
                    param.GetLeadingTrivia(),
                    true
                );
        }

        /// <summary>
        /// Converts a type parameter to Swift
        /// </summary>
        /// <example>Method<T: IEnumerable></typeparam></example>
        /// <param name="param">The parameter to convert</param>
        /// <returns>The converted Swift parameter</returns>
        [ParsesType(typeof(TypeParameterSyntax))]
        public static string TypeParameter(TypeParameterSyntax param)
        {
            if (!(param.Parent.Parent is MethodDeclarationSyntax)) return param.Identifier.Text;

            var typeConstraints = ((MethodDeclarationSyntax)param.Parent.Parent).ConstraintClauses;
            var constraints = typeConstraints
                .FirstOrDefault(constr => constr.Name.Identifier.Text == param.Identifier.Text).Constraints;

            return "<" + param.Identifier.Text + ": " + string.Join(", ", constraints) + ">"; //TODO: check if this is the right syntax for multiple constraints
        }
    }
}
