namespace SantMarti.Tap.Tzx;

public class TzxLoader
{
    private readonly Stream _stream;
    public static async Task<TzxFile> LoadFromFile(string path)
    {
        var bytes = await File.ReadAllBytesAsync(path);
        var file = new TzxFile(bytes);
        file.Parse();
        return file;
    }
}