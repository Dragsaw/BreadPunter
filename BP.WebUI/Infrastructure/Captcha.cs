using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BP.WebUI.Infrastructure
{
    public static class Captcha
    {
        private static readonly int captchaLength = 4, width = 200, height = 70;
        private static readonly Random random = new Random();

        public static byte[] Generate()
        {
            string captchaString = random.Next(1000, 9999).ToString();
            HttpContext.Current.Session["captcha"] = captchaString;

            Bitmap image = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Matrix matrix = new Matrix();

            for (int i = 0; i < captchaLength; i++)
            {
                matrix.Reset();
                int x = (width / captchaLength) * i;
                int y = height / 2;
                matrix.RotateAt(random.Next(-40, 40), new PointF(x, y));
                graphics.Transform = matrix;
                graphics.DrawString(captchaString[i].ToString(),
                    new Font(FontFamily.GenericSerif, random.Next(25, 40)),
                    new SolidBrush(Color.FromArgb(random.Next(100), random.Next(100), random.Next(100))),
                    x, random.Next(5, 10));
            }

            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);

            return ms.GetBuffer();
        }
    }
}