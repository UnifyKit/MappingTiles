using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MappingTiles
{
    public interface IGraphics
    {
        Rectangle ClipRectangle
        {
            get;
            set;
        }

        double Opacity
        {
            get;
            set;
        }

        object DrawingGraphics
        {
            get;
            set;
        }

        SmoothingMode SmoothingMode
        {
            get;
            set;
        }

        Image CreateImageMask(Color maskColor, Image image);

        void DrawImage(Image image, int x, int y);

        void DrawImage(Image image, int x, int y, int width, int height);

        void DrawLine(Color color, float x1, float y1, float x2, float y2);

        void DrawLine(Color color, float x1, float y1, float x2, float y2, float width);

        void DrawLine(Color color, float x1, float y1, float x2, float y2, float width, DashStyle dashStyle);


    }
}
