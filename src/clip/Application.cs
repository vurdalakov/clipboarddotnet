namespace Vurdalakov.ClipboardDotNet
{
    using System;

    public class Application : DosToolsApplication
    {
        protected override Int32 Execute()
        {
            if (_commandLineParser.FileNames.Length != 0)
            {
                Help();
            }

            if (_commandLineParser.IsOptionSet("e", "empty"))
            {
                Clipboard.Empty();
            }
            else if (_commandLineParser.IsOptionSet("p", "parse"))
            {
                var count = Clipboard.CountFormats();

                if (0 == count)
                {
                    Console.WriteLine("Clipboard is empty");
                }
                else
                {
                    Console.WriteLine("{0} data formats available on the Clipboard:\n", count);
                    Console.WriteLine("{0,5} {1,5} {2}", "ID", "Size", "Name");
                    Console.WriteLine(new String('-', 30));

                    var entries = Clipboard.GetEntries();
                    foreach (var entry in entries)
                    {
                        Console.WriteLine("{0,5} {1,5} {2}", entry.Format, entry.DataSize, entry.Name);
                    }
                }
            }
            else if (_commandLineParser.IsOptionSet("t", "text"))
            {
                var text = Clipboard.GetText();

                if (text != null)
                {
                    Console.WriteLine("{0}", text);
                }
            }
            else if (_commandLineParser.IsOptionSet("f", "formats"))
            {
                for (var id = UInt16.MinValue; id < UInt16.MaxValue; id++)
                {
                    var name = Clipboard.GetRegisteredFormatName(id);

                    if (name != null)
                    {
                        Console.WriteLine("{0,5} 0x{0:X04} {1}", id, name);
                    }
                }
            }

            return 0;
        }

        protected override void Help()
        {
            Console.WriteLine("CLIP {0} | https://github.com/vurdalakov/dostools\n", ApplicationVersion);
            Console.WriteLine("Works with Windows Clipboard.\n");
            Console.WriteLine("Usage:\n\tclip <-command> [-silent]\n");
            Console.WriteLine("Commands:\n\t-empty - empties Clipboard\n\t-parse - parses Clipboard\n\t-text - extracts text from Clipboard\n\t-formats - lists all registered formats\n");
            Console.WriteLine("Options:\n\t-silent - no error messsages are shown; check exit code\n");
            Console.WriteLine("Exit codes:\n\t0 - command succeeded\n\t1 - command failed\n\t-1 - invalid command line syntax\n");

            base.Help();
        }
    }
}
