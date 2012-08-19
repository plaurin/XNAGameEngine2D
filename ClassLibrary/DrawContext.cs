﻿using System;

using ClassLibrary.Cameras;
using ClassLibrary.Drawing;

namespace ClassLibrary
{
    public abstract class DrawContext
    {
        public abstract void DrawString(
            DrawContext drawContext,
            Camera camera,
            string finalText,
            Vector finalVector,
            float finalZoomFactor,
            DrawingFont drawingFont,
            Color color);

        public abstract void DrawLine(DrawContext drawContext, Camera camera, Vector from, Vector to, Color color);

        public abstract void DrawLine(Vector vectorFrom, Vector vectorTo, float width, Color color);
    }
}
