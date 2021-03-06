﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*From Mitja at 
 * https://social.msdn.microsoft.com/Forums/vstudio/en-US/7f62b87d-a35c-4074-a0f0-84a9dd7ff0a5/convert-string-to-formula
*/
namespace WPFlindao
{
    public class StringToFormula
    {
        private string[] _operators = { "-", "+", "/", "*", "^"};
        private Func<float, float, float>[] _operations = {
		(a1, a2) => a1 - a2,
		(a1, a2) => a1 + a2,
		(a1, a2) => a1 / a2,
		(a1, a2) => a1 * a2,
		(a1, a2) => (float) Math.Pow(a1, a2)
	};

        public float Eval(string expression)
        {
            List<string> tokens = getTokens(expression);
            Stack<float> operandStack = new Stack<float>();
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;

            while (tokenIndex < tokens.Count)
            {
                string token = tokens[tokenIndex];
                if (token == "(")
                {
                    string subExpr = getSubExpression(tokens, ref tokenIndex);
                    operandStack.Push(Eval(subExpr));
                    continue;
                }
                if (token == ")")
                {
                    throw new ArgumentException("Mis-matched parentheses in expression");
                }
                //If this is an operator  
                if (Array.IndexOf(_operators, token) >= 0) {
                    while (operatorStack.Count > 0 && Array.IndexOf(_operators, token) < Array.IndexOf(_operators, operatorStack.Peek()))
                    {
                        string op = operatorStack.Pop();
                        float arg2 = operandStack.Pop();
                        float arg1 = operandStack.Pop();
                        operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                    }
                    operatorStack.Push(token);
                }
                else
                {
                    operandStack.Push(float.Parse(token));
                }
                tokenIndex += 1;
            }

            while (operatorStack.Count > 0) {
                string op = operatorStack.Pop();
                float arg2 = operandStack.Pop();
                float arg1 = operandStack.Pop();
                operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
            }
            return operandStack.Pop();
        }

        private string getSubExpression(List<string> tokens, ref int index) {
            StringBuilder subExpr = new StringBuilder();
            int parenlevels = 1;
            index += 1;
            while (index < tokens.Count && parenlevels > 0) {
                string token = tokens[index];
                if (tokens[index] == "(")
                {
                    parenlevels += 1;
                }

                if (tokens[index] == ")")
                {
                    parenlevels -= 1;
                }

                if (parenlevels > 0)
                {
                    subExpr.Append(token);
                }

                index += 1;
            }

            if ((parenlevels > 0))
            {
                throw new ArgumentException("Mis-matched parentheses in expression");
            }
            return subExpr.ToString();
        }

        private List<string> getTokens(string expression) {
            string operators = "()^*/+-";
            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in expression.Replace(" ", string.Empty)) {
                if (operators.IndexOf(c) >= 0) {
                    if ((sb.Length > 0)) {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add(c.ToString());
                } else {
                    sb.Append(c);
                }
            }

            if ((sb.Length > 0))
            {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }
    }
}
