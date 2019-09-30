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
    class a3_summon_lianxie : BaseSummon
    {
        public a3_summon_lianxie(Transform trans, string name) : base(trans, name)
        {
            init();
        }

        GameObject helpcon;
        GameObject r1;
        GameObject r2;
        Transform attCon;

        void init() {
            r1 = tranObj.transform.FindChild("r1").gameObject;
            r2 = tranObj.transform.FindChild("r2").gameObject;
            attCon = r2.transform.FindChild("attcon");
            new BaseButton(tranObj.transform.FindChild("r1/getbtn")).onClick = (GameObject go) =>
            {
                if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                    return;
                if (CurSummonID == A3_SummonModel.getInstance().nowShowAttackID)
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_summon3"));
                    return;
                }
                a3_BagItemData data = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
                if (data.summondata.linkdata == null) return;
                bool cando = false;
                foreach (link_data dx in data.summondata.linkdata.Values) {
                    if (dx.lock_state == false) { cando = true; break; }
                }
                if (cando)
                {
                    if (getSummonWin() == null) return;
                    GameObject toSrue = getSummonWin().GetSmallWin("uilayer_getlianxie");
                    toSrue.SetActive(true);
                }
                else {
                    flytxt.instance.fly(ContMgr.getCont("a3_summon_lianxie_alllock"));
                }
            };
            helpcon = tranObj.transform.FindChild("r1/help").gameObject;
            new BaseButton(tranObj.transform.FindChild("r1/help_btn")).onClick = (GameObject go) => {
                helpcon.SetActive(true);
            };
            new BaseButton(helpcon.transform.FindChild("close")).onClick = (GameObject go) => {
                helpcon.SetActive(false);
            };

            new BaseButton(r1.transform.FindChild("lianxieCon_btn")).onClick = (GameObject go) => {
                if ( !CheckSummon()) {
                    flytxt.instance.fly(ContMgr.getCont("nullsummon_link"));
                    return;
                }
                OpenlianxieCon();
            };

            new BaseButton(r2.transform.FindChild("close")).onClick = (GameObject go) => {
                CloselianxieCon();
            };

            new BaseButton(r2.transform.FindChild("Battle")).onClick = (GameObject go) => {

                if (A3_SummonModel.getInstance().link_list.Count < 10)
                {
                    List<uint> ids = new List<uint>();
                    if (A3_SummonModel.getInstance().link_list.Contains(CurSummonID)) return;
                    ids.AddRange ( A3_SummonModel.getInstance().link_list);
                    ids.Add(CurSummonID);
                    A3_SummonProxy.getInstance().sendlink(ids);
                }
                else {
                    flytxt.instance.fly(ContMgr .getCont ("maxnum"));
                }
            };

            new BaseButton(r2.transform.FindChild("cancel")).onClick = (GameObject go) => {
                List<uint> ids = new List<uint>();
                if (!A3_SummonModel.getInstance().link_list.Contains(CurSummonID)) return;
                ids.AddRange(A3_SummonModel.getInstance().link_list);
                ids.Remove(CurSummonID);
                A3_SummonProxy.getInstance().sendlink(ids);
            };

            new BaseButton(r2.transform.FindChild("onekey")).onClick = (GameObject go) => {
                List < link_info > dic = new List< link_info>();
                List<uint> idslist = new List<uint>();
                foreach (a3_BagItemData it in A3_SummonModel .getInstance ().GetSummons().Values) {
                    bool can = false;
                    foreach (int idx in it.summondata.linkdata.Keys)
                    {
                        if (it.summondata.linkdata[idx].type != 0)
                        {
                            can = true;
                            break;
                        }
                    }
                    if (!can) continue;
                    link_info l = new link_info();
                    l.id = (uint)it.summondata.id;
                    l.combpt = it.summondata.linkCombpt;
                    dic.Add(l);
                }
                dic.Sort();
                int ids = 1;
                foreach (link_info com in dic) {
                    idslist.Add(com.id);
                    ids++;
                    if (ids > 10) { break; }
                }
                A3_SummonProxy.getInstance().sendlink(idslist);
            };
            inText();
        }
                
        public override void onShowed()
        {
            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_REFLIANXIE , onlianxie);
            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_LINK, onlink);
            CloselianxieCon();
            helpcon.SetActive(false);
            setInfo();
            setlianxie_Value();
            useItem_num(); 
        }
        public override void onClose()
        {
            A3_SummonProxy.getInstance().removeEventListener(A3_SummonProxy.EVENT_REFLIANXIE, onlianxie);
            A3_SummonProxy.getInstance().removeEventListener(A3_SummonProxy.EVENT_LINK, onlink);
            CloselianxieCon();
        }
        void inText()
        {
            tranObj.transform.FindChild("r1/getbtn/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_summon_lianxie_4");
            tranObj.transform.FindChild("r1/tet").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_summon_lianxie_5");
            tranObj.transform.FindChild("r1/main/con_btn/scrollview/btnTemp/nul/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_summon_lianxie_6");
            tranObj.transform.FindChild("r1/main/con_btn/scrollview/btnTemp/nulType/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_summon_lianxie_7");
            tranObj.transform.FindChild("r1/main/Text1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_summon_lianxie_8");
            tranObj.transform.FindChild("r1/main/Text2").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_summon_lianxie_9");
            tranObj.transform.FindChild("r1/main/Text3").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_summon_lianxie_10");

            tranObj.transform.FindChild("r1/info/Text1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_summon_lianxie_11");
            tranObj.transform.FindChild("r1/main/Text4").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_summon_lianxie_12");

            tranObj.transform.FindChild("r1/help/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_summon_lianxie_13");//help提示
            tranObj.transform.FindChild("r1/help/close/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_summon_lianxie_14");//知道了

            getComponentByPath<Text>("r1/info/att/life").text = ContMgr.getCont("a3_summoninfo_5");
            getComponentByPath<Text>("r1/info/att/phyatk").text = ContMgr.getCont("a3_summoninfo_6");
            getComponentByPath<Text>("r1/info/att/phydef").text = ContMgr.getCont("a3_summoninfo_7");
            getComponentByPath<Text>("r1/info/att/manadef").text = ContMgr.getCont("a3_summoninfo_8");
            getComponentByPath<Text>("r1/info/att/hit").text = ContMgr.getCont("a3_summoninfo_9");
            getComponentByPath<Text>("r1/info/att/manaatk").text = ContMgr.getCont("a3_summoninfo_10");
            getComponentByPath<Text>("r1/info/att/crit").text = ContMgr.getCont("a3_summoninfo_11");
            getComponentByPath<Text>("r1/info/att/dodge").text = ContMgr.getCont("a3_summoninfo_12");
            getComponentByPath<Text>("r1/info/att/hitit").text = ContMgr.getCont("a3_summoninfo_13");
            getComponentByPath<Text>("r1/info/att/fatal_damage").text = ContMgr.getCont("a3_summoninfo_14");
            getComponentByPath<Text>("r1/info/att/reflect_crit_rate").text = ContMgr.getCont("a3_summoninfo_15");

            getComponentByPath<Text>("r2/CombptText").text = ContMgr.getCont("uilayer_a3_summon_lianxie_15"); //连携战力
            getComponentByPath<Text>("r2/CombptText2").text = ContMgr.getCont("uilayer_a3_summon_lianxie_16"); //连携属性
            getComponentByPath<Text>("r2/topText").text = ContMgr.getCont("uilayer_a3_summon_lianxie_17"); //上阵队列
            getComponentByPath<Text>("r2/onekey/Text").text = ContMgr.getCont("uilayer_a3_summon_lianxie_18"); //一键上阵
            getComponentByPath<Text>("r2/Battle/Text").text = ContMgr.getCont("uilayer_a3_summon_lianxie_19"); //上阵
            getComponentByPath<Text>("r2/cancel/Text").text = ContMgr.getCont("uilayer_a3_summon_lianxie_20"); //取消上阵
            getComponentByPath<Text>("r1/lianxieCon_btn/Text").text = ContMgr.getCont("uilayer_a3_summon_lianxie_21");//连携
        }

        


        int index = 0;
        public override void onAddNewSmallWin(string name)
        {
            switch (name)
            {
                case "uilayer_getlianxie":
                    GameObject getlianxie = getSummonWin()?.GetSmallWin(name);
                    getlianxie.transform.FindChild("yes/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_getlianxie_1");
                    getlianxie.transform.FindChild("no/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_getlianxie_2");
                    getlianxie.transform.FindChild("Text_mian").GetComponent<Text>().text = ContMgr.getCont("uilayer_getlianxie_3");
                    new BaseButton(getlianxie.transform.FindChild("yes/Text")).onClick = (GameObject go) => {
                        if (CanGet)
                        {
                            A3_SummonProxy.getInstance().sendlianxie(CurSummonID);
                        }
                        else {
                            if (XMLMgr.instance.GetSXML("item.item", "id==" + NeedItem_lianxie).GetNode("drop_info") == null)
                                return;
                            ArrayList data1 = new ArrayList();
                            data1.Add(a3_BagModel.getInstance().getItemDataById((uint)NeedItem_lianxie));
                            data1.Add(InterfaceMgr.A3_SUMMON_NEW);
                            if (getSummonWin().avatorobj != null)
                                data1.Add(getSummonWin().avatorobj);
                            else data1.Add(null);
                            ArrayList n = new ArrayList();
                            n.Add("lianxie");
                            data1.Add(n);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data1);
                        }
                        getlianxie.SetActive(false);
                    };
                    new BaseButton (getlianxie.transform.FindChild("tach")).onClick =
                    new BaseButton(getlianxie.transform.FindChild("no/Text")).onClick = (GameObject go) => {
                        getlianxie.SetActive(false);
                    };
                    break;
                case "uilayer_addlianxie":
                    GameObject addlianxie = getSummonWin()?.GetSmallWin(name);
                    addlianxie.transform.FindChild("yes/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_addlianxie_1");
                    addlianxie.transform.FindChild("no/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_addlianxie_2");
                    addlianxie.transform.FindChild("Text_mian").GetComponent<Text>().text = ContMgr.getCont("uilayer_addlianxie_3");
                    new BaseButton(addlianxie.transform.FindChild("yes/Text")).onClick = (GameObject go) => {
                        if (CanAdd)
                            A3_SummonProxy.getInstance().sendaddlx(CurSummonID);
                        else {
                            if (XMLMgr.instance.GetSXML("item.item", "id==" + AddlianxieId).GetNode("drop_info") == null)
                                return;
                            ArrayList data1 = new ArrayList();
                            data1.Add(a3_BagModel.getInstance().getItemDataById((uint)AddlianxieId));
                            data1.Add(InterfaceMgr.A3_SUMMON_NEW);
                            if (getSummonWin().avatorobj != null)
                                data1.Add(getSummonWin().avatorobj);
                            else data1.Add(null);
                            ArrayList n = new ArrayList();
                            n.Add("lianxie");
                            data1.Add(n);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data1);

                        }
                        addlianxie.SetActive(false);
                    };
                    new BaseButton(addlianxie.transform.FindChild("tach")).onClick =
                    new BaseButton(addlianxie.transform.FindChild("no/Text")).onClick = (GameObject go) => {
                        addlianxie.SetActive(false);
                    };
                    for (int i = 0; i < addlianxie.transform.FindChild("icon").childCount; i++)
                    {
                        GameObject.Destroy(addlianxie.transform.FindChild("icon").GetChild(i).gameObject);
                    }
                    int itemid = sumXml.GetNode("link").getInt("add_link");
                    a3_ItemData itemdata = a3_BagModel.getInstance().getItemDataById((uint)itemid);
                    GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(itemdata);
                    icon.transform.SetParent(addlianxie.transform.FindChild("icon"), false);
                    new BaseButton(icon.transform).onClick = (GameObject go) =>
                    {
                        ArrayList arr = new ArrayList();
                        arr.Add((uint)itemid);
                        arr.Add(1);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
                    };
                    break;
                case "uilayer_lock":
                    GameObject locking = getSummonWin()?.GetSmallWin(name);
                    locking.transform.FindChild("yes/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_lock_1");
                    locking.transform.FindChild("no/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_lock_2");
                    locking.transform.FindChild("Text_mian").GetComponent<Text>().text = ContMgr.getCont("uilayer_lock_3");
                    new BaseButton(locking.transform.FindChild("yes/Text")).onClick = (GameObject go) => {
                        if (CanLock)
                            A3_SummonProxy.getInstance().sendlocklx(CurSummonID, (uint)index, true);
                        else {
                            if (XMLMgr.instance.GetSXML("item.item", "id==" + lockItem).GetNode("drop_info") == null)
                                return;
                            ArrayList data1 = new ArrayList();
                            data1.Add(a3_BagModel.getInstance().getItemDataById((uint)lockItem));
                            data1.Add(InterfaceMgr.A3_SUMMON_NEW);
                            if (getSummonWin().avatorobj != null)
                                data1.Add(getSummonWin().avatorobj);
                            else data1.Add(null);
                            ArrayList n = new ArrayList();
                            n.Add("lianxie");
                            data1.Add(n);
                            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data1);

                        }
                        locking.SetActive(false);
                    };
                    new BaseButton(locking.transform.FindChild("tach")).onClick =
                    new BaseButton(locking.transform.FindChild("no/Text")).onClick = (GameObject go) => {
                       locking.SetActive(false);
                   };
                    for (int i = 0; i < locking.transform.FindChild("icon").childCount; i++)
                    {
                        GameObject.Destroy(locking.transform.FindChild("icon").GetChild(i).gameObject);
                    }
                    int itemid2 = sumXml.GetNode("link").getInt("change_lock");
                    a3_ItemData itemdata2 = a3_BagModel.getInstance().getItemDataById((uint)itemid2);
                    GameObject icon2 = IconImageMgr.getInstance().createA3ItemIcon(itemdata2);
                    icon2.transform.SetParent(locking.transform.FindChild("icon"), false);
                    new BaseButton(icon2.transform).onClick = (GameObject go) =>
                    {
                        ArrayList arr = new ArrayList();
                        arr.Add((uint)itemid2);
                        arr.Add(1);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
                    };
                    break;
                case "uilayer_unlock":
                    GameObject unlock = getSummonWin()?.GetSmallWin(name);
                    unlock.transform.FindChild("yes/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_unlock_1");
                    unlock.transform.FindChild("no/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_unlock_2");
                    unlock.transform.FindChild("Text_mian").GetComponent<Text>().text = ContMgr.getCont("uilayer_unlock_3");
                    new BaseButton(unlock.transform.FindChild("yes/Text")).onClick = (GameObject go) => {
                        A3_SummonProxy.getInstance().sendlocklx(CurSummonID, (uint)index, false );
                        unlock.SetActive(false);
                    };
                    new BaseButton(unlock.transform.FindChild("tach")).onClick =
                    new BaseButton(unlock.transform.FindChild("no/Text")).onClick = (GameObject go) => {
                        unlock.SetActive(false);
                    };
                    for (int i = 0; i < unlock.transform.FindChild("icon").childCount; i++)
                    {
                        GameObject.Destroy(unlock.transform.FindChild("icon").GetChild(i).gameObject);
                    }
                    int itemid1 = sumXml.GetNode("link").getInt("change_lock");
                    a3_ItemData itemdata1 = a3_BagModel.getInstance().getItemDataById((uint)itemid1);
                    GameObject icon1 = IconImageMgr.getInstance().createA3ItemIcon(itemdata1);
                    icon1.transform.SetParent(unlock.transform.FindChild("icon"), false);
                    new BaseButton(icon1.transform).onClick = (GameObject go) =>
                    {
                        ArrayList arr = new ArrayList();
                        arr.Add((uint)itemid1);
                        arr.Add(1);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
                    };
                    break;
            }
        }

        void setInfo() {
            if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                return;
            a3_BagItemData data = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
            tranObj.transform.FindChild("r1/info/lvl").GetComponent<Text>().text = "LV " + data.summondata.level;
            tranObj.transform.FindChild("r1/info/name").GetComponent<Text>().text = data.summondata.name;
            tranObj.transform.FindChild("r1/info/zhanli").GetComponent<Text>().text = data.summondata.power.ToString();

            tranObj.transform.FindChild("r1/info/att/life/Text").GetComponent<Text>().text = data.summondata.maxhp.ToString();
            tranObj.transform.FindChild("r1/info/att/phyatk/Text").GetComponent<Text>().text = data.summondata.min_attack + "-" + data.summondata.max_attack;
            tranObj.transform.FindChild("r1/info/att/phydef/Text").GetComponent<Text>().text = data.summondata.physics_def.ToString();
            tranObj.transform.FindChild("r1/info/att/manadef/Text").GetComponent<Text>().text = data.summondata.magic_def.ToString();
            tranObj.transform.FindChild("r1/info/att/hit/Text").GetComponent<Text>().text = (float)data.summondata.physics_dmg_red / 10 + "%";
            tranObj.transform.FindChild("r1/info/att/manaatk/Text").GetComponent<Text>().text = (float)data.summondata.magic_dmg_red / 10 + "%";
            tranObj.transform.FindChild("r1/info/att/crit/Text").GetComponent<Text>().text = data.summondata.double_damage_rate.ToString();
            tranObj.transform.FindChild("r1/info/att/dodge/Text").GetComponent<Text>().text = data.summondata.dodge.ToString();
            tranObj.transform.FindChild("r1/info/att/fatal_damage/Text").GetComponent<Text>().text = (float)data.summondata.fatal_damage / 10 + "%";
            tranObj.transform.FindChild("r1/info/att/hitit/Text").GetComponent<Text>().text = data.summondata.hit.ToString();
            tranObj.transform.FindChild("r1/info/att/reflect_crit_rate/Text").GetComponent<Text>().text = data.summondata.reflect_crit_rate.ToString();
        }

        int NeedItem_lianxie;
        bool CanGet = false;

        void useItem_num() {

            for (int i = 0; i < tranObj.transform.FindChild("r1/item/icon").childCount; i++)
            {
                GameObject.Destroy(tranObj.transform.FindChild("r1/item/icon").GetChild(i).gameObject);
            }
            int itemid = sumXml.GetNode("link").getInt("change_link");
            NeedItem_lianxie = itemid;
            int num = sumXml.GetNode("link").getInt("change_link_num");
            a3_ItemData itemdata = a3_BagModel.getInstance().getItemDataById((uint)itemid);
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(itemdata);
            icon.transform.SetParent(tranObj.transform.FindChild("r1/item/icon"),false);
            new BaseButton(icon.transform).onClick = (GameObject go) =>
            {
                ArrayList arr = new ArrayList();
                arr.Add((uint)itemid);
                arr.Add(1);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
            };
            int haveCount = a3_BagModel.getInstance().getItemNumByTpid((uint)itemid);

            if (num <= haveCount)
            {
                tranObj.transform.FindChild("r1/item/num").GetComponent<Text>().text = "<color=#00FF56FF>" + haveCount + "/" + num + "</color>";
                //tranObj.transform.FindChild("getbtn").GetComponent<Button>().interactable = true;
                CanGet = true;
            }
            else
            {
                tranObj.transform.FindChild("r1/item/num").GetComponent<Text>().text = "<color=#f90e0e>" + haveCount + "/" + num + "</color>";
                //tranObj.transform.FindChild("getbtn").GetComponent<Button>().interactable = false;
                CanGet = false;
            }

        }

        void setlianxie_Value()
        {
            if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                return;
            a3_BagItemData data = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
            if (data.summondata.linkdata == null) return;
            GameObject item = tranObj.transform.FindChild("r1/main/con_btn/scrollview/btnTemp").gameObject;
            Transform con = tranObj.transform.FindChild("r1/main/con_btn/scrollview/con");
            for (int i = 0; i < con.childCount; i++ ) {
                GameObject.Destroy(con.GetChild (i).gameObject );
            }
             
            foreach (int idx in data.summondata.linkdata.Keys ) {
                GameObject clon = GameObject.Instantiate(item) as GameObject;
                clon.SetActive(true);
                clon.transform.SetParent(con,false);
                if (data.summondata.linkdata[idx].type == 0)
                {
                    clon.transform.FindChild("nulType").gameObject.SetActive(true);
                }
                else {
                    clon.transform.FindChild("have").gameObject.SetActive(true);
                    
                    string str = data.summondata.linkdata[idx].per + "%";
                    if (data.summondata.linkdata[idx].per >= 1 && data.summondata.linkdata[idx].per <= 5) { str =  "<color=#00FF00>" + str + "</color>"; }
                    else if (data.summondata.linkdata[idx].per >= 6 && data.summondata.linkdata[idx].per <= 9) { str = "<color=#66FFFF>" + str + "</color>"; }
                    else if (data.summondata.linkdata[idx].per >= 10 && data.summondata.linkdata[idx].per <= 13) { str = "<color=#FF00FF>" + str + "</color>"; }
                    else if (data.summondata.linkdata[idx].per >= 14 && data.summondata.linkdata[idx].per <= 19) { str = "<color=#f7790a>" + str + "</color>"; }
                    else if (data.summondata.linkdata[idx].per >= 20) { str = "<color=#f90e0e>" + str + "</color>"; }
                    clon.transform.FindChild("have/value").GetComponent<Text>().text = str;
                    int attvalue = (int)Math.Ceiling((A3_SummonModel.getInstance().getAttValue(data.summondata, data.summondata.linkdata[idx].type) * ((float)data.summondata.linkdata[idx].per / 100.00f)));
                    
                    clon.transform .FindChild ("have/att").GetComponent <Text>().text = Globle.getAttrAddById(data.summondata.linkdata[idx].type, attvalue);
                    bool loack = data.summondata.linkdata[idx].lock_state;


                    if (loack)
                    {
                        clon.transform.FindChild("have/loack").GetComponent<Image>().enabled = false;
                        clon.transform.FindChild("have/loack/b").gameObject.SetActive(loack);
                    }
                    else {
                        clon.transform.FindChild("have/loack").GetComponent<Image>().enabled = true;
                        clon.transform.FindChild("have/loack/b").gameObject.SetActive(loack);
                    }

                    int indx = idx;
                    new BaseButton(clon.transform.FindChild("have/loack")).onClick = (GameObject go) => {
                        if (loack)
                        {
                            if (getSummonWin() == null) return;
                            GameObject toSrue = getSummonWin().GetSmallWin("uilayer_unlock");
                            index = indx;
                            toSrue.SetActive(true);
                        }
                        else
                        {
                            if (getSummonWin() == null) return;
                            GameObject toSrue = getSummonWin().GetSmallWin("uilayer_lock");
                            index = indx;
                            toSrue.SetActive(true);
                            int num = sumXml.GetNode("link").getInt("change_lock_num");
                            int itemid = sumXml.GetNode("link").getInt("change_lock");
                            lockItem = itemid;
                            int haveCount = a3_BagModel.getInstance().getItemNumByTpid((uint)itemid);

                            if (num <= haveCount)
                            {
                                toSrue.transform.FindChild("num").GetComponent<Text>().text = "<color=#00FF56FF>" + haveCount + "/" + num + "</color>";
                                //toSrue.transform.FindChild("yes").GetComponent<Button>().interactable = true;
                                CanLock = true;
                            }
                            else
                            {
                                toSrue.transform.FindChild("num").GetComponent<Text>().text = "<color=#f90e0e>" + haveCount + "/" + num + "</color>";
                                //toSrue.transform.FindChild("yes").GetComponent<Button>().interactable = false;
                                CanLock = false;
                            }
                        }
                    };
                }
            }
            if (data.summondata.linkdata.Count < data.summondata.star)
            {
                GameObject clon_null = GameObject.Instantiate(item) as GameObject;
                clon_null.transform.FindChild("nul").gameObject.SetActive(true);
                clon_null.transform.SetParent(con, false);
                clon_null.SetActive(true);
                new BaseButton(clon_null.transform.FindChild("nul/btn")).onClick = (GameObject go) =>
                {
                    if (getSummonWin() == null) return;
                    GameObject toSrue = getSummonWin().GetSmallWin("uilayer_addlianxie");
                    toSrue.SetActive(true);
                    int num = sumXml.GetNode("link").getInt("add_link_num");
                    int itemid = sumXml.GetNode("link").getInt("add_link");
                    AddlianxieId = itemid;
                    int haveCount = a3_BagModel.getInstance().getItemNumByTpid((uint)itemid);

                    if (num <= haveCount)
                    {
                        toSrue.transform.FindChild("num").GetComponent<Text>().text = "<color=#00FF56FF>" + haveCount + "/" + num + "</color>";
                        //toSrue.transform.FindChild("yes").GetComponent<Button>().interactable = true;
                        CanAdd = true;
                    }
                    else
                    {
                        toSrue.transform.FindChild("num").GetComponent<Text>().text = "<color=#f90e0e>" + haveCount + "/" + num + "</color>";
                        //toSrue.transform.FindChild("yes").GetComponent<Button>().interactable = false;
                        CanAdd = false;
                    }
                };
            }
        }

        bool CheckSummon() {
            bool canlink = false;
            foreach (a3_BagItemData sm in A3_SummonModel.getInstance().GetSummons().Values)
            {
                if (canlink) break;
                foreach (int idx in sm.summondata.linkdata.Keys)
                {
                    if (sm.summondata.linkdata[idx].type != 0)
                    {
                        canlink = true;
                        break;
                    }
                }
            }
            return canlink;
        }

        int AddlianxieId;
        bool CanAdd = false;

        int lockItem;
        bool CanLock = false;

        void onlianxie(GameEvent v) {
            setlianxie_Value();
            useItem_num();
        }
        public override void onCurSummonIdChange()
        {
            base.onCurSummonIdChange();
            setBtn();
            setframe();
            if (r1.activeSelf) {
                setInfo();
                setlianxie_Value();
            }
            else if (r2.activeSelf ) {
                setInfo_lianxie();
            }
        }

        public void OpenlianxieCon() {
            r1.SetActive(false);
            r2.SetActive(true);

            refreSumlist((int)CurSummonID, true);

            setInfo_lianxie();
            onlink(null);
            SetFristSelect();
        }
        public void CloselianxieCon() {
            r1.SetActive(true);
            r2.SetActive(false);

            refreSumlist((int)CurSummonID, false , 6);
            setInfo();
            setlianxie_Value();
        }


        void onlink(GameEvent v) {
            Transform con = r2.transform.FindChild("sumcon/summons/scroll/content");
            GameObject item = r2.transform.FindChild("sumcon/summons/scroll/0").gameObject;
            for (int i = 0; i < con.childCount; i++ ) {
                GameObject.Destroy(con.GetChild (i).gameObject);
            }
            summonObj.Clear();
            foreach (uint id in A3_SummonModel .getInstance ().link_list) {
                a3_BagItemData data = A3_SummonModel.getInstance().GetSummons()[id];
                GameObject clon = GameObject.Instantiate(item) as GameObject;
                clon.SetActive(true);
                clon.transform.SetParent(con.transform, false);
                clon.name = data.summondata.id.ToString();
                clon.transform.FindChild("articles").gameObject.SetActive(true);
                new BaseButton(clon.transform).onClick = (GameObject go) => {
                    getSummonWin().setCurSummonID((uint)data.summondata.id);
                    getSummonWin().setframe();
                    setframe();
                };
                summonObj[(uint)data.summondata.id] = clon;
                setSum_one((uint)data.summondata.id);
            }
            setframe();
            setBtn();
            refreSumlist((int)CurSummonID, true);
        }

        void setBtn() {
            if (A3_SummonModel.getInstance().link_list.Contains(CurSummonID))
            {
                r2.transform.FindChild("cancel").gameObject.SetActive(true);
                r2.transform.FindChild("Battle").gameObject.SetActive(false);
            }
            else {
                r2.transform.FindChild("cancel").gameObject.SetActive(false);
                r2.transform.FindChild("Battle").gameObject.SetActive(true);
            }
        }


        public void setSum_one(uint id)
        {
            if (summonObj.ContainsKey(id))
            {
                a3_BagItemData it = A3_SummonModel.getInstance().GetSummons()[id];
                summonObj[id].transform.FindChild("lv").GetComponent<Text>().text = it.summondata.level.ToString();
                SetIcon(it, summonObj[id].transform.FindChild("icon"));
                setStar(summonObj[id].transform.FindChild("stars"), it.summondata.star);
            }
        }

        public GameObject SetIcon(a3_BagItemData data, Transform parent, int num = -1)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                GameObject.Destroy(parent.GetChild(i).gameObject);
            }
            data.confdata.borderfile = "icon_itemborder_b039_0" + (data.summondata.star);
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data.confdata, false, num, 1, false, -1, 0, false, false);
            icon.transform.SetParent(parent, false);
            icon.transform.localScale = new Vector3(.9f, .9f, 0);
            return icon;
        }
        public void setStar(Transform starRoot, int num)
        {
            for (int i = 0; i < 5; i++)
            {
                starRoot.GetChild(i).FindChild("b").gameObject.SetActive(false);
            }

            for (int j = 0; j < num; j++)
            {
                starRoot.GetChild(j).FindChild("b").gameObject.SetActive(true);
            }
        }
        void SetLinkSum() {
        }

        public void  SetFristSelect() {
            if (CurSummonID <= 0 && r2.transform.FindChild("sumcon/summons/scroll/content").childCount > 0) {
                getSummonWin().setCurSummonID(uint.Parse(r2.transform.FindChild("sumcon/summons/scroll/content").GetChild(0).gameObject.name));
                getSummonWin().setframe();
                setframe();
            }
        }

        void setframe()
        {
            foreach (uint id in summonObj.Keys)
            {
                if (id == CurSummonID)
                    summonObj[id].transform.FindChild("frame").gameObject.SetActive(true);
                else
                    summonObj[id].transform.FindChild("frame").gameObject.SetActive(false);
            }
        }

        Dictionary<uint, GameObject> summonObj = new Dictionary<uint, GameObject>();
        void setInfo_lianxie() {
            if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                return;
            a3_BagItemData data = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
            tranObj.transform.FindChild("r2/lvl").GetComponent<Text>().text = "LV " + data.summondata.level;
            tranObj.transform.FindChild("r2/name").GetComponent<Text>().text = data.summondata.name;
            tranObj.transform.FindChild("r2/Combpt").GetComponent<Text>().text = data.summondata.linkCombpt .ToString ();
            for (int i = 0;  i< attCon.childCount;i++ ) {
                attCon.GetChild(i).gameObject.SetActive(false);
            }
            int n = 1;
            foreach (int idx in data.summondata.linkdata.Keys) {
                attCon.FindChild("att" + n).gameObject.SetActive(true);
                attCon.FindChild("att" + n).FindChild("null").gameObject.SetActive(false);
                attCon.FindChild("att" + n).FindChild("att").gameObject.SetActive(false);
                if (data.summondata.linkdata[idx].type == 0)
                {
                    attCon.FindChild("att" + n).FindChild("null").gameObject.SetActive(true);
                }
                else
                {
                    attCon.FindChild("att" + n).FindChild("att").gameObject.SetActive(true);
                    int attvalue = (int)Math.Ceiling((A3_SummonModel.getInstance().getAttValue(data.summondata, data.summondata.linkdata[idx].type) * ((float)data.summondata.linkdata[idx].per / 100.00f)));

                    attCon.FindChild("att" + n).FindChild("att").GetComponent<Text>().text = Globle.getAttrAddById(data.summondata.linkdata[idx].type, attvalue);
                }
                n++;
            }
        }
    }
}
