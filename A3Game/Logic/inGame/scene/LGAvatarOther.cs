using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
namespace MuGame
{


    class LGAvatarOther : LGAvatarGameInst
    {
        public LGAvatarOther(gameManager m)
            : base(m)
        {
        }

        //private uint _iid;
        //private uint _cid;

        override public uint getIid()
        {
            return viewInfo["iid"]._uint;
        }

        override public uint getCid()
        {
            return viewInfo["cid"]._uint;
        }

        protected override void onClick(GameEvent e)
        {
            base.onClick(e);
            GameTools.PrintNotice("other click iid[" + getIid() + "] "); 
            lgMainPlayer.onSelectOther(this);
        }

        public void initData(Variant info)
        {//{ x, y, name, sex, carr, iid }
            viewInfo = info;
            //playerInfos.get_player_detailinfo(getCid(), onGetDetialInfo);

            //增加视野内，有玩家时装改变的处理
            //playerInfos.addActionDressChange(getCid(), revDressChange);
        }

        private void onGetDetialInfo(Variant data)
        {
            if (this.destroy) return;
            foreach (string key in data.Keys)
            {
                viewInfo[key] = data[key];
            }

            if( viewInfo.ContainsKey("moving") )
			{
				addMoving( viewInfo["moving"] );
			}

			if( viewInfo.ContainsKey("atking") )
			{
				addAttack( viewInfo["atking"]["tar_iid"]._uint, null );
			}

            this.g_mgr.g_sceneM.dispatchEvent(
                GameEvent.Createimmedi(GAME_EVENT.SCENE_CREATE_OTHER_CHAR, this, null)
            );

            ////MU添加装备显示??
            //AddShowEqps( this.data["equip"], true );
            //debug.Log("玩家进入 " + this.viewInfo.dump() + "  data " + data.dump());

            //如果角色身上有时装
            if (data.ContainsKey("dressments"))
            {
                //debug.Log("进入视野的角色身上有时装---------------------------------------------------");
                grAvatar.m_nSex = data["sex"]._int;
                List<Variant> dressments = data["dressments"]._arr;
                foreach (Variant dressone in dressments)
                {
                    int ndressid = dressone["dressid"]._int;
                    SXML dressxml = XMLMgr.instance.GetSXML("dress.dress_info","id==" + dressone["dressid"]._str);
                    int npartid = dressxml.getInt("dress_type");
                    if (npartid >= 0 && npartid < grAvatar.m_nOtherDress.Length)
                    {
                        grAvatar.m_nOtherDress[npartid] = ndressid;
                    }

                    //debug.Log("他的时装的时装的ID " + dressone["dressid"]);
                    //grAvatar.RefreshOtherAvatar();
                }
            }

            this.dispatchEvent(GameEvent.Create(GAME_EVENT.SPRITE_SET_DATA, this, this.viewInfo));

            this.addEventListener(GAME_EVENT.SPRITE_ON_CLICK, onClick);
        }

        private void revDressChange(Variant data)
        {
            if (grAvatar == null)
                return;


            //如果角色身上有时装
            if (data.ContainsKey("dressments"))
            {
                //debug.Log("收到身上的时装改变了的消息");
                //grAvatar.m_nSex = data["sex"]._int;
                List<Variant> dressments = data["dressments"]._arr;
                int i=0;
                foreach (Variant dressone in dressments)
                {
                    int npartid = i;
                    int ndressid = dressone["dressid"]._int;
                    //if(ndressid>1000)
                    //    npartid = ndressid % 1000 - 1;
                    if (npartid >= 0 && npartid < grAvatar.m_nOtherDress.Length)
                    {
                        grAvatar.m_nOtherDress[npartid] = ndressid;
                    }
                    i++;
                    //debug.Log("他的时装的时装的ID " + dressone["dressid"]);
                    
                }
                grAvatar.RefreshOtherAvatar();
            }
        }
        
        override public string processName
		{
			get
			{
				return "LGAvatarOther";
			}
			set
			{
				_processName =value;
			}
			 
		}
    }
}
