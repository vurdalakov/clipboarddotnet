namespace Vurdalakov.ClipboardDotNet
{
    using System;

    public class Application : DosToolsApplication
    {
        protected override Int32 Execute()
        {
            if (_commandLineParser.FileNames.Length != 1)
            {
                Help();
            }

            var fileName = _commandLineParser.FileNames[0];

            if (_commandLineParser.IsOptionSet("p", "parse"))
            {
                var entries = ClipboardFile.Parse(fileName);

                Console.WriteLine("{0} different data formats available in file:\n", entries.Length);
                Console.WriteLine("{0,5} {1,5} {2}", "ID", "Size", "Name");
                Console.WriteLine(new String('-', 30));

                foreach (var entry in entries)
                {
                    Console.WriteLine("{0,5} {1,5} {2}", entry.Id, entry.DataSize, entry.Name);
                }
            }
            else if (_commandLineParser.IsOptionSet("t", "text"))
            {
                var text = ClipboardFile.GetText(fileName);
                if (text != null)
                {
                    Console.WriteLine("{0}", text);
                }
            }
            else if (_commandLineParser.IsOptionSet("s", "save"))
            {
                ClipboardFile.Save(fileName);
            }
            else if (_commandLineParser.IsOptionSet("r", "restore"))
            {
                ClipboardFile.Restore(fileName);
            }

            return 0;
        }

        protected override void Help()
        {
            Console.WriteLine("CLP {0} | https://github.com/vurdalakov/dostools\n", ApplicationVersion);
            Console.WriteLine("Saves, restores and parses Windows Clipboard files (CLP).\n");
            Console.WriteLine("Usage:\n\tclp <filename> <-command> [-silent]\n");
            Console.WriteLine("Commands:\n\t-parse - parses CLP file\n\t-text - extracts text from CLP file\n\t-save - saves Clipboard to CLP file\n\t-restore - restores Clipboard from CLP file\n");
            Console.WriteLine("Options:\n\t-silent - no error messsages are shown; check exit code\n");
            Console.WriteLine("Exit codes:\n\t0 - command succeeded\n\t1 - command failed\n\t-1 - invalid command line syntax\n");

            base.Help();
        }
    }
}
