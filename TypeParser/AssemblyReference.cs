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
        private AssemblyReference()
        {
            Name = string.Empty;    
        }

        public AssemblyReference(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public static AssemblyReference Empty { get { return _Empty; } }
    }
}
