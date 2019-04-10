using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProtocolBuilder.Converters
{
    partial class BuilderStatic
    {
        //private static List<TypeSyntax> _TypeSyntaxNullable = new List<TypeSyntax>();

        /// <summary>
        /// Returns the Swift equivilant for a C# type
        /// </summary>
        /// <param name="typeName">The C# type's identifier as a string</param>
        /// <param name="implyUnwrapped">If true, unwraps the type with an ! at the end</param>
        /// <returns>The Swift equivilant type as a string</returns>
        private static string Type(string typeName, bool implyUnwrapped = false)//, bool isNullable = false)
        {
            var result = typeName;
            switch (result)
            {
                case "DateTime":
                    result = "string";
                    break;
                case "string":
                    switch (Builder.Instance.Language)
                    {
                        case Languages.Php:
                        case Languages.TypeScript:
                            result = "string";
                            break;
                        default:
                            result = "String";
                            break;
                    }
                    break;
                case "char":
                    result = "Character";
                    break;
                case "int":
                    switch (Builder.Instance.Language)
                    {
                        case Languages.TypeScript:
                            result = "number";
                            break;
                        case Languages.Php:
                            result = "int";
                            break;
                        default:
                            result = "Int";
                            break;
                    }
                    break;
                case "long":
                    switch (Builder.Instance.Language)
                    {
                        case Languages.Swift:
                            result = "Int64";
                            break;
                        case Languages.Kotlin:
                            result = "Long";
                            break;
                        case Languages.TypeScript:
                            result = "number";
                            break;
                        case Languages.Php:
                            result = "int";
                            break;
                    }
                    break;
                case "double":
                    switch (Builder.Instance.Language)
                    {
                        case Languages.TypeScript:
                            result = "number";
                            break;
                        case Languages.Php:
                            result = "float";
                            break;
                        default:
                            result = "Double";
                            break;
                    }
                    break;
                case "float":
                    switch (Builder.Instance.Language)
                    {
                        case Languages.TypeScript:
                            result = "number";
                            break;
                        case Languages.Php:
                            result = "float";
                            break;
                        default:
                            result = "Double";
                            break;
                    }
                    break;
                case "void":
                    switch (Builder.Instance.Language)
                    {
                        case Languages.TypeScript:
                            result = "void";
                            break;
                        case Languages.Php:
                            result = "";
                            break;
                        case Languages.Swift:
                            result = "Void";
                            break;
                        case Languages.Kotlin:
                            result = "Unit";
                            break;
                    }
                    break;
                case "bool":
                    switch (Builder.Instance.Language)
                    {
                        case Languages.Php:
                            result = "bool";
                            break;
                        case Languages.TypeScript:
                            result = "boolean";
                            break;
                        case Languages.Swift:
                            result = "Bool";
                            break;
                        case Languages.Kotlin:
                            result = "Boolean";
                            break;
                    }
                    break;
            }
            //result = $"{result}{(isNullable ? "?" : "")}";
            return result;
        }

        /// <summary>
        /// Converts an array type to Swift
        /// </summary>
        /// <param name="array">The array type to convert</param>
        /// <returns>The converted Swift type</returns>
        [ParsesType(typeof(ArrayTypeSyntax))]
        public static string ArrayType(ArrayTypeSyntax array)
        {
            return "[" + SyntaxNode(array.ElementType) + "]"; //TODO: rankspecifiers
        }

        /// <summary>
        /// Converts a PredefinedType to Swift
        /// </summary>
        /// <param name="type">The type to convert</param>
        /// <returns>The converted Swift type</returns>
        [ParsesType(typeof(PredefinedTypeSyntax))]
        public static string PredefinedType(PredefinedTypeSyntax type)
        {
            return Type(type.Keyword.Text);//, isNullable: _TypeSyntaxNullable.Contains(type));
        }

        /// <summary>
        /// Converts a nullable type to Swift
        /// </summary>
        /// <param name="typedSyntax">The type to convert</param>
        /// <returns>The converted Swift type</returns>
        [ParsesType(typeof(NullableTypeSyntax))]
        public static string NullableType(NullableTypeSyntax typedSyntax)
        {
            //_TypeSyntaxNullable.Add(typedSyntax.ElementType);
            return SyntaxNode(typedSyntax.ElementType);
        }

        /// <summary>
        /// Converts a simple base type to Swift
        /// </summary>
        /// <param name="typedSyntax">The type to convert</param>
        /// <returns>The converted Swift type</returns>
        [ParsesType(typeof(SimpleBaseTypeSyntax))]
        public static string SimpleBaseType(SimpleBaseTypeSyntax typedSyntax)
        {
            return SyntaxNode(typedSyntax.Type);
        }
    }
}
