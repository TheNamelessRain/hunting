using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interceptor;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using GregsStack.InputSimulatorStandard;
using System.Net;

namespace hunting
{
    public partial class Form1 : Form
    {

        private bool isRunning = false;
        private CancellationTokenSource tokenSource;
        private static InputSimulator Ins = new InputSimulator();



        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                isRunning = true;
                button1.Text = "Stop";

                tokenSource = new CancellationTokenSource();

                await Task.Run(() => StartHunting(tokenSource.Token));
            }
            else
            {
                isRunning = false;
                button1.Text = "Start";
                tokenSource.Cancel();
            }
        }



        private void StartHunting(CancellationToken cancellationToken)
        {
            Input input = new Input();
            input.KeyboardFilterMode = KeyboardFilterMode.All;
            input.Load();

            while (!cancellationToken.IsCancellationRequested)
            {
                int x = 925;
                int y = 800;
                Color targetColor = Color.FromArgb(239, 222, 199);

                if (GetPixelColor(x, y) == targetColor)
                {
                    Ins.Keyboard.TextEntry("BibaIBoba");
                    Ins.Keyboard.KeyPress(GregsStack.InputSimulatorStandard.Native.VirtualKeyCode.SPACE);
                }
            }
        }

        private Color GetPixelColor(int x, int y)
        {
            using (Bitmap screenPixel = new Bitmap(1, 1))
            {
                using (Graphics g = Graphics.FromImage(screenPixel))
                {
                    g.CopyFromScreen(x, y, 0, 0, new System.Drawing.Size(1, 1));
                }
                return screenPixel.GetPixel(0, 0);
            }
        }
    }
}