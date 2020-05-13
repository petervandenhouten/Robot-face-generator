using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GalaxyFootball
{
    public class RobotGenerator
    {
        // StartFace must be 24-bits bitmap (not an indexed bitmap)

        static private Random _rnd = new Random();
        static private string[] _eyes = null;
        static private string[] _noses = null;
        static private string[] _mouths = null;
        static private string[] _ears = null;
        static private string[] _jaws = null;
        static private string[] _hairs = null;
        static private string[] _eyebrows = null;
        static private string[] _oneeyes = null;
        static private string[] _antennas = null;

        static private int _eyes_offset;

        private static string _imagePath = @"..\Resources\Images\RobotGenerator\";

        // input images
        static public void SetImagePath(string path)
        {
            _imagePath = path;
        }

        static public Bitmap GenerateFace(Color color1, Color color2, Color color3)
        {
            var bmp = GenerateFace();
            bmp = ChangeClubColors(bmp, color1, color2, color3);
            return bmp;
        }

        static public Bitmap GenerateFace()
        {
            LoadEyes();
            LoadOneEyes();
            LoadNoses();
            LoadMouths();
            LoadEars();
            LoadJaws();
            LoadHairs();
            LoadEyebrows();
            LoadAntennas();

            string filename = _imagePath + @"StartFace1.bmp";
            if (File.Exists(filename) == true)
            {
                Bitmap face = new Bitmap(filename);

                _eyes_offset = -1;
                
                Graphics g = Graphics.FromImage(face);

                DrawJaw(g);

                if (_rnd.NextDouble() > 0.3)
                {
                    DrawEars(g);
                    if (_rnd.NextDouble() > 0.95)
                    {
                        DrawAntenna(g);
                    }
                }
                else if (_rnd.NextDouble() > 0.3)
                {
                    DrawAntenna(g);
                }

                if (_rnd.NextDouble() > 0.4) DrawEyebrow(g);

                if (_rnd.NextDouble() > 0.5)
                {
                    DrawOneEye(g);
                }
                else
                {
                    DrawEyes(g);
                }
                DrawNose(g);
                DrawMouth(g);
                
                DrawHair(g);

                ChangeSkinColor(face);

                return face;
            }
            return null;
        }

        private static void ChangeSkinColor(Bitmap bmp)
        {
            Color OrgDarkSkinColor   = Color.FromArgb(160, 160, 160);
            Color OrgMediumSkinColor = Color.FromArgb(180, 180, 180);
            Color OrgLightSkinColor  = Color.FromArgb(200, 200, 200);

            Color newDarSkinColor;
            Color newMediumSkinColor;
            Color newLightSkinColor;

            GetRandomColorSet(out newDarSkinColor, out newMediumSkinColor, out newLightSkinColor);

            ReplaceColor(bmp, OrgDarkSkinColor,   newDarSkinColor);
            ReplaceColor(bmp, OrgMediumSkinColor, newMediumSkinColor);
            ReplaceColor(bmp, OrgLightSkinColor,  newLightSkinColor);
            
        }

        private static void GetRandomColorSet(out Color DarkSkinColor,out Color MediumSkinColor, out Color LightSkinColor)
        {
            int x = (int)(_rnd.NextDouble() * 10);

            switch(x)
            {
                case 0: // coral
                    DarkSkinColor = Color.DeepPink;
                    MediumSkinColor = Color.Coral;
                    LightSkinColor = Color.LightCoral;
                    break;

                case 1: // gold
                    DarkSkinColor   = Color.DarkGoldenrod;
                    MediumSkinColor = Color.Gold;
                    LightSkinColor = Color.LightGoldenrodYellow;
                    break;

                case 2: // silver
                    DarkSkinColor   = Color.DarkSlateGray;
                    MediumSkinColor = Color.SlateGray;
                    LightSkinColor  = Color.LightSlateGray;
                    break;

                case 3: // red white blue
                    DarkSkinColor = Color.Pink;
                    MediumSkinColor = Color.White;
                    LightSkinColor = Color.LightPink;
                    break;

                case 4: // darker gray
                    DarkSkinColor = Color.FromArgb(140, 140, 140);
                    MediumSkinColor = Color.FromArgb(155, 155, 155);
                    LightSkinColor = Color.FromArgb(185, 185, 185);
                    break;

                case 5: // mid dark gray
                    DarkSkinColor   = Color.FromArgb(150,150,150); 
                    MediumSkinColor = Color.FromArgb(159,159,159);
                    LightSkinColor = Color.FromArgb(171, 171, 171);
                    break;

                case 6: // lighter gray
                    DarkSkinColor = Color.FromArgb(165, 165, 165);
                    MediumSkinColor = Color.FromArgb(185, 185, 185);
                    LightSkinColor = Color.FromArgb(205, 205, 205);
                    break;

                case 7: // cyan
                    DarkSkinColor = Color.DarkCyan;
                    MediumSkinColor = Color.Cyan;
                    LightSkinColor = Color.LightCyan;
                    break;

                case 8: // blueish light gray
                    DarkSkinColor = Color.FromArgb(165, 165, 175);
                    MediumSkinColor = Color.FromArgb(185, 185, 195);
                    LightSkinColor = Color.FromArgb(205, 205, 215);
                    break;

                default:
                    DarkSkinColor   = Color.FromArgb(155,155,155); 
                    MediumSkinColor = Color.FromArgb(175,175,175);
                    LightSkinColor = Color.FromArgb(195, 195, 195);
                    break;
            }

        }

        private static void DrawHair(Graphics g)
        {
            Color TransparentColor = Color.FromArgb(255, 0, 255);
            string filename = GetRandomHair();
            Bitmap hair = new Bitmap(filename);
            hair.MakeTransparent(TransparentColor);

            // give hair colour
            Color ReplaceHairColorDark = Color.FromArgb(0, 200, 200);
            Color ReplaceHairColorLight = Color.FromArgb(0, 255, 255);

            Color newhairColorLight;
            Color newhairColorDark;
            Color newDumymColor;

            GetRandomColorSet(out newhairColorDark, out newhairColorLight, out newDumymColor);

            ReplaceColor(hair, ReplaceHairColorLight, newhairColorLight);
            ReplaceColor(hair, ReplaceHairColorDark, newhairColorDark);

            int middleX = (int)g.VisibleClipBounds.Width / 2;
            int y_ear = 120;
            int y = y_ear - hair.Height;
            int x = middleX - hair.Width / 2;
            g.DrawImage(hair, x, y);
        }

        private static void DrawAntenna(Graphics g)
        {
            Color TransparentColor = Color.FromArgb(255, 0, 255);
            string filename = GetRandomAntenna();
            Bitmap antenna = new Bitmap(filename);
            antenna.MakeTransparent(TransparentColor);

            int middleX = (int)g.VisibleClipBounds.Width / 2;
            int y = 0;
            int x = middleX - antenna.Width / 2;
            g.DrawImage(antenna, x, y);
        }

        private static void DrawNose(Graphics g)
        {  
            Color TransparentColor = Color.FromArgb(255, 0, 255);
            string filename = GetRandomNose();
            Bitmap nose = new Bitmap(filename);
            nose.MakeTransparent(TransparentColor);

            int middleX = (int)g.VisibleClipBounds.Width / 2;
            int nose_x_offset = GetRandom(-1, 1);
            int nose_y_offset = GetRandom(-2, 2);

            DrawImageCentered(g, nose, middleX - nose_x_offset, 125 + nose_y_offset);
        }

        private static void DrawEyebrow(Graphics g)
        {
            Color TransparentColor = Color.FromArgb(255, 0, 255);
            string filename = GetRandomImage(_eyebrows);
            Bitmap eyebrow_left = new Bitmap(filename);
            eyebrow_left.MakeTransparent(TransparentColor);

            Bitmap eyebrow_right = new Bitmap(eyebrow_left);
            eyebrow_right.RotateFlip(RotateFlipType.RotateNoneFlipX);

            int middleX = (int)g.VisibleClipBounds.Width / 2;
            int Y = 100;

            int eyebrow_y_offset = GetRandom(-2, 2);
            int eyebrow_x_offset = GetRandom(-2, 2);

            int eyes_offset = _eyes_offset;
            if (eyes_offset < 0)
            {
                eyes_offset = GetRandom(18, 24);
                _eyes_offset = eyes_offset;
            }

            DrawImageCentered(g, eyebrow_left, middleX - eyes_offset + eyebrow_x_offset, Y + eyebrow_y_offset);
            DrawImageCentered(g, eyebrow_right, middleX + eyes_offset + eyebrow_x_offset, Y + eyebrow_y_offset);
        }



        private static void DrawJaw(Graphics g)
        {
            Color TransparentColor = Color.FromArgb(255, 0, 255);
            string filename = GetRandomJaw();
            if (filename.Length != 0)
            {
                Bitmap jaw = new Bitmap(filename);
                jaw.MakeTransparent(TransparentColor);

                int middleX = (int)g.VisibleClipBounds.Width / 2;
                int yshoulder = 176;
                int y = yshoulder - jaw.Height;
                int x = middleX - jaw.Width / 2;
                g.DrawImage(jaw, x, y);
                //DrawImageCentered(g, jaw, middleX - x_offset, 150 + y_offset);
            }
        }

        private static void DrawMouth(Graphics g)
        {
            Color TransparentColor = Color.FromArgb(255, 0, 255);
            string filename = GetRandomMouth();
            Bitmap mouth = new Bitmap(filename);
            mouth.MakeTransparent(TransparentColor);

            int middleX = (int)g.VisibleClipBounds.Width / 2;
            int x_offset = GetRandom(-2, 2);
            int y_offset = GetRandom(0, 7);

            DrawImageCentered(g, mouth, middleX - x_offset, 148 + y_offset);
        }

        private static void DrawEars(Graphics g)
        {
            Color TransparentColor = Color.FromArgb(255, 0, 255);
            string filename = GetRandomEars();
            Bitmap ears = new Bitmap(filename);
            ears.MakeTransparent(TransparentColor);
            int middleX = (int)g.VisibleClipBounds.Width / 2;
            int y_offset = GetRandom(115,120);
            DrawImageCentered(g,ears, middleX, y_offset);
        }

        private static void DrawOneEye(Graphics g)
        {
            Color ReplaceEyeColor = Color.FromArgb(0, 255, 255);
            Color TransparentColor = Color.FromArgb(255, 0, 255);

            string filename = GetRandomOneEyes();

            int middleX = (int)g.VisibleClipBounds.Width / 2;

            Bitmap single_eye = new Bitmap(filename);
            single_eye.MakeTransparent(TransparentColor);

            ReplaceColor(single_eye, ReplaceEyeColor, GetRandomEyeColor());

            int eyes_offset = GetRandom(-6, 4);
            DrawImageCentered(g, single_eye, middleX, 108 - eyes_offset);
        }

        private static Color GetRandomEyeColor()
        {
            int i = _rnd.Next(10);

            switch (i)
            {
                case 0: return Color.LightCoral;
                case 1: return Color.DarkGreen;
                case 2: return Color.Green;
                case 3: return Color.Blue;
                case 4: return Color.DarkGoldenrod;
                case 5: return Color.DarkMagenta;
                case 6: return Color.DarkOrchid;
                case 7: return Color.DarkSeaGreen;
                case 8: return Color.DarkViolet;
                case 9: return Color.DeepPink;

                default:
                    return Color.Black;
            }
        }


        private static void DrawEyes(Graphics g)
        {
            Color ReplaceEyeColor = Color.FromArgb(0, 255, 255);
            Color TransparentColor = Color.FromArgb(255, 0, 255);

            string filename = GetRandomEyes();
            
            int middleX = (int)g.VisibleClipBounds.Width / 2;

            Bitmap eye_left = new Bitmap(filename);
            eye_left.MakeTransparent(TransparentColor);

            ReplaceColor(eye_left, ReplaceEyeColor, GetRandomEyeColor());

            Bitmap eye_right = new Bitmap(eye_left);
            eye_right.RotateFlip(RotateFlipType.RotateNoneFlipX);

            int eyes_offset = _eyes_offset;
            if ( eyes_offset < 0 )
            {
                eyes_offset  = GetRandom(18, 24);
                _eyes_offset = eyes_offset;
            }
           

            DrawImageCentered(g, eye_left, middleX - eyes_offset, 110);
            DrawImageCentered(g, eye_right, middleX + eyes_offset, 110);
               
        }

        private static void ReplaceColor(Bitmap b, Color oldColor, Color newColor)
        {
            const int colormargin = 2;

            for (int y = 0; y < b.Height; y++)
            {
                for (int x = 0; x < b.Width; x++)
                {
                    Color color1 = b.GetPixel(x,y);

                    int diff_r = Math.Abs(color1.R - oldColor.R);
                    int diff_g = Math.Abs(color1.G - oldColor.G);
                    int diff_b = Math.Abs(color1.B - oldColor.B);

                    if (diff_r < colormargin &&
                        diff_g < colormargin &&
                        diff_b < colormargin)
                    {
                        b.SetPixel(x, y, newColor);
                    }
                }
            }
        }

        private static string GetRandomAntenna()
        {
            int i = GetRandom(0, _antennas.Length);
            return _antennas[i];
        }

        private static string GetRandomHair()
        {
            int i = GetRandom(0, _hairs.Length);
            return _hairs[i];
        }

        private static string GetRandomEyes()
        {
            int i = GetRandom(0, _eyes.Length);
            return _eyes[i];
        }

        private static string GetRandomOneEyes()
        {
            int i = GetRandom(0, _oneeyes.Length);
            return _oneeyes[i];
        }

        private static string GetRandomNose()
        {
            int i = GetRandom(0, _noses.Length);
            return _noses[i];
        }

        private static string GetRandomEars()
        {
            int i = GetRandom(0, _ears.Length);
            return _ears[i];
        }

        private static string GetRandomMouth()
        {
            int i = GetRandom(0, _mouths.Length);
            return _mouths[i];
        }

        private static string GetRandomImage(string[] list)
        {
            int i = GetRandom(0, list.Length);
            return list[i];
        }

        private static string GetRandomJaw()
        {
            if (_jaws.Length == 0) return "";
            // if (GetRandom(0, 10) < 4) return ""; // use deault jaws
            int i = GetRandom(0, _jaws.Length);
            return _jaws[i];
        }

        static private void DrawImageCentered(Graphics g,Bitmap b, int x, int y)
        {
            int w = b.Width / 2;
            int h = b.Height / 2;

            g.DrawImage(b, x - w, y - h);
        }

        static int GetRandom(int minimum, int maximum)
        {
            return minimum + (int)(_rnd.NextDouble() * (maximum - minimum));
        }

        static bool LoadNoses()
        {
            string filter = @"nose*.bmp";
            _noses = Directory.GetFiles(_imagePath, filter);
            return true;
        }

        static bool LoadEyes()
        {
            string filter = @"eyes*.bmp";
            _eyes = Directory.GetFiles(_imagePath, filter);
            return true; 
        }

        private static void LoadEyebrows()
        {
            string filter = @"eyebrow*.bmp";
            _eyebrows = Directory.GetFiles(_imagePath, filter);
        }

        private static void LoadAntennas()
        {
            string filter = @"antenna*.bmp";
            _antennas = Directory.GetFiles(_imagePath, filter);
        }

        private static void LoadHairs()
        {
            string filter = @"hair*.bmp";
            _hairs = Directory.GetFiles(_imagePath, filter);
        }


        private static void LoadMouths()
        {
            string filter = @"mouth*.bmp";
            _mouths = Directory.GetFiles(_imagePath, filter);
        }

        private static void LoadEars()
        {
            string filter = @"ears*.bmp";
            _ears = Directory.GetFiles(_imagePath, filter);
        }

        private static void LoadJaws()
        {
            string filter = @"jaw*.bmp";
            _jaws = Directory.GetFiles(_imagePath, filter);
        }

        private static void LoadOneEyes()
        {
            string filter = @"oneeye*.bmp"; // not EYES!
            _oneeyes = Directory.GetFiles(_imagePath, filter);
        }

        public static void SaveJPG100(Bitmap bmp, string filename)
        {
            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            bmp.Save(filename, GetEncoder(ImageFormat.Jpeg), encoderParameters);
        }

        public static void SaveJPG100(Bitmap bmp, Stream stream)
        {
            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            bmp.Save(stream, GetEncoder(ImageFormat.Jpeg), encoderParameters);
        }

        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static Bitmap ChangeClubColors(Bitmap bmp, Color color1, Color color2, Color color3)
        {
            Color AlmostBlack = Color.FromArgb(1,1,1);

            if (IsBlack(color1)) color1=AlmostBlack;
            if (IsBlack(color2)) color2=AlmostBlack;
            if (IsBlack(color3)) color3=AlmostBlack;

            Graphics g = Graphics.FromImage(bmp);
            FloodFill(bmp, 13, 202, color1);
            FloodFill(bmp, 83, 202, color2);
            FloodFill(bmp, 155, 202, color3);
            return bmp;
        }

        private static bool IsBlack(Color c)
        {
            return (c.R == 0 && c.B == 0 && c.G == 0);
        }

        private static void FloodFill(Bitmap bitmap, int x, int y, Color color)
        {
            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int[] bits = new int[data.Stride / 4 * data.Height];
            Marshal.Copy(data.Scan0, bits, 0, bits.Length);

            LinkedList<Point> check = new LinkedList<Point>();
            int floodTo = color.ToArgb();
            int floodFrom = bits[x + y * data.Stride / 4];
            bits[x + y * data.Stride / 4] = floodTo;

            if (floodFrom != floodTo)
            {
                check.AddLast(new Point(x, y));
                while (check.Count > 0)
                {
                    Point cur = check.First.Value;
                    check.RemoveFirst();

                    foreach (Point off in new Point[] {
                new Point(0, -1), new Point(0, 1), 
                new Point(-1, 0), new Point(1, 0)})
                    {
                        Point next = new Point(cur.X + off.X, cur.Y + off.Y);
                        if (next.X >= 0 && next.Y >= 0 &&
                            next.X < data.Width &&
                            next.Y < data.Height)
                        {
                            if (bits[next.X + next.Y * data.Stride / 4] == floodFrom)
                            {
                                check.AddLast(next);
                                bits[next.X + next.Y * data.Stride / 4] = floodTo;
                            }
                        }
                    }
                }
            }

            Marshal.Copy(bits, 0, data.Scan0, bits.Length);
            bitmap.UnlockBits(data);
        }

    }
}
