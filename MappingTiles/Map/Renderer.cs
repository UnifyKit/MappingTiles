namespace MappingTiles
{
    public abstract class Renderer
    {
        private object target;

        protected Renderer()
            : this(null)
        { }

        protected Renderer(object target)
        {
            this.Target = target;
        }

        public object Target
        {
            get
            {
                return target;
            }
            protected set
            {
                target = value;
            }
        }

        public abstract void Draw(RenderContext renderContext);
    }
}
