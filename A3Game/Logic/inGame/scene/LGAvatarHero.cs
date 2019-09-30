using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
using UnityEngine;
namespace MuGame
{

    class LGAvatarHero : LGAvatarGameInst
    {
        public LGAvatarHero(gameManager m)
            : base(m)
        {
        }

        override public uint getIid()
        {
            return viewInfo["iid"]._uint;
        }
        override public uint getMid()
        {
            return viewInfo["mid"]._uint;
        }

        public int ownid;
        public bool isUserOwnHero = false;

        public void initData(Variant info)
        {//{ x, y, name, mid, iid }
            viewInfo = info;
            int mid = info["mid"]._int;

            //viewInfo["name"] = HeroModel.getInstance().getHeroDataById(mid).name;
            ownid = viewInfo["owner_cid"];


            isUserOwnHero = ownid == PlayerModel.getInstance().cid;

            //  setConfig(d);

            muGRClient.instance.createHero(this);

            if (viewInfo.ContainsKey("moving"))
            {
                addMoving(viewInfo["moving"]);
            }

            if (viewInfo.ContainsKey("atking"))
            {
                addAttack(viewInfo["atking"]["tar_iid"]._uint, null);
            }

            Variant oriconf = localGeneral.GetMonOriConf(mid);
            if (oriconf != null)
            {
                viewInfo["ori"] = oriconf["octOri"]._int * GameConstant.ORI_UINT;
            }

            viewInfo["ori"] = 2 * GameConstant.ORI_UINT;

            //Variant owner_mon = localGeneral.GetOwnerMonConf(mid);
            //if (owner_mon != null && info.ContainsKey("owner_cid"))
            //{
            //    playerInfos.GetPlayerInfoByCid(info["owner_cid"], _getOwnerInfoBack);
            //}
            //else
            //{
            this.dispatchEvent(GameEvent.Create(GAME_EVENT.SPRITE_SET_DATA, this, this.viewInfo));
            //  }

            //this.addEventListener( GAME_EVENT.SPRITE_ON_CLICK, onClick );

        }

        private void _getOwnerInfoBack(uint cid, Variant data)
        {
            if (this.destroy) return;
            Variant ownerMon = localGeneral.GetOwnerMonConf(m_conf["mid"]);
            string monName = LanguagePack.getLanguageText("monName", m_conf["mid"]._str);
            string name = LanguagePack.getLanguageText("common", "belongsto");
            viewInfo["name"] = DebugTrace.Printf(name, data["name"]._str, monName);

            viewInfo["titleConf"] = getTitleConf("name", ownerMon["showtp"]._int, GameTools.createGroup("text", viewInfo["name"]));

            this.dispatchEvent(GameEvent.Create(GAME_EVENT.SPRITE_SET_DATA, this, this.viewInfo));

        }

        protected override void onMovePosSingleReach(GameEvent e)
        {
            setMeshAni("idle", 0);
        }

        //protected override void onClick(GameEvent e)
        //{
        //    base.onClick(e);
        //    GameTools.PrintNotice("monster click mid[" + getMid() + "] iid[" + getIid() + "]!");
        //    selfPlayer.onSelectMonster(this);
        //}

        protected override void playAttackAni(bool criatk = false)
        {
         //   return;

         //   debug.Log(":::" + m_ani);
            if (m_ani == "run" || m_ani == "idle")
            {

                base.playAttackAni(criatk);
                Variant mon = MonsterConfig.instance.getMonster(viewInfo["mid"]._str);
                if (mon.ContainsKey("atk_effect"))
                    MapEffMgr.getInstance().play(mon["atk_effect"], pGameobject.transform, 450f - lg_ori_angle, 0f);


            }


            //if (attConf != null)
            //{
            //    this.g_mgr.g_uiM.dispatchEvent(
            //        GameEvent.Create(GAME_EVENT.LG_MEDIA_PLAY, this,
            //        GameTools.createGroup("sid", attConf["sid"]._str, "loop", false))
            //        );
            //}
        }



        private SvrMonsterConfig svrMonsterConf
        {
            get
            {
                return (this.g_mgr.g_gameConfM as muCLientConfig).svrMonsterConf;
            }
        }
        private ClientGeneralConf localGeneral
        {
            get
            {
                return (this.g_mgr.g_gameConfM as muCLientConfig).localGeneral;
            }
        }

        public bool IsBoss()
        {
            return false;
        }

        public override void Die(Variant data)
        {
            base.Die(data);
        }

        override public string processName
        {
            get
            {
                return "LGAvatarMonster";
            }
            set
            {
                _processName = value;
            }

        }



    }
}
