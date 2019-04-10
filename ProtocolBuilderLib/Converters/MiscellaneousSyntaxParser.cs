using Microsoft.CodeAnalysis;

namespace ProtocolBuilder.Converters
{
    partial class BuilderStatic
    {
        /// <summary>
        /// Adds a semicolon to the end of a line if there is one
        /// </summary>
        /// <param name="semicolonToken">The semicolon</param>
        /// <returns>The semicolon and newline</returns>
        public static string Semicolon(SyntaxToken semicolonToken)
        {
            switch (Builder.Instance.Language)
            {
                case Languages.TypeScript:
                    return ";";
                case Languages.Php:
                    return ";";
                default:
                    return "";
            }
        }
    }
}
