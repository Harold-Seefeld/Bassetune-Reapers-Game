using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class MD5String : MonoBehaviour
{
    public static string Md5Sum(string strToEncrypt, string salt = null)
    {
        strToEncrypt += salt;
        var ue = new UTF8Encoding();
        var bytes = ue.GetBytes(strToEncrypt);

        // Encrypt bytes
        var md5 = new MD5CryptoServiceProvider();
        var hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        var hashString = "";

        for (var i = 0; i < hashBytes.Length; i++) hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');

        return hashString.PadLeft(32, '0');
    }
}