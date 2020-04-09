using common.Config;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace common.Utils
{
    public class Common
    {
        /// <summary>
        /// 读取文本文件内容
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string GetFileContent(string path)
        {
            string str = null;
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    str += line.ToString();
                }
            }
            return str;
        }

        /// <summary>
        /// 生成guid
        /// </summary>
        /// <param name="_fmt">
        /// 格式
        /// null: {ee69e211-a55e-4211-af69-e046c293c39e}
        /// N: ee69e211a55e4211af69e046c293c39e
        /// D: ee69e211-a55e-4211-af69-e046c293c39e
        /// B: {ee69e211-a55e-4211-af69-e046c293c39e}
        /// P: (ee69e211-a55e-4211-af69-e046c293c39e)
        /// X: {0xee69e211,0xa55e,0x4211,{0xaf,0x69,0xe0,0x46,0xc2,0x93,0xc3,0x9e}}
        /// </param>
        /// <returns></returns>
        public static string MakeGuid(string _fmt = "N")
        {
            return Guid.NewGuid().ToString(_fmt);
        }

        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="str">加密串</param>
        /// <param name="_mark">混合</param>
        /// <returns></returns>
        public static string MakeMd5(string str, string _mark = null)
        {
            MD5 md5 = MD5.Create();
            byte[] bt_str = Encoding.Default.GetBytes(_mark + str + _mark);
            byte[] md5_str = md5.ComputeHash(bt_str);
            md5_str = md5.ComputeHash(md5_str);

            return BitConverter.ToString(md5_str).Replace("-", "");
        }

        /// <summary>
        /// @xis aes 加密 2020-2-19 21:42:35
        /// </summary>
        /// <param name="str"></param>
        /// <param name="_salt"></param>
        /// <returns></returns>
        public static string AESEncrypt(string str, string _salt)
        {
            var encryptKey = Encoding.UTF8.GetBytes(_salt);
            var iv = Encoding.UTF8.GetBytes("ASDFGHJKLQWERTYU"); //偏移量,最小为16
            using var aesAlg = Aes.Create();
            using var encryptor = aesAlg.CreateEncryptor(encryptKey, iv);
            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
                swEncrypt.Write(str);
            var decryptedContent = msEncrypt.ToArray();

            return Convert.ToBase64String(decryptedContent);
        }

        /// <summary>
        /// @xis AES 解密 2020-2-19 21:43:01
        /// </summary>
        /// <param name="str"></param>
        /// <param name="_salt"></param>
        /// <returns></returns>
        public static string AESDecrypt(string str, string _salt)
        {
            var fullCipher = Convert.FromBase64String(str);
            var iv = Encoding.UTF8.GetBytes("ASDFGHJKLQWERTYU");
            var key = Encoding.UTF8.GetBytes(_salt);

            using var aesAlg = Aes.Create();
            using var decryptor = aesAlg.CreateDecryptor(key, iv);
            using var msDecrypt = new MemoryStream(fullCipher);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }


    }
}
