// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day18 : Day
{
    private static readonly Regex _validCharacters = new("^[ 0-9+*()]+$");

    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        foreach (string line in input)
        {
            if (line.Length < 3)
            {
                errorMessage = $"Input line \"{line.Left(20, true)}\" is too short.";
                return false;
            }
            if (line.Length > 255)
            {
                errorMessage = $"Input line \"{line.Left(20, true)}\" is too long.";
                return false;
            }
            if (!_validCharacters.IsMatch(line))
            {
                errorMessage = $"Input line \"{line.Left(20, true)}\" contains invalid characters.";
                return false;
            }
            int parens = 0;
            foreach (char c in line)
            {
                if (c == '(')
                {
                    parens++;
                }
                else if (c == ')')
                {
                    parens--;
                }
                if (parens < 0)
                {
                    errorMessage = $"Input line \"{line.Left(20, true)}\" contains mismatched parentheses.";
                }
            }
            if (parens != 0)
            {
                errorMessage = $"Input line \"{line.Left(20, true)}\" contains mismatched parentheses.";
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();
        List<TokenizedExpression> expressions = input.Select(line => new TokenizedExpression(line)).ToList();
        foreach (TokenizedExpression expression in expressions)
        {
            if (!expression.Success)
            {
                return output.WriteError($"Invalid expression \"{expression.Original.Left(20, true)}\".");
            }
        }

        long partOneAnswer = 0;
        foreach (TokenizedExpression expression in expressions)
        {
            Queue<Token> parsed = Parse(new Queue<Token>(expression.Tokens));
            if (!TryEvaluate(parsed, out long result))
            {
                return output.WriteError($"Unable to evaluate \"{expression.Original.Left(20, true)}\".");
            }
            partOneAnswer += result;
        }

        long partTwoAnswer = 0;
        foreach (TokenizedExpression expression in expressions)
        {
            Queue<Token> parsed = Parse(expression.Tokens, false);
            if (!TryEvaluate(parsed, out long result))
            {
                return output.WriteError($"Unable to evaluate \"{expression.Original.Left(20, true)}\".");
            }
            partTwoAnswer += result;
        }

        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private static Queue<Token> Tokenize(string expression)
    {
        Queue<Token> tokens = new();
        foreach (char c in expression)
        {
            if (c == ' ') { continue; }
            tokens.Enqueue(new Token(c));
        }
        return tokens;
    }

    // https://en.wikipedia.org/wiki/Shunting_yard_algorithm
    private static Queue<Token> Parse(Queue<Token> tokens, bool isPartOne = true)
    {
        Queue<Token> output = new();
        Stack<Token> operatorStack = new();
        while (tokens.Any())
        {
            Token current = tokens.Dequeue();
            switch (current.Type)
            {
                case TokenType.Integer:
                    output.Enqueue(current);
                    break;
                case TokenType.Addition:
                case TokenType.Multiplication:
                    while (operatorStack.Any() && operatorStack.Peek().Type != TokenType.LeftParenthesis
                        && ((operatorStack.Peek().Type == TokenType.Addition
                        && current.Type == TokenType.Multiplication) || isPartOne))
                    {
                        output.Enqueue(operatorStack.Pop());
                    }
                    operatorStack.Push(current);
                    break;
                case TokenType.LeftParenthesis:
                    operatorStack.Push(current);
                    break;
                case TokenType.RightParenthesis:
                    while (operatorStack.Peek().Type != TokenType.LeftParenthesis)
                    {
                        output.Enqueue(operatorStack.Pop());
                    }
                    operatorStack.Pop();
                    break;
                default:
                    break;
            }
        }
        while (operatorStack.Any())
        {
            output.Enqueue(operatorStack.Pop());
        }
        return output;
    }

    private static bool TryEvaluate(Queue<Token> tokens, out long result)
    {
        result = 0;
        Stack<long> stack = new();
        while (tokens.Any())
        {
            Token current = tokens.Dequeue();
            if (current.Type == TokenType.Integer)
            {
                stack.Push(current.Value);
            }
            else if (stack.Count < 2)
            {
                return false;
            }
            else if (current.Type == TokenType.Addition)
            {
                stack.Push(stack.Pop() + stack.Pop());
            }
            else if (current.Type == TokenType.Multiplication)
            {
                stack.Push(stack.Pop() * stack.Pop());
            }
            else
            {
                return false;
            }
        }
        if (stack.Count != 1)
        {
            return false;
        }
        result = stack.Pop();
        return true;
    }

    private class TokenizedExpression
    {
        public bool Success { get; private set; }
        public Queue<Token> Tokens { get; private set; }
        public string Original { get; private set; }

        public TokenizedExpression(string input)
        {
            Original = input;
            Tokens = Tokenize(input);
            Success = !Tokens.Any(t => t.Type == TokenType.Invalid);
        }
    }

    private class Token
    {
        public TokenType Type { get; private set; }
        public int Value { get; private set; }

        public Token (char character)
        {
            Type = character switch
            {
                char c when ('0' <= c) && (c <= '9') => TokenType.Integer,
                '+' => TokenType.Addition,
                '*' => TokenType.Multiplication,
                '(' => TokenType.LeftParenthesis,
                ')' => TokenType.RightParenthesis,
                _ => TokenType.Invalid
            };
            Value = character - '0';
        }
    }

    private enum TokenType
    {
        Invalid = -1,
        Integer,
        Addition,
        Multiplication,
        LeftParenthesis,
        RightParenthesis
    }
}
