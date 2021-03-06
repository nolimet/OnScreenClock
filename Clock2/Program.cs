namespace Clock2
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(params string[] args)
        {
            int x = 0;
            int y = 0;
            bool drawPosition = false;

            for (int i = 0; i < args.Length; i++)
            {
                var v = args[i].ToLower();
                if (v == "x" || v == "xpos")
                {
                    x = int.TryParse(args[i + 1], out int xPos) ? xPos : x;
                }
                else if (v == "y" || v == "ypos")
                {
                    y = int.TryParse(args[i + 1], out int yPos) ? yPos : y;
                }
                else if (v == "dp" || v == "drawposition")
                {
                    drawPosition = true;
                }
            }

            ApplicationConfiguration.Initialize();

            var clock = new Clock();

            clock.SetPosition(x, y);
            clock.SetPositionDrawing(drawPosition);

            Application.Run(clock);
        }
    }
}