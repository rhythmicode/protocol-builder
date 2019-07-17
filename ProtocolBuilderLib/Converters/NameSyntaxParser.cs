using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System;

namespace ProtocolBuilder.Converters
{
    partial class BuilderStatic
    {
        /// <summary>
        /// Converts an IdentifierNameSyntax to Swift
        /// </summary>
        /// <param name="identifierName">The identifier to convert</param>
        /// <returns>The Swift identifier</returns>
        [ParsesType(typeof(IdentifierNameSyntax))]
        public static string IdentifierName(IdentifierNameSyntax identifierName)
        {
            //Looks for an ExportAttribute
            var symbol = Model.GetSymbolInfo(identifierName).Symbol;
            string nameToUse = null;

            if (symbol != null)
            {
                //Check for an [Export()] attribute
                var exportAttr = symbol.GetAttributes()
                    .FirstOrDefault(attr => attr.AttributeClass.Name.Contains("ExportAttribute"));
                if (exportAttr != null)
                {
                    nameToUse = exportAttr.ConstructorArguments[0].Value.ToString();
                }
            }

            return nameToUse ??
                   SyntaxTokenConvert(identifierName.Identifier)
                       .Replace(identifierName.Identifier.Text, Type(identifierName.Identifier.Text));
        }

        /// <summary>
        /// Converts a generic name to Swift
        /// </summary>
        /// <param name="name">The name to convert</param>
        /// <returns>The converted Swift code</returns>
        [ParsesType(typeof(GenericNameSyntax))]
        public static string GenericName(GenericNameSyntax name)
        {
            //TODO: replace the screwed up Action<>/Func<> conversions w/ DNSwift implementations
            switch (name.Identifier.Text)
            {
                case "Action":
                    //Action<string, int> converts to (String, Int) -> Void
                    return ": (" + SyntaxNode(name.TypeArgumentList) + ")";
                case "Func":
                    //Func<string, int, string> converts to (String, Int) -> String
                    var output = ": (";

                    //The last generic argument in Func<> is used as a return type
                    var allButLastArguments =
                        name.TypeArgumentList.Arguments.Take(name.TypeArgumentList.Arguments.Count - 1);

                    output += allButLastArguments.ConvertSyntaxNodes(", ");

                    return output + ") -> " + SyntaxNode(name.TypeArgumentList.Arguments.Last());
                case "Unwrapped":
                    return SyntaxNode(name.TypeArgumentList.Arguments.First()).TrimEnd('!') + "!";
                case "Optional":
                    return SyntaxNode(name.TypeArgumentList.Arguments.First()).TrimEnd('!') + "?";
                case "AmbiguousWrapping":
                    return SyntaxNode(name.TypeArgumentList.Arguments.First()).TrimEnd('!');
            }

            //Something<another, thing> converts to Something<another, thing> :D
            if (name.IsTypeList())
            {
                switch (Builder.Instance.Language)
                {
                    case Languages.Swift:
                        return "[" + SyntaxNode(name.TypeArgumentList).Trim('<', '>') + "]";
                    case Languages.Kotlin:
                        return "ArrayList<" + SyntaxNode(name.TypeArgumentList).Trim('<', '>') + ">";
                    case Languages.TypeScript:
                        return "Array<" + SyntaxNode(name.TypeArgumentList).Trim('<', '>') + ">";
                    case Languages.Php:
                        return SyntaxNode(name.TypeArgumentList).Trim('<', '>') + "[]";
                    default:
                        throw new ArgumentException();
                }
            }
            else if (name.IsTypeDictionary())
            {
                switch (Builder.Instance.Language)
                {
                    case Languages.Swift:
                        return "[" + SyntaxNode(name.TypeArgumentList).Trim('<', '>').Replace(",", ":") + "]";
                    case Languages.Kotlin:
                        return "HashMap<" + SyntaxNode(name.TypeArgumentList).Trim('<', '>') + ">";
                    case Languages.TypeScript:
                        return "Map<" + SyntaxNode(name.TypeArgumentList).Trim('<', '>') + ">";
                    case Languages.Php:
                        return "array";
                    default:
                        throw new ArgumentException();
                }
            }
            else
                return Type(name.Identifier.Text) + SyntaxNode(name.TypeArgumentList);
        }
    }
}