using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeParser
{
    public static class TypeParsingExtensions
    {
        public static TypeReference AsTypeReference(this string name)
        {
            if (string.IsNullOrEmpty(name))
                return TypeReference.Empty;

            int _;
            return ParseType(name, 0, name.Length, out _);
        }

        private static TypeReference ParseType(string name, int start, int end, out int finish)
        {
            var namespaceEndIndex = FindEndOfNamespace(name, start, end);
            string @namespace = namespaceEndIndex > start ? name.Substring(start, namespaceEndIndex - start) : null;

            var typeEndIndex = FindEndOfType(name, namespaceEndIndex + 1, end);
            string typeName = name.Substring(namespaceEndIndex + 1, typeEndIndex - namespaceEndIndex);

            int argumentsEndIndex;
            var arguments = ParseArguments(name, typeEndIndex + 1, end, out argumentsEndIndex);

            AssemblyReference assembly = ParseAssembly(name, argumentsEndIndex, end, out finish);
            return new TypeReference(assembly, @namespace, typeName, arguments);
        }

        private static TypeReference[] ParseArguments(string name, int start, int end, out int finish)
        {
            finish = start;
            if (start >= name.Length) return null;
            if (name[start] != '`') return null;

            int openBracket = name.IndexOf('[', start);
            int closeBracket = name.LastIndexOf(']', end - 1);

            int count = int.Parse(name.Substring(start + 1, openBracket - (start + 1)));
            TypeReference[] arguments = new TypeReference[count];

            int position = openBracket;
            for (int i = 0; i < count; i++)
            {
                int typeOpenBracket = name.IndexOf('[', position + 1);
                arguments[i] = ParseType(name, typeOpenBracket + 1, end, out position);

                finish = position + 1;
            }

            return arguments;
            //int bracketOpenIndex = name.IndexOf('[', start);
            //int count = int.Parse(name.Substring(start + 1, bracketOpenIndex - (start + 1)));

            //int position = bracketOpenIndex + 1;
            //TypeReference[] arguments = new TypeReference[count];
            //for (int i = 0; i < count; i++)
            //{
            //    bracketOpenIndex = name.IndexOf('[', position);
            //    int bracketCloseIndex = name.LastIndexOf(']', end-1);

            //    var subType = name.Substring(bracketOpenIndex + 1, (bracketCloseIndex - bracketOpenIndex) - 1);
            //    arguments[i] = ParseType(subType, 0, subType.Length);

            //    // update position to character after close bracket
            //    position = bracketCloseIndex + 1;
            //}

            //finish = position;
            //return arguments;
        }

        static int FindEndOfNamespace(string name, int start, int end)
        {
            int lastPeriodIndex = start - 1;
            while (start < end)
            {
                switch (name[start])
                {
                    case '.':
                        lastPeriodIndex = start;
                        break;
                    case '+':
                        if (start == 0 || name[start - 1] != '\\')
                            return lastPeriodIndex;

                        break;
                    case ',':
                    case '`':
                        return lastPeriodIndex;
                }
                start++;
            }

            return lastPeriodIndex;
        }

        static int FindEndOfType(string name, int start, int end)
        {
            while (start < end)
            {
                switch (name[start])
                {
                    case ',':
                    case '`':
                        return start - 1;
                }

                start++;
            }

            return Math.Min(start, end - 1);
        }

        private static AssemblyReference ParseAssembly(string name, int start, int end, out int finish)
        {
            finish = end;
            if (start >= name.Length) return AssemblyReference.Empty;

            while (start < end)
            {
                if (char.IsLetter(name[start]))
                    break;

                start++;
            }

            var nameEndIndex = name.IndexOfAny(new[] { ',', ' ', ']' }, start + 1);
            if (nameEndIndex == -1)
                nameEndIndex = end;

            var assemblyName = name.Substring(start, nameEndIndex - start);

            finish = nameEndIndex;
            return new AssemblyReference(assemblyName);
        }
    }
}
