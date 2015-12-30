using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MappingTiles
{
    public class BackgroundLayer : Layer
    {
        private Color backColor;
        private string imageFilePath;

        public Color BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        public string ImageFilePath
        {
            get { return imageFilePath; }
            set { imageFilePath = value; }
        }

        public override void ClearCache()
        {
        }

        public override void ViewChanged(UpdateMode updateMode, RenderContext renderContext)
        {
            throw new NotImplementedException();
        }
    }
}
