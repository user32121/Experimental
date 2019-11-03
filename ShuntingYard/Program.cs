using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShuntingYard
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, double> opPrecedence = new Dictionary<string, double>() {
                {"POW", -1 },
                { "+", 0.1 }, { "-", 0.1 },
                { "*", 1.1 }, { "/", 1.1 },
            };

            while (true)
            {
                Queue<string> input = new Queue<string>(Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                Stack<string> opStack = new Stack<string>();
                Queue<string> output = new Queue<string>();

                while (input.Count > 0)
                {
                    string token = input.Dequeue(); //get token                
                    if (char.IsDigit(token[0]) || token.Count() > 1 && char.IsDigit(token[1]))    //is token a number
                        output.Enqueue(token);
                    else if (opPrecedence.TryGetValue(token, out double op1))   //function or operator
                    {
                        while (opStack.Count > 0 && opStack.Peek() != "(" &&    //not empty or left parentheses
                            opPrecedence.TryGetValue(opStack.Peek(), out double op2) && (op2 > op1 ||    //compare precedence
                            op2 == op1 && (int)op2 != op2))    //check left assosciative if it is equal
                            output.Enqueue(opStack.Pop());  //add to output queue
                        opStack.Push(token);
                    }
                    else if (token == "(")  //left parentheses
                        opStack.Push(token);
                    else if (token == ")")  //right parentheses
                    {
                        while (opStack.Peek() != "(")   //move from operator stack to output until parentheses
                            output.Enqueue(opStack.Pop());
                        opStack.Pop();
                    }
                }
                while (opStack.Count > 0)
                    output.Enqueue(opStack.Pop());  //move remaining operators to ouput

                while (output.Count > 0)
                {
                    Console.Write(output.Dequeue() + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
