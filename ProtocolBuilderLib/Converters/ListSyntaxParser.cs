using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProtocolBuilder.Converters
{
    partial class BuilderStatic
    {
        /// <summary>
        /// Converts a type argument list to Swift
        /// </summary>
        /// <param name="list">The list to convert</param>
        /// <returns>The converted Swift list</returns>
        [ParsesType(typeof(TypeArgumentListSyntax))]
        public static string TypeArgumentList(TypeArgumentListSyntax list)
        {
            return "<" + list.Arguments.ConvertSeparatedSyntaxList() + ">";
        }

        /// <summary>
        /// Converts a type parameter list to Swift
        /// </summary>
        /// <param name="list">The list to convert</param>
        /// <returns>The converted Swift list</returns>
        [ParsesType(typeof(TypeParameterListSyntax))]
        public static string TypeParameterList(TypeParameterListSyntax list)
        {
            return list.Parameters.ConvertSeparatedSyntaxList();
        }

        /// <summary>
        /// Converts a parameter list to Swift
        /// </summary>
        /// <example>(ParameterListSyntax list)</example>
        /// <param name="list">The parameter list to convert</param>
        /// <returns>The converted Swift list</returns>
        [ParsesType(typeof(ParameterListSyntax))]
        public static string ParameterList(ParameterListSyntax list)
        {
            var paramsConverted = list.Parameters.ConvertSeparatedSyntaxList(itemConverter: (toConvert) =>
                ((Builder.Instance.Language == Languages.Swift) ? "_ " : "")
                + SyntaxNode(toConvert)
            );
            return
                $"{SyntaxTokenConvert(list.OpenParenToken)}{paramsConverted}{SyntaxTokenConvert(list.CloseParenToken)}";
        }

        /// <summary>
        /// Converts an attribute list to Swift
        /// </summary>
        /// <param name="list">The list to convert</param>
        /// <returns>The converted Swift list</returns>
        [ParsesType(typeof(AttributeListSyntax))]
        public static string AttributeList(AttributeListSyntax list)
        {
            return list.Attributes.ConvertSeparatedSyntaxList();
        }

        /// <summary>
        /// Converts an argument list to Swift
        /// </summary>
        /// <param name="list">The list to convert</param>
        /// <returns>The converted Swift list</returns>
        [ParsesType(typeof(ArgumentListSyntax))]
        public static string ArgumentList(ArgumentListSyntax list)
        {
            return
                $"{SyntaxTokenConvert(list.OpenParenToken)}{list.Arguments.ConvertSeparatedSyntaxList()}{SyntaxTokenConvert(list.CloseParenToken)}";
        }

        /// <summary>
        /// Converts an attribute argument list to Swift
        /// </summary>
        /// <param name="list">The list to convert</param>
        /// <returns>The converted Swift list</returns>
        [ParsesType(typeof(AttributeArgumentListSyntax))]
        public static string AttributeArgumentList(AttributeArgumentListSyntax list)
        {
            return
                $"{SyntaxTokenConvert(list.OpenParenToken)}{list.Arguments.ConvertSeparatedSyntaxList()}{SyntaxTokenConvert(list.CloseParenToken)}";
        }
    }
}