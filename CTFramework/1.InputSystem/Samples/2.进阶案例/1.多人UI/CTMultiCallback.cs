/****************************************************
  文件：CTMultiCallback.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/28 0:52:38
  功能：Nothing
*****************************************************/
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class CTMultiCallback : MonoBehaviour 
{
    public PlayerInputManager manager;
    public GameObject canvas;

    public void JoinCallback(PlayerInput player)
    {
        //Debug.Log(manager.playerCount); //有玩家连接就 + 1，该值至少为1
        //取得并激活相应面板
        GameObject panel = canvas.transform.GetChild(manager.playerCount - 1).gameObject;
        panel.SetActive(true);

        //面板绑定player
        MultiplayerEventSystem sys = player.transform.parent.Find("EventSystem").GetComponent<MultiplayerEventSystem>();
        sys.playerRoot = panel;
        sys.firstSelectedGameObject = panel.transform.GetChild(0).gameObject;
    }
}