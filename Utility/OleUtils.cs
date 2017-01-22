using System;
using System.IO.Packaging;
using System.Reflection;

namespace Kennan.Revit.File.Utility
{
    /// <summary>
    ///     Inspired by the soruce code of the specific article:
    ///     http://thebuildingcoder.typepad.com/blog/2010/06/open-revit-ole-storage.html
    /// </summary>
    internal class OleUtils
    {
        /// <summary>
        ///     Invoke a method of System.IO.Packaging.StorageRoot using reflection
        /// </summary>
        /// <param name="storageRoot">The object to invoke the method</param>
        /// <param name="methodName">The name of the method</param>
        /// <param name="methodArgs">Arguments to invoke the method，here we need FileName,FileMode，FileAccess，FileShare</param>
        /// <returns>A StorageInfo object</returns>
        internal static StorageInfo InvokeStorageRootMethod(StorageInfo storageRoot, string methodName,
            params object[] methodArgs)
        {
            try
            {
                const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public |
                                                  BindingFlags.NonPublic | BindingFlags.InvokeMethod;
                var storageRootType = typeof (StorageInfo).Assembly.GetType("System.IO.Packaging.StorageRoot", true,
                    false);
                var result = storageRootType.InvokeMember(methodName, bindingFlags, null, storageRoot, methodArgs);
                return result as StorageInfo;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}