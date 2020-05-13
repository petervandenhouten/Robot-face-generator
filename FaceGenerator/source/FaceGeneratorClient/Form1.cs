using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GalaxyFootball;

namespace FaceGeneratorClient
{
    public partial class Form1 : Form
    {
        private Timer _timer;

        public Form1()
        {
            InitializeComponent();
            GenerateAndShowNewBitmap();
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            GenerateAndShowNewBitmap();
        }

        private void GenerateAndShowNewBitmap()
        {
            Bitmap bmp;
            switch (comboBoxImageSelection.SelectedIndex)
            {
                case 0:
                    bmp = GalaxyFootball.FaceGenerator.GenerateFace();
                    break;

                case 1:
                    bmp = GalaxyFootball.RobotGenerator.GenerateFace(Color.Red, Color.Yellow, Color.Blue);
                    break;

                default:
                    GalaxyFootball.PlanetGenerator.SetImagePath(@"..\Resources\Images\PlanetGenerator\");
                    bmp = GalaxyFootball.PlanetGenerator.GeneratePlanet();
                    break;
            }

            _pictureBox.Image = bmp;
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            if (_timer == null)
            {
                _timer = new Timer();
                _timer.Interval = 250;
                _timer.Tick += OnTimerTick;
            }
            _timer.Enabled = true;
        }

        void OnTimerTick(object sender, EventArgs e)
        {
            GenerateAndShowNewBitmap();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (_pictureBox.Image!= null)
            {
                _pictureBox.Image.Save("Output.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
