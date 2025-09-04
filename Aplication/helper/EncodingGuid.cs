using System;
using System.Text;

namespace Application.Helpers
{
    public static class EncodingGuid
    {
        private const string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public static string EncodeBase62(Guid guid)
        {
            byte[] bytes = guid.ToByteArray();
            StringBuilder result = new StringBuilder();
            ulong value = 0;

            // Convertir todos los bytes del GUID en un número grande
            for (int i = 0; i < bytes.Length; i++)
            {
                value = (value << 8) | bytes[i];
            }

            // Convertir el número a Base62
            while (value > 0)
            {
                result.Insert(0, Base62Chars[(int)(value % 62)]);
                value /= 62;
            }

            return result.ToString();
        }
    }
}