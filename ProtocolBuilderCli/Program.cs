using System;

namespace ProtocolBuilderCli
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            if (args.Length <= 0)
            {
                args = new[]
                {
                   System.IO.Path.GetFullPath(@"..\SampleSetup\Protocol\"),
                   "-o",
                   System.IO.Path.GetFullPath(@"..\SampleSetup\Output\PHP\"),
                   "-l",
                   "php",
                   "-n",
                   ""
                };
                // args = new[]
                // {
                //     System.IO.Path.GetFullPath(@"..\..\..\..\SampleSetup\Protocol\"),
                //     "-o",
                //     System.IO.Path.GetFullPath(@"..\..\..\..\SampleSetup\Output\TypeScript\"),
                //     "-l",
                //     "typescript",
                //     "--use-single-quotes-imports",
                //     "true",
                //     "--use-single-quotes-strings",
                //     "true",
                // };
                //args = new[]
                //{
                //    System.IO.Path.GetFullPath(@"..\..\..\..\SampleSetup\Protocol\"),
                //    "-o",
                //    System.IO.Path.GetFullPath(@"..\..\..\..\SampleSetup\Output\Swift\"),
                //    "-l",
                //    "swift"
                //};
                //args = new[]
                //{
                //    System.IO.Path.GetFullPath(@"..\..\..\..\SampleSetup\Protocol\"),
                //    "-o",
                //    System.IO.Path.GetFullPath(@"..\..\..\..\SampleSetup\Output\Kotlin\"),
                //    "-l",
                //    "kotlin"
                //};
            }
#endif

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