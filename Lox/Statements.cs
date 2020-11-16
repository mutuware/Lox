using System.Collections.Generic;

namespace Lox
{
    public abstract record Stmt
    {
        public interface IVisitor<T>
        {
            T VisitBlockStmt(Block stmt);
            T VisitClassStmt(Class stmt);
            T VisitExpressionStmt(Expression stmt);
            T VisitFunctionStmt(Function stmt);
            T VisitIfStmt(If stmt);
            T VisitPrintStmt(Print stmt);
            T VisitReturnStmt(Return stmt);
            T VisitVarStmt(Var stmt);
            T VisitWhileStmt(While stmt);
        }

        // Nested Stmt classes here...
        public record Block(List<Stmt> Statements) : Stmt { public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitBlockStmt(this); }
        public record Expression(Expr expression) : Stmt { public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitExpressionStmt(this); }
        public record Print(Expr expression) : Stmt { public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitPrintStmt(this); }
        public record Var(Token name, Expr initializer) : Stmt { public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitVarStmt(this); }
        public record Class();
        public record Function();
        public record If();
        public record Return();
        public record While();

        public abstract T Accept<T>(IVisitor<T> visitor);
    }
}
