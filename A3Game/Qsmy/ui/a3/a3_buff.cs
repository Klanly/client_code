using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using GameFramework;

namespace MuGame
{
    class a3_buff: FloatUi
    {
        static public a3_buff instance;
        public GameObject buff_tp;
        public GameObject buff_info;
        public Transform buff_num;
        public Transform contain;
        public GameObject pre;
        private bool legion_bf;
        private bool pet_bf;
        public int num;
        public long endCD_exp;
        
        public Transform exp_pos;
        public string name_exp;
        public bool timego;
        private Transform bless_pos;
        private string name_bless;
        private long endCD_bless;

        private uint[] skill_id=new uint[11];

        Transform fuwen_pos;
        string name_fuwen;
        long endCD_fuwen;
        public bool dou_exp;
        public bool bless_ad;
       Transform[] skill_pos=new Transform[11];
       string[] name_skill=new string[11];
       long[] endCD_skill=new long[11];

        //public   List<uint> skillList = new List<uint>();
        public override void init()
        {
            instance = this;
            getGameObjectByPath("close").SetActive(false);
            buff_tp = transform.FindChild("view_buff").gameObject;
            buff_info = transform.FindChild("buff_info").gameObject;
            buff_num = buff_tp.transform.FindChild("con/text");
            contain = buff_info.transform.FindChild("view/con");

            pre = buff_info.transform.FindChild("view/temp").gameObject;
            buff_info.SetActive(false);
            buff_tp.SetActive(false);
            new BaseButton(buff_tp.transform).onClick = (GameObject go) =>
            {
                if (buff_info.activeInHierarchy == false)
                {
                    getGameObjectByPath("close").SetActive(true);
                    buff_info.SetActive(true);
                }
                else
                {
                    getGameObjectByPath("close").SetActive(false);
                    buff_info.SetActive(false);
                }
            };
            new BaseButton(getTransformByPath("close")).onClick = (GameObject go) =>
              {
                  if (buff_info.activeInHierarchy)
                  {
                      getGameObjectByPath("close").SetActive(false);
                      buff_info.SetActive(false);
                  }
              };
            legion_pet_buff();
        }

        private void reshbuff(GameEvent obj)
        {
            resh_buff();
        }
        #region
        private void get_pettime(GameEvent e)
        {
            pet_bf = true;
            num += 1;
            resh_buff();
        }
        void  changePet(GameEvent e)
        {
            pet_bf = true;
            resh_buff();
        }
        private void closePet(GameEvent e)
        {
            pet_bf = false;
            num -= 1;
            resh_buff();
        }

        private void OpenPet(GameEvent e)
        {
            pet_bf = true;
            num += 1;
            resh_buff();
        }

        private void Deleteclan(GameEvent e)
        {
            if (PlayerModel.getInstance().clan_buff_lvl <= 0) return;
            legion_bf = false;
            num -= 1;
            resh_buff();
        }

        private void Join(GameEvent e)
        {
            if (PlayerModel.getInstance().clan_buff_lvl <= 0) return;
            legion_bf = true;
            num += 1;
            resh_buff();
        }

        private void Quit(GameEvent e)
        {
            if (PlayerModel.getInstance().clan_buff_lvl <= 0) return;
            legion_bf = false;
            num -= 1;
            resh_buff();
        }
        #endregion//宠物和军团的监听事件
        public void Quited()
        {
            if (PlayerModel.getInstance().clan_buff_lvl <= 0) return;
            legion_bf = false;
            num -= 1;
            resh_buff();
        }

        void legion_pet_buff()
        {
            if (PlayerModel.getInstance().clanid != 0&& PlayerModel.getInstance().clan_buff_lvl > 0)
            {
              
                legion_bf = true;
                num += 1;
            }
          
            if (PlayerModel.getInstance().havePet == true && PlayerModel.getInstance().last_time != 0)
            {
                pet_bf = true;
                num += 1;
            }
        }

        public void legion_buf()
        {
            legion_bf = true;
            num += 1;
            resh_buff();
        }
        override public void onShowed()
        {
            // 军团buff另加。监听事件EVENT_REMOVE
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_QUIT, Quit);
            // A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_REMOVE, Quit);
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_CREATE, Join);
            A3_LegionProxy.getInstance().addEventListener(A3_LegionProxy.EVENT_DELETECLAN, Deleteclan);
            //宠物buff另加。监听事件
            //A3_PetProxy.getInstance().addEventListener(A3_PetProxy.EVENT_GET_PET, OpenPet);//得到宠物
            A3_PetProxy.getInstance().addEventListener(A3_PetProxy.CHANGE_PET, changePet);//更换宠物
            A3_PetProxy.getInstance().addEventListener(A3_PetProxy.EVENT_HAVE_PET, closePet);//饲料到期
            A3_PetProxy.getInstance().addEventListener(A3_PetProxy.EVENT_GET_LAST_TIME, get_pettime);//购买饲料

            BattleProxy.getInstance().addEventListener(A3_PetProxy.EVENT_GET_LAST_TIME, reshbuff);
            //addProxyListener(PKG_NAME.S2C_MAP_CHANGE, buff);

            resh_buff();
        }

        override public void onClosed()
        {
            // 军团buff另加。监听事件EVENT_REMOVE
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_QUIT, Quit);
            // A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_REMOVE, Quit);
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_CREATE, Join);
            A3_LegionProxy.getInstance().removeEventListener(A3_LegionProxy.EVENT_DELETECLAN, Deleteclan);
            //宠物buff另加。监听事件
            //A3_PetProxy.getInstance().removeEventListener(A3_PetProxy.EVENT_GET_PET, OpenPet);//得到宠物
            A3_PetProxy.getInstance().removeEventListener(A3_PetProxy.CHANGE_PET, changePet);//更换宠物
            A3_PetProxy.getInstance().removeEventListener(A3_PetProxy.EVENT_HAVE_PET, closePet);//饲料到期
            A3_PetProxy.getInstance().removeEventListener(A3_PetProxy.EVENT_GET_LAST_TIME, get_pettime);//购买饲料

            BattleProxy.getInstance().removeEventListener(A3_PetProxy.EVENT_GET_LAST_TIME, reshbuff);        }

        public void resh_buff()
        {
            if (SelfRole._inst?.isDead ?? false)
            {
                return;
            }
            Clear_con();
            var buff = A3_BuffModel.getInstance().BuffCd;
            Set_cancel();
            if (num < 0) num = 0;
            if (buff.Count == 0 && num == 0 )
            {
                buff_tp.SetActive(false);
                buff_info.SetActive(false);
                getGameObjectByPath( "close" ).SetActive( false );
                return;
            }
            else
            {
                buff_tp.SetActive(true);
                // buff_info.SetActive(false);
            }

            //if(A3_BuffModel.getInstance().BuffCd.Count>1)
           

            buff_num.GetComponent<Text>().text = "buff * " + (buff.Count+num);
            contain.GetComponent<RectTransform>().sizeDelta = new Vector2(pre.GetComponent<RectTransform>().sizeDelta.x, pre.GetComponent<RectTransform>().sizeDelta.y * (buff.Count + num));
            foreach (uint buffId in A3_BuffModel.getInstance().BuffCd.Keys)
            {
                var go = GameObject.Instantiate(pre) as GameObject;
                go.transform.SetParent(contain);
                go.transform.localScale = Vector3.one;
                go.SetActive(true);
             
                Set_Line(go.transform, buff[buffId]);
            }
            //宠物和军团的buff重新加
            if (num != 0)
            {
                if (legion_bf == true&& PlayerModel.getInstance().clan_buff_lvl>0)
                {
                    var go = GameObject.Instantiate(pre) as GameObject;
                    go.transform.SetParent(contain);
                    go.transform.localScale = Vector3.one;
                    go.SetActive(true);
                    go.transform.FindChild("item_text").GetComponent<Text>().text = ContMgr.getCont("a3_buff");
                   // Debug.LogError(PlayerModel.getInstance().clan_buff_lvl);
                   // if (PlayerModel.getInstance().clan_buff_lvl == 0 ) PlayerModel.getInstance().clan_buff_lvl = 1;
                    var dv_self = XMLMgr.instance.GetSXML("clan.clan_buff", "lvl==" + PlayerModel.getInstance().clan_buff_lvl);

                    go.transform.FindChild("Text").GetComponent<Text>().text = dv_self.getString("buff_dc");

                }
                if (pet_bf == true)
                {
                   
                    var go = GameObject.Instantiate(pre) as GameObject;
                    go.transform.SetParent(contain);
                    go.transform.localScale = Vector3.one;
                    go.SetActive(true);
                    go.transform.FindChild("item_text").GetComponent<Text>().text = ContMgr.getCont("a3_buff_pet");
                    if (A3_PetModel.curPetid == 0) A3_PetModel.curPetid = 2;
                     var petXML = XMLMgr.instance.GetSXML("newpet.pet", "id==" + A3_PetModel.curPetid);                  
                    go.transform.FindChild("Text").GetComponent<Text>().text = petXML.getString("buff_dc");
                }
           }

           

        }

        private void Set_cancel()
        {
            for (int i = 0; i < 10; i++)
            {
                if (!A3_BuffModel.getInstance().BuffCd.ContainsKey(skill_id[i]))
                {
                 
                    if (IsInvoking("do_skillCD_" + i))
                        CancelInvoke("do_skillCD_" + i);
                }               
                  
            }
        }

        private void Clear_con()
        {
            if (contain!=null&&contain.childCount == 0) return;
            else
            {
                for (int i = 0; i < contain.childCount; i++)
                {
                    Destroy(contain.GetChild(i).gameObject);
                }
            }
        }
        uint oldtime = 0;
        private void Set_Line(Transform go, BuffInfo v)
        {
            SXML xml = XMLMgr.instance?.GetSXML("skill.state", "id==" + v.id);
           // Debug.LogError(v.id);
            if (v.id == 10001)//经验卷buff
            {
                dou_exp = true;
                if (a3_insideui_fb.instance)
                    a3_insideui_fb.instance.Using_jc();
                exp_pos = go.transform.FindChild("item_text");
                name_exp = xml.getString("name");

                if (IsInvoking("do_expCD"))
                {
                    // print("v.end_time：" + v.end_time + " v.start_time:" + v.start_time);
                    //exp_pos = go.transform.FindChild("item_text");
                    if (v.end_time != oldtime)
                    {
                        endCD_exp = (v.end_time - v.start_time) / 1000;
                        oldtime = v.end_time;
                    }

                    CancelInvoke("do_expCD");
                }
                else
                {
                    endCD_exp = (v.end_time - v.start_time) / 1000;
                }
                InvokeRepeating("do_expCD", 0, 1);


                // go.transform.FindChild("item_text").GetComponent<Text>().text = xml.getString("name");
                go.transform.FindChild("Text").GetComponent<Text>().text = xml.getString("desc");
            }
            else if (v.id == 99996 || v.id == 99997 || v.id == 99998 || v.id == 99999)//组队
            {

                go.transform.FindChild("item_text").GetComponent<Text>().text = xml.getString("name");
                go.transform.FindChild("Text").GetComponent<Text>().text = xml.getString("desc");
            }
            else if (v.id == 10000)//鼓舞
            {
                bless_pos = go.transform.FindChild("item_text");
                name_bless = xml.getString("name");
                bless_ad= true;
                if (a3_insideui_fb.instance)
                    a3_insideui_fb.instance.Using_jc();
                if (IsInvoking("do_blessCD"))
                {
                    bless_pos = go.transform.FindChild("item_text");
                    CancelInvoke("do_blessCD");
                }
                else
                {
                    endCD_bless = (v.end_time - v.start_time) / 1000;
                }
                InvokeRepeating("do_blessCD", 0, 1);


                // go.transform.FindChild("item_text").GetComponent<Text>().text = xml.getString("name"); 
                go.transform.FindChild("Text").GetComponent<Text>().text = xml.getString("desc");
            }
            else if ((v.id > 0 && v.id < 203) || (v.id > 3000 && v.id < 3101))//符文
            {
                fuwen_pos = go.transform.FindChild("item_text");
                name_fuwen = xml.getString("name");

                if (IsInvoking("do_fuwenCD"))
                {
                    fuwen_pos = go.transform.FindChild("item_text");
                    CancelInvoke("do_fuwenCD");
                }
                else
                {
                    endCD_fuwen = (v.end_time - v.start_time) / 1000;
                }
                InvokeRepeating("do_fuwenCD", 0, 1);

                //go.transform.FindChild("item_text").GetComponent<Text>().text = xml.getString("name");
                go.transform.FindChild("Text").GetComponent<Text>().text = xml.getString("desc");
            }
            else if (v.id >= 300 && v.id < 6050)//技能
            {
                SXML xml2 = XMLMgr.instance?.GetSXML("skill.skill", "id==" + xml.getInt("skill_id"));

                List<SXML> xml3 = xml2.GetNodeList("skill_att");
                for (int i = 0; i < xml3.Count; i++)
                {

                    SXML xml4 = xml3[i].GetNode("sres");/*, "tar_state=="+v.id);*/
                    if (xml4 == null)
                    {
                        xml4 = xml3[i].GetNode("tres");
                        if( xml4.getInt("tar_state") == v.id)
                        {
                            go.transform.FindChild("Text").GetComponent<Text>().text = xml3[i].getString("descr3");
                            break;
                        }
                         
                    }
                    else if(xml4.getInt("tar_state") == v.id)
                    {
                        go.transform.FindChild("Text").GetComponent<Text>().text = xml3[i].getString("descr2");
                        break;
                    }
                }
              
                //if (SelfRole._inst.isDead)
                //{
                //    CancelInvoke("do_skillCD_0");
                //    CancelInvoke("do_skillCD_1");
                //    CancelInvoke("do_skillCD_2");
                //    CancelInvoke("do_skillCD_3");
                //    CancelInvoke("do_skillCD_4");
                //    CancelInvoke("do_skillCD_5");
                //    CancelInvoke("do_skillCD_6");
                //    CancelInvoke("do_skillCD_7");
                //    CancelInvoke("do_skillCD_8");
                //    return;

                //}
                
                // go.transform.FindChild("Text").GetComponent<Text>().text = xml2.getString("descr1");

                //每个技能的倒计时。暂时没想到别的方法。
                #region
                switch (xml.getInt("skill_id"))
                {
                    case 5008:
                        skill_id[0] = v.id;
                        skill_pos[0] = go.transform.FindChild("item_text");
                        name_skill[0] = xml.getString("name");
                       
                        if (IsInvoking("do_skillCD_0"))
                        {
                            skill_pos[0] = go.transform.FindChild("item_text");
                            CancelInvoke("do_skillCD_0");
                           
                        }
                        else
                        {
                            endCD_skill[0] = (v.end_time - v.start_time) / 1000;
                        }
                        InvokeRepeating("do_skillCD_0", 0, 1);
                        break;
                    case 2005:
                        skill_id[1] = v.id;
                        skill_pos[1] = go.transform.FindChild("item_text");
                        name_skill[1] = xml.getString("name");
                       
                        if (IsInvoking("do_skillCD_1"))
                        {
                       
                            skill_pos[1] = go.transform.FindChild("item_text");
                            CancelInvoke("do_skillCD_1");
                          
                        }
                        else
                        {
                         
                            endCD_skill[1] = (v.end_time - v.start_time) / 1000;
                        }
                        InvokeRepeating("do_skillCD_1", 0, 1);
                        break;
                    case 2010:
                        skill_id[2] = v.id;
                        skill_pos[2] = go.transform.FindChild("item_text");
                        name_skill[2] = xml.getString("name");
                       
                        if (IsInvoking("do_skillCD_2"))
                        {
                            skill_pos[2] = go.transform.FindChild("item_text");
                            CancelInvoke("do_skillCD_2");
                          
                        }
                        else
                        {
                            endCD_skill[2] = (v.end_time - v.start_time) / 1000;
                        }
                        InvokeRepeating("do_skillCD_2", 0, 1);
                        break;
                    case 3008:
                        skill_id[3] = v.id;
                        skill_pos[3] = go.transform.FindChild("item_text");
                        name_skill[3] = xml.getString("name");
                     
                        if (IsInvoking("do_skillCD_3"))
                        {
                            skill_pos[3] = go.transform.FindChild("item_text");
                            CancelInvoke("do_skillCD_3");
                          
                        }
                        else
                        {
                            endCD_skill[3] = (v.end_time - v.start_time) / 1000;
                        }
                        InvokeRepeating("do_skillCD_3", 0, 1);
                        break;
                    case 3009:
                        skill_id[9] = v.id;
                        skill_pos[9] = go.transform.FindChild("item_text");
                        name_skill[9] = xml.getString("name");

                        if (IsInvoking("do_skillCD_9"))
                        {
                            skill_pos[3] = go.transform.FindChild("item_text");
                            CancelInvoke("do_skillCD_9");

                        }
                        else
                        {
                            endCD_skill[9] = (v.end_time - v.start_time) / 1000;
                        }
                        InvokeRepeating("do_skillCD_9", 0, 1);
                        break;
                    case 3010:
                        skill_id[4] = v.id;
                        skill_pos[4] = go.transform.FindChild("item_text");
                        name_skill[4] = xml.getString("name");
                       
                        if (IsInvoking("do_skillCD_4"))
                        {
                            skill_pos[4] = go.transform.FindChild("item_text");
                            CancelInvoke("do_skillCD_4");
                         
                        }
                        else
                        {
                            endCD_skill[4] = (v.end_time - v.start_time) / 1000;
                        }
                        InvokeRepeating("do_skillCD_4", 0, 1);

                        break;
                    case 5005:
                        skill_id[5] = v.id;
                        skill_pos[5] = go.transform.FindChild("item_text");
                        name_skill[5] = xml.getString("name");
                       
                        if (IsInvoking("do_skillCD_5"))
                        {
                            skill_pos[5] = go.transform.FindChild("item_text");
                            CancelInvoke("do_skillCD_5");
                           
                        }
                        else
                        {
                            endCD_skill[5] = (v.end_time - v.start_time) / 1000;
                        }
                        InvokeRepeating("do_skillCD_5", 0, 1);

                        break;
                    case 5009:
                        skill_id[6] = v.id;
                        skill_pos[6] = go.transform.FindChild("item_text");
                        name_skill[6] = xml.getString("name");
                       
                        if (IsInvoking("do_skillCD_6"))
                        {
                            skill_pos[6] = go.transform.FindChild("item_text");
                            CancelInvoke("do_skillCD_6");
                          
                        }
                        else
                        {
                            endCD_skill[6] = (v.end_time - v.start_time) / 1000;
                        }
                        InvokeRepeating("do_skillCD_6", 0, 1);

                        break;
                    case 5010:
                        skill_id[7] = v.id;
                        skill_pos[7] = go.transform.FindChild("item_text");
                        name_skill[7] = xml.getString("name");
                      
                        if (IsInvoking("do_skillCD_7"))
                        {
                            skill_pos[7] = go.transform.FindChild("item_text");
                            CancelInvoke("do_skillCD_7");
                      
                        }
                        else
                        {
                            endCD_skill[7] = (v.end_time - v.start_time) / 1000;
                        }
                        InvokeRepeating("do_skillCD_7", 0, 1);
                        break;
                    case 2008:
                        skill_id[8] = v.id;
                        skill_pos[8] = go.transform.FindChild("item_text");
                        name_skill[8] = xml.getString("name");
                       
                        if (IsInvoking("do_skillCD_8"))
                        {
                            skill_pos[8] = go.transform.FindChild("item_text");
                            CancelInvoke("do_skillCD_8");
                        
                        }
                        else
                        {
                            endCD_skill[8] = (v.end_time - v.start_time) / 1000;
                        }
                        InvokeRepeating("do_skillCD_8", 0, 1);
                        break;
                    case 6053:
                        skill_id[10] = v.id;
                        skill_pos[10] = go.transform.FindChild("item_text");
                        name_skill[10] = xml.getString("name");
                       
                        if (IsInvoking("do_skillCD_10"))
                        {
                            skill_pos[10] = go.transform.FindChild("item_text");
                            CancelInvoke("do_skillCD_10");

                        }
                        else
                        {
                            endCD_skill[10] = (v.end_time - v.start_time) / 1000;
                        }
                        InvokeRepeating("do_skillCD_10", 0, 1);

                        break;
                    default:
                        go.transform.FindChild("item_text").GetComponent<Text>().text = ContMgr.getCont("a3_buff_ewai");
                        go.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_buff_ewai1");
                        break;
                }
                #endregion
            }
            else
            {
                go.transform.FindChild("item_text").GetComponent<Text>().text = ContMgr.getCont("a3_buff_ewai");
                go.transform.FindChild("Text").GetComponent<Text>().text = ContMgr.getCont("a3_buff_ewai1");
            }

        }
#region
        public void do_skillCD_0()
        {                         
                endCD_skill[0]--;
                if (endCD_skill[0] <= 0)
                {
                    endCD_skill[0] = 0;
                    CancelInvoke("do_skillCD_0");
                A3_BuffModel.getInstance().RemoveBuff(skill_id[0]);
                return;
              
            }
          
                string cd = string.Format("{0:D2}:{1:D2}", /*endCD_skill[0] / 3600,*/ endCD_skill[0] % 3600 / 60, endCD_skill[0] % 60);
            if(skill_pos[0])
                skill_pos[0].GetComponent<Text>().text = name_skill[0] + "(" + cd + ")";           
        }
        public void do_skillCD_1()
        {
            endCD_skill[1]--;
            if (endCD_skill[1] <= 0)
            {
                endCD_skill[1] = 0;
                 CancelInvoke("do_skillCD_1");
                A3_BuffModel.getInstance().RemoveBuff(skill_id[1]);
                return;
              
            }
            string cd = string.Format("{0:D2}:{1:D2}", /*endCD_skill[1] / 3600,*/ endCD_skill[1] % 3600 / 60, endCD_skill[1] % 60);
            if (skill_pos[1])
                skill_pos[1].GetComponent<Text>().text = name_skill[1] + "(" + cd + ")";
        }
        public void do_skillCD_2()
        {
            endCD_skill[2]--;
            if (endCD_skill[2] <= 0)
            {
                endCD_skill[2] = 0;
                CancelInvoke("do_skillCD_2");
                A3_BuffModel.getInstance().RemoveBuff(skill_id[2]);
                return;
               
            }
            string cd = string.Format("{0:D2}:{1:D2}", /*endCD_skill[2] / 3600,*/ endCD_skill[2] % 3600 / 60, endCD_skill[2] % 60);
            if (skill_pos[2])
                skill_pos[2].GetComponent<Text>().text = name_skill[2] + "(" + cd + ")";
        }
        public void do_skillCD_3()
        {
            endCD_skill[3]--;
            if (endCD_skill[3] <= 0)
            {
                endCD_skill[3] = 0;
                CancelInvoke("do_skillCD_3");
                A3_BuffModel.getInstance().RemoveBuff(skill_id[3]);
                return;
              
            }
            string cd = string.Format("{0:D2}:{1:D2}", /*endCD_skill[3] / 3600,*/ endCD_skill[3] % 3600 / 60, endCD_skill[3] % 60);
            if (skill_pos[3])
                skill_pos[3].GetComponent<Text>().text = name_skill[3] + "(" + cd + ")";
        }
        public void do_skillCD_4()
        {
            endCD_skill[4]--;
            if (endCD_skill[4] <= 0)
            {
                endCD_skill[4] = 0;
                CancelInvoke("do_skillCD_4");
                A3_BuffModel.getInstance().RemoveBuff(skill_id[4]);
                return;
               
            }
            string cd = string.Format("{0:D2}:{1:D2}",/* endCD_skill[4] / 3600,*/ endCD_skill[4] % 3600 / 60, endCD_skill[4] % 60);
            if (skill_pos[4])
                skill_pos[4].GetComponent<Text>().text = name_skill[4] + "(" + cd + ")";
        }
        public void do_skillCD_5()
        {
            endCD_skill[5]--;
            if (endCD_skill[5] <= 0)
            {
                endCD_skill[5] = 0;
                CancelInvoke("do_skillCD_5");
                A3_BuffModel.getInstance().RemoveBuff(skill_id[5]);
                return;
               
            }
            string cd = string.Format("{0:D2}:{1:D2}", /*endCD_skill[5] / 3600,*/ endCD_skill[5] % 3600 / 60, endCD_skill[5] % 60);
            if (skill_pos[5])
                skill_pos[5].GetComponent<Text>().text = name_skill[5] + "(" + cd + ")";

          
                
        }
        public void do_skillCD_6()
        {
            endCD_skill[6]--;
            if (endCD_skill[6] <= 0)
            {
                endCD_skill[6] = 0;
                CancelInvoke("do_skillCD_6");
                A3_BuffModel.getInstance().RemoveBuff(skill_id[6]);
                return;
            }
            string cd = string.Format("{0:D2}:{1:D2}", /*endCD_skill[6] / 3600,*/ endCD_skill[6] % 3600 / 60, endCD_skill[6] % 60);
            if (skill_pos[6])
                skill_pos[6].GetComponent<Text>().text = name_skill[6] + "(" + cd + ")";
        }
        public void do_skillCD_7()
        {
            endCD_skill[7]--;
            if (endCD_skill[7] <= 0)
            {
                endCD_skill[7] = 0;
                CancelInvoke("do_skillCD_7");
                A3_BuffModel.getInstance().RemoveBuff(skill_id[7]);
                return;

            }
            string cd = string.Format("{0:D2}:{1:D2}", /*endCD_skill[7] / 3600,*/ endCD_skill[7] % 3600 / 60, endCD_skill[7] % 60);
            if (skill_pos[7])
                skill_pos[7].GetComponent<Text>().text = name_skill[7] + "(" + cd + ")";
        }
        public void do_skillCD_8()
        {
            endCD_skill[8]--;
            if (endCD_skill[8] <= 0)
            {
                endCD_skill[8] = 0;
                CancelInvoke("do_skillCD_8");
                A3_BuffModel.getInstance().RemoveBuff(skill_id[8]);
                return;
            }
            string cd = string.Format("{0:D2}:{1:D2}", /*endCD_skill[8] / 3600,*/ endCD_skill[8] % 3600 / 60, endCD_skill[8] % 60);
            if (skill_pos[8])
                skill_pos[8].GetComponent<Text>().text = name_skill[8] + "(" + cd + ")";
        }

        public void do_skillCD_9()
        {
            endCD_skill[9]--;
            if (endCD_skill[9] <= 0)
            {
                endCD_skill[9] = 0;
                CancelInvoke("do_skillCD_9");
                A3_BuffModel.getInstance().RemoveBuff(skill_id[9]);
                return;
            }
            string cd = string.Format("{0:D2}:{1:D2}", /*endCD_skill[8] / 3600,*/ endCD_skill[9] % 3600 / 60, endCD_skill[9] % 60);
            if (skill_pos[9])
                skill_pos[9].GetComponent<Text>().text = name_skill[9] + "(" + cd + ")";
        }
        public void do_skillCD_10()
        {
            endCD_skill[10]--;
            if (endCD_skill[10] <= 0)
            {
                endCD_skill[10] = 0;
                CancelInvoke("do_skillCD_10");
                A3_BuffModel.getInstance().RemoveBuff(skill_id[10]);
                return;
            }
            string cd = string.Format("{0:D2}:{1:D2}", /*endCD_skill[8] / 3600,*/ endCD_skill[10] % 3600 / 60, endCD_skill[10] % 60);
            if (skill_pos[10])
                skill_pos[10].GetComponent<Text>().text = name_skill[10] + "(" + cd + ")";
        }

        #endregion//InvokeRepeating的方法


        public void do_expCD()
        {
            endCD_exp--;
            if (endCD_exp <= 0)
            {
                endCD_exp = 0;
                dou_exp = false;
                if(a3_insideui_fb.instance)
                   a3_insideui_fb.instance.Using_jc();
                A3_BuffModel.getInstance().RemoveBuff(10001);
                CancelInvoke("do_expCD");
                return;
            }
            string cd;
            if (endCD_exp > 3600)
                cd = string.Format("{0:D2}:{1:D2}:{2:D2}", endCD_exp / 3600, endCD_exp % 3600 / 60, endCD_exp % 60);
            else
            {
                cd = string.Format("{0:D2}:{1:D2}",/* endCD_exp / 3600,*/ endCD_exp % 3600 / 60, endCD_exp % 60);
            }
            if(exp_pos)
            exp_pos.GetComponent<Text>().text = name_exp + "(" + cd + ")";
        }
        public void do_fuwenCD()
        {
            endCD_fuwen--;
            if (endCD_fuwen <= 0)
            {
                endCD_fuwen = 0;
                return;
            }
            string cd = string.Format("{0:D2}:{1:D2}",/* endCD_fuwen / 3600,*/ endCD_fuwen % 3600 / 60, endCD_fuwen % 60);
            if(fuwen_pos)
            fuwen_pos.GetComponent<Text>().text = name_fuwen + "(" + cd + ")";
        }
        public void do_blessCD()
        {
            endCD_bless--;
            if (endCD_bless <= 0)
            {
                bless_ad = false;
                if (a3_insideui_fb.instance)
                    a3_insideui_fb.instance.Using_jc();
                endCD_bless = 0;
                CancelInvoke("do_blessCD");
                A3_BuffModel.getInstance().RemoveBuff(10000);
                return;
            }
            string cd = string.Format("{0:D2}:{1:D2}", /*endCD_bless / 3600,*/ endCD_bless % 3600 / 60, endCD_bless % 60);
            if (bless_pos)
            bless_pos.GetComponent<Text>().text = name_bless + "(" + cd + ")";
        }





      
    }
}
