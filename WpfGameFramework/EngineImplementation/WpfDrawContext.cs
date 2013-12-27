using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

using GameFramework;
using GameFramework.Cameras;
using GameFramework.Drawing;

using Color = GameFramework.Color;
using Vector = GameFramework.Vector;

namespace WpfGameFramework.EngineImplementation
{
    public class WpfDrawContext : DrawContext
    {
        private readonly Viewport viewport;

        private readonly DrawingVisual drawingVisual;

        private readonly DrawingContext drawingContext;

        public WpfDrawContext(Viewport viewport)
        {
            this.viewport = viewport;
            this.drawingVisual = new DrawingVisual();
            this.drawingContext = this.drawingVisual.RenderOpen();
        }

        public DrawingVisual Finish()
        {
            this.drawingContext.Close();
            return this.drawingVisual;
        }

        public override void FillColor(Color color)
        {
            var brush = new SolidColorBrush(color.ToWinColor());
            this.drawingContext.DrawRectangle(brush, null, new Rect(0, 0, this.viewport.Width, this.viewport.Height));
        }

        public override void DrawImage(Texture texture, Rectangle destination)
        {
            var winTexture = (WpfTexture)texture;

            this.drawingContext.DrawImage(winTexture.BitmapSource, destination.ToRect());
        }

        public override void DrawImage(Texture texture, Rectangle source, Rectangle destination)
        {
            var winTexture = (WpfTexture)texture;

            var tile = winTexture.GetTile(source);

            this.drawingContext.DrawImage(tile, destination.ToRect());
        }

        public override void DrawString(DrawContext drawContext, Camera camera, string text, Vector vector, float zoomFactor, DrawingFont drawingFont, Color color)
        {
            var brush = new SolidColorBrush(color.ToWinColor());

            // Create the initial formatted text string.
            var formattedText = new FormattedText(
                text,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                11,
                brush);

            // Draw the formatted text string to the DrawingContext of the control.
            this.drawingContext.DrawText(formattedText, vector.ToWinPoint());
        }

        public override void DrawLine(Vector vectorFrom, Vector vectorTo, float width, Color color)
        {
            var pen = new Pen(new SolidColorBrush(color.ToWinColor()), width);

            this.drawingContext.DrawLine(pen, vectorFrom.ToWinPoint(), vectorTo.ToWinPoint());
        }
    }
}