/****************************************************
  文件：CTMultiDisconnect.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/28 11:15:40
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CTMultiDisconnect : MonoBehaviour 
{
    //当PlayerInput被删除时触发
    public void OnLeftEvent(PlayerInput input)
    {
        Debug.Log("有玩家离开");
    }
}