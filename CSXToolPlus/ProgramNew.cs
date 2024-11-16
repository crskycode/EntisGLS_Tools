using System;
using System.CommandLine;
using System.IO;

namespace CSXToolPlus
{
    internal class ProgramNew
    {
        static void HandleDisassembleCommand(string scriptPath, string format, string textPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textPath))
                    textPath = Path.ChangeExtension(scriptPath, ".txt");

                var version = format switch
                {
                    "v1" => 1u,
                    "v2" => 2u,
                    _ => 3u
                };

                var image = new ECSExecutionImage();
                image.Load(scriptPath, version);
                image.Disassemble(textPath);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                Environment.ExitCode = 1;
            }
        }

        static void HandleExportCommand(string scriptPath, string format, string textPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textPath))
                    textPath = Path.ChangeExtension(scriptPath, ".txt");

                var version = format switch
                {
                    "v1" => 1u,
                    "v2" => 2u,
                    _ => 3u
                };

                var image = new ECSExecutionImage();
                image.Load(scriptPath, version);
                image.ExportAllText(textPath);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                Environment.ExitCode = 1;
            }
        }

        static void HandleImportCommand(string scriptPath, string format, string textPath, string outputScriptPath, bool updateStringLiteral)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textPath))
                    textPath = Path.ChangeExtension(scriptPath, ".txt");

                if (string.IsNullOrWhiteSpace(outputScriptPath))
                    outputScriptPath = Path.ChangeExtension(scriptPath, ".new.csx");

                var version = format switch
                {
                    "v1" => 1u,
                    "v2" => 2u,
                    _ => 3u
                };

                var image = new ECSExecutionImage();
                image.Load(scriptPath, version);
                image.ImportText(textPath, updateStringLiteral);
                image.Save(outputScriptPath);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                Environment.ExitCode = 1;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("CSXToolPlus v1.4");
            Console.WriteLine("  created by Crsky");
            Console.WriteLine();

            var rootCommand = new RootCommand("Export and import strings from EntisGLS CSX script file.");

            {
                var disassembleCommand = new Command("disassemble", "Parse VM code.");

                disassembleCommand.AddAlias("d");

                var scriptPathArgument = new Argument<string>("path", "Specify the path to the script file.");
                disassembleCommand.AddArgument(scriptPathArgument);

                var formatOption = new Option<string>("--format", () => "v3", "Specify the format of the script file. Available: v1, v2, v3");
                formatOption.AddAlias("-F");
                disassembleCommand.AddOption(formatOption);

                var textPathOption = new Option<string>("--text-path", "Specify the path to the output text file.");
                textPathOption.AddAlias("-T");
                disassembleCommand.AddOption(textPathOption);

                disassembleCommand.SetHandler(HandleDisassembleCommand, scriptPathArgument, formatOption, textPathOption);

                rootCommand.AddCommand(disassembleCommand);
            }

            {
                var exportCommand = new Command("export", "Export string to text file.");

                exportCommand.AddAlias("e");

                var scriptPathArgument = new Argument<string>("path", "Specify the path to the script file.");
                exportCommand.AddArgument(scriptPathArgument);

                var formatOption = new Option<string>("--format", () => "v3", "Specify the format of the script file. Available: v1, v2, v3");
                formatOption.AddAlias("-F");
                exportCommand.AddOption(formatOption);

                var textPathOption = new Option<string>("--text-path", "Specify the path to the output text file.");
                textPathOption.AddAlias("-T");
                exportCommand.AddOption(textPathOption);

                exportCommand.SetHandler(HandleExportCommand, scriptPathArgument, formatOption, textPathOption);

                rootCommand.AddCommand(exportCommand);
            }

            {
                var importCommand = new Command("import", "Import string from text file.");

                importCommand.AddAlias("i");

                var scriptPathArgument = new Argument<string>("path", "Specify the path to the script file.");
                importCommand.AddArgument(scriptPathArgument);

                var formatOption = new Option<string>("--format", () => "v3", "Specify the format of the script file. Available: v1, v2, v3");
                formatOption.AddAlias("-F");
                importCommand.AddOption(formatOption);

                var textPathOption = new Option<string>("--text-path", "Specify the path to the input text file.");
                textPathOption.AddAlias("-T");
                importCommand.AddOption(textPathOption);

                var outputScriptPathOption = new Option<string>("--output-path", "Specify the path to the output script file.");
                outputScriptPathOption.AddAlias("-O");
                importCommand.AddOption(outputScriptPathOption);

                var updateStringLiteralOption = new Option<bool>("--update-string-literal", () => false, "Update the string literal in the image section.");
                updateStringLiteralOption.AddAlias("-S");
                importCommand.AddOption(updateStringLiteralOption);

                importCommand.SetHandler(HandleImportCommand, scriptPathArgument, formatOption, textPathOption, outputScriptPathOption, updateStringLiteralOption);

                rootCommand.AddCommand(importCommand);
            }

            rootCommand.Invoke(args);

            if (OperatingSystem.IsWindows())
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }
}
