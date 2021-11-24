using System.Runtime.InteropServices;

namespace Clock2
{
    public partial class Clock : Form
    {
        private readonly SolidBrush brushText = new(Color.FromArgb(255, 255, 255));
        private readonly SolidBrush brushRectangle = new(Color.FromArgb(0, 0, 1));

        public Clock()
        {
            InitializeComponent();
            Width = 190;
            Height = 60;

            this.TransparencyKey = BackColor;
            this.AllowTransparency = true;

            if (SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS))
            {
            }

            //Location = new Point(-240, 1310);

            //if (Screen.AllScreens.Length > 0)
            //{
            //    var ScreenTwoBounds = Screen.AllScreens[1].Bounds;
            //    this.Location = new Point(ScreenTwoBounds.Right - Width, ScreenTwoBounds.Bottom - Height);
            //}

            RefreshTask();
        }

        public void SetPosition(int x, int y)
        {
            Location = new Point(x, y);
        }

        private async void RefreshTask()
        {
            while (true)
            {
                await Task.Delay(1000);

                Refresh();
            }
        }

        private void OnPaintForground(object sender, PaintEventArgs e)
        {
            var graphic = e.Graphics;
            var time = DateTime.Now;

            graphic.FillRectangle(brushRectangle, 10, 10, 175, 50);
            graphic.DrawString(time.ToString("T"), this.Font, brushText, new PointF(0, 0));
        }

        #region Strange voodoo

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        //Allows window to bemoved around
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)
                m.Result = (IntPtr)HTCAPTION;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        #endregion Strange voodoo
    }
}