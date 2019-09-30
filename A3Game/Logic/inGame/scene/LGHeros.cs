using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
using UnityEngine;
namespace MuGame
{

    class LGHeros : lgGDBase, IObjectPlugin
    {

        public static LGHeros instacne;
        public LGHeros(gameManager m)
            : base(m)
        {
            instacne = this;
        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new LGHeros(m as gameManager);
        }

        private Dictionary<uint, LGAvatarHero> _heros;
        private Dictionary<uint, Variant> _heroInfos;
        private Dictionary<uint, Variant> _heroWaitCreateInfos = new Dictionary<uint, Variant>();
        private bool _initFlag = false;
        private bool _mapChageFlag = false;

        public Dictionary<uint, LGAvatarHero> getHeros()
        {
            return _heros;
        }

        public LGAvatarHero getHeroById(uint id)
        {
            if (!_heros.ContainsKey(id)) return null;
            return _heros[id];
        }


        override public void init()
        {
            //return;	 
            _heros = new Dictionary<uint, LGAvatarHero>();
            _heroInfos = new Dictionary<uint, Variant>();
            //this.g_mgr.g_netM.addEventListener(
            //    PKG_NAME.S2C_MONSTER_ENTER_ZONE,
            //    onHeroEnterZone
            //);

            //this.g_mgr.g_netM.addEventListenerCL(
            //    OBJECT_NAME.DATA_JOIN_WORLD,
            //    PKG_NAME.S2C_BEGIN_CHANGE_MAP_RES,
            //    onMapchgBegin
            //);

            this.g_mgr.g_netM.addEventListener(
                PKG_NAME.S2C_SPRITE_LEAVE_ZONE,
                onSpriteLeaveZone
            );

            this.g_mgr.g_gameM.addEventListenerCL(
                OBJECT_NAME.LG_JOIN_WORLD,
                GAME_EVENT.ON_MAP_CHANGE,
                onMapchg
            );

            this.g_mgr.g_gameM.addEventListenerCL(
                OBJECT_NAME.LG_JOIN_WORLD,
                GAME_EVENT.ON_ENTER_GAME,
                onMapchg
            );

            this.g_mgr.g_netM.addEventListener(
             PKG_NAME.S2C_MONSTER_SPAWN,
             onRespawn
         );




            this.g_mgr.g_processM.addProcess(new processStruct(update, "LGHeros"));
        }

        private void update(float tmSlice)
        {
            if (_heroWaitCreateInfos.Count <= 0) return;
            foreach (Variant p in _heroWaitCreateInfos.Values)
            {
                createHero(p);
                uint iid = p["iid"]._uint;
                _heroWaitCreateInfos.Remove(iid);
                break;
            }
        }

        private void onMapchgBegin(GameEvent e)
        {
            _mapChageFlag = false;
            _initFlag = false;
            clear();
        }

        public void clear()
        {
           debug.Log("dele!!!!!!!!!!!!!!!!!!!!!!!!  "+debug.count);

            foreach (LGAvatarHero ct in _heros.Values)
            {
                this.g_mgr.g_processM.removeRender(ct);
                ct.dispose();
            }
            _heros.Clear();
            _heroInfos.Clear();
        }

        public void onMapchg(GameEvent e)
        {
            _mapChageFlag = true;
           
            createZoneSprites();
           
        }

        private void onSpriteLeaveZone(GameEvent e)
        {
            Variant data = e.data;
            foreach (uint iid in data["iidary"]._arr)
            {

                if (!_heroInfos.ContainsKey(iid)) continue;
                _heroInfos.Remove(iid);

                if (_heroWaitCreateInfos.ContainsKey(iid))
                {
                    _heroWaitCreateInfos.Remove(iid);
                }


                if (!_heros.ContainsKey(iid)) continue;
                LGAvatarHero ct = _heros[iid];
                _heros.Remove(iid);
                ct.dispose();
            }
        }

        private void onRespawn(GameEvent e)
        {
            Variant info = e.data;
            if (!info.ContainsKey("iid")) return;
            uint iid = info["iid"]._uint;

            if (!_heros.ContainsKey(iid))
                return;
            LGAvatarHero hero = _heros[iid];
            hero.Respawn(info);
        }

        public void onHeroEnterZone(Variant m)
        {
            if (GRMap.playingPlot)
                return;

            uint iid = m["iid"]._uint;
            if (_heros.ContainsKey(iid))
            {//err?

            }
            else
            {
                _heroInfos[iid] = m;
                if (_initFlag) createHero(m);
            }
          //   debug.Log("!!onHeroEnterZone!!" + " iid:"+iid+" mid:"+m["mid"] + " "+debug.count);
        }
        private void createZoneSprites()
        {
            if (!_mapChageFlag) return;
            _initFlag = true;
            foreach (Variant p in _heroInfos.Values)
            {
                createHero(p);
            }
            _mapChageFlag = false;
        }
        private void addCreateHero(Variant m)
        {
            uint iid = m["iid"]._uint;
            _heroWaitCreateInfos[iid] = m;
        }

        private int herologhtIdx = 0;
        public void createHero(Variant m)
        {
            int heroid = m["mid"]._int;
            uint iid = m["iid"]._uint;


            Variant b = MonsterConfig.instance.conf;
            Variant mconf = b["monsters"][heroid + ""];

            if (mconf == null)
            {
                GameTools.PrintError(" hero[ " + heroid + " ] no conf ERR!");
                return;
            }

            LGAvatarHero ct = new LGAvatarHero(this.g_mgr);
            _heros[iid] = ct;

            if (m["owner_cid"] == PlayerModel.getInstance().cid)
            {
                ct.addEffect("heroenterworld" + herologhtIdx, "hero_enterscence", true);
            }
          
            ct.initData(m);
            ct.init();

           

            this.g_mgr.g_processM.addRender(ct);

        }

        public LGAvatarHero get_hero_by_iid(uint iid)
        {
            if (!_heros.ContainsKey(iid)) return null;
            return _heros[iid];
        }

        public LGAvatarHero get_hero_by_mid(uint mid)
        {
            if (_heros == null || _heros.Values == null) return null;
            foreach (LGAvatarHero m in _heros.Values)
            {
                if (m.getMid() != mid) continue;
                return m;
            }
            return null;
        }


        public LGAvatarHero getNearHero()
        {
            if (_heros == null || _heros.Values == null) return null;
            LGAvatarHero charHero = null;
            float curdis = 1000f;
            int x = lgMainPlayer.viewInfo["x"];
            int y = lgMainPlayer.viewInfo["y"];
            foreach (LGAvatarHero ct in _heros.Values)
            {
                if (ct.IsDie())
                    continue;
                if (ct.IsCollect())
                    continue;

                float dis = Math.Abs(ct.x - x) + Math.Abs(ct.y - y);

                if (dis > 1000)
                    continue;

                if (charHero == null)
                {
                    charHero = ct;
                    curdis = dis;
                }
                //  else if (Math.Abs((charHero.x - x) * (charHero.x - x)) + Math.Abs((charHero.y - y) * (charHero.y - y)) > Math.Abs((ct.x - x) * (ct.x - x)) + Math.Abs((ct.y - y) * (ct.y - y)))
                else if (curdis > dis)
                {
                    charHero = ct;
                    curdis = dis;
                }
            }
            return charHero;
        }


        private lgSelfPlayer lgMainPlayer
        {
            get
            {
                return this.g_mgr.getObject(OBJECT_NAME.LG_MAIN_PLAY) as lgSelfPlayer;
            }
        }

    }
}

