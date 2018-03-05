using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Equilaterus.Vortex.Engine
{
    public class VortexDataAdjuntable : VortexData
    {
        public FileStream File { get; private set; }

        public string Extension { get; set; }

        public VortexDataAdjuntable(object entity, FileStream file, string extension) : base(entity)
        {
            File = file;
            Extension = extension;
        }
    }
}
