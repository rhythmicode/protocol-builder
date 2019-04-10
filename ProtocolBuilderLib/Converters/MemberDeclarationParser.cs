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
        /// Parses an attribute
        /// </summary>
        /// <param name="attributeLists">The list of attributes to parse</param>
        /// <param name="separator">The separator to use between attributes</param>
        /// <returns>A tuple where the first value is the Swift attributes and the second contains the value of an ExportAttribute</returns>
        private static Tuple<string, string> ParseAttributes(SyntaxList<AttributeListSyntax> attributeLists,
            string separator = " ")
        {
            if (!attributeLists.Any()) return new Tuple<string, string>("", null);

            var output = "";
            string exportAs = null;

            foreach (var attribute in attributeLists.SelectMany(attrList => attrList.Attributes))
            {
                if (IsProtocolBuilderAttribute(attribute, "ExportAttribute"))
                {
                    exportAs = SyntaxNode(attribute.ArgumentList.Arguments[0].Expression).Trim().Trim('"');
                    continue;
                }

                var attOutput = default(string);
                if (attribute.Name.ToString().ToLower().Contains("outputas"))
                {
                    var attLanguage = (Languages)Enum.Parse(typeof(Languages),
                        attribute.ArgumentList.Arguments[0].Expression.ToString().Split('.').Last());
                    if (attLanguage == Builder.Instance.Language)
                        attOutput = attribute.ArgumentList.Arguments[1].Expression.ToString().Trim('"');
                    if (string.IsNullOrWhiteSpace(attOutput))
                        continue;
                }

                if (string.IsNullOrWhiteSpace(attOutput))
                {
                    var syntaxNodeResult = SyntaxNode(attribute);
                    if (!string.IsNullOrWhiteSpace(syntaxNodeResult))
                        attOutput = SyntaxNode(attribute);
                }

                if (!string.IsNullOrWhiteSpace(attOutput))
                    output += attOutput + separator;
            }

            return new Tuple<string, string>(output, exportAs);
        }

        /// <summary>
        /// Converts member declaration modifiers to Swift
        /// NOTE that internal will be converted to public, as Swift doesn't have an internal modifier
        /// </summary>
        /// <example>public readonly</example>
        /// <param name="modifiers">The modifiers to convert</param>
        /// <returns>The converted Swift modifiers</returns>
        private static string ParseModifiers(SyntaxTokenList modifiers)
        {
            var result = string.Join("", modifiers.Select(modifier =>
            {
                var resultToken = SyntaxTokenConvert(modifier);
                switch (Builder.Instance.Language)
                {
                    case Languages.Kotlin:
                        resultToken = resultToken.Replace("static", "");
                        resultToken = resultToken.Replace("internal", "");
                        resultToken = resultToken.Replace("public", "");
                        break;
                    case Languages.Swift:
                    default:
                        resultToken = resultToken.Replace("internal", "public");
                        break;
                }

                return resultToken;
            })).Trim();
            if (result.Length > 0)
                result += " ";
            return result;
        }

        /// <summary>
        /// Converts a class declaration to Swift
        /// </summary>
        /// <example>public class SomeClass { }</example>
        /// <param name="declaration">The class to convert</param>
        /// <returns>The converted Swift class</returns>
        [ParsesType(typeof(ClassDeclarationSyntax))]
        public static string ClassDeclaration(ClassDeclarationSyntax declaration)
        {
            var parsedAttributes =
                ParseAttributes(declaration.AttributeLists, NewLine + declaration.GetLeadingTrivia().ToFullString());
            var nameToUse = parsedAttributes.Item2;

            var output = declaration.GetLeadingTrivia().ToFullString();
            var isStatic = declaration.Modifiers.Any(a1 => a1.Text.ToLower() == "static") == true;
            if (declaration.AttributeLists.Any(a1 => a1.ToString().ToLower().Contains("enumasstring")))
            {
                output +=
                    Builder.Instance.LanguageConvertEnum()
                    + " "
                    + (nameToUse ?? SyntaxTokenConvert(declaration.Identifier).TrimEnd())
                    + Builder.Instance.LanguageConvertEnumPostfix("String");
            }
            else
            {
                output += parsedAttributes.Item1;
                output +=
                    Builder.Instance.LanguageConvertClass(isStatic)
                    + (nameToUse ?? SyntaxTokenConvert(declaration.Identifier).TrimEnd())
                    + Builder.Instance.LanguageConvertClassPostfix();
            }

            //parse the base type, if there is one
            if (declaration.BaseList != null)
            {
                var baseType = declaration.BaseList.Types.OfType<IdentifierNameSyntax>().FirstOrDefault();
                var typeGeneric = declaration.BaseList.Types.OfType<SimpleBaseTypeSyntax>().FirstOrDefault();

                switch (Builder.Instance.Language)
                {
                    case Languages.Swift:
                        if (baseType != null)
                            output += ", " + SyntaxNode(baseType);
                        else if (typeGeneric != null)
                            output += ", " + SyntaxNode(typeGeneric);
                        else
                            output += ", " + declaration.BaseList.ToString().Trim(' ', ':');
                        break;
                    case Languages.Kotlin:
                        if (baseType != null)
                            output += $": {SyntaxNode(baseType).TrimEnd()}()";
                        else if (typeGeneric != null)
                            output += $": {SyntaxNode(typeGeneric).TrimEnd()}()";
                        else
                            output += $": {declaration.BaseList.ToString().Trim(' ', ':')}()";
                        break;
                    case Languages.Php:
                        if (baseType != null)
                            output += $" extends {SyntaxNode(baseType).TrimEnd()}";
                        else if (typeGeneric != null)
                            output += $" extends {SyntaxNode(typeGeneric).TrimEnd()}";
                        else
                            output += $" extends {declaration.BaseList.ToString().Trim(' ', ':')}";
                        break;
                }
            }

            output +=
                " "
                + SyntaxTokenConvert(declaration.OpenBraceToken).TrimStart()
                + declaration.Members.ConvertSyntaxList()
                + SyntaxTokenConvert(declaration.CloseBraceToken);
            return output;
        }

        /// <summary>
        /// Converts a method to Swift
        /// </summary>
        /// <example>public void Something() { }</example>
        /// <param name="method">The method to convert</param>
        /// <returns>The converted Swift method</returns>
        [ParsesType(typeof(MethodDeclarationSyntax))]
        public static string MethodDeclaration(MethodDeclarationSyntax method)
        {
            var parsedAttributes = ParseAttributes(method.AttributeLists);

            var output = parsedAttributes.Item1;
            var nameToUse = parsedAttributes.Item2;

            output += ParseModifiers(method.Modifiers);

            switch (Builder.Instance.Language)
            {
                case Languages.Kotlin:
                    output += "fun ";
                    break;
                case Languages.Swift:
                default:
                    output += "func ";
                    break;
            }

            output += (nameToUse ?? SyntaxTokenConvert(method.Identifier));

            if (method.TypeParameterList != null) //public string Something<T>
            {
                output += SyntaxNode(method.TypeParameterList);
            }

            output += SyntaxNode(method.ParameterList).TrimEnd();

            var hasReturnType = method.ReturnType != null;
            var returnTypeResult = "";
            if (hasReturnType)
            {
                returnTypeResult = SyntaxNode(method.ReturnType).TrimEnd();
                switch (Builder.Instance.Language)
                {
                    case Languages.Kotlin:
                        hasReturnType = returnTypeResult != "Unit";
                        break;
                    case Languages.Swift:
                    default:
                        break;
                }
            }

            if (hasReturnType)
            {
                switch (Builder.Instance.Language)
                {
                    case Languages.Kotlin:
                        output += ": ";
                        break;
                    case Languages.Swift:
                    default:
                        output += " -> ";
                        break;
                }

                output += returnTypeResult;
            }

            output +=
                " "
                + SyntaxNode(method.Body).TrimStart();
            return method.ConvertTo(output.Trim());
        }

        /// <summary>
        /// Converts an enum member to Swift
        /// </summary>
        /// <example>name = 1</example>
        /// <param name="declaration">The declaration to convert</param>
        /// <returns>The converted Swift declaration</returns>
        [ParsesType(typeof(EnumMemberDeclarationSyntax))]
        public static string EnumMemberDeclaration(EnumMemberDeclarationSyntax declaration)
        {
            var output = "";
            switch (Builder.Instance.Language)
            {
                case Languages.Swift:
                    output = declaration.GetLeadingTrivia().ToFullString()
                             + "case "
                             + SyntaxTokenConvert(declaration.Identifier).TrimStart();
                    if (declaration.EqualsValue != null)
                    {
                        output += SyntaxNode(declaration.EqualsValue);
                    }
                    break;
                case Languages.Kotlin:
                    output = SyntaxTokenConvert(declaration.Identifier).TrimEnd();
                    if (declaration.EqualsValue != null)
                    {
                        output += $"({SyntaxNode(declaration.EqualsValue.Value)})";
                    }
                    break;
                case Languages.TypeScript:
                    output = SyntaxTokenConvert(declaration.Identifier).TrimEnd();
                    if (declaration.EqualsValue != null)
                    {
                        output += $" = {SyntaxNode(declaration.EqualsValue.Value)}";
                    }
                    break;
                case Languages.Php:
                    output = $"{declaration.Identifier.LeadingTrivia.ToFullString()}public const {declaration.Identifier.ToString().TrimEnd()}";
                    if (declaration.EqualsValue != null)
                    {
                        output += $" = {SyntaxNode(declaration.EqualsValue.Value).TrimEnd()};";
                    }
                    break;
            }

            return output;
        }

        /// <summary>
        /// Converts an enum to Swift
        /// </summary>
        /// <example>public enum thing { }</example>
        /// <param name="declaration">The enum declaration to convert</param>
        /// <returns>The converted Swift enum</returns>
        [ParsesType(typeof(EnumDeclarationSyntax))]
        public static string EnumDeclaration(EnumDeclarationSyntax declaration)
        {
            var parsedAttributes = ParseAttributes(declaration.AttributeLists);

            var output = declaration.GetLeadingTrivia().ToFullString();
            output += parsedAttributes.Item1;
            var nameToUse = parsedAttributes.Item2;

            output +=
                Builder.Instance.LanguageConvertEnum()
                + " "
                + (nameToUse ?? SyntaxTokenConvert(declaration.Identifier).TrimEnd());

            //Get the value of the enum
            foreach (var decl in declaration.ChildNodes().OfType<EnumMemberDeclarationSyntax>()
                .Where(decl => decl.EqualsValue != null).Select(decl => decl.EqualsValue.Value))
            {
                if (decl.IsKind(SyntaxKind.StringLiteralExpression))
                {
                    output += Builder.Instance.LanguageConvertEnumPostfix("String");
                }
                else if (decl.IsKind(SyntaxKind.CharacterLiteralExpression))
                {
                    output += Builder.Instance.LanguageConvertEnumPostfix("Character");
                }
                else if (decl.IsKind(SyntaxKind.NumericLiteralExpression))
                {
                    output += Builder.Instance.LanguageConvertEnumPostfix("Int");
                }
                else
                {
                    continue;
                }

                break;
            }

            output +=
                " "
                + SyntaxTokenConvert(declaration.OpenBraceToken).TrimStart()
                + declaration.Members.ConvertSeparatedSyntaxList(
                    separatorForced: Builder.Instance.LanguageEnumMemberSeparator()
                ).TrimEnd()
                + NewLine
                + SyntaxTokenConvert(declaration.CloseBraceToken);
            return output;
        }

        [ParsesType(typeof(AccessorDeclarationSyntax))]
        public static string AccessorDeclaration(AccessorDeclarationSyntax node)
        {
            //TODO: implement this
            return SyntaxNode(node.Body);
        }

        /// <summary>
        /// Converts a field declaration to Swift
        /// </summary>
        /// <param name="declaration">The declarationt to convert</param>
        /// <returns>The converted Swift code</returns>
        [ParsesType(typeof(FieldDeclarationSyntax))]
        public static string FieldDeclaration(FieldDeclarationSyntax declaration)
        {
            var parsedAttributes = ParseAttributes(declaration.AttributeLists);

            var output = parsedAttributes.Item1;
            var nameToUse = parsedAttributes.Item2;

            return declaration.ConvertTo(
                output +
                SyntaxNode(declaration.Declaration) +
                (declaration.IsInsideEnum() ? "" : Semicolon(declaration.SemicolonToken))
            );
        }

        /// <summary>
        /// Converts a property declaration to Swift
        /// </summary>
        /// <param name="declaration">The declaration to convert</param>
        /// <returns>The converted Swift code</returns>
        [ParsesType(typeof(PropertyDeclarationSyntax))]
        public static string PropertyDeclaration(PropertyDeclarationSyntax declaration)
        {
            var parsedAttributes = ParseAttributes(declaration.AttributeLists);

            var output = parsedAttributes.Item1;
            var nameToUse = parsedAttributes.Item2;

            return declaration.ConvertTo(
                output +
                Builder.Instance.LanguageDeclaration(declaration.IsInsideEnum(), false, false, false, declaration.Identifier, declaration.Type, declaration.AttributeLists, declaration.Initializer, declaration.SemicolonToken)
            );
        }

        /// <summary>
        /// Converts a constructor to Swift
        /// </summary>
        /// <param name="constructor">The constructor to convert</param>
        /// <returns>The converted Swift constructor</returns>
        [ParsesType(typeof(ConstructorDeclarationSyntax))]
        public static string ConstructorDeclaration(ConstructorDeclarationSyntax constructor)
        {
            var parsedAttributes = ParseAttributes(constructor.AttributeLists);

            var output = parsedAttributes.Item1;

            return output + "init" + SyntaxNode(constructor.ParameterList) + " " + Block(constructor.Body);
        }

        /// <summary>
        /// Converts a destructor to Swift
        /// </summary>
        /// <param name="destructor">The destructor to convert</param>
        /// <returns>The converted Swift destructor</returns>
        [ParsesType(typeof(DestructorDeclarationSyntax))]
        public static string DestructorDeclaration(DestructorDeclarationSyntax destructor)
        {
            var parsedAttributes = ParseAttributes(destructor.AttributeLists);

            var output = parsedAttributes.Item1;

            return output + "deinit " + Block(destructor.Body);
        }
    }
}