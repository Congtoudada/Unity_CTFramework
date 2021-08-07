/****************************************************
  文件：CTSample5.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/27 20:23:50
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class CTSample5 : MonoBehaviour 
{
    //所有动作结束或被中断均会触发canceled

    //PressOnly	按下的时候触发started和performed
    public void OnPress(CallbackContext context)
    {
        switch(context.phase)
        {
            case InputActionPhase.Started:
                Debug.Log("OnPress->Started");
                break;
            case InputActionPhase.Performed:
                Debug.Log("OnPress->Performed");
                break;
            case InputActionPhase.Canceled:
                Debug.Log("OnPress->Canceled");
                break;
            case InputActionPhase.Disabled:
                Debug.Log("OnPress->Disabled");
                break;
            case InputActionPhase.Waiting:
                Debug.Log("OnPress->Waiting");
                break;
        }
    }

    //ReleaseOnly 按下的时候触发started，松开的时候触发performed
    public void OnRelease(CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Debug.Log("OnRelease->Started");
                break;
            case InputActionPhase.Performed:
                Debug.Log("OnRelease->Performed");
                break;
            case InputActionPhase.Canceled:
                Debug.Log("OnRelease->Canceled");
                break;
            case InputActionPhase.Disabled:
                Debug.Log("OnRelease->Disabled");
                break;
            case InputActionPhase.Waiting:
                Debug.Log("OnRelease->Waiting");
                break;
        }
    }

    //OnPressAndRelease 按下的时候触发started和performed，松开的时候会再次触发performed
    public void OnPressAndRelease(CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Debug.Log("OnPressAndRelease->Started");
                break;
            case InputActionPhase.Performed:
                Debug.Log("OnPressAndRelease->Performed");
                break;
            case InputActionPhase.Canceled:
                Debug.Log("OnPressAndRelease->Canceled");
                break;
            case InputActionPhase.Disabled:
                Debug.Log("OnPressAndRelease->Disabled");
                break;
            case InputActionPhase.Waiting:
                Debug.Log("OnPressAndRelease->Waiting");
                break;
        }
    }

    //OnHold 适用于需要输入设备保持一段时间的操作。当按钮按下会触发started，若在松开按钮前，
    //按住时间大于等于Hold Time则会触发performed（时间一到就触发），否则触发canceled。
    public void OnHold(CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Debug.Log("OnHold->Started");
                break;
            case InputActionPhase.Performed:
                Debug.Log("OnHold->Performed");
                break;
            case InputActionPhase.Canceled:
                Debug.Log("OnHold->Canceled");
                break;
            case InputActionPhase.Disabled:
                Debug.Log("OnHold->Disabled");
                break;
            case InputActionPhase.Waiting:
                Debug.Log("OnHold->Waiting");
                break;
        }
    }

    //OnTap 和Hold相反，需要在一段时间内按下松开来触发。当按钮按下会触发started，
    //若在Max Tap Duriation时间内（小于）松开按钮，触发performed（此时不触发canceled），否则触发canceled。
    public void OnTap(CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Debug.Log("OnTap->Started");
                break;
            case InputActionPhase.Performed:
                Debug.Log("OnTap->Performed");
                break;
            case InputActionPhase.Canceled:
                Debug.Log("OnTap->Canceled");
                break;
            case InputActionPhase.Disabled:
                Debug.Log("OnTap->Disabled");
                break;
            case InputActionPhase.Waiting:
                Debug.Log("OnTap->Waiting");
                break;
        }
    }

    //OnSlowTap 类似Hold，但是它在按住时间大于等于Max Tap Duration，并不会立刻触发performed，
    //而是会在松开的时候才触发performed。时间小于Max Tap Duration触发canceled
    public void OnSlowTap(CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Debug.Log("OnSlowTap->Started");
                break;
            case InputActionPhase.Performed:
                Debug.Log("OnSlowTap->Performed");
                break;
            case InputActionPhase.Canceled:
                Debug.Log("OnSlowTap->Canceled");
                break;
            case InputActionPhase.Disabled:
                Debug.Log("OnSlowTap->Disabled");
                break;
            case InputActionPhase.Waiting:
                Debug.Log("OnSlowTap->Waiting");
                break;
        }
    }

    //OnMultiTap 用作于多次点击，例如双击或者三连击。Tap Count为点击次数，Max Tap Spacing为每次点击之间的间隔
    //（默认值为 2 * Max Tap Duration）。Max Tap Duration为每次点击的持续时间，即按下和松开按钮的这段时间。
    //当每次点击时间小于Max Tap Duration，且点击间隔时间小于Max Tap Spacing，点击Tap Count次，触发performed。否则触发canceled
    public void OnMultiTap(CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Debug.Log("OnMultiTap->Started");
                break;
            case InputActionPhase.Performed:
                Debug.Log("OnMultiTap->Performed");
                break;
            case InputActionPhase.Canceled:
                Debug.Log("OnMultiTap->Canceled");
                break;
            case InputActionPhase.Disabled:
                Debug.Log("OnMultiTap->Disabled");
                break;
            case InputActionPhase.Waiting:
                Debug.Log("OnMultiTap->Waiting");
                break;
        }
    }
}