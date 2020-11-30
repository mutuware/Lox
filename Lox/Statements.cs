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
        public record Class();
        public record Expression(Expr expression) : Stmt { public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitExpressionStmt(this); }
        public record Function(Token Name, List<Token> Params, List<Stmt> body) : Stmt { public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitFunctionStmt(this); }
        public record If(Expr Condition, Stmt ThenBranch, Stmt ElseBranch) : Stmt { public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitIfStmt(this); }
        public record Print(Expr expression) : Stmt { public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitPrintStmt(this); }
        public record Return(Token keyword, Expr value) : Stmt { public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitReturnStmt(this); };
        public record Var(Token Name, Expr Initializer) : Stmt { public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitVarStmt(this); }
        public record While(Expr Condition, Stmt Body) : Stmt { public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitWhileStmt(this); }


        public abstract T Accept<T>(IVisitor<T> visitor);
    }
}
