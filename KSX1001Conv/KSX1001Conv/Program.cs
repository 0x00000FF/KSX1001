using System.Text;

var fileBytes = File.ReadAllBytes(args[0]);
var builder = new StringBuilder();

for (int i = 0; i < fileBytes.Length; i += 2)
{
    if ((fileBytes[i] & 0x80) == 0)
    {
        builder.Append((char)fileBytes[i]);
        Console.Write((char)fileBytes[i]);

        i--;
    }
    else
    {
        var chosung = ((fileBytes[i] >> 2) & 0x1F) - 2;

        var jungsung = ((fileBytes[i] & 0x03) << 3) | (fileBytes[i + 1] >> 5) & 0x07;

        if (jungsung > 7 && jungsung < 16) jungsung -= 5;
        else if (jungsung > 15 && jungsung < 24) jungsung -= 7;
        else if (jungsung > 25) jungsung -= 9;
        else jungsung -= 3;

        var jongsung = (fileBytes[i + 1] & 0x1F) - 1;

        if (jongsung > 16) jongsung -= 1;

        var unicode = (short)(chosung * 588 + jungsung * 28 + jongsung + 44032);
        var bytes = BitConverter.GetBytes(unicode);
        var str = Encoding.Unicode.GetString(bytes);

        builder.Append(str);
        Console.Write(str);
    }
}