using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace MappingTiles
{
    public class GdiPlusRender : Render
    {
        private Graphics graphics;

        public GdiPlusRender()
        {
        }

        public override void Draw(RenderContext renderContext)
        {
            if (Target == null)
            {
                Target = new Bitmap((int)renderContext.View.Width, (int)renderContext.View.Height);
                graphics = Graphics.FromImage((Bitmap)Target);
            }

            if (renderContext.RenderObject is byte[])
            {
                DrawImage(renderContext);
            }
        }

        private void DrawImage(RenderContext renderContext)
        {
            using (MemoryStream stream = new MemoryStream(renderContext.RenderObject as byte[]))
            {
                Image img = Image.FromStream(stream);
                Point position = new Point((int)renderContext.RenderPosition.X, (int)renderContext.RenderPosition.Y);
                graphics.DrawImageUnscaled(img, position);
                img.Dispose();
            }
        }
    }
}
