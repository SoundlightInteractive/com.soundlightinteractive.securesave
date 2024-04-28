using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace SoundlightInteractive.SecureSave
{
    public class SecurePlayerPrefs : ScriptableObject
    {
        private static byte[] keyArray = Encoding.UTF8.GetBytes("CK2XkHwlxUifg5NMZ8e29RVK2SxoAxGt");

        private static string Encrypt(string toEncrypt)
        {
#if UNITY_EDITOR

            return toEncrypt;
#endif

            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();

            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        private static string Decrypt(string toDecrypt)
        {
#if UNITY_EDITOR

            return toDecrypt;
#endif

            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();

            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

        private static string UTF8ByteArrayToString(byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);

            return constructedString;
        }

        private static byte[] StringToUTF8ByteArray(string pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();

            byte[] byteArray = encoding.GetBytes(pXmlString);

            return byteArray;
        }


        public static void SetInt(string Key, int Value)
        {
            PlayerPrefs.SetString(Encrypt(Key), Encrypt(Value.ToString()));
        }

        public static void SetString(string Key, string Value)
        {
            PlayerPrefs.SetString(Encrypt(Key), Encrypt(Value));
        }

        public static void SetFloat(string Key, float Value)
        {
            PlayerPrefs.SetString(Encrypt(Key), Encrypt(Value.ToString(CultureInfo.CurrentUICulture)));
        }

        public static void SetBool(string Key, bool Value)
        {
            PlayerPrefs.SetString(Encrypt(Key), Encrypt(Value.ToString()));
        }

        public static string GetString(string Key)
        {
            string t = PlayerPrefs.GetString(Encrypt(Key));

            return t == "" ? "" : Decrypt(t);
        }

        public static int GetInt(string Key)
        {
            string t = PlayerPrefs.GetString(Encrypt(Key));

            return t == "" ? 0 : int.Parse(Decrypt(t));
        }

        public static float GetFloat(string Key)
        {
            string t = PlayerPrefs.GetString(Encrypt(Key));
            if (t == "") return 0;
            return float.Parse(Decrypt(t));
        }

        public static bool GetBool(string Key)
        {
            string t = PlayerPrefs.GetString(Encrypt(Key));
            return t != "" && bool.Parse(Decrypt(t));
        }

        public static void DeleteKey(string Key)
        {
            PlayerPrefs.DeleteKey(Encrypt(Key));
        }

        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        public static void Save()
        {
            PlayerPrefs.Save();
        }

        public static bool HasKey(string Key)
        {
            return PlayerPrefs.HasKey(Encrypt(Key));
        }
    }
}