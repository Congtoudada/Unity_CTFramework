/****************************************************
  文件：CTPlayerInput.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/27 18:24:08
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CTPlayerInput : MonoBehaviour 
{
    //使用Send Messages或Broadcast Messages
    public void OnMove(InputValue value)
    {
        Debug.Log(transform.name + " Move: " + value.Get<Vector2>());
    }

    //使用UnityEvent
    public void MoveCallback(InputAction.CallbackContext obj)
    {
        Debug.Log(transform.name + " Move: " + obj.ReadValue<Vector2>());
    }
}