namespace Vurdalakov.ClipboardDotNet
{
    using System;
    using System.IO;
    using System.Text;

    internal class ClipboardFileReader
    {
        public ClipboardFileReader()
        {
        }

        private FileStream _fileStream;
        private BinaryReader _binaryReader;
        private UInt32 _dataSize;
        private UInt32 _dataOffset;

        public void Read(String fileName)
        {
            if (null == EntryRead)
            {
                throw new InvalidOperationException("Event not set");
            }

            using (_fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (_binaryReader = new BinaryReader(_fileStream))
                {
                    var signature = _binaryReader.ReadUInt16();
                    if (signature != ClipboardFile.Signature)
                    {
                        throw new InvalidDataException("Wrong file signature");
                    }

                    var count = _binaryReader.ReadUInt16();

                    for (var i = 0; i < count; i++)
                    {
                        var format = _binaryReader.ReadUInt16();
                        _dataSize = _binaryReader.ReadUInt32();
                        _dataOffset = _binaryReader.ReadUInt32();

                        var ascii = _binaryReader.ReadBytes(79);

                        var length = 0;
                        for (var j = 0; j < ascii.Length; j++)
                        {
                            if (0 == ascii[j])
                            {
                                length = j;
                                break;
                            }
                        }

                        var name = Encoding.ASCII.GetString(ascii, 0, length);

                        var e = new ClipboardFileReaderEventArgs(format, name, _dataSize);
                        EntryRead(this, e);

                        if (e.Cancel)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public event EventHandler<ClipboardFileReaderEventArgs> EntryRead;

        public Byte[] ReadData()
        {
            var position = _binaryReader.BaseStream.Position;

            _binaryReader.BaseStream.Seek(_dataOffset, SeekOrigin.Begin);
            var data = _binaryReader.ReadBytes((int)_dataSize);

            _binaryReader.BaseStream.Seek(position, SeekOrigin.Begin);

            return data;
        }
    }

    internal class ClipboardFileReaderEventArgs : EventArgs
    {
        public Boolean Cancel { get; set; }
        public ClipboardEntry Entry { get; private set; }

        public ClipboardFileReaderEventArgs(UInt16 format, String name, UInt64 dataSize)
        {
            Cancel = false;
            Entry = new ClipboardEntry(format, name, dataSize);
        }
    }
}
