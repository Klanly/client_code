using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using Cross;
using UnityEngine.UI;
using System.Collections;

namespace MuGame
{
    class a3_summon_shouhun : BaseSummon
    {
        Transform con;

        int curtype = 0;

        Dictionary<int, GameObject> soulObj = new Dictionary<int, GameObject>();
        public a3_summon_shouhun(Transform trans, string name) : base(trans, name)
        {
            init();
        }
        void init()
        {

            tranObj.transform.FindChild("bg/bg/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_shouhun_0");
            tranObj.transform.FindChild("info/att/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_shouhun_1");
            tranObj.transform.FindChild("info/nextatt/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_shouhun_2");
            tranObj.transform.FindChild("info/go/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_shouhun_3");
            tranObj.transform.FindChild("info/onekeygo/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_shouhun_4");



            con = tranObj.transform.FindChild("View/scroll/content");
            for (int i = 1;i<= con.childCount;i++)
            {
                soulObj[i] = con.FindChild(i.ToString()).gameObject;
                int tp = i;
                new BaseButton(soulObj[tp].transform).onClick = (GameObject go) => 
                {
                    setcurtype(tp);
                };
            }

            new BaseButton(tranObj.transform.FindChild("info/go")).onClick = (GameObject go) =>
            {
                A3_SummonProxy.getInstance().sendshouhun(CurSummonID, (uint)curtype, 1);
                a3_BagItemData data = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
                if (!data.summondata.shouhun.ContainsKey(curtype)) return;
                SXML Soulxml = sumXml.GetNode("mon_soul", "soul_id==" + curtype);
                int itemid = Soulxml.getInt("item_id");

                if (a3_BagModel.getInstance().getItemNumByTpid((uint)itemid) <= 0) {
                    if (XMLMgr.instance.GetSXML("item.item", "id==" + itemid).GetNode("drop_info") == null)
                        return;
                    ArrayList data1 = new ArrayList();
                    data1.Add(a3_BagModel.getInstance().getItemDataById((uint)itemid));
                    data1.Add(InterfaceMgr.A3_SUMMON_NEW);
                    if (getSummonWin().avatorobj != null)
                        data1.Add(getSummonWin().avatorobj);
                    else data1.Add(null);
                    ArrayList n = new ArrayList();
                    n.Add("shouhun");
                    data1.Add(n);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data1);
                }
            };

            new BaseButton(tranObj.transform.FindChild("info/onekeygo")).onClick = (GameObject go) =>
            {
                if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                    return;
                a3_BagItemData data = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
                if (!data.summondata.shouhun.ContainsKey(curtype)) return;
                SXML Soulxml = sumXml.GetNode("mon_soul", "soul_id==" + curtype);
                int itemid = Soulxml.getInt("item_id");
                a3_ItemData item_data = a3_BagModel.getInstance().getItemDataById((uint)itemid);
                int itemcount_all = a3_BagModel.getInstance().getItemNumByTpid((uint)itemid);
                if (itemcount_all<= 0) {
                    if (XMLMgr.instance.GetSXML("item.item", "id==" + itemid).GetNode("drop_info") == null)
                        return;
                    ArrayList data1 = new ArrayList();
                    data1.Add(a3_BagModel.getInstance().getItemDataById((uint)itemid));
                    data1.Add(InterfaceMgr.A3_SUMMON_NEW);
                    if (getSummonWin().avatorobj != null)
                        data1.Add(getSummonWin().avatorobj);
                    else data1.Add(null);
                    ArrayList n = new ArrayList();
                    n.Add("shouhun");
                    data1.Add(n);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data1);
                    flytxt.instance.fly(ContMgr.getCont("a3_summon_shouhun_noitem"));
                    return;
                }
                int curlvl = data.summondata.shouhun[curtype].lvl;
                int curExp = data.summondata.shouhun[curtype].exp;
                int maxlvl = sumXml.GetNodeList("mon_soul_exp").Count;
                int needExp = 0;
                for (int i = curlvl+1; i <= maxlvl;i++)
                {
                    needExp += sumXml.GetNode("mon_soul_exp", "lvl==" + i).getInt("need_exp");
                }
                needExp -= curExp;
                int needItemCount = 0;

                int useExp = item_data.main_effect;
                if (needExp % useExp > 0)
                    needItemCount = (needExp / useExp) + 1;
                else
                    needItemCount = needExp / useExp;
                if (itemcount_all >= needItemCount)
                {
                    A3_SummonProxy.getInstance().sendshouhun(CurSummonID, (uint)curtype, (uint)needItemCount);
                }
                else {
                    A3_SummonProxy.getInstance().sendshouhun(CurSummonID, (uint)curtype, (uint)itemcount_all);
                }

            };
            //SetView();
        }

        Dictionary<int, GameObject> shouhunObj = new Dictionary<int, GameObject>();

        void SetView()
        {
            Transform Con = tranObj.transform.FindChild("View/scroll/content");
            shouhunObj.Clear();
            for (int i = 0; i < Con.childCount; i++)
            {
                GameObject.Destroy(Con.GetChild(i).gameObject);
            }
            GameObject item = tranObj.transform.FindChild("View/scroll/0").gameObject;
            List<SXML> SoulList = sumXml.GetNodeList("mon_soul");
            foreach (SXML Soul in SoulList)
            {
                GameObject clon = GameObject.Instantiate(item) as GameObject;
                clon.SetActive(true);
                clon.transform.SetParent(Con, false);
                clon.transform.FindChild("name").GetComponent<Text>().text = Soul.getString("name");
                int soulid = Soul.getInt("soul_id");
                new BaseButton(clon.transform.FindChild("todo")).onClick = (GameObject go) => {
                    A3_SummonProxy.getInstance().sendshouhun(CurSummonID,(uint)soulid,1);
                };
                int itemid = Soul.getInt("item_id");
                a3_ItemData data = a3_BagModel.getInstance().getItemDataById((uint)itemid);
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data);
                icon.SetActive(true);
                icon.transform.SetParent(clon.transform.FindChild("todo/icon"),false);
   
                shouhunObj[soulid] = clon;
            }
        }

        void refreView()
        {
            if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                return;
            a3_BagItemData data = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
            List<SXML> explist = sumXml.GetNodeList("mon_soul_exp");
            foreach (int type in data.summondata.shouhun.Keys)
            {
                if (!shouhunObj.ContainsKey(type)) continue;
                SXML Soulxml = sumXml.GetNode("mon_soul", "soul_id==" + type);

                int itemid = Soulxml.getInt("item_id");
                int itemcount = a3_BagModel.getInstance().getItemNumByTpid((uint)itemid);
                shouhunObj[type].transform.FindChild("todo/num").GetComponent<Text>().text = itemcount.ToString();
                shouhunObj[type].transform.FindChild("lv").GetComponent<Text>().text = data.summondata.shouhun[type].lvl.ToString();
                if (data.summondata.shouhun[type].lvl > 0)
                {
                    SXML attXml = Soulxml.GetNode("att", "lvl==" + data.summondata.shouhun[type].lvl);
                    List<SXML> attType = attXml.GetNodeList("value");
                    if (attType.Count > 1)
                    {
                        shouhunObj[type].transform.FindChild("att").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_gjl") + ": " + attType[0].getInt("att") + " - " + attType[1].getInt("att");
                    }
                    else
                    {
                        shouhunObj[type].transform.FindChild("att").GetComponent<Text>().text = Globle.getAttrNameById(attType[0].getInt("type")) + ": " + attType[0].getInt("att");
                    }
                }
                else {
                    SXML attXml = Soulxml.GetNode("att", "lvl==" + (data.summondata.shouhun[type].lvl+1));
                    List<SXML> attType = attXml.GetNodeList("value");
                    if (attType.Count > 1)
                    {
                        shouhunObj[type].transform.FindChild("att").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_gjl") + ": " +0 + " - " + 0;
                    }
                    else
                    {
                        shouhunObj[type].transform.FindChild("att").GetComponent<Text>().text = Globle.getAttrNameById(attType[0].getInt("type")) + ": " + 0;
                    }

                }
                SXML NextAtt = Soulxml.GetNode("att", "lvl==" + (data.summondata.shouhun[type].lvl + 1));
                if (NextAtt != null)
                {
                    List<SXML> _attType = NextAtt.GetNodeList("value");
                    if (_attType.Count > 1)
                    {
                        shouhunObj[type].transform.FindChild("nextatt").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_gjl") + ": " + _attType[0].getInt ("att") + " - " + _attType[1].getInt("att");
                    }
                    else
                    {
                        shouhunObj[type].transform.FindChild("nextatt").GetComponent<Text>().text = Globle.getAttrNameById(_attType[0].getInt("type")) + ": " + _attType[0].getInt("att");
                    }

                    int maxexp = sumXml.GetNode("mon_soul_exp", "lvl==" + (data.summondata.shouhun[type].lvl + 1)).getInt("need_exp");

                    shouhunObj[type].transform.FindChild("jindu/slider").GetComponent<Image>().fillAmount = (float)data.summondata.shouhun[type].exp / maxexp;
                }


                //List<SXML> lvlList = Soulxml.GetNodeList("att");
                //int alllvl = lvlList.Count;
                //shouhunObj[type].transform.FindChild("jindu/slider").GetComponent<Image>().fillAmount = (float)data.summondata.shouhun[type].lvl / alllvl;
            }
        }


        void setcurtype(int v,bool refersh =false)
        {
            if (refersh)
            {
                curtype = v;
                selsct(curtype);
            }
            else {
                if (curtype != v)
                {
                    curtype = v;
                    selsct(curtype);
                }
            }
        }
        void selsct(int type)
        {
            foreach (int i in soulObj.Keys)
            {
                if (type == i)
                {
                    soulObj[i].transform.FindChild("select").gameObject.SetActive(true);
                }
                else { soulObj[i].transform.FindChild("select").gameObject.SetActive(false); }
            }
            setInfo(type);
        }

        void setshouhun_lvl()
        {
            if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                return;
            a3_BagItemData data = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
            for (int i = 1; i <= con.childCount; i++) {
                soulObj[i].transform.FindChild("lvl").GetComponent<Text>().text = data.summondata.shouhun[i].lvl.ToString();
            }
        }
        void setInfo( int type)
        {
            if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                return;
            a3_BagItemData data = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
            Transform sum_icon = tranObj.transform.FindChild("icon");
            for (int i = 0; i < sum_icon.childCount; i++)
            {
                GameObject.Destroy(sum_icon.GetChild(i).gameObject);
            }
            GameObject icon_sum = IconImageMgr.getInstance().createA3ItemIcon(data.confdata);
            icon_sum.transform.SetParent(sum_icon, false);
            //setAvator();
            if (!soulObj.ContainsKey(type)) return;
            soulObj[type].transform.FindChild ("lvl").GetComponent<Text>().text = data.summondata.shouhun[type].lvl.ToString();
            SXML Soulxml = sumXml.GetNode("mon_soul", "soul_id==" + type);
            int itemid = Soulxml.getInt("item_id");
            a3_ItemData item_data = a3_BagModel.getInstance().getItemDataById((uint)itemid);
            int itemcount = a3_BagModel.getInstance().getItemNumByTpid((uint)itemid);
            tranObj .transform.FindChild("info/todo/num").GetComponent<Text>().text = itemcount.ToString();
            tranObj.transform.FindChild("info/todo/name").GetComponent<Text>().text = item_data.item_name;
            tranObj.transform.FindChild("info/todo/desc").GetComponent<Text>().text = item_data.desc;
            tranObj.transform.FindChild("info/lv").GetComponent<Text>().text = data.summondata.shouhun[type].lvl.ToString();
            tranObj.transform.FindChild("info/name").GetComponent<Text>().text = Soulxml.getString("name");
            Transform itencon = tranObj.transform.FindChild("info/todo/icon");
            for (int i = 0; i < itencon.childCount;i++)
            {
                GameObject.Destroy(itencon.GetChild (i).gameObject);
            }
            a3_ItemData itemdata = a3_BagModel.getInstance().getItemDataById((uint)itemid);
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(itemdata);
            icon.SetActive(true);
            icon.transform.SetParent(itencon, false);
            new BaseButton(icon.transform).onClick = (GameObject go) => 
            {
                ArrayList arr = new ArrayList();
                arr.Add((uint)itemid);
                arr.Add(1);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
            };
            SXML attXml = Soulxml.GetNode("att", "lvl==" + data.summondata.shouhun[type].lvl);
            List<SXML> attType = attXml.GetNodeList("value");
            if (attType.Count > 1)
            {
                tranObj.transform.FindChild("info/att").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_gjl") + ": " + attType[0].getInt("att") + " - " + attType[1].getInt("att");
            }
            else
            {
                tranObj.transform.FindChild("info/att").GetComponent<Text>().text = Globle.getAttrNameById(attType[0].getInt("type")) + ": " + attType[0].getInt("att");
            }
            SXML NextAtt = Soulxml.GetNode("att", "lvl==" + (data.summondata.shouhun[type].lvl + 1));
            if (NextAtt != null)
            {
                List<SXML> _attType = NextAtt.GetNodeList("value");
                tranObj.transform.FindChild("info/nextatt").gameObject.SetActive(true);
                if (_attType.Count > 1)
                {
                    tranObj.transform.FindChild("info/nextatt").GetComponent<Text>().text = ContMgr.getCont("a3_equiptip_gjl") + ": " + _attType[0].getInt("att") + " - " + _attType[1].getInt("att");
                }
                else
                {
                    tranObj.transform.FindChild("info/nextatt").GetComponent<Text>().text = Globle.getAttrNameById(_attType[0].getInt("type")) + ": " + _attType[0].getInt("att");
                }

                int maxexp = sumXml.GetNode("mon_soul_exp", "lvl==" + (data.summondata.shouhun[type].lvl + 1)).getInt("need_exp");

                tranObj.transform.FindChild("info/jindu/slider").GetComponent<Image>().fillAmount = (float)data.summondata.shouhun[type].exp / maxexp;
                tranObj.transform.FindChild("info/jindu/text").GetComponent<Text>().text = data.summondata.shouhun[type].exp + "/" + maxexp;
            }
            else {
                tranObj.transform.FindChild("info/jindu/slider").GetComponent<Image>().fillAmount = 1;
                tranObj.transform.FindChild("info/jindu/text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_shouhun_manji");
                tranObj.transform.FindChild("info/nextatt").gameObject.SetActive(false);
            }

        }

        public override void onShowed()
        {
            //refreView();
            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_SHOUHUN, onShouhun);
            setcurtype(1,true);
            setshouhun_lvl();
        }
        public override void onClose() {
            A3_SummonProxy.getInstance().removeEventListener(A3_SummonProxy.EVENT_SHOUHUN, onShouhun);
            //SetDispose();
        }
        public override void onAddNewSmallWin(string name) {}
        public override void onCurSummonIdChange() {
            base.onCurSummonIdChange();
            //refreView();
            setcurtype(1, true);
            setshouhun_lvl();
        }

        void onShouhun(GameEvent evt)
        {
            //refreView();
            setcurtype(curtype, true);
        }
        void SetDispose()
        {
            if (getSummonWin().avatorobj != null)
            {
                GameObject.Destroy(getSummonWin().avatorobj);
                GameObject.Destroy(getSummonWin().camobj);
                getSummonWin().avatorobj = null;
                getSummonWin().camobj = null;
            }
        }
        void setAvator()
        {
            if (CurSummonID <= 0) return;
            if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                return;
            a3_BagItemData sum = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
            SetDispose();
            SXML xml = sumXml.GetNode("callbeast", "id==" + sum.tpid);
            int mid = xml.getInt("mid");
            SXML mxml = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + mid);
            int objid = mxml.getInt("obj");
            GameObject obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject("monster_" + objid);
            getSummonWin().avatorobj = GameObject.Instantiate(obj_prefab, new Vector3(-153.4f, 0.9f, 0f), Quaternion.identity) as GameObject;
            foreach (Transform tran in getSummonWin().avatorobj.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
            }
            Transform cur_model = getSummonWin().avatorobj.transform.FindChild("model");
            var animm = cur_model.GetComponent<Animator>();
            animm.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            cur_model.gameObject.AddComponent<Summon_Base_Event>();
            cur_model.Rotate(Vector3.up, 270 - mxml.getInt("smshow_face"));
            cur_model.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            GameObject t_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_roleinfo_ui_camera");
            getSummonWin().camobj = GameObject.Instantiate(t_prefab) as GameObject;
            Camera cam = getSummonWin().camobj.GetComponentInChildren<Camera>();
            if (cam != null)
            {
                float r_size = cam.orthographicSize * 1920 / 1080 * Screen.height / Screen.width;
                cam.orthographicSize = r_size;
            }
        }
    }
}
