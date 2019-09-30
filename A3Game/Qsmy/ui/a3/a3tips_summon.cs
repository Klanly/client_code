using System.Collections;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace MuGame
{
    internal class a3tips_summon : Window
    {
        private uint curid;
        private a3_BagItemData item_data;
        private int cur_num;
		Action backevent;

        public override void init()
        {
            BaseButton btn_close = new BaseButton(transform.FindChild("touch"));
            btn_close.onClick = onclose;
            //BaseButton btn_identify = new BaseButton(transform.FindChild("info/btn_identify"));
            //btn_identify.onClick = onidentify;
            //BaseButton btn_sell = new BaseButton(transform.FindChild("info/sell"));
            //btn_sell.onClick = onsell;

            BaseButton details_sell = new BaseButton(transform.FindChild("details/info/action/sell/Button"));
            details_sell.onClick = onsell;
            BaseButton details_use = new BaseButton(transform.FindChild("details/info/action/use/Button"));
            details_use.onClick = onuse;

            getComponentByPath<Text>("details/info/natural/title").text = ContMgr.getCont("a3tips_summon_0");
            getComponentByPath<Text>("details/info/att/title").text = ContMgr.getCont("a3tips_summon_1");
            getComponentByPath<Text>("details/info/action/use/Button/Text").text = ContMgr.getCont("a3tips_summon_2");
            getComponentByPath<Text>("details/info/action/sell/Button/Text").text = ContMgr.getCont("a3tips_summon_3");

        }

        public override void onShowed()
        {
            transform.SetAsLastSibling();

            if (uiData == null)
                return;
            if (uiData.Count != 0)
            {
                //curid = (uint)uiData[0];
                item_data = (a3_BagItemData)uiData[0];
				if (uiData.Count>2)
				backevent = (Action)uiData[2]; 
                if (item_data.isSummon)
                {
                    var de = transform.transform.FindChild("details");
                    if (de != null) de.gameObject.SetActive(true);
                    initItemDetail();
                }
                else
                {
                    //transform.transform.FindChild("info").gameObject.SetActive(true);
                    //var de = transform.transform.FindChild("details");
                    //if (de != null) de.gameObject.SetActive(false);
                    //initItemInfo();
                }
                if (uiData.Count > 1)
                {
                    transform.FindChild("details/info/action/sell").gameObject.SetActive(false);
                    transform.FindChild("details/info/action/use").gameObject.SetActive(false);
                }
                else
                {
                    transform.FindChild("details/info/action/sell").gameObject.SetActive(true);
                    transform.FindChild("details/info/action/use").gameObject.SetActive(true);
                }
            }
        }

        public override void onClosed()
        {
			if (backevent != null) backevent();
        }

        //private void initItemInfo()
        //{
        //    Transform info = transform.FindChild("info");

        //    info.FindChild("name").GetComponent<Text>().text = item_data.confdata.item_name;
        //    info.FindChild("name").GetComponent<Text>().color = Globle.getColorByQuality(item_data.confdata.quality);
        //    info.FindChild("desc").GetComponent<Text>().text = item_data.confdata.desc;
        //    info.FindChild("value").GetComponent<Text>().text = item_data.confdata.value.ToString();

        //    Transform Image = info.FindChild("icon");
        //    if (Image.childCount > 0)
        //    {
        //        Destroy(Image.GetChild(0).gameObject);
        //    }
        //    GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(item_data);
        //    icon.transform.SetParent(Image, false);

        //    cur_num = 1;

        //    var spestr = item_data.summondata.isSpecial ? ContMgr.getCont("a3_summon16") +"*" : "";
        //    info.FindChild("grade").GetComponent<Text>().text = A3_SummonModel.getInstance().IntGradeToStr(item_data.summondata.grade);
        //    info.FindChild("type").GetComponent<Text>().text = A3_SummonModel.getInstance().IntNaturalToStr(item_data.summondata.naturaltype);
        //}

        private void initItemDetail()
        {
            Transform info = transform.FindChild("details/info");
            info.FindChild("name").GetComponent<Text>().text = item_data.summondata.name;
            info.FindChild("name").GetComponent<Text>().color = Globle.getColorByQuality(item_data.summondata.grade + 1);
            info.FindChild("basic/left/lv").GetComponent<Text>().text = ContMgr.getCont("a3_summon12") + item_data.summondata.level;
            //info.FindChild("basic/left/blood").GetComponent<Text>().text = ContMgr.getCont("a3_summon13") + (item_data.summondata.blood > 1 ? ContMgr.getCont("a3_summon14") : ContMgr.getCont("a3_summon15"));
            var spestr = item_data.summondata.isSpecial ? ContMgr.getCont("a3_summon16") + "*" : "";
           // info.FindChild("basic/left/grade").GetComponent<Text>().text = ContMgr.getCont("a3_summon25") + A3_SummonModel.getInstance().IntGradeToStr(item_data.summondata.grade);
            info.FindChild("basic/right/lifespan").GetComponent<Text>().text = ContMgr.getCont("a3_summon7") + "：" + item_data.summondata.lifespan;
            info.FindChild("basic/right/luck").GetComponent<Text>().text = ContMgr.getCont("a3_summon27") + item_data.summondata.luck;
            //info.FindChild("basic/right/type").GetComponent<Text>().text = ContMgr.getCont("a3_summon17") + A3_SummonModel.getInstance().IntNaturalToStr(item_data.summondata.naturaltype);
            info.FindChild("natural/values/1").GetComponent<Text>().text = ContMgr.getCont("a3tips_summon0") + item_data.summondata.attNatural;
            info.FindChild("natural/values/2").GetComponent<Text>().text = ContMgr.getCont("a3tips_summon1") + item_data.summondata.defNatural;
            info.FindChild("natural/values/3").GetComponent<Text>().text = ContMgr.getCont("a3tips_summon2") + item_data.summondata.agiNatural;
            info.FindChild("natural/values/4").GetComponent<Text>().text = ContMgr.getCont("a3tips_summon3") + item_data.summondata.conNatural;
            info.FindChild("att/values/1").GetComponent<Text>().text = ContMgr.getCont("a3tips_summon4") + item_data.summondata.maxhp;
            info.FindChild("att/values/2").GetComponent<Text>().text = ContMgr.getCont("a3tips_summon5") + item_data.summondata.min_attack + " ~ " + item_data.summondata.max_attack;
            info.FindChild("att/values/3").GetComponent<Text>().text = ContMgr.getCont("a3tips_summon6") + item_data.summondata.physics_def;
            info.FindChild("att/values/4").GetComponent<Text>().text = ContMgr.getCont("a3tips_summon7") + item_data.summondata.magic_def;
            info.FindChild("att/values/5").GetComponent<Text>().text = ContMgr.getCont("a3tips_summon8") + (float)item_data.summondata.physics_dmg_red / 10 + "%";
            info.FindChild("att/values/6").GetComponent<Text>().text = ContMgr.getCont("a3tips_summon9") + (float)item_data.summondata.magic_dmg_red / 10 + "%";
            info.FindChild("att/values/7").GetComponent<Text>().text = ContMgr.getCont("a3tips_summon10") + item_data.summondata.double_damage_rate;
            info.FindChild("att/values/8").GetComponent<Text>().text = ContMgr.getCont("a3tips_summon11") + item_data.summondata.reflect_crit_rate;
            var starRoot = info.FindChild("stars");
            SetStar(starRoot, item_data.summondata.star);

            Transform Image = info.FindChild("icon");
            if (Image.childCount > 0)
            {
                Destroy(Image.GetChild(0).gameObject);
            }
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(item_data);
            icon.transform.SetParent(Image, false);

            var SkillCon = this.transform.FindChild("skills");
            for (int i = 0; i < SkillCon.childCount; i++)
            {
                SkillCon.GetChild(i).FindChild("icon/icon").gameObject.SetActive(false);
                SkillCon.GetChild(i).FindChild("lock").gameObject.SetActive(true);
            }
            int idner = 1;
            foreach (summonskill skill in item_data.summondata.skills.Values)
            {
                Transform skillCell = SkillCon.FindChild(idner.ToString());
                skillCell.FindChild("icon/icon").gameObject.SetActive(true);
                skillCell.FindChild("lock").gameObject.SetActive(false);
                SXML xx = XMLMgr.instance.GetSXML("skill.skill", "id==" + skill.skillid);
                skillCell.FindChild("icon/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_smskill_" + xx.getInt("icon"));
                idner++;
            }
        }

        private bool needEvent = true;

        private void onclose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3TIPS_SUMMON);
        }

        private void SetStar(Transform starRoot, int num)
        {
            int count = 0;
            foreach (var v in starRoot.GetComponentsInChildren<Transform>(true))
            {
                if (v.parent != null && v.parent.parent == starRoot.transform)
                {
                    if (count < num) { v.gameObject.SetActive(true); count++; }
                    else { v.gameObject.SetActive(false); }
                }
            }
        }

        private void onsell(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3TIPS_SUMMON);
            BagProxy.getInstance().sendSellItems(item_data.id, 1);
        }

        private void onuse(GameObject go)
        {
            debug.Log("Use Item");
            InterfaceMgr.getInstance().close(InterfaceMgr.A3TIPS_SUMMON);
            A3_SummonProxy.getInstance().sendUseSummon((uint)item_data.id);
        }

        private void onidentify(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3TIPS_SUMMON);
            ArrayList datas = new ArrayList();
            datas.Add(1);
            //InterfaceMgr.getInstance().open(InterfaceMgr.A3_SUMMON, datas);
        }
    }
}