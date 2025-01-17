using System.IO;
using System.Windows.Media.Imaging;
using WpfMath.Rendering;

namespace WpfMath
{
    public static class Extensions
    {
        public static byte[] RenderToPng(this TexFormula texForm,
            double scale,
            double x,
            double y,
            string systemTextFontName)
        {
            var environment = WpfTeXEnvironment.Create(scale: scale, systemTextFontName: systemTextFontName);
            BitmapSource image = texForm.RenderToBitmap(environment, scale, x, y);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }
    }
}
