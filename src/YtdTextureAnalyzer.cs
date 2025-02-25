using System;
using System.IO;
using System.Linq;
using CodeWalker.GameFiles;

namespace YtdTextureAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args.Contains("-h") || args.Contains("--help"))
            {
                Console.WriteLine("Usage: YtdTextureAnalyzer <ytd_file> [--fix-script-rt]");
                Console.WriteLine("\nOptions:");
                Console.WriteLine("  -h, --help         Show this help message");
                Console.WriteLine("  --fix-script-rt    Fix script_rt textures by converting them to A8R8G8B8 format with 1 mipmap");
                return;
            }

            string filePath = args.FirstOrDefault(arg => !arg.StartsWith("-")) ?? string.Empty;
            bool fixScriptRT = args.Contains("--fix-script-rt");

            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Error: No valid YTD file provided.");
                return;
            }
            
            AnalyzeYTD(filePath, fixScriptRT);
        }

        static void AnalyzeYTD(string filePath, bool fixScriptRT)
        {
            YtdFile ytd = new YtdFile();
            
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File not found - {filePath}");
                return;
            }

            byte[] fileData = File.ReadAllBytes(filePath);
            ytd.Load(fileData);

            if (ytd.TextureDict == null || ytd.TextureDict.Textures == null)
            {
                Console.WriteLine("No textures found in this YTD file.");
                return;
            }

            int textureCount = ytd.TextureDict.Textures.Count;
            long totalSize = 0;
            bool modified = false;

            string foundTexturesMessage = $"Found {textureCount} textures in {filePath}:";
            
            for (int i = 0; i < textureCount; i++)  
            {
                var texture = ytd.TextureDict.Textures[i]; 

                if (texture == null)
                    continue;

                string name = texture.Name;
                string compression = texture.Format.ToString().Replace("D3DFMT_", "");
                long fileSize = texture.Data?.BlockLength ?? 0;
                double fileSizeMB = fileSize / (1024.0 * 1024.0);
                int width = texture.Width;
                int height = texture.Height;
                int mipmapLevels = texture.Levels;
                totalSize += fileSize;

                // Fix script_rt textures only if format or mipmaps are incorrect
                if (fixScriptRT && name.StartsWith("script_rt") && (texture.Format != TextureFormat.D3DFMT_A8R8G8B8 || texture.Levels != 1))
                {
                    texture.Format = TextureFormat.D3DFMT_A8R8G8B8;
                    texture.Levels = 1;
                    Console.WriteLine($"[Fixed] Texture: {name} updated to A8R8G8B8 with 1 Mipmap");
                    modified = true;
                }

                if (!fixScriptRT || name.StartsWith("script_rt"))
                {
                    Console.WriteLine($"Texture: {name}, Compression: {compression}, Resolution: {width}x{height}, Mipmaps: {mipmapLevels}, File Size: {fileSize} bytes ({fileSizeMB:F2} MB)");
                }
            }

            double totalSizeMB = totalSize / (1024.0 * 1024.0);
            
            Console.WriteLine(foundTexturesMessage);
            Console.WriteLine($"Total Texture Data Size: {totalSize} bytes ({totalSizeMB:F2} MB)");
            
            // Overwrite the original YTD file only if changes were made
            if (fixScriptRT && modified)
            {
                byte[] newFileData = ytd.Save();
                File.WriteAllBytes(filePath, newFileData);
                Console.WriteLine($"\n[✅] Modified YTD file overwritten: {filePath}");
            }
            else if (fixScriptRT)
            {
                Console.WriteLine("\n[ℹ] No fixes were needed for this YTD file.");
            }
        }
    }
}
