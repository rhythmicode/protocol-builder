using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProtocolBuilder.Converters
{
    partial class BuilderStatic
    {
        /// <summary>
        /// Converts an equals value clause to Swift
        /// </summary>
        /// <example>= something_else()</example>
        /// <param name="clause">The clause to convert</param>
        /// <returns>The converted Swift clause</returns>
        [ParsesType(typeof(EqualsValueClauseSyntax))]
        public static string EqualsValueClause(EqualsValueClauseSyntax clause)
        {
            //return clause.EqualsToken + " " + SyntaxNode(clause.Value).Trim();
            //if (clause.EqualsToken.ToString() == "=")
            //    return ""; //$" = {SyntaxNode(clause.Value).Trim()}";
            //else
            return SyntaxTokenConvert(clause.EqualsToken) + SyntaxNode(clause.Value).Trim();
        }
    }
}
