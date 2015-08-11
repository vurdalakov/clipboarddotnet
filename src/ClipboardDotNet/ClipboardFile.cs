namespace Vurdalakov.ClipboardDotNet
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    static public class ClipboardFile
    {
        public const UInt16 Signature = 0xC350;

        static public ClipboardEntry[] Parse(String fileName)
        {
            var reader = new ClipboardFileReader();

            var entries = new List<ClipboardEntry>();
            reader.EntryRead += (s, e) =>
            {
                entries.Add(e.Entry);
            };

            reader.Read(fileName);

            return entries.ToArray();
        }

        static public String GetText(String fileName)
        {
            var reader = new ClipboardFileReader();

            String text = null;
            reader.EntryRead += (s, e) =>
            {
                if (ClipboardText.IsTextFormat(e.Entry.Id))
                {
                    var data = reader.ReadData();
                    text = ClipboardText.ExtractText(e.Entry.Id, data);
                    e.Cancel = true;
                }
            };

            reader.Read(fileName);

            return text;
        }

        static public void Restore(String fileName)
        {
            Clipboard.Empty();

            var reader = new ClipboardFileReader();

            using (var clipboard = new ClipboardApi())
            {
                reader.EntryRead += (s, e) =>
                {
                    var data = reader.ReadData();
                    clipboard.SetData(e.Entry.Id, data);
                };

                reader.Read(fileName);
            }
        }

        static public void Save(String fileName)
        {
            var entries = Clipboard.GetEntries();

            UInt16 count = 0;
            foreach (var entry in entries)
            {
                if (entry.DataSize != 0)
                {
                    count++;
                }

                if (entry.DataSize > UInt32.MaxValue)
                {
                    throw new InvalidOperationException(String.Format("Clipboard format {0} ({1}) data is too big to be saved in CLP format ({2:D} bytes)", entry.Id, entry.Name, entry.DataSize));
                }
            }

            try
            {
                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (var binaryWriter = new BinaryWriter(fileStream))
                    {
                        binaryWriter.Write(Signature);
                        binaryWriter.Write(count);

                        var offset = Convert.ToUInt32(4 + count * 89);
                        foreach (var entry in entries)
                        {
                            if (0 == entry.DataSize)
                            {
                                continue;
                            }

                            binaryWriter.Write(entry.Id);
                            binaryWriter.Write(Convert.ToUInt32(entry.DataSize & 0xFFFFFFFF));
                            binaryWriter.Write(offset);

                            var name = Encoding.ASCII.GetBytes(entry.Name);
                            binaryWriter.Write(name);

                            var filler = new Byte[79 - name.Length];
                            binaryWriter.Write(filler);

                            offset += Convert.ToUInt32(entry.DataSize);
                        }

                        foreach (var entry in entries)
                        {
                            if (0 == entry.DataSize)
                            {
                                continue;
                            }

                            var data = Clipboard.GetData(entry.Id);

                            binaryWriter.Write(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.Delete(fileName);
                throw ex;
            }
        }
    }
}
