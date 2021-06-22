using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace PascalLexer
{
    public class Lexer
    {
        public static List<PascalToken> LexString(string text)
        {
            var result = new List<PascalToken>();
            
            var reader = new StringReader(text);
            var lexer = new PascalLexerGenerated(reader);
            PascalToken t;
            while ((t = lexer.nextToken()) != null)
            {
                result.Add(t);
            }

            return result;
        }
    }

    public class Tests
    {
        [Test]
        public void TestIdentifiers()
        {
            var result = Lexer.LexString("a abc ABC _Abc a1A3B8c_2");
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Identifier, "a", 0, 0, 1), result[0]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Identifier, "abc", 0, 2, 5), result[1]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Identifier, "ABC", 0, 6, 9), result[2]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Identifier, "_Abc", 0, 10, 14), result[3]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Identifier, "a1A3B8c_2", 0, 15, 24), result[4]);
        }

        [Test]
        public void TestLongIdentifier()
        {
            const string longIdentifier = "this_identifier_is_127_characters_long_which_is_as_much_as_the_pascal_specification_permits_i_wonder_why_______________________";
            var result = Lexer.LexString(longIdentifier);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Identifier, longIdentifier, 0, 0, 127), result[0]);
        }
        
        [Test]
        public void TestIdentifierLengthExceeded()
        {
            const string overlyLongIdentifier = "this_identifier_is_128_characters_long_which_is_as_much_as_the_pascal_specification_permits_i_wonder_why________________________";
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));
            var result = Lexer.LexString(overlyLongIdentifier);
            Assert.IsEmpty(result);
            Assert.AreEqual("Identifier length limit exceeded for token [" + overlyLongIdentifier + "]", sb.ToString().TrimEnd());
        }
        
        [Test]
        public void TestReservedWords()
        {
            const string reservedWordsString = "absolute and array asm begin case const constructor" +
                                               " destructor div do downto else end file for function goto" +
                                               " if implementation in inherited inline interface label " +
                                               "mod nil not object of operator or packed procedure program " +
                                               "record reintroduce repeat self set shl shr string " +
                                               "then to type unit until uses var while with xor " +
                                               "as class dispinterface except exports finalization finally " +
                                               "initialization inline is library on out packed property raise " +
                                               "resourcestring threadvar try";
            var result = Lexer.LexString(reservedWordsString);
            var reservedWords = reservedWordsString.Split();
            Assert.AreEqual(reservedWords.Length, result.Count);
            var pos = 0;
            for (var i = 0; i < reservedWords.Length; i++)
            {
                Assert.AreEqual(new PascalToken(LexerUtils.TokenType.ReservedWord, reservedWords[i], 0, pos, pos + reservedWords[i].Length), result[i]);
                pos += reservedWords[i].Length + 1;
            }
        }

        [Test]
        public void TestUnsignedInteger()
        {
            var result = Lexer.LexString("30");
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "30", 0, 0, 2), result[0]);
        }
        
        [Test]
        public void TestHexInteger()
        {
            var result = Lexer.LexString("$115AF2bCb");
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "$115AF2bCb", 0, 0, 10), result[0]);
        }
        
        [Test]
        public void TestHexIntegerNoPrefix()
        {
            var result = Lexer.LexString("115AF2bCb");
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "115", 0, 0, 3), result[0]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Identifier, "AF2bCb", 0, 3, 9), result[1]);
        }
        
        [Test]
        public void TestHexIntegerIncorrectDigits()
        {
            var result = Lexer.LexString("$115AF2gCb");
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "$115AF2", 0, 0, 7), result[0]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Identifier, "gCb", 0, 7, 10), result[1]);
        }
        
        [Test]
        public void TestOctalInteger()
        {
            var result = Lexer.LexString("&115267");
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "&115267", 0, 0, 7), result[0]);
        }
        
        [Test]
        public void TestOctalIntegerIncorrectDigits()
        {
            var result = Lexer.LexString("&1152678115");
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "&115267", 0, 0, 7), result[0]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "8115", 0, 7, 11), result[1]);
        }

        [Test]
        public void TestBinaryInteger()
        {
            var result = Lexer.LexString("%0110010");
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "%0110010", 0, 0, 8), result[0]);
        }
        
        [Test]
        public void TestBinaryIntegerIncorrectDigits()
        {
            var result = Lexer.LexString("%01100102110");
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "%0110010", 0, 0, 8), result[0]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "2110", 0, 8, 12), result[1]);
        }

        [Test]
        public void TestUnsignedReal()
        {
            var result = Lexer.LexString("111.369 1628e3 677e-156 0.212e-13 0.00021e100500");
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "111.369", 0, 0, 7), result[0]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "1628e3", 0, 8, 14), result[1]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "677e-156", 0, 15, 23), result[2]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "0.212e-13", 0, 24, 33), result[3]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "0.00021e100500", 0, 34, 48), result[4]);
        }

        [Test]
        public void TestLeadingZeroes() // allowed in Pascal
        {
            var result = Lexer.LexString("0000030 0000.000030 %000011110 &00030 $000030");
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "0000030", 0, 0, 7), result[0]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "0000.000030", 0, 8, 19), result[1]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "%000011110", 0, 20, 30), result[2]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "&00030", 0, 31, 37), result[3]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "$000030", 0, 38, 45), result[4]);
        }
        
        [Test]
        public void TestSignedInteger()
        {
            var result = Lexer.LexString("-30 +3030 -%11110 +&30 -$30");
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "-30", 0, 0, 3), result[0]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "+3030", 0, 4, 9), result[1]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "-%11110", 0, 10, 17), result[2]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "+&30", 0, 18, 22), result[3]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "-$30", 0, 23, 27), result[4]);
        }

        [Test]
        public void TestSignedReal()
        {
            var result = Lexer.LexString("-3.141529 +33e+33 -111.111e-111 -111.111e+111");
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "-3.141529", 0, 0, 9), result[0]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "+33e+33", 0, 10, 17), result[1]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "-111.111e-111", 0, 18, 31), result[2]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "-111.111e+111", 0, 32, 45), result[3]);
        }

        [Test]
        public void TestQuotedString()
        {
            var result = Lexer.LexString("'quoted string indeed'");
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.CharacterString, "'quoted string indeed'", 0, 0, 22), result[0]);
        }
        
        [Test]
        public void TestControlString()
        {
            var result = Lexer.LexString("#13");
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.CharacterString, "#13", 0, 0, 3), result[0]);
        }
        
        [Test]
        public void TestCharacterString()
        {
            var result = Lexer.LexString("'character'#32#32'string'");
            // counts as one token, not four
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.CharacterString, "'character'#32#32'string'", 0, 0, 25), result[0]);
        }

        [Test]
        public void TestOneCharacterSymbols()
        {
            const string stringWithSymbols = "' + - * / = < > [ ] . , ( ) : ^ @ $ # & %";
            var result = Lexer.LexString(stringWithSymbols);
            var symbols = stringWithSymbols.Split();
            Assert.AreEqual(symbols.Length, result.Count);
            for (var i = 0; i < symbols.Length; i++)
            {
                Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Symbol, symbols[i], 0, 2 * i, 2 * i + 1), result[i]);
            }
        }

        [Test]
        public void TestTwoCharacterSymbols()
        {
            const string stringWithSymbols = "<<  >>  **  <>  ><  <=  >=  :=  +=  -=  *=  /=  (.  .)";
            var result = Lexer.LexString(stringWithSymbols);
            var symbols = stringWithSymbols.Split(new string[] { "  " }, StringSplitOptions.None);
            Assert.AreEqual(symbols.Length, result.Count);
            for (var i = 0; i < symbols.Length; i++)
            {
                Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Symbol, symbols[i], 0, 4 * i, 4 * i + 2), result[i]);
            }
        }

        [Test]
        public void TestMultilineInput()
        {
            var result = Lexer.LexString("abra\ncadabra\n2021");
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Identifier, "abra", 0, 0, 4), result[0]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Identifier, "cadabra", 1, 5, 12), result[1]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "2021", 2, 13, 17), result[2]);
        }

        [Test]
        public void TestMultilineCharacterString() // are not allowed in Pascal
        {
            var result = Lexer.LexString("'abra'\r\n'cadabra\r\n2021'");
            // tokenized not as two strings but a quote, an identifier, a number, and a quote
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.CharacterString, "'abra'", 0, 0, 6), result[0]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Symbol, "'", 1, 8, 9), result[1]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Identifier, "cadabra", 1, 9, 16), result[2]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "2021", 2, 18, 22), result[3]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Symbol, "'", 2, 22, 23), result[4]);
        }
        
        // various combinations

        [Test]
        public void TestBinaryExpression()
        {
            var result = Lexer.LexString("2 + a");
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Number, "2", 0, 0, 1), result[0]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Symbol, "+", 0, 2, 3), result[1]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Identifier, "a", 0, 4, 5), result[2]);
        }
        
        [Test]
        public void TestHelloWorld()
        {
            const string helloWorld = 
                "program Hello;\r\n" +
                "begin\r\n" +
                "    writeln('Hello World')\r\n" +
                "end.";
            var result = Lexer.LexString(helloWorld);
            Assert.AreEqual(10, result.Count);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.ReservedWord, "program", 0, 0, 7), result[0]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Identifier, "Hello", 0, 8, 13), result[1]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Symbol, ";", 0, 13, 14), result[2]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.ReservedWord, "begin", 1, 16, 21), result[3]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Identifier, "writeln", 2, 27, 34), result[4]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Symbol, "(", 2, 34, 35), result[5]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.CharacterString, "'Hello World'", 2, 35, 48), result[6]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Symbol, ")", 2, 48, 49), result[7]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.ReservedWord, "end", 3, 51, 54), result[8]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Symbol, ".", 3, 54, 55), result[9]);
        }

        [Test]
        public void TestAsteriskComment()
        {
            const string commentText = "multiline\r\ncomment\r\ntext\r\n";
            const string text = "some text\r\n" +
                                "(*\r\n" +
                                commentText +"*)\r\n"+
                                "other";
            var result = Lexer.LexString(text);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Comment, "\r\n" + commentText, 1, 13, 13 + 2 + commentText.Length), result[2]);
        }
        
        [Test]
        public void TestBracketComment()
        {
            const string commentText = "multiline\r\ncomment\r\ntext\r\n";
            const string text = "some text\r\n" +
                                "{\r\n" +
                                commentText +"}\r\n"+
                                "other";
            var result = Lexer.LexString(text);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Comment, "\r\n" + commentText, 1, 12, 12 + 2 + commentText.Length), result[2]);
        }
        
        [Test]
        public void TestOneLineComment()
        {
            const string commentText = "comment text";
            const string text = "some text\r\n" +
                                "//" +
                                commentText +"\r\n"+
                                "other";
            var result = Lexer.LexString(text);
            Assert.AreEqual(4, result.Count);
            var commentEndPosition = 13 + commentText.Length;
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Comment, commentText, 1, 13, commentEndPosition), result[2]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Identifier, "other", 2, commentEndPosition + 2, commentEndPosition + 2 + "other".Length), result[3]);
        }
        
        [Test]
        public void TestIllegalCharacter()
        {
            var result = Lexer.LexString("symbol ? + !");
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Identifier, "symbol", 0, 0, 6), result[0]);
            Assert.AreEqual(new PascalToken(LexerUtils.TokenType.Symbol, "+", 0, 9, 10), result[1]);
        }
    }
}