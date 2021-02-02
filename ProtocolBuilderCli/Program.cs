using System;
using System.Collections.Generic;

namespace ProtocolBuilderCli
{
    class Program
    {
        static void Main(string[] args)
        {
#if !DEBUG
            RunWithArgs(args);
            return;
#endif
            if (args.Length > 0)
            {
                RunWithArgs(args);
                return;
            }

            var sampleSetupRoot = System.IO.Path.Combine("..", "SampleSetup");
            var allArgs = new List<string[]>();
            allArgs.Add(new[]
                {
                   System.IO.Path.GetFullPath(System.IO.Path.Combine(sampleSetupRoot,"Protocol")),
                   "-o",
                   System.IO.Path.GetFullPath(System.IO.Path.Combine(sampleSetupRoot,"Output","PHP")),
                   "-l",
                   "php",
                   "--language-version",
                   "7.4",
                   "--folder-hierarchy-skip-namespace-from-root",
                   "2",
                   "-n",
                   ""
                });
            allArgs.Add(new[]
                {
                   System.IO.Path.GetFullPath(System.IO.Path.Combine(sampleSetupRoot,"Protocol")),
                   "-o",
                   System.IO.Path.GetFullPath(System.IO.Path.Combine(sampleSetupRoot,"Output","PHP_8")),
                   "-l",
                   "php",
                   "--language-version",
                   "8.0",
                   "--folder-hierarchy-skip-namespace-from-root",
                   "2",
                   "-n",
                   ""
                });
            allArgs.Add(new[]
                {
                    System.IO.Path.GetFullPath(System.IO.Path.Combine(sampleSetupRoot,"Protocol")),
                    "-o",
                    System.IO.Path.GetFullPath(System.IO.Path.Combine(sampleSetupRoot,"Output","TypeScript")),
                    "-l",
                    "typescript",
                    "--use-single-quotes-imports",
                    "true",
                    "--use-single-quotes-strings",
                    "true",
                });
            allArgs.Add(new[]
                {
                    System.IO.Path.GetFullPath(System.IO.Path.Combine(sampleSetupRoot,"Protocol")),
                    "-o",
                    System.IO.Path.GetFullPath(System.IO.Path.Combine(sampleSetupRoot,"Output","Swift")),
                    "-l",
                    "swift"
                });
            allArgs.Add(new[]
                {
                    System.IO.Path.GetFullPath(System.IO.Path.Combine(sampleSetupRoot,"Protocol")),
                    "-o",
                    System.IO.Path.GetFullPath(System.IO.Path.Combine(sampleSetupRoot,"Output","Kotlin")),
                    "-l",
                    "kotlin"
                });
            foreach (var feArgs in allArgs)
            {
                RunWithArgs(feArgs);
            }
        }

        static void RunWithArgs(string[] args)
        {
            var builder = new ProtocolBuilder.Builder(args);
            if (!builder.Clean())
            {
                Console.WriteLine("Sorry, there was a fatal error with your arguments");
                return;
            }

            builder.Process();
        }
    }
}