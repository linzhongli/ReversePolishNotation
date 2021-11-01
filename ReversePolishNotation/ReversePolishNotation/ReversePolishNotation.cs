using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ReversePolishNotation
{
    public partial class ReversePolishNotation
    {
        public delegate object GetValue(string s);

        internal class OutInfo { }

        internal class VarInfo : OutInfo { public string Value = null; }

        internal class OpInfo : OutInfo { public string Op = null; }

        internal partial class FunctionName
        {
            public const string sqrt = "Sqrt";
            public const string sin = "Sin";
            public const string cos = "Cos";
            public const string max = "Max";
            public const string min = "Min";
            public const string vector3 = "Vector3";
            public const string quaternion = "Quaternion";
            public const string dist = "Dist";
        }

        static Dictionary<string, int> OpPriority = new Dictionary<string, int>() {
            { ",", 0 },
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 },
            { "--", 3 },   //负号
            { FunctionName.sqrt ,4},
            { FunctionName.sin ,4},
            { FunctionName.cos ,4},
            { FunctionName.max ,4},
            { FunctionName.min ,4},
            { FunctionName.vector3 ,4},
            { FunctionName.quaternion ,4},
            { FunctionName.dist ,4},
            { "(", 9 },
            { ")", 9 },
        };

        private string InputStr;
        private Stack<OpInfo> OpStack;
        private List<OutInfo> OutList;
        private bool LastIsOp = true;  //判断最后一个是否是操作符

        public ReversePolishNotation(string inputStr)
        {
            InputStr = inputStr;

            OpStack = new Stack<OpInfo>();
            OutList = new List<OutInfo>();
            LastIsOp = true;
        }

        bool IsOp(char c)
        {
            return IsOp(c.ToString());
        }

        bool IsOp(string c)
        {
            if (OpPriority.ContainsKey(c.ToString()))
                return true;
            return false;
        }

        public void Transform()
        {
            InputStr = InputStr.Replace(" ", "");
            InputStr = InputStr.Replace("\r\n", "");
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < InputStr.Length; i++)
            {
                if (sb.Length > 0 && IsOp(InputStr[i]))
                {
                    var str = sb.ToString();
                    if (IsOp(str))
                    {
                        OpStack.Push(new OpInfo() { Op = str });
                        LastIsOp = true;
                    }
                    else
                    {
                        OutList.Add(new VarInfo() { Value = str });
                        LastIsOp = false;
                    }
                    sb.Clear();
                }

                switch (InputStr[i])
                {
                    case '+':
                    case '*':
                    case '/':
                    case ',':
                        OperateOpStack(InputStr[i].ToString());
                        LastIsOp = true;
                        break;
                    case '-':
                        {
                            if (LastIsOp)
                                OperateOpStack("--");//做为负号计算
                            else
                                OperateOpStack(InputStr[i].ToString());
                            LastIsOp = true;
                        }
                        break;
                    case '(':
                        OpStack.Push(new OpInfo() { Op = InputStr[i].ToString() });
                        LastIsOp = true;
                        break;
                    case ')':
                        OperationParen();
                        break;
                    default:
                        sb.Append(InputStr[i]);
                        break;
                }
            }

            if (sb.Length > 0)
            {
                var str = sb.ToString();
                OutList.Add(new VarInfo() { Value = str });
                sb.Clear();
                LastIsOp = false;
            }

            while (OpStack.Count > 0)
                OutList.Add(OpStack.Pop());
        }

        void OperateOpStack(string op)
        {
            while (OpStack.Count > 0)
            {
                OpInfo top = OpStack.Pop();
                if (top.Op == "(")
                {
                    OpStack.Push(top);
                    break;
                }

                if (OpPriority[op] <= OpPriority[top.Op])  //op的优先级 小于 top的优先级
                {
                    OutList.Add(top);
                }
                else
                {
                    OpStack.Push(top);
                    break;
                }
            }
            OpStack.Push(new OpInfo() { Op = op });
        }

        void OperationParen()
        {
            while (OpStack.Count > 0)
            {
                OpInfo topOp = OpStack.Pop();
                if (topOp.Op == "(")
                    break;
                else
                    OutList.Add(topOp);
            }
        }

        private void Solve(OpInfo opInfo, ref Stack<ValueWarp> s)
        {
            switch (opInfo.Op)
            {
                case "+":
                    {
                        var right = s.Pop();
                        var left = s.Pop();
                        var res = left + right;
                        s.Push(new ValueWarp(res));
                        break;
                    }
                case "-":
                    {
                        var right = s.Pop();
                        var left = s.Pop();
                        var res = left - right;
                        s.Push(new ValueWarp(res));
                        break;
                    }
                case "--":
                    {
                        var num = s.Pop();
                        s.Push(new ValueWarp(-num));
                        break;
                    }
                case "*":
                    {
                        var right = s.Pop();
                        var left = s.Pop();
                        var res = left * right;
                        s.Push(new ValueWarp(res));
                        break;
                    }
                case "/":
                    {
                        var right = s.Pop();
                        var left = s.Pop();
                        var res = left / right;
                        s.Push(new ValueWarp(res));
                        break;
                    }
                case FunctionName.sqrt:
                    {
                        float num = Convert.ToSingle(s.Pop().Value);
                        s.Push(new ValueWarp((float)Math.Sqrt(num)));
                        break;
                    }
                case FunctionName.sin:
                    {
                        float num = Convert.ToSingle(s.Pop().Value);
                        s.Push(new ValueWarp((float)Math.Sin(num)));
                        break;
                    }
                case FunctionName.cos:
                    {
                        float num = Convert.ToSingle(s.Pop().Value);
                        s.Push(new ValueWarp((float)Math.Cos(num)));
                        break;
                    }
                case FunctionName.max:
                    {
                        float right = Convert.ToSingle(s.Pop().Value);
                        float left = Convert.ToSingle(s.Pop().Value);
                        s.Push(new ValueWarp(Math.Max(left, right)));
                        break;
                    }
                case FunctionName.min:
                    {
                        float right = Convert.ToSingle(s.Pop().Value);
                        float left = Convert.ToSingle(s.Pop().Value);
                        s.Push(new ValueWarp(Math.Min(left, right)));
                        break;
                    }
                case FunctionName.vector3:
                    {
                        float right = Convert.ToSingle(s.Pop().Value);
                        float mid = Convert.ToSingle(s.Pop().Value);
                        float left = Convert.ToSingle(s.Pop().Value);
                        s.Push(new ValueWarp(new Vector3(left, mid, right)));
                        break;
                    }
                case FunctionName.quaternion:
                    {
                        float w = Convert.ToSingle(s.Pop().Value);
                        float z = Convert.ToSingle(s.Pop().Value);
                        float y = Convert.ToSingle(s.Pop().Value);
                        float x = Convert.ToSingle(s.Pop().Value);
                        s.Push(new ValueWarp(new Quaternion(x, y, z, w)));
                        break;
                    }
                case FunctionName.dist:
                    {
                        Vector3 right = (Vector3)s.Pop().Value;
                        Vector3 left = (Vector3)s.Pop().Value;
                        s.Push(new ValueWarp(Vector3.Distance(left, right)));
                        break;
                    }
                case ",":
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public object Calc(GetValue getValue)
        {
            Stack<ValueWarp> s = new Stack<ValueWarp>();

            for (int i = 0; i < OutList.Count; i++)
            {
                if (OutList[i] is OpInfo opInfo)
                {
                    Solve(opInfo, ref s);
                }
                else if (OutList[i] is VarInfo varInfo)
                {
                    if (varInfo.Value.StartsWith("@"))
                    {
                        object val = getValue(varInfo.Value);
                        s.Push(new ValueWarp(val));
                    }
                    else
                    {
                        object val = float.Parse(varInfo.Value.TrimEnd('f'));
                        s.Push(new ValueWarp(val));
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            return s.Pop().Value;
        }
    }
}