using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
namespace MuGame
{
    class LGGRAvatarHero : LGGRAvatar, IObjectPlugin
    {

        private uint _iid = 0;
        public LGGRAvatarHero(muGRClient m)
            : base(m)
        {
        }

        public static IObjectPlugin create(IClientBase m)
        {
            return new LGGRAvatarHero(m as muGRClient);
        }



        override protected void initData(GameEvent e)
        {
            Variant data = e.data;
            _iid = data["iid"];

            uint mid = data["mid"]._uint;

            //mid = mid * 10;
            //SXML xml = XMLMgr.instance.GetSXML("hero.hero", "id=="+mid.ToString());
            //mid = xml.getUint("obj");

            this.avatarid = m_mgr.getHeroAvararId(mid);
            Variant avatarConf = m_mgr.getEntityConf(this.avatarid);
            if (avatarConf == null)
            {
                //DebugTrace.print( "mid [ "+mid+" ]avatarConf[ "+ this.avatarid +" ] is NULL!" );
            }

            Variant info = new Variant();
            info["isCreateName"] = true;
            if (data.ContainsKey("titleConf"))
            {
                info["titleConf"] = data["titleConf"];
            }
            else
            {
                info["titleConf"] = getTitleConf("name", 0, GameTools.createGroup("text", data["name"]));
            }

            info["name"] = data["name"];
            info["equip"] = null;
            info["avatarConf"] = avatarConf;

            info["max_hp"] = this.gameCtrl.max_hp;
            info["hp"] = this.gameCtrl.hp;
            info["mp"] = this.gameCtrl.mp;

            if (data.ContainsKey("ori"))
            {
                info["ori"] = data["ori"];
            }
            else
            {
                info["ori"] = 0;
            }
            info["ani"] = GameConstant.ANI_IDLE_NORMAL;
            info["loop"] = true;

            setPos(
                (m_gameCtrl as LGAvatar).x,
                (m_gameCtrl as LGAvatar).y
            );


            info["x"] = m_x;
            info["y"] = m_z;
            info["z"] = m_y;



            this.dispatchEvent(
                GameEvent.Create(GAME_EVENT.SPRITE_SET_DATA, this, info)
            );
        }

        override protected bool isRender()
        {
            return _iid > 0;
        }

        private LGAvatarHero gameCtrl
        {
            get
            {
                return m_gameCtrl as LGAvatarHero;
            }
        }

    }
}

