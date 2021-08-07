/****************************************************
  文件：CTIndirectInput.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/27 18:14:36
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CTIndirectInput : MonoBehaviour 
{
    MyInputActions inputActions;

    private void Awake()
    {
        inputActions = new MyInputActions();
        inputActions.Player.Jump.performed += Jump_performed;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void Jump_performed(CallbackContext obj)
    {
        Debug.Log(obj.ReadValueAsButton()); //按下true,松开false
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

}