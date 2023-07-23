using System.Drawing;
using System.Text;
using JMSoftware.AsciiConversion;
using JMSoftware.ImageHelper;

public static class Program
{
    public static void Main(string[] arguments)
    {
        if (arguments != null && arguments.Length == 3)
        {
            var sourcePath = arguments[0];
            var width = int.Parse(arguments[1]);
            var height = int.Parse(arguments[2]);
            DrawImage(sourcePath, width, height);
        }
    }

    private static void DrawImage(string imagePath, int conWidth, int conHeight)
    {
        try
        {
            Image image;

            using (var loadedImage = Image.FromFile(imagePath))
            {
                var size = new Size(loadedImage.Width, loadedImage.Height);
                image = new Bitmap(size.Width, size.Height);

                using (var g = Graphics.FromImage(image))
                {
                    g.Clear(Color.White);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(loadedImage, 0, 0, size.Width, size.Height);
                }
            }

            Console.OutputEncoding = Encoding.UTF8;
            
            if (conHeight > conWidth / 2)
                conHeight = conWidth / 2;
            
            var lines = DoConvertConsole(image, conWidth, conHeight);
            foreach (var line in lines)
            {
                byte[] bytes = Encoding.Default.GetBytes(line);
                var utf8line = Encoding.UTF8.GetString(bytes);
                Console.WriteLine(utf8line);
            }
        }
        catch (OutOfMemoryException)
        {
            Console.WriteLine("Unknown or Unsupported File");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found");
        }
    }

    private static string[] DoConvertConsole(Image image, int conWidth, int conHeight)
    {
        var values = ImageToValues.Convert(
            image,
            new Size(conWidth, conHeight),
            JMSoftware.Matrices.Identity(),
            new Rectangle(0, 0, image.Width, image.Height));
            
        // var ramp = string.Join(string.Empty, "$@B%8&WM*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\\|()1{}[]?-_+~<>i!lI;:,\"^`'.".Reverse());
        var ramp = string.Join(string.Empty, " █▓▒░ ".Reverse());

        var valuesToText = new ValuesToFixedWidthTextConverter(ramp);
        var lines = valuesToText.Apply(values);

        return lines;
    }
}