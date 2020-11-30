using System;
using System.Collections.Generic;

namespace Lox
{
    public class LoxFunction : ILoxCallable
    {
        private Stmt.Function declaration;

        public LoxFunction(Stmt.Function declaration)
        {
            this.declaration = declaration;
        }

        public int Arity() => declaration.Params.Count;

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            Environment environment = new Environment(interpreter.globals);
            for (int i = 0; i < declaration.Params.Count; i++)
            {
                environment.Define(declaration.Params[i].Lexeme, arguments[i]);
            }

            try
            {
                interpreter.ExecuteBlock(declaration.body, environment);
            }
            catch (ReturnException returnValue)
            {
                return returnValue.Value;
            }

            return null;
        }

        public override string ToString()
        {
            return $"<fn {declaration.Name.Lexeme}>";
        }
    }
}