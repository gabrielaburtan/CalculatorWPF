using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    partial class Operations
    {
        public Stack<double> numberStack = new Stack<double>(20);
        public Stack<string> operationStack = new Stack<string>(20);
        public double previousValue = 0;
        private string previousOperation = "";
        private double firstValueFromStack = 0;
        private double secondValueFromStack = 0;

        public string MakeOperation(string text, string typeOfOperation)
        {
            bool firstOperation = false;
            if (!operationStack.Any())
            {
                previousValue = previousValue + Double.Parse(text);
                numberStack.Push(previousValue);
                firstOperation = true;
            }
            else
            {
                numberStack.Push(Double.Parse(text));
                previousOperation = operationStack.Pop();
            }
            if(firstOperation == true)
            {
                operationStack.Push(typeOfOperation);
                return previousValue.ToString();
            }
            else
            {
                operationStack.Push(typeOfOperation);
            }
            switch (previousOperation)
            {
                case "+":
                    firstValueFromStack = numberStack.Pop();
                    secondValueFromStack = numberStack.Pop();
                    numberStack.Push(secondValueFromStack + firstValueFromStack);
                    return numberStack.Peek().ToString();
                case "-":
                    firstValueFromStack = numberStack.Pop();
                    secondValueFromStack = numberStack.Pop();
                    numberStack.Push(secondValueFromStack - firstValueFromStack);
                    return numberStack.Peek().ToString();
                case "*":
                    firstValueFromStack = numberStack.Pop();
                    secondValueFromStack = numberStack.Pop();
                    numberStack.Push(secondValueFromStack * firstValueFromStack);
                    return numberStack.Peek().ToString();
                case "/":
                    firstValueFromStack = numberStack.Pop();
                    secondValueFromStack = numberStack.Pop();
                    if (firstValueFromStack == 0)
                    {
                        return "Cannot divide by zero!";
                    }
                    else
                    {
                        numberStack.Push(secondValueFromStack / firstValueFromStack);
                        return numberStack.Peek().ToString();
                    }
                default:
                    return "0";
            }
        }

        public string EqualOperation(string text)
        {
            previousValue = 0;
            numberStack.Push(Double.Parse(text));
            if (numberStack.Count == 1)
                return numberStack.Peek().ToString();
            else
            {
                firstValueFromStack = numberStack.Pop();
                secondValueFromStack = numberStack.Pop();
                string operation = operationStack.Pop();
                double result = 0;
                switch (operation)
                {
                    case "+":
                        result = secondValueFromStack + firstValueFromStack;
                        numberStack.Push(result);
                        return result.ToString();
                    case "-":
                        result = secondValueFromStack - firstValueFromStack;
                        numberStack.Push(result);
                        return result.ToString();
                    case "*":
                        result = secondValueFromStack * firstValueFromStack;
                        numberStack.Push(result);
                        return result.ToString();
                    case "/":
                        if (firstValueFromStack == 0)
                        {
                            return "Cannot divide by zero!";                           
                        }
                        else
                        {
                            result = secondValueFromStack / firstValueFromStack;                            
                            numberStack.Push(result);
                            return result.ToString();
                        }
                    default:
                        return "0";
                }
            }
        }
    }
}
