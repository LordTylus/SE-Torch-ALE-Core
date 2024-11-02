using System.IO;

namespace ALE_Core.Utils {
    public class FileUtils {

        public static string ToValidatedInput(string input) {

            foreach (var c in Path.GetInvalidFileNameChars()) 
                input = input.Replace(c, '_');

            return input;
        }
    }
}
