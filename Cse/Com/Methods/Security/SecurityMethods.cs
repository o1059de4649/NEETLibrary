using System;
using System.Security.Cryptography;
using System.Text;


namespace CSELibrary.Com.Methods.Security
{
    /// <summary>
    /// セキュリティメソッド群
    /// </summary>
    public static class SecurityMethods {

        /// <summary>
        /// パスワードをハッシュ化
        /// </summary>
        /// <param name="passWord">パスワード</param>
        /// <param name="saltSize">ソルト配列長</param>
        /// <param name="roopCount">ループ回数</param>
        /// <returns>ハッシュ値</returns>
        public static string EncodeHashPassword(string passWord,string saltstr,int roopCount) {
            var salt = GenerateSalt(saltstr);
            var pasHash = GeneratePasswordHashPBKDF2(passWord, salt, roopCount);
            return pasHash;
        }

        /// <summary>
        /// ランダムソルト生成
        /// </summary>
        /// <param name="salt_size">ソルト配列長</param>
        /// <returns>ソルト文字列</returns>
        public static string GenerateSalt(string saltstr)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] buff = utf8.GetBytes(saltstr);
            return Convert.ToBase64String(buff);
        }
        /// <summary>
        /// ハッシュ関数
        /// </summary>
        /// <param name="pwd">パスワード</param>
        /// <param name="salt">ソルト文字列</param>
        /// <returns>ハッシュ値</returns>
        public static string GeneratePasswordHash(
          string pwd, string salt)
        {
            var result = "";
            var saltAndPwd = String.Concat(pwd, salt);
            var encoder = new UTF8Encoding();
            var buffer = encoder.GetBytes(saltAndPwd);
            using (var csp = SHA256.Create())
            {
                var hash = csp.ComputeHash(buffer);
                result = Convert.ToBase64String(hash);
            }
            return result;
        }
        /// <summary>
        /// ループ付きハッシュ関数
        /// </summary>
        /// <param name="pwd">パスワード</param>
        /// <param name="salt">ソルト文字列</param>
        /// <param name="roop">ループ回数</param>
        /// <returns>ハッシュ値</returns>
        public static string GeneratePasswordHashPBKDF2(
            string pwd, string salt, int roop)
        {
            var result = "";
            var encoder = new UTF8Encoding();
            var b = new Rfc2898DeriveBytes(
              pwd, encoder.GetBytes(salt), roop);
            var k = b.GetBytes(32);
            result = Convert.ToBase64String(k);
            return result;
        }
    }
}
