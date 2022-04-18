/****************************************************
  文件：_CT.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/14 15:25:25
  功能：框架Director
*****************************************************/
using CT.AuSys;
using CT.ResSys;
using CT.UISys;

namespace CT
{
    public class _CT : SingletonMono<_CT>
    {
        #region 框架核心模块
        public static IRes ResMgr { get; private set; }
        
        public static IUI UIMgr { get; private set; }

        public static IAudio AuMgr { get; private set; }

        #endregion

        private new void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            base.Awake();
            DontDestroyOnLoad(this);
            ResMgr = CTFactory.GetResMgr();
            UIMgr = CTFactory.GetUIMgr();
            AuMgr = CTFactory.GetAuMgr();
        }

    }
}