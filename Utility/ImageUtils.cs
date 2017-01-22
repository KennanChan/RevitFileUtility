using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Kennan.Revit.File.Utility
{
    internal class ImageUtils
    {
        internal static Image ResolveImageFromBytes(byte[] imageBytes, ImageFormat format)
        {
            if (null == format)
                format = ImageFormat.Png;
            if (null == imageBytes)
                return null;
            var startPosition = FindHeadPosition(imageBytes, format);
            if (startPosition < 0)
                return null;
            var memoryStream = new MemoryStream(imageBytes, startPosition, imageBytes.Length - startPosition);
            var thumbnailImage = Image.FromStream(memoryStream);
            memoryStream.Flush();
            memoryStream.Close();
            return thumbnailImage;
        }

        private static int FindHeadPosition(byte[] imageBytes, ImageFormat format)
        {
            if (format.Guid.Equals(ImageFormat.Png.Guid))
                return FindPngHeadPosition(imageBytes);
            return -1;
        }

        /// <summary>
        ///     找到png图像的文件头
        ///     <para>
        ///         PNG文件头示例如下：
        ///     </para>
        ///     <para>
        ///         十进制数	    137 80  78  71  13  10  26  10
        ///     </para>
        ///     <para>
        ///         十六进制数	89  50  4E  47  0D  0A  1A  0A
        ///     </para>
        /// </summary>
        /// <param name="pngImageBytes">获取到的预览图数据</param>
        /// <remarks>Inspired by "http://thebuildingcoder.typepad.com/blog/2010/06/open-revit-ole-storage.html"</remarks>
        /// <returns>文件头位置</returns>
        private static int FindPngHeadPosition(byte[] pngImageBytes)
        {
            var markerFound = false;
            var startingOffset = 0;
            var previousValue = 0;
            using (var ms = new MemoryStream(pngImageBytes))
            {
                for (var i = 0; i < pngImageBytes.Length; i++)
                {
                    var currentValue = ms.ReadByte();
                    if (currentValue == 137) // 0x89
                    {
                        markerFound = true;
                        startingOffset = i;
                        previousValue = currentValue;
                        continue;
                    }

                    switch (currentValue)
                    {
                        case 80: // 0x50
                            if (markerFound && (previousValue == 137))
                            {
                                previousValue = currentValue;
                                continue;
                            }
                            markerFound = false;
                            break;

                        case 78: // 0x4E
                            if (markerFound && (previousValue == 80))
                            {
                                previousValue = currentValue;
                                continue;
                            }
                            markerFound = false;
                            break;

                        case 71: // 0x47
                            if (markerFound && (previousValue == 78))
                            {
                                previousValue = currentValue;
                                continue;
                            }
                            markerFound = false;
                            break;

                        case 13: // 0x0D
                            if (markerFound && (previousValue == 71))
                            {
                                previousValue = currentValue;
                                continue;
                            }
                            markerFound = false;
                            break;

                        case 10: // 0x0A
                            if (markerFound && (previousValue == 26))
                            {
                                return startingOffset;
                            }
                            if (markerFound && (previousValue == 13))
                            {
                                previousValue = currentValue;
                                continue;
                            }
                            markerFound = false;
                            break;

                        case 26: // 0x1A
                            if (markerFound && (previousValue == 10))
                            {
                                previousValue = currentValue;
                                continue;
                            }
                            markerFound = false;
                            break;
                    }
                }
            }
            return -1;
        }
    }
}