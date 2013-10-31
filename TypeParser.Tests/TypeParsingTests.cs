using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace TypeParser
{
    public class TypeParsingTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void should_return_empty_type_reference_when_no_fqtn_provided(string type)
        {
            var result = type.AsTypeReference();

            Assert.Equal(TypeReference.Empty, result);
        }

        [Fact]
        public void should_be_able_to_parse_basic_type()
        {
            string type = "Namespace.Type, AssemblyName";

            var result = type.AsTypeReference();

            Assert.Equal("Type", result.Name);
            Assert.Equal("Namespace", result.Namespace);
            Assert.Equal("AssemblyName", result.Assembly.Name);
        }

        [Fact]
        public void should_handle_escaped_characters()
        {
            var type = "TopNamespace.Sub\\+Namespace.ContainingClass+NestedClass, MyAssembly, Version=1.3.0.0, Culture=neutral, PublicKeyToken=b17a5c561934e089";

            var result = type.AsTypeReference();

            Assert.Equal("TopNamespace.Sub\\+Namespace", result.Namespace);
            Assert.Equal("ContainingClass+NestedClass", result.Name);
            Assert.Equal("MyAssembly", result.Assembly.Name);
        }

        [Fact]
        public void should_be_able_to_parse_namespace_free_type()
        {
            string type = "Type, AssemblyName";

            var result = type.AsTypeReference();

            Assert.Equal("Type", result.Name);
            Assert.Null(result.Namespace);
            Assert.Equal("AssemblyName", result.Assembly.Name);
        }

        [Fact]
        public void should_be_able_to_parse_generic_arguments()
        {
            var type = "Namespace.Type`1[[Namespace2.Type2, Assembly2]], Assembly";

            var result = type.AsTypeReference();

            Assert.Equal("Namespace", result.Namespace);
            Assert.Equal("Type", result.Name);
            Assert.Equal("Assembly", result.Assembly.Name);

            Assert.NotEmpty(result.GenericTypeArguments);

            var argument = result.GenericTypeArguments[0];
            Assert.Equal("Namespace2", argument.Namespace);
            Assert.Equal("Type2", argument.Name);
            Assert.Equal("Assembly2", argument.Assembly.Name);
        }

        [Fact]
        public void should_be_able_to_parse_multiple_generic_arguments()
        {
            var type = "Namespace.Type`2[[Namespace2.Type2, Assembly2],[Namespace3.Type3, Assembly3]], Assembly";

            var result = type.AsTypeReference();

            Assert.Equal("Namespace", result.Namespace);
            Assert.Equal("Type", result.Name);
            Assert.Equal("Assembly", result.Assembly.Name);

            Assert.NotEmpty(result.GenericTypeArguments);

            var argument1 = result.GenericTypeArguments[0];
            Assert.Equal("Namespace2", argument1.Namespace);
            Assert.Equal("Type2", argument1.Name);
            Assert.Equal("Assembly2", argument1.Assembly.Name);

            var argument2 = result.GenericTypeArguments[1];
            Assert.Equal("Namespace3", argument2.Namespace);
            Assert.Equal("Type3", argument2.Name);
            Assert.Equal("Assembly3", argument2.Assembly.Name);
        }

        [Fact]
        public void should_be_able_to_parse_nested_generic_type()
        {
            var type = "Namespace.Type`1[[Namespace2.Type2`1[[Namespace3.Type3, Assembly3]], Assembly2]], Assembly";

            var result = type.AsTypeReference();

            Assert.Equal("Namespace", result.Namespace);
            Assert.Equal("Type", result.Name);
            Assert.Equal("Assembly", result.Assembly.Name);

            Assert.NotEmpty(result.GenericTypeArguments);

            var argument1 = result.GenericTypeArguments[0];
            Assert.Equal("Namespace2", argument1.Namespace);
            Assert.Equal("Type2", argument1.Name);
            Assert.Equal("Assembly2", argument1.Assembly.Name);

            var argument2 = argument1.GenericTypeArguments[0];
            Assert.Equal("Namespace3", argument2.Namespace);
            Assert.Equal("Type3", argument2.Name);
            Assert.Equal("Assembly3", argument2.Assembly.Name);
        }
    }
}
