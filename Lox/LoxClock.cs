using System;
using System.Collections.Generic;

namespace Lox
{
    public class LoxClock : ILoxCallable
    {
        public int Arity() => 0;

        public object Call(Interpreter interpreter, List<object> arguments) => (DateTime.UtcNow - DateTime.UnixEpoch).Milliseconds / 1000.0;
    }
}