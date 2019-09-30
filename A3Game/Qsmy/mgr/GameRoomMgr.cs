using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using UnityEngine;
using GameFramework;
namespace MuGame
{
    public class GameRoomMgr
    {
        public Dictionary<uint, BaseRoomItem> dRooms;

        public GameRoomMgr()
        {
            dRooms = new Dictionary<uint, BaseRoomItem>();
            dRooms[0] = new BaseRoomItem();
            dRooms[9999999] = new PlotRoom();
            dRooms[3334] = new ExpRoom();
			dRooms[3335] = new MoneyRoom();
			dRooms[3339] = new FSWZRoom();
			dRooms[3338] = new MLZDRoom();
			dRooms[3340] = new ZHSLYRoom();
            dRooms[3348] = new ZHSLYRoom();
            dRooms[3349] = new ZHSLYRoom();
            dRooms[3350] = new ZHSLYRoom();
            dRooms[3351] = new ZHSLYRoom();
            dRooms[3352] = new ZHSLYRoom();
            dRooms[3353] = new ZHSLYRoom();
            dRooms[3354] = new ZHSLYRoom();
            dRooms[3355] = new ZHSLYRoom();
            dRooms[3356] = new ZHSLYRoom();
            dRooms[3341] = new PVPRoom();
            dRooms[3342] = new WdsyRoom();
            dRooms[3344] = new DragonRoom();
            dRooms[3345] = new TlfbRoom109();
            dRooms[3346] = new TlfbRoom110();
            dRooms[3347] = new TlfbRoom111();
            dRooms[3357] = new JDZCRoom();
            dRooms[3358] = new CityWarRoom();

        }
        public BaseRoomItem curRoom;

        public bool checkCityRoom()
        {
            return curRoom == dRooms[0];
        }

        public void onChangeLevel(Variant svrconf, Variant cursvrmsg)
        {
            if (curRoom != null)
            {
                curRoom.onEnd();
                curRoom = null;
            }
         //    debug.Log("!!!!:::::::::::"+cursvrmsg.dump());




            //skillbar.setAutoFightType(0);
            uint curId = svrconf["id"];

            if (dRooms.ContainsKey(curId))
            {
                curRoom = dRooms[curId];
            }
            else if (MapModel.getInstance().curLevelId>0)
            {
                curRoom = dRooms[9999999];
            }
            else
            {
                curRoom = dRooms[0];
            }
            curRoom.onStart(svrconf);



            if (cursvrmsg!=null &&cursvrmsg.ContainsKey("dpitms"))
            {
               
                List<Variant> dps = cursvrmsg["dpitms"]._arr;
                Dictionary<string, List<DropItemdta>> ll = new Dictionary<string, List<DropItemdta>>();
                long curtimer = NetClient.instance.CurServerTimeStampMS;
                foreach (Variant v in dps)
                {
                    int x = v["x"];
                    int y = v["y"];

                    string strpos = x + "+" + y;
                    if (!ll.ContainsKey(strpos))
                        ll[strpos] = new List<DropItemdta>();
                    DropItemdta d = new DropItemdta();
                    d.init(v, curtimer);
                    d.x = x;
                     d.y = y;
                    ll[strpos].Add(d);
                }

                foreach (List<DropItemdta> l in ll.Values)
                {
                    Vector3 ve  =new Vector3((float)l[0].x/GameConstant.PIXEL_TRANS_UNITYPOS,0f,(float)l[0].y/GameConstant.PIXEL_TRANS_UNITYPOS);
                    BaseRoomItem.instance.showDropItem(ve, l);
                }
            }

            //if (curId == 0)
            //{
            //    if (skillbar.instance != null)
            //    {
            //        skillbar.instance.autifight.visiable = false;

            //    }

            //    InterfaceMgr.getInstance().close(InterfaceMgr.MONSTER_DICT);
            //    InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            //    return;
            //}
            //if (expbar.instance != null)
            //{     
            //    skillbar.instance.autifight.visiable = true;

            //}

            //SXML xml = XMLMgr.instance.GetSXML("worldcfg.world", "level_id==" + MapModel.getInstance().curLevelId);

            //bool b = xml.hasFound;
            //if (xml.hasFound)
            //{     
            //    skillbar.setAutoFightType();
            //    InterfaceMgr.getInstance().open(InterfaceMgr.MONSTER_DICT);
            //    InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FB_BATTLE);
            //}
            //else if (dRooms.ContainsKey(curId))
            //{
            //    dRooms[curId].onStart();
            //    curRoom = dRooms[curId];
            //}

            //MapProxy.getInstance().sendShowMapObj();
        }

        public bool onLevelFinish(Variant msgData) {
			if (MapModel.getInstance().curLevelId > 0) {
				if (MapModel.getInstance().getMapDta((int)MapModel.getInstance().curLevelId) != null) {
					var leveldata = MapModel.getInstance().getMapDta((int)MapModel.getInstance().curLevelId);
					if (msgData.ContainsKey("kill_m")) {
						Variant kmd = msgData["kill_m"];
						int kmm = 0;
						foreach (Variant v in kmd._arr)
						{
							if (v.ContainsKey("cnt")) ;
							kmm += v["cnt"];
						}
						leveldata.kmNum = kmm;
					}
				}
			}
            if (curRoom != null)
                return curRoom.onLevelFinish(msgData);
            return false;
        }

        public bool onPrizeFinish(Variant msgData)
        {
            if (curRoom != null)
                return curRoom.onPrizeFinish(msgData);
            return false;
        }

		public bool onLevelStatusChanges(Variant msgData) {
			if (curRoom != null)
				return curRoom.onLevel_Status_Changes(msgData);
			return false;
		}

        public void onGetMapMoney(int money)
        {
            if (curRoom != null)
                curRoom.onGetMapMoney(money);
        }


        //public void onMonsterEnterView(GRAvatar gr)
        //{
        //    if (curRoom != null)
        //        curRoom.onMonsterEnterView(gr);
        //}



        private static GameRoomMgr _instance;
        public static GameRoomMgr getInstance()
        {
            if (_instance == null)
                _instance = new GameRoomMgr();
            return _instance;
        }


    }


}
