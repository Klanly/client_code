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
    enum Todo_type {
        nul =0,
        tochange = 1,
        tosave = 2
    }
    class a3_summon_xilian : BaseSummon
    {
        Todo_type curtype_do = Todo_type.nul;
        public a3_summon_xilian(Transform trans, string name) : base(trans, name)
        {
            init();
        }

        GameObject helpcon;
        void init()
        {
            tranObj.transform.FindChild("old/top").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_0");
            tranObj.transform.FindChild("old/text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_1");
            tranObj.transform.FindChild("old/minjie/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_2");
            tranObj.transform.FindChild("old/tili/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_3");
            tranObj.transform.FindChild("old/gongji/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_4");
            tranObj.transform.FindChild("old/fangyu/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_5");
            tranObj.transform.FindChild("old/xingyun/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_6");
            tranObj.transform.FindChild("new/title/top").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_7");
            tranObj.transform.FindChild("new/text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_1");
            tranObj.transform.FindChild("new/minjie/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_2");
            tranObj.transform.FindChild("new/tili/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_3");
            tranObj.transform.FindChild("new/gongji/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_4");
            tranObj.transform.FindChild("new/fangyu/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_5");
            tranObj.transform.FindChild("new/xingyun/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_6");
            tranObj.transform.FindChild("save/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_8");
            tranObj.transform.FindChild("todo/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_9");
            tranObj.transform.FindChild("needitem").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_10");
            tranObj.transform.FindChild("help/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_11");//help提示
            tranObj.transform.FindChild("help/close/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_12");//知道了
            new BaseButton(tranObj.transform.FindChild("todo")).onClick = (GameObject go) =>
           {
               if (curSummon_wmd >= curSummon_wmd_new)
               {
                   if (CanDo_change)
                   {
                       A3_SummonProxy.getInstance().sendXilian(CurSummonID);
                   }
                   else {
                       if (XMLMgr.instance.GetSXML("item.item", "id==" + NeedItemId).GetNode("drop_info") == null)
                           return;
                       ArrayList data1 = new ArrayList();
                       data1.Add(a3_BagModel.getInstance().getItemDataById((uint)NeedItemId));
                       data1.Add(InterfaceMgr.A3_SUMMON_NEW);
                       if (getSummonWin().avatorobj != null)
                           data1.Add(getSummonWin().avatorobj);
                       else data1.Add(null);
                       ArrayList n = new ArrayList();
                       n.Add("xilian");
                       data1.Add(n);
                       InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data1);
                   }
               }
               else
               {
                   if (getSummonWin() == null) return;
                   GameObject plan = getSummonWin().GetSmallWin("uilayer_savecon");
                   plan.transform.FindChild("Text_mian").GetComponent<Text>().text = ContMgr.getCont("savecon_1");

                   curtype_do = Todo_type.tochange;
                   setChangeCon(plan, Todo_type.tochange);
               }


           };
            helpcon = tranObj.transform.FindChild("help").gameObject;
            new BaseButton(tranObj.transform.FindChild("help_btn")).onClick = (GameObject go) => {
                helpcon.SetActive(true);
            };
            new BaseButton(helpcon.transform.FindChild("close")).onClick = (GameObject go) => {
                helpcon.SetActive(false);
            };
            new BaseButton(tranObj.transform.FindChild("save")).onClick = (GameObject go) => 
            {

                if (curSummon_wmd < curSummon_wmd_new)
                    A3_SummonProxy.getInstance().sendXilian_save(CurSummonID);
                else
                {
                    if (getSummonWin() == null) return;
                    GameObject plan = getSummonWin().GetSmallWin("uilayer_savecon");

                    plan.transform.FindChild("Text_mian").GetComponent<Text>().text = ContMgr.getCont("savecon_1");
                    curtype_do = Todo_type.tosave;
                    setChangeCon(plan, Todo_type.tosave);
                }
            };

            
        }
        public override void onShowed()
        {
            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_XILIAN, onXilain);
            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_SAVE, onsave);
            helpcon.SetActive(false);
            SetCurSuminfo();
            SetNeedItem();
            SetNewSumInfo();
            curtype_do = Todo_type.nul;
            closeWin("uilayer_savecon");
        }
        public override void onClose()
        {
            A3_SummonProxy.getInstance().removeEventListener(A3_SummonProxy.EVENT_XILIAN, onXilain);
            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_SAVE, onsave);
        }
        public override void onCurSummonIdChange() {
            base.onCurSummonIdChange();
            SetCurSuminfo();
            SetNeedItem();
            SetNewSumInfo();
        }

        public override void onAddNewSmallWin(string name)
        {
            switch (name)
            {
                case "uilayer_savecon":
                    GameObject saveCon = getSummonWin()?.GetSmallWin(name);
                    saveCon.transform.FindChild("Text_top").GetComponent<Text>().text = ContMgr.getCont("savecon_0");
                    saveCon.transform.FindChild("no/Text").GetComponent<Text>().text = ContMgr.getCont("savecon_2");
                    saveCon.transform.FindChild("yes/Text").GetComponent<Text>().text = ContMgr.getCont("ToSure_summon_1");
                    new BaseButton(saveCon.transform.FindChild("no")).onClick =
                    new BaseButton(saveCon.transform.FindChild("tach")).onClick = (GameObject go) => { saveCon.SetActive(false); };
                    new BaseButton(saveCon.transform.FindChild("yes")).onClick = 
                        (GameObject go) =>
                        { if (curtype_do == Todo_type.tosave)
                            { A3_SummonProxy.getInstance().sendXilian_save(CurSummonID); }
                            else if (curtype_do == Todo_type.tochange)
                            {
                                A3_SummonProxy.getInstance().sendXilian(CurSummonID);
                            }
                            saveCon.SetActive(false);
                        };
                    break;
            }
        }


        void setChangeCon(GameObject plan, Todo_type type) {
            plan.SetActive(true);
            if (type == Todo_type.tochange) {
                plan.transform.FindChild("Text_top").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xl_top1");
                plan.transform.FindChild("Text_mian").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xl_mian1");
            }
            else if (type == Todo_type.tosave) {
                plan.transform.FindChild("Text_top").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xl_top2");
                plan.transform.FindChild("Text_mian").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xl_mian2");
            }
        }
        void SetCurSuminfo()
        {
            if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                return;
            a3_BagItemData data = A3_SummonModel.getInstance().GetSummons()[CurSummonID];

            // tranObj.transform.FindChild("old/name").GetComponent<Text>().text = data.summondata.name;

            int count = tranObj.transform.FindChild("old/icon").childCount;
            for (int i =0;i< count; i++)
            {
                GameObject.Destroy(tranObj.transform.FindChild("old/icon").GetChild(i).gameObject);
            }
            data.confdata.borderfile = "icon_itemborder_b039_0" + (data.summondata.star);
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data.confdata, false, -1, 1, false, -1, 0, false, false);
            icon.transform.SetParent(tranObj .transform.FindChild ("old/icon"), false);
            SXML xml = sumXml.GetNode("callbeast", "id==" + data.summondata.tpid);
            SXML attxml = xml.GetNode("star", "star_sum==" + data.summondata.star);

            tranObj.transform.FindChild("old/gongji/value").GetComponent<Text>().text = data.summondata.attNatural + "/" + attxml.GetNode("att").getInt("reset_max");
            tranObj.transform.FindChild("old/fangyu/value").GetComponent<Text>().text = data.summondata.defNatural + "/" + attxml.GetNode("def").getInt("reset_max");
            tranObj.transform.FindChild("old/minjie/value").GetComponent<Text>().text = data.summondata.agiNatural + "/" + attxml.GetNode("agi").getInt("reset_max");
            tranObj.transform.FindChild("old/tili/value").GetComponent<Text>().text = data.summondata.conNatural + "/" + attxml.GetNode("con").getInt("reset_max");
            tranObj.transform.FindChild("old/xingyun/value").GetComponent<Text>().text = data.summondata.luck.ToString ();

            float attValue = (float)data.summondata.attNatural / (float)attxml.GetNode("att").getInt("reset_max");
            float defValue = (float)data.summondata.defNatural / (float)attxml.GetNode("def").getInt("reset_max");
            float agiValue = (float)data.summondata.agiNatural / (float)attxml.GetNode("agi").getInt("reset_max");
            float conValue = (float)data.summondata.conNatural / (float)attxml.GetNode("con").getInt("reset_max");

            tranObj.transform.FindChild("old/gongji/slider").GetComponent<Image>().fillAmount = attValue;
            tranObj.transform.FindChild("old/fangyu/slider").GetComponent<Image>().fillAmount = defValue;
            tranObj.transform.FindChild("old/minjie/slider").GetComponent<Image>().fillAmount = agiValue;
            tranObj.transform.FindChild("old/tili/slider").GetComponent<Image>().fillAmount = conValue;

            int allValue =  (int)(((attValue + defValue + agiValue + conValue )/4)*100);
            tranObj.transform.FindChild("old/value").GetComponent<Text>().text = allValue + "%";

            curSummon_wmd = allValue;

        }

        bool CanDo_change = false;
        int NeedItemId;

        void SetNeedItem()
        {
            if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                return;
            a3_BagItemData data = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
            List<SXML> reset = sumXml.GetNodeList("reset");
            int itemid = 0;
            int num = 0;
            foreach (SXML it in reset)
            {
                if (data.summondata.star == it.getInt("star"))
                {
                    itemid = it.getInt("need_item_id");
                    NeedItemId = itemid;
                    num = it.getInt("need_item_sum");
                    break;
                }
            }
            if (itemid> 0&& num!= 0)
            {
                for (int i = 0; i < tranObj.transform.FindChild("needitem/icon").childCount; i++)
                {
                    GameObject.Destroy(tranObj.transform.FindChild("needitem/icon").GetChild(i).gameObject);
                }
                a3_ItemData itemdata = a3_BagModel.getInstance().getItemDataById((uint)itemid);
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(itemdata);
                icon.transform.SetParent(tranObj.transform.FindChild("needitem/icon"), false);
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
                    tranObj.transform.FindChild("needitem/count").GetComponent<Text>().text = "<color=#00FF56FF>" + haveCount + "/" + num + "</color>";
                    //tranObj.transform.FindChild("todo").GetComponent<Button>().interactable = true;
                    CanDo_change = true;
                }
                else
                {
                    tranObj.transform.FindChild("needitem/count").GetComponent<Text>().text = "<color=#f90e0e>" + haveCount + "/" + num + "</color>";
                    //tranObj.transform.FindChild("todo").GetComponent<Button>().interactable = false;
                    CanDo_change = false;
                }
            }
        }


        int curSummon_wmd = 0;
        int curSummon_wmd_new = 0;
        void SetNewSumInfo()
        {
            if (!A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
                return;
            a3_BagItemData data = A3_SummonModel.getInstance().GetSummons()[CurSummonID];
            if (data.summondata.haveReset)
            {

                //tranObj.transform.FindChild("new/name").GetComponent<Text>().text = data.summondata.name;
                for (int i = 0; i < tranObj.transform.FindChild("old/icon").childCount; i++)
                {
                    GameObject.Destroy(tranObj.transform.FindChild("old/icon").GetChild(i).gameObject);
                }
                data.confdata.borderfile = "icon_itemborder_b039_0" + (data.summondata.star);
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data.confdata, false, -1, 1, false, -1, 0, false, false);
                icon.transform.SetParent(tranObj.transform.FindChild("old/icon"), false);
                SXML xml = sumXml.GetNode("callbeast", "id==" + data.summondata.tpid);
                SXML attxml = xml.GetNode("star", "star_sum==" + data.summondata.star);
                tranObj.transform.FindChild("new/gongji/value").GetComponent<Text>().text = data.summondata.resetatt + "/" + attxml.GetNode("att").getInt("reset_max");
                tranObj.transform.FindChild("new/fangyu/value").GetComponent<Text>().text = data.summondata.resetdef + "/" + attxml.GetNode("def").getInt("reset_max");
                tranObj.transform.FindChild("new/minjie/value").GetComponent<Text>().text = data.summondata.resetagi + "/" + attxml.GetNode("agi").getInt("reset_max");
                tranObj.transform.FindChild("new/tili/value").GetComponent<Text>().text = data.summondata.resetcon + "/" + attxml.GetNode("con").getInt("reset_max");
                tranObj.transform.FindChild("new/xingyun/value").GetComponent<Text>().text = data.summondata.resetluck .ToString ();

                float attValue = (float)data.summondata.resetatt / (float)attxml.GetNode("att").getInt("reset_max");
                float defValue = (float)data.summondata.resetdef / (float)attxml.GetNode("def").getInt("reset_max");
                float agiValue = (float)data.summondata.resetagi / (float)attxml.GetNode("agi").getInt("reset_max");
                float conValue = (float)data.summondata.resetcon / (float)attxml.GetNode("con").getInt("reset_max");

                if (data.summondata.resetatt >= data.summondata.attNatural)
                {
                    tranObj.transform.FindChild("new/gongji/slider_up").gameObject.SetActive(true);
                    tranObj.transform.FindChild("new/gongji/slider_down").gameObject.SetActive(false);
                    tranObj.transform.FindChild("new/gongji/slider_up").GetComponent<Image>().fillAmount = attValue;
                }
                else {
                    tranObj.transform.FindChild("new/gongji/slider_up").gameObject.SetActive(false);
                    tranObj.transform.FindChild("new/gongji/slider_down").gameObject.SetActive(true);
                    tranObj.transform.FindChild("new/gongji/slider_down").GetComponent<Image>().fillAmount = attValue;
                }
                if (data.summondata.resetdef >= data.summondata.defNatural)
                {
                    tranObj.transform.FindChild("new/fangyu/slider_up").gameObject.SetActive(true);
                    tranObj.transform.FindChild("new/fangyu/slider_down").gameObject.SetActive(false);
                    tranObj.transform.FindChild("new/fangyu/slider_up").GetComponent<Image>().fillAmount = defValue;
                }
                else {
                    tranObj.transform.FindChild("new/fangyu/slider_up").gameObject.SetActive(false);
                    tranObj.transform.FindChild("new/fangyu/slider_down").gameObject.SetActive(true);
                    tranObj.transform.FindChild("new/fangyu/slider_down").GetComponent<Image>().fillAmount = defValue;
                }

                if (data.summondata.resetagi >= data.summondata.agiNatural)
                {
                    tranObj.transform.FindChild("new/minjie/slider_up").gameObject.SetActive(true);
                    tranObj.transform.FindChild("new/minjie/slider_down").gameObject.SetActive(false);
                    tranObj.transform.FindChild("new/minjie/slider_up").GetComponent<Image>().fillAmount = agiValue;
                }
                else
                {
                    tranObj.transform.FindChild("new/minjie/slider_up").gameObject.SetActive(false);
                    tranObj.transform.FindChild("new/minjie/slider_down").gameObject.SetActive(true);
                    tranObj.transform.FindChild("new/minjie/slider_down").GetComponent<Image>().fillAmount = agiValue;

                }

                if (data.summondata.resetcon >= data.summondata.conNatural)
                {
                    tranObj.transform.FindChild("new/tili/slider_up").gameObject.SetActive(true);
                    tranObj.transform.FindChild("new/tili/slider_down").gameObject.SetActive(false);
                    tranObj.transform.FindChild("new/tili/slider_up").GetComponent<Image>().fillAmount = conValue;
                }
                else
                {
                    tranObj.transform.FindChild("new/tili/slider_up").gameObject.SetActive(false );
                    tranObj.transform.FindChild("new/tili/slider_down").gameObject.SetActive(true);
                    tranObj.transform.FindChild("new/tili/slider_down").GetComponent<Image>().fillAmount = conValue;
                }

                int allValue = (int)(((attValue + defValue + agiValue + conValue ) / 4) * 100);
                tranObj.transform.FindChild("new/value").GetComponent<Text>().text = allValue + "%";

                curSummon_wmd_new = allValue;

                tranObj.transform.FindChild("new").gameObject.SetActive(true);
                tranObj.transform.FindChild("save").gameObject.SetActive(true);
            }
            else
            {
                tranObj.transform.FindChild("new").gameObject.SetActive(false);
                tranObj.transform.FindChild("save").gameObject.SetActive(false);
                curSummon_wmd_new = 0;
            }
        }

        void onXilain(GameEvent evt)
        {
            Variant data = evt.data;
            if (data.ContainsKey ("reset_info"))
            {
                SetNeedItem();
                SetNewSumInfo();
            }
        }
        void onsave(GameEvent evt)
        {
            Variant data = evt.data;
            SetCurSuminfo();
            SetNewSumInfo();
        }
    }
}
