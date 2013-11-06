using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeParser
{
    public class TypeReference
    {
        static readonly TypeReference _Empty = new TypeReference();
        static readonly TypeReference[] NO_ARGUMENTS = new TypeReference[0];

        public TypeReference(AssemblyReference assembly = null, string @namespace = null, string name = null, TypeReference[] typeArguments = null)
        {
            Assembly = assembly ?? AssemblyReference.Empty;
            Namespace = @namespace ?? string.Empty;
            Name = name ?? string.Empty;
            GenericTypeArguments = typeArguments ?? NO_ARGUMENTS;
        }

        public AssemblyReference Assembly { get; private set; }
        public string Namespace { get; private set; }
        public string Name { get; private set; }
        public IReadOnlyList<TypeReference> GenericTypeArguments { get; set; }

        public static TypeReference Empty { get { return _Empty; } }

        public override string ToString()
        {
            return string.Concat(Namespace, '.', Name, ", ", Assembly);
        }
    }
}
