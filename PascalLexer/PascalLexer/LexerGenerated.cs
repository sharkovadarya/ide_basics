namespace PascalLexer
{
using System;
using System.Text;
using System.Collections.Generic;
class LexFile
{
    public static void Main(String[] argv)
    {
        String [] args = Environment.GetCommandLineArgs();
        System.IO.FileStream f = new System.IO.FileStream(args[1], System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read, 8192);
        PascalLexerGenerated lexer = new PascalLexerGenerated(f);
        PascalToken t;
        while ((t = lexer.nextToken()) != null)
            Console.WriteLine(t);
    }
}
namespace LexerUtils {
    public enum TokenType
    {
        Symbol, // this includes a semicolon even though it's nowhere to be found in the specification. which is bizarre tbh. what is pascal w/o semicolons?
        Comment,
        Identifier,
        Number,
        CharacterString,
        ReservedWord
    }
    public class Util
    {
        public static readonly HashSet<string> ReservedWords = new HashSet<string>
        {
            "absolute", "and", "array", "asm", "begin", "case", "const", "constructor", "destructor", "div", "do", "downto", "else", "end", "file", 
            "for", "function", "goto", "if", "implementation", "in", "inherited", "inline", "interface", "label", "mod", "nil", "not", "object", "of", "operator",
            "or", "packed", "procedure", "program", "record", "reintroduce", "repeat", "self", "set", "shl", "shr", "string", "then", "to", "type", "unit",
            "until", "uses", "var", "while", "with", "xor",
            "as", "class", "dispinterface", "except", "exports", "finalization",
            "finally", "initialization", "inline", "is", "library", "on", "out",
            "packed", "property", "raise", "resourcestring", "threadvar", "try"
        };
        public static void IllChar(string s)
        {
            var sb = new StringBuilder("Illegal character: <");
            foreach (var t in s)
                if (t >= 32)
                    sb.Append(t);
                else
                {
                    sb.Append("^");
                    sb.Append(Convert.ToChar(t+'A'-1));
                }
            sb.Append(">");
            Console.WriteLine(sb.ToString());
        }
    } 
}
public class PascalToken
{
    private readonly LexerUtils.TokenType _type;
    private readonly string _text;
    private readonly int _line;
    private readonly int _charBegin;
    private readonly int _charEnd;
    internal PascalToken(LexerUtils.TokenType type, String text, int line, int charBegin, int charEnd)
    {
        _type = type;
        _text = text;
        _line = line;
        _charBegin = charBegin;
        _charEnd = charEnd;
    }
    public override string ToString()
    {
        return "Token #"+ _type + ": " + _text
               + " (line "+ _line + ")";
    }
    private bool Equals(PascalToken other)
    {
        return _type == other._type && 
               _text == other._text && 
               _line == other._line && 
               _charBegin == other._charBegin && 
               _charEnd == other._charEnd;
    }
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((PascalToken) obj);
    }
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int) _type;
            hashCode = (hashCode * 397) ^ (_text != null ? _text.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ _line;
            hashCode = (hashCode * 397) ^ _charBegin;
            hashCode = (hashCode * 397) ^ _charEnd;
            return hashCode;
        }
    }
}
/* test */


internal class PascalLexerGenerated
{
private const int YY_BUFFER_SIZE = 512;
private const int YY_F = -1;
private const int YY_NO_STATE = -1;
private const int YY_NOT_ACCEPT = 0;
private const int YY_START = 1;
private const int YY_END = 2;
private const int YY_NO_ANCHOR = 4;
delegate PascalToken AcceptMethod();
AcceptMethod[] accept_dispatch;
private const int YY_BOL = 128;
private const int YY_EOF = 129;
private System.IO.TextReader yy_reader;
private int yy_buffer_index;
private int yy_buffer_read;
private int yy_buffer_start;
private int yy_buffer_end;
private char[] yy_buffer;
private int yychar;
private int yyline;
private bool yy_at_bol;
private int yy_lexical_state;

internal PascalLexerGenerated(System.IO.TextReader reader) : this()
  {
  if (null == reader)
    {
    throw new System.ApplicationException("Error: Bad input stream initializer.");
    }
  yy_reader = reader;
  }

internal PascalLexerGenerated(System.IO.FileStream instream) : this()
  {
  if (null == instream)
    {
    throw new System.ApplicationException("Error: Bad input stream initializer.");
    }
  yy_reader = new System.IO.StreamReader(instream);
  }

private PascalLexerGenerated()
  {
  yy_buffer = new char[YY_BUFFER_SIZE];
  yy_buffer_read = 0;
  yy_buffer_index = 0;
  yy_buffer_start = 0;
  yy_buffer_end = 0;
  yychar = 0;
  yyline = 0;
  yy_at_bol = true;
  yy_lexical_state = YYINITIAL;
accept_dispatch = new AcceptMethod[] 
 {
  null,
  null,
  new AcceptMethod(this.Accept_2),
  new AcceptMethod(this.Accept_3),
  new AcceptMethod(this.Accept_4),
  new AcceptMethod(this.Accept_5),
  new AcceptMethod(this.Accept_6),
  new AcceptMethod(this.Accept_7),
  new AcceptMethod(this.Accept_8),
  new AcceptMethod(this.Accept_9),
  new AcceptMethod(this.Accept_10),
  new AcceptMethod(this.Accept_11),
  new AcceptMethod(this.Accept_12),
  new AcceptMethod(this.Accept_13),
  new AcceptMethod(this.Accept_14),
  new AcceptMethod(this.Accept_15),
  new AcceptMethod(this.Accept_16),
  new AcceptMethod(this.Accept_17),
  null,
  new AcceptMethod(this.Accept_19),
  new AcceptMethod(this.Accept_20),
  new AcceptMethod(this.Accept_21),
  new AcceptMethod(this.Accept_22),
  new AcceptMethod(this.Accept_23),
  new AcceptMethod(this.Accept_24),
  new AcceptMethod(this.Accept_25),
  null,
  new AcceptMethod(this.Accept_27),
  new AcceptMethod(this.Accept_28),
  new AcceptMethod(this.Accept_29),
  new AcceptMethod(this.Accept_30),
  null,
  new AcceptMethod(this.Accept_32),
  new AcceptMethod(this.Accept_33),
  new AcceptMethod(this.Accept_34),
  new AcceptMethod(this.Accept_35),
  null,
  new AcceptMethod(this.Accept_37),
  new AcceptMethod(this.Accept_38),
  new AcceptMethod(this.Accept_39),
  null,
  new AcceptMethod(this.Accept_41),
  new AcceptMethod(this.Accept_42),
  null,
  new AcceptMethod(this.Accept_44),
  null,
  new AcceptMethod(this.Accept_46),
  null,
  new AcceptMethod(this.Accept_48),
  null,
  new AcceptMethod(this.Accept_50),
  null,
  new AcceptMethod(this.Accept_52),
  null,
  new AcceptMethod(this.Accept_54),
  null,
  new AcceptMethod(this.Accept_56),
  null,
  null,
  null,
  null,
  };
  }

PascalToken Accept_2()
    { // begin accept action #2
{ 
    return null;
}
    } // end accept action #2

PascalToken Accept_3()
    { // begin accept action #3
{ 
    return null;
}
    } // end accept action #3

PascalToken Accept_4()
    { // begin accept action #4
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Symbol, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #4

PascalToken Accept_5()
    { // begin accept action #5
{ 
    yybegin(COMMENT_BRACKET);
    return null;
}
    } // end accept action #5

PascalToken Accept_6()
    { // begin accept action #6
{
    LexerUtils.Util.IllChar(yytext());
    return null;
}
    } // end accept action #6

PascalToken Accept_7()
    { // begin accept action #7
{ 
    string token = yytext();
    if (LexerUtils.Util.ReservedWords.Contains(token)) 
    {
        return (new PascalToken(LexerUtils.TokenType.ReservedWord, token, yyline, yychar, yychar + token.Length));  
    }
    if (token.Length > 127) 
    {
        Console.WriteLine("Identifier length limit exceeded for token ["+token+"]");
        return null;        
    }
    return (new PascalToken(LexerUtils.TokenType.Identifier, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #7

PascalToken Accept_8()
    { // begin accept action #8
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Number, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #8

PascalToken Accept_9()
    { // begin accept action #9
{ 
    yybegin(COMMENT_ASTERISK);
    return null;
}
    } // end accept action #9

PascalToken Accept_10()
    { // begin accept action #10
{ 
    yybegin(COMMENT_ONE_LINE);
    return null;
}
    } // end accept action #10

PascalToken Accept_11()
    { // begin accept action #11
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.CharacterString, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #11

PascalToken Accept_12()
    { // begin accept action #12
{
    // if it ever ends with a proper closure, good
    // if not, the rest of the file is considered a comment, so it's still good
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Comment, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #12

PascalToken Accept_13()
    { // begin accept action #13
{
    yybegin(YYINITIAL);
    return null;
}
    } // end accept action #13

PascalToken Accept_14()
    { // begin accept action #14
{
    // if it ever ends with a proper closure, good
    // if not, the rest of the file is considered a comment, so it's still good
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Comment, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #14

PascalToken Accept_15()
    { // begin accept action #15
{
    yybegin(YYINITIAL);
    return null;
}
    } // end accept action #15

PascalToken Accept_16()
    { // begin accept action #16
{
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Comment, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #16

PascalToken Accept_17()
    { // begin accept action #17
{
    yybegin(YYINITIAL);
    return null;
}
    } // end accept action #17

PascalToken Accept_19()
    { // begin accept action #19
{ 
    return null;
}
    } // end accept action #19

PascalToken Accept_20()
    { // begin accept action #20
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Symbol, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #20

PascalToken Accept_21()
    { // begin accept action #21
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Number, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #21

PascalToken Accept_22()
    { // begin accept action #22
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.CharacterString, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #22

PascalToken Accept_23()
    { // begin accept action #23
{
    // if it ever ends with a proper closure, good
    // if not, the rest of the file is considered a comment, so it's still good
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Comment, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #23

PascalToken Accept_24()
    { // begin accept action #24
{
    // if it ever ends with a proper closure, good
    // if not, the rest of the file is considered a comment, so it's still good
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Comment, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #24

PascalToken Accept_25()
    { // begin accept action #25
{
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Comment, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #25

PascalToken Accept_27()
    { // begin accept action #27
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Symbol, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #27

PascalToken Accept_28()
    { // begin accept action #28
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Number, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #28

PascalToken Accept_29()
    { // begin accept action #29
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.CharacterString, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #29

PascalToken Accept_30()
    { // begin accept action #30
{
    // if it ever ends with a proper closure, good
    // if not, the rest of the file is considered a comment, so it's still good
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Comment, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #30

PascalToken Accept_32()
    { // begin accept action #32
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Symbol, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #32

PascalToken Accept_33()
    { // begin accept action #33
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Number, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #33

PascalToken Accept_34()
    { // begin accept action #34
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.CharacterString, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #34

PascalToken Accept_35()
    { // begin accept action #35
{
    // if it ever ends with a proper closure, good
    // if not, the rest of the file is considered a comment, so it's still good
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Comment, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #35

PascalToken Accept_37()
    { // begin accept action #37
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Symbol, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #37

PascalToken Accept_38()
    { // begin accept action #38
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Number, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #38

PascalToken Accept_39()
    { // begin accept action #39
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.CharacterString, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #39

PascalToken Accept_41()
    { // begin accept action #41
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Symbol, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #41

PascalToken Accept_42()
    { // begin accept action #42
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Number, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #42

PascalToken Accept_44()
    { // begin accept action #44
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Symbol, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #44

PascalToken Accept_46()
    { // begin accept action #46
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Symbol, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #46

PascalToken Accept_48()
    { // begin accept action #48
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Symbol, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #48

PascalToken Accept_50()
    { // begin accept action #50
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Symbol, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #50

PascalToken Accept_52()
    { // begin accept action #52
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Symbol, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #52

PascalToken Accept_54()
    { // begin accept action #54
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Symbol, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #54

PascalToken Accept_56()
    { // begin accept action #56
{ 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Symbol, token, yyline, yychar, yychar + token.Length));  
}
    } // end accept action #56

private const int YYINITIAL = 0;
private const int COMMENT_BRACKET = 2;
private const int COMMENT_ONE_LINE = 3;
private const int COMMENT_ASTERISK = 1;
private static int[] yy_state_dtrans = new int[] 
  {   0,
  12,
  14,
  16
  };
private void yybegin (int state)
  {
  yy_lexical_state = state;
  }

private char yy_advance ()
  {
  int next_read;
  int i;
  int j;

  if (yy_buffer_index < yy_buffer_read)
    {
    return yy_buffer[yy_buffer_index++];
    }

  if (0 != yy_buffer_start)
    {
    i = yy_buffer_start;
    j = 0;
    while (i < yy_buffer_read)
      {
      yy_buffer[j] = yy_buffer[i];
      i++;
      j++;
      }
    yy_buffer_end = yy_buffer_end - yy_buffer_start;
    yy_buffer_start = 0;
    yy_buffer_read = j;
    yy_buffer_index = j;
    next_read = yy_reader.Read(yy_buffer,yy_buffer_read,
                  yy_buffer.Length - yy_buffer_read);
    if (next_read <= 0)
      {
      return (char) YY_EOF;
      }
    yy_buffer_read = yy_buffer_read + next_read;
    }
  while (yy_buffer_index >= yy_buffer_read)
    {
    if (yy_buffer_index >= yy_buffer.Length)
      {
      yy_buffer = yy_double(yy_buffer);
      }
    next_read = yy_reader.Read(yy_buffer,yy_buffer_read,
                  yy_buffer.Length - yy_buffer_read);
    if (next_read <= 0)
      {
      return (char) YY_EOF;
      }
    yy_buffer_read = yy_buffer_read + next_read;
    }
  return yy_buffer[yy_buffer_index++];
  }
private void yy_move_end ()
  {
  if (yy_buffer_end > yy_buffer_start && 
      '\n' == yy_buffer[yy_buffer_end-1])
    yy_buffer_end--;
  if (yy_buffer_end > yy_buffer_start &&
      '\r' == yy_buffer[yy_buffer_end-1])
    yy_buffer_end--;
  }
private bool yy_last_was_cr=false;
private void yy_mark_start ()
  {
  int i;
  for (i = yy_buffer_start; i < yy_buffer_index; i++)
    {
    if (yy_buffer[i] == '\n' && !yy_last_was_cr)
      {
      yyline++;
      }
    if (yy_buffer[i] == '\r')
      {
      yyline++;
      yy_last_was_cr=true;
      }
    else
      {
      yy_last_was_cr=false;
      }
    }
  yychar = yychar + yy_buffer_index - yy_buffer_start;
  yy_buffer_start = yy_buffer_index;
  }
private void yy_mark_end ()
  {
  yy_buffer_end = yy_buffer_index;
  }
private void yy_to_mark ()
  {
  yy_buffer_index = yy_buffer_end;
  yy_at_bol = (yy_buffer_end > yy_buffer_start) &&
    (yy_buffer[yy_buffer_end-1] == '\r' ||
    yy_buffer[yy_buffer_end-1] == '\n');
  }
internal string yytext()
  {
  return (new string(yy_buffer,
                yy_buffer_start,
                yy_buffer_end - yy_buffer_start)
         );
  }
private int yylength ()
  {
  return yy_buffer_end - yy_buffer_start;
  }
private char[] yy_double (char[] buf)
  {
  int i;
  char[] newbuf;
  newbuf = new char[2*buf.Length];
  for (i = 0; i < buf.Length; i++)
    {
    newbuf[i] = buf[i];
    }
  return newbuf;
  }
private const int YY_E_INTERNAL = 0;
private const int YY_E_MATCH = 1;
private static string[] yy_error_string = new string[]
  {
  "Error: Internal error.\n",
  "Error: Unmatched input.\n"
  };
private void yy_error (int code,bool fatal)
  {
  System.Console.Write(yy_error_string[code]);
  if (fatal)
    {
    throw new System.ApplicationException("Fatal Error.\n");
    }
  }
private static int[] yy_acpt = new int[]
  {
  /* 0 */   YY_NOT_ACCEPT,
  /* 1 */   YY_NO_ANCHOR,
  /* 2 */   YY_NO_ANCHOR,
  /* 3 */   YY_NO_ANCHOR,
  /* 4 */   YY_NO_ANCHOR,
  /* 5 */   YY_NO_ANCHOR,
  /* 6 */   YY_NO_ANCHOR,
  /* 7 */   YY_NO_ANCHOR,
  /* 8 */   YY_NO_ANCHOR,
  /* 9 */   YY_NO_ANCHOR,
  /* 10 */   YY_NO_ANCHOR,
  /* 11 */   YY_NO_ANCHOR,
  /* 12 */   YY_NO_ANCHOR,
  /* 13 */   YY_NO_ANCHOR,
  /* 14 */   YY_NO_ANCHOR,
  /* 15 */   YY_NO_ANCHOR,
  /* 16 */   YY_NO_ANCHOR,
  /* 17 */   YY_NO_ANCHOR,
  /* 18 */   YY_NOT_ACCEPT,
  /* 19 */   YY_NO_ANCHOR,
  /* 20 */   YY_NO_ANCHOR,
  /* 21 */   YY_NO_ANCHOR,
  /* 22 */   YY_NO_ANCHOR,
  /* 23 */   YY_NO_ANCHOR,
  /* 24 */   YY_NO_ANCHOR,
  /* 25 */   YY_NO_ANCHOR,
  /* 26 */   YY_NOT_ACCEPT,
  /* 27 */   YY_NO_ANCHOR,
  /* 28 */   YY_NO_ANCHOR,
  /* 29 */   YY_NO_ANCHOR,
  /* 30 */   YY_NO_ANCHOR,
  /* 31 */   YY_NOT_ACCEPT,
  /* 32 */   YY_NO_ANCHOR,
  /* 33 */   YY_NO_ANCHOR,
  /* 34 */   YY_NO_ANCHOR,
  /* 35 */   YY_NO_ANCHOR,
  /* 36 */   YY_NOT_ACCEPT,
  /* 37 */   YY_NO_ANCHOR,
  /* 38 */   YY_NO_ANCHOR,
  /* 39 */   YY_NO_ANCHOR,
  /* 40 */   YY_NOT_ACCEPT,
  /* 41 */   YY_NO_ANCHOR,
  /* 42 */   YY_NO_ANCHOR,
  /* 43 */   YY_NOT_ACCEPT,
  /* 44 */   YY_NO_ANCHOR,
  /* 45 */   YY_NOT_ACCEPT,
  /* 46 */   YY_NO_ANCHOR,
  /* 47 */   YY_NOT_ACCEPT,
  /* 48 */   YY_NO_ANCHOR,
  /* 49 */   YY_NOT_ACCEPT,
  /* 50 */   YY_NO_ANCHOR,
  /* 51 */   YY_NOT_ACCEPT,
  /* 52 */   YY_NO_ANCHOR,
  /* 53 */   YY_NOT_ACCEPT,
  /* 54 */   YY_NO_ANCHOR,
  /* 55 */   YY_NOT_ACCEPT,
  /* 56 */   YY_NO_ANCHOR,
  /* 57 */   YY_NOT_ACCEPT,
  /* 58 */   YY_NOT_ACCEPT,
  /* 59 */   YY_NOT_ACCEPT,
  /* 60 */   YY_NOT_ACCEPT
  };
private static int[] yy_cmap = new int[]
  {
  10, 10, 10, 10, 10, 10, 10, 10,
  3, 3, 2, 10, 10, 1, 10, 10,
  10, 10, 10, 10, 10, 10, 10, 10,
  10, 10, 10, 10, 10, 10, 10, 10,
  3, 10, 10, 22, 11, 20, 18, 23,
  4, 9, 5, 14, 24, 14, 15, 7,
  21, 21, 19, 19, 19, 19, 19, 19,
  13, 13, 27, 24, 25, 26, 25, 10,
  24, 17, 17, 17, 17, 16, 17, 12,
  12, 12, 12, 12, 12, 12, 12, 12,
  12, 12, 12, 12, 12, 12, 12, 12,
  12, 12, 12, 24, 10, 24, 24, 12,
  10, 17, 17, 17, 17, 16, 17, 12,
  12, 12, 12, 12, 12, 12, 12, 12,
  12, 12, 12, 12, 12, 12, 12, 12,
  12, 12, 12, 6, 10, 8, 10, 10,
  0, 0 
  };
private static int[] yy_rmap = new int[]
  {
  0, 1, 2, 2, 3, 1, 1, 4,
  5, 1, 1, 6, 7, 1, 8, 1,
  9, 1, 10, 1, 11, 12, 13, 14,
  15, 16, 17, 18, 19, 20, 21, 22,
  1, 23, 24, 25, 12, 12, 26, 27,
  19, 28, 29, 23, 30, 31, 19, 32,
  23, 33, 34, 35, 35, 34, 36, 37,
  38, 39, 40, 41, 29 
  };
private static int[,] yy_nxt = new int[,]
  {
  { 1, 18, 2, 3, 4, 20, 5, 27,
   32, 32, 6, 37, 7, 8, 41, 44,
   7, 7, 46, 8, 48, 8, 50, 52,
   32, 54, 32, 56 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, 3, 3, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, -1, -1, -1, 9, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, 32,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, 7, 7, -1, -1,
   7, 7, -1, 7, -1, 7, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 8, -1, 26,
   31, -1, -1, 8, -1, 8, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 11, -1, -1,
   -1, -1, -1, 11, -1, 11, 53, 51,
   -1, -1, -1, -1 },
  { 1, 23, 23, 23, 23, 55, 23, 23,
   23, 57, 23, -1, 23, 23, 23, 23,
   23, 23, 23, 23, 23, 23, 23, 23,
   23, 23, 23, 23 },
  { 1, 24, 24, 24, 24, 24, 24, 24,
   15, 24, 24, -1, 24, 24, 24, 24,
   24, 24, 24, 24, 24, 24, 24, 24,
   24, 24, 24, 24 },
  { 1, 59, 17, 25, 25, 25, 25, 25,
   25, 25, 25, 25, 25, 25, 25, 25,
   25, 25, 25, 25, 25, 25, 25, 25,
   25, 25, 25, 25 },
  { -1, -1, 19, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, -1, -1, -1, 32, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, 32, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 21, -1, -1,
   21, 21, -1, 21, -1, 21, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, 53, 51,
   -1, -1, -1, -1 },
  { -1, 23, 23, 23, 23, 58, 23, 23,
   23, 57, 23, -1, 23, 23, 23, 23,
   23, 23, 23, 23, 23, 23, 23, 23,
   23, 23, 23, 23 },
  { -1, 24, 24, 24, 24, 24, 24, 24,
   -1, 24, 24, -1, 24, 24, 24, 24,
   24, 24, 24, 24, 24, 24, 24, 24,
   24, 24, 24, 24 },
  { -1, -1, -1, 25, 25, 25, 25, 25,
   25, 25, 25, 25, 25, 25, 25, 25,
   25, 25, 25, 25, 25, 25, 25, 25,
   25, 25, 25, 25 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 38, -1, -1,
   -1, -1, -1, 38, -1, 38, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, 10,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, 32, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, 28, -1, 28, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 29, -1, -1,
   29, 29, -1, 29, -1, 29, 53, 51,
   -1, -1, -1, -1 },
  { -1, 23, 23, 23, 23, 30, 23, 23,
   23, 57, 23, -1, 23, 23, 23, 23,
   23, 23, 23, 23, 23, 23, 23, 23,
   23, 23, 23, 23 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 42, 60, -1,
   -1, -1, -1, 42, -1, 42, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 33, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, 34, -1, 34, 53, 51,
   -1, -1, -1, -1 },
  { -1, 23, 23, 23, 23, 58, 23, 23,
   23, 35, 23, -1, 23, 23, 23, 23,
   23, 23, 23, 23, 23, 23, 23, 23,
   23, 23, 23, 23 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 38, -1, -1,
   31, -1, -1, 38, -1, 38, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 39, 53, 51,
   -1, -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, 36, -1, 8, -1, -1,
   -1, -1, 40, 8, 43, 8, -1, -1,
   -1, -1, 32, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 42, -1, -1,
   -1, -1, -1, 42, -1, 42, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, 32, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 29, -1, -1,
   29, 29, -1, 29, -1, 29, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, 34, -1, 34, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 39, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, 45, -1, 11, -1, -1,
   -1, -1, 47, 11, 49, 11, -1, -1,
   -1, -1, -1, -1 },
  { -1, -1, 51, 51, 51, 51, 51, 51,
   51, 51, 51, 51, 51, 51, 51, 51,
   51, 51, 51, 51, 51, 51, 51, 22,
   51, 51, 51, 51 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, 32, 32, -1 },
  { -1, 23, 23, 23, 23, 30, 23, 23,
   23, 13, 23, -1, 23, 23, 23, 23,
   23, 23, 23, 23, 23, 23, 23, 23,
   23, 23, 23, 23 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, 32, -1 },
  { -1, 23, 23, 23, 23, -1, 23, 23,
   23, 35, 23, -1, 23, 23, 23, 23,
   23, 23, 23, 23, 23, 23, 23, 23,
   23, 23, 23, 23 },
  { -1, 23, 23, 23, 23, 30, 23, 23,
   23, -1, 23, -1, 23, 23, 23, 23,
   23, 23, 23, 23, 23, 23, 23, 23,
   23, 23, 23, 23 },
  { -1, -1, 17, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1 }
  };
public PascalToken nextToken()
  {
  char yy_lookahead;
  int yy_anchor = YY_NO_ANCHOR;
  int yy_state = yy_state_dtrans[yy_lexical_state];
  int yy_next_state = YY_NO_STATE;
  int yy_last_accept_state = YY_NO_STATE;
  bool yy_initial = true;
  int yy_this_accept;

  yy_mark_start();
  yy_this_accept = yy_acpt[yy_state];
  if (YY_NOT_ACCEPT != yy_this_accept)
    {
    yy_last_accept_state = yy_state;
    yy_mark_end();
    }
  while (true)
    {
    if (yy_initial && yy_at_bol)
      yy_lookahead = (char) YY_BOL;
    else
      {
      yy_lookahead = yy_advance();
      }
    yy_next_state = yy_nxt[yy_rmap[yy_state],yy_cmap[yy_lookahead]];
    if (YY_EOF == yy_lookahead && yy_initial)
      {
        return null;
      }
    if (YY_F != yy_next_state)
      {
      yy_state = yy_next_state;
      yy_initial = false;
      yy_this_accept = yy_acpt[yy_state];
      if (YY_NOT_ACCEPT != yy_this_accept)
        {
        yy_last_accept_state = yy_state;
        yy_mark_end();
        }
      }
    else
      {
      if (YY_NO_STATE == yy_last_accept_state)
        {
        throw new System.ApplicationException("Lexical Error: Unmatched Input.");
        }
      else
        {
        yy_anchor = yy_acpt[yy_last_accept_state];
        if (0 != (YY_END & yy_anchor))
          {
          yy_move_end();
          }
        yy_to_mark();
        if (yy_last_accept_state < 0)
          {
          if (yy_last_accept_state < 61)
            yy_error(YY_E_INTERNAL, false);
          }
        else
          {
          AcceptMethod m = accept_dispatch[yy_last_accept_state];
          if (m != null)
            {
            PascalToken tmp = m();
            if (tmp != null)
              return tmp;
            }
          }
        yy_initial = true;
        yy_state = yy_state_dtrans[yy_lexical_state];
        yy_next_state = YY_NO_STATE;
        yy_last_accept_state = YY_NO_STATE;
        yy_mark_start();
        yy_this_accept = yy_acpt[yy_state];
        if (YY_NOT_ACCEPT != yy_this_accept)
          {
          yy_last_accept_state = yy_state;
          yy_mark_end();
          }
        }
      }
    }
  }
}

}
