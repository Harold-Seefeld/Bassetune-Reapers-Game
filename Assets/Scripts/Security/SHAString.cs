using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SHAString : MonoBehaviour
{
    public string GetSHA1HashData(string data)
    {
        var sha1 = SHA1.Create();

        var hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));

        var returnValue = new StringBuilder();

        for (var i = 0; i < hashData.Length; i++) returnValue.Append(hashData[i].ToString());

        return returnValue.ToString();
    }

    public string GetSHA256HashData(string data)
    {
        var sha256 = SHA256.Create();

        var hashData = sha256.ComputeHash(Encoding.Default.GetBytes(data));

        var returnValue = new StringBuilder();

        for (var i = 0; i < hashData.Length; i++) returnValue.Append(hashData[i].ToString());

        return returnValue.ToString();
    }

    public string GetSHA384HashData(string data)
    {
        var sha384 = SHA384.Create();

        var hashData = sha384.ComputeHash(Encoding.Default.GetBytes(data));

        var returnValue = new StringBuilder();

        for (var i = 0; i < hashData.Length; i++) returnValue.Append(hashData[i].ToString());

        return returnValue.ToString();
    }

    public string GetSHA512HashData(string data)
    {
        var sha512 = SHA512.Create();

        var hashData = sha512.ComputeHash(Encoding.Default.GetBytes(data));

        var returnValue = new StringBuilder();

        for (var i = 0; i < hashData.Length; i++) returnValue.Append(hashData[i].ToString());

        return returnValue.ToString();
    }
}