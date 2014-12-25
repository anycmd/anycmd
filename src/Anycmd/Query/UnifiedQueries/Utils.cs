namespace Anycmd.Query.UnifiedQueries
{
    using System.Security.Cryptography;
    using System.Text;

    public static class Utils
    {
        public static string GetUniqueIdentifier(int length)
        {
            while (true)
            {
                var maxSize = length;
                const string availableChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                var chars = availableChars.ToCharArray();
                var data = new byte[1];
                var crypto = new RNGCryptoServiceProvider();
                crypto.GetNonZeroBytes(data);
                var size = maxSize;
                data = new byte[size];
                crypto.GetNonZeroBytes(data);
                var result = new StringBuilder(size);
                foreach (var b in data)
                {
                    result.Append(chars[b%(chars.Length - 1)]);
                }

                // Unique identifiers cannot begin with 0-9
                if (result[0] >= '0' && result[0] <= '9')
                {
                    continue;
                }

                return result.ToString();
                break;
            }
        }
    }
}
