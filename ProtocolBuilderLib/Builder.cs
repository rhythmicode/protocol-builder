using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
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
        /// Path to the folder to parse
        /// </summary>
        public string InputPath { get; set; }

        /// <summary>
        /// Boolean indicating whether to indent the output
        /// </summary>
        public bool Indent { get; set; }

        /// <summary>
        /// Path to the output folder
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// Root Namespace
        /// </summary>
        public string Namespace { get; set; } = null;

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
                        case "namespace":
                        case "n":
                            Namespace = arg;
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
            switch (Language)
            {
                case Languages.Php:
                    output += "<?php\n";
                    break;
                case Languages.Swift:
                case Languages.Kotlin:
                case Languages.TypeScript:
                default:
                    break;
            }
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
                    {
                        output += $"package {rootNamespace.Name}{BuilderStatic.NewLine}{BuilderStatic.NewLine}";
                        var imports = ParseUsings(root)
                            .Select(a => $"import {a.ns}.*")
                            .ToList();
                        if (imports.Count > 0)
                            output += string.Join(BuilderStatic.NewLine, imports.Distinct()) + BuilderStatic.NewLine;
                    }
                    break;
                case Languages.TypeScript:
                    foreach (var feUsingParsed in ParseUsingsAsRelativePath(root, rootNamespace))
                        output += $"import {{ { feUsingParsed.name } }} from '{feUsingParsed.relativePath}';{BuilderStatic.NewLine}";
                    break;
                case Languages.Php:
                    {
                        var uParsed = ParseUsingsAsRelativePath(root, rootNamespace);

                        if (Namespace != null)
                        {
                            var namespacePrefix = string.IsNullOrWhiteSpace(Namespace) ? "" : $"{Namespace}\\";
                            output += $"namespace {namespacePrefix}{rootNamespace.Name.ToString().Replace(".", "\\")};{BuilderStatic.NewLine}{BuilderStatic.NewLine}";

                            var imports = ParseUsings(root)
                                .Where(a => !string.IsNullOrWhiteSpace(a.alias))
                                .Select(a => $"use {namespacePrefix}{a.ns.Replace(".", "\\")}\\{a.alias};")
                                .ToList();
                            imports.AddRange(uParsed.Select(a => $"use {namespacePrefix}{RelativeNamespacePathToAbs(rootNamespace, a.relativePath).Replace(".", "\\")};"));
                            if (imports.Count > 0)
                                output += string.Join(BuilderStatic.NewLine, imports.Distinct()) + BuilderStatic.NewLine;
                        }
                        else
                        {
                            foreach (var feUsingParsed in uParsed)
                                output += $"require_once(dirname(__FILE__).'/{ feUsingParsed.relativePath }.php');{BuilderStatic.NewLine}";
                        }
                    }
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
                     + output
                     + BuilderStatic.NewLine;

            if (doIndent)
            {
                output = Indenter.IndentDocument(output);
            }

            var saveRelativeDir = "";
            switch (Language)
            {
                case Languages.Php:
                case Languages.TypeScript:
                case Languages.Kotlin:
                    saveRelativeDir = $"{string.Join("/", rootNamespace.Name.ToString().Split('.'))}";
                    break;
                default:
                    break;
            }

            return (saveRelativeDir, output);
        }

        private static string RelativeNamespacePathToAbs(NamespaceDeclarationSyntax rootNamespace, string relativePath)
        {
            var relativePathSegments = relativePath.Split("/").ToList();
            var rootNsSegments = rootNamespace.Name.ToString().Split(".").ToList();
            foreach (var relSegment in relativePathSegments)
            {
                switch (relSegment)
                {
                    case ".":
                        break;
                    case "..":
                        rootNsSegments.RemoveAt(rootNsSegments.Count - 1);
                        break;
                    default:
                        rootNsSegments.Add(relSegment);
                        break;
                }
            }

            return string.Join('.', rootNsSegments);
        }

        private static List<(string ns, string alias)> ParseUsings(CompilationUnitSyntax root)
        {
            var result = new List<(string ns, string alias)>();
            foreach (var fe1 in root.Usings.Where(a1 => !a1.Name.ToString().ToLower().StartsWith("system")
                                                        && !a1.Name.ToString().StartsWith(nameof(ProtocolBuilder))
            ))
            {
                var fe1Name = fe1.Name.ToString();
                var fe1Alias = fe1.Alias?.Name?.ToString();
                if (!string.IsNullOrWhiteSpace(fe1Alias))
                    fe1Name = fe1Name.Replace($".{fe1Alias}", "");
                result.Add((ns: fe1Name, alias: fe1Alias));
            }
            return result;
        }

        private static List<(string relativePath, string name)> ParseUsingsAsRelativePath(CompilationUnitSyntax root, NamespaceDeclarationSyntax rootNamespace)
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
            result.AddRange(rootNamespace
                .Members
                .OfType<ClassDeclarationSyntax>()
                .SelectMany(c => c.AttributeLists)
                .SelectMany(a => a.Attributes)
                .Where(a => a.Name.ToString() == nameof(UsingRef))
                .Select(
                    a =>
                    {
                        var argName = ParseUsingRefParam(a.ArgumentList.Arguments[0].Expression.ToString());
                        var relPathSegments = new List<string>();
                        for (int iArg = 1; iArg < a.ArgumentList.Arguments.Count; iArg++)
                            relPathSegments.Add(ParseUsingRefParam(a.ArgumentList.Arguments[iArg].Expression.ToString()));
                        var path = string.Join("/", relPathSegments);
                        if (string.IsNullOrWhiteSpace(path))
                            path = ".";
                        return (path + "/" + argName, argName);
                    }
                )
            );
            return result;
        }

        private static string ParseUsingRefParam(string source)
        {
            var r = source;
            if (r.StartsWith("\""))
            {
                r = r.Trim('"').Trim();
            }
            else if (r.StartsWith("nameof"))
            {
                r = r.Replace("nameof", "");
                r = r.Trim('(', ')');
                r = r.Split('.').Last();
            }
            return r;
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
                    return "class ";
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
                case Languages.Php:
                    return "abstract class";
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
                case Languages.Php:
                case Languages.TypeScript:
                    return $"";
                default:
                    throw new ArgumentException();
            }
        }

        public List<string> ClassConstructorLines { get; set; } = new List<string>();

        public Dictionary<string, string> EnumMapToNames { get; set; } = new Dictionary<string, string>();

        public string LanguageDeclaration(
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
            var resultPrefix = "";
            if (isEnum)
            {
                switch (Language)
                {
                    case Languages.Swift:
                        resultPrefix = $"case ";
                        break;
                    case Languages.TypeScript:
                    case Languages.Kotlin:
                        resultPrefix = $"";
                        break;
                    case Languages.Php:
                        resultPrefix = $"const ";
                        break;
                }
            }
            else
            {
                switch (Language)
                {
                    case Languages.Swift:
                        resultPrefix = ((isStatic || isConst) ? "static " : "") + (isDeclaredAsReadOnly ? "let " : "var ");
                        break;
                    case Languages.Kotlin:
                        resultPrefix = //(isStatic ? "" : "") + (THERE IS NO STATIC KEYWORD IN KOTLIN)
                            (isDeclaredAsReadOnly ? "val " : "var ");
                        break;
                    case Languages.TypeScript:
                        resultPrefix =
                            ((isStatic || isConst) ? "static " : "");
                        break;
                    case Languages.Php:
                        resultPrefix =
                            $"{(isConst ? "" : "public ")}{(isConst ? "const " : (isStatic ? "static " : ""))}";
                        break;
                }
            }

            var resultName = Converters.BuilderStatic.SyntaxTokenConvert(identifier).TrimEnd();

            var resultType = "";
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
                    case Languages.Php:
                        resultType = isConst ? "" : $"/** @var {Converters.BuilderStatic.SyntaxNode(declarationType).TrimEnd()}{(isNullable ? "|null" : "")} */ ";
                        break;
                    case Languages.TypeScript:
                        resultType = $": {Converters.BuilderStatic.SyntaxNode(declarationType).TrimEnd()}{(isNullable ? " | null" : "")}";
                        break;
                    case Languages.Swift:
                    case Languages.Kotlin:
                    default:
                        resultType = $": {Converters.BuilderStatic.SyntaxNode(declarationType).TrimEnd()}{((isNullable) ? "?" : "")}";
                        break;
                }
            }

            var resultInitializerValue = "";
            var resultInitializer = "";
            if (initializer != null &&
                (
                    Language == Languages.Kotlin
                    || isConst
                    || !isDeclaredAsReadOnly
                    || isStatic
                ))
            {
                resultInitializerValue = Converters.BuilderStatic.SyntaxNode(initializer.Value);
                if (isEnum)
                {
                    switch (Language)
                    {
                        case Languages.Swift:
                            resultInitializer = " " + Converters.BuilderStatic.SyntaxNode(initializer);
                            break;
                        case Languages.Kotlin:
                            resultInitializer = $"({resultInitializerValue})";
                            break;
                        case Languages.TypeScript:
                            resultInitializer = $" {Converters.BuilderStatic.SyntaxNode(initializer)}";
                            break;
                        case Languages.Php:
                            resultInitializer = $" {Converters.BuilderStatic.SyntaxNode(initializer)}";
                            break;
                    }
                }
                else
                {
                    if (initializer.Value.ToString() == "DateTime.UtcNow" || initializer.Value.ToString() == "DateTime.Now")
                        resultInitializer = " = \"\"";
                    else
                        resultInitializer = " " + Converters.BuilderStatic.SyntaxNode(initializer);

                    switch (Language)
                    {
                        case Languages.Php:
                            if (!isConst)
                            {
                                ClassConstructorLines.Add($"$this->{resultName}{resultInitializer}");
                                resultInitializer = "";
                            }
                            break;
                        case Languages.Swift:
                        case Languages.Kotlin:
                        case Languages.TypeScript:
                        default:
                            break;
                    }
                }
            }

            var resultPostfix = "";
            if (isEnum)
            {
                switch (Language)
                {
                    case Languages.Swift:
                        resultPostfix = "";
                        break;
                    case Languages.Kotlin:
                        resultPostfix = $",";
                        break;
                    case Languages.TypeScript:
                        resultPostfix = $",";
                        break;
                    case Languages.Php:
                        resultPostfix = $";";
                        break;
                }
            }
            else
            {
                if (semicolonToken != null)
                    resultPostfix = Converters.BuilderStatic.Semicolon(semicolonToken.Value);
            }

            if (isEnum)
            {
                EnumMapToNames.Add(resultInitializerValue, resultName);
            }

            var result = "";
            switch (Language)
            {
                case Languages.Php:
                    result = $"{resultType}{resultPrefix}{(isEnum || isConst ? "" : "$")}{resultName}{resultInitializer}{resultPostfix}";
                    break;
                case Languages.TypeScript:
                case Languages.Kotlin:
                case Languages.Swift:
                default:
                    result = $"{resultPrefix}{resultName}{resultType}{resultInitializer}{resultPostfix}";
                    break;
            }

            return result;
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
                        case Languages.Php:
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

        public string LanguageEnumMemberSeparator()
        {
            string result = null;
            switch (Language)
            {
                case Languages.Php:
                case Languages.Swift:
                    result = "\n";
                    break;
                case Languages.Kotlin:
                case Languages.TypeScript:
                default:
                    break;
            }
            return result;
        }

        public string LanguageMemberAccessOperatorToken(SyntaxToken token)
        {
            switch (Language)
            {
                case Languages.Php:
                    return "::";
                case Languages.Swift:
                case Languages.Kotlin:
                case Languages.TypeScript:
                default:
                    return LanguageSyntaxTokenConvert(token);
            }
        }

        public string LanguageConvertClassConstructor()
        {
            var result = "";
            if (ClassConstructorLines.Count > 0)
            {
                switch (Language)
                {
                    case Languages.Php:
                        result = $@"
{BuilderStatic.Indent}{BuilderStatic.Indent}public function __construct()
{BuilderStatic.Indent}{BuilderStatic.Indent}{{
{string.Join(BuilderStatic.NewLine, ClassConstructorLines.Select(a => $"{BuilderStatic.Indent}{BuilderStatic.Indent}{BuilderStatic.Indent}{a};"))}
{BuilderStatic.Indent}{BuilderStatic.Indent}}}
";
                        break;
                    case Languages.Swift:
                    case Languages.Kotlin:
                    case Languages.TypeScript:
                    default:
                        break;
                }
            }
            return result;
        }

        public string LanguageConvertEnumMapToName()
        {
            var result = "";
            if (EnumMapToNames.Count > 0)
            {
                switch (Language)
                {
                    case Languages.Php:
                        result = $@"
{BuilderStatic.Indent}{BuilderStatic.Indent}const MapToName = array(
{string.Join("," + BuilderStatic.NewLine, EnumMapToNames.Select(a => $"{BuilderStatic.Indent}{BuilderStatic.Indent}{BuilderStatic.Indent}{a.Key} => \"{a.Value}\""))}
{BuilderStatic.Indent}{BuilderStatic.Indent});{BuilderStatic.NewLine}";
                        break;
                    case Languages.Swift:
                    case Languages.Kotlin:
                    case Languages.TypeScript:
                    default:
                        break;
                }
            }
            return result;
        }
    }
}
