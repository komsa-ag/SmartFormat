﻿using System;
using NUnit.Framework;

namespace SmartFormat.Tests.Core
{
    /// <summary>
    /// Settings.ConvertCharacterStringLiterals is required, if the format string does not
    /// come from code but from a file (e.g. a resource):
    /// While in case of code all character literals are converted to Unicode by the compiler,
    /// SmartFormat can do the conversion in case of strings coming from a file.
    /// </summary>
    [TestFixture]
    public class LiteralTextTests
    {
        [Test]
        public void AHowTo()
        {
            Smart.Default.Settings.ConvertCharacterStringLiterals = false;
            Smart.Default.Parser.UseAlternativeEscapeChar();
            var result = Smart.Format(@"a\ {b}c");
            var x = result;
        }

        [Test]
        public void FormatCharacterLiteralsAsString()
        {
            const string formatWithFileBehavior = @"No carriage return\r, no line feed\n";
            const string formatWithCodeBahavior = "With carriage return\r, and with line feed\n";

            var formatter = Smart.CreateDefaultSmartFormat();
            formatter.Settings.ConvertCharacterStringLiterals = false;

            var result = formatter.Format(formatWithFileBehavior);
            Assert.AreEqual(string.Format(formatWithFileBehavior), result);

            result = formatter.Format(formatWithCodeBahavior);
            Assert.AreEqual(string.Format(formatWithCodeBahavior), result);
        }

        [Test]
        public void FormatCharacterLiteralsAsUnicode()
        {
            const string formatWithFileBehavior = @"With line feed,\n and now\twith a TAB";
            const string formatWithCodeBahavior = "With line feed,\n and now\twith a TAB";
            
            var formatter = Smart.CreateDefaultSmartFormat();
            formatter.Settings.ConvertCharacterStringLiterals = true;

            var result = formatter.Format(formatWithFileBehavior);
            Assert.AreEqual(string.Format(formatWithCodeBahavior), result);
        }

        [Test]
        public void ConvertCharacterLiteralsToUnicodeWithListFormatter()
        {
            // a useful practical test case: separate members of a list by a new line
            var items = new[] { "one", "two", "three" };
            // Note the @ before the format string will switch off conversion of \n by the compiler
            Smart.Default.Settings.ConvertCharacterStringLiterals = true;
            var result = Smart.Default.Format(@"{0:list:{}|\n|\nand }", new object[] { items });
            Smart.Default.Settings.ConvertCharacterStringLiterals = false;
            Assert.AreEqual("one\ntwo\nand three", result);
        }

        [Test]
        public void IllegalEscapeSequenceThrowsException()
        {
            Smart.Default.Settings.ConvertCharacterStringLiterals = true;

            Assert.Throws<ArgumentException>(() => {
                Smart.Default.Format(@"Illegal excape sequences = \z");
            });
            Assert.Throws<ArgumentException>(() => {
                Smart.Default.Format(@"Illegal excape sequences = \");
            });

            Smart.Default.Settings.ConvertCharacterStringLiterals = false;
        }
    }
}
