using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
/// <summary>
/// XML���л�������
/// </summary>
public static class XMLHelper
{
    /// <summary>
    /// ���л����ļ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data">����</param>
    /// <param name="path">����·��</param>
    public static void SaveToFile<T>(T data, string path, bool compress = false, bool encrypt = false)
    {
        if (!Directory.Exists(Path.GetDirectoryName(path)))//·�������ڻ�δ����
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
        if (Path.GetFileName(path) == null) return;//·��δ�����ļ���
        SaveString(ObjectToXML(data, compress, encrypt), path);
    }
    /// <summary>
    /// ���ļ������л�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T LoadFromFile<T>(string path, bool compress = false, bool encrypt = false)
    {
        if (!File.Exists(path)) return default;//�ļ�������
        return XMLToObject<T>(LoadString(path), compress, encrypt);
    }
    /// <summary>
    /// ��ȡ�ļ�����
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string LoadString(string path)
    {
        if (!File.Exists(path)) return default;//�ļ�������
        using (StreamReader sr = new StreamReader(path))
        {
            return sr.ReadToEnd();
        }
    }
    /// <summary>
    /// �������ݵ��ļ�
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static void SaveString(string content, string path)
    {
        if (!Directory.Exists(Path.GetDirectoryName(path))) return;//·�������ڻ�δ����
        if (Path.GetFileName(path) == null) return;//·��δ�����ļ���
        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.WriteLine(content);
        }
    }
    /// <summary>
    /// ���л���string
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string ObjectToXML<T>(T data, bool compress = false, bool encrypt = false)
    {
        if (data == null) return null;
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        StringBuilder sb = new StringBuilder();
        using (StringWriter stream = new StringWriter(sb))
        {
            serializer.Serialize(stream, data);
        }

        string s = sb.ToString();
        if (compress)
            s = Compress(s);
        if (encrypt)
            s = Encrypt(s);
        return s;
    }
    /// <summary>
    /// ��string�����л�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static T XMLToObject<T>(string xml, bool compress = false, bool encrypt = false)
    {
        if (string.IsNullOrEmpty(xml)) return default;
        if (encrypt)
            xml = Decrypt(xml);
        if (compress)
            xml = Decompress(xml);
        T t;
        using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
        {
            using (StreamReader sr = new StreamReader(ms, true))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                try
                {
                    t = (T)serializer.Deserialize(sr);
                }
                catch
                {
                    t = default;
                }
            }
        }
        return t;
    }

    /// </summary>
    /// AES
    /// </summary>
    /// <param name="encryptStr">����</param>
    /// <param name="key">��Կ</param>
    /// <returns></returns>

    const string key = "1234567812345678";
    public static string Encrypt(string encryptStr, string key = key)
    {
        if (string.IsNullOrEmpty(encryptStr)) return "";
        byte[] toEncryptArray = Encoding.UTF8.GetBytes(encryptStr);
        byte[] resultArray = Encrypt(toEncryptArray, key);
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }
    public static byte[] Encrypt(byte[] encryptByte, string key = key)
    {
        byte[] keyArray = Encoding.UTF8.GetBytes(key);
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateEncryptor();
        return cTransform.TransformFinalBlock(encryptByte, 0, encryptByte.Length);
    }
    /// AES����
    /// </summary>
    /// <param name="decryptStr">����</param>
    /// <param name="key">��Կ</param>
    /// <returns></returns>
    public static string Decrypt(string decryptStr, string key = key)
    {
        if (string.IsNullOrEmpty(decryptStr)) return "";
        byte[] toEncryptArray = Convert.FromBase64String(decryptStr);
        byte[] resultArray = Decrypt(toEncryptArray, key);
        return UTF8Encoding.UTF8.GetString(resultArray);
    }
    public static byte[] Decrypt(byte[] decryptByte, string key = key)
    {
        byte[] keyArray = Encoding.UTF8.GetBytes(key);
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateDecryptor();
        return cTransform.TransformFinalBlock(decryptByte, 0, decryptByte.Length);
    }
    public static string Compress(string str)
    {
        if (string.IsNullOrEmpty(str)) return "";
        var compressBeforeByte = Encoding.GetEncoding("UTF-8").GetBytes(str);
        var compressAfterByte = Compress(compressBeforeByte);
        string compressString = Convert.ToBase64String(compressAfterByte);
        return compressString;
    }

    public static string Decompress(string str)
    {
        if (string.IsNullOrEmpty(str)) return "";
        var compressBeforeByte = Convert.FromBase64String(str);
        var compressAfterByte = Decompress(compressBeforeByte);
        string compressString = Encoding.GetEncoding("UTF-8").GetString(compressAfterByte);
        return compressString;
    }

    /// <summary>
    /// Compress
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private static byte[] Compress(byte[] data)
    {
        try
        {
            var ms = new MemoryStream();
            var zip = new GZipStream(ms, CompressionMode.Compress, true);
            zip.Write(data, 0, data.Length);
            zip.Close();
            var buffer = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(buffer, 0, buffer.Length);
            ms.Close();
            return buffer;

        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// Decompress
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private static byte[] Decompress(byte[] data)
    {
        try
        {
            var ms = new MemoryStream(data);
            var zip = new GZipStream(ms, CompressionMode.Decompress, true);
            var msreader = new MemoryStream();
            var buffer = new byte[0x1000];
            while (true)
            {
                var reader = zip.Read(buffer, 0, buffer.Length);
                if (reader <= 0)
                {
                    break;
                }
                msreader.Write(buffer, 0, reader);
            }
            zip.Close();
            ms.Close();
            msreader.Position = 0;
            buffer = msreader.ToArray();
            msreader.Close();
            return buffer;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
