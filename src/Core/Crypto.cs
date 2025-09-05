using System.Security.Cryptography;
using System.Text.Json;

namespace CyberArk.Migrator.Core.Security;

public static class AesGcmFile
{
    private record Header(string alg, string nonce, string aad);

    public static async Task EncryptAsync(string inputPath, string outputPath, byte[] dek, byte[] aad)
    {
        var plaintext = await File.ReadAllBytesAsync(inputPath);
        var nonce = RandomNumberGenerator.GetBytes(12);
        var tag = new byte[16];
        var ciphertext = new byte[plaintext.Length];

        using var aes = new AesGcm(dek);
        aes.Encrypt(nonce, plaintext, ciphertext, tag, aad);

        var header = new Header("AES-256-GCM", Convert.ToBase64String(nonce), Convert.ToBase64String(aad));
        var hdrBytes = JsonSerializer.SerializeToUtf8Bytes(header);
        var len = BitConverter.GetBytes(hdrBytes.Length);

        using var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
        await fs.WriteAsync(len);
        await fs.WriteAsync(hdrBytes);
        await fs.WriteAsync(tag);
        await fs.WriteAsync(ciphertext);
    }

    public static async Task<byte[]> DecryptAsync(string path, byte[] dek, byte[] aad)
    {
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        var lenBytes = new byte[4];
        await fs.ReadExactlyAsync(lenBytes, 0, 4);
        var headerLen = BitConverter.ToInt32(lenBytes, 0);
        var hdrBytes = new byte[headerLen];
        await fs.ReadExactlyAsync(hdrBytes, 0, headerLen);
        var hdr = JsonDocument.Parse(hdrBytes).RootElement;

        var tag = new byte[16];
        await fs.ReadExactlyAsync(tag, 0, 16);
        var ciphertext = new byte[fs.Length - 4 - headerLen - 16];
        await fs.ReadExactlyAsync(ciphertext, 0, ciphertext.Length);

        var nonce = Convert.FromBase64String(hdr.GetProperty("nonce").GetString()!);

        var plaintext = new byte[ciphertext.Length];
        using var aes = new AesGcm(dek);
        aes.Decrypt(nonce, ciphertext, tag, plaintext, aad);
        return plaintext;
    }
}

public interface IKekProvider
{
    byte[] WrapKey(byte[] dek);
    byte[] UnwrapKey(byte[] wrapped);
}

public sealed class DpapiKekProvider : IKekProvider
{
    public byte[] WrapKey(byte[] dek) =>
        ProtectedData.Protect(dek, null, DataProtectionScope.CurrentUser);

    public byte[] UnwrapKey(byte[] wrapped) =>
        ProtectedData.Unprotect(wrapped, null, DataProtectionScope.CurrentUser);
}

public sealed class CryptoService
{
    private readonly IKekProvider _kek;
    public CryptoService(IKekProvider kek) => _kek = kek;

    public (byte[] dek, byte[] wrapped) NewDek()
    {
        var dek = RandomNumberGenerator.GetBytes(32);
        return (dek, _kek.WrapKey(dek));
    }

    public byte[] Unwrap(byte[] wrapped) => _kek.UnwrapKey(wrapped);
}
