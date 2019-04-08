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
            if (Builder.Instance.Language == Languages.TypeScript)
                return ";";
            else
                return "";
        }
    }
}
