using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProtocolBuilder.Converters
{
    partial class BuilderStatic
    {
        /// <summary>
        /// The newline character to use for the outputted Swift code
        /// </summary>
        public static readonly string NewLine = Environment.NewLine;

        /// <summary>
        /// The Roslyn SemanticModel to reference when converting C# code
        /// </summary>
        public static SemanticModel Model;

        /// <summary>
        /// Converts a C# Roslyn SyntaxNode to its Swift equivilant
        /// </summary>
        /// <param name="node">Roslyn SyntaxNode representing the C# code to convert</param>
        /// <returns>A string with the converted Swift code</returns>
        public static string SyntaxNode(SyntaxNode node)
        {
            if (node == null) return "";

            if (node.HasLeadingTrivia)
            {
                var ignoreNode = node.GetLeadingTrivia()
                    .Where(trivia =>
                        trivia.IsKind(SyntaxKind.MultiLineCommentTrivia) ||
                        trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
                    .Any(trivia => trivia.ToString().TrimStart('/', '*').ToLower().Trim() == "ignore");

                if (ignoreNode) return "";
            }

            if (node is BlockSyntax)
            {
                //Block() takes two arguments, & I don't want to worry about it w/ reflection.
                return Block((BlockSyntax) node);
            }

            /*
             * We're gonna search through ConvertToSwift's static methods for one
             * with the ParsesType attribute that matches the typeof node.
             * If one isn't found we'll just return the C# code
             */
            var nodeType = node.GetType();

            var methods = typeof(BuilderStatic).GetMethods();
            var matchedMethod =
                methods.FirstOrDefault(method => //find method that parses this syntax
                    method.GetCustomAttributes(true).OfType<ParsesTypeAttribute>()
                        .Any(attr => attr.ParsesType == nodeType));

            if (matchedMethod != null)
            {
                return matchedMethod.Invoke(new BuilderStatic(), new[] {node}).ToString();
            }

            return node + NewLine;
        }

        /// <summary>
        /// Parses a C# Roslyn code block (code between two curly brackets)
        /// </summary>
        /// <param name="node">The BlockSyntax to convert</param>
        /// <param name="braces"></param>
        /// <returns>A Swift converted version of the code</returns>
        [ParsesType(typeof(BlockSyntax))]
        private static string Block(BlockSyntax node, bool braces = true)
        {
            return node.ConvertTo(
                SyntaxTokenConvert(node.OpenBraceToken).TrimStart()
                + node.ChildNodes().ConvertSyntaxNodes()
                + SyntaxTokenConvert(node.CloseBraceToken).TrimEnd()
            );
        }

        internal static string SyntaxTokenConvert(SyntaxToken source)
        {
            return Builder.Instance.LanguageSyntaxTokenConvert(source);
        }
    }

    public static class Extensions
    {
        public static string ConvertSeparatedSyntaxList<T>(
            this SeparatedSyntaxList<T> source,
            Func<T, string> itemConverter = null,
            string separatorForced = null
        ) where T : SyntaxNode
        {
            var separator = separatorForced ??
                            BuilderStatic.SyntaxTokenConvert(source.GetSeparators().FirstOrDefault());
            if (itemConverter == null)
                itemConverter = BuilderStatic.SyntaxNode;

            return string.Join(separator, source.Select(itemConverter));
        }

        public static string ConvertSyntaxList<T>(this SyntaxList<T> source) where T : SyntaxNode
        {
            return string.Join("", source.Select(BuilderStatic.SyntaxNode));
        }

        public static string ConvertSyntaxNodes(this IEnumerable<SyntaxNode> source, string separator = "")
        {
            return string.Join(separator, source.Select(BuilderStatic.SyntaxNode));
        }

        public static string ConvertTo(this SyntaxNode source, string middle)
        {
            return $"{source.GetLeadingTrivia().ToFullString()}{middle}{source.GetTrailingTrivia().ToFullString()}";
        }

        public static bool FindInParents<T>(this SyntaxNode source)
        {
            var result = false;
            var parent = source?.Parent;
            while (parent != null)
            {
                if (parent is T)
                {
                    result = true;
                    break;
                }

                parent = parent?.Parent;
            }

            return result;
        }
    }
}