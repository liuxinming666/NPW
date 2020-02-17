using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EWF.Util
{
    public class ZEncypt
    {
        /// <summary>
        /// MD5函数,需引用：using System.Security.Cryptography;
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string instr, Encoding enc = null)
        {
            string result;
            if (string.IsNullOrEmpty(instr))
            {
                result = "";
                return result;
            }
            if (enc == null)
            {
                enc = Encoding.Default;
            }
            byte[] toByte = enc.GetBytes(instr);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            toByte = md5.ComputeHash(toByte);
            result = BitConverter.ToString(toByte).Replace("-", "");

            return result;
        }
    }
}
