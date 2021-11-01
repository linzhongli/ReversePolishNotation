using System;
using System.Numerics;
using System.Collections.Generic;

namespace ReversePolishNotation
{
    class Program
    {
        static void Main(string[] args)
        {
            Test.Start();
        }
    }

    class Test
    {
        public static void Start()
        {
            string[] equations = new string[] {
                "123",
                "@Key1 + 123",
                "-Vector3(1.0f,1,1)",
                "Sqrt(2)",
                "Sin(60)",
                "Dist(@Key2 , Vector3(1,1,1))",
            };

            foreach (var equation in equations)
                CalcEquation(equation);
        }

        static void CalcEquation(string equation)
        {
            Console.WriteLine("计算公式:" + equation);
            ReversePolishNotation rpn = new ReversePolishNotation(equation);
            rpn.Transform();
            var res = rpn.Calc(GetValue);
            Console.WriteLine("结果 = " + res);
        }

        static Dictionary<string, object> DataDict = new Dictionary<string, object>()
        {
            { "Key1" , 1 },
            { "Key2" , new Vector3(1,2,3) }
        };

        static object GetValue(string key)
        {
            //去除最开头的@
            key = key.Replace("@", "");

            if (DataDict.TryGetValue(key, out var res))
                return res;
            return null;
        }
    }
}
