/****************************************************
  文件：FSMState.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/3 22:19:12
  功能：Nothing
*****************************************************/
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.AISys
{
    public abstract class FSMState : IRelease
    {
        #region 通用属性
        //过渡--状态表
        public TransitionProfile profile;
        //当前状态ID
        public StateID stateID;
        //管理者
        public FSMSystem fsm;
        #endregion

        #region 特殊属性：适用于人物动作，场景机关等
        //当前过程
        public ProcessState processState = ProcessState.Ready;
        //动作优先级
        //（优先级低的动作会被高的动作打断，否则要等高优先结束当前轮次）
        public int priority = 1;
        #endregion

        public FSMState(FSMSystem fsm)
        {
            this.fsm = fsm;
        }
        
        //添加一个过渡键值对
        public void AddTransition(Transition trans, StateID id)
        {
            if(!profile.transition.ContainsKey(trans))
            {
                profile.transition.Add(trans, id);
            }
        }

        //删除一个过渡键值对
        public void RemoveTransition(Transition trans)
        {
            if(profile.transition.ContainsKey(trans))
            {
                profile.transition.Remove(trans);
            }
            else
            {
                DebugMgr.Log("删除过渡键值对失败，没有找到Key: " + trans, SystemEnum.AISystem);
            }
        }

        //得到一个过渡键的值
        public StateID GetOutputState(Transition trans)
        {
            if(profile.transition.ContainsKey(trans))
            {
                return profile.transition[trans];
            }
            else
            {
                DebugMgr.Log("获取过渡键值失败，没有找到Key: " + trans, SystemEnum.AISystem);
                return StateID.NullStateID;
            }
        }

        //执行状态前
        public virtual void DoBeforeEntering() { }

        //状态退出前
        public virtual void DoBeforeLeaving() { }

        //状态决策
        public abstract void Reason();

        //状态执行
        public abstract void Act();

        //状态销毁
        public virtual void Release()
        {
            fsm = null;
            profile = null;
            stateID = StateID.NullStateID;
        }
    }
}
