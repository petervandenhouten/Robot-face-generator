using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace GalaxyFootball
{
    public class PlanetGenerator
    {
        static private Random _rnd = new Random();
        static private string[] _skies = null;
        static private string[] _landscapes = null;
        static private string[] _buildings = null;

        // static private int _eyes_offset;

        private static string _imagePath = @"..\Resources\Images\PlanetGenerator\";

        // input images
        static public void SetImagePath(string path)
        {
            _imagePath = path;
        }

        static public Bitmap GeneratePlanet()
        {
            LoadSkies();
            LoadLandscapes();
            LoadBuildings();

            Bitmap planetView = new Bitmap(GetRandomSky());
            Graphics g = Graphics.FromImage(planetView);
            Drawlandscape(g);

            int nrTypeBuildings = 1+_rnd.Next(3);
            List<string> buildingForThisPlanet = new List<string>();
            for (int i = 0; i < nrTypeBuildings; i++)
            {
                buildingForThisPlanet.Add(GetRandomBuilding());
            }

            int nrRandomBlackBuildings = _rnd.Next(18);
            for (int i = 0; i < nrRandomBlackBuildings; i++)
            {
                DrawBuildingBlack(g);
            }

            for (int i = 0; i < buildingForThisPlanet.Count; i++)
            {
                int nrBlackBuildings = _rnd.Next(5);
                for (int j = 0; j < nrBlackBuildings; j++)
                {
                    DrawBuildingBlack(g, buildingForThisPlanet[i]);
                }
            }

            int nrBuildings = 1 + _rnd.Next(18);
            for (int j = 0; j < nrBuildings; j++)
            {
                int building = _rnd.Next(buildingForThisPlanet.Count);
                DrawBuilding(g, buildingForThisPlanet[building]);
            }

            g.Dispose();

            return planetView;
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

                case 5: // lighter gray
                    DarkSkinColor   = Color.FromArgb(165,165,165); 
                    MediumSkinColor = Color.FromArgb(185,185,185);
                    LightSkinColor = Color.FromArgb(205, 205, 205);
                    break;

                case 7: // cyan
                    DarkSkinColor = Color.DarkCyan;
                    MediumSkinColor = Color.Cyan;
                    LightSkinColor = Color.LightCyan;
                    break;

                default:
                    DarkSkinColor   = Color.FromArgb(155,155,155); 
                    MediumSkinColor = Color.FromArgb(175,175,175);
                    LightSkinColor = Color.FromArgb(195, 195, 195);
                    break;
            }

        }

        private static void DrawBuilding(Graphics g)
        {
            string filename = GetRandomBuilding();
            DrawBuilding(g, filename);
        }

        private static void DrawBuilding(Graphics g, string filename)
        {
            Bitmap building = new Bitmap(filename);
            int y = (int)g.VisibleClipBounds.Height - building.Height + _rnd.Next(building.Height / 4);
            int x = 50 + _rnd.Next((int)g.VisibleClipBounds.Width - 100) - building.Width / 2;
            g.DrawImage(building, x, y, building.Width, building.Height);
        }

        private static void DrawBuildingBlack(Graphics g)
        {
            string filename = GetRandomBuilding();
            DrawBuildingBlack(g, filename);
        }

        private static void DrawBuildingBlack(Graphics g,string filename)
        {
            Bitmap building = new Bitmap(filename);
            ReplaceNonTransparantColor(building, Color.Black);
            int y = (int)g.VisibleClipBounds.Height - building.Height + 20 + _rnd.Next(building.Height);
            int x = 50 + _rnd.Next((int)g.VisibleClipBounds.Width - 100) - building.Width / 2;
            g.DrawImage(building, x, y, building.Width, building.Height);
        }

        private static void Drawlandscape(Graphics g)
        {
            string filename = GetRandomLandscape();
            Bitmap landscape = new Bitmap(filename);
            int y = (int)g.VisibleClipBounds.Height - landscape.Height;
            g.DrawImage(landscape, 0, y, landscape.Width, landscape.Height);
        }

        private static Bitmap ResizeBitmap(Bitmap sourceBMP, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
                g.DrawImage(sourceBMP, 0, 0, width, height);
            return result;
        }

        private static Color GetRandomColor()
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

        private static void ReplaceNonTransparantColor(Bitmap b, Color colorToKeep)
        {
            for (int y = 0; y < b.Height; y++)
            {
                for (int x = 0; x < b.Width; x++)
                {
                    Color color1 = b.GetPixel(x, y);

                    if ( color1.A != 0 )
                    {
                        b.SetPixel(x, y, colorToKeep);
                    }
                }
            }
        }

        private static string GetRandomSky()
        {
            int i = GetRandom(0, _skies.Length);
            return _skies[i];
        }

        private static string GetRandomLandscape()
        {
            int i = GetRandom(0, _landscapes.Length);
            return _landscapes[i];
        }

        private static string GetRandomBuilding()
        {
            int i = GetRandom(0, _buildings.Length);
            return _buildings[i];
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

        #region load bitmaps
        private static bool LoadSkies()
        {
            string filter = @"background*.png";
            _skies = Directory.GetFiles(_imagePath, filter);
            return true;
        }

        private static bool LoadLandscapes()
        {
            string filter = @"landscape*.png";
            _landscapes = Directory.GetFiles(_imagePath, filter);
            return true;
        }

        private static bool LoadBuildings()
        {
            string filter = @"building*.png";
            _buildings = Directory.GetFiles(_imagePath, filter);
            return true;
        }
        #endregion

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
    }
}
