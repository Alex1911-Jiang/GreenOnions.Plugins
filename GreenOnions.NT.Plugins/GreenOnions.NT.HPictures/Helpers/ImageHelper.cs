using GreenOnions.NT.Base;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace GreenOnions.NT.HPictures.Helpers
{
    internal static class ImageHelper
    {
        internal static byte[] AntiShielding(this byte[] imgData)
        {
            try
            {
                using Image img = Image.Load(imgData);
                using Image<Rgba32> bmp = img.CloneAs<Rgba32>();
                bmp.AntiShielding();
                using MemoryStream ms = new MemoryStream();
                bmp.SaveAsPng(ms);
                byte[] result = ms.ToArray();
                LogHelper.LogMessage("反和谐图片成功");
                if (SngletonInstance.Config?.DebugMode == true)
                    File.WriteAllBytes("反和谐.png", result);
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "反和谐图片失败");
                return imgData;
            }
        }

        internal static void AntiShielding(this Image<Rgba32> bmp)
        {
            for (int i = 0; i < 4; i++)
            {
                Random r = new Random(Guid.NewGuid().GetHashCode());
                int x = r.Next(0, bmp.Width);
                int y = r.Next(0, bmp.Height);
                int offset = r.Next(1, 6);
                var pixel = bmp[x, y];
                bmp[x, y] = new Rgba32(pixel.R, pixel.G, pixel.B, pixel.A + (pixel.A > 128 ? -offset : offset));
            }
        }
    }
}
