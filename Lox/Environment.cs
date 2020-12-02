using System.Collections.Generic;

namespace Lox
{
    public class Environment
    {
        public readonly Environment Enclosing;

        public Environment()
        {
            Enclosing = null;
        }

        public Environment(Environment enclosing)
        {
            this.Enclosing = enclosing;
        }

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

            if (Enclosing != null) return Enclosing.Get(name);

            throw new RuntimeError(name, "Undefined variable '" + name.Lexeme + "'.");
        }

        public object GetAt(int distance, string name)
        {
            //Ancestor(distance).values.TryGetValue(name, out var value);
            //return value;
            return Ancestor(distance).values[name];
        }

        Environment Ancestor(int distance)
        {
            Environment environment = this;
            for (int i = 0; i < distance; i++)
            {
                environment = environment.Enclosing;
            }

            return environment;
        }

        public void AssignAt(int distance, Token name, object value)
        {
            Ancestor(distance).values[name.Lexeme] = value;
        }

        public void Assign(Token name, object value)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                values[name.Lexeme] = value;
                return;
            }

            if (Enclosing != null)
            {
                Enclosing.Assign(name, value);
                return;
            }

            throw new RuntimeError(name, "Undefined variable '" + name.Lexeme + "'.");
        }
    }
}
