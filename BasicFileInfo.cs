using System;
using System.Collections.Generic;
using Kennan.Revit.File.Enumeration;
using Kennan.Revit.File.Utility;

namespace Kennan.Revit.File
{
    public class BasicFileInfo
    {
        private ProductType _productType = ProductType.Unknown;
        private RevitVersion _version = RevitVersion.Unknown;

        public BasicFileInfo(string revitBasicFileInfoString)
        {
            FileInfoPair = new Dictionary<FileInfoKey, string>();
            Initialize(revitBasicFileInfoString);
        }

        private Dictionary<FileInfoKey, string> FileInfoPair { get; }

        public RevitVersion Version
        {
            get
            {
                if (_version != RevitVersion.Unknown)
                    return _version;
                var value = FileInfoPair[FileInfoKey.RevitBuild];
                _version = RevitFileUtils.ResolveRevitVersion(value);
                return _version;
            }
        }

        public string VersionString => LabelUtils.GetLabelForRevitVersion(Version);

        public ProductType ProductType
        {
            get
            {
                if (_productType != ProductType.Unknown)
                    return _productType;
                var value = FileInfoPair[FileInfoKey.RevitBuild];
                _productType = RevitFileUtils.ResolveProductType(value);
                return _productType;
            }
        }

        public bool IsWorkSharing
        {
            get
            {
                const string notEnabled = "Not Enabled";
                var value = FileInfoPair[FileInfoKey.Worksharing];
                return value.Equals(notEnabled, StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public string UserName
        {
            get
            {
                var value = FileInfoPair[FileInfoKey.Username];
                return value;
            }
        }

        private void Initialize(string revitBasicFileInfoString)
        {
            if (string.IsNullOrEmpty(revitBasicFileInfoString))
                return;
            //Clear disturbs
            var basicFileInfoString = revitBasicFileInfoString.Replace("\0", string.Empty);
            //Read lines
            var detailInfoLines = basicFileInfoString.Split(new[] {"\r\n"}, new StringSplitOptions());
            foreach (var detailInfoLine in detailInfoLines)
            {
                //Divide by ":"
                var index = detailInfoLine.IndexOf(":", StringComparison.Ordinal);
                if (index < 0)
                    continue;
                var infoKey = detailInfoLine.Substring(0, index).Trim();
                var fileInfoKey = RevitFileUtils.ResolveFileInfoKey(infoKey);
                if (fileInfoKey == FileInfoKey.Invalid)
                    continue;
                var infoValue = detailInfoLine.Substring(index + 1).Trim();
                FileInfoPair.Add(fileInfoKey, infoValue);
            }
        }
    }
}