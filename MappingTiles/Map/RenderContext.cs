using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MappingTiles
{
    public class RenderContext
    {
        private View view;
        private Render render;

        public RenderContext()
        {
        }

        public View View
        {
            get { return view; }
            set { view = value; }
        }

        public Render Render
        {
            get { return render; }
            set { render = value; }
        }
    }
}
