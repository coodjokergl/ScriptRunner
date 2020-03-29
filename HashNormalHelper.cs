using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ScriptRunner
{
    public class HashNormalHelper
    {
        /// <summary>
        /// 图片取HASH
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static string Hash(Bitmap img)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms,System.Drawing.Imaging.ImageFormat.Bmp); 
                var hashBytes = HashData(ms.GetBuffer(), ComputeCodeMd5);
                return ByteArrayToHexString(hashBytes);
            }
        }

        #region HASH 工具

        /// <summary>
        ///     Md5算法
        /// </summary>
        public const string ComputeCodeMd5 = "md5";

        /// <summary>
        ///     SHA1算法
        /// </summary>
        public const string ComputeCodeSHA1 = "sha1";

        /// <summary>
        ///     计算文件的 MD5 值
        /// </summary>
        /// <param name="value">要计算 MD5 值字符串</param>
        /// <returns>MD5 值16进制字符串</returns>
        public static string MD5String(string value)
        {
            var hashBytes = HashData(Encoding.UTF8.GetBytes(value), ComputeCodeMd5);
            return ByteArrayToHexString(hashBytes);
        }

        /// <summary>
        ///     计算文件的 Sha1 值
        /// </summary>
        /// <param name="value">要计算 MD5 值字符串</param>
        /// <returns>MD5 值16进制字符串</returns>
        public static string Sha1String(string value)
        {
            var hashBytes = HashData(Encoding.UTF8.GetBytes(value), ComputeCodeSHA1);
            return ByteArrayToHexString(hashBytes);
        }

        /// <summary>
        ///     计算文件的哈希值
        /// </summary>
        /// <param name="fileName">要计算哈希值的文件名和路径</param>
        /// <param name="algName">算法:sha1,md5</param>
        /// <returns>哈希值16进制字符串</returns>
        private static string HashFile(string fileName, string algName)
        {
            if (!File.Exists(fileName)) return string.Empty;

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var hashBytes = HashData(StreamToByte(fs), algName);
                return ByteArrayToHexString(hashBytes);
            }
        }

        /// <summary>
        ///     计算哈希值
        /// </summary>
        /// <param name="data">要计算哈希值的 数据</param>
        /// <param name="algName">算法:sha1,md5</param>
        /// <returns>哈希值字节数组</returns>
        private static byte[] HashData(byte[] data, string algName)
        {
            foreach (var hashMapItem in _dictHashMapper)
                if (string.Compare(algName, hashMapItem.Key, StringComparison.OrdinalIgnoreCase) == 0)
                    return hashMapItem.Value(data);

            throw new NotSupportedException(algName);
        }

        private static readonly Dictionary<string, Func<byte[], byte[]>> _dictHashMapper =
            new Dictionary<string, Func<byte[], byte[]>>
            {
                {ComputeCodeMd5, s => MD5.Create().ComputeHash(s)},
                {ComputeCodeSHA1, s => SHA1.Create().ComputeHash(s)}
            };

        /// <summary>
        ///     字节数组转换为16进制表示的字符串
        /// </summary>
        private static string ByteArrayToHexString(byte[] buf)
        {
            return BitConverter.ToString(buf).Replace("-", "");
        }

        /// <summary>
        ///     读取流数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static byte[] StreamToByte(Stream stream)
        {
            var buff = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(buff, 0, buff.Length);
            return buff;
        }

        #endregion
    }
}
