/****************************************************
  文件：CTMultiCallback2.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/28 11:06:06
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CTMultiCallback2 : MonoBehaviour 
{
    public void MoveCallback(CallbackContext context)
    {
        Debug.Log(context.control.displayName +  ": " + context.ReadValue<Vector2>());
    }

}