using System.IO;
using System.Text;

namespace Kennan.Revit.File.Utility
{
    internal static class StreamUtils
    {
        internal static string StreamToString(Stream stream, Encoding encoding)
        {
            return encoding.GetString(StreamToBytes(stream));
        }

        internal static byte[] StreamToBytes(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }
    }
}