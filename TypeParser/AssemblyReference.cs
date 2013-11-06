using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeParser
{
    public class AssemblyReference
    {
        static readonly AssemblyReference _Empty = new AssemblyReference();

        public AssemblyReference(string name = null)
        {
            Name = name ?? string.Empty;
        }

        public string Name { get; private set; }

        public static AssemblyReference Empty { get { return _Empty; } }

        public override string ToString()
        {
            return Name;
        }
    }
}
