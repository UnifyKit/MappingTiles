
namespace MappingTiles
{
    public class RenderContext
    {
        private Viewport viewport;
        private Renderer renderer;
        private object drawnObject;
        private Pixel renderPosition;

        public RenderContext()
        {
        }

        public Viewport Viewport
        {
            get { return viewport; }
            set { viewport = value; }
        }

        public Renderer Renderer
        {
            get { return renderer; }
            set { renderer = value; }
        }

        public object DrawnObject
        {
            get { return drawnObject; }
            set { drawnObject = value; }
        }

        public Pixel DrawnPosition
        {
            get { return renderPosition; }
            set { renderPosition = value; }
        }
    }
}
