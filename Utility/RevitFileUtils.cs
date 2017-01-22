using System;
using System.Linq;
using Kennan.Revit.File.Enumeration;

namespace Kennan.Revit.File.Utility
{
    internal static class RevitFileUtils
    {
        #region Internal Methods

        /// <summary>
        ///     Resolve a RevitVersion object from a revit version string
        /// </summary>
        /// <param name="revitVersionString">
        ///     A string retrieved from revit ole structure which would be formatted like :
        ///     "Autodesk Revit 2016 (Build: 20150220_1215(x64))"
        /// </param>
        /// <returns>A RevitVersion object</returns>
        internal static RevitVersion ResolveRevitVersion(string revitVersionString)
        {
            if (string.IsNullOrEmpty(revitVersionString))
                return RevitVersion.Unknown;
            var versionNumber = StringUtils.TakeNumbers(revitVersionString).Substring(0, 4);
            int number;
            if (!int.TryParse(versionNumber, out number)) return RevitVersion.Unknown;
            var validNumbers = Enum.GetValues(typeof (RevitVersion));
            if (validNumbers.Cast<int>().Any(value => value == number))
                return (RevitVersion) number;
            return RevitVersion.Unknown;
        }

        /// <summary>
        ///     Resolve a ProductType object from a revit version string
        /// </summary>
        /// <param name="revitVersionString">
        ///     A string retrieved from revit ole structure which would be formatted like : "Autodesk
        ///     Revit 2016 (Build: 20150220_1215(x64))"
        /// </param>
        /// <returns>A ProductType object</returns>
        internal static ProductType ResolveProductType(string revitVersionString)
        {
            if (string.IsNullOrEmpty(revitVersionString))
                return ProductType.Unknown;
            var partStrings = revitVersionString.Split(' ');
            for (var i = 0; i < partStrings.Length; i++)
            {
                int version;
                if (!int.TryParse(partStrings[i], out version))
                    continue;
                if (i - 1 < 0) continue;
                var partString = partStrings[i - 1];
                ProductType productType;
                if (Enum.TryParse(partString, out productType))
                    return productType;
            }
            return ProductType.Unknown;
        }

        /// <summary>
        ///     Map a key string to a FileInfoKey object
        /// </summary>
        /// <param name="keyString">A string representing a key</param>
        /// <returns>A FileInfoKey object representing a key</returns>
        internal static FileInfoKey ResolveFileInfoKey(string keyString)
        {
            var completeString =
                Enum.GetNames(typeof (FileInfoKey))
                    .FirstOrDefault(name => keyString.Replace(" ", string.Empty).ToLower().Contains(name.ToLower()));
            if (string.IsNullOrEmpty(completeString))
                return FileInfoKey.Invalid;
            return (FileInfoKey) Enum.Parse(typeof (FileInfoKey), completeString);
        }

        /// <summary>
        ///     Map a key string to a FileInfoType object
        /// </summary>
        /// <param name="keyString">A string representing a key</param>
        /// <returns>A FileInfoType object representing a key</returns>
        internal static FileInfoType ResolveFileInfoType(string keyString)
        {
            var completeString =
                Enum.GetNames(typeof (FileInfoType))
                    .FirstOrDefault(name => name.ToLower().Contains(keyString.ToLower()));
            if (string.IsNullOrEmpty(completeString))
                return FileInfoType.Invalid;
            return (FileInfoType) Enum.Parse(typeof (FileInfoType), completeString);
        }

        #endregion
    }
}