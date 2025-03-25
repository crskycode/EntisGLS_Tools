using System;
using System.IO;
using System.Linq;

namespace CSXToolPlus
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("CSXToolPlus v1.5");
                Console.WriteLine("  created by Crsky");
                Console.WriteLine();
                Console.WriteLine("Usage:");
                Console.WriteLine("  Export all text     : CSXToolPlus -a script.csx [-v1|-v2|-v3]");
                Console.WriteLine("  Disassemble         : CSXToolPlus -d script.csx [-v1|-v2|-v3]");
                Console.WriteLine("  Rebuild script      : CSXToolPlus -b script.csx [-v1|-v2|-v3] [-f]");
                Console.WriteLine();
                Console.WriteLine("NOTE: The tool is compatible with EntisGL 2.x script format.");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var mode = args[0];
            var path = Path.GetFullPath(args[1]);

            // By default, we handle V3 format.
            var fullVer = 3u;

            if (args.Length > 2)
            {
                switch (args[2])
                {
                    case "-v1":
                        fullVer = 1u;
                        break;
                    case "-v2":
                        fullVer = 2u;
                        break;
                    case "-v3":
                        fullVer = 3u;
                        break;
                }
            }

            switch (mode)
            {
                case "-a":
                {
                    var txtPath = Path.ChangeExtension(path, ".txt");

                    var image = new ECSExecutionImage();
                    image.Load(path, fullVer);
                    image.ExportAllText(txtPath);

                    break;
                }
                case "-d":
                {
                    var txtPath = Path.ChangeExtension(path, ".d.txt");

                    var image = new ECSExecutionImage();
                    image.Load(path, fullVer);
                    image.Disassemble(txtPath);

                    break;
                }
                case "-b":
                {
                    var txtPath = Path.ChangeExtension(path, ".txt");
                    var newPath = Path.ChangeExtension(path, ".new.csx");

                    var updateStringLiterals = args.Skip(2).Any(x => x == "-f");

                    var image = new ECSExecutionImage();
                    image.Load(path, fullVer);
                    image.ImportText(txtPath, updateStringLiterals);
                    image.Save(newPath);

                    break;
                }
            }
        }
    }
}
