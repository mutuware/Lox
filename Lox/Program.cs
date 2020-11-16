using System;
using System.Collections.Generic;
using System.IO;

namespace Lox
{
    class Program
    {
        static bool hadError = false;

        static int Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: jlox [script]");
                return 64;
            }
            else if (args.Length == 1)
            {
                RunFile(args[0]);
            }
            else
            {
                RunPrompt();
            }

            return hadError ? 1 : 0;
        }

        static void RunFile(string path)
        {
            Run(File.ReadAllText(path));
        }

        private static void RunPrompt()
        {
            for (; ; )
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (line == null) break; // CTRL+Z
                Run(line);
                hadError = false;
            }
        }

        private static void Run(string source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();

            Parser parser = new Parser(tokens);
            Expr expression = parser.Parse();

            // Stop if there was a syntax error.
            if (hadError) return;

            Console.WriteLine(new AstPrinter().Print(expression));
        }

        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        public static void Error(Token token, String message)
        {
            if (token.Type == TokenType.EOF)
            {
                Report(token.Line, " at end", message);
            }
            else
            {
                Report(token.Line, " at '" + token.Lexeme + "'", message);
            }
        }

        private static void Report(int line, string where, string message)
        {
            Console.WriteLine($"[line {line}] Error {where} : {message}");
            hadError = true;
        }
    }
}
