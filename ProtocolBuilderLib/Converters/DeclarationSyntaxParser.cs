using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace ProtocolBuilder.Converters
{
    partial class BuilderStatic
    {
        /// <summary>
        /// Converts a local declaration statement to Swift.
        /// Differs from VariableDeclaration in that it includes the semicolon.
        /// </summary>
        /// <example>var something = something_else;</example>
        /// <param name="declaration">The LocalDeclarationStatementSyntax to convert</param>
        /// <returns>The Swift declaration, including the semicolon if necessary</returns>
        [ParsesType(typeof(LocalDeclarationStatementSyntax))]
        public static string LocalDeclarationStatement(LocalDeclarationStatementSyntax declaration)
        {
            return declaration.ConvertTo(SyntaxNode(declaration.Declaration) + Semicolon(declaration.SemicolonToken));
        }

        /// <summary>
        /// Converts a variable declaration to Swift
        /// </summary>
        /// <example>var hello = 1</example>
        /// <example>string something, something_else = "123"</example>
        /// <param name="declaration">The VariableDeclarationSyntax to convert.</param>
        /// <returns>A Swift variable declaration</returns>
        [ParsesType(typeof(VariableDeclarationSyntax))]
        public static string VariableDeclaration(VariableDeclarationSyntax declaration)
        {
            var isConst = false;
            isConst = declaration.Parent is LocalDeclarationStatementSyntax
                      && ((LocalDeclarationStatementSyntax)declaration.Parent).IsConst;
            if (!isConst)
            {
                isConst = declaration.Parent?.ToString()?.Contains("const") == true;
            }

            var isStatic =
                (declaration.Parent as BaseFieldDeclarationSyntax)?.Modifiers.Any(a1 =>
                    a1.Text.ToLower() == "static") == true;
            var isReadonly =
                (declaration.Parent as BaseFieldDeclarationSyntax)?.Modifiers.Any(a1 =>
                    a1.Text.ToLower() == "readonly") == true;
            var output = "";
            var isEnum = declaration.IsInsideEnum();

            var declareAsReadOnly = isReadonly || isConst;
            switch (Builder.Instance.Language)
            {
                case Languages.Kotlin:
                    break;
                case Languages.Swift:
                default:
                    if (!declaration.FindInParents<MethodDeclarationSyntax>() && declaration.Variables.All(a1 => a1.Initializer == null))
                        declareAsReadOnly = true;
                    break;
            }

            output = "";
            output += declaration.Variables.ConvertSeparatedSyntaxList(currVar =>
            {
                return Builder.Instance.LanguageDeclaration(
                    isEnum,
                    isStatic,
                    isConst,
                    declareAsReadOnly,
                    currVar.Identifier,
                    declaration.Type,
                    (declaration.Parent as BaseFieldDeclarationSyntax)?.AttributeLists,
                    currVar.Initializer,
                    null
                );
            });

            return output.TrimEnd(' ');
        }
    }
}