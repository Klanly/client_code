using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
namespace MuGame
{
    class LGAvatarNpc : LGAvatarGameInst
    {
        public static Dictionary<int,TalkMainData> dTalk=new Dictionary<int,TalkMainData>();


        public LGAvatarNpc(gameManager m)
            : base(m)
        {

        }

        public int dialogId = -1;

        private List<int> titleImgs = new List<int>();

        private uint _npcid;
        public uint npcid
        {
            get
            {
                return _npcid;
            }
            set
            {
                _npcid = value;
            }

        }
        override public uint getNid()
        {
            return _npcid;
        }



        TalkMainData curTalkDta;
        public void doTalk()
        {
            if (dialogId == -1)
                return;

            string str = SvrNPCConfig.instance.get_dialog(dialogId);

            if (str == "")
                return;

            if (dTalk.ContainsKey(dialogId))
            {
                curTalkDta = dTalk[dialogId];
            }
            else
            {
                
                TalkMainData d = new TalkMainData();
                d.init(str, _npcid.ToString(), viewInfo["name"]);
                dTalk[dialogId] = d;
                curTalkDta = d;
            }
            curTalkDta.beginTalk(this);
        }


        public void initData(Variant info)
        {//{ x, y, name, nid }
            viewInfo = info;
         //   viewInfo["name"] = LanguagePack.getLanguageText("npcName", info["nid"]._str);
            _npcid = info["nid"];

            viewInfo["ori"] = info["octOri"]._int;

            this.g_mgr.g_sceneM.dispatchEvent(
                GameEvent.Createimmedi(GAME_EVENT.SCENE_CREATE_NPC, this, null)
            );

            this.dispatchEvent(GameEvent.Create(GAME_EVENT.SPRITE_SET_DATA, this, this.viewInfo));

            //this.addEventListener( GAME_EVENT.SPRITE_ON_CLICK, onClick );
        }
        override public void updateProcess(float tmSlice)
        {

        }
        protected override void onClick(GameEvent e)
        {
            base.onClick(e);
            GameTools.PrintNotice("npc click [" + getNid() + "] ");
            lgMainPlayer.onSelectNpc(this);
        }

        ////这个久的接口，已经不用了，可以重构掉不要了 jason
        //public override void AddTitleSprite(string tp, int showtp = 0, Variant showInfo = null)
        //{
        //    //if (titleImgs.Contains(showtp))
        //    //    return;

        //    //titleImgs.Add(showtp);

        //    //base.AddTitleSprite(tp, showtp, showInfo);
        //}

        //public override void RemoveTitleSprite(string tp, int showtp = 0)
        //{
        //    if (!titleImgs.Contains(showtp))
        //        return;

        //    titleImgs.Remove(showtp);

        //    base.RemoveTitleSprite(tp, showtp);
        //}


        override public string processName
        {
            get
            {
                return "LGAvatarNpc";
            }
            set
            {
                _processName = value;
            }
        }
    }

    class TalkMainData
    {
        public List<TalkDialogData> lDialog;
        private int curIdx;

        public string _name;
        public string _avatarid;

        public void init(string str, string avatarid, string name)
        {
            _name = name;
            _avatarid = avatarid;

            lDialog= new List<TalkDialogData>();
           string[] arr =  str.Split('|');
           for (int i = 0; i < arr.Length; i++)
           {
               TalkDialogData dd = new TalkDialogData();
               dd.init(arr[i]);
               lDialog.Add(dd);
            }
        }
        
        public void beginTalk(LGAvatarNpc npc)
        {
            curIdx=0;
         //   dialog.showTalk(this);
        }

        public TalkDialogData doTalk()
        {
            if (curIdx < lDialog.Count)
            {
                TalkDialogData dta  = lDialog[curIdx];
                curIdx++;
                return dta;
            }
            curIdx = 0;
            return null;
        }
    }

    class TalkDialogData
    {
        public bool isUser=false;
        public string cont="--";

        public void init(string str)
        {
            string[] arr = str.Split(':');
            if (arr.Length < 2)
                return;
            isUser = int.Parse(arr[0]) == 0;
            cont = arr[1];
        }
    }
}
