using System;

namespace ProtocolBuilderCli
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            args = new[]
            {
                System.IO.Path.GetFullPath(@"..\..\..\..\SampleSetup\Protocol\"),
                "-o",
                System.IO.Path.GetFullPath(@"..\..\..\..\SampleSetup\Output\PHP\"),
                "-l",
                "php",
                "-n",
                ""
            };
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
