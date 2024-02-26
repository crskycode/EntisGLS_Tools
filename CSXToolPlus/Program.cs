using System;
using System.IO;

namespace CSXToolPlus
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("CSXToolPlus v1.0");
                Console.WriteLine("  created by Crsky");
                Console.WriteLine();
                Console.WriteLine("Usage:");
                Console.WriteLine("  Export all text     : CSXToolPlus -a script.csx");
                Console.WriteLine("  Disassemble         : CSXToolPlus -d script.csx");
                Console.WriteLine("  Rebuild script      : CSXToolPlus -b script.csx");
                Console.WriteLine();
                Console.WriteLine("NOTE: The tool is compatible with EntisGL 2.x script format.");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var mode = args[0];
            var path = Path.GetFullPath(args[1]);

            switch (mode)
            {
                case "-a":
                {
                    var txtPath = Path.ChangeExtension(path, ".txt");

                    var image = new ECSExecutionImage();
                    image.Load(path);
                    image.ExportAllText(txtPath);

                    break;
                }
                case "-d":
                {
                    var txtPath = Path.ChangeExtension(path, ".d.txt");

                    var image = new ECSExecutionImage();
                    image.Load(path);
                    image.Disassemble(txtPath);

                    break;
                }
                case "-b":
                {
                    var txtPath = Path.ChangeExtension(path, ".txt");
                    var newPath = Path.ChangeExtension(path, ".new.csx");

                    var image = new ECSExecutionImage();
                    image.Load(path);
                    image.ImportText(txtPath);
                    image.Save(newPath);

                    break;
                }
            }
        }
    }
}
