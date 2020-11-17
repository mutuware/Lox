﻿namespace Lox
{
    // In the book these classes were auto-generated. I'll try using records
    public abstract record Expr
    {
        public abstract T Accept<T>(IVisitor<T> visitor);

        public interface IVisitor<T>
        {
            T VisitBinaryExpr(Binary expr);
            T VisitGroupingExpr(Grouping expr);
            T VisitLiteralExpr(Literal expr);
            T VisitUnaryExpr(Unary expr);
            T VisitVariableExpr(Variable expr);
        }
    }

    public record Binary(Expr Left, Token Operator, Expr Right) : Expr { public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitBinaryExpr(this); };
    public record Grouping(Expr Expression) : Expr { public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitGroupingExpr(this); }
    public record Literal(object Value) : Expr { public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitLiteralExpr(this); };
    public record Unary(Token Operator, Expr Right) : Expr { public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitUnaryExpr(this); };
    public record Variable(Token Name) : Expr { public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitVariableExpr(this); };
}
