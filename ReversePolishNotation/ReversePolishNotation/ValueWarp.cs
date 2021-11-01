using System;
using System.Collections.Generic;
using System.Numerics;

namespace ReversePolishNotation
{
    public struct ValueWarp
    {
        internal enum TypeEnum
        {
            NONE,
            FLOAT,
            VECTOR3,
            QUATERNION,
        }

        TypeEnum TYPE;
        float FloatVal;
        Vector3 Vector3Val;
        Quaternion QuaternionVal;

        public object Value
        {
            get
            {
                switch (TYPE)
                {
                    case TypeEnum.FLOAT:
                        return FloatVal;
                    case TypeEnum.VECTOR3:
                        return Vector3Val;
                    case TypeEnum.QUATERNION:
                        return QuaternionVal;
                }
                throw new NotImplementedException();
            }
        }

        public ValueWarp(object value)
        {
            TYPE = TypeEnum.NONE;
            FloatVal = default(float);
            Vector3Val = default(Vector3);
            QuaternionVal = default(Quaternion);

            switch (value)
            {
                case Vector3 vector3Val:
                    {
                        TYPE = TypeEnum.VECTOR3;
                        Vector3Val = vector3Val;
                        return;
                    }
                case Quaternion quaternionVal:
                    {
                        TYPE = TypeEnum.QUATERNION;
                        QuaternionVal = quaternionVal;
                        return;
                    }
            }

            var floatVal = Convert.ToSingle(value);
            TYPE = TypeEnum.FLOAT;
            FloatVal = floatVal;
        }

        public static object operator +(ValueWarp a, ValueWarp b)
        {
            return AddDict[(a.TYPE, b.TYPE)](a, b);
        }

        public static object operator -(ValueWarp a, ValueWarp b)
        {
            return SubDict[(a.TYPE, b.TYPE)](a, b);
        }

        public static object operator *(ValueWarp a, ValueWarp b)
        {
            return MulDict[(a.TYPE, b.TYPE)](a, b);
        }

        public static object operator /(ValueWarp a, ValueWarp b)
        {
            return DivDict[(a.TYPE, b.TYPE)](a, b);
        }

        public static object operator -(ValueWarp a)
        {
            return MinusDict[a.TYPE](a);
        }

        #region 加法
        static Dictionary<(TypeEnum, TypeEnum), Func<ValueWarp, ValueWarp, object>> AddDict = new Dictionary<(TypeEnum, TypeEnum), Func<ValueWarp, ValueWarp, object>>()
        {
            { (TypeEnum.FLOAT,TypeEnum.FLOAT ) , AddFloatFloat},
            { (TypeEnum.VECTOR3,TypeEnum.VECTOR3 ) , AddVector3Vector3},
        };

        static object AddFloatFloat(ValueWarp x, ValueWarp y)
        {
            return x.FloatVal + y.FloatVal;
        }

        static object AddVector3Vector3(ValueWarp x, ValueWarp y)
        {
            return x.Vector3Val + y.Vector3Val;
        }
        #endregion

        #region 减法
        static Dictionary<(TypeEnum, TypeEnum), Func<ValueWarp, ValueWarp, object>> SubDict = new Dictionary<(TypeEnum, TypeEnum), Func<ValueWarp, ValueWarp, object>>()
        {
            { (TypeEnum.FLOAT,TypeEnum.FLOAT ) , SubFloatFloat},
            { (TypeEnum.VECTOR3,TypeEnum.VECTOR3 ) , SubVector3Vector3},
        };

        static object SubFloatFloat(ValueWarp x, ValueWarp y)
        {
            return x.FloatVal - y.FloatVal;
        }

        static object SubVector3Vector3(ValueWarp x, ValueWarp y)
        {
            return x.Vector3Val - y.Vector3Val;
        }
        #endregion

        #region 乘法

        static Dictionary<(TypeEnum, TypeEnum), Func<ValueWarp, ValueWarp, object>> MulDict = new Dictionary<(TypeEnum, TypeEnum), Func<ValueWarp, ValueWarp, object>>()
        {
            { (TypeEnum.FLOAT,TypeEnum.FLOAT ) , MulFloatFloat},
            { (TypeEnum.FLOAT,TypeEnum.VECTOR3 ) , MulFloatVector3},
            { (TypeEnum.VECTOR3,TypeEnum.FLOAT ) , MulVector3Float},
            { (TypeEnum.QUATERNION,TypeEnum.VECTOR3 ),MulQuaternionVector3 },
            { (TypeEnum.QUATERNION,TypeEnum.QUATERNION ) , MulQuaternionQuaternion},
        };

        static object MulFloatFloat(ValueWarp x, ValueWarp y)
        {
            return x.FloatVal * y.FloatVal;
        }

        static object MulFloatVector3(ValueWarp x, ValueWarp y)
        {
            return x.FloatVal * y.Vector3Val;
        }

        static object MulVector3Float(ValueWarp x, ValueWarp y)
        {
            return x.Vector3Val * y.FloatVal;
        }

        static object MulQuaternionQuaternion(ValueWarp x, ValueWarp y)
        {
            return x.QuaternionVal * y.QuaternionVal;
        }

        static object MulQuaternionVector3(ValueWarp x, ValueWarp y)
        {
            return Vector3.Transform(y.Vector3Val, x.QuaternionVal);
        }
        #endregion

        #region 除法
        static Dictionary<(TypeEnum, TypeEnum), Func<ValueWarp, ValueWarp, object>> DivDict = new Dictionary<(TypeEnum, TypeEnum), Func<ValueWarp, ValueWarp, object>>()
        {
            { (TypeEnum.FLOAT,TypeEnum.FLOAT ) , DivFloatFloat},
            { (TypeEnum.VECTOR3,TypeEnum.VECTOR3 ) , DivVector3Float},
        };

        static object DivFloatFloat(ValueWarp x, ValueWarp y)
        {
            return x.FloatVal / y.FloatVal;
        }

        static object DivVector3Float(ValueWarp x, ValueWarp y)
        {
            return x.Vector3Val / y.FloatVal;
        }
        #endregion

        #region 负号

        static Dictionary<TypeEnum, Func<ValueWarp, object>> MinusDict = new Dictionary<TypeEnum, Func<ValueWarp, object>>()
        {
            { TypeEnum.FLOAT , MinusFloat},
            { TypeEnum.VECTOR3 , MinusVector3},
        };

        static object MinusFloat(ValueWarp x)
        {
            return -x.FloatVal;
        }

        static object MinusVector3(ValueWarp x)
        {
            return -x.Vector3Val;
        }

        #endregion
    }
}