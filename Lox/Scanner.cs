using System;
using System.Collections.Generic;
using Lox;

namespace Lox
{
    public class Scanner
    {
        public List<Token> Tokens { get; } = new List<Token>();
        public string Source { get; }

        private int start = 0;
        private int current = 0;
        private int line = 1;

        public Scanner(string source)
        {
            Source = source;
        }


        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                // We are at the beginning of the next lexeme.
                start = current;
                ScanToken();
            }

            Tokens.Add(new Token(TokenType.EOF, "", null, line));
            return Tokens;
        }

        private bool IsAtEnd() => current >= Source.Length;

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACE); break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case '-': AddToken(TokenType.MINUS); break;
                case '+': AddToken(TokenType.PLUS); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '*': AddToken(TokenType.STAR); break;
                case '!':
                    AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;

                case '/':
                    if (Match('/'))
                    {
                        // A comment goes until the end of the line.
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;

                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace.
                    break;

                case '\n':
                    line++;
                    break;

                case '"':
                    DoString();
                    break;

                default:
                    if (char.IsDigit(c)) // not constants so can't use as in case statement
                    {
                        DoNumber();
                    }
                    else if (char.IsLetter(c))
                    {
                        DoIdentifier();
                    }
                    else
                    {
                        Program.Error(line, "Unexpected character.");
                    }
                    break;
            }
        }

        private void DoIdentifier()
        {
            while (char.IsLetterOrDigit(Peek())) Advance();

            var text = Source.JavaStyleSubstring(start, current);

            if (Keywords.TryGetValue(text, out TokenType type))
            {
                type = TokenType.IDENTIFIER;
            }

            AddToken(type);
        }

        private void DoNumber()
        {
            while (char.IsDigit(Peek())) Advance();

            // Look for a fractional part.
            if (Peek() == '.' && char.IsDigit(PeekNext()))
            {
                // Consume the "."
                Advance();

                while (char.IsDigit(Peek())) Advance();
            }

            AddToken(TokenType.NUMBER, double.Parse(Source.JavaStyleSubstring(start, current)));
        }

        private void DoString()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') line++;
                Advance();
            }

            if (IsAtEnd())
            {
                Program.Error(line, "Unterminated string.");
                return;
            }

            // The closing ".
            Advance();

            // Trim the surrounding quotes.
            var value = Source.JavaStyleSubstring(start + 1, current - 1);
            AddToken(TokenType.STRING, value);
        }

        private char Peek() // lookahead (no advance)
        {
            if (IsAtEnd()) return '\0';
            return Source[current];
        }

        private char PeekNext()
        {
            if (current + 1 >= Source.Length) return '\0';
            return Source[current + 1];
        }

        private bool Match(char expected) // conditional advance
        {
            if (IsAtEnd()) return false;
            if (Source[current] != expected) return false;

            current++;
            return true;
        }

        private char Advance()
        {
            current++;
            return Source[current - 1];
        }

        private void AddToken(TokenType type) => AddToken(type, null);
        private void AddToken(TokenType type, object literal)
        {
            var text = Source.JavaStyleSubstring(start, current);
            Tokens.Add(new Token(type, text, literal, line));
        }

        private static Dictionary<string, TokenType> Keywords = new Dictionary<string, TokenType>
        {
    {"and",    TokenType.AND},
    {"class",  TokenType.CLASS},
    {"else",   TokenType.ELSE},
    {"false",  TokenType.FALSE},
    {"for",    TokenType.FOR},
    {"fun",    TokenType.FUN},
    {"if",     TokenType.IF},
    {"nil",    TokenType.NIL},
    {"or",     TokenType.OR},
    {"print",  TokenType.PRINT},
    {"return", TokenType.RETURN},
    {"super",  TokenType.SUPER},
    {"this",   TokenType.THIS},
    {"true",   TokenType.TRUE},
    {"var",    TokenType.VAR},
    {"while",  TokenType.WHILE}
        };
    }

}
