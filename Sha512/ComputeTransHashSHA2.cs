using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sha512
{
    class ComputeTransHashSHA2
    {
        static void TestTransHashSHA2(string[] args)
        {
            String key = "14B9609FFE2378449B3C0886046DD3B0F20DF12DEB758E48B5FFE1B5875615F0D2A50F7DDB1EAC417EBF76A1FAC374079793650AA493CE127601CB0960938E82";
            String transId = "60115446273";
            String apiLogin = "5T9cRn9FK";
            String amount = "9.00";
            //transHashSHA2 represents the computed TransHash2 using SignatureKey for the given transaction
            //textToHash is formed by concatenating apilogin id , transId for the given transaction and transaction amount.
            // For more details please visit https://developer.authorize.net/support/hash_upgrade/?utm_campaign=19Q2%20MD5%20Hash%20EOL%20Partner&utm_medium=email&utm_source=Eloqua for implementation details.    
            String transHashSHA2 = ComputeTransHashSHA2.HMACSHA512(key, "^"+ apiLogin+"^"+ transId +"^"+ amount+"^");    
        }

        /**
            * This is method to generate HMAC512 key for a given signature key and Text to Hash
            * @param signatureKey
            * @param textToHash
            * @return
            * @throws Exception
           */
        public static string HMACSHA512(string key, string textToHash)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("HMACSHA512: key", "Parameter cannot be empty.");
            if (string.IsNullOrEmpty(textToHash))
                throw new ArgumentNullException("HMACSHA512: textToHash", "Parameter cannot be empty.");
            if (key.Length % 2 != 0 || key.Trim().Length < 2)
            {
                throw new ArgumentNullException("HMACSHA512: key", "Parameter cannot be odd or less than 2 characters.");
            }
            try
            {
                // This is the section to con vert byte array to hexadecimal string
                byte[] k = Enumerable.Range(0, key.Length)
                            .Where(x => x % 2 == 0)
                            .Select(x => Convert.ToByte(key.Substring(x, 2), 16))
                            .ToArray();
                HMACSHA512 hmac = new HMACSHA512(k);
                byte[] HashedValue = hmac.ComputeHash((new System.Text.ASCIIEncoding()).GetBytes(textToHash));
                return BitConverter.ToString(HashedValue).Replace("-", string.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception("HMACSHA512: " + ex.Message);
            }
        }

    }
}
