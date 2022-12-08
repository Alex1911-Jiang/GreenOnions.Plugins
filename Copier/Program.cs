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
            Directory.CreateDirectory(to);
            foreach (var item in Directory.GetDirectories(from))
                CopyFiles(item, Path.Combine(to, item.Substring(item.LastIndexOf('\\') + 1)));
            foreach (var item in Directory.GetFiles(from))
                File.Copy(item, Path.Combine(to, Path.GetFileName(item)), true);
        }
    }
}