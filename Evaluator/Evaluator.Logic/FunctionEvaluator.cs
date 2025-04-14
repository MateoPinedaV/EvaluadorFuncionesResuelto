namespace Evaluator.Logic
{
    public class FunctionEvaluator
    {
        public static double Evaluate(string infix)
        {
            var postfix = ToPostfix(infix);
            return Calculate(postfix);
        }

        private static double Calculate(List<string> postfix)
        {
            var stack = new Stack<double>();
            foreach (var token in postfix)
            {
                if (IsOperator(token))
                {
                    var b = stack.Pop();
                    var a = stack.Pop();
                    stack.Push(Result(a, token, b));
                }
                else
                {
                    stack.Push(double.Parse(token, System.Globalization.CultureInfo.InvariantCulture));
                }
            }
            return stack.Pop();
        }

        private static double Result(double a, string op, double b)
        {
            return op switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => a / b,
                "^" => Math.Pow(a, b),
                _ => throw new Exception("Operador inválido."),
            };
        }

        private static List<string> ToPostfix(string infix)
        {
            var output = new List<string>();
            var stack = new Stack<string>();
            var number = "";

            for (int i = 0; i < infix.Length; i++)
            {
                var c = infix[i];

                if (char.IsDigit(c) || c == '.')
                {
                    number += c;
                }
                else
                {
                    if (!string.IsNullOrEmpty(number))
                    {
                        output.Add(number);
                        number = "";
                    }

                    if (c == ' ') continue;

                    var token = c.ToString();

                    if (IsOperator(token))
                    {
                        while (stack.Count > 0 && Priority(token) <= Priority(stack.Peek()))
                        {
                            output.Add(stack.Pop());
                        }
                        stack.Push(token);
                    }
                    else if (c == '(')
                    {
                        stack.Push(token);
                    }
                    else if (c == ')')
                    {
                        while (stack.Peek() != "(")
                            output.Add(stack.Pop());
                        stack.Pop(); // Remove '('
                    }
                }
            }

            if (!string.IsNullOrEmpty(number))
                output.Add(number);

            while (stack.Count > 0)
                output.Add(stack.Pop());

            return output;
        }

        private static bool IsOperator(string c) => new[] { "+", "-", "*", "/", "^" }.Contains(c);

        private static int Priority(string op) => op switch
        {
            "^" => 3,
            "*" or "/" => 2,
            "+" or "-" => 1,
            _ => 0
        };
    }
}
