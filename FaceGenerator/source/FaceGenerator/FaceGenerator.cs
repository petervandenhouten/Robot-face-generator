using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GalaxyFootball
{
    public class FaceGenerator
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

        static private int _eyes_offset;

        // const string _imagePath = @"FaceGenerator\";
        // const string _imagePath = @"..\..\Art\";
        const string _imagePath = @"..\Resources\Images\FaceGenerator\";
        //const string _imagePath = @"..\..\Resources\Images\RobotGenerator\";


        static public Bitmap GenerateFace()
        {
            LoadEyes();
            LoadNoses();
            LoadMouths();
            LoadEars();
            LoadJaws();
            LoadHairs();
            LoadEyebrows();

            string filename = _imagePath + @"StartFace3.bmp";
            if (File.Exists(filename) == true)
            {
                Bitmap face = new Bitmap(filename);

                _eyes_offset = -1;

                Graphics g = Graphics.FromImage(face);

                DrawJaw(g);
                DrawEars(g);
                DrawEyebrow(g);
                DrawEyes(g);
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
           // Color OrgDarkSkinColor1 = Color.FromArgb(240, 193, 170);
            // Color OrgLightSkinColor1 = Color.FromArgb(254, 217, 197);

            Color OrgDarkSkinColor2 = Color.FromArgb(218, 158, 142);
            Color OrgMediumSkinColor2 = Color.FromArgb(252, 207, 189);
            Color OrgLightSkinColor2 = Color.FromArgb(253, 232, 221);

            Random rand = new Random();

            Color newDarSkinColor;
            Color newMediumSkinColor;
            Color newLightSkinColor;

            int x = (int)(rand.NextDouble() * 10);

            GetSkinColor(x, out newDarSkinColor, out newMediumSkinColor, out newLightSkinColor);

           // ReplaceColor( bmp, OrgDarkSkinColor1, newDarSkinColor);
           // ReplaceColor( bmp, OrgLightSkinColor1, newLigthSkinColor );

            ReplaceColor(bmp, OrgDarkSkinColor2, newDarSkinColor);
            ReplaceColor(bmp, OrgMediumSkinColor2, newMediumSkinColor);
            ReplaceColor(bmp, OrgLightSkinColor2, newLightSkinColor);
            
        }

        private static void GetSkinColor(int x, out Color DarkSkinColor,out Color MediumSkinColor, out Color LightSkinColor)
        {
            switch(x)
            {
                case 0:
                    DarkSkinColor = Color.LightPink;
                    MediumSkinColor = Color.Pink;
                    LightSkinColor = Color.LightPink;
                    break;

                case 1: // dark brown
                    DarkSkinColor   = Color.FromArgb(130,87,65);
                    MediumSkinColor = Color.FromArgb(160, 117, 85);
                    LightSkinColor  = Color.FromArgb(195,153,107);
                    break;

                case 2: // latin
                    DarkSkinColor   = Color.FromArgb(188, 132, 104);
                    MediumSkinColor = Color.FromArgb(218, 162, 134);
                    LightSkinColor  = Color.FromArgb(238,182,154);
                    break;

                default:
                    DarkSkinColor   = Color.FromArgb(218, 158, 142); 
                    MediumSkinColor = Color.FromArgb(240, 193, 170);
                    LightSkinColor  = Color.FromArgb(254, 217, 197);
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
            Color ReplaceHairColor1 = Color.FromArgb(0, 255, 255);
            Color ReplaceHairColorDark = Color.FromArgb(0, 200, 200);
            ReplaceColor(hair, ReplaceHairColor1, Color.Goldenrod);
            ReplaceColor(hair, ReplaceHairColorDark, Color.DarkGoldenrod);


            int middleX = (int)g.VisibleClipBounds.Width / 2;
            int y_ear = 120;
            int y = y_ear - hair.Height;
            int x = middleX - hair.Width / 2;
            g.DrawImage(hair, x, y);
        }

        private static void DrawNose(Graphics g)
        {  
            Color TransparentColor = Color.FromArgb(255, 0, 255);
            string filename = GetRandomNose();
            Bitmap nose = new Bitmap(filename);
            nose.MakeTransparent(TransparentColor);

            int middleX = (int)g.VisibleClipBounds.Width / 2;
            int nose_x_offset = GetRandom(-1, 1);
            int nose_y_offset = GetRandom(-3, 3);

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
            int y_offset = GetRandom(0, 10);

            DrawImageCentered(g, mouth, middleX - x_offset, 150 + y_offset);
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

        private static void DrawEyes(Graphics g)
        {
            Color ReplaceEyeColor = Color.FromArgb(0, 255, 255);
            Color TransparentColor = Color.FromArgb(255, 0, 255);

            string filename = GetRandomEyes();
            
            int middleX = (int)g.VisibleClipBounds.Width / 2;

            Bitmap eye_left = new Bitmap(filename);
            eye_left.MakeTransparent(TransparentColor);


            ReplaceColor(eye_left, ReplaceEyeColor, Color.LightBlue);

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
            const int colormargin = 24;

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
    }
}
