using System.Runtime.InteropServices;

namespace Clock2
{
    public partial class Clock : Form
    {
        private readonly SolidBrush brushText = new(Color.FromArgb(255, 255, 255));
        private readonly SolidBrush brushRectangle = new(Color.FromArgb(0, 0, 1));
        private readonly Font positionFont;
        private readonly Font timeFont;

        private bool drawPosition = false;

        public Clock()
        {
            InitializeComponent();

            this.TransparencyKey = BackColor;
            this.AllowTransparency = true;

            this.positionFont = new Font(this.Font.FontFamily, 15);
            this.timeFont = this.Font;

            if (SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS))
            {
            }

            RefreshTask();
        }

        public void SetPosition(int x, int y)
        {
            Location = new Point(x, y);
            Width = 190;
            Height = 60;
        }

        //Draws the position instead of the time
        public void SetPositionDrawing(bool drawPosition)
        {
            this.drawPosition = drawPosition;
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

            if (drawPosition)
                graphic.DrawString(Location.ToString(), positionFont, brushText, 15, 20); //draws position
            else
                graphic.DrawString(time.ToString("T"), timeFont, brushText, 0, 0); // draws time
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

        //Sets the window position
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        #endregion Strange voodoo
    }
}