using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using ProtocolBuilder;
using ProtocolBuilder.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProtocolBuilder
{
    public class Builder
    {
        public static Builder Instance { get; set; }

        /// <summary>
        /// Target language
        /// </summary>
        public Languages Language { get; set; } = Languages.Swift;

        /// <summary>
        /// Path to the file to parse
        /// </summary>
        public string InputPath { get; set; }

        /// <summary>
        /// Boolean indicating whether to indent the output
        /// </summary>
        public bool Indent { get; set; }

        /// <summary>
        /// Path to the output .swift file
        /// </summary>
        public string OutputPath { get; set; }

        public Builder(string[] args)
        {
            Indent = false;

            string namedArgument = null;
            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (arg.StartsWith("-"))
                {
                    namedArgument = arg.ToLower().Trim('-');
                    if (namedArgument == "no-indent")
                    {
                        Indent = false;
                    }
                    continue;
                }

                if (namedArgument != null)
                {
                    switch (namedArgument)
                    {
                        case "input":
                        case "i":
                            InputPath = arg;
                            break;
                        //case "solution":
                        //case "sln":
                        //case "s":
                        //SlnPath = arg;
                        //break;
                        case "output":
                        case "o":
                            OutputPath = arg;
                            break;
                        case "language":
                        case "l":
                            switch (arg)
                            {
                                case "php":
                                    Language = Languages.Php;
                                    break;
                                case "kotlin":
                                    Language = Languages.Kotlin;
                                    break;
                                case "typescript":
                                    Language = Languages.TypeScript;
                                    break;
                                case "swift":
                                    Language = Languages.Swift;
                                    break;
                                default:
                                    throw new ArgumentException("Language parameter is not correct/supported.");
                            }
                            break;
                    }
                    namedArgument = null;
                    continue;
                }

                switch (i)
                {
                    case 0:
                        InputPath = arg;
                        break;
                    case 1:
                        OutputPath = arg;
                        break;
                        //case 2:
                        //SlnPath = arg;
                        //break;
                }
            }

            Instance = this;
        }

        public void Process()
        {
            var toProcess = new List<FileInfo>();
            if (File.Exists(InputPath))
                toProcess.Add(new FileInfo(InputPath));
            else
            {
                OutputPath = new DirectoryInfo(OutputPath).FullName;
                InputPath = new DirectoryInfo(InputPath).FullName;
                toProcess.AddRange(DirectoryCrawl(InputPath, "*.cs"));
            }

            var existingFiles = DirectoryCrawl(OutputPath, $"*{LanguageFileExtension()}");

            foreach (var feFile in toProcess)
            {
                var parsed = ParseFile(feFile.FullName, Indent);
                if (string.IsNullOrWhiteSpace(parsed.content))
                    continue;

                var outPath = "";
                if (string.IsNullOrWhiteSpace(parsed.saveRelativeDir))
                {
                    outPath =
                        $"{OutputPath}/{feFile.FullName.Replace(InputPath, "").Replace(".cs", LanguageFileExtension())}";
                }
                else
                {
                    outPath = Path.Combine(OutputPath, parsed.saveRelativeDir,
                        $"{Path.GetFileNameWithoutExtension(feFile.Name)}{LanguageFileExtension()}");
                }

                var fileOut = new FileInfo(outPath);
                if (!fileOut.Directory.Exists)
                    fileOut.Directory.Create();

                using (var writer = new StreamWriter(fileOut.FullName))
                {
                    writer.Write(parsed.content);
                    writer.Flush();
                }

                var existingMatch = existingFiles.FirstOrDefault(a => a.FullName == fileOut.FullName);
                if (existingMatch != null)
                    existingFiles.Remove(existingMatch);
            }

            foreach (var existingFileWithoutMatch in existingFiles)
            {
                existingFileWithoutMatch.Delete();
            }
            Console.WriteLine($"Cleaned Up: {existingFiles.Count}");

            Console.WriteLine("Done.");
        }

        private static List<FileInfo> DirectoryCrawl(string dirPath, string searchPattern)
        {
            var result = new List<FileInfo>();
            Queue<DirectoryInfo> dirs1 = new Queue<DirectoryInfo>();
            dirs1.Enqueue(new DirectoryInfo(dirPath));
            while (dirs1.Count > 0)
            {
                var dir1 = dirs1.Dequeue();
                if (!dir1.Exists)
                    continue;
                foreach (var fe1 in dir1.GetDirectories())
                {
                    dirs1.Enqueue(fe1);
                }

                foreach (var feFile in dir1.GetFiles(searchPattern))
                {
                    if (!result.Any(a1 => a1.FullName == feFile.FullName))
                        result.Add(feFile);
                }
            }
            return result;
        }

        private static Document GetDocumentFromSolution(string solutionPath, string documentPath)
        {
            var workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += (s1, e1) => { Console.WriteLine(e1.Diagnostic.ToString()); };
            var solution = workspace.OpenSolutionAsync(solutionPath).Result;
            var docsAll = solution.Projects.SelectMany(project => project.Documents).ToList();

            //AnalyzerManager manager = new AnalyzerManager(solutionPath);
            //manager.Projects.SelectMany(a1 =>
            //{
            //    var ws1 = a1.Value.GetWorkspace();
            //    ws1.get
            //});

            return docsAll.FirstOrDefault(document => document.FilePath.EndsWith(documentPath));
        }

        static string GetImportsFromTrivia(SyntaxTriviaList triviaList)
        {
            return triviaList
                .Where(trivia =>
                    trivia.IsKind(SyntaxKind.MultiLineCommentTrivia) ||
                    trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
                .Select(trivia => trivia.ToString().TrimStart('/', '*').Trim())
                .Where(comment => comment.StartsWith("import"))
                .Aggregate("", (current, comment) => current + (comment + BuilderStatic.NewLine));
        }

        //static List<string> _Pathes_StartLineAdded = new List<string>();
        (string saveRelativeDir, string content) ParseFile(string path, bool doIndent)
        {
            Console.WriteLine("Parsing file " + path);

            string output = "";

            //var doc = GetDocumentFromSolution(solutionPath, path);
            //Console.WriteLine("Step1");
            //if (doc == null)
            //    return null;
            //Console.WriteLine("Step2");
            //var tree = doc.GetSyntaxTreeAsync().Result;
            //ConvertToSwift.Model = doc.GetSemanticModelAsync().Result;
            var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(path));
            var compilation = CSharpCompilation.Create("Virta.Shared.Analysis")
                .AddReferences(
                    MetadataReference.CreateFromFile(
                        typeof(object).Assembly.Location))
                .AddSyntaxTrees(tree);
            BuilderStatic.Model = compilation.GetSemanticModel(tree); //doc.GetSemanticModelAsync().Result;

            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rootNamespace = root.Members.OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            /*

                        // Search for summary section
                        var rootString = root.ToString();
                        var summaryTermStart = "/// <summary>";
                        var summaryTermEnd = "/// </summary>";
                        var summaryIndexStart = rootString.IndexOf(summaryTermStart);
                        var summaryIndexEnd = -1;
                        if (summaryIndexStart > 0)
                            summaryIndexEnd = rootString.IndexOf(summaryTermEnd, summaryIndexStart + summaryTermStart.Length);
                        var summaryText = "";
                        if (summaryIndexStart > 0 && summaryIndexEnd > summaryIndexStart)
                        {
                            summaryText = rootString.Substring(summaryIndexStart + summaryTermStart.Length, summaryIndexEnd - summaryIndexStart - summaryTermStart.Length).Trim();
                            summaryText = string.Join("\n", summaryText.Split('\n').Select(a1 => a1.Trim()));
                            summaryText = summaryText.Replace("///", "//");
                            summaryText = "//" + ConvertToSwift.NewLine + summaryText;
                        }

                        // Search for "//import XYZ" in the usings to output in the final Swift code

                        //output = root.Usings.Aggregate(output, (current, usingDir) => current + GetImportsFromTrivia(usingDir.GetLeadingTrivia()));
                        //output += GetImportsFromTrivia(root.Usings.Last().GetTrailingTrivia());
                        //output += GetImportsFromTrivia(rootNamespace.GetLeadingTrivia());
                        //output += "" + ConvertToSwift.NewLine;
            */

            switch (Language)
            {
                case Languages.Swift:
                    break;
                case Languages.Kotlin:
                    output += $"package {rootNamespace.Name}{BuilderStatic.NewLine}{BuilderStatic.NewLine}";
                    var imports = new List<string>();
                    foreach (var fe1 in root.Usings.Where(a1 => !a1.Name.ToString().ToLower().StartsWith("system")
                                                                && !a1.Name.ToString().StartsWith(nameof(ProtocolBuilder))
                    ))
                    {
                        var fe1Name = fe1.Name.ToString();
                        if (!string.IsNullOrWhiteSpace(fe1.Alias?.ToString()))
                            fe1Name = fe1Name.Replace($".{fe1.Alias.Name.ToString()}", "");
                        imports.Add($"import {fe1Name}.*");
                    }
                    if (imports.Count > 0)
                        output += string.Join(BuilderStatic.NewLine, imports.Distinct()) + BuilderStatic.NewLine;
                    break;
                case Languages.TypeScript:
                    foreach (var feUsingParsed in ParseUsingsWithAlias(root, rootNamespace))
                        output += $"import {{ { feUsingParsed.name } }} from '{feUsingParsed.relativePath}';{BuilderStatic.NewLine}";
                    break;
                case Languages.Php:
                    foreach (var feUsingParsed in ParseUsingsWithAlias(root, rootNamespace))
                        output += $"require('{ feUsingParsed.relativePath }');{BuilderStatic.NewLine}";
                    break;
            }

            if (rootNamespace.Members.OfType<EnumDeclarationSyntax>().Count() > 0)
            {
                // Parses each enum in the file into output.
                output = rootNamespace
                    .Members
                    .OfType<EnumDeclarationSyntax>()
                    .Aggregate(output, (current, childType) =>
                        current + Indenter.FixIndent(BuilderStatic.SyntaxNode(childType))
                    );
            }
            else
            {
                // Parses each class in the file into output.
                output = rootNamespace
                    .Members
                    .OfType<ClassDeclarationSyntax>()
                    .Aggregate(output, (current, childClass) =>
                        current + Indenter.FixIndent(BuilderStatic.SyntaxNode(childClass))
                    );
            }

            output = ""
                     //+ (string.IsNullOrWhiteSpace(summaryText) ? "" : (summaryText + ConvertToSwift.NewLine))
                     + BuilderStatic.NewLine
                     //+ "import RxSwift"
                     //+ ConvertToSwift.NewLine
                     //+ ConvertToSwift.NewLine
                     + output
                     + BuilderStatic.NewLine;

            if (doIndent)
            {
                output = Indenter.IndentDocument(output);
            }

            var saveRelativeDir = "";
            switch (Language)
            {
                case Languages.TypeScript:
                case Languages.Kotlin:
                    saveRelativeDir = $"{string.Join("/", rootNamespace.Name.ToString().Split('.'))}";
                    break;
                default:
                    break;
            }

            return (saveRelativeDir, output);
        }

        private static List<(string relativePath, string name)> ParseUsingsWithAlias(CompilationUnitSyntax root, NamespaceDeclarationSyntax rootNamespace)
        {
            var result = new List<(string relativePath, string name)>();
            var rootNamespaceSections = rootNamespace
                .Name
                .ToString()
                .Split('.')
                .Where(a1 => !string.IsNullOrWhiteSpace(a1))
                .ToList();
            foreach (var fe1 in root.Usings.Where(a1 =>
                !a1.Name.ToString().ToLower().StartsWith("system") &&
                !a1.Name.ToString().StartsWith(nameof(ProtocolBuilder))
            ))
            {
                if (string.IsNullOrWhiteSpace(fe1.Alias?.ToString()))
                    continue;
                var fe1Sections = fe1
                    .Name
                    .ToString()
                    .Split('.')
                    .Where(a1 => !string.IsNullOrWhiteSpace(a1))
                    .ToList();
                var iSectionMatched = 0;
                while (
                    iSectionMatched < rootNamespaceSections.Count &&
                    iSectionMatched < fe1Sections.Count &&
                    rootNamespaceSections[iSectionMatched] == fe1Sections[iSectionMatched]
                )
                {
                    iSectionMatched++;
                }
                var filePath = "";
                if (iSectionMatched < rootNamespaceSections.Count)
                {
                    for (int iSectionReverse = iSectionMatched; iSectionReverse < rootNamespaceSections.Count; iSectionReverse++)
                    {
                        filePath += "../";
                    }
                }
                if (string.IsNullOrWhiteSpace(filePath))
                    filePath = "./";
                if (iSectionMatched < fe1Sections.Count)
                {
                    for (int iSectionForward = iSectionMatched; iSectionForward < fe1Sections.Count; iSectionForward++)
                    {
                        filePath += $"{fe1Sections[iSectionForward]}/";
                    }
                }
                filePath = filePath.Trim('/');

                result.Add((relativePath: filePath, name: fe1.Alias.Name.ToString()));
            }
            return result;
        }

        private string FindSolution(string path, int levels)
        {
            if (File.Exists(path))
            {
                path = (new FileInfo(path)).DirectoryName;
            }

            if (levels == 0) //too much recursion is bad o:
            {
                return "";
            }

            return Directory.GetFiles(path).FirstOrDefault(file => file.EndsWith(".sln")) ??
                   FindSolution((new DirectoryInfo(path)).Parent.FullName, levels - 1);
        }

        public bool Clean()
        {
            if ((!File.Exists(InputPath) || !InputPath.EndsWith(".cs")) && !Directory.Exists(InputPath))
            {
                return false;
            }

            //SlnPath = SlnPath ?? FindSolution(InputPath, 5);
            //if (!File.Exists(SlnPath) || !SlnPath.EndsWith(".sln"))
            //{
            //    return false;
            //}

            OutputPath = OutputPath ?? InputPath.Substring(0, (InputPath.Length - ".cs".Length)) + LanguageFileExtension();

            return true; // OutputPath.EndsWith(LanguageFileExtension());
        }

        public string LanguageFileExtension()
        {
            var r = "";
            switch (Language)
            {
                case Languages.Swift:
                    r = ".swift";
                    break;
                case Languages.Kotlin:
                    r = ".kt";
                    break;
                case Languages.TypeScript:
                    r = ".ts";
                    break;
                case Languages.Php:
                    r = ".php";
                    break;
            }
            return r;
        }

        public string LanguageConvertClass(bool isStatic)
        {
            switch (Language)
            {
                case Languages.Swift:
                    return "struct ";
                case Languages.Kotlin:
                    return isStatic ? "object " : "open class ";
                case Languages.TypeScript:
                    return "export class ";
                case Languages.Php:
                    return "<?php class ";
                default:
                    throw new ArgumentException();
            }
        }

        public string LanguageConvertClassPostfix()
        {
            switch (Language)
            {
                case Languages.Swift:
                    return ": Codable";
                case Languages.Kotlin:
                    return "";
                case Languages.TypeScript:
                    return "";
                case Languages.Php:
                    return "";
                default:
                    throw new ArgumentException();
            }
        }

        public string LanguageConvertEnum()
        {
            switch (Language)
            {
                case Languages.Swift:
                    return "enum";
                case Languages.Kotlin:
                    return "enum class";
                case Languages.TypeScript:
                    return "export enum";
                default:
                    throw new ArgumentException();
            }
        }

        public string LanguageConvertEnumPostfix(string type)
        {
            switch (Language)
            {
                case Languages.Swift:
                    return $": {type}, Codable";
                case Languages.Kotlin:
                    return $"(val rawValue: {type})";
                case Languages.TypeScript:
                    return $"";
                default:
                    throw new ArgumentException();
            }
        }

        public string LanguageDeclarationPart1(
            bool isEnum,
            bool isStatic,
            bool isConst,
            bool isDeclaredAsReadOnly
        )
        {
            var output = "";
            if (isEnum)
            {
                switch (Language)
                {
                    case Languages.Swift:
                        output = $"case ";
                        break;
                    case Languages.TypeScript:
                    case Languages.Kotlin:
                        output = $"";
                        break;
                }
            }
            else
            {
                switch (Language)
                {
                    case Languages.Swift:
                        output = ((isStatic || isConst) ? "static " : "") + (isDeclaredAsReadOnly ? "let " : "var ");
                        break;
                    case Languages.Kotlin:
                        output = //(isStatic ? "" : "") + (THERE IS NO STATIC KEYWORD IN KOTLIN)
                            (isDeclaredAsReadOnly ? "val " : "var ");
                        break;
                    case Languages.TypeScript:
                        output =
                            ((isStatic || isConst) ? "static " : "");
                        break;
                }
            }
            return output;
        }

        public string LanguageDeclarationPart2(
            bool isEnum,
            bool isStatic,
            bool isConst,
            bool isDeclaredAsReadOnly,
            SyntaxToken identifier,
            TypeSyntax declarationType,
            SyntaxList<AttributeListSyntax>? attributes,
            EqualsValueClauseSyntax initializer,
            SyntaxToken? semicolonToken
        )
        {
            var outputVariable = Converters.BuilderStatic.SyntaxTokenConvert(identifier).TrimEnd();

            if (!isEnum && !declarationType.IsVar)
            {
                var isNullable = false;
                if (attributes?.Any(a1 => a1.ToString().ToLower().Contains("nullable")) == true ||
                    declarationType is NullableTypeSyntax ||
                    initializer?.Value?.Kind() == SyntaxKind.NullLiteralExpression)
                {
                    isNullable = true;
                }

                switch (Language)
                {
                    case Languages.TypeScript:
                        outputVariable += $": {Converters.BuilderStatic.SyntaxNode(declarationType).TrimEnd()}{(isNullable ? " | null" : "")}";
                        break;
                    case Languages.Swift:
                    case Languages.Kotlin:
                    default:
                        outputVariable += $": {Converters.BuilderStatic.SyntaxNode(declarationType).TrimEnd()}{((isNullable) ? "?" : "")}";
                        break;
                }
            }

            if (initializer != null &&
                (
                    Language == Languages.Kotlin
                    || isConst
                    || !isDeclaredAsReadOnly
                    || isStatic
                ))
            {
                if (isEnum)
                {
                    switch (Language)
                    {
                        case Languages.Swift:
                            outputVariable += " " + Converters.BuilderStatic.SyntaxNode(initializer);
                            break;
                        case Languages.Kotlin:
                            outputVariable += $"({Converters.BuilderStatic.SyntaxNode(initializer.Value)})";
                            break;
                        case Languages.TypeScript:
                            outputVariable += $" {Converters.BuilderStatic.SyntaxNode(initializer)}";
                            break;
                    }
                }
                else
                {
                    if (initializer.Value.ToString() == "DateTime.UtcNow")
                        outputVariable += " = \"\"";
                    else
                        outputVariable += " " + Converters.BuilderStatic.SyntaxNode(initializer);
                }
            }
            if (isEnum)
            {
                switch (Language)
                {
                    case Languages.Swift:
                        outputVariable += "";
                        break;
                    case Languages.Kotlin:
                        outputVariable += $",";
                        break;
                    case Languages.TypeScript:
                        outputVariable += $",";
                        break;
                }
            }
            else
            {
                if (semicolonToken != null)
                    outputVariable += Converters.BuilderStatic.Semicolon(semicolonToken.Value);
            }
            return outputVariable;
        }

        public string LanguageSyntaxTokenConvert(SyntaxToken source)
        {
            var result = source.ToFullString();
            switch (source.Kind())
            {
                case SyntaxKind.NewKeyword:
                    switch (Language)
                    {
                        case Languages.Swift:
                        case Languages.Kotlin:
                            result = "";
                            break;
                        case Languages.TypeScript:
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

    }
}
