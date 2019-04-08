using System;

namespace ProtocolBuilderCli
{
    class Program
    {
        static void Main(string[] args)
        {
            //TEST WINDOWS
            //args = new[]
            //{
            //    Path.GetFullPath(@"..\..\..\..\..\..\Roju.Shared\"),
            //    "-o",
            //    Path.GetFullPath(@"..\..\..\..\..\..\Output\TypeScript\"),
            //    "-l",
            //    "typescript"
            //};

            //args = new[]
            //{
            //    Path.GetFullPath(@"..\..\..\..\..\..\Roju.Shared\"),
            //    "-o",
            //    Path.GetFullPath(@"..\..\..\..\..\..\Output\Swift\"),
            //    "-l",
            //    "swift"
            //};

            /*
                        args = new[]
                        {
                            Path.GetFullPath(@"..\..\..\..\..\..\Roju.Shared\"),
                            "-o",
                            Path.GetFullPath(@"..\..\..\..\..\..\Output\Kotlin\"),
                            "-l",
                            "kotlin"
                        };
            */

            //TEST MONO
            //args = new string[] { "/Users/ma/Virta/Source/bb/virta-android-2/shared-model/CSharp/SharedCode/", "-o", "/Users/ma/Virta/Source/bb/virta-android-2/shared-model/Output/Swift/", "-l", "swift" };
            //args = new string[] { "/Users/ma/Virta/Source/bb/virta-android-2/shared-model/CSharp/SharedCode/", "-o", "/Users/ma/Virta/Source/bb/virta-android-2/shared-model/Output/Kotlin/", "-l", "kotlin" };

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
