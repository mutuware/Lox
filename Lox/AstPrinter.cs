using System.Text;

namespace Lox
{
    public class AstPrinter : IVisitor<string>
    {
        public string Print(Expr expr) => expr.Accept(this);

        public string VisitBinaryExpr(Binary expr) => Parenthesize(expr.Operator.Lexeme, expr.Left, expr.Right);
        public string VisitGroupingExpr(Grouping expr) => Parenthesize("group", expr.Expression);
        public string VisitLiteralExpr(Literal expr) => expr.Value == null ? "nil" : expr.Value.ToString();
        public string VisitUnaryExpr(Unary expr) => Parenthesize(expr.Operator.Lexeme, expr.Right);

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
    }
}
