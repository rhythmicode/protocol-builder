using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProtocolBuilder.Converters
{
    //TODO: Double check & add examples to all the expressions
    partial class BuilderStatic
    {
        /// <summary>
        /// Converts an arbitrary expression to Swift
        /// </summary>
        /// <param name="expression">The expression to convert</param>
        /// <returns>The converted Swift expression.</returns>
        [ParsesType(typeof(ExpressionSyntax))]
        public static string Expression(ExpressionSyntax expression)
        {
            return expression.ToString();
        }

        /// <summary>
        /// Converts an expression statement (with semicolon) to Swift
        /// </summary>
        /// <param name="statement">The statement to convert</param>
        /// <returns>The converted Swift statement</returns>
        [ParsesType(typeof(ExpressionStatementSyntax))]
        public static string ExpressionStatement(ExpressionStatementSyntax statement)
        {
            return statement.ConvertTo(SyntaxNode(statement.Expression).TrimStart() +
                                       Semicolon(statement.SemicolonToken));
        }

        [ParsesType(typeof(AssignmentExpressionSyntax))]
        public static string AssignmentExpressionSyntax(AssignmentExpressionSyntax statement)
        {
            return
                $"{SyntaxNode(statement.Left)}{SyntaxTokenConvert(statement.OperatorToken)}{SyntaxNode(statement.Right)}";
        }

        [ParsesType(typeof(CastExpressionSyntax))]
        public static string CastExpressionSyntax(CastExpressionSyntax statement)
        {
            switch (Builder.Instance.Language)
            {
                case Languages.Kotlin:
                    return $"({SyntaxNode(statement.Expression)}).to{SyntaxNode(statement.Type)}()";
                case Languages.TypeScript:
                    return $"{SyntaxNode(statement.Expression)} as {SyntaxNode(statement.Type)}";
                case Languages.Swift:
                default:
                    return $"{SyntaxNode(statement.Type)}({SyntaxNode(statement.Expression)})";
            }
        }

        /// <summary>
        /// Converts a postfix unary expression to Swift
        /// </summary>
        /// <param name="expression">The expression to convert</param>
        /// <returns>The converted Swift expression</returns>
        [ParsesType(typeof(PostfixUnaryExpressionSyntax))]
        public static string PostfixUnaryExpression(PostfixUnaryExpressionSyntax expression)
        {
            return SyntaxNode(expression.Operand) + SyntaxTokenConvert(expression.OperatorToken);
        }

        /// <summary>
        /// Converts a prefix unary expression to Swift
        /// </summary>
        /// <param name="expression">The expression to convert</param>
        /// <returns>The converted Swift expression</returns>
        [ParsesType(typeof(PrefixUnaryExpressionSyntax))]
        public static string PrefixUnaryExpression(PrefixUnaryExpressionSyntax expression)
        {
            return SyntaxTokenConvert(expression.OperatorToken) + SyntaxNode(expression.Operand);
        }

        /// <summary>
        /// Converts a member access expression to Swift
        /// </summary>
        /// <param name="expression">The expression to convert</param>
        /// <returns>The converted Swift expression</returns>
        [ParsesType(typeof(MemberAccessExpressionSyntax))]
        public static string MemberAccessExpression(MemberAccessExpressionSyntax expression)
        {
            var result = SyntaxNode(expression.Expression) + SyntaxTokenConvert(expression.OperatorToken) +
                         SyntaxNode(expression.Name);
            switch (Builder.Instance.Language)
            {
                case Languages.Kotlin:
                    result = result.Replace("Math.Floor", "kotlin.math.floor");
                    result = result.Replace("Math.Pow", "Math.pow");
                    result = result.Replace("Math.Max", "kotlin.math.max");
                    result = result.Replace("Math.Min", "kotlin.math.min");
                    result = result.Replace("Math.Log", "kotlin.math.log2");
                    result = result.Replace("Math.Round", "kotlin.math.round");
                    result = result.Replace("Math.PI", "kotlin.math.PI");
                    result = result.Replace("Math.Sin", "kotlin.math.sin");
                    result = result.Replace("Math.Cos", "kotlin.math.cos");
                    result = result.Replace("Math.Atan2", "kotlin.math.atan2");
                    result = result.Replace("Math.Sqrt", "kotlin.math.sqrt");
                    result = result.Replace("Math.Tan", "kotlin.math.tan");
                    result = result.Replace("Math.Asin", "kotlin.math.asin");
                    result = result.Replace("Math.Abs", "kotlin.math.abs");
                    break;
                case Languages.Swift:
                default:
                    result = result.Replace("Math.Floor", "floor");
                    result = result.Replace("Math.Pow", "pow");
                    result = result.Replace("Math.Max", "max");
                    result = result.Replace("Math.Min", "min");
                    result = result.Replace("Math.Log", "log");
                    result = result.Replace("Math.Round", "round");
                    result = result.Replace("Math.PI", "Double.pi");
                    result = result.Replace("Math.Sin", "sin");
                    result = result.Replace("Math.Cos", "cos");
                    result = result.Replace("Math.Atan2", "atan2");
                    result = result.Replace("Math.Sqrt", "sqrt");
                    result = result.Replace("Math.Tan", "tan");
                    result = result.Replace("Math.Asin", "asin");
                    result = result.Replace("Math.Abs", "abs");
                    break;
            }

            return result;
        }

        /// <summary>
        /// Converts an invocation expression to Swift
        /// </summary>
        /// <example>something.Method("arg")</example>
        /// <param name="invocation">The invocation expression to convert</param>
        /// <returns>The converted Swift code</returns>
        [ParsesType(typeof(InvocationExpressionSyntax))]
        public static string InvocationExpression(InvocationExpressionSyntax invocation)
        {
            return SyntaxNode(invocation.Expression) + SyntaxNode(invocation.ArgumentList);
        }

        /// <summary>
        /// Converts an object creation expression to Swift
        /// </summary>
        /// <example>new Something()</example>
        /// <param name="expression">The expression to convert</param>
        /// <returns>The converted Swift object creation expression</returns>
        [ParsesType(typeof(ObjectCreationExpressionSyntax))]
        public static string ObjectCreationExpression(ObjectCreationExpressionSyntax expression)
        {
            if (expression.ArgumentList != null)
            {
                //Name all the arguments, since Swift usually requires named arguments when you create new objects.
                //Thanks! http://stackoverflow.com/questions/24174602/get-constructor-declaration-from-objectcreationexpressionsyntax-with-roslyn/24191494#24191494
                var symbol = Model.GetSymbolInfo(expression).Symbol as IMethodSymbol;

                var namedArgumentsList = new SeparatedSyntaxList<ArgumentSyntax>();

                for (var i = 0; i < expression.ArgumentList.Arguments.Count; i++)
                {
                    var oldArgumentSyntax = expression.ArgumentList.Arguments[i];
                    var parameterName = symbol.Parameters[i].Name;

                    var nameColonSyntax = SyntaxFactory
                        .NameColon(SyntaxFactory.IdentifierName(parameterName))
                        .WithTrailingTrivia(SyntaxFactory.Whitespace(" "));

                    var namedArgumentSyntax = SyntaxFactory.Argument(nameColonSyntax, oldArgumentSyntax.RefOrOutKeyword,
                        oldArgumentSyntax.Expression);

                    namedArgumentsList = namedArgumentsList.Add(namedArgumentSyntax);
                }

                //NOTE: this takes out expression.parent and everything, and probably screws with SyntaxModel stuff to
                return SyntaxTokenConvert(expression.NewKeyword) + SyntaxNode(expression.Type) + SyntaxNode(SyntaxFactory.ArgumentList(namedArgumentsList));
            }
            else
            {
                return SyntaxNode(expression.Initializer);
            }
        }

        /// <summary>
        /// Converts a binary expression to Swift
        /// </summary>
        /// <example>something (+/-/+=/etc) something_else</example>
        /// <param name="expression">The expression to convert</param>
        /// <returns>The converted Swift expression</returns>
        [ParsesType(typeof(BinaryExpressionSyntax))]
        public static string BinaryExpression(BinaryExpressionSyntax expression)
        {
            if (Builder.Instance.Language == Languages.Swift && expression.OperatorToken.IsKind(SyntaxKind.PercentToken))
            {
                return
                    $"{SyntaxNode(expression.Left).TrimEnd()}.truncatingRemainder(dividingBy: {SyntaxNode(expression.Right)})";
            }

            return
                $"{SyntaxNode(expression.Left)}{SyntaxTokenConvert(expression.OperatorToken)}{SyntaxNode(expression.Right)}";
        }

        [ParsesType(typeof(ParenthesizedExpressionSyntax))]
        public static string ParenthesizedExpression(ParenthesizedExpressionSyntax expression)
        {
            return
                $"{SyntaxTokenConvert(expression.OpenParenToken)}{SyntaxNode(expression.Expression)}{SyntaxTokenConvert(expression.CloseParenToken)}";
        }

        /// <summary>
        /// Converts a "base" expression to Swift
        /// </summary>
        /// <example>base</example>
        /// <param name="expression">The expression to convet</param>
        /// <returns>"super"</returns>
        [ParsesType(typeof(BaseExpressionSyntax))]
        public static string BaseExpression(BaseExpressionSyntax expression)
        {
            return "super";
        }

        /// <summary>
        /// Converts a C# literal expression to Swift
        /// </summary>
        /// <example>"hello!"</example>
        /// <example>123</example>
        /// <param name="expression">The literal expression to convert</param>
        /// <returns>The converted Swift literal</returns>
        [ParsesType(typeof(LiteralExpressionSyntax))]
        public static string LiteralExpression(LiteralExpressionSyntax expression)
        {
            var r = "";
            switch (expression.Kind())
            {
                //Swift doesn't use the same 'c' character literal syntax, instead you create a String and type annotate it as a Character
                case SyntaxKind.CharacterLiteralExpression:
                    //this is sketch, probably shouldn't use char literals o.o
                    r = '"' + expression.Token.ValueText.Replace("\\'", "'").Replace("\"", "\\\"") + '"';
                    break;
                case SyntaxKind.NullKeyword:
                case SyntaxKind.NullLiteralExpression:
                    switch (Builder.Instance.Language)
                    {
                        case Languages.Swift:
                            r = "nil";
                            break;
                        case Languages.Kotlin:
                            r = "null";
                            break;
                        case Languages.TypeScript:
                            r = "null";
                            break;
                    }

                    break;
                default:
                    r = expression.ToString();
                    break;
            }

            r = expression.ToFullString().Replace(expression.ToString(), r);

            return r;
        }

        /// <summary>
        /// Converts an implicit array to Swift
        /// </summary>
        /// <example>new[] { 1, 2, 3 }</example>
        /// <param name="array">The implicit array to convert</param>
        /// <returns>The converted Swift array</returns>
        [ParsesType(typeof(ImplicitArrayCreationExpressionSyntax))]
        public static string ImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax array)
        {
            var output = "[ ";
            output += array.Initializer.Expressions.ConvertSeparatedSyntaxList();
            return output + " ]";
        }

        /// <summary>
        /// Converts an explicit array to Swift
        /// </summary>
        /// <example>new string[] { 1, 2, 3 }</example>
        /// <param name="node">The explicit array to convert</param>
        /// <returns>The converted Swift array</returns>
        [ParsesType(typeof(ArrayCreationExpressionSyntax))]
        public static string ArrayCreationExpression(ArrayCreationExpressionSyntax node)
        {
            var output = "[ ";
            output += node.Initializer.Expressions.ConvertSeparatedSyntaxList();
            return output + " ]";
        }
    }
}