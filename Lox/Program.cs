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
            ParserSanityCheck();

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

        private static void ParserSanityCheck()
        {
            Expr expression = new Binary(
            new Unary(
            new Token(TokenType.MINUS, "-", null, 1),
            new Literal(123)),
            new Token(TokenType.STAR, "*", null, 1),
            new Grouping(
            new Literal(45.67)));

            Console.WriteLine(new AstPrinter().Print(expression));
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

            // For now, just print the tokens.
            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
        }

        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        private static void Report(int line, string where, string message)
        {
            Console.WriteLine($"[line {line}] Error {where} : {message}");
            hadError = true;
        }
    }
}
