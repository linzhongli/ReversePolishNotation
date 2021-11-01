## ReversePolishNotation

### 原理
利用逆波兰表达式计算算术表达式


### 已实现

+ 含有括号
+ 含有负号
+ 含有数字、向量、四元数的加减乘除
+ 含有自定义值的变量
+ 含有常用的函数Max,Min,Sqrt,Dist,Sin,Cos等,详细见FunctionName类
    
###实例
	
* 123 + 456
* @Key1 + 123
* -Vector3(1.0f,1,1)
* Sqrt(2)
* Sin(60)
* Dist(@Key2 , Vector3(1,1,1))


### 使用

```
ReversePolishNotation rpn = new ReversePolishNotation(equation);
rpn.Transform();      			//解析算式构造逆波兰表达式
var res = rpn.Calc(GetValue);   //计算并返回结果object类型
```
函数Calc的参数GetValue是个委托,传入字符串Key返回对应的值，用于自定义值的变量