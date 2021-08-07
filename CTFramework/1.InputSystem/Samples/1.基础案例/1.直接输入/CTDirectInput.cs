/****************************************************
  文件：CTDirectInput.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/27 18:10:28
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CTDirectInput : MonoBehaviour 
{
    public bool isGamepad;
    public bool isKeyboard;
    public bool isMouse;
    public bool isPointer;

    public bool gamepadToggle = true;

    private void Update()
    {
        Gamepad gamepad = Gamepad.current; //手柄
        Keyboard keyboard = Keyboard.current; //键盘
        Mouse mouse = Mouse.current; //鼠标
        Pointer pointer = Pointer.current; //指针

        #region 手柄篇
        if (gamepadToggle)
        {
            InputSystem.ResumeHaptics(); //对震动进行恢复，不影响按键
        }
        else
        {
            InputSystem.PauseHaptics(); //对震动进行停止，不影响按键
        }

        if (gamepad != null && isGamepad)
        {
            //手柄左摇杆偏移
            //Debug.Log("gamepad.leftStick: " + gamepad.leftStick.ReadValue());
            if (gamepad.bButton.wasPressedThisFrame)
            {
                Debug.Log("按下B键");
                gamepad.SetMotorSpeeds(0, 0);
            }
            if (gamepad.buttonNorth.wasPressedThisFrame)
            {
                Debug.Log("按下Y键");
                gamepad.SetMotorSpeeds(0, 0.75f);
            }
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                Debug.Log("按下A键");
                gamepad.SetMotorSpeeds(0.75f, 0);
            }
        }
        #endregion

        #region 键盘篇
        if (keyboard != null)
        {
            //执行顺序：isPressed = false -> 按下：wasPressedThisFrame = true -> 中途：isPressed = true -> 松开：wasReleasedThisFrame = true -> isPressed = false
            if (keyboard.wKey.wasPressedThisFrame)
                Debug.Log("w键按下（一次）");
            if (keyboard.wKey.wasReleasedThisFrame)
                Debug.Log("w键松开");
            //Debug.Log("是否按住w键: " + keyboard.wKey.isPressed);
            if (keyboard.numpad0Key.wasPressedThisFrame)
                Debug.Log("小键盘0");
            if (keyboard.digit0Key.wasPressedThisFrame)
                Debug.Log("数字0");
        }
        #endregion

        #region 鼠标篇
        Debug.Log(mouse.scroll.ReadValue()); //鼠标的滚动值，向前滚为正，向后滚为负
        if (mouse.leftButton.wasPressedThisFrame)
            Debug.Log("按鼠标左键");
        if (mouse.rightButton.wasPressedThisFrame)
            Debug.Log("按鼠标右键");
        if (mouse.middleButton.wasPressedThisFrame)
            Debug.Log("按鼠标中键");
        Debug.Log("鼠标位置: " + mouse.position.ReadValue()); //和pointer.position一致
        #endregion

        #region 指针篇
        if (pointer != null)
        {
            Debug.Log("指针位置: " + pointer.position.ReadValue()); //和mouse.position一致
            Debug.Log("指针偏移: " + pointer.delta.ReadValue());
        }
        #endregion
    }
}