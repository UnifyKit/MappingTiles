
namespace MappingTiles
{
    public class RenderContext
    {
        private View view;
        private Render render;
        private object renderObject;
        private Pixel renderPosition;

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

        public object RenderObject
        {
            get { return renderObject; }
            set { renderObject = value; }
        }

        public Pixel RenderPosition
        {
            get { return renderPosition; }
            set { renderPosition = value; }
        }
    }
}
