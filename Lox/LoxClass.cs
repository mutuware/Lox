using System;
using System.Collections.Generic;

namespace Lox
{
    public class LoxClass : ILoxCallable
    {
        public readonly string Name;
        public readonly LoxClass Superclass;
        public readonly Dictionary<string, LoxFunction> Methods;

        public LoxClass(string name)
        {
            this.Name = name;
        }

        public LoxClass(string name, LoxClass superclass, Dictionary<string, LoxFunction> methods)
        {
            this.Name = name;
            this.Superclass = superclass;
            this.Methods = methods;
        }

        public int Arity()
        {
            LoxFunction initializer = FindMethod("init");
            if (initializer == null) return 0;
            return initializer.Arity();
        }

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            LoxInstance instance = new LoxInstance(this);
            LoxFunction initializer = FindMethod("init");
            if (initializer != null)
            {
                initializer.Bind(instance).Call(interpreter, arguments);
            }

            return instance;
        }

        public LoxFunction FindMethod(string name)
        {
            if (Methods.ContainsKey(name))
            {
                return Methods[name];
            }

            if (Superclass != null)
            {
                return Superclass.FindMethod(name);
            }

            return null;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
