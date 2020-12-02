using System.Text;

namespace Lox
{
    public class AstPrinter : Expr.IVisitor<string>, Stmt.IVisitor<string>
    {
        public string Print(Expr expr) => expr.Accept(this);
        public string Print(Stmt stmt) => stmt.Accept(this);

        // expressions
        public string VisitAssignExpr(Assign expr) => Parenthesize("assignstmt", expr.Value);
        public string VisitBinaryExpr(Binary expr) => Parenthesize(expr.Operator.Lexeme, expr.Left, expr.Right);
        public string VisitCallExpr(Call expr) => Parenthesize("call", expr.Callee);
        public string VisitGroupingExpr(Grouping expr) => Parenthesize("group", expr.Expression);
        public string VisitLiteralExpr(Literal expr) => expr.Value == null ? "nil" : expr.Value.ToString();
        public string VisitUnaryExpr(Unary expr) => Parenthesize(expr.Operator.Lexeme, expr.Right);
        public string VisitVariableExpr(Variable expr) => $"var:{expr.Name.Lexeme}";
        public string VisitLogicalExpr(Logical expr) => $"{expr.Operator.Type}";

        // statements
        public string VisitBlockStmt(Stmt.Block stmt) => "block";
        public string VisitClassStmt(Stmt.Class stmt) => throw new System.NotImplementedException();
        public string VisitExpressionStmt(Stmt.Expression stmt) => Parenthesize("exprstmt", stmt.expression);
        public string VisitFunctionStmt(Stmt.Function stmt) => throw new System.NotImplementedException();
        public string VisitIfStmt(Stmt.If stmt) => Parenthesize("ifstmt", stmt.Condition);
        public string VisitPrintStmt(Stmt.Print stmt) => Parenthesize("printstmt", stmt.expression);
        public string VisitReturnStmt(Stmt.Return stmt) => throw new System.NotImplementedException();
        public string VisitVarStmt(Stmt.Var stmt) => Parenthesize("varstmt", stmt.Initializer);
        public string VisitWhileStmt(Stmt.While stmt) => throw new System.NotImplementedException();

        private string Parenthesize(string name, params Stmt[] stmts)
        {
            var builder = new StringBuilder();

            builder.Append('(').Append(name);
            foreach (Stmt stmt in stmts)
            {
                builder.Append(' ');
                builder.Append(stmt.Accept(this));
            }
            builder.Append(')');

            return builder.ToString();
        }

        private string Parenthesize(string name, params Expr[] exprs)
        {
            var builder = new StringBuilder();

            builder.Append('(').Append(name);
            foreach (Expr expr in exprs)
            {
                builder.Append(' ');
                builder.Append(expr.Accept(this));
            }
            builder.Append(')');

            return builder.ToString();
        }

        public string VisitGetExpr(Get expr)
        {
            return $"Get-{expr.Name}";
        }

        public string VisitSetExpr(Set expr)
        {
            return $"Set-{expr.Name}";
        }

        public string VisitThisExpr(This expr)
        {
            return "this";
        }
    }
}
