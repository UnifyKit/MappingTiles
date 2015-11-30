using System;
using System.Collections.Generic;
using System.IO;

namespace MappingTiles
{
    public abstract class Render
    {
        private object target;

        protected Render()
        { }

        protected Render(object target)
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

        public abstract void Draw(IEnumerable<Layer> layers, View view);

        public virtual Stream SaveStream(IEnumerable<Layer> layers, View view)
        {
            throw new NotImplementedException();
        }
    }
}
