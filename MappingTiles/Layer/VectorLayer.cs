﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MappingTiles
{
    public class VectorLayer : Layer
    {
        public override void ClearCache()
        {
            throw new NotImplementedException();
        }

        public override void ViewChanged(UpdateMode updateMode, DrawingParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}