using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MyTests
{
    public interface IExpressionVisitor
    {
        void Visit(Literal expression);
        void Visit(Variable expression);
        void Visit(BinaryExpression expression);
        void Visit(ParenExpression expression);
    }
    
    public interface IExpression
    {
        void Accept(IExpressionVisitor visitor);
    }

    public class Literal : IExpression
    {
        public Literal(string value)
        {
            Value = value;
        }

        public readonly string Value;
        
        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Literal) obj;
            return other.Value.Equals(Value);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }

    public class Variable : IExpression
    {
        public Variable(string name)
        {
            Name = name;
        }

        public readonly string Name;
        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Variable) obj;
            return other.Name.Equals(Name);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
    
    public class BinaryExpression : IExpression
    {
        public readonly IExpression FirstOperand;
        public readonly IExpression SecondOperand;
        public readonly string Operator;

        public BinaryExpression(IExpression firstOperand, IExpression secondOperand, string @operator)
        {
            FirstOperand = firstOperand;
            SecondOperand = secondOperand;
            Operator = @operator;
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (BinaryExpression) obj;
            return other.FirstOperand.Equals(FirstOperand) && 
                   other.SecondOperand.Equals(SecondOperand) && 
                   other.Operator.Equals(Operator);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (FirstOperand != null ? FirstOperand.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SecondOperand != null ? SecondOperand.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Operator != null ? Operator.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
    
    public class ParenExpression : IExpression
    {
        public ParenExpression(IExpression operand)
        {
            Operand = operand;
        }

        public readonly IExpression Operand;
        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (ParenExpression) obj;
            return other.Operand.Equals(Operand);
        }

        public override int GetHashCode()
        {
            return (Operand != null ? Operand.GetHashCode() : 0);
        }
    }
    
    
    public class SimpleParser
    {
        private static readonly Dictionary<char, int> Priorities = 
            new Dictionary<char, int>{{'+', 1}, {'-', 1}, {'*', 2}, {'/', 2}, {'(', 0}, {')', 0}};

        public static IExpression Parse(string text)
        {
            var terms = new Stack<IExpression>();
            var operations = new Stack<char>();
            
            foreach (var ch in text)
            {
                if (ch == '(')
                {
                    operations.Push(ch);
                }
                else if (ch == ')')
                {
                    while (operations.Count > 0 && operations.Peek() != '(')
                    {
                        if (!ProcessExpression(operations.Pop(), ref terms))
                        {
                            return null;
                        }

                        operations.Pop();
                        terms.Push(new ParenExpression(terms.Pop()));   
                    }
                }
                else if (Priorities.ContainsKey(ch))
                {
                    while (operations.Count > 0 && Priorities[operations.Peek()] >= Priorities[ch])
                    {
                        if (!ProcessExpression(operations.Pop(), ref terms))
                        {
                            return null;
                        }
                    }
                    operations.Push(ch);
                }
                else if (char.IsDigit(ch))
                {
                    terms.Push(new Literal(ch.ToString()));
                }
                else if (char.IsLetter(ch))
                {
                    terms.Push(new Variable(ch.ToString()));
                }
                else if (!char.IsWhiteSpace(ch))
                {
                    Console.WriteLine("Malformed expression: unrecognized symbol " + ch);
                }
            }

            while (operations.Count > 0) 
            {
                ProcessExpression(operations.Pop(), ref terms);
            }

            return terms.Pop();
        }

        private static bool ProcessExpression(char op, ref Stack<IExpression> terms)
        {
            if (terms.Count < 2)
            {
                Console.WriteLine("Malformed expression");
                return false;
            }

            var right = terms.Pop();
            var left = terms.Pop();
            
            terms.Push(new BinaryExpression(left, right, op.ToString()));

            return true;
        }
    }
    
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestLiteral()
        {
            var expression = SimpleParser.Parse("4");
            Assert.AreEqual(new Literal("4"),expression);
        }
        
        [Test]
        public void TestVariable()
        {
            var expression = SimpleParser.Parse("a");
            Assert.AreEqual(new Variable("a"), expression);
        }
        
        [Test]
        public void TestAddition()
        {
            var expression = SimpleParser.Parse("2+3");
            Assert.AreEqual(new BinaryExpression(new Literal("2"), new Literal("3"), "+"), expression);
        }
        
        [Test]
        public void TestSubtraction()
        {
            var expression = SimpleParser.Parse("3-2");
            Assert.AreEqual(new BinaryExpression(new Literal("3"), new Literal("2"), "-"), expression);
        }
        
        [Test]
        public void TestMultiplication()
        {
            var expression = SimpleParser.Parse("2*3");
            Assert.AreEqual(new BinaryExpression(new Literal("2"), new Literal("3"), "*"), expression);
        }
        
        [Test]
        public void TestDivision()
        {
            var expression = SimpleParser.Parse("4/2");
            Assert.AreEqual(new BinaryExpression(new Literal("4"), new Literal("2"), "/"), expression);
        }
        
        [Test]
        public void TestAdditionAndMultiplication()
        {
            var expression = SimpleParser.Parse("2+3*4");
            Assert.AreEqual(new BinaryExpression(new Literal("2"),
                    new BinaryExpression(new Literal("3"), new Literal("4"), "*"),
                    "+"),
                expression);
        }

        [Test]
        public void TestParentheses()
        {
            var expression = SimpleParser.Parse("(2+3)*4");
            Assert.AreEqual(new BinaryExpression(
                    new ParenExpression(
                        new BinaryExpression(
                            new Literal("2"),
                            new Literal("3"),
                            "+")),
                    new Literal("4"),
                    "*"),
                expression);
        }

        [Test]
        public void MixedVariablesAndLiterals()
        {
            var expression = SimpleParser.Parse("4 * a + b * 5");
            Assert.AreEqual(new BinaryExpression(
                    new BinaryExpression(
                        new Literal("4"),
                        new Variable("a"),
                        "*"),
                    new BinaryExpression(
                        new Variable("b"),
                        new Literal("5"),
                        "*"),
                    "+"),
                expression);
        }
    }
}