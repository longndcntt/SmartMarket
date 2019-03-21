using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMarket.Utilities
{
    public class DataConverter
    {
        #region Static Methods

        /// <summary>
        /// Convert bytes to hexadecimal binary encoding
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Bytes2Hex(byte[] bytes)
        {
            if (bytes != null)
                return BitConverter.ToString(bytes).ToLower().Replace("-", "");
            return null;
        }

        /// <summary>
        /// Convert bytes to 2-byte integer
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static short Bytes2Int16(byte[] bytes)
        {
            if (bytes.Length > 2)
                throw new Exception("Invalid byte size for Int16.");
            return BitConverter.ToInt16(bytes, 0);
        }

        /// <summary>
        /// Convert bytes to 4-byte integer
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int Bytes2Int32(byte[] bytes)
        {
            if (bytes.Length > 4)
                throw new Exception("Invalid byte size for Int32.");
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Convert bytes to 8-byte integer
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Int64 Bytes2Int64(byte[] bytes)
        {
            if (bytes.Length > 8)
                throw new Exception("Invalid byte size for Int64.");
            return BitConverter.ToInt64(bytes, 0);
        }

        /// <summary>
        /// Convert bytes to UTF8 text encoding
        /// </summary>
        /// <returns></returns>
        public static string Bytes2UTF8(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Convert bytes to Base64 binary encoding
        /// </summary>
        /// <returns></returns>
        public static string Bytes2Base64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }


        /// <summary>
        /// Convert hexadecimal binary encoding to bytes
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] Hex2Bytes(string hex)
        {
            try
            {
                var s = hex.ToLower();
                if (s.Length % 2 != 0)
                    throw new Exception("Hexadecimal length should be even.");
                var bytes = Enumerable.Range(0, s.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(s.Substring(x, 2), 16))
                    .ToArray();
                return bytes;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return HexStringToByteArray(hex: hex);
            }
        }

        /// <summary>
        /// Convert hexadecimal binary encoding to bytes
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(string hex)
        {
            var result = new List<byte>();

            for (var i = hex.Length - 1; i >= 0; i -= 2)
            {
                result.Insert(0,
                    i > 0
                        ? Convert.ToByte(hex.Substring(i - 1, 2), 16)
                        : Convert.ToByte(hex.Substring(i, 1), 16));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Convert hexadecimnal binary encoding to Base64 binary encoding
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static string Hex2Base64(string hex)
        {
            var input = Hex2Bytes(hex.ToLower());
            return Convert.ToBase64String(input);
        }

        /// <summary>
        /// Convert hexadecimnal binary encoding to UTF8 text encoding
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static string Hex2UTF8(string hex)
        {
            var input = Hex2Bytes(hex.ToLower());
            return Encoding.UTF8.GetString(input, 0, input.Length);
        }

        /// <summary>
        /// Convert Base64 binary encoding to hexadecimal binary encoding
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static byte[] Base642Bytes(string base64)
        {
            return Convert.FromBase64String(base64);
        }


        /// <summary>
        /// Convert Base64 binary encoding to hexadecimal binary encoding
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string Base642Hex(string base64)
        {
            var input = Convert.FromBase64String(base64);
            return Bytes2Hex(input).ToLower();
        }

        /// <summary>
        /// Convert UTF8 text encoding to hexadecimal binary encoding
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string UTF82Hex(string utf8)
        {
            var bytes = Encoding.UTF8.GetBytes(utf8);
            return Bytes2Hex(bytes).ToLower();
        }


        /// <summary>
        /// Check if current system is little endian architecture
        /// </summary>
        /// <returns></returns>
        public static bool IsLittleEndian()
        {
            return BitConverter.IsLittleEndian;
        }

        /// <summary>
        /// Reverse endian of hexadecimal binary encoding 
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static string ReverseEndian(string hex)
        {
            IList<byte> bytes = new List<byte>(Hex2Bytes(hex));
            return Bytes2Hex(bytes.Reverse().ToArray());
        }

        /// <summary>
        /// Reverse endian of bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] ReverseEndian(byte[] bytes)
        {
            IList<byte> temp = new List<byte>(bytes);
            return temp.Reverse().ToArray();
        }

        /// <summary>
        /// Create a copy of subarray from a byte array
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] SubBytes(byte[] bytes, int startIndex, int length)
        {
            var temp = new byte[length];
            Buffer.BlockCopy(bytes, startIndex, temp, 0, length);
            return temp;
        }

        public static byte[] XOR(string hex1, string hex2)
        {
            if (hex1.Length != hex2.Length)
                throw new Exception("XOR cannot run on different length strings.");
            byte[] bytes1 = Hex2Bytes(hex1);
            byte[] bytes2 = Hex2Bytes(hex2);

            for (int i = 0; i < bytes1.Length; i++)
            {
                bytes1[i] = (byte)(bytes1[i] ^ bytes2[i]);
            }
            return bytes1;
        }


        /// <summary>
        /// Factory method for builder class
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static DataConverter Parse(byte[] bytes)
        {
            DataConverter hex = new DataConverter(bytes);
            return hex;
        }
        /// <summary>
        /// Factory method for builder class
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DataConverter Parse(string s)
        {
            return new DataConverter(s);
        }

        #endregion

        //----------------------------

        private byte[] _bytes;

        #region Constructors
        public DataConverter(byte[] bytes)
        {
            _bytes = new byte[bytes.Length];
            bytes.CopyTo(_bytes, 0);
        }
        public DataConverter(string hex)
        {
            _bytes = Hex2Bytes(hex);
        }
        #endregion
        
        #region Transform
        /// <summary>
        /// Reverse endian of internal representation 
        /// </summary>
        /// <returns></returns>
        public DataConverter Reverse()
        {
            _bytes = ReverseEndian(_bytes);
            return this;
        }

        /// <summary>
        /// Append array to the front
        /// </summary>
        /// <returns></returns>
        public DataConverter AddToFront(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(Parse(bytes).ToString());
            sb.Append(ToString());
            _bytes = Hex2Bytes(sb.ToString());
            return this;
        }

        /// <summary>
        /// Append string to the front
        /// </summary>
        /// <returns></returns>
        public DataConverter AddToFront(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Append(ToString());
            _bytes = Hex2Bytes(sb.ToString());
            return this;
        }

        /// <summary>
        /// Append array to the back
        /// </summary>
        /// <returns></returns>
        public DataConverter AddToBack(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(this.ToString());
            sb.Append(Parse(bytes).ToString());
            _bytes = Hex2Bytes(sb.ToString());
            return this;
        }

        /// <summary>
        /// Append string to the back
        /// </summary>
        /// <returns></returns>
        public DataConverter AddToBack(string s)
        {
            StringBuilder sb = new StringBuilder(this.ToString());
            sb.Append(s);
            _bytes = Hex2Bytes(sb.ToString());
            return this;
        }

        #endregion
    }
}
