using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using System.Collections;
using System.Linq;

namespace MuGame
{
    class a3_Winginfo : Window
    {
        private GameObject m_SelfObj;//翅膀的avatar
        private GameObject scene_Camera;
        private GameObject scene_Obj;
        Transform stars;

        public override void init()
        {
            stars = transform.FindChild("playerInfo/panel_attr/stars");
            new BaseButton(getTransformByPath("btn_close")).onClick = (GameObject go) =>
            {
                if (a3_ranking.isshow && a3_ranking.isshow.Toback)
                {
                    a3_ranking.isshow.Toback.SetActive(true);
                    if (a3_ranking.isshow.showAvt != null && a3_ranking.isshow.scene_Camera != null)
                    {
                        a3_ranking.isshow.showAvt.SetActive(true);
                        a3_ranking.isshow.scene_Camera.SetActive(true);
                    }
                    a3_ranking.isshow.Toback = null;
                }
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_WINGINFO); InterfaceMgr.getInstance().close(InterfaceMgr.A3_WINGINFO);
            };
            this.getEventTrigerByPath("avatar_touch").onDrag = OnDrag;
        }

        int carr = 0;
        int stage = 0;
        int lvl = 0;
        public override void onShowed()
        {
            if (uiData != null && uiData.Count > 0)
            {
                carr = (int)uiData[0];
                stage = (int)uiData[1];
                lvl = (int)uiData[2];
            }
            else {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_WINGINFO);
                if (a3_ranking.isshow && a3_ranking.isshow.Toback)
                {
                    a3_ranking.isshow.Toback.SetActive(true);
                    if (a3_ranking.isshow.showAvt != null && a3_ranking.isshow.scene_Camera != null)
                    {
                        a3_ranking.isshow.showAvt.SetActive(true);
                        a3_ranking.isshow.scene_Camera.SetActive(true);
                    }
                    a3_ranking.isshow.Toback = null;
                }
                flytxt.instance.fly(ContMgr.getCont("a3_Winginfo_bad"));
            }
            createAvatar(carr, stage);
            RefreshAtt(carr, stage, lvl);
            setStars(lvl);
            this.transform.SetAsLastSibling();
            GRMap.GAME_CAMERA.SetActive(false);
            transform.FindChild("ig_bg_bg").gameObject.SetActive(false);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
        }
        public override void onClosed()
        {
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            GRMap.GAME_CAMERA.SetActive(true);
            disposeAvatar();
            if (a3_ranking.isshow && a3_ranking.isshow.Toback)
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_RANKING);
            }
        }

        private void setStars(int lvl)
        {
            for (int i = 0; i < 10; i++)
            {
                stars.GetChild(i).FindChild("b").gameObject.SetActive(false);
            }
            for (int j = 0; j < lvl; j++)
            {
                stars.GetChild(j).FindChild("b").gameObject.SetActive(true);
            }
        }

        private void RefreshAtt(int carr,int stage,int lvl )
        {
            GameObject tempPgaeAtt = this.transform.FindChild("playerInfo/panel_attr/att/att_temp").gameObject;
            SXML carrXml = XMLMgr.instance.GetSXML("wings.wing", "career=="+ carr);
            SXML satgeXml = carrXml.GetNode("wing_stage", "stage_id==" + stage);
            SXML lvlXml = satgeXml.GetNode("wing_level", "level_id==" + lvl);
            List<SXML> atts = lvlXml.GetNodeList("att", null);

            this.transform.FindChild("playerInfo/panel_attr/name").GetComponent<Text>().text = satgeXml.getString("name");
            this.transform.FindChild("playerInfo/panel_attr/lv").GetComponent<Text>().text = "LV" + lvl + "(" + stage + ContMgr.getCont("a3_auction_jie") + ")";

            Transform con = this.transform.FindChild("playerInfo/panel_attr/att/grid");
            for (int i = 0; i < con.childCount;i++)
            {
                Destroy(con.GetChild (i).gameObject);
            }

            Dictionary<int, string> dicAtt = new Dictionary<int, string>();

            int att_type;
            float att_value;
            string[] attackStr = new string[2];

            for (int j = 0; j < atts.Count; j++)
            {
                att_type = atts[j].getInt("att_type");
                att_value = atts[j].getFloat("att_value");

                if (att_type == 5)
                {
                    attackStr[1] = att_value.ToString();
                    dicAtt.Add(att_type, "");
                }
                else if (att_type == 38)
                    attackStr[0] = att_value.ToString();
                else
                    dicAtt.Add(att_type, att_value.ToString());
            }
            if (dicAtt.ContainsKey(5))
            {
                dicAtt[5] = attackStr[0] + "-" + attackStr[1];
            }
            int k = 0;
            int childCount = con.childCount;
            //List<int> listKeys = dicAtt.Keys.ToList<int>();
            foreach (int att in dicAtt.Keys)
            {
                GameObject attentity;
                attentity = GameObject.Instantiate(tempPgaeAtt) as GameObject;
                attentity.transform.SetParent(con, false);
                attentity.gameObject.SetActive(true);
                Text textName = attentity.transform.FindChild("text_name").GetComponent<Text>();
                Text textValue = attentity.transform.FindChild("text_value").GetComponent<Text>();
                textName.text = Globle.getAttrNameById(att);
                textValue.text = dicAtt[att];
            }
        }

        public void createAvatar(int carr, int stage)
        {
            GameObject obj_prefab;
            obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_scene_ui_camera");
            scene_Camera = GameObject.Instantiate(obj_prefab) as GameObject;
            obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_show_scene");
            scene_Obj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.49f, 15.1f), new Quaternion(0, 180, 0, 0)) as GameObject;
            foreach (Transform tran in scene_Obj.GetComponentsInChildren<Transform>())
            {
                if (tran.gameObject.name == "scene_ta")
                    tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;
                else
                    tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
            }
            string path = "";
            switch (carr)
            {
                case 2:
                    path = "profession_warrior_wing_l_" + stage;
                    break;
                case 3:
                    path = "profession_mage_wing_l_" + stage;
                    break;
                case 5:
                    path = "profession_assa_wing_l_" + stage;
                    break;
            }
            obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject(path);
            if (obj_prefab != null)
            {
                m_SelfObj = GameObject.Instantiate(obj_prefab, new Vector3(-77.34f, 0.45f, 15.028f), Quaternion.AngleAxis(180, Vector3.up)) as GameObject;
                m_SelfObj.transform.localScale = new Vector3(0.7f,0.7f,0.7f);
                foreach (Transform tran in m_SelfObj.GetComponentsInChildren<Transform>())
                {
                    tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;// 更改物体的Layer层
                }
            }

        }

        public void disposeAvatar()
        {
            if (m_SelfObj != null) GameObject.Destroy(m_SelfObj);
            if (scene_Obj != null) GameObject.Destroy(scene_Obj);
            if (scene_Camera != null) GameObject.Destroy(scene_Camera);
        }
        void OnDrag(GameObject go, Vector2 delta)
        {
            if (m_SelfObj != null)
            {
                m_SelfObj.transform.Rotate(Vector3.up, -delta.x);
            }
        }
    }
}
