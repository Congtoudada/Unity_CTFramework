/****************************************************
  文件：FSMEnum.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/3 22:49:56
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.AISys
{
    //过渡条件枚举
    public enum Transition
    {
        NullTransition = 0,
    }

    //状态枚举
    public enum StateID
    {
        NullStateID = 0,
    }

    //过程枚举：具体由Reason决策，Act调整状态
    public enum ProcessState
    {
        Ready,      //就绪态：可以随时切换状态
        Before,     //前摇：一般不可被打断
        Running,  //动作中：判定结果，一般不可被打断
        After,       //后摇：可以被高优先级动作打断
        //Over        //后摇结束：瞬时，一轮攻击结束，可以转换状态
    }
}
