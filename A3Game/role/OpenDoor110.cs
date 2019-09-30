using System;
using GameFramework;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using Cross;
using MuGame.role;
using System.Collections.Generic;
using UnityEngine.UI;

namespace MuGame
{
    public class OpenDoor110 : MonoBehaviour
    {
        public int triggerTimes = 1;
        int km_count;
        int mid;
        //int lastkillall = 0;
        public int doorkill;
        Dictionary<int, int> allkill = new Dictionary<int, int>();
        Dictionary<int, Variant> phase = new Dictionary<int, Variant>();
        Dictionary<int, Dictionary<string, string>> phaseChild = new Dictionary<int, Dictionary<string, string>>();
        Variant data;
        //Transform wall0;
        //Transform wall1;
        //Transform wall2;
        //Transform wall3;
        //int level = 2;
        BoxCollider box;
        Transform idtext;
        void Start()
        {
            box = gameObject.GetComponent<BoxCollider>();
            if (box == null)
                box = gameObject.AddComponent<BoxCollider>();
            box.isTrigger = true;
            gameObject.layer = EnumLayer.LM_PT;
            //wall0 = this.gameObject.transform.parent.parent.FindChild("FX_common_door00");
            //wall1 = this.gameObject.transform.parent.parent.FindChild("FX_common_door01");
            //wall2 = this.gameObject.transform.parent.parent.FindChild("FX_common_door02");
            //wall3 = this.gameObject.transform.parent.parent.FindChild("FX_common_door03");
            data = SvrLevelConfig.instacne.get_level_data(110);
            doorkill = a3_counterpart.lvl;//副本难度
            read();


            TeamProxy.getInstance().addEventListener(PKG_NAME.S2C_LVL_PVPINFO_BOARD_RES, killNum);
        }
        Dictionary<int, int> killsome = new Dictionary<int, int>();
        void read()
        {
            killsome.Clear();
            //=================
            phase.Clear();
            phaseChild.Clear();
            for (int i = 0; i < data["diff_lvl"][doorkill]["phase"]._arr.Count; i++)
            {
                if (!phase.ContainsKey(i))
                    phase.Add(i, data["diff_lvl"][doorkill]["phase"][i]);//相应难度之下的任务
            }
            foreach (var item in phase)
            {
                Dictionary<string, string> itemvalue = new Dictionary<string, string>();

                itemvalue.Add("p", item.Value["p"]._str);
                itemvalue.Add("des", item.Value["des"]._str);
                itemvalue.Add("target", item.Value["target"]._str);
                itemvalue.Add("num", item.Value["num"]._str);

                if (!phaseChild.ContainsKey(item.Key))
                    phaseChild.Add(item.Key, itemvalue);
                if (!killsome.ContainsKey(item.Value["p"]._int))
                    killsome.Add(item.Value["p"]._int, item.Value["num"]._int);//读取相应阶段所需要的击杀总数   1开始

            }

        }
        void killNum(GameEvent e)
        {
            idtext = a3_insideui_fb.instance.GetNowTran().FindChild("info/info_killnums/mid");
            debug.Log(e.data.dump());
            km_count = e.data["km_count"];
            mid = e.data["m_id"]._int;
            if (allkill.ContainsKey(mid))
            {
                allkill[mid] = km_count;
            }
            else
            {
                allkill.Add(mid, km_count);
            }

            var tns = a3_insideui_fb.instance.GetNowTran().FindChild("info/info_killnums/Text");//队伍击杀
            if (!killsome.ContainsKey(a3_insideui_fb.instance.doors + 1))
            {
                return;
            }
            if (tns != null)
                tns.GetComponent<Text>().text =
                    ContMgr.getCont("fb_info_8", km_count.ToString()) + "/" + killsome[a3_insideui_fb.instance.doors + 1];
            if (km_count == killsome[a3_insideui_fb.instance.doors + 1])
            {
                a3_insideui_fb.instance.doors++;
                if (!killsome.ContainsKey(a3_insideui_fb.instance.doors + 1))
                {
                    return;
                }
                if (idtext != null) idtext.GetComponent<Text>().text = ContMgr.getCont("fb_info_9", phaseChild[a3_insideui_fb.instance.doors]["des"]);
                if (tns != null) tns.GetComponent<Text>().text = ContMgr.getCont("fb_info_8", 0.ToString()) + "/" + killsome[a3_insideui_fb.instance.doors + 1];
            }
            
        }
        void OnDestroy()
        {
            TeamProxy.getInstance().removeEventListener(PKG_NAME.S2C_LVL_PVPINFO_BOARD_RES, killNum);
        }
    }
    //public void OnTriggerEnter(Collider other)
    //{
    //    int idx = 0;
    //    int name = int.Parse(box.name);
    //    int kill = data["diff_lvl"][doorkill]["map"][0]["trigger"][name]["km"][0]["kmcnt"];//需要击杀的数量
    //    int monster = data["diff_lvl"][doorkill]["map"][0]["trigger"][name]["km"][0]["mid"];//相应的mid

    //    idtext = a3_insideui_fb.instance.GetNowTran().FindChild("info/info_killnums/mid");
    //    //==========================
    //    var tns = a3_insideui_fb.instance.GetNowTran().FindChild("info/info_killnums/Text");//队伍击杀
    //    if (box.name == "0" && allkill.ContainsKey(monster) && allkill[monster] >= kill)
    //    {
    //        level = 3;
    //        //wall0.gameObject.SetActive(false);
    //        a3_insideui_fb.instance.needkill -= killsome[name + 1];
    //        a3_insideui_fb.instance.doors = 1;
    //        if (idtext != null) idtext.GetComponent<Text>().text = ContMgr.getCont("fb_info_9", phaseChild[a3_insideui_fb.instance.doors]["des"]);
    //        if (tns != null) tns.GetComponent<Text>().text = ContMgr.getCont("fb_info_8", 0.ToString()) + "/" + killsome[a3_insideui_fb.instance.doors + 1];
    //        Destroy(box.gameObject);
    //    }
    //    else if (box.name == "0")
    //        level = 2;

    //    if (box.name == "1" && allkill.ContainsKey(monster) && allkill[monster] >= kill)
    //    {
    //        level = 4;
    //        a3_insideui_fb.instance.doors = 2;
    //        if (idtext != null) idtext.GetComponent<Text>().text = ContMgr.getCont("fb_info_9", phaseChild[a3_insideui_fb.instance.doors]["des"]);
    //        if (tns != null) tns.GetComponent<Text>().text = ContMgr.getCont("fb_info_8", 0.ToString()) + "/" + killsome[a3_insideui_fb.instance.doors + 1];
    //        //wall1.gameObject.SetActive(false);
    //        Destroy(box.gameObject);
    //    }
    //    else if (box.name == "1")
    //        level = 3;

    //    if (box.name == "2" && allkill.ContainsKey(monster) && allkill[monster] >= kill)
    //    {
    //        level = 5;
    //        a3_insideui_fb.instance.needkill -= killsome[name + 1];
    //        a3_insideui_fb.instance.doors = 3;
    //        if (idtext != null) idtext.GetComponent<Text>().text = ContMgr.getCont("fb_info_9", phaseChild[a3_insideui_fb.instance.doors]["des"]);
    //        if (tns != null) tns.GetComponent<Text>().text = ContMgr.getCont("fb_info_8", 0.ToString()) + "/" + killsome[a3_insideui_fb.instance.doors+1];
    //        //wall2.gameObject.SetActive(false);
    //        Destroy(box.gameObject);
    //    }
    //    else if (box.name == "2")
    //        level = 4;

    //    for (int i = 0; i < level; i++)
    //    {
    //        idx += NavmeshUtils.listARE[i];
    //    }
    //    if (idx == 0)
    //        return;
    //    SelfRole._inst.setNavLay(idx);
    //}
}
