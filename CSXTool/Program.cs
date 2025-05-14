using CSXTool.ECS;
using System;
using System.IO;

namespace CSXTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("CSXTool v1.5.1");
                Console.WriteLine("  created by Crsky");
                Console.WriteLine();
                Console.WriteLine("Usage:");
                Console.WriteLine("  Export message text : CSXTool -e script.csx");
                Console.WriteLine("  Export all text     : CSXTool -a script.csx");
                Console.WriteLine("  Disassemble         : CSXTool -d script.csx");
                Console.WriteLine("  Rebuild script      : CSXTool -b script.csx");
                Console.WriteLine();
                Console.WriteLine("NOTE: The tool is compatible with EntisGL 1.x script format.");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var mode = args[0];
            var path = Path.GetFullPath(args[1]);

            switch (mode)
            {
                case "-e":
                {
                    var txtPath = Path.ChangeExtension(path, ".txt");

                    var image = new ECSExecutionImage();
                    image.Load(path);
                    image.ExportText(txtPath);

                    break;
                }
                case "-a":
                {
                    var txtPath = Path.ChangeExtension(path, ".s.txt");

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
                    image.Disasm(txtPath);

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
