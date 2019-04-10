using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolBuilder
{
    public static class Extensions
    {
        public static bool IsInsideEnum(this SyntaxToken node)
        {
            return node.Parent?.IsInsideEnum() == true;
        }
        public static bool IsInsideEnum(this SyntaxNode node)
        {
            if (node == null) return false;
            if ((node as ClassDeclarationSyntax)?
                .AttributeLists
                .Any(a1 => a1
                    .ToString()
                    .ToLower()
                    .Contains("enumasstring")
                ) == true)
            {
                return true;
            }
            else
            {
                return IsInsideEnum(node.Parent);
            }
        }
    }
}
