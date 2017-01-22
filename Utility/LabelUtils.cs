using System;
using Kennan.Revit.File.Enumeration;

namespace Kennan.Revit.File.Utility
{
    public static class LabelUtils
    {
        public static string GetLabelForRevitProductType(ProductType productType)
        {
            switch (productType)
            {
                case ProductType.Architecture:
                    return "Revit Architecture";
                case ProductType.MEP:
                    return "Revit MEP";
                case ProductType.Revit:
                    return "Revit";
                case ProductType.Structure:
                    return "Revit Structure";
                default:
                    return "Unknown ProductType";
            }
        }

        public static string GetLabelForRevitVersion(RevitVersion revitVersion)
        {
            var label = Enum.GetName(typeof (RevitVersion), revitVersion);
            return label ?? "";
        }

        public static string GetLabelForStorageUnit(StorageUnit storageUnit)
        {
            switch (storageUnit)
            {
                case StorageUnit.Byte:
                    return "B";
                case StorageUnit.KByte:
                    return "KB";
                case StorageUnit.MByte:
                    return "MB";
                case StorageUnit.GByte:
                    return "GB";
                case StorageUnit.TByte:
                    return "TB";
                default:
                    return "Unknown";
            }
        }

        internal static string GetLabelForFileInfoType(FileInfoType type)
        {
            var label = Enum.GetName(typeof (FileInfoType), type);
            return label ?? "";
        }

        internal static string GetLabelForFileInfoKey(FileInfoKey key)
        {
            var label = Enum.GetName(typeof (FileInfoKey), key);
            return label ?? "";
        }
    }
}