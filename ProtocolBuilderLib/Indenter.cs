using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProtocolBuilder
{
    internal static class Indenter
    {
        /// <summary>
        /// Indents a Swift file with everything inside { }s indented one level.
        /// </summary>
        /// <param name="swift">The Swift code to indent</param>
        /// <param name="newLine">The newline character(s) to use. Defaults to Environment.NewLine</param>
        /// <param name="indentWith">What character(s) to use to indent. Defaults to four spaces.</param>
        /// <returns></returns>
        public static string IndentDocument(string swift, string newLine = null, string indentWith = "    ")
        {
            if (newLine == null)
            {
                newLine = Environment.NewLine;
            }

            var output = "";
            var lines = swift.Split(new[] {"\n", "\r\n", "\r", Environment.NewLine, newLine}, StringSplitOptions.None);
            var currIndent = "";
            foreach (var line in lines)
            {
                if (line.Contains("}") && !line.Contains("{"))
                {
                    currIndent = currIndent.Substring(indentWith.Length);
                }

                output += currIndent + line + newLine;

                if (line.Contains("{") && !line.Contains("}"))
                {
                    currIndent += indentWith;
                }
            }

            return output.Trim() + newLine;
        }

        public static string FixIndent(string source)
        {
            var indent = new string(' ', 4);
            var lines = new List<string>();
            var isAllIndented = true;
            using (var stringR = new StringReader(source))
            {
                while (true)
                {
                    var line = stringR.ReadLine();
                    if (line == null)
                        break;
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith(indent))
                    {
                        isAllIndented = false;
                        break;
                    }

                    lines.Add(line);
                }
            }

            if (!isAllIndented)
                return source;
            return string.Join("\n", lines.Select(s => string.IsNullOrWhiteSpace(s) ? s : s.Substring(indent.Length)));
        }
    }
}