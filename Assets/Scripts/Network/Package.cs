using System;
using System.Text;
using Network.Exceptions;
using System.IO;

namespace Network
{
    /// <summary>
    /// Stores various data as an array of bytes for later transfer between processes.
    /// </summary>
    public sealed class Package
    {
        MemoryStream _bytes = new MemoryStream();
        private int _bytesLength = 0;
        private readonly bool _reading;

        /// <summary>
        /// Package type.
        /// </summary>
        public int Type { get; private set; }

        /// <summary>
        /// Creates a package for letter writing.
        /// </summary>
        /// <param name="type">Package type.</param>
        public Package(int type)
        {
            _reading = false;

            Type = type;
            WriteInt(type);
        }

        /// <summary>
        /// Creates a package for letter writing.
        /// </summary>
        /// <param name="type">Package type. Must be convertible to integer. You can use enumerations.</param>
        public Package(object type)
        {
            _reading = false;

            int integerType;

            try
            {
                integerType = Convert.ToInt32(type);
            }
            catch
            {
                throw new PackageException("Can't convert type to integer");
            }

            Type = integerType;
            WriteInt(integerType);
        }

        /// <summary>
        /// Creates a package for letter reading.
        /// </summary>
        /// <param name="bytes">Byte array.</param>
        public Package(byte[] bytes)
        {
            _bytes.Write(bytes, 0, bytes.Length);
            _bytes.Seek(0, SeekOrigin.Begin);

            _reading = true;

            Type = ReadInt();
        }

        /// <summary>
        /// Returns the package as an array of bytes.
        /// </summary>
        /// <returns>Package byte array.</returns>
        public byte[] GetBytes()
        {
            return _bytes.GetBuffer();
        }

        /// <summary>
        /// Writes <b>ushort</b> to the package (2 bytes).
        /// </summary>
        /// <param name="value"><b>ushort</b> value.</param>
        public void WriteUInt16(ushort value)
        {
            Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes <b>ushort</b> to the package (2 bytes).
        /// </summary>
        /// <param name="package">The package where the data will be written.</param>
        /// <param name="value">Value to be written.</param>
        /// <returns>Package with added value.</returns>
        public static Package operator +(Package package, ushort value)
        {
            package.WriteUInt16(value);
            return package;
        }

        /// <summary>
        /// Writes <b>short</b> to the package (2 bytes).
        /// </summary>
        /// <param name="value"><b>short</b> value.</param>
        public void WriteInt16(short value)
        {
            Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes <b>short</b> to the package (2 bytes).
        /// </summary>
        /// <param name="package">The package where the data will be written.</param>
        /// <param name="value">Value to be written.</param>
        /// <returns>Package with added value.</returns>
        public static Package operator +(Package package, short value)
        {
            package.WriteInt16(value);
            return package;
        }

        /// <summary>
        /// Writes <b>uint</b> to the package (4 bytes).
        /// </summary>
        /// <param name="value"><b>uint</b> value.</param>
        public void WriteUInt(uint value)
        {
            Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes <b>uint</b> to the package (4 bytes).
        /// </summary>
        /// <param name="package">The package where the data will be written.</param>
        /// <param name="value">Value to be written.</param>
        /// <returns>Package with added value.</returns>
        public static Package operator +(Package package, uint value)
        {
            package.WriteUInt(value);
            return package;
        }

        /// <summary>
        /// Writes <b>int</b> to the package (4 bytes).
        /// </summary>
        /// <param name="value"><b>int</b> value.</param>
        public void WriteInt(int value)
        {
            Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes <b>int</b> to the package (4 bytes).
        /// </summary>
        /// <param name="package">The package where the data will be written.</param>
        /// <param name="value">Value to be written.</param>
        /// <returns>Package with added value.</returns>
        public static Package operator +(Package package, int value)
        {
            package.WriteInt(value);
            return package;
        }

        /// <summary>
        /// Writes <b>ulong</b> to the package (8 bytes).
        /// </summary>
        /// <param name="value"><b>ulong</b> value.</param>
        public void WriteUInt64(ulong value)
        {
            Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes <b>ulong</b> to the package (8 bytes).
        /// </summary>
        /// <param name="package">The package where the data will be written.</param>
        /// <param name="value">Value to be written.</param>
        /// <returns>Package with added value.</returns>
        public static Package operator +(Package package, ulong value)
        {
            package.WriteUInt64(value);
            return package;
        }

        /// <summary>
        /// Writes <b>long</b> to the package (8 bytes).
        /// </summary>
        /// <param name="value"><b>long</b> value.</param>
        public void WriteInt64(long value)
        {
            Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes <b>long</b> to the package (8 bytes).
        /// </summary>
        /// <param name="package">The package where the data will be written.</param>
        /// <param name="value">Value to be written.</param>
        /// <returns>Package with added value.</returns>
        public static Package operator +(Package package, long value)
        {
            package.WriteInt64(value);
            return package;
        }

        /// <summary>
        /// Writes <b>float</b> to the package (4 bytes).
        /// </summary>
        /// <param name="value"><b>float</b> value.</param>
        public void WriteFloat(float value)
        {
            Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes <b>float</b> to the package (4 bytes).
        /// </summary>
        /// <param name="package">The package where the data will be written.</param>
        /// <param name="value">Value to be written.</param>
        /// <returns>Package with added value.</returns>
        public static Package operator +(Package package, float value)
        {
            package.WriteFloat(value);
            return package;
        }

        /// <summary>
        /// Writes <b>double</b> to the package (8 bytes).
        /// </summary>
        /// <param name="value"><b>double</b> value.</param>
        public void WriteDouble(double value)
        {
            Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes <b>double</b> to the package (8 bytes).
        /// </summary>
        /// <param name="package">The package where the data will be written.</param>
        /// <param name="value">Value to be written.</param>
        /// <returns>Package with added value.</returns>
        public static Package operator +(Package package, double value)
        {
            package.WriteDouble(value);
            return package;
        }

        /// <summary>
        /// Writes <b>bool</b> to the package (1 byte).
        /// </summary>
        /// <param name="value"><b>bool</b> value.</param>
        public void WriteBool(bool value)
        {
            Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes <b>bool</b> to the package (1 byte).
        /// </summary>
        /// <param name="package">The package where the data will be written.</param>
        /// <param name="value">Value to be written.</param>
        /// <returns>Package with added value.</returns>
        public static Package operator +(Package package, bool value)
        {
            package.WriteBool(value);
            return package;
        }

        /// <summary>
        /// Writes <b>char</b> to the package (2 bytes).
        /// </summary>
        /// <param name="value"><b>char</b> value.</param>
        public void WriteChar(char value)
        {
            Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes <b>char</b> to the package (2 bytes).
        /// </summary>
        /// <param name="package">The package where the data will be written.</param>
        /// <param name="value">Value to be written.</param>
        /// <returns>Package with added value.</returns>
        public static Package operator +(Package package, char value)
        {
            package.WriteChar(value);
            return package;
        }

        /// <summary>
        /// Writes <b>string</b> to the package (4 bytes to storing size, the size of the string depends on its content).
        /// Default encoding - UTF8.
        /// </summary>
        /// <param name="value"><b>string</b> value.</param>
        public void WriteString(string value)
        {
            WriteString(value, Encoding.UTF8);
        }

        /// <summary>
        /// Writes <b>string</b> to the package (4 bytes to storing size, the size of the string depends on its content).
        /// Default encoding - UTF8.
        /// </summary>
        /// <param name="package">The package where the data will be written.</param>
        /// <param name="value">Value to be written.</param>
        /// <returns>Package with added value.</returns>
        public static Package operator +(Package package, string value)
        {
            package.WriteString(value, Encoding.UTF8);
            return package;
        }

        /// <summary>
        /// Writes <b>string</b> to the package in current encoding (4 bytes to storing size, the size of the string depends on its content).
        /// </summary>
        /// <param name="value"><b>string</b> value.</param>
        /// <param name="encoding">String encoding.</param>
        public void WriteString(string value, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(value);

            WriteInt(bytes.Length);
            Write(bytes);
        }

        /// <summary>
        /// Reads <b>ushort</b> from the package (2 bytes).
        /// </summary>
        /// <returns><b>ushort</b> value.</returns>
        public ushort ReadUInt16()
        {
            return BitConverter.ToUInt16(Read(sizeof(ushort)), 0);
        }

        /// <summary>
        /// Reads <b>short</b> from the package (2 bytes).
        /// </summary>
        /// <returns><b>short</b> value.</returns>
        public short ReadInt16()
        {
            return BitConverter.ToInt16(Read(sizeof(short)), 0);
        }

        /// <summary>
        /// Reads <b>uint</b> from the package (4 bytes).
        /// </summary>
        /// <returns><b>uint</b> value.</returns>
        public uint ReadUInt()
        {
            return BitConverter.ToUInt32(Read(sizeof(uint)), 0);
        }

        /// <summary>
        /// Reads <b>int</b> from the package (4 bytes).
        /// </summary>
        /// <returns><b>int</b> value.</returns>
        public int ReadInt()
        {
            return BitConverter.ToInt32(Read(sizeof(int)), 0);
        }

        /// <summary>
        /// Reads <b>ulong</b> from the package (8 bytes).
        /// </summary>
        /// <returns><b>ulong</b> value.</returns>
        public ulong ReadUInt64()
        {
            return BitConverter.ToUInt64(Read(sizeof(ulong)), 0);
        }

        /// <summary>
        /// Reads <b>long</b> from the package (8 bytes).
        /// </summary>
        /// <returns><b>long</b> value.</returns>
        public long ReadInt64()
        {
            return BitConverter.ToInt64(Read(sizeof(long)), 0);
        }

        /// <summary>
        /// Reads <b>float</b> from the package (4 bytes).
        /// </summary>
        /// <returns><b>float</b> value.</returns>
        public float ReadFloat()
        {
            return BitConverter.ToSingle(Read(sizeof(float)), 0);
        }

        /// <summary>
        /// Reads <b>double</b> from the package (8 bytes).
        /// </summary>
        /// <returns><b>double</b> value.</returns>
        public double ReadDouble()
        {
            return BitConverter.ToDouble(Read(sizeof(double)), 0);
        }

        /// <summary>
        /// Reads <b>bool</b> from the package (1 byte).
        /// </summary>
        /// <returns><b>bool</b> value.</returns>
        public bool ReadBool()
        {
            return BitConverter.ToBoolean(Read(sizeof(bool)), 0);
        }

        /// <summary>
        /// Reads <b>char</b> from the package (2 bytes).
        /// </summary>
        /// <returns><b>char</b> value.</returns>
        public char ReadChar()
        {
            return BitConverter.ToChar(Read(sizeof(char)), 0);
        }

        /// <summary>
        /// Reads <b>string</b> from the package (4 bytes - stored size, the size of the string depends on its content).
        /// Default encoding - UTF8.
        /// </summary>
        /// <returns><b>string</b> value.</returns>
        public string ReadString()
        {
            return ReadString(Encoding.UTF8);
        }

        /// <summary>
        /// Reads <b>string</b> from the package (4 bytes - stored size, the size of the string depends on its content).
        /// </summary>
        /// <param name="encoding">String encoding.</param>
        /// <returns><b>string</b> value.</returns>
        public string ReadString(Encoding encoding)
        {
            int length = ReadInt();
            return encoding.GetString(Read(length));
        }

        private void Write(byte[] bytes)
        {
            if (_reading)
                throw new PackageException("Can't write in reading mode");

            _bytes.Write(bytes, 0, bytes.Length);
            _bytesLength += bytes.Length;
        }

        private byte[] Read(int size)
        {
            if (!_reading)
                throw new PackageException("Can't read in writing mode");

            byte[] bytes = new byte[size];

            _bytes.Read(bytes, 0, size);

            return bytes;
        }
    }
}
