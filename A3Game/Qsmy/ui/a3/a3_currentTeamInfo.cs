using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Cross;

namespace MuGame
{
    class a3_currentTeamInfo : FloatUi
    {
        public static a3_currentTeamInfo mInstance;
        Transform root;
        Transform itemCurrentInfoPrefab;
        Dictionary<uint, ItemCurrentTeamInfo> m_ItemCurrentTeamInfoDic;
        public Transform mContant;
        public Material mMaterialGrey;
        public Sprite leaveOnLineSprite;
        public Sprite deadSprite;
        Transform friend;
        public int job;
        override public void init()
        {
            mInstance = this;
            inText();
            m_ItemCurrentTeamInfoDic = new Dictionary<uint, ItemCurrentTeamInfo>();
            friend = getTransformByPath("contant/friend/friend");
            root = this.transform;
            mMaterialGrey = U3DAPI.U3DResLoad<Material>("uifx/uiGray");
            mContant = transform.FindChild("contant");
            itemCurrentInfoPrefab = transform.FindChild("temp/itemMemberInfo");
          
            leaveOnLineSprite = GAMEAPI.ABUI_LoadSprite("icon_comm_icon_notice");//TODO:临时离线图片资源替代
            deadSprite = GAMEAPI.ABUI_LoadSprite("icon_job_icon_boss");
            new BaseButton(getTransformByPath("contant/friend/friend/go")).onClick = (GameObject go) =>
              {
                  if (GRMap.instance.m_nCurMapID >= 3333)
                  {
                      flytxt.instance.fly(ContMgr.getCont("a3_currentTeamInfo_cant_invite_inFb"));
                      return;
                  }
                  ArrayList array = new ArrayList();
                  array.Add(2);
                  InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_SHEJIAO, array);
              };
        }

        void inText() {
            this.transform.FindChild("contant/friend/friend/go").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_currentTeamInfo_1");
        }

        override public void onShowed()
        {
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_SYNCTEAMBLOOD, onSyncTeamBlood);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_LEAVETEAM, onLeaveTeam);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_DISSOLVETEAM, onDissolveTeam);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_NOTICEHAVEMEMBERLEAVE, onNoticeHaveMemberLeave);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_AFFIRMINVITE, onAffirmInvite);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_KICKOUT, onNoticeHaveMemberLeave);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_NEWMEMBERJOIN, onNewMemberJoin);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_CHANGETEAMINFO, onChangeTeamInfo);
            TeamProxy.getInstance().addEventListener(TeamProxy.EVENT_NOTICEONLINESTATECHANGE, onNoticeOnlineStateChange);

            if (uiData != null)
            {

                Transform parent = (Transform)uiData[0];
                this.transform.SetParent(parent, false);
                this.transform.localScale = Vector3.one;
                this.transform.localPosition = Vector3.one;
                InvokeRepeating("SendSyncTeamBlood", 0.0f, 3f);//TODO:暂定1秒
                if (TeamProxy.getInstance().MyTeamData != null)
                {
                    for (int i = 0; i < TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count; i++)
                    {
                        uint cid = TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].cid;
                        if (cid == PlayerModel.getInstance().cid) continue;
                        if (m_ItemCurrentTeamInfoDic != null && !m_ItemCurrentTeamInfoDic.ContainsKey(cid))
                        {
                            ItemCurrentTeamInfo icti = new ItemCurrentTeamInfo(itemCurrentInfoPrefab);
                            string name = TeamProxy.getInstance().MyTeamData.itemTeamDataList[i].name;
                            icti.SetTxtName(name);
                            m_ItemCurrentTeamInfoDic.Add(cid, icti);
                        }
                    }
                    if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 5)
                    {
                        if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                        {
                            getGameObjectByPath("contant/friend").SetActive(true);
                            getTransformByPath("contant/friend").SetAsLastSibling();//放到最后
                        }
                        else
                        {
                            if (TeamProxy.getInstance().MyTeamData.membInv)
                            {
                                getGameObjectByPath("contant/friend").SetActive(true);
                                getTransformByPath("contant/friend").SetAsLastSibling();//放到最后
                            }
                            else
                                getGameObjectByPath("contant/friend").SetActive(false);
                        }
                    }
                    else
                        getGameObjectByPath("contant/friend").SetActive(false);
                }                
            }
        }
        void SendSyncTeamBlood()
        {
            if(TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count > 1)
                TeamProxy.getInstance().SendSyncTeamBlood();
        }
        override public void onClosed()
        {
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_SYNCTEAMBLOOD, onSyncTeamBlood);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_LEAVETEAM, onLeaveTeam);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_DISSOLVETEAM, onDissolveTeam);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_NOTICEHAVEMEMBERLEAVE, onNoticeHaveMemberLeave);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_AFFIRMINVITE, onAffirmInvite);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_KICKOUT, onNoticeHaveMemberLeave);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_NEWMEMBERJOIN, onNewMemberJoin);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_CHANGETEAMINFO, onChangeTeamInfo);
            TeamProxy.getInstance().removeEventListener(TeamProxy.EVENT_NOTICEONLINESTATECHANGE, onNoticeOnlineStateChange);

            CancelInvoke("SendSyncTeamBlood");
        }
        void onChangeTeamInfo(GameEvent e)
        {
            if (e.data.ContainsKey("memb_inv"))
            {
                if (TeamProxy.getInstance().MyTeamData.meIsCaptain)
                {
                    getGameObjectByPath("contant/friend").SetActive(true);
                }
                else
                {
                    if (e.data["memb_inv"])
                    {
                        getGameObjectByPath("contant/friend").SetActive(true);
                    }
                    else
                        getGameObjectByPath("contant/friend").SetActive(false);
                }
            }
        }
        void onSyncTeamBlood(GameEvent e)
        {
            Variant data = e.data;
            List<Variant> infos = data["infos"]._arr;
            for (int i = 0; i < infos.Count; i++)
            {
                uint cid = infos[i]["cid"];
                if (!TeamProxy.getInstance().MyTeamData.IsInMyTeam(cid)) continue;
                uint maxHp = infos[i]["max_hp"];
                uint hp = infos[i]["hp"];
                ItemTeamData itd = new ItemTeamData();
                itd.cid = cid;
                itd.hp = hp;
                itd.maxHp = maxHp;
                for (int j = 0; j < TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count; j++)
                {
                    if (cid == TeamProxy.getInstance().MyTeamData.itemTeamDataList[j].cid)
                    {
                        itd.carr = TeamProxy.getInstance().MyTeamData.itemTeamDataList[j].carr;

                        itd.isCaptain = TeamProxy.getInstance().MyTeamData.itemTeamDataList[j].isCaptain;
                        itd.online = TeamProxy.getInstance().MyTeamData.itemTeamDataList[j].online;
                        break;
                    }
                }
                if (m_ItemCurrentTeamInfoDic.ContainsKey(cid))
                {
                    m_ItemCurrentTeamInfoDic[cid].SetInfo(itd);
                }
                else
                {
                    ItemCurrentTeamInfo icti = new ItemCurrentTeamInfo(itemCurrentInfoPrefab);
                    icti.SetInfo(itd);
                    m_ItemCurrentTeamInfoDic.Add(cid, icti);
                }
            }
        }
        void onLeaveTeam(GameEvent e)
        {

            DistroyItemCurrentTeamInfo();

        }
        void onDissolveTeam(GameEvent e)
        {

            DistroyItemCurrentTeamInfo();
        }
        void onNoticeHaveMemberLeave(GameEvent e)
        {
            Variant data = e.data;
            uint cid = data["cid"];
            if (m_ItemCurrentTeamInfoDic.ContainsKey(cid))
            {
                Destroy(m_ItemCurrentTeamInfoDic[cid].root.gameObject);
                m_ItemCurrentTeamInfoDic.Remove(cid);
            }
        }
        void onAffirmInvite(GameEvent e)
        {
            Variant data = e.data;
            if (data == null) return;
            if (data.ContainsKey("cid"))
            {
                uint cid = data["cid"];
                List<ItemTeamData> itdList = TeamProxy.getInstance().MyTeamData.itemTeamDataList;
                for (int i = 0; i < itdList.Count; i++)
                {
                    if (!m_ItemCurrentTeamInfoDic.ContainsKey(cid) && cid != PlayerModel.getInstance().cid)
                    {
                        ItemCurrentTeamInfo icti = new ItemCurrentTeamInfo(itemCurrentInfoPrefab);
                        m_ItemCurrentTeamInfoDic.Add(cid, icti);
                    }
                }
            }
            if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 5)
            {
                getGameObjectByPath("contant/friend").SetActive(true);
            }
            else
                getGameObjectByPath("contant/friend").SetActive(false);
            getTransformByPath("contant/friend").SetAsLastSibling();

        }
        void onNewMemberJoin(GameEvent e)
        {
            Variant data = e.data;
            uint cid = data["cid"];
            string name = data["name"];
            uint carr = data["carr"];
            if (!m_ItemCurrentTeamInfoDic.ContainsKey(cid))
            {
                ItemCurrentTeamInfo icti = new ItemCurrentTeamInfo(itemCurrentInfoPrefab);
                icti.SetTxtName(name);
                icti.sethead(carr);
                m_ItemCurrentTeamInfoDic.Add(cid, icti);
            }
            else
            {
                m_ItemCurrentTeamInfoDic[cid].sethead(carr);
                m_ItemCurrentTeamInfoDic[cid].SetTxtName(name);
            }
            if (TeamProxy.getInstance().MyTeamData.itemTeamDataList.Count < 5)
            {
                getGameObjectByPath("contant/friend").SetActive(true);
            }
            else
                getGameObjectByPath("contant/friend").SetActive(false);
            getTransformByPath("contant/friend").SetAsLastSibling();
        }
        void onNoticeOnlineStateChange(GameEvent e)
        {
            Variant data = e.data;
            uint cid = data["cid"];
            bool online = data["online"];
            if (m_ItemCurrentTeamInfoDic.ContainsKey(cid))
            {
                m_ItemCurrentTeamInfoDic[cid].SetOnLine(online);
            }

        }
        void DistroyItemCurrentTeamInfo()
        {
            CancelInvoke("SendSyncTeamBlood");
            foreach (KeyValuePair<uint, ItemCurrentTeamInfo> item in m_ItemCurrentTeamInfoDic)
            {
                Destroy(item.Value.root.gameObject);
            }
            m_ItemCurrentTeamInfoDic.Clear();
        }
        class ItemCurrentTeamInfo
        {
            Image iconHead;
            Image img_state;
            Image img_captain;
            Text txtName;
            Slider sldBlood;
            GameObject job;
            Image sldImgBackGround;//血量slider的背景
            Image sldImgForeGround;//血量slider的前景

            float[] PercentLow100 = new float[4] { 40f / 255f, 255f / 255f, 0f, 255f / 255f };// {40,255,0,255};//90-100%
            float[] PercentLow90 = new float[4] { 255f / 255f, 237f / 255f, 0f, 255f / 255f };//50-89%
            float[] PercentLow50 = new float[4] { 255 / 255f, 0f, 37f / 255f, 255f / 255f };//1-49%EVENT_NOTICEONLINESTATECHANGE
            public Transform root;

            public ItemCurrentTeamInfo(Transform trans)
            {
                GameObject go = Instantiate(trans.gameObject) as GameObject;
                root = go.transform;
                root.SetParent(a3_currentTeamInfo.mInstance.mContant);
                root.gameObject.SetActive(true);
                root.localScale = Vector3.one;
                iconHead = root.FindChild("icon").GetComponent<Image>();
                job = root.FindChild("head").gameObject;
                img_state = iconHead.transform.FindChild("state").GetComponent<Image>();
                img_captain = root.FindChild("iconCaptain").GetComponent<Image>();
                txtName = root.FindChild("txtName").GetComponent<Text>();
                sldBlood = root.FindChild("Slider").GetComponent<Slider>();
                sldImgBackGround = sldBlood.transform.FindChild("Background").GetComponent<Image>();
                sldImgForeGround = sldBlood.transform.FindChild("Fill Area/Fill").GetComponent<Image>();
            }
            public void SetInfo(ItemTeamData itd)
            {
                sethead(itd.carr);
                SetSldImgBackGround(itd.hp);
                SetSldImgForeGround(itd.hp, itd.maxHp);
                SetOnLine(itd.online);
                SetCaptainSign(itd.isCaptain);
                // Debug.LogError(itd.name+ itd.carr );
            }

            void SetSldImgBackGround(uint hp)//背景是否中空
            {
                if (hp <= 0)
                {
                    sldImgBackGround.fillCenter = false;
                    sldBlood.value = 0.0f;
                }
                else
                {
                    sldImgBackGround.fillCenter = true;
                }
            }
            void SetSldImgForeGround(uint hp, uint maxHp)//前景血量减少百分比变色
            {
                float percent = (float)hp / (float)maxHp;
                sldBlood.value = float.Parse(percent.ToString("F2"));
                //job.transform.FindChild("job" + a3_currentTeamInfo.mInstance.job).gameObject.SetActive(true);
                if (hp <= 0f)//玩家死亡时候显示
                {
                    SetIconHead(true);
                }
                if ((float)hp / (float)maxHp < 0.5f)
                {
                    sldImgForeGround.color = new UnityEngine.Color(PercentLow50[0], PercentLow50[1], PercentLow50[2], PercentLow50[3]);
                    return;
                }
                if ((float)hp / (float)maxHp < 0.9f)
                {
                    sldImgForeGround.color = new UnityEngine.Color(PercentLow90[0], PercentLow90[1], PercentLow90[2], PercentLow90[3]);
                    return;
                }
                if ((float)hp / (float)maxHp <= 1.0f)
                {
                    sldImgForeGround.color = new UnityEngine.Color(PercentLow100[0], PercentLow100[1], PercentLow100[2], PercentLow100[3]);
                    return;
                }

            }
            public void SetTxtName(string nameStr, bool isDead = false)//设置队员名称
            {
                txtName.text = nameStr;
            }
            void SetIconHead(bool isDead = false)//设置队员头像图标
            {
                if (isDead)
                {
                    iconHead.material = a3_currentTeamInfo.mInstance.mMaterialGrey;
                    img_state.sprite = a3_currentTeamInfo.mInstance.deadSprite;//need  edit
                }
                else
                {
                    iconHead.material = null;

                }

            }

            void SetSlider(uint hp, uint maxHp)
            {
                if (hp > 0)
                {
                    sldBlood.maxValue = maxHp;
                    sldBlood.minValue = hp;
                }
                else
                {
                    sldBlood.value = 0f;
                }
            }
            public void SetOnLine(bool online)//
            {
                if (online)
                {
                    sldImgForeGround.material = null;
                    sldImgBackGround.material = null;
                    iconHead.material = null;
                }
                else
                {
                    sldImgForeGround.material = a3_currentTeamInfo.mInstance.mMaterialGrey;
                    sldImgBackGround.material = a3_currentTeamInfo.mInstance.mMaterialGrey;
                    iconHead.material = a3_currentTeamInfo.mInstance.mMaterialGrey;
                }

            }
            public void SetCaptainSign(bool b)//设置队长icon
            {
                img_captain.gameObject.SetActive(b);
            }

            internal void sethead(uint carr)
            {

                job.transform.FindChild("job2").gameObject.SetActive(false);
                job.transform.FindChild("job3").gameObject.SetActive(false);
                job.transform.FindChild("job5").gameObject.SetActive(false);

                job.transform.FindChild("job" + carr).gameObject.SetActive(true);
            }
        }

    }
}
