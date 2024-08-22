using System;
using System.Drawing;
using Zen.Barcode;

namespace Utils
{
    public static class BarCodeImage
    {
        public static Image GenerateH(string codigo)
        {
            return Generate(codigo, RotateFlipType.RotateNoneFlipNone);
        }

        public static Image GenerateV(string codigo)
        {
            return Generate(codigo, RotateFlipType.Rotate90FlipNone);
        }

        private static Image Generate(string codigo, RotateFlipType rotate)
        {
            Code39BarcodeDraw bdf = BarcodeDrawFactory.Code39WithoutChecksum;
            Image img = bdf.Draw(codigo, 200, 5);
            img.RotateFlip(rotate);
            return img;
        }
    }
}
