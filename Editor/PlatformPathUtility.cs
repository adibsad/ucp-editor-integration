using System.IO;
using System.Diagnostics;

namespace Hackerzhuli.Code.Editor
{
    internal static class PlatformPathUtility
    {
        public static string GetRealPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

#if UNITY_EDITOR_WIN
            return path;
#elif UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
            try
            {
                return ResolveUnixSymlink(path);
            }
            catch
            {
                return path;
            }
#else
            return path;
#endif
        }

        private static string ResolveUnixSymlink(string path)
        {
            var proc = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "readlink",
                    Arguments = $"-f \"{path}\"", // -f follows all symlinks to the final target
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            } ;

            proc.Start();
            string output = proc.StandardOutput.ReadToEnd().Trim();
            proc.WaitForExit();

            return string.IsNullOrEmpty(output) ? path : output;
        }
    }
}