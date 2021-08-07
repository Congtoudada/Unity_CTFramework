/****************************************************
	文件：TimelinePlaybackManager.cs
	作者：聪头
	邮箱: 1322080797@qq.com
	日期：2020/12/13 22:50   	
	功能：Timeline管理类，玩家需要有PlayerTag标签
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace CT.Tools
{
    public class TimelinePlaybackManager : MonoBehaviour
    {
        [Header("Timeline References")]
        public PlayableDirector playableDirector;
        [Header("Timeline Settings")]
        [Tooltip("是否只播放一次Timeline")]
        public bool playTimelineOnlyOnce;
        [Header("Player Settings")]
        [Tooltip("Timeline播放时，是否接受玩家输入，默认为false允许玩家输入")]
        public bool disablePlayerInput = false;
        [Tooltip("进入区域后触发Timeline的交互按键，为None则不交互直接触发")]
        public KeyCode interactKey = KeyCode.None;


        [Header("Player Timeline Position")]
        [Tooltip("是否改变玩家位置和朝向，默认为false不改变")]
        public bool setPlayerTimelinePosition = false;
        [Tooltip("仅在setPlayerTimelinePosition为false时有效，更改后玩家的Transform值")]
        public Transform playerTimelinePosition;

        [Header("Trigger Zone Settings")]
        public GameObject triggerZoneObject;

        [Header("UI Interact Settings")]
        [Tooltip("是否显示UI，默认为false不显示")]
        public bool displayUI = false;
        [Tooltip("仅在displayUI为true时有效,Timeline播放前打开的UI界面")]
        public GameObject interactDisplay;

        [Header("Player Settings")]
        [Tooltip("主角标签")]
        public string playerTag = "Player";

        private GameObject player;    //人物对象
        private bool playerInZone = false;  //是否进入交互区域
        private bool timelinePlaying = false;   //是否播放TimeLine
        private float timelineDuration;     //TimeLine播放持续时间
        private bool timelineOver = true;
        void Start()
        {
            player = GameObject.FindWithTag(playerTag);
            playableDirector.stopped += TimelineOver;
            ToggleInteractUI(false);
        }
        //进入交互区域就调用
        public void PlayerEnteredZone()
        {
            playerInZone = true;
            ToggleInteractUI(true);
        }
        //离开交互区域就调用
        public void PlayerExitedZone()
        {
            playerInZone = false;
            ToggleInteractUI(false);
        }
        //控制UI是否开启
        void ToggleInteractUI(bool newState)
        {
            if (displayUI)
            {
                //如果有展示UI则展示
                interactDisplay.SetActive(newState);
            }
        }

        void Update()
        {
            if (playerInZone && !timelinePlaying)
            {
                bool activateTimelineInput = Input.GetKey(interactKey);
                if (interactKey == KeyCode.None)
                {
                    //外部没有指定交互按键，就直接播放
                    PlayTimeline();
                }
                else
                {
                    //外部有指定的话，那么只有按下才播放
                    if (activateTimelineInput)
                    {
                        PlayTimeline();
                        ToggleInteractUI(false);
                    }
                }
            }
        }
        public void PlayTimeline()
        {
            if (setPlayerTimelinePosition)
            {
                //是否修正朝向
                SetPlayerToTimelinePosition();
            }
            if (playableDirector)
            {
                //如果存在TimeLine就播放
                playableDirector.Play();
            }
            triggerZoneObject.SetActive(false);
            timelinePlaying = true;
            StartCoroutine(WaitForTimelineToFinish());
        }
        //瞬间修改人物朝向
        void SetPlayerToTimelinePosition()
        {
            player.transform.localPosition = playerTimelinePosition.position;
            player.transform.localRotation = playerTimelinePosition.rotation;
        }
        //不交互模式-等待Timeline播放结束的协程
        IEnumerator WaitForTimelineToFinish()
        {
            timelineDuration = (float)playableDirector.duration;    //获得动画时长

            ToggleInput(false);
            timelineOver = false;
            yield return new WaitUntil(() => timelineOver);
            ToggleInput(true);
            //判断是否只播放一次	
            if (!playTimelineOnlyOnce)
            {
                triggerZoneObject.SetActive(true);
            }
            playerInZone = false;
            timelinePlaying = false;
        }
        //播放过程中锁定玩家输入
        void ToggleInput(bool newState)
        {
            if (disablePlayerInput)
            {
                //TODO 为true，禁止玩家输入，具体逻辑根据需求编写
                //InputManager.IsInput = newState;
            }
        }
        public void TimelineOver(PlayableDirector pd)
        {
            timelineOver = true;
        }
    }
}


