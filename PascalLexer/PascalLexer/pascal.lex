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


%%
%line
%char

%namespace PascalLexer
%class PascalLexerGenerated
%function nextToken
%type PascalToken

%state COMMENT_ASTERISK
%state COMMENT_BRACKET
%state COMMENT_ONE_LINE

IDENTIFIER=[_a-zA-Z][_a-zA-Z0-9]*

NONNEWLINE_WHITE_SPACE_CHAR=[\ \t\b\012]
NEWLINE=((\r\n)|\n)

ASTERISK_COMMENT_TEXT=([^)*$]|[^*$]")"[^*$]|[^)$]"*"[^)$]|"*"[^)$]|")"[^*$])*
BRACKET_COMMENT_TEXT=([^}$])*
ONE_LINE_COMMENT_TEXT=[^\r\n]*

DIGIT_SEQUENCE=[0-9]+
HEX_DIGIT_SEQUENCE=[0-9A-Fa-f]+
OCTAL_DIGIT_SEQUENCE=[0-7]+
BINARY_DIGIT_SEQUENCE=[01]+
UNSIGNED_INTEGER=({DIGIT_SEQUENCE})|("$"({HEX_DIGIT_SEQUENCE}))|("&"({OCTAL_DIGIT_SEQUENCE}))|("%"({BINARY_DIGIT_SEQUENCE}))
SCALE_FACTOR=[Ee][+-]?{DIGIT_SEQUENCE}
UNSIGNED_REAL={DIGIT_SEQUENCE}(\.{DIGIT_SEQUENCE}|{SCALE_FACTOR}|\.({DIGIT_SEQUENCE})({SCALE_FACTOR}))
UNSIGNED_NUMBER=({UNSIGNED_REAL})|({UNSIGNED_INTEGER})
NUMBER=[+-]?({UNSIGNED_NUMBER})

CONTROL_STRING=#({UNSIGNED_INTEGER})
QUOTED_STRING='[^'\r]*'
CHARACTER_STRING=(({CONTROL_STRING})|({QUOTED_STRING}))+

SYMBOL=(["'+-*/=<>[].,():^@{}$#&%;"])|"<<"|">>"|"**"|"<>"|"><"|"<="|">="|":="|"+="|"-="|"*="|"/="|"(."|".)"
%% 


<YYINITIAL> {NEWLINE} { 
    return null;
}

<YYINITIAL> {NONNEWLINE_WHITE_SPACE_CHAR}+ { 
    return null;
}

<YYINITIAL> "(*" { 
    yybegin(COMMENT_ASTERISK);
    return null;
}

<YYINITIAL> "{" { 
    yybegin(COMMENT_BRACKET);
    return null;
}

<YYINITIAL> "//" { 
    yybegin(COMMENT_ONE_LINE);
    return null;
}

<COMMENT_ASTERISK> {ASTERISK_COMMENT_TEXT} {
    // if it ever ends with a proper closure, good
    // if not, the rest of the file is considered a comment, so it's still good
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Comment, token, yyline, yychar, yychar + token.Length));  
}

<COMMENT_BRACKET> {BRACKET_COMMENT_TEXT} {
    // if it ever ends with a proper closure, good
    // if not, the rest of the file is considered a comment, so it's still good
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Comment, token, yyline, yychar, yychar + token.Length));  
}

<COMMENT_ONE_LINE> {ONE_LINE_COMMENT_TEXT} {
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Comment, token, yyline, yychar, yychar + token.Length));  
}

<COMMENT_ASTERISK> "*)" {
    yybegin(YYINITIAL);
    return null;
}

<COMMENT_BRACKET> "}" {
    yybegin(YYINITIAL);
    return null;
}

<COMMENT_ONE_LINE> {NEWLINE} {
    yybegin(YYINITIAL);
    return null;
}


<YYINITIAL> {IDENTIFIER} { 
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

<YYINITIAL> {NUMBER} { 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Number, token, yyline, yychar, yychar + token.Length));  
}

<YYINITIAL> {CHARACTER_STRING} { 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.CharacterString, token, yyline, yychar, yychar + token.Length));  
}

<YYINITIAL> {SYMBOL} { 
    string token = yytext();
    return (new PascalToken(LexerUtils.TokenType.Symbol, token, yyline, yychar, yychar + token.Length));  
}


<YYINITIAL> . {
    LexerUtils.Util.IllChar(yytext());
    return null;
}
