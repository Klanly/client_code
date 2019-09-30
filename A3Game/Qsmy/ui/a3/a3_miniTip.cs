using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using MuGame;

namespace MuGame
{
    /// <summary>
    /// 调用方法
    /// 在需要显示tip处:
    /// ArrayList arr = new ArrayList();
    /// arr.Add(itemId);//第一项为物品id
    /// arr.Add(showType);//第二项为显示类型： 1-道具Tip 2-装备Tip
    /// arr.Add(rewardDescText);//目前为首领界面专有 见onShowed-switch case 3
    /// 后续可以根据需要增加,在onShowed中增加更多类型和功能
    /// InterfaceMgr.getInstance().open(InterfaceMgr.A3_MINITIP,arr);
    /// </summary>
    class a3_miniTip : Window
    {
        public const int ITEM_ID = 0;
        public const int SHOW_TYPE = 1;
        public const int REWARD_DESC_TEXT = 2;
    
        private static a3_miniTip instance;
        public static a3_miniTip Instance { get { return instance; } set { instance = value; } }
        public RewardItemTip rewardItemTip;
        public RewardEquipTip rewardEquipTip;
        
        public override void init()
        {
            inText();
            Instance = this;
            rewardItemTip = new RewardItemTip(transform.FindChild("rewardItemTip").gameObject);
            rewardEquipTip = new RewardEquipTip(transform.FindChild("rewardEqpTip").gameObject);
        }


        void inText()
        {
            this.transform.FindChild("rewardItemTip/text_bg/nameBg/has").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_MiniTip_1");//已拥有：
            this.transform.FindChild("rewardEqpTip/text_bg/nameBg/carrReq").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_MiniTip_2");//使用职业:
            this.transform.FindChild("rewardEqpTip/text_bg/textBase/baseDisc").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_MiniTip_3");//基本属性：
            this.transform.FindChild("rewardEqpTip/text_bg/textAddBase").GetComponent<Text>().text = ContMgr.getCont("uilayer_A3_MiniTip_4");//追加属性类型：
        }
        public override void onShowed()
        {            
            switch ((int)uiData[SHOW_TYPE])
            {
                default:
                case -1:
                case 1:
                    if (uiData.Count >= 1 + ITEM_ID)
                    {
                        rewardItemTip.owner.SetActive(true);
                        rewardItemTip.ShowItemTip((uint)uiData[ITEM_ID]);
                    }
                    break;
                case 2:
                    if (uiData.Count >= 1 + ITEM_ID)
                    {
                        rewardEquipTip.owner.SetActive(true);
                        rewardEquipTip.ShowEquipTip((uint)uiData[ITEM_ID]);
                    }
                    break;
                case 3:
                    if (uiData.Count >= 1 + REWARD_DESC_TEXT)
                    {
                        rewardEquipTip.owner.SetActive(true);
                        rewardEquipTip.ShowXMLCustomizedEquipTip((uint)uiData[ITEM_ID], (RewardDescText)uiData[REWARD_DESC_TEXT]);
                    }
                    break;
            }
            this.transform.SetAsLastSibling();
        }
        public override void onClosed()
        {
            for (int i = 1 /* 0为半透明背景 */ ; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 物品的名称和描述tip
    /// </summary>
    class RewardItemTip 
    {
        public GameObject owner;
        public GameObject itemIcon;        
        public Text textDesc;        
        public Text textName;
        public Text textNum;
                
        public RewardItemTip(GameObject owner)
        {
           this. owner = owner;
            itemIcon = owner.transform.FindChild("text_bg/iconbg/icon").gameObject;
            textDesc = owner.transform.FindChild("text_bg/text").GetComponent<Text>();           
            textName = owner.transform.FindChild("text_bg/nameBg/itemName").GetComponent<Text>();
            textNum = owner.transform.FindChild("text_bg/nameBg/hasnum").GetComponent<Text>();
            new BaseButton(owner.transform.FindChild("close_btn")).onClick = (GameObject go) => InterfaceMgr.getInstance().close(InterfaceMgr.A3_MINITIP);
        }

        public void ShowItemTip(uint itemId)
        {
            if (itemIcon.transform.childCount > 0)
                for (int i = itemIcon.transform.childCount - 1; i >= 0; i--)
                    GameObject.Destroy(itemIcon.transform.GetChild(i).gameObject);
            IconImageMgr.getInstance().createA3ItemIcon(itemId, ignoreLimit: true).transform.SetParent(itemIcon.transform, false);
            SXML item = a3_BagModel.getInstance().getItemXml((int)itemId);
            textDesc.text = StringUtils.formatText(item.getString("desc"));
            textName.text = item.getString("item_name");
            textNum.text = a3_BagModel.getInstance().getItemNumByTpid(itemId)+ ContMgr.getCont("ge"); 
        }        
    }

    /// <summary>
    /// 装备的各项描述tip
    /// </summary>
    class RewardEquipTip
    {
        public GameObject owner;
        public GameObject itemIcon;
        Text textItemName,      //道具名称
             textTipDesc,       //Tip描述
             textCarrLimit,     //职业限制
             textBaseHead,      //基本属性
             textBaseAttr,
             textAddAttr,       //追加属性
             textExtraDesc1,    //附加描述1
             textExtraDesc2,    //附加描述2
             textExtraDesc3;    //附加描述3
        List<Text> listTextExtraDesc;        
        List<GameObject> listGoBgImg;//背景颜色组:List<GameObject>
        public RewardEquipTip(GameObject owner)
        {
            this.owner = owner;
            listGoBgImg = new List<GameObject>();
            listTextExtraDesc = new List<Text>();
            Transform tfBgImgParent = owner.transform.FindChild("bgImg");
            for (int i = 0, max = tfBgImgParent?.childCount ?? 0; i < max; i++)
                listGoBgImg.Add(tfBgImgParent.GetChild(i).gameObject);

            itemIcon = owner.transform.FindChild("text_bg/iconbg/icon").gameObject;
            textItemName = owner.transform.FindChild("text_bg/nameBg/itemName").GetComponent<Text>();
            textTipDesc = owner.transform.FindChild("text_bg/nameBg/tipDesc/dispText").GetComponent<Text>();
            textCarrLimit = owner.transform.FindChild("text_bg/nameBg/carrReq/dispText").GetComponent<Text>();
            textBaseHead = owner.transform.FindChild("text_bg/textBase/baseDisc").GetComponent<Text>();
            textBaseAttr = owner.transform.FindChild("text_bg/textBase/dispText").GetComponent<Text>();
            textAddAttr = owner.transform.FindChild("text_bg/textAddBase/dispText").GetComponent<Text>();
            listTextExtraDesc.Add(textExtraDesc1 = owner.transform.FindChild("text_bg/textTip1").GetComponent<Text>());
            listTextExtraDesc.Add(textExtraDesc2 = owner.transform.FindChild("text_bg/textTip2").GetComponent<Text>());
            listTextExtraDesc.Add(textExtraDesc3 = owner.transform.FindChild("text_bg/textTip3").GetComponent<Text>());
            new BaseButton(owner.transform.FindChild("close_btn")).onClick = (GameObject go) => InterfaceMgr.getInstance().close(InterfaceMgr.A3_MINITIP);
        }
        /// <summary>
        /// 根据道具品质显示对应的背景图片
        /// </summary>
        /// <param name="quality"> 道具品质 </param>
        /// <param name="beginWith"> 最低道具品质对应数值 </param>
        public void ShowBgImgByQuality(int quality, int beginWith = 0)
        {
            if (listGoBgImg.Count < quality - beginWith || quality < beginWith)
                return;
            else
            {
                for (int i = 0; i < listGoBgImg.Count; i++)
                    listGoBgImg[i]?.SetActive(false);
                listGoBgImg[quality - beginWith]?.SetActive(true);
            }
        }

        public void ShowEquipTip(uint itemId)
        {
            if (itemIcon.transform.childCount > 0)
                for (int i = itemIcon.transform.childCount - 1; i >= 0; i--)
                    GameObject.Destroy(itemIcon.transform.GetChild(i).gameObject);
            textBaseHead.gameObject.SetActive(false);
            IconImageMgr.getInstance().createA3ItemIcon(itemId, ignoreLimit: true).transform.SetParent(itemIcon.transform, false);
            SXML item = a3_BagModel.getInstance().getItemXml((int)itemId);
            textItemName.text = item.getString("item_name");

            //职业
            int carr = item.getInt("job_limit");
            switch (carr)
            {
                default:
                case -1:
                    textCarrLimit.text = ContMgr.getCont("a3_active_wuxianzhi");
                    break;
                case 2:
                    textCarrLimit.text = ContMgr.getCont("a3_firstRechargeAward_p1");
                    break;
                case 3:
                    textCarrLimit.text = ContMgr.getCont("a3_firstRechargeAward_p2");
                    break;
                case 5:
                    textCarrLimit.text = ContMgr.getCont("a3_firstRechargeAward_p3");
                    break;
            }

            //基础属性
            int attType = item.getInt("att_type");
            SXML itemInfo = XMLMgr.instance.GetSXML("item.stage", "stage_level==0");
            SXML eqpInfo = itemInfo.GetNode("stage_info", "itemid==" + itemId);
            if (eqpInfo != null)
            {
                switch (attType)
                {
                    case 5:
                        string[] attackValue = eqpInfo.getString("basic_att").Split(',');
                        int minVal, maxVal;
                        if (attackValue.Length == 1)
                            maxVal = minVal = int.Parse(attackValue[0]);
                        else if (attackValue.Length == 2)
                        {
                            minVal = int.Parse(attackValue[0]);
                            maxVal = int.Parse(attackValue[1]);
                        }
                        else
                            return;
                        textBaseAttr.text =ContMgr.getCont("a3_mapChangeLine10", new List<string>() { minVal.ToString(), maxVal.ToString() });//"攻击"属性存在一个范围
                        break;
                    default:
                        int val = eqpInfo.getInt("basic_att");
                        textBaseAttr.text = string.Format("{0}:{1}", Globle.getAttrNameById(attType), val);//其它属性是没有范围的
                        break;
                }
            }

            //基础追加属性
            textAddAttr.text = Globle.getAttrNameById(item.getInt("att_type"));

            //其它Tips说明
            SXML itemTip = item.GetNode("default_tip");
            if (itemTip == null)
            {
                Debug.LogError("未配置default_tip");
                return;
            }
            textTipDesc.text = itemTip.getString("equip_desc");
            List<SXML> listItemTipExtraDesc = itemTip.GetNodeList("random_tip");
            for (int i = 0; i < listItemTipExtraDesc.Count; i++)
            {
                if (i >= listTextExtraDesc.Count)
                    break;
                listTextExtraDesc[i].text = listItemTipExtraDesc[i].getString("tip");
            }

            ShowBgImgByQuality(item.getInt("quality"), beginWith: 1);
        }
        /// <summary>
        /// 根据配置表来显示
        /// </summary>
        /// <param name="itemId">物品的id</param>  
        /// <param name="rewardDescText">物品的文本信息</param>  
        public void ShowXMLCustomizedEquipTip(uint itemId,RewardDescText? rewardDescText)
        {
            if (!rewardDescText.HasValue)
                return;
            if (itemIcon.transform.childCount > 0)
                for (int i = itemIcon.transform.childCount - 1; i >= 0; i--)
                    GameObject.Destroy(itemIcon.transform.GetChild(i).gameObject);
            IconImageMgr.getInstance().createA3ItemIcon(itemId, ignoreLimit: true).transform.SetParent(itemIcon.transform, false);
            RewardDescText valRewardDescText = rewardDescText.Value;
            textItemName.text = valRewardDescText.strItemName;     //道具名称
            textTipDesc.text = valRewardDescText.strTipDesc;       //Tip描述
            textCarrLimit.text = valRewardDescText.strCarrLimit;   //职业限制
            textBaseHead.gameObject.SetActive(true);
            textBaseAttr.text = valRewardDescText.strBaseAttr;     //基本属性
            textAddAttr.text = valRewardDescText.strAddAttr;       //追加属性
            textExtraDesc1.text = valRewardDescText.strExtraDesc1; //附加描述1
            textExtraDesc2.text = valRewardDescText.strExtraDesc2; //附加描述2
            textExtraDesc3.text = valRewardDescText.strExtraDesc3; //附加描述3
        }
    }

    /// <summary>
    /// EquipTip 文本内容
    /// </summary>
    public struct RewardDescText        
    {
        public string strItemName;
        public string strTipDesc;
        public string strCarrLimit;
        public string strBaseAttr;
        public string strAddAttr;
        public string strExtraDesc1;
        public string strExtraDesc2;
        public string strExtraDesc3;
    }
}
