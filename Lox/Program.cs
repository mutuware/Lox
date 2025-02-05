﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Lox
{
    class Program
    {
        static Interpreter interpreter = new Interpreter();
        static bool hadError = false;
        static bool hadRuntimeError = false;


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

            if (hadError) System.Environment.Exit(65);
            if (hadRuntimeError) System.Environment.Exit(70);
        }

        private static void RunPrompt()
        {
            for (; ; )
            {
                Console.ForegroundColor = ConsoleColor.White;
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
            List<Stmt> statements = parser.Parse();

            // Stop if there was a syntax error.
            if (hadError) return;

            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var statement in statements)
            {
                //Console.WriteLine(new AstPrinter().Print(statement));
            }
            Console.ForegroundColor = ConsoleColor.Cyan;

            Resolver resolver = new Resolver(interpreter);
            resolver.Resolve(statements);

            // Stop if there was a resolution error.
            if (hadError) return;

            interpreter.Interpret(statements);
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

        public static void RuntimeError(RuntimeError error)
        {
            Console.WriteLine(error.Message + "\n[line " + error.token.Line + "]");
            hadRuntimeError = true;
        }

        private static void Report(int line, string where, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[line {line}] Error {where} : {message}");
            Console.ForegroundColor = ConsoleColor.White;
            hadError = true;
        }
    }
}
