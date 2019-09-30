using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using UnityEngine;
using DG.Tweening;
using GameFramework;
using System.Threading;
using System.Collections;

namespace MuGame
{
    public class BaseRoomItem
    {
        uint fakeItemIdx = 0;
        Variant svrConf;
        bool started = false;
        GameObject roomItemCon;
        TickItem process;
        //记录金币、经验
        public uint goldnum;
        public uint expnum;
        uint lvl;
        uint zhuan;
        public int getach;
        public int getExp;
         List<drop_data> list = new List<drop_data>();
        public List<DropItemdta> list2 = new List<DropItemdta>();
        public static BaseRoomItem instance;
        virtual public void onStart(Variant conf)
        {
            if (started)
                return;
            started = true;
            instance = this;
            dDropItem = new Dictionary<uint, DropItem>();
            dDropItem_own = new Dictionary<uint, DropItem>();
            dDropFakeItem = new Dictionary<uint, DropItem>();
            dPickItem = new Dictionary<uint, DropItem>();
            clearlist();
            svrConf = conf;
            fakeItemIdx = 0;
            SceneCamera.refreshMiniMapCanvas();
            process = new TickItem(onTick);
            TickMgr.instance.addTick(process);
            initRoomObj();
            if (a1_gamejoy.inst_skillbar) { a1_gamejoy.inst_skillbar.clearCD(); }
            goldnum = 0;
            expnum = PlayerModel.getInstance().exp;
            lvl = PlayerModel.getInstance().lvl;
            zhuan = PlayerModel.getInstance().up_lvl;
            a3_fb_finish.room = this;
        }

        virtual public void onEnd()
        {
            if (!started)
                return;
            dDropItem = null;
            dDropItem_own = null;
            DropItem.dropItemCon = null;
            svrConf = null;
            started = false;
            TickMgr.instance.removeTick(process);
            a3_fb_finish.room = null;

            //鼓舞次数重置为0
            A3_ActiveModel.getInstance().blessnum_yb = 0;
            A3_ActiveModel.getInstance().blessnum_ybbd = 0;
        }

        public void clearlist()
        {
            list.Clear();
        }
        int idx = 0;
        float f = 0.05f;
        virtual public void onTick(float s)
        {
            if (list.Count > 0)
            {
                f -= Time.deltaTime;
                for (int m = 0; m < list.Count; m++)
                {
                    if (f <= 0)
                    {
                        idx = showDrop(list[m].vec, 0, idx, list[m].data[0], list[m].isfake);
                        list[m].data.Remove(list[m].data[0]);
                        if (list[m].data.Count <= 0)
                        {
                            list.Remove(list[m]);
                        }
                        if (m + 1 >= list.Count) { f = 0.05f; }
                    }
                }
            }
            if (NetClient.instance == null) return;

            long curTimer = NetClient.instance.CurServerTimeStampMS;
            var temp = new List<DropItem>(dDropItem.Values);
            for (int i = 0; i < temp.Count; i++)
            {
                temp[i].update(curTimer);
                if (temp[i].itemdta.ownerId == 0)
                {//归属时间过了所有玩家可以拾取
                    if (!dDropItem_own.ContainsKey(temp[i].itemdta.dpid))
                    {
                        dDropItem_own[temp[i].itemdta.dpid] = temp[i];
                    }
                }
            }
        }

        void initRoomObj()
        {
            if (svrConf.ContainsKey("l"))
            {
                List<Variant> l = svrConf["l"]._arr;
                for(int i=0;i<l.Count;i++)
                    addRoomObj(l[i]);
            }
            if (svrConf.ContainsKey("fb"))
            {
                List<Variant> fb = svrConf["fb"]._arr;
                for (int i = 0; i < fb.Count; i++)
                    addRoomObjForUI(fb[i]);
            }
        }
        void addRoomObj(Variant v)
        {
            if (roomItemCon == null)
            {
                roomItemCon = new GameObject();
                roomItemCon.name = "roomObjs";
            }


            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.SetParent(roomItemCon.transform);
            go.transform.position = new Vector3(v["ux"], v["uy"], v["uz"]);
            go.transform.localScale = new Vector3(v["uw"], 1, v["uh"]);

            TransMapPoint room = go.AddComponent<TransMapPoint>();
            room.id = v["id"];
            room.mapid = v["gto"]._uint;
            if (v.ContainsKey("faceto"))
                room.faceto = v["faceto"];
            room.init();
        }
        void addRoomObjForUI(Variant v)
        {
            if (roomItemCon == null)
            {
                roomItemCon = new GameObject();
                roomItemCon.name = "roomObjs";
            }


            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.SetParent(roomItemCon.transform);
            go.transform.position = new Vector3(v["ux"], v["uy"], v["uz"]);
            go.transform.localScale = new Vector3(v["uw"], 1, v["uh"]);

            CoFBMapPoint fbRoom = go.AddComponent<CoFBMapPoint>();
            if(v.ContainsKey("id"))
                fbRoom.id = v["id"];
            if(v.ContainsKey("fb_id"))
                fbRoom.levelId = v["fb_id"]._uint;
            //if (v.ContainsKey("faceto"))
            //    fbRoom.faceto = v["faceto"];
            fbRoom.init();
        }
        public void removeDropItm(uint dpid, bool isfake)
        {
            if (isfake)
            {
                if (dDropFakeItem.ContainsKey(dpid))
                {
                    dPickItem[dpid] = dDropFakeItem[dpid];

                    dDropFakeItem[dpid].dispose();
                    dDropFakeItem.Remove(dpid);
                }
            }
            else
            {
                if (dDropItem?.ContainsKey(dpid) ?? false)
                {
                    dPickItem[dpid] = dDropItem[dpid];

                    dDropItem[dpid].dispose();
                    DropItemUIMgr.getInstance().hideOne(dDropItem[dpid]);
                    dDropItem.Remove(dpid);
                    if (dDropItem_own.ContainsKey(dpid))
                        dDropItem_own.Remove(dpid);
                }
            }

        }
        //public int drop_id(uint dpid)
        //{
        //    int ss=0;
        //    int num = 0;
        //    if (dDropItem.ContainsKey(dpid))
        //    {
        //        ss= dDropItem[dpid].itemdta.itemXml.getInt("id");
        //        num = dDropItem[dpid].itemdta.count;
        //    }
        //    return ss;
        //}
        public void flyGetItmTxt(uint dpid, bool isfake)
        {
            if (isfake)
            {
                if (dDropFakeItem.ContainsKey(dpid))
                {
                    flygetText(dDropFakeItem[dpid]);
                }
                else if (dPickItem.ContainsKey(dpid))
                {
                    flygetText(dPickItem[dpid]);
                    dPickItem.Remove(dpid);
                }
            }
            else
            {
                if (dDropItem.ContainsKey(dpid))
                {
                    flygetText(dDropItem[dpid]);
                }
                else if (dPickItem.ContainsKey(dpid))
                {
                    flygetText(dPickItem[dpid]);
                    dPickItem.Remove(dpid);
                }
            }
        }
        public void flygetText(DropItem item)
        {
            if (item.itemdta.tpid == 0)
            {
                MediaClient.instance.PlaySoundUrl("audio_common_collect_coin", false, null);
                // flytxt.instance.fly("拾取金币 " + item.itemdta.count);
                //已有金币添加飘字
                // if (a3_insideui_fb.instance != null) a3_insideui_fb.instance.SetInfMoney(item.itemdta.count);
                if (GameRoomMgr.getInstance().curRoom != null) GameRoomMgr.getInstance().curRoom.onPickMoney(item.itemdta.count);

                //记录拾取金币
                //goldnum += (uint)item.itemdta.count;
                if (a3_insideui_fb.instance != null)
                    goldnum = (uint)a3_insideui_fb.instance.addmoney;
                else
                {
                    goldnum += (uint)item.itemdta.count;
                }
            }
            else
            {
                string name = item.itemdta.itemXml.getString("item_name");
                int quality = item.itemdta.itemXml.getInt("quality");
                flytxt.instance.fly(ContMgr.getCont("gameroom_pick") + Globle.getColorStrByQuality(name, quality) + "x" + item.itemdta.count);

                DropItemdta v = new DropItemdta();

                v.tpid = item.itemdta.itemXml.getInt("id");
                v.num = item.itemdta.count;
                foreach (var it in list2)
                {
                    if (v.tpid == it.tpid)
                    {
                        it.num += item.itemdta.count;
                        return;
                    }


                }
                list2.Add(v);






            }
        }

        public Dictionary<uint, DropItem> dDropItem;
        public Dictionary<uint, DropItem> dDropItem_own; //自己打出来的物品，可以拾取
        public Dictionary<uint, DropItem> dDropFakeItem;
        public Dictionary<uint, DropItem> dPickItem;
        public void clear()
        {

            foreach (DropItem item in dDropItem.Values)
            {
                item.dispose();
            }
            dDropItem.Clear();
            dDropItem_own.Clear();


            foreach (DropItem item in dDropFakeItem.Values)
            {
                item.dispose();
            }
            dDropFakeItem.Clear();
        }
        protected List<Vector3> lDropOffset;
        public void showDropItem(Vector3 pos, List<DropItemdta> vecItem, bool isfake = false)
        {
            if (GRMap.grmap_loading)
                return;

            if (vecItem.Count > 0)
            {
                drop_data d = new drop_data();
                d.data = vecItem;
                d.vec = pos;
                d.isfake = isfake;
                list.Add(d);
            }

            initDropOffset();
        }

        private int showDrop(Vector3 dropPos, int wrongcount, int idx, DropItemdta item, bool isfake = false)
        {
            if (lDropOffset.Count <= idx)
                idx = 0;
            Vector3 vec = dropPos + lDropOffset[idx];
            //  GameObject go = GameObject.Find("coin");
            Vector3 begin = vec;
            begin.y = -99;



            NavMeshHit hit;
            //if (NavMesh.Raycast(begin, vec, out hit, NavMesh.GetNavMeshLayerFromName("Default")))
            NavMesh.SamplePosition(vec, out hit, 100f, NavmeshUtils.allARE);
            Vector3 pos = hit.position;
            if (pos.x == vec.x && pos.z == vec.z)
            {
                vec.y = pos.y;
                DropItem itm = getDropItem(vec, Vector3.zero, item, isfake);

                DropItemUIMgr.getInstance().show(itm, itm.itemdta.getDropItemName());
                if (!isfake)
                {
                    dDropItem[item.dpid] = itm;
                    if (item.ownerId == PlayerModel.getInstance().cid || item.ownerId == 0 ||
                        (TeamProxy.getInstance().MyTeamData != null && item.ownerId == TeamProxy.getInstance().MyTeamData.teamId))
                    {
                        dDropItem_own[item.dpid] = itm;
                    }
                }
                else
                {

                    item.dpid = fakeItemIdx;
                    dDropFakeItem[item.dpid] = itm;
                    fakeItemIdx++;
                }

                return idx + 1;
            }
            else if (wrongcount >= 3)
            {
                debug.Log(":" + item.dpid);
                DropItem itm = getDropItem(pos, Vector3.zero, item);
                DropItemUIMgr.getInstance().show(itm, itm.itemdta.getDropItemName());

                if (!isfake)
                {
                    dDropItem[item.dpid] = itm;
                    //if (item.ownerId == PlayerModel.getInstance().cid)
                    //{
                    //dDropItem_own[item.dpid] = itm;
                    if (item.ownerId == PlayerModel.getInstance().cid || item.ownerId == 0 ||
                        (TeamProxy.getInstance().MyTeamData != null && item.ownerId == TeamProxy.getInstance().MyTeamData.teamId))
                    {
                        dDropItem_own[item.dpid] = itm;
                    }
                    //}
                }
                else
                {
                    item.dpid = fakeItemIdx;
                    dDropFakeItem[item.dpid] = itm;
                    fakeItemIdx++;
                }
                return idx + 1;
            }
            wrongcount++;
            return showDrop(dropPos, wrongcount, idx + 1, item, isfake);
        }

        public static DropItem getDropItem(Vector3 vec, Vector3 eulerAngles, DropItemdta item, bool isfake = false)
        {
            DropItem dropitem = DropItem.create(item);
            dropitem.lastGetTimer = 0;
            dropitem.transform.position = vec;
            dropitem.transform.eulerAngles = dropitem.transform.eulerAngles + eulerAngles;
            dropitem.isFake = isfake;
            return dropitem;
        }
        //自动捡物
        public void CollectAllDrops()
        {//不用自动捡
            //if (dDropItem != null)
            //    foreach (var v in dDropItem.Keys)
            //    {
            //        MapProxy.getInstance().sendPickUpItem(v);
            //    }
        }
        public void CollectAllDrops1()
        {
            if (dDropItem != null)
                foreach (var v in dDropItem.Keys)
                {
                    removeDropItm(v, false);
                    MapProxy.getInstance().sendPickUpItem(v);
                }
        }
        const decimal stepDrop = 1.7m;
        private void initDropOffset()
        {
            if (lDropOffset == null)
            {
                lDropOffset = new List<Vector3>();
                lDropOffset.Add(Vector3.zero);
                formatDropOffset(stepDrop);
            }
        }
        private void formatDropOffset(decimal step)
        {

            decimal begin = -step;
            for (decimal i = begin; i <= step; i += stepDrop)
            {
                if (i == begin || i == step)
                {
                    for (decimal j = begin; j <= step; j += stepDrop)
                    {
                        lDropOffset.Add(new Vector3((float)i, 0, (float)j));
                    }
                }
                else
                {
                    lDropOffset.Add(new Vector3((float)i, 0, (float)step));
                    lDropOffset.Add(new Vector3((float)i, 0, (float)-step));
                }
            }

            if (step < (stepDrop * 3.0m))
                formatDropOffset(step + stepDrop);
        }

        virtual public void onMonsterDied(MonsterRole monster)
        {

        }

        virtual public void onPickMoney(int num)
        {

        }

        virtual public void onAddExp(int num)
        {

        }


        //virtual public void onMonsterEnterView(GRAvatar gr)
        //{

        //}

        virtual public bool onLevelFinish(Variant msgData)
        {
            expnum = PlayerModel.getInstance().GetNeedExp(zhuan, lvl, expnum, PlayerModel.getInstance().up_lvl, PlayerModel.getInstance().lvl, PlayerModel.getInstance().exp);
            if (msgData.ContainsKey("kill_exp"))
            {
                expnum = msgData["kill_exp"];
            }

            if (msgData.ContainsKey("money"))
            {
                goldnum += msgData["money"];
            }
            return false;
        }

        virtual public bool onPrizeFinish(Variant msgData)
        {
            return false;
        }

        virtual public bool onLevel_Status_Changes(Variant msgData)
        {
            return false;
        }

        virtual public void onGetMapMoney(int money)
        {

        }


    }

    public class RoomDropItem : MonoBehaviour
    {
        public int id = 0;
        public int num = 0;


        void Start()
        {
            BoxCollider coll = GetComponent<BoxCollider>();
            if (coll == null)
                coll = gameObject.AddComponent<BoxCollider>();
            coll.isTrigger = true;

            int layer = EnumLayer.LM_PT;
            gameObject.layer = layer;

            //    transform.eulerAngles = new Vector3(0, (float)ConfigUtil.getRandom(0, 360), 0);

            //Vector3 endPos = transform.position;
            //transform.position = new Vector3(endPos.x, endPos.y + 5, endPos.z);
            // transform.DOMove(endPos, .5f);

            //transform.localScale=Vector3.zero;
            //transform.DOScale(.5f, .5f)

        }



        public void OnTriggerEnter(Collider other)
        {
            int layer = EnumLayer.LM_SELFROLE;

            if (other.gameObject.layer == layer)
            {
                FightText.play(FightText.MONEY_TEXT, SelfRole._inst.getHeadPos(), 10);
                Destroy(gameObject);
            }
        }
    }



    class CoFBMapPoint : RoomObj
    {
        public int id;
        public uint levelId;
        public float faceto = 0f;
        public override void init()
        {
            base.init();
            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            if (mr != null)
                GameObject.Destroy(mr);
            MeshFilter mf = gameObject.GetComponent<MeshFilter>();
            if (mf != null)
                GameObject.Destroy(mf);
            gameObject.name = "FbRoomObj" + id;
        }

        protected override void onTrigger()
        {
            if(!FunctionOpenMgr.instance.Check(FunctionOpenMgr.TEAMPART))
            {
                flytxt.instance.fly(ContMgr.getCont("fb_team_close"));
                return;

            }
            if (!PlayerModel.getInstance().CheckLevel(a3_counterpartModel.getInstance().GetLevelLimitZhuanByLevelId(levelId), a3_counterpartModel.getInstance().GetLevelLimitLevelByLevelId(levelId)))
            {
                flytxt.instance.fly(ContMgr.getCont("a3_counterpart_lvl"));
            }
            else
            {
                ArrayList arr = new ArrayList();
                arr.Add(levelId);
                if (PlayerModel.getInstance().IsCaptain)
                {
                    TeamProxy.getInstance().sendobject_change((int)levelId);
                }
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_FB_TEAM, arr);
            }

        }
    }
    class TransMapPoint : RoomObj
    {
        public int id;
        public uint mapid;
        public float faceto = 0f;
        static public bool is_need_stop_fsm = false;
        public override void init()
        {
            base.init();
            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            if (mr != null)
                GameObject.Destroy(mr);
            MeshFilter mf = gameObject.GetComponent<MeshFilter>();
            if (mf != null)
                GameObject.Destroy(mf);
            gameObject.name = "RoomObj" + id;
        }

        protected override void onTrigger()
        {

            Variant v = SvrMapConfig.instance.getSingleMapConf(mapid);

            int lvup = 0;
            if (v.ContainsKey("lv_up"))
                lvup = v["lv_up"];

            int lv = 0;
            if (v.ContainsKey("lv"))
                lv = v["lv"];

            if (lvup > PlayerModel.getInstance().up_lvl)
            {
                flytxt.instance.fly(ContMgr.getCont("comm_nolvmap", lvup.ToString(), lv.ToString(), v["map_name"]));
                return;
            }
            else if (lvup == PlayerModel.getInstance().up_lvl)
            {
                if (lv > PlayerModel.getInstance().lvl)
                {
                    flytxt.instance.fly(ContMgr.getCont("comm_nolvmap", lvup.ToString(), lv.ToString(), v["map_name"]));
                    return;
                }
            }

            loading_cloud.showIt(() =>
            {
                if (is_need_stop_fsm)
                {
                    SelfRole.fsm.Stop();
                    is_need_stop_fsm = false;
                }
                
                a1_gamejoy.inst_joystick.stop();
                MapProxy.getInstance().changingMap = true;

                PlayerModel.getInstance().mapBeginroatate = faceto;

                MapProxy.getInstance().sendBeginChangeMap(id);
            });
        }
    }

    public class RoomObj : MonoBehaviour
    {

        public virtual void init()
        {
            BoxCollider box = gameObject.GetComponent<BoxCollider>();
            if (box == null)
                box = gameObject.AddComponent<BoxCollider>();
            box.isTrigger = true;
            gameObject.layer = EnumLayer.LM_PT;
        }

        public void OnTriggerEnter(Collider other)
        {

            if (GRMap.changeMapTimeSt == 0 || NetClient.instance.CurServerTimeStamp - GRMap.changeMapTimeSt < 2)
                return;
            onTrigger();

        }

        protected bool disposed = false;
        virtual public void dispose()
        {
            if (disposed) return;
            disposed = true;
            Destroy(gameObject);
        }

        protected virtual void onTrigger()
        {

        }
    }

    class drop_data
    {
        public List<DropItemdta> data;
        public Vector3 vec;
        public bool isfake;
    }

}
