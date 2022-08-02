using System.Text;

var fileBytes = File.ReadAllBytes(args[0]);
var list = new List<byte>();

for (int i = 0; i < fileBytes.Length; i += 2)
{
    if ((fileBytes[i] & 0x80) == 0)
    {
        list.Add(fileBytes[i]);
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

        var unicode = (ushort)(chosung * 588 + jungsung * 28 + jongsung + 44032);
        list.AddRange(new byte[] {
            (byte)(0xE0 | (unicode >> 12)),
            (byte)(0x80 | ((unicode >> 6) & 0x3F)),
            (byte)(0x80 | (unicode & 0x3F))
        });
    }
}

File.WriteAllBytes(args[0] + ".utf8", list.ToArray());