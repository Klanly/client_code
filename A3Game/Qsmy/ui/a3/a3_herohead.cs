using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MuGame;
using Cross;
using MuGame.Qsmy.model;

namespace MuGame
{
    class a3_herohead : FloatUi
    {
        public int exp_time;

         public bool isclear;

        GameObject Buff_idfo_btn;
        GameObject Buff_info;
        GameObject item ;
        Transform con ;
        GameObject item_text ;
        Transform con_text ;
        static public a3_herohead instance;

        Image sum_hp;
        Image sum_sm;
        Image sum_cd;
        public HudunModel hudunModel = HudunModel.getInstance();
        public A3_HudunProxy HudunProxy = A3_HudunProxy.getInstance();
        public override void init()
        {
            alain();
            instance = this;
          
            sum_hp = this.getComponentByPath<Image>("info/summon/bg/hp");
            sum_sm = this.getComponentByPath<Image>("info/summon/bg/sm");
            sum_cd = this.getComponentByPath<Image>("info/summon/bg/cd");
            new BaseButton(this.transform.FindChild("info/summon/bg")).onClick = (GameObject go) => 
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SUMMON_NEW);
            };
        }

        public override void onShowed()
        {
            refresh_sumbar();
        }
      
        public uint auto_AddHudun = 0;
        public void Add_energy_auto(uint time, bool isAuto)
        {
            if (isAuto)
            {
                auto_AddHudun = time;
                CancelInvoke("autoTimeGo");
                InvokeRepeating("autoTimeGo", 0, 1);
            }
        }
        void autoTimeGo()
        {
           if (hudunModel.is_auto)
            {
                if (hudunModel.isNoAttack && hudunModel.Level>0)//没有受到攻击
                {
                    auto_AddHudun--;
                   
                    if (auto_AddHudun <= 0)
                    {
                        auto_AddHudun = 0;
                        HudunProxy.Add_energy_auto();
                        CancelInvoke("autoTimeGo");
                        Add_energy_auto(hudunModel.auto_time, hudunModel.is_auto);
                    }
                }
                else //受到攻击
                {
                    wait_attack(hudunModel.noAttackTime);
                    CancelInvoke("autoTimeGo");
                    return;
                }
            }
            else
            {
                CancelInvoke("autoTimeGo");
                return;
            }
        }

        public uint waitTime_baotu = 0;
        public void wait_attack_baotu(uint time)
        {
            waitTime_baotu = time;
            CancelInvoke("wait_Time_baotu");
            InvokeRepeating("wait_Time_baotu", 0, 1);
        }

        void wait_Time_baotu()
        {
            waitTime_baotu--;
            if (waitTime_baotu<=0)
            {
                waitTime_baotu = 0;
                CancelInvoke("wait_Time_baotu");
                FindBestoModel.getInstance().Canfly = true;
            }
        }

        public uint waitTime = 0;
        public void wait_attack(uint time ) 
        {
            waitTime = time;
            CancelInvoke("wait_Time");
            InvokeRepeating("wait_Time", 0, 1);
        }

        void wait_Time() 
        {
            //print("wait_Time:::" + waitTime);
            waitTime--;
            if (waitTime <= 0)
            {
                waitTime = 0;
                CancelInvoke("wait_Time");
                hudunModel.isNoAttack = true;
                if (hudunModel.is_auto && hudunModel.Level > 0)
                {
                    HudunProxy.Add_energy_auto();
                    Add_energy_auto(hudunModel.auto_time, hudunModel.is_auto);
                }
            }
        }

        void onHudun(GameObject go) 
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_HUDUN);
        }

        public void refresh_sumHp(int cur,int max)
        {
            sum_hp.fillAmount = ((float)cur / (float)max)/2.0f;
        }
        public void refresh_sumSm(int cur, int max)
        {
            sum_sm.fillAmount = ((float)cur / (float)max)/2.0f;
        }


        public void showbtnIcon(bool show)
        {
            if (show) {
                this.transform.FindChild("info/summon").localScale = Vector3.one;
            } else
            {
                this.transform.FindChild("info/summon").localScale = Vector3.zero ;
            }
        }

       // public uint nowsum_id = 0;

        public void refresh_sumbar()
        { 

            if (A3_SummonModel.getInstance().GetSummons().Count <= 0)
            {
                this.transform.FindChild("info/summon").gameObject.SetActive(false);
                return;
            }
            else {
                this.transform.FindChild("info/summon").gameObject.SetActive(true);
                this.transform.FindChild("info/summon/bg/icon").gameObject.SetActive(false);
                this.transform.FindChild("info/summon/bg/add").gameObject.SetActive(false);
                sum_cd.gameObject.SetActive(false);
            }

            if (A3_SummonModel.getInstance().nowShowAttackID > 0 && A3_SummonModel.getInstance().GetSummons().ContainsKey(A3_SummonModel.getInstance().nowShowAttackID))
            {
                a3_BagItemData m_AttData = A3_SummonModel.getInstance().GetSummons()[A3_SummonModel.getInstance().nowShowAttackID];
                int typeid = m_AttData.summondata.tpid;
                var x = XMLMgr.instance.GetSXML("item.item", "id==" + typeid);
                this.transform.FindChild("info/summon/bg/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_sm_ft_head_" + x.getString("icon_file"));
                this.transform.FindChild("info/summon/bg/icon").gameObject.SetActive(true);
                refresh_sumSm(m_AttData.summondata.lifespan, 100);
                //当前有出战

            }
            else if (A3_SummonModel.getInstance().lastatkID > 0 && A3_SummonModel.getInstance().GetSummons().ContainsKey(A3_SummonModel.getInstance().lastatkID))
            {
                a3_BagItemData m_AttData1 = A3_SummonModel.getInstance().GetSummons()[A3_SummonModel.getInstance().lastatkID];
                refresh_sumSm(m_AttData1.summondata.lifespan, 100);
                refresh_sumHp(0, 1);
                this.transform.FindChild("info/summon/bg/icon").gameObject.SetActive(true);
                sum_cd.gameObject.SetActive(true);
                do_sum_CD = true;
                //当前没有出战，但有上次出战记录

            }
            else
            {
                refresh_sumSm(0, 100);
                refresh_sumHp(0, 1);
                this.transform.FindChild("info/summon/bg/add").gameObject.SetActive(true);
                //当前没出战，且没有出战记录

            }                   
        }

       public bool do_sum_CD = false;
        public void  refresh_sumCd (float time)
        {
            sum_cd.fillAmount = time / 20f;
            sum_cd.transform.FindChild("time").GetComponent<Text>().text = ((int)time).ToString();
            if (time <= 0)
            {
                sum_cd.gameObject.SetActive(false);
                do_sum_CD = false;
                A3_SummonProxy.getInstance().sendChuzhan(A3_SummonModel.getInstance().lastatkID);
            }
        }
    
        void clear() 
        {
            for (int i = 0; i < con_text.childCount;i++ ) 
            {
                Destroy(con_text.GetChild (i).gameObject);
            }
        }

        private void onHpLowerSliderChange(float v)
        {
            AutoPlayModel.getInstance().NHpLower = (int)v;
        }

        private void onMpLowerSliderChange(float v)
        {
            AutoPlayModel.getInstance().NMpLower = (int)v;
        }

        void Update()
        {
            if (do_sum_CD && A3_SummonModel.getInstance().getSumCds().ContainsKey((int)A3_SummonModel.getInstance().lastatkID))
            {
                refresh_sumCd(A3_SummonModel.getInstance().getSumCds()[(int)A3_SummonModel.getInstance().lastatkID]);
            }


            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameSdkMgr.record_quit();
            }
        }
    }
}
