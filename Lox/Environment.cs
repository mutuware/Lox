﻿using System.Collections.Generic;

namespace Lox
{
    public class Environment
    {
        Dictionary<string, object> values = new Dictionary<string, object>();

        public void Define(string name, object value)
        {
            values[name] = value;
        }

        public object Get(Token name)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                return values[name.Lexeme];
            }

            throw new RuntimeError(name, "Undefined variable '" + name.Lexeme + "'.");
        }

        public void Assign(Token name, object value)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                values[name.Lexeme] =  value;
                return;
            }

            throw new RuntimeError(name, "Undefined variable '" + name.Lexeme + "'.");
        }
    }
}
