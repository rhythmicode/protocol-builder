using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProtocolBuilder.Converters
{
    partial class BuilderStatic
    {
        /// <summary>
        /// Converts to Swift
        /// </summary>
        [ParsesType(typeof(InitializerExpressionSyntax))]
        public static string InitializerExpression(InitializerExpressionSyntax expressionSyntax)
        {
            switch (Builder.Instance.Language)
            {
                case Languages.Swift:
                    //Dictionary
                    if (expressionSyntax.Expressions.All(a1 => a1 is InitializerExpressionSyntax && (a1 as InitializerExpressionSyntax)?.Expressions.Count == 2))
                    {
                        var items = (from a1 in expressionSyntax.Expressions.Cast<InitializerExpressionSyntax>()
                                     select $"{SyntaxNode(a1.Expressions[0])}: {SyntaxNode(a1.Expressions[1])}").ToList();
                        return $"[{string.Join(", ", items)}]";
                    }
                    else //List
                        return $"[{expressionSyntax.Expressions.ConvertSeparatedSyntaxList()}]"; ;
                case Languages.Kotlin:
                    //Dictionary
                    if (expressionSyntax.Expressions.All(a1 => a1 is InitializerExpressionSyntax && (a1 as InitializerExpressionSyntax)?.Expressions.Count == 2))
                    {
                        var items = (from a1 in expressionSyntax.Expressions.Cast<InitializerExpressionSyntax>()
                                     select $"{SyntaxNode(a1.Expressions[0])} to {SyntaxNode(a1.Expressions[1])}").ToList();
                        return $"hashMapOf({string.Join(", ", items)})";
                    }
                    else //List
                        return $"arrayListOf({expressionSyntax.Expressions.ConvertSeparatedSyntaxList()})"; ;
                default:
                    throw new ArgumentException();
            }
        }
    }
}