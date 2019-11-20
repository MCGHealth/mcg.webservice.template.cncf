using System;
using NUnit.Framework;
using Serilog.Events;
using FluentAssertions;
using Mcg.Webservice.Cncf.Api;

namespace Mcg.Webservice.UnitTests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestFixture]
    public class TypeExtensionTests
    {
        internal enum Animal
        {
            Dog,
            Cat,
            Hedgehog
        }

        [Test]
        public void SafeString_correctly_formats_double_underscores()
        {
            var target = "mytype__mymethod";
            var expected = "mytype_mymethod";

            var result = target.SafeString();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SafeString_correctly_generic_type_name()
        {
            var target = "mytype`1_mymethod";
            var expected = "mytype_mymethod";

            var result = target.SafeString();

            Assert.AreEqual(expected, result);
        }

        [TestCase("0", 0)]
        [TestCase("42", 42)]
        [TestCase("-42", -42)]
        public void ToInt_correctly_converts_whole_number_string_to_int(string target, int expected)
        {
            var result = target.ToInt();
            Assert.AreEqual(expected, result);
        }

        [TestCase("information", LogEventLevel.Information)]
        [TestCase("verbose", LogEventLevel.Verbose)]
        [TestCase("CAT", Animal.Cat)]
        [TestCase("hedgehog", Animal.Hedgehog)]
        [TestCase("dOg", Animal.Dog)]

        public void ToEnum_correctly_converts_string_to_Enum<TEnum>(string target, TEnum expected) where TEnum : struct
        {
            var (success, newValue) = target.ToEnum<TEnum>();
            Assert.AreEqual(true, success);
            Assert.AreEqual(expected, newValue);
        }

        [TestCase("0.1")]
        [TestCase("four")]
        [TestCase("$5")]
        [TestCase("a")]
        [TestCase("")]
        public void ToInt_throws_FormatException(string target)
        {
            Action a = () =>
            {
                _ = target.ToInt();
            };

            a.Should().Throw<FormatException>();
        }

        [TestCase("bloop", LogEventLevel.Information)]
        [TestCase("bloop", Animal.Hedgehog)]

        public void ToEnum_returns_false<TEnum>(string target, TEnum _ ) where TEnum : struct
        {
            var (success, _) = target.ToEnum<TEnum>();

            Assert.IsFalse(success);
        }
    }
}
