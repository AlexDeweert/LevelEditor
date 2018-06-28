using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LevelEditor
{
    class Merge
    {
        public Merge()
        { }

        public Bitmap MergeTwoImages(Image imageOne, Image imageTwo)
        {
            if (imageOne == null || imageTwo == null) throw new ArgumentNullException("One of the images called by MergeTwoImages is NULL!");

            int outputWidth = (int)(imageOne.Width + imageTwo.Width);
            int outputHeight = (int)imageOne.Height;

            Bitmap outputImage = new Bitmap(outputWidth, outputHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(outputImage))
            {
                graphics.DrawImage(imageOne, new Rectangle(new Point(), imageOne.Size), new Rectangle(new Point(), imageOne.Size), GraphicsUnit.Pixel);
                graphics.DrawImage(imageTwo, new Rectangle(new Point(0, imageOne.Height + 1), imageTwo.Size), new Rectangle(new Point(), imageTwo.Size), GraphicsUnit.Pixel);
            }

            return outputImage;
        }
    }

    
}
