/****************************************************
	�ļ���TimelineTriggerArea.cs
	���ߣ���ͷ
	����: 1322080797@qq.com
	���ڣ�2020/12/13 22:50   	
	���ܣ�
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.Tools
{
    public class TimelineTriggerArea : MonoBehaviour
    {
        [Header("Component References")]
        public TimelinePlaybackManager timelinePlaybackManager;
        [Header("Settings")]
        public string playerString = "Player";
        void OnTriggerEnter(Collider theCollision)
        {
            Debug.Log("Trigger:" + theCollision.tag);
            if(theCollision.CompareTag(playerString))
            {
                timelinePlaybackManager.PlayerEnteredZone();
            }
        }
        void OnTriggerExit(Collider theCollision)
        {
            timelinePlaybackManager.PlayerExitedZone();
        }
    }
}

