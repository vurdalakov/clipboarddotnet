namespace Vurdalakov.ClipboardDotNet
{
    using System;

    public interface IHexLineDataSource
    {
        Byte[] GetData(Int32 offset);
    }

    public class HexLineViewModel : ViewModelBase
    {
        private IHexLineDataSource _dataSource;

        public Int32 Offset { get; private set; }

        public Byte[] Data { get { return _dataSource.GetData(Offset); } }

        public HexLineViewModel(Int32 offset, IHexLineDataSource dataSource)
        {
            Offset = offset;
            _dataSource = dataSource;
        }
    }
}
