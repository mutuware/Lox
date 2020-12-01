﻿using System;
using System.Collections.Generic;

namespace Lox
{
    public class LoxClass : ILoxCallable
    {
        public readonly string Name;
        public readonly Dictionary<string, LoxFunction> Methods;

        public LoxClass(string name)
        {
            this.Name = name;
        }

        public LoxClass(string name, Dictionary<string, LoxFunction> methods)
        {
            this.Name = name;
            this.Methods = methods;
        }

        public int Arity() => 0;

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            LoxInstance instance = new LoxInstance(this);
            return instance;
        }

        public LoxFunction FindMethod(string name)
        {
            if (Methods.ContainsKey(name))
            {
                return Methods[name];
            }

            return null;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
