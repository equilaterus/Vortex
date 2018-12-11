using System.IO;

namespace Equilaterus.Vortex.Saturn
{
    public class AttachedFile
    {
        public Stream File { get; private set; }

        public string Extension { get; set; }

        public AttachedFile(Stream file, string extension)
        {
            File = file;
            Extension = extension;
        }
    }
}
