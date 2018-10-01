using System.IO;

namespace Equilaterus.Vortex.Saturn
{
    public class VortexDataAttacheable : VortexData
    {
        public Stream File { get; private set; }

        public string Extension { get; set; }

        public VortexDataAttacheable(object entity, Stream file, string extension) : base(entity)
        {
            File = file;
            Extension = extension;
        }
    }
}
