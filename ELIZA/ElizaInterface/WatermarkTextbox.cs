using System;
using System.Drawing;
using System.Windows.Forms;

namespace ElizaInterface
{
    public partial class WatermarkTextbox : TextBox
    {
        private Font oldFont = null;
        private bool watermarkEnabled = false;
        private Color watermarkColor = Color.Gray;
        private string watermarkText = string.Empty;

        public Color WatermarkColor
        {
            get { return watermarkColor; }
            set
            {
                watermarkColor = value;
                Invalidate();
            }
        }

        public string WatermarkText
        {
            get { return watermarkText; }
            set { watermarkText = value; }
        }

        public WatermarkTextbox()
        {
            JoinEvents(true);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            ToggleWatermark(null, null);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var drawFont = new Font(Font.FontFamily, Font.Size, Font.Style, Font.Unit);
            var drawBrush = new SolidBrush(WatermarkColor);
            e.Graphics.DrawString((watermarkEnabled && ! Focused? WatermarkText: Text),
                drawFont, drawBrush, new PointF(0, 0));
            base.OnPaint(e);
        }

        private void JoinEvents(bool join)
        {
            if (join)
            {
                TextChanged+= new EventHandler(ToggleWatermark);
                LostFocus+= new EventHandler(ToggleWatermark);
                FontChanged += new EventHandler(WatermarkFontChanged);
                DoubleClick+= new EventHandler(ToggleWatermark);
                Click+= new EventHandler(ToggleWatermark);
            }
        }

        private void ToggleWatermark(object sender, EventArgs args)
        {
            if(!Focused && Text.Length <= 0)
                EnableWatermark();
            else DisableWatermark();
        }

        private void EnableWatermark()
        {
            oldFont = new Font(Font.FontFamily, Font.Size, Font.Style, Font.Unit);
            this.SetStyle(ControlStyles.UserPaint, true);
            watermarkEnabled = true;
            Refresh();
        }

        private void DisableWatermark()
        {
            watermarkEnabled = false;
            this.SetStyle(ControlStyles.UserPaint, false);
            if (oldFont != null)
                Font = oldFont;
        }

        private void WatermarkFontChanged(object sender, EventArgs args)
        {
            if (watermarkEnabled)
            {
                oldFont = new Font(Font.FontFamily, Font.Size, Font.Style, Font.Unit);
                Refresh();
            }
        }
    }
}
