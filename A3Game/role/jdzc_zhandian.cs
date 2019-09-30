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
using DG.Tweening;

namespace MuGame
{


   public  enum flag_type {
        RED = 1,
        BULE = 2,
        GRAY = 3
    }
    public class flag_info
    {
        public int flag_idx;
        public int time;
        public int owner_side;
        public Vector3 flag_pos;
        public flag_type type = flag_type.GRAY;
    }
    public class killinfo {
        public string name_k;
        public string name_d;
        public int cid_k;
        public int cid_d;
        public int carr_k;
        public int carr_d;

    }

    public class jdzc_zhandian : MonoBehaviour
    {


        //1红色 2 蓝色
        public int triggerTimes = 1;
        //GameObject point1;
        //GameObject point2;
        //GameObject point3;

        int owner;
        int score_self = 0;
        int score_other = 0;
        Text score_blue_text;
        Text score_red_text;

        Transform kill_info_con;

        Image Bar_bule;
        Image Bar_red;

        public static  Dictionary<int, flag_info> flag_list = new Dictionary<int, flag_info>();
        change_judian cj;

        List<killinfo> kill_list = new List<killinfo>();
        int range = 0;

        Text MyRecord;

        void Start()
        {
            PlayerModel.getInstance().inSpost = true;
            a3_sportsProxy.getInstance().getTeam_info();
            a3_sportsProxy.getInstance().addEventListener(a3_sportsProxy.EVENT_INFO, OnInfo);
            a3_sportsProxy.getInstance().addEventListener(a3_sportsProxy.EVENT_OWNER_INFO, OnOwner);
            a3_sportsProxy.getInstance().addEventListener(a3_sportsProxy.EVENT_KILL_INFO, Onkill_info);
            a3_sportsProxy.getInstance().addEventListener(a3_sportsProxy.EVENT_KILL_SCE, Onkill);
            a3_sportsProxy.getInstance().addEventListener(a3_sportsProxy.EVENT_HELP_KILL, OnhelpKill);



            SXML Xml = XMLMgr.instance.GetSXML("pointarena.initpoint");
            range = Xml.getInt("range");
            flag_list.Clear();

            for (int i = 1;i <= this.transform.childCount;i++) {

                if (this.transform.GetChild(i - 1).gameObject.name == "judian" + i)
                {
                    flag_info flag = new flag_info();
                    flag.flag_idx = i;
                    flag.flag_pos = new Vector3(this.transform.GetChild(i - 1).FindChild("zc_judian").position.x* GameConstant .PIXEL_TRANS_UNITYPOS,
                        this.transform.GetChild(i - 1).FindChild("zc_judian").position.y * GameConstant.PIXEL_TRANS_UNITYPOS,
                        this.transform.GetChild(i - 1).FindChild("zc_judian").position.z * GameConstant.PIXEL_TRANS_UNITYPOS
                        );  ;
                    flag_list[flag.flag_idx] = flag;

                }
            }
            cj = this.gameObject.GetComponent<change_judian>();
        }

        

        void OnOwner(GameEvent e) {
            Variant data = e.data;
            a3_sportsModel .getInstance ().GameInfo.Clear();
            debug.Log("收到" + data.dump());
            foreach (Variant one in data["info"]._arr )
            {
                if (one.ContainsKey ("cid"))
                {
                    if (one ["cid"] == PlayerModel.getInstance ().cid )
                    {
                        owner = one["lvlsideid"];
                        PlayerModel.getInstance().lvlsideid = (uint)owner;
                    }
                }
                info_teamPlayer p = new info_teamPlayer();
                p.carr = one["carr"];
                p.cid = one["cid"];
                p.name = one["name"];
                p.zhuan = one["zhuan"];
                p.lvl = one["lvl"];
                p.lvlsideid = one["lvlsideid"];
                a3_sportsModel.getInstance().GameInfo[p.cid] = p;
            }
             a3_sportsProxy.getInstance().removeEventListener(a3_sportsProxy.EVENT_OWNER_INFO, OnOwner);
        }



        void Onkill(GameEvent e) {
            Variant data = e.data;
            if (data.ContainsKey("sideid")) {
                if (data["sideid"] == owner)
                {
                    if (data.ContainsKey("score"))
                    {
                        score_self = data["score"];
                    }
                }
                else
                {
                    if (data.ContainsKey("score"))
                    {
                        score_other = data["score"];
                    }
                }
            }
            setscore();
        }

        void changeFlag_type( int tag, int type ) {
            cj.change(tag, type);
        }


        void OnInfo(GameEvent e) {
            Variant data = e.data;
            int idx = data["flag_idx"];
            
            if (data.ContainsKey ("sideid"))
            {
                if (data["sideid"] == owner)
                {
                    if (data.ContainsKey("score"))
                    {
                        score_self = data["score"];
                    }
                }
                else {
                    if (data.ContainsKey("score"))
                    {
                        score_other = data["score"];
                    }
                }
            }
            int time = data["time"];
            if (flag_list .ContainsKey (idx)) {
                flag_list[idx].time = time;
                if (data.ContainsKey("owner_side"))
                {
                    flag_list[idx].owner_side = data["owner_side"];
                    switch (flag_list[idx].owner_side) {
                        case 0:
                            if (flag_list[idx].type != flag_type.GRAY ) {
                                first = true;
                                changeFlag_type(idx, 0);
                                flag_list[idx].type = flag_type.GRAY;
                            }
                            break;
                        case 1:
                            if (flag_list[idx].type != flag_type.RED)
                            {
                                first = true;
                                changeFlag_type(idx, 2);
                                flag_list[idx].type = flag_type.RED;
                            }
                            break;
                        case 2:
                            if (flag_list[idx].type != flag_type.BULE)
                            {
                                first = true;
                                changeFlag_type(idx, 1);
                                flag_list[idx].type = flag_type.BULE;
                            }
                            break;
                    }
                }
            }
            setscore();
        }



        void Set_kill_new() {
            if (kill_info_con == null)
            {
                kill_info_con = a3_insideui_fb.instance.GetNowTran().FindChild("kill_new");
            }


            int ss = 0;
           // viewhight = change_att[0].Count * itemsize;
            Tweener tween1 = DOTween.To(() => ss, (float s) => {
                ss = (int)s;
                kill_info_con.gameObject.SetActive(true);
                kill_info_con.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal , ss);
                //bg.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ss);
            }, (float)320f, 0.5f);
            tween1.OnStart(delegate ()
            {
                kill_info_con.FindChild("info").gameObject.SetActive(false);
                kill_info_con.FindChild("info/winner/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_hero_" + kill_list[0].carr_k);
                kill_info_con.FindChild("info/winner/name").GetComponent<Text>().text = kill_list[0].name_k;
                kill_info_con.FindChild("info/loser/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_hero_" + kill_list[0].carr_d);
                kill_info_con.FindChild("info/loser/name").GetComponent<Text>().text = kill_list[0].name_d;
            }
            );
            kill_info_con.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 20f);
            tween1.OnComplete(delegate ()
            {
                kill_info_con.FindChild("info").gameObject.SetActive(true);
                wite();
            });

        }


        void OnhelpKill(GameEvent e)
        {
            a3_sportsModel.getInstance().helpkill_count++;
            a3_insideui_fb.instance.JDZC_setMyRecord();
        }
        void wite() {
            if (kill_info_con == null)
            {
                kill_info_con = a3_insideui_fb.instance.GetNowTran().FindChild("kill_new");
            }
            int ss = 0;
            Tweener tween1 = DOTween.To(() => ss, (float s) => {
            }, (float)0,3f);
            tween1.OnComplete(delegate ()
            {
                kill_info_con.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
                kill_info_con.gameObject.SetActive(false); 
                kill_list.Remove(kill_list[0]);
                show_kill = true;
            });
        }


        bool show_kill = true;
        void Update() {

            if (kill_list.Count > 0) {

                if (show_kill) {
                    Set_kill_new();
                    show_kill = false;
                }
            }

            if (SelfRole._inst.m_curModel == null) return;

            Vector3 mpos = new Vector3 (SelfRole._inst.m_curModel.position.x*GameConstant.PIXEL_TRANS_UNITYPOS, SelfRole._inst.m_curModel.position.y * GameConstant.PIXEL_TRANS_UNITYPOS, SelfRole._inst.m_curModel.position.z * GameConstant.PIXEL_TRANS_UNITYPOS);

            if (Vector3.Distance(mpos, flag_list[1].flag_pos) <= range)
            {
           
                if ((owner == 1 && flag_list[1].time <= -11  ) || (owner == 2 && flag_list[1].time >= 11 ))
                {
                    a3_insideui_fb.instance?.GetNowTran().FindChild("cd").gameObject.SetActive(false);
                    first = true;
                }
                else {
                    a3_insideui_fb.instance?.GetNowTran().FindChild("cd").gameObject.SetActive(true);
                    setBar(flag_list[1].time);
                    first = false;
                }
            }
            else if (Vector3.Distance(mpos, flag_list[2].flag_pos) <= range) {
                if ((owner == 1 && flag_list[2].time <= -11 ) || (owner == 2 && flag_list[2].time >= 11 ))
                {
                    a3_insideui_fb.instance?.GetNowTran().FindChild("cd").gameObject.SetActive(false);
                    first = true;
                }
                else
                {
                    a3_insideui_fb.instance?.GetNowTran().FindChild("cd").gameObject.SetActive(true);
                    setBar(flag_list[2].time);
                    first = false;
                }
            }
            else if (Vector3.Distance(mpos, flag_list[3].flag_pos) <= range) {
                if ((owner == 1 && flag_list[3].time <= -11) || (owner == 2 && flag_list[3].time >= 11 ))
                {
                    a3_insideui_fb.instance?.GetNowTran().FindChild("cd").gameObject.SetActive(false);
                    first = true;
                }
                else
                {
                    a3_insideui_fb.instance?.GetNowTran().FindChild("cd").gameObject.SetActive(true);
                    setBar(flag_list[3].time);
                    first = false;
                }
            }
            else { a3_insideui_fb.instance?.GetNowTran().FindChild("cd").gameObject.SetActive(false); first = true; }
        }


        void Onkill_info(GameEvent e) {
            Variant data = e.data;
            killinfo k = new killinfo();
            k.name_k = data["name_k"];
            k.name_d = data["name_d"];
            k.cid_k = data["cid_k"];
            k.cid_d = data["cid_d"];
            k.carr_k = data["carr_k"];
            k.carr_d = data["carr_d"];
            kill_list.Add(k);

            if (data["cid_k"] == PlayerModel .getInstance ().cid ) { a3_sportsModel.getInstance().kill_count++; }
            if (data["cid_d"] == PlayerModel.getInstance().cid) { a3_sportsModel.getInstance().die_count++; }
            a3_insideui_fb.instance.JDZC_setMyRecord();
        }

        bool first = true;


        
        //1 红色 负值
        //2 蓝色 正值

        void setBar( int value ) {
            if (Bar_bule == null) {
                Bar_bule = a3_insideui_fb.instance.GetNowTran().FindChild("cd/barbg/weteambar").GetComponent<Image>();
            }
            if (Bar_red == null) {
                Bar_red = a3_insideui_fb.instance.GetNowTran().FindChild("cd/barbg/enemybar").GetComponent<Image>();
            }

  
            if (owner == 1)//我是红方
            {
                if (value <= 0)
                {
                    Bar_red.gameObject.SetActive(true);
                    Bar_bule.gameObject.SetActive(false);
                    if (first)
                    {
                        float v = (float)Mathf.Abs(value);
                        Bar_red.fillAmount = (float)(v / 10);
      
                    }
                    else
                    {
                        float v = (float)Mathf.Abs(value);
                        Bar_red.fillAmount = Mathf.Lerp(Bar_red.fillAmount, (float)(v / 10), Time.deltaTime);
                    }
                }
                else {
                    Bar_red.gameObject.SetActive(false);
                    Bar_bule.gameObject.SetActive(true);
                    if (first)
                        Bar_bule.fillAmount = (float)((float)Mathf.Abs(value) / 10);
                    else
                        Bar_bule.fillAmount = Mathf.Lerp(Bar_bule.fillAmount, (float)((float)Mathf.Abs(value) / 10), Time.deltaTime);
                }
            } else if (owner == 2)//我是蓝方
            {
                if (value < 0)
                {
                    Bar_bule.gameObject.SetActive(false);
                    Bar_red.gameObject.SetActive(true);
                    if (first)
                    {
                        float v = (float)Mathf.Abs(value);
                        Bar_red.fillAmount = (float)(v / 10);
                    }
                    else
                    {
                        float v = (float)Mathf.Abs(value);
                        Bar_red.fillAmount = Mathf.Lerp(Bar_red.fillAmount, (float)(v / 10), Time.deltaTime);
                    }

                }
                else
                {
                    Bar_bule.gameObject.SetActive(true);
                    Bar_red.gameObject.SetActive(false);
                    if (first)
                        Bar_bule.fillAmount = (float)((float)Mathf.Abs(value) / 10);
                    else
                        Bar_bule.fillAmount = Mathf.Lerp(Bar_bule.fillAmount, (float)((float)Mathf.Abs(value) / 10), Time.deltaTime);
                }
            }

        }

        void setscore() {
            if (a3_insideui_fb.instance == null) return;
            if (score_blue_text == null) {
                score_blue_text = a3_insideui_fb.instance.GetNowTran().FindChild("gameinfo/blueteam").GetComponent<Text>();
            }
            if (score_red_text == null) {
                score_red_text= a3_insideui_fb.instance.GetNowTran().FindChild("gameinfo/redteam").GetComponent<Text>();
            }

            if (owner == 2 || PlayerModel.getInstance().lvlsideid == 2)
            {
                score_blue_text.text = score_self.ToString();
                score_red_text.text = score_other.ToString();
            }
            else if (owner == 1 || PlayerModel.getInstance().lvlsideid == 1) 
            {
                score_blue_text.text = score_other.ToString();
                score_red_text.text = score_self.ToString();
            }
        }

        void OnDestroy()
        {
            a3_sportsProxy.getInstance().removeEventListener(a3_sportsProxy.EVENT_INFO, OnInfo);
            a3_sportsProxy.getInstance().removeEventListener(a3_sportsProxy.EVENT_KILL_SCE, Onkill);
            a3_sportsProxy.getInstance().removeEventListener(a3_sportsProxy.EVENT_KILL_INFO, Onkill_info);
            a3_sportsProxy.getInstance().removeEventListener(a3_sportsProxy.EVENT_HELP_KILL, OnhelpKill);
            owner = 0;
            PlayerModel.getInstance().lvlsideid = 0;
            PlayerModel.getInstance().inSpost = false;
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_LITEMINIBASEMAP1);
            a3_sportsModel.getInstance().kill_count = 0;
            a3_sportsModel.getInstance().die_count = 0;
            a3_sportsModel.getInstance().helpkill_count = 0;
            flag_list.Clear();
        }
    }
}
