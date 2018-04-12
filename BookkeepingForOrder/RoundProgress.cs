using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ExtGui
{
    public partial class RoundProgress : UserControl
    {
        private const int       POINT_RADIUS        = 10;
        private const float     POINT_RADIUS_F      = (float)POINT_RADIUS;

        private const int       HALF_RADIUS         = POINT_RADIUS / 2;
        private const float     HALF_RADIUS_F       = POINT_RADIUS_F / 2;

        private const int       CIRCLE_RADIUS       = 14;
        private const float     CIRCLE_RADIUS_F     = (float)CIRCLE_RADIUS;

        private static Size     POINT_SIZE          = new Size  (POINT_RADIUS, POINT_RADIUS);
        private static SizeF    POINT_SIZE_F        = new SizeF (POINT_RADIUS_F, POINT_RADIUS_F);

        private static float    SIN_OF_2            = (float)System.Math.Sqrt (2f) / 2;
        private static float    POINT_SINUS         = SIN_OF_2 * CIRCLE_RADIUS;

        public RoundProgress ()
        {
            InitializeComponent ();

            this.DoubleBuffered     = true;

            this.timer_.Interval    = 100;
            this.timer_.Enabled     = true;
            this.timer_.Tick       += new EventHandler (OnTick);

            this.MakePoints ();
            this.FillPoints ();
        }

        public string DataText
        {
            get { return this.text_; }
            set { this.text_ = value; }
        }

        private void MakePoints ()
        {
            int x = this.Width / 2;
            int y = (this.Height / 2) - 10;

            this.north_ = new Point (x - HALF_RADIUS, y - CIRCLE_RADIUS - HALF_RADIUS);
            this.south_ = new Point (x - HALF_RADIUS, y + CIRCLE_RADIUS - HALF_RADIUS);

            this.west_ = new Point (x - HALF_RADIUS - CIRCLE_RADIUS, y - HALF_RADIUS);
            this.east_ = new Point (x - HALF_RADIUS + CIRCLE_RADIUS, y - HALF_RADIUS);

            this.south_east_ = this.MakePoint ( 1,  1);
            this.north_west_ = this.MakePoint (-1, -1);
            this.north_east_ = this.MakePoint ( 1, -1);
            this.south_west_ = this.MakePoint (-1,  1);
        }

        private PointF MakePoint (int xSign, int ySign)
        {
            float x = (float)this.Width  / 2 - HALF_RADIUS_F + POINT_SINUS * xSign;
            float y = ((float)(this.Height / 2) - 10) - HALF_RADIUS_F + POINT_SINUS * ySign;

            return new PointF (x, y);
        }

        private void FillPoints ()
        {
            this.points_.Clear ();

            this.points_.Add (new RectangleF (north_,        POINT_SIZE_F));
            this.points_.Add (new RectangleF (north_west_,   POINT_SIZE_F));
            this.points_.Add (new RectangleF (west_,         POINT_SIZE_F));
            this.points_.Add (new RectangleF (south_west_,   POINT_SIZE_F));
            this.points_.Add (new RectangleF (south_,        POINT_SIZE_F));
            this.points_.Add (new RectangleF (south_east_,   POINT_SIZE_F));
            this.points_.Add (new RectangleF (east_,         POINT_SIZE_F));
            this.points_.Add (new RectangleF (north_east_,   POINT_SIZE_F));
        }

        private void OnTick (object sender, EventArgs e)
        {
            this.Invalidate (); 
        }

        private void DrawPoints (Graphics grphx)
        {
            foreach (RectangleF point in this.points_)
            {
                Color clr = this.colors_.Next ();
                grphx.FillEllipse (new SolidBrush (clr), point);
            }

            this.colors_.Next ();
        }

        private void DrawText (Graphics grphx)
        {
            grphx.PageUnit = GraphicsUnit.Pixel;

            int y = this.Height / 2 + CIRCLE_RADIUS + POINT_RADIUS * 2;
            y -= 5;

            SizeF size = grphx.MeasureString (this.text_, SystemFonts.DefaultFont);
            int x = this.Width / 2 - (int)(size.Width / 2) + 0;

            grphx.DrawString (this.text_, SystemFonts.DefaultFont, SystemBrushes.ControlText, x, y);
        }

        protected override void OnPaint (PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            this.DrawPoints (e.Graphics);
            this.DrawText   (e.Graphics);

            base.OnPaint (e);
        }

        protected override void OnSizeChanged (EventArgs e)
        {
            base.OnSizeChanged (e);

            this.MakePoints ();
            this.FillPoints ();

            this.Invalidate ();
        }

        private Point   north_;
        private Point   south_;
        private Point   west_;
        private Point   east_;
        private PointF  south_east_;
        private PointF  north_west_;
        private PointF  north_east_;
        private PointF  south_west_;

        private List <RectangleF>   points_ = new List<RectangleF> ();

        private Timer               timer_  = new Timer ();
        private string              text_;
        private ColorGiver          colors_ = new ColorGiver ();
    }

    internal class ColorGiver
    {
        private static Color    COLOR  = Color.Black;

        public Color Next ()
        {
            Color clr = colors[0];

            colors.RemoveAt (0);
            colors.Add      (clr);

            return clr;
        }

        private List <Color> colors = new List<Color> ()
        {
            Color.FromArgb (250,    COLOR),
            Color.FromArgb (190,    COLOR),
            Color.FromArgb (170,    COLOR),
            Color.FromArgb (130,    COLOR),
            Color.FromArgb (90,     COLOR), 
            Color.FromArgb (50,     COLOR),
            Color.FromArgb (45,     COLOR),
            Color.FromArgb (20,     COLOR)
        };

    }

}
