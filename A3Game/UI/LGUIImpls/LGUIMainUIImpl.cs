using System;
using System.Collections;
using System.Collections.Generic;
using Cross;
using GameFramework;

namespace MuGame
{
    public class LGUIMainUIImpl_NEED_REMOVE 
    {
        public static LGUIMainUIImpl_NEED_REMOVE instance;
        public LGUIMainUIImpl_NEED_REMOVE()
        {
            instance = this;
        }

        public static LGUIMainUIImpl_NEED_REMOVE getInstance()
        {
            if (instance == null)
            {
                instance = new LGUIMainUIImpl_NEED_REMOVE();
            }
            return instance;
        }

        private Dictionary<string, processStruct> m_process = new Dictionary<string, processStruct>();

        protected const uint UI_LOADING_COUNT = 20;

        //static public IObjectPlugin create(IClientBase m)
        //{
        //    return new LGUIMainUIImpl_NEED_REMOVE(m as muUIClient);
        //}
        //override public void init()
        //{

        //}
        //override protected void _regEventListener()
        //{

        //}
        //override protected void _unRegEventListener()
        //{

        //}
        //override protected void onOpen(Variant data)
        //{
        //    base.onOpen(data);
        //}
        //private uint _selfYb;
        //private String _selfName;
        public const int NO_STATE = 0;		//无状态
        public const int COLLECT = 1;		//采集
        public const int FINDWAY = 2;		//寻路
        public const int AUTO_GAME = 3;		//挂机
        public const int RECOVER = 4;		//打坐
        protected int _CurAIState = -1;
        private bool _isFirstOpenMainUI = true;

        private float _turns_Interval = 1000;
        private float _cur_truns_tm = 0;

        //private GMCommand _gmCommand;

        //------------------------shortcurtbtns Start快速技能栏----------------------------------
        protected Variant _pkgItems = null;//背包道具
        protected Variant _skillArr = null;//技能数组
        protected Variant _clientConf = null;
        protected bool _isFirstInitShortcutbtns = true;

        protected float _saveQuickbarTm = 0;//快捷栏改变后经过多少时间向服务器保存设置(毫秒)  60000
        protected bool _needSave = false;

        private const uint CF_FAST_SKILL = 1;		//快捷技能
        private const uint CF_FUNCTION = 2;			//右侧功能面板
        private const uint CF_FUNCTBAR1 = 4;		//上方功能栏
        private const uint CF_MAINCHAT = 8;			//下方聊天
        private const uint CF_OPTION = 16;			//下方人物、背包、合成、锻造等
        private uint _startOpenFlag = 15;		//加上需要等待数据的
      
        //private lgSelfPlayer lgMainPlayer
        //{
        //    get
        //    {
        //        return (this.g_mgr.g_gameM as muLGClient).getObject(OBJECT_NAME.LG_MAIN_PLAY) as lgSelfPlayer;
        //    }
        //}
    
        public bool isShowAll
        {
            get
            {
                //return funbar1 != null && chainfo != null && centermsg1 != null && centermsg2 != null
                //    && centermsg3 != null && shortcutbtns != null && chat != null && rbmsg1 != null
                //    && rbmsg2 != null && rbmsg3 != null && rbmsgbox != null && mini_map != null
                //    && hpmp != null && funbar2 != null && funbar3 != null && midmsgbox != null
                //    && mapNamePan != null && optionPanel != null && mu_MainChat != null && ui_joystick != null
                //    && nextPanel != null && currentAct != null && screeneff != null && screeneffDown != null;
                return true;
            }
        }


        private void initBoard()
        {
            //lucisa 临时屏蔽新手指引

            NewbieModel.getInstance();
            NewbieModel.getInstance().initNewbieData();
            //lguiBoard.initBoard();
        }
        TickItem p;
        public void show_all()
        {
            if (_isFirstOpenMainUI)
            {
                _isFirstOpenMainUI = false;
                DelayDoManager.singleton.AddDelayDo(initBoard, 1);

                FunctionOpenMgr.init();
                InterfaceMgr.getInstance().initFirstUi();


                p = new TickItem(onProcess);
                TickMgr.instance.addTick(p);
            }
        }
    


        //-------------------------------------技能设置 end--------------------------------------------------------------------------
        static public bool CHECK_MAPLOADING_LAYER = false;
        static public bool TRY_SHOW_MAP_OBJ = false;
        static public bool TO_LEVEL = false;

        static private float waitShowMapObj = 0;
        private float recordTimer = 0;
        static public bool FIRST_IN_GAME = true;
        private float refreshTime = 2000;//刷新时间
        
        public void onProcess(float tmSlice)
        {
            if (recordTimer > 0)
            {
                if (UnityEngine.Time.time > recordTimer)
                {
                 //   LGPlatInfo.inst.logSDKCustomAP("run");
                    recordTimer = 0;
                }
            }

            if (!isShowAll)
            {
                GameTools.PrintNotice(" !isShowAll ");
                return;
            }

            
            if (TRY_SHOW_MAP_OBJ)
            {
                if (TO_LEVEL)
                {
                    TRY_SHOW_MAP_OBJ = false;
                }
                else
                {
                    waitShowMapObj++;
                    if (waitShowMapObj < 5)
                        return;

                    

                    TRY_SHOW_MAP_OBJ = false;
                    waitShowMapObj = 0;
                    MapProxy.getInstance().sendShowMapObj();
                }
            }

            if (CHECK_MAPLOADING_LAYER && LoaderBehavior.ms_HasAllLoaded)
            {
                CHECK_MAPLOADING_LAYER = false;
                if (FIRST_IN_GAME)
                {
                    // MapProxy.getInstance().sendShowMapObj();
                    FIRST_IN_GAME = false;

                    if (login.instance)
                    {
                        
                    }
                    else
                    {
                        if (maploading.instance != null)
                            maploading.instance.closeLoadWait(0.2f);
                    //    LGPlatInfo.inst.logSDKCustomAP("load");
                        
                        if (a3_mapname.instance)
                            a3_mapname.instance.refreshInfo();
                    }
               
                    recordTimer = UnityEngine.Time.time + 300;
                }
                else
                {
                    //    InterfaceMgr.getInstance().open(InterfaceMgr.LOADING_CLOUD);
                }

                //if (GRMap.HasNoWaitPlotSound())
                //{
                    //GRMap.ClearWaitPlotSound();

                    //请求播放剧情
                    if (loading_cloud.instance == null)
                    {
                        //gameST.REQ_PLAY_PLOT();
                    }
                    else
                    {
                        //loading_cloud.instance.hide(gameST.REQ_PLAY_PLOT);
                        loading_cloud.instance.hide(null);
                    }
                //}
            }
        } 
    }
}

