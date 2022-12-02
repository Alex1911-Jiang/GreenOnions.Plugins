namespace Copier
{ 
    public static class Program
    {
        public static void Main()
        {
            string[] firstDir = Directory.GetDirectories(Environment.CurrentDirectory);
            for (int i = 0; i < firstDir.Length; i++)
            {
                Copy(firstDir, i, "Debug");
                Copy(firstDir, i, "Release");
            }
        }

        private static void Copy(string[] firstDir, int i, string debugOrRelease)
        {
            string itemDebugPath = Path.Combine(firstDir[i], "bin", debugOrRelease);
            if (Directory.Exists(itemDebugPath))
            {
                string? projectName = Path.GetFileName(firstDir[i]);
                string? buildPath = Directory.GetDirectories(itemDebugPath).FirstOrDefault();
                if (buildPath != null)
                {
                    string toPath = Path.Combine(Environment.CurrentDirectory, "bin", debugOrRelease, projectName);
                    if (!Directory.Exists(toPath))
                        Directory.CreateDirectory(toPath);
                    CopyFiles(buildPath, toPath);
                }
            }
        }

        private static void CopyFiles(string from, string to)
        {
            string[] itemDirs =  Directory.GetDirectories(from);
            for (int i = 0; i < itemDirs.Length; i++)
            {
                string nameWithOutPath = Path.GetFileNameWithoutExtension(itemDirs[i]);
                CopyFiles(itemDirs[i], Path.Combine(to, nameWithOutPath));
            }
            string[] itemFiles = Directory.GetFiles(from);
            for (int i = 0; i < itemFiles.Length; i++)
            {
                string fileName = Path.GetFileName(itemFiles[i]);
                if (!Directory.Exists(to))
                    Directory.CreateDirectory(to);
                File.Copy(itemFiles[i], Path.Combine(to, fileName), true);
            }
        }
    }
}