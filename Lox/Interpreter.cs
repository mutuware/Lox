﻿using System;
using System.Collections.Generic;

namespace Lox
{
    public class Interpreter : Expr.IVisitor<Object>, Stmt.IVisitor<Object> // book has <Void>, statements don't return values
    {
        private Environment environment = new Environment();

        public void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach(Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            catch (RuntimeError error)
            {
                Program.RuntimeError(error);
            }
        }

        //public void Interpret(Expr expression)
        //{
        //    try
        //    {
        //        object value = Evaluate(expression);
        //        Console.WriteLine(Stringify(value));
        //    }
        //    catch (RuntimeError error)
        //    {
        //        Program.RuntimeError(error);
        //    }
        //}

        private string Stringify(object obj)
        {
            if (obj == null) return "nil";

            if (obj is double)
            {
                string text = obj.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text.JavaStyleSubstring(0, text.Length - 2);
                }
                return text;
            }

            return obj.ToString();
        }

        public object VisitBinaryExpr(Binary expr)
        {
            object left = Evaluate(expr.Left);
            object right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.GREATER:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left <= (double)right;
                case TokenType.MINUS:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left - (double)right;
                case TokenType.PLUS:
                    if (left is double && right is double)
                    {
                        return (double)left + (double)right;
                    }
                    if (left is string && right is string)
                    {
                        return (string)left + (string)right;
                    }
                    throw new RuntimeError(expr.Operator, "Operands must be two numbers or two strings.");
                case TokenType.SLASH:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left / (double)right;
                case TokenType.STAR:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left * (double)right;
                case TokenType.BANG_EQUAL:
                    return !IsEqual(left, right);
                case TokenType.EQUAL_EQUAL:
                    return IsEqual(left, right);

            }

            // unreachable
            return null;
        }

        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;

            return a.Equals(b);
        }

        public object VisitGroupingExpr(Grouping expr)
        {
            return Evaluate(expr.Expression);
        }

        public object VisitLiteralExpr(Literal expr)
        {
            return expr.Value;
        }

        public object VisitUnaryExpr(Unary expr)
        {
            object right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.MINUS:
                    return -(double)right;
                case TokenType.BANG:
                    return !IsTruthy(right);
            }

            // Unreachable.
            return null;
        }

        private bool IsTruthy(object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            return true;
        }

        private void CheckNumberOperand(Token @operator, object operand)
        {
            if (operand is double) return;
            throw new RuntimeError(@operator, "Operand must be a number.");
        }

        private void CheckNumberOperands(Token @operator,
                                  object left, object right)
        {
            if (left is double && right is double) return;

            throw new RuntimeError(@operator, "Operands must be numbers.");
        }

        private object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        private void Execute(Stmt stmt)
        {
            stmt.Accept(this);
        }

        // Statement.IVisitor implementaion
        public object VisitBlockStmt(Stmt.Block stmt)
        {
            throw new NotImplementedException();
        }

        public object VisitClassStmt(Stmt.Class stmt)
        {
            throw new NotImplementedException();
        }

        public object VisitExpressionStmt(Stmt.Expression stmt)
        {
            Evaluate(stmt.expression);
            return null;
        }

        public object VisitFunctionStmt(Stmt.Function stmt)
        {
            throw new NotImplementedException();
        }

        public object VisitIfStmt(Stmt.If stmt)
        {
            throw new NotImplementedException();
        }

        public object VisitPrintStmt(Stmt.Print stmt)
        {
            object value = Evaluate(stmt.expression);
            Console.WriteLine(Stringify(value));
            return null;
        }

        public object VisitReturnStmt(Stmt.Return stmt)
        {
            throw new NotImplementedException();
        }

        public object VisitVarStmt(Stmt.Var stmt)
        {
            object value = null;
            if (stmt.Initializer != null)
            {
                value = Evaluate(stmt.Initializer);
            }

            environment.Define(stmt.Name.Lexeme, value);
            return null;
        }

        public object VisitWhileStmt(Stmt.While stmt)
        {
            throw new NotImplementedException();
        }

        public object VisitVariableExpr(Variable expr)
        {
            return environment.Get(expr.Name);
        }

        public object VisitAssignExpr(Assign expr)
        {
            object value = Evaluate(expr.Value);
            environment.Assign(expr.Name, value);
            return value;
        }
    }
}
