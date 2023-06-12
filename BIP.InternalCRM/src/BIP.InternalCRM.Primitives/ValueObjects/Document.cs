namespace BIP.InternalCRM.Primitives.ValueObjects;

public abstract record Document(
    string Filename
)
{
    private byte[]? _data;

    public byte[] Data
    {
        get => _data;
        protected init => LoadData(value);
    }

    public void LoadData(byte[] data)
    {
        _data = new byte[data.Length];
        Array.Copy(data, _data, data.Length);
    }
}
