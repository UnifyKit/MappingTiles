using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace MappingTiles
{
    public class GdiPlusRender : Renderer
    {
        private Graphics graphics;

        public GdiPlusRender()
        {
        }

        public override void Draw(RenderContext renderContext)
        {
            if (Target == null)
            {
                Target = new Bitmap((int)renderContext.Viewport.Width, (int)renderContext.Viewport.Height);
                graphics = Graphics.FromImage((Bitmap)Target);
            }

            if (renderContext.DrawnObject is byte[])
            {
                DrawImage(renderContext);
            }
        }

        private void DrawImage(RenderContext renderContext)
        {
            using (MemoryStream stream = new MemoryStream(renderContext.DrawnObject as byte[]))
            {
                Image img = Image.FromStream(stream);
                Point position = new Point((int)renderContext.DrawnPosition.X, (int)renderContext.DrawnPosition.Y);
                graphics.DrawImageUnscaled(img, position);
                img.Dispose();
            }
        }
    }
}
