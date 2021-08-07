/****************************************************
  文件：CTButtonClick.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/28 0:06:59
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTButtonClick : MonoBehaviour 
{
    public void OnBtnClick(int id)
    {
        Debug.Log(this.transform.name + ":" + id);
    }
}