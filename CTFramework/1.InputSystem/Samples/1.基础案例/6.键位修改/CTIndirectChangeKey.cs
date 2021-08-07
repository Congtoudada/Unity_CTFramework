/****************************************************
  文件：CTIndirectChangeKey.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/27 21:04:50
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class CTIndirectChangeKey : MonoBehaviour 
{
    public PlayerInput input;
    public GameObject panel;

    #region 键盘改键
    public void OnIndirectChangeKeyClick()
    {
        Keyboard.current.onTextInput += OnTextInput;
        panel.SetActive(true);
    }

    private void OnTextInput(char ch)
    {
        //Debug.Log(ch);
        string keyPath = Keyboard.current.FindKeyOnCurrentKeyboardLayout(ch.ToString()).path;
        Debug.Log("检测到按键: " + ch + "--path: " + keyPath);

        //overridePath不会覆盖掉原path
        input.actions.FindActionMap("Player").FindAction("Press").ApplyBindingOverride(new InputBinding()
        {
            path = "<Keyboard>/q",
            overridePath = keyPath
        });
        //input.actions.FindActionMap("Player").FindAction("Press").ApplyBindingOverride(keyPath);

        Keyboard.current.onTextInput -= OnTextInput;
        panel.SetActive(false);
    }
    #endregion

    #region
    private bool isChangeOver = true;
    private string resultKey;
    public void OnIndirectChangeGamepadClick()
    {
        if(isChangeOver)
        {
            panel.SetActive(true);
            isChangeOver = false;
            StartCoroutine(ChangeGamepadEnumerator());
        }
    }

    IEnumerator ChangeGamepadEnumerator()
    {
        bool isChange = false;
        var gamepad = Gamepad.current;
        while (!isChange && gamepad != null)
        {
            if(gamepad.buttonNorth.wasPressedThisFrame)
            {
                resultKey = "<Gamepad>/buttonNorth";
                isChange = true;
            }
            yield return new WaitForEndOfFrame();
        }

        input.actions.FindActionMap("Player").FindAction("Press").ApplyBindingOverride(new InputBinding()
        {
            path = "<Gamepad>/buttonWest",
            overridePath = resultKey
        });

        panel.SetActive(false);
        isChangeOver = true;
    }



    #endregion
}