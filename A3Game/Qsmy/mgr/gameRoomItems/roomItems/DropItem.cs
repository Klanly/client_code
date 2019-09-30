using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Cross;
using GameFramework;
using MuGame.Qsmy.model;

namespace MuGame
{
    public class DropItem : RoomObj, INameObj
    {
        public bool canPick = true;

        public static List<Color> lColor = new List<Color>
        {
             new Color(1f, 0.84f, 0f, 0.2f),
            new Color(1f, 1f, 1f, 0.1f),
            new Color(0f, 1f, 0f, 0.2f),
              new Color(0f, 0f, 1f, 0.2f),
            new Color(1f, 0f, 1f,0.2f),
            new Color(1f, 0.94f, 0.23f, 0.2f),
            new Color(1f, 0, 0, 0.2f)
        };



        public static List<string> leqType = new List<string>
        {
            "",
           "Item_diaoluo_helmet",
            "Item_diaoluo_shoulder",
             "Item_diaoluo_armour",
              "Item_diaoluo_trousers",
                "Item_diaoluo_shoe",
                "Item_box"
        };


        public string roleName
        {
            get
            {
                return itemdta.getName();
            }
            set { }
        }
        private int _curhp = 100;
        public int curhp { get { return _curhp; } set { _curhp = value; } }
        private int _maxHp = 100;
        public int maxHp { get { return _maxHp; } set { _maxHp = value; } }
        private int _title_id = 0;
        public int title_id { get { return _title_id; } set { _title_id = value; } }
        private bool _isactive = true;
        public bool isactive
        {
            get
            {
                return isactive;
            }
            set
            {
                _isactive = value;
                if (_isactive == false)
                    _title_id = 0;
            }
        }
        private int _rednm = 0;
        public int rednm { get { return _rednm; } set { _rednm = value; } }
        public uint _hidbacktime = 0;


        private int _lvlsideid = 0;
        public int spost_lvlsideid { get { return _lvlsideid; } set { _lvlsideid = value; } }
        public uint hidbacktime { get { return _hidbacktime; } set { _hidbacktime = value; } }


        public Vector3 lastHeadPos = Vector3.zero;
        public UnityEngine.Vector3 getHeadPos()
        {
            if (SceneCamera.m_curCamera == null)
                return UnityEngine.Vector3.zero;

            if (this == null || gameObject == null || disposed)
            {
                dispose();
                return UnityEngine.Vector3.zero;
            }
            Vector3 v3 = transform.position + headOffset;

            v3 = SceneCamera.m_curCamera.WorldToScreenPoint(v3);

            //v3 *= SceneCamera.m_fGameScreenPow;//需要根据game_screen 来缩放

            v3.z = 0f;
            lastHeadPos = v3;
            // lastHeadpos = v3;
            return v3;
        }

        public static GameObject tempEffect;
        public static GameObject tempGolden;
        public static DropItem create(DropItemdta item)
        {
            GameObject go = new GameObject();
            DropItem dropitem = go.AddComponent<DropItem>();
            dropitem.itemdta = item;
            dropitem.init();
            return dropitem;
        }

        public Vector3 headOffset = Vector3.zero;
        public bool isFake = false;
        public static Transform dropItemCon;
        public DropItemdta itemdta;
        public override void init()
        {
            base.init();

            if (dropItemCon == null)
            {
                GameObject conGo = new GameObject();
                dropItemCon = conGo.transform;
                dropItemCon.name = "DROP_CON";
            }
            transform.SetParent(dropItemCon, false);
            //  PlayerNameUIMgr.getInstance().show(this);
            initItem();
        }


        public void initItem()
        {
            GameObject tempGo = null;
            Vector3 effsize = Vector3.one;
            if (itemdta.tpid == 0)
            {
                if (tempGolden == null)
                    tempGolden = GAMEAPI.ABModel_LoadNow_GameObject("Item_golenCoin");
                tempGo = GameObject.Instantiate(tempGolden) as GameObject;
            }
            else if (itemdta.itemXml.getInt("item_type") == 2)
            {
                int equiptp = itemdta.itemXml.getInt("equip_type");

                if (equiptp == 6)
                {
                    int jobLimit = itemdta.itemXml.getInt("job_limit");
                    tempGo = getWeapom(itemdta);
                    if (jobLimit == 3)
                        effsize = new Vector3(0.3f, 2f, 1f);
                    else if (jobLimit == 2)
                        effsize = new Vector3(0.3f, 2f, 1f);
                }
                else
                {
                    //if (equiptp > leqType.Count) Debug.LogError("不支持的掉落物品-->equiptp:" + equiptp + "id:-->" + itemdta.dpid);
                    GameObject boxgo;
                    if (equiptp >= leqType.Count)
                    {
                        boxgo = GAMEAPI.ABModel_LoadNow_GameObject("Item_box");
                    }
                    else
                    {
                        boxgo = GAMEAPI.ABModel_LoadNow_GameObject(leqType[equiptp]);

                    }

                    tempGo = GameObject.Instantiate(boxgo) as GameObject;
                }
            }
            else
            {
                GameObject boxgo = GAMEAPI.ABModel_LoadNow_GameObject("Item_box");
                tempGo = GameObject.Instantiate(boxgo) as GameObject;
            }


            if (tempGo == null)
                return;

            if (itemdta.tpid != 0)
            {
                if (tempEffect == null)
                    tempEffect = GAMEAPI.ABFight_LoadPrefab("FX_comFX_fx_diaoluo_guang") as GameObject;
                GameObject effect = GameObject.Instantiate(tempEffect) as GameObject;
                effect.transform.localScale = effsize;
                effect.transform.SetParent(transform, false);
                MeshRenderer skin = effect.transform.GetChild(0).GetComponent<MeshRenderer>();
                if (itemdta.tpid == 0)
                    skin.material.SetColor(EnumShader.SPI_TINT_COLOR, lColor[0]);
                else
                    skin.material.SetColor(EnumShader.SPI_TINT_COLOR, lColor[itemdta.itemXml.getInt("quality")]);

                ////// 武器加个特效 比较叼的装备特殊加的

                if (itemdta.itemXml.getInt("item_type") == 2 && itemdta.itemXml.getInt("quality") >= 5 )
                {
                    GameObject equipFX = GAMEAPI.ABFight_LoadPrefab("FX_comFX_fx_diaoluo_EquipFx") as GameObject;
                     equipFX = GameObject.Instantiate(equipFX) as GameObject;
                     equipFX.transform.SetParent(transform, false);
                     equipFX.transform.localPosition = Vector3.zero;
                }

            }

            tempGo.transform.SetParent(transform, false);
            tempGo.gameObject.layer = EnumLayer.LM_MAP_ITEM;


            //CapsuleCollider CapsuleCollider = gameObject.GetComponent<CapsuleCollider>();
            //headOffset = CapsuleCollider.center;
            //headOffset.y += CapsuleCollider.height / 2;
            headOffset.y = 0.5f;
        }

        public int lastGetTimer = 0;
        public static long cantGetTimer = 0;
        public void OnTriggerEnter(Collider other) => PickUpItem();
        public void PickUpItem()
        {
            if (NetClient.instance == null)
            {
                dispose();
                return;
            }

            long curMs = NetClient.instance.CurServerTimeStampMS;

            if (curMs < cantGetTimer)
                return;

            if (NetClient.instance != null)
            {
                if (curMs - itemdta.initedTimer < 100)
                    return;
            }

            if (isFake)
            {
                FightText.play(FightText.MONEY_TEXT, SelfRole._inst.getHeadPos(), itemdta.count);
                dispose();
            }
            else
            {
                //int timer = NetClient.instance.CurServerTimeStamp;
                //if (timer - lastGetTimer < 3)
                //    return;
                //lastGetTimer = timer;
                //BaseRoomItem.instance.removeDropItm(itemdta.dpid);

                if (SelfRole.fsm.Autofighting &&
                    !AutoPlayModel.getInstance().WillPick((uint)itemdta.tpid))
                    return;
                if (a3_BagModel.getInstance().curi <= a3_BagModel.getInstance().getItems().Count && (
                         a3_BagModel.getInstance().getItemNumByTpid((uint)itemdta.tpid) == 0 ||
                         a3_BagModel.getInstance().getItemNumByTpid((uint)itemdta.tpid) >= a3_BagModel.getInstance().getItemDataById((uint)itemdta.tpid).maxnum)
                         && itemdta.tpid != 0)
                {
                    flytxt.instance.fly(ContMgr.getCont("BagProxy_noplace"));
                    return;
                }
                if (itemdta.ownerId == PlayerModel.getInstance().cid || itemdta.ownerId == 0) {

                    if(!A3_RollModel.getInstance().rollMapping.ContainsKey(itemdta.dpid)) BaseRoomItem.instance.removeDropItm(itemdta.dpid, false);

                }

                MapProxy.getInstance().sendPickUpItem(itemdta.dpid);

            }
        }
        public void update(long curTimer)
        {
            if (disposed)
                return;
            if (checkOverTime(curTimer))
                BaseRoomItem.instance.removeDropItm(itemdta.dpid, isFake);
            if (checkOwnerTime())
            {
                itemdta.ownerId = 0;
            }
        }

        public static List<Vector3> weaponDorpOffset = new List<Vector3>()
        {
            Vector3.zero,
               Vector3.zero,
             Vector3.zero,
           Vector3.zero,
              Vector3.zero,
              new Vector3(0f,0f,-0.4f)
        };
        public static List<Vector3> weaponDorpScale = new List<Vector3>()
        {
            Vector3.zero,
               Vector3.zero,
                new Vector3(1f,1f,1f),
            new Vector3(0.7f,0.7f,0.7f),
              Vector3.zero,
              new Vector3(0.6f,0.6f,0.6f)
        };
        public static List<Vector3> weaponDorpRot = new List<Vector3>()
        {
            Vector3.zero,
               Vector3.zero,
            new Vector3(0, 0, 90),
              new Vector3(0, 0, 90),
              Vector3.zero,
            new Vector3(90, 90, 0)
        };
        public static List<string> weaponDorpPath = new List<string>()
        {
            "",
            "",
            "profession_warrior_weaponr_l_",
              "profession_mage_weaponl_l_" ,
             "",
            "profession_assa_weaponl_l_"
        };
        private GameObject getWeapom(DropItemdta item)
        {
            GameObject go = new GameObject();
            int job = item.itemXml.getInt("job_limit");
            string path = "";
            uint modelid = item.itemXml.getUint("obj");

            Vector3 rot;


            GameObject obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject(weaponDorpPath[job] + modelid);
            if (obj_prefab == null) return null;
            GameObject m_dropWeapon_LObj = GameObject.Instantiate(obj_prefab) as GameObject;


            Transform eff = m_dropWeapon_LObj.transform.FindChild("eff");
            if (eff != null)
                GameObject.Destroy(eff.gameObject);

            if (job == 5)
            {
                GameObject m_dropWeapon_LObj2 = GameObject.Instantiate(obj_prefab) as GameObject;
                m_dropWeapon_LObj2.transform.SetParent(go.transform, false);
                m_dropWeapon_LObj2.transform.eulerAngles = weaponDorpRot[job] * 1.4f;
                m_dropWeapon_LObj2.transform.position = weaponDorpOffset[job];
                m_dropWeapon_LObj2.gameObject.layer = EnumLayer.LM_MAP_ITEM;
                foreach (Transform tran in m_dropWeapon_LObj2.GetComponentsInChildren<Transform>())
                {
                    tran.gameObject.layer = EnumLayer.LM_MAP_ITEM; ;//更改物体的Layer层  
                }
            }

            m_dropWeapon_LObj.gameObject.layer = EnumLayer.LM_MAP_ITEM;
            foreach (Transform tran in m_dropWeapon_LObj.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_MAP_ITEM; ;//更改物体的Layer层  
            }
            m_dropWeapon_LObj.transform.SetParent(go.transform, false);
            m_dropWeapon_LObj.transform.eulerAngles = weaponDorpRot[job];
            go.transform.localScale = Vector3.one; //weaponDorpScale[job];//TODO:测试用,之前的有的物品有点小,xuyin add
            m_dropWeapon_LObj.transform.position = weaponDorpOffset[job];
            //   go.transform.eulerAngles = new Vector3(0, ConfigUtil.getRandom(-45, 45), 0);
            return go;
        }

        public bool checkOverTime(long curTimer)
        {
            return curTimer >= itemdta.left_tm;
        }
        public bool checkOwnerTime()
        {//检测道具归属是否到期，到期后变为无主之物
            if (itemdta.owner_tm < (uint)NetClient.instance.CurServerTimeStamp)
                return true;
            else
                return false;
        }
        //public override void dispose()
        //{
        //    if (disposed) return;

        //  //  PlayerNameUIMgr.getInstance().hide(this);

        //    base.dispose();
        //}
    }

    public class DropItemdta
    {
        public int x;
        public int y;

        public uint ownerId;  //0表示无主之物
        public uint dpid;
        public uint tp;      //0所有人可以拾取，1一人，2队伍，3所有人不能拾取
        public uint owner_tm;
        public bool no_owner = false;
        public int tpid;
        public long left_tm;
        public int count = 1;
        public int intensify_lv;
        public int add_level;
        public int stage;
        public SXML itemXml;
        public long initedTimer;
        public int num;

        public void init(Variant v, long timer)
        {
            no_owner = false;

            dpid = v["dpid"];
            if (v.ContainsKey("tp"))
                tp = v["tp"];
            else
                tp = 0;
            initedTimer = timer;
            left_tm = (long)v["left_tm"] * 1000 + timer;

            if (v.ContainsKey("eqp"))
            {
                Variant sub = v["eqp"];
                tpid = sub["tpid"];
                intensify_lv = sub["intensify_lv"];
                add_level = sub["add_level"];
                stage = sub["stage"];
                itemXml = a3_BagModel.getInstance().getItemXml(tpid);
            }
            else if (v.ContainsKey("itm"))
            {
                Variant sub = v["itm"];
                tpid = sub["id"];
                if (sub.ContainsKey("cnt"))
                    count = sub["cnt"];
                itemXml = a3_BagModel.getInstance().getItemXml(tpid);
            }
            else if (v.ContainsKey("gold"))
            {
                tpid = 0;
                count = v["gold"];
            }

            if (v.ContainsKey("owner") && tp != 3) //tpid == 3是不属于所有人
                ownerId = v["owner"]._uint;
            else
                ownerId = 99999;//所有人不能拾取
            if (v.ContainsKey("owner_tm"))
                owner_tm = v["owner_tm"]._uint;
            else
                owner_tm = (uint)NetClient.instance.CurServerTimeStamp + 1000;
        }




        public string getName()
        {
            if (tpid == 0)
                return "<color=#ffd800>" + ContMgr.getCont("comm_money", count.ToString()) + "</color>";
            return Globle.getColorStrByQuality(itemXml.getString("item_name"), itemXml.getInt("quality"));
        }
        public string getDropItemName()
        {
            if (tpid == 0)
                return ContMgr.getCont("comm_money", count.ToString());
            //string strGod = a3_BagModel.getInstance().addGodStr(itemXml.getInt("equip_level"));
            return Globle.getColorStrByQuality(itemXml.getString("item_name"), itemXml.getInt("quality"));
        }

    }
}
