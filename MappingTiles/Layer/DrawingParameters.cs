using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MappingTiles
{
    public class DrawingParameters
    {
        private View view;
        private Render render;
        private Collection<Layer> layers;

        public DrawingParameters()
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

        public Collection<Layer> Layers
        {
            get
            {
                if (layers == null)
                {
                    layers = new Collection<Layer>();
                }

                return layers;
            }
        }
    }
}
