using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProtocolBuilder.Converters
{
    partial class BuilderStatic
    {
        /// <summary>
        /// Detects whether an attribute is a certain ProtocolBuilder-specific Attribute (like [ExportAs])
        /// </summary>
        /// <param name="attribute">The AttributeSyntax to check</param>
        /// <param name="expectingName">The attribute name you're looking for, or null to match all ProtocolBuilder attributes</param>
        /// <returns>A boolean value representing whether it is or isn't the ProtocolBuilder attribute</returns>
        private static bool IsProtocolBuilderAttribute(AttributeSyntax attribute, string expectingName = null)
        {
            var symbol = Model.GetSymbolInfo(attribute.Name).Symbol;
            if (symbol == null) return false;

            var containingNamespace = symbol.ContainingSymbol.ContainingNamespace;

            var containingContainingNamespace = containingNamespace.ContainingNamespace;
            if (containingContainingNamespace == null) return false;

            return containingContainingNamespace.Name == "ProtocolBuilder"
                   && containingNamespace.Name == "Attributes"
                   && (expectingName == null || symbol.ContainingSymbol.Name == expectingName);
        }

        /// <summary>
        /// Converts a C# attribute, ignoring ProtocolBuilder-specific attributes
        /// </summary>
        /// <param name="attribute">The attribute to convert</param>
        /// <returns>Swift code representing the same attribute</returns>
        [ParsesType(typeof (AttributeSyntax))]
        public static string AttributeSyntax(AttributeSyntax attribute)
        {
            var output = "@" + SyntaxNode(attribute.Name).TrimEnd('!');

            if (IsProtocolBuilderAttribute(attribute) || attribute.ToString().ToLower().Contains("nullable"))
            {
                return "";
            }

            if (attribute.ArgumentList != null)
            {
                output += SyntaxNode(attribute.ArgumentList);
            }

            return output;
        }
    }
}
