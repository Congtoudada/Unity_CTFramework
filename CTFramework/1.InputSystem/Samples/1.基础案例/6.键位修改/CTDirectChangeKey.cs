/****************************************************
  文件：DirectChangeKey.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/27 20:58:16
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CTDirectChangeKey : MonoBehaviour 
{
    public PlayerInput input;

    private void Start()
    {
        input.actions.FindActionMap("Player").FindAction("Press").ApplyBindingOverride(
            new InputBinding()
            {
                path = "<Keyboard>/q",
                overridePath = "<Keyboard>/a"
            });
    }
}