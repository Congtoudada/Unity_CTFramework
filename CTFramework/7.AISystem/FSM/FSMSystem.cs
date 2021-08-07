/****************************************************
  文件：FSMSystem.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/3 23:16:32
  功能：管理整个FSM
*****************************************************/
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.AISys
{
    public class FSMSystem : IRelease
    {
        public GameObject myself; //绑定的GameObject对象

        private Dictionary<StateID, FSMState> states; //状态字典
        private FSMState currentState; //当前状态
        public FSMState CurrentState { get { return currentState; } }

        public FSMSystem(GameObject myself)
        {
            this.myself = myself;
            states = new Dictionary<StateID, FSMState>();
        }
        //添加一个状态
        public void AddState(FSMState s)
        {
            if(!states.ContainsKey(s.stateID))
            {
                states.Add(s.stateID, s);
            }
            else
            {
                DebugMgr.Log(myself.name + "已经存在该状态了，无需重复添加", SystemEnum.AISystem);
            }
        }
        
        //移除一个状态
        public void RemoveState(StateID id)
        {
            if(states.ContainsKey(id))
            {
                ReleaseTool.TryRelease(states[id]);
                states.Remove(id);
            }
            else
            {
                DebugMgr.Log(myself.name + "的状态机中没有找到状态: " + id, SystemEnum.AISystem);
            }
        }
        
        //实现过渡
        public void PerformTransition(Transition trans)
        {
            StateID id = currentState.GetOutputState(trans);
            if(id == StateID.NullStateID)
            {
                if (states.ContainsKey(id))
                {
                    currentState.DoBeforeLeaving();
                    currentState = states[id];
                    currentState.DoBeforeEntering();
                }
            }
        }

        public void Release()
        {
            currentState = null;
            myself = null;
            foreach (StateID key in states.Keys)
            {
                ReleaseTool.TryRelease(states[key]);
            }
            states.Clear();
            states = null;
        }
    }
}
