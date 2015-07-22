using System;
using System.Linq;
using System.Text;

namespace managedcrypter.IO
{

    class CryptFile
    {
        public CryptFile()
        {
            setEncryptionKey();
        }

        public CryptFile(byte[] fileData)
        {
            this.FileData = fileData;

            setEncryptionKey();
        }

        public byte[] FileData { get; private set; }
        public byte[] EncryptedData { get; private set; }
        public byte[] EncodedData { get; private set; }
        public byte[] EncryptionKey { get; private set; }

        public void EncryptData()
        {
            EncryptedData = xorEncryptDecrypt(FileData, EncryptionKey);
        }

        public void EncodeData()
        {
            EncodedData = new ASCIIEncoding().GetBytes(Convert.ToBase64String(EncryptedData));
        }

#if DEBUG
        public bool SanityCheck()
        {
            byte[] buff = new byte[FileData.Length];
            buff = Convert.FromBase64String(new ASCIIEncoding().GetString(EncodedData));
            buff = xorEncryptDecrypt(buff, EncryptionKey);
            return buff.SequenceEqual(FileData);
        }
#endif

        void setEncryptionKey()
        {
            Random R = new Random(Guid.NewGuid().GetHashCode());
            byte[] Key = new byte[1024];
            R.NextBytes(Key);
            EncryptionKey = Key;
        }

        byte[] xorEncryptDecrypt(byte[] array, byte[] key)
        {
            byte[] ret = new byte[array.Length];
            array.CopyTo(ret, 0);

            for (int i = 0; i < ret.Length; i++)
                ret[i] ^= key[i % key.Length];

            return ret;
        }
    }
}
