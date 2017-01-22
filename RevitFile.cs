using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Kennan.Revit.File.Enumeration;
using Kennan.Revit.File.Utility;

namespace Kennan.Revit.File
{
    /// <summary>
    ///     Represent a revit file from which one can get basic file info and thumbnail
    /// </summary>
    public class RevitFile
    {
        private BasicFileInfo _basicFileInfo;
        private Image _thumbnail;

        public RevitFile(string filePath)
        {
            FilePath = filePath;
            GetAllContents();
        }

        public string FilePath { get; }
        private Dictionary<FileInfoType, byte[]> SubContents { get; set; }

        /// <summary>
        ///     Basic file info of the revit file
        /// </summary>
        public BasicFileInfo BasicFileInfo
        {
            get
            {
                if (null != _basicFileInfo)
                    return _basicFileInfo;
                if (null == SubContents)
                    return null;
                if (!SubContents.ContainsKey(FileInfoType.BasicFileInfo))
                    return null;
                var content = Encoding.UTF8.GetString(SubContents[FileInfoType.BasicFileInfo]);
                _basicFileInfo = new BasicFileInfo(content);
                return _basicFileInfo;
            }
        }

        /// <summary>
        ///     Get the embedded thumbnail
        /// </summary>
        public Image Thumbnail
        {
            get
            {
                if (null != _thumbnail)
                    return _thumbnail;
                if (null == SubContents)
                    return null;
                if (!SubContents.ContainsKey(FileInfoType.RevitPreview))
                    return null;
                var imageBytes = SubContents[FileInfoType.RevitPreview];
                _thumbnail = ImageUtils.ResolveImageFromBytes(imageBytes, ImageFormat.Png);
                return _thumbnail;
            }
        }

        /// <summary>
        ///     Retrieve a value indicating whether the input file a valid Revit file
        /// </summary>
        /// <returns></returns>
        public bool IsValidRevitFile
        {
            get
            {
                var extension = Path.GetExtension(FilePath)?.ToLower();
                switch (extension)
                {
                    //Revit family file
                    case ".rfa":
                        return true;
                    //Revit project file
                    case ".rvt":
                        return true;
                    //Revit project template file
                    case ".rte":
                        return true;
                    //Revit family template file
                    case ".rft":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        ///     Get the length of the file in byte
        /// </summary>
        /// <returns>The length of the file in byte</returns>
        public long GetFileSize()
        {
            var fileInfo = new FileInfo(FilePath);
            return fileInfo.Length;
        }

        /// <summary>
        ///     Get the length of the file in specified unit
        /// </summary>
        /// <param name="fileSizeUnit">The unit type</param>
        /// <returns>The size of the file in the specified unit</returns>
        public double GetFileSize(StorageUnit fileSizeUnit)
        {
            var byteSize = GetFileSize();
            var power = (int) fileSizeUnit;
            return byteSize/Math.Pow(1024, power);
        }

        /// <summary>
        ///     Read the ole structure of the file and save all the streams
        /// </summary>
        /// <returns>A dictionary to reach the content</returns>
        private void GetAllContents()
        {
            //Open the OLE storage of the file
            var storageInfo = OleUtils.InvokeStorageRootMethod(null, "Open", FilePath, FileMode.Open, FileAccess.Read,
                FileShare.Read);
            if (null == storageInfo)
                return;
            try
            {
                SubContents = new Dictionary<FileInfoType, byte[]>();
                //Read all streams from the OLE storage
                var streamInfos = storageInfo.GetStreams();
                foreach (var streamInfo in streamInfos)
                    SubContents.Add(RevitFileUtils.ResolveFileInfoType(streamInfo.Name),
                        StreamUtils.StreamToBytes(streamInfo.GetStream()));
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                //Close the OLE storage of the file
                OleUtils.InvokeStorageRootMethod(storageInfo, "Close");
            }
        }
    }
}