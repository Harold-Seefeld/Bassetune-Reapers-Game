using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SHAString : MonoBehaviour
{

    public string GetSHA1HashData(string data)
    {
        SHA1 sha1 = SHA1.Create();

        byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));

        StringBuilder returnValue = new StringBuilder();

        for (int i = 0; i < hashData.Length; i++)
        {
            returnValue.Append(hashData[i].ToString());
        }

        return returnValue.ToString();
    }

    public string GetSHA256HashData(string data)
    {
        SHA256 sha256 = SHA256.Create();

        byte[] hashData = sha256.ComputeHash(Encoding.Default.GetBytes(data));

        StringBuilder returnValue = new StringBuilder();

        for (int i = 0; i < hashData.Length; i++)
        {
            returnValue.Append(hashData[i].ToString());
        }

        return returnValue.ToString();
    }

    public string GetSHA384HashData(string data)
    {
        SHA384 sha384 = SHA384.Create();

        byte[] hashData = sha384.ComputeHash(Encoding.Default.GetBytes(data));

        StringBuilder returnValue = new StringBuilder();

        for (int i = 0; i < hashData.Length; i++)
        {
            returnValue.Append(hashData[i].ToString());
        }

        return returnValue.ToString();
    }

    public string GetSHA512HashData(string data)
    {
        SHA512 sha512 = SHA512.Create();

        byte[] hashData = sha512.ComputeHash(Encoding.Default.GetBytes(data));

        StringBuilder returnValue = new StringBuilder();

        for (int i = 0; i < hashData.Length; i++)
        {
            returnValue.Append(hashData[i].ToString());
        }

        return returnValue.ToString();
    }
}
