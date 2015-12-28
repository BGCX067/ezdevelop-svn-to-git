using System;
using System.IO;
using System.Security.Cryptography;

namespace EZDev
{
    /// <summary>
    /// 加密单元
    /// 代码来源 http://www.cnblogs.com/rush/archive/2011/09/24/2189399.html
    /// </summary>
    public class CryptographyUtils
    {
        /// <summary>
        /// 得到MD5散列字符串
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string GetMD5(string sourceString)
        {
            return HashBytes(new MD5CryptoServiceProvider(), System.Text.Encoding.UTF8.GetBytes(sourceString));
        }

        /// <summary>
        /// 得到SHA512散列字符串
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string GetSHA(string sourceString)
        {
            return HashBytes(new SHA512CryptoServiceProvider(), System.Text.Encoding.UTF8.GetBytes(sourceString));
        }

        /// <summary>
        /// 得到指定散列算法的散列值，并转换为Base64字符串
        /// </summary>
        /// <param name="hashAlgorithm"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string HashBytes(HashAlgorithm hashAlgorithm, byte[] bytes)
        {
            return Convert.ToBase64String(hashAlgorithm.ComputeHash(bytes));
        }


        /// <summary>
        /// 对称加密
        /// </summary>
        /// <param name="algorithm">加密算法</param>
        /// <param name="plainText">明文</param>
        /// <param name="key">密码</param>
        /// <param name="iv">加密向量</param>
        /// <param name="salt">盐</param>
        /// <param name="pwdIterations">密码重复次数</param>
        /// <param name="keySize">密码长度</param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <returns></returns>
        public static byte[] SymmetricEncrypt(SymmetricAlgorithm algorithm, byte[] plainText, string key, string iv, string salt, int pwdIterations, int keySize, CipherMode cipherMode, PaddingMode paddingMode)
        {
            if (null == plainText)
                throw new ArgumentNullException("原文不能为空！");
            if (null == algorithm)
                throw new ArgumentNullException("加密算法不能为空！");
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("密码不能为空！");
            if (String.IsNullOrEmpty(iv))
                throw new ArgumentNullException("向量不能为空！");
            if (String.IsNullOrEmpty(salt))
                throw new ArgumentNullException("盐值不能为空！");

            // Note the salt should be equal or greater that 64bit (8 byte).
            var rfc = new Rfc2898DeriveBytes(key, salt.ToByteArray(), pwdIterations);
            using (SymmetricAlgorithm symmAlgo = algorithm)
            {
                symmAlgo.Mode = cipherMode;
                //symmAlgo.Padding = paddingMode;
                byte[] cipherTextBytes = null;
                using (var encryptor = symmAlgo.CreateEncryptor(
                    rfc.GetBytes(keySize / 8), iv.ToByteArray()))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(
                            ms, encryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(plainText, 0, plainText.Length);
                            cs.FlushFinalBlock();
                            cipherTextBytes = ms.ToArray();
                            ms.Close();
                            cs.Close();
                        }
                    }
                    symmAlgo.Clear();
                    return cipherTextBytes;
                }
            }
        }

        public static byte[] SymmetricDecrypt(SymmetricAlgorithm algorithm, byte[] cipherText, string key, string iv, string salt, int pwdIterations, int keySize, CipherMode cipherMode, PaddingMode paddingMode)
        {
            if (null == cipherText)
                throw new ArgumentNullException("密文不能为为空！");
            if (null == algorithm)
                throw new ArgumentNullException("加密算法不能为空！");
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("密码不能为空！");
            if (String.IsNullOrEmpty(iv))
                throw new ArgumentNullException("向量不能为空！");
            if (String.IsNullOrEmpty(salt))
                throw new ArgumentNullException("盐值不能为空！");

            // Note the salt should be equal or greater that 64bit (8 byte).
            var rfc = new Rfc2898DeriveBytes(key, salt.ToByteArray(), pwdIterations);

            using (SymmetricAlgorithm symmAlgo = algorithm)
            {
                symmAlgo.Mode = cipherMode;
                //symmAlgo.Padding = paddingMode;
                byte[] plainTextBytes = new byte[cipherText.Length];
                int cnt = -1;
                using (var encryptor = symmAlgo.CreateDecryptor(
                    rfc.GetBytes(keySize / 8), iv.ToByteArray()))
                {
                    using (var ms = new MemoryStream(cipherText))
                    {
                        using (var cs = new CryptoStream(
                            ms, encryptor, CryptoStreamMode.Read))
                        {
                            cnt = cs.Read(plainTextBytes, 0, plainTextBytes.Length);
                            ms.Close();
                            cs.Close();

                        }
                    }
                }
                symmAlgo.Clear();
                Array.Resize(ref plainTextBytes, cnt);
                return plainTextBytes;
            }
        }
    }
}