using System.IO.Compression;

namespace Copier
{
    public static class Program
    {
        private static bool _zip = false;
        public static void Main()
        {
            Console.WriteLine("输入 Y 拷贝完成后自动压缩，输入其他任意键仅拷贝");
            string? read = Console.ReadLine();
            if (read == "y" || read == "Y")
                _zip = true;

            string[] firstDir = Directory.GetDirectories(Environment.CurrentDirectory);
            for (int i = 0; i < firstDir.Length; i++)
            {
                string projectName = new DirectoryInfo(firstDir[i]).Name;
                if (projectName == "Copier" || projectName == "GreenOnions.PluginTest" || projectName == "GreenOnions.PluginConfigs")
                    continue;
                Copy(firstDir, i, "Debug");
                Copy(firstDir, i, "Release");
            }
            Console.WriteLine($"所有拷贝已完成");
        }

        private static void Copy(string[] firstDir, int i, string debugOrRelease)
        {
            string itemDebugPath = Path.Combine(firstDir[i], "bin", debugOrRelease);
            if (Directory.Exists(itemDebugPath))
            {
                string? projectName = Path.GetFileName(firstDir[i]);
                string? buildPath = Directory.GetDirectories(itemDebugPath).OrderByDescending(p => p.Length).FirstOrDefault();
                if (buildPath != null)
                {
                    string toPath = Path.Combine(Environment.CurrentDirectory, "bin", debugOrRelease, projectName);
                    if (!Directory.Exists(toPath))
                        Directory.CreateDirectory(toPath);
                    Console.WriteLine($"正在拷贝{projectName}");
                    CopyFiles(buildPath, toPath);

                    string zipName = $"{toPath}.zip";
                    if (File.Exists(zipName))
                        File.Delete(zipName);
                    if (_zip)
                    {
                        Console.WriteLine($"正在压缩{projectName}.zip");
                        ZipFile.CreateFromDirectory(toPath, zipName, CompressionLevel.SmallestSize, true);
                    }
                }
            }
        }

        private static void CopyFiles(string from, string to)
        {
            Directory.CreateDirectory(to);
            foreach (var item in Directory.GetDirectories(from))
                CopyFiles(item, Path.Combine(to, new DirectoryInfo(item).Name));
            foreach (var item in Directory.GetFiles(from))
                File.Copy(item, Path.Combine(to, Path.GetFileName(item)), true);
        }
    }
}