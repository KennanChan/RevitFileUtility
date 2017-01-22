using System.Linq;

namespace Kennan.Revit.File.Utility
{
    internal static class StringUtils
    {
        internal static string TakeNumbers(string source)
        {
            var characters = source.ToCharArray();
            var numbers = characters.Where(character => character >= '0' && character <= '9');
            return string.Join("", numbers);
        }
    }
}