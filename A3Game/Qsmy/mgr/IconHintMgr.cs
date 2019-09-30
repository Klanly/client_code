using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
   /// <summary>
   /// 消息提示类
   /// </summary>
         
    class IconHintMgr
    {
        
        public static int TYPE_ROLE = 1;                     //角色
        public static int TYPE_SHEJIAO = 2;                  //社交
        public static int TYPE_ACHIEVEMENT = 3;              //成就
        public static int TYPE_SKILL = 4;                    //技能
        public static int TYPE_YGYUWU = 5;                   //试炼
        public static int TYPE_EQUIP = 6;                    //锻造
        public static int TYPE_WING_SKIN= 7;                 //飞翼
        public static int TYPE_BAG = 8;                      //背包 
        public static int TYPE_MAIL = 9;                     //邮件 
        public static int TYPE_QIANGHUA = 10;                //强化
        public static int TYPE_JINGLIAN = 11;                //精炼
        public static int TYPE_ZHUIJIA = 12;              //追加
        public static int TYPE_XIANGQIAN = 13;                //镶嵌   

        Dictionary<int, GameObject> dic_hints = new Dictionary<int, GameObject>();
        Dictionary<int, List<GameObject>> dic_hints_havenum = new Dictionary<int, List<GameObject>>();

        //通用hint
        public void addHint(Transform tsm, int type)
        {
            GameObject obj = new GameObject("hide");
            string path= "icon_hint_tips";
            common(tsm, obj, path,-22f,-22f,1);
            dic_hints[type] = obj;
        }
        //显示num的hint
        public void addHint_havenum(Transform tsm, int type)
        {
            List<GameObject> lst = new List<GameObject>();
            GameObject obj = new GameObject("hide");
            string path = "icon_hint_sign_bagremind";
            common(tsm, obj, path, -22f, -20f, 0.7f);
            lst.Add(obj);

            GameObject objs = new GameObject("num");         
            Text txt = objs.AddComponent<Text>();
            objs.AddComponent<Shadow>();
            RectTransform rects = objs.transform.GetComponent<RectTransform>();
            rects.sizeDelta = new Vector2(28f, 25.7f);
            rects.localPosition = new Vector3(0, 0, 0);
            rects.transform.localScale = new Vector3(1f, 1f,1f);
            txt.font = a3_expbar.instance.ft;
            txt.fontStyle = FontStyle.Normal;
            txt.fontSize = 20;
            txt.alignment = TextAnchor.MiddleCenter;         
            txt.color = new Color(1, 0.92f, 0.54f,1);
            //txt.text = "2";
            objs.transform.SetParent(obj.transform, false);


            GameObject obj_c = new GameObject("hide");
            string path_man = "icon_hint_sign_bagremind3";
            common(tsm, obj_c, path_man,-22f,-20f,0.7f);
            lst.Add(obj_c);
            dic_hints_havenum[type] = lst;
        }
        //锻造里面按钮的hint
        public void addHint_equip(Transform tsm,int type)
        {
            GameObject obj = new GameObject("hide");
            string path = "icon_hint_tips";
            common(tsm, obj, path, -125f, -38f, 1);
            dic_hints[type] = obj;
        }
        void common(Transform obj_tsm,GameObject obj,string path,float x,float y,float scale)
        {          
            Image image = obj.AddComponent<Image>();
            string paths = path;

            RectTransform rect = image.transform.GetComponent<RectTransform>();
            rect.anchorMax = new Vector2(1, 1);
            rect.anchorMin = new Vector2(1, 1);

            rect.localPosition = new Vector3(x, y, 0);
            rect.transform.localScale = new Vector3(scale, scale, scale);

            image.sprite = GAMEAPI.ABUI_LoadSprite(paths);
            image.transform.SetParent(obj_tsm, false);
            image.SetNativeSize();
            obj.SetActive(false);
        }

        /// <summary>
        /// 打开消息提示
        /// </summary>
        /// <param name="type">类型</param>                 //例如飞翼传IconHintMgr.TYPE_WING_SKIN
        /// <param name="FunctionOpenMgr_id">等级锁</param> //例如飞翼传FunctionOpenMgr.WING
        /// <param name="num">显示的数量</param>
        /// <param name="isman">是否满格</param>
        public void showHint(int type, int FunctionOpenMgr_id = -1, int num = -1,bool isman = false)
        {
            //界面未初始化
            if(!inituiisok)
            {
                hintinfo ht = new hintinfo();
                ht.type = type;
                ht.FunctionOpenMgr_id = FunctionOpenMgr_id;
                ht.num = num;
                ht.isman = isman;
                lst_info.Add(ht);
                return;
            }
            if (FunctionOpenMgr_id > 0 && !FunctionOpenMgr.instance.Check(FunctionOpenMgr_id))
                return;
            if (num > -1)
            {
                if (dic_hints_havenum.ContainsKey(type))
                {
                    dic_hints_havenum[type][1].SetActive(isman ? true : false);
                    dic_hints_havenum[type][0].SetActive(isman ? false: true);
                    if (!isman)
                        dic_hints_havenum[type][0].transform.GetChild(0).GetComponent<Text>().text = num.ToString();                
                }
                else
                    return;
            }
            else
            {
                if (dic_hints.ContainsKey(type))
                    dic_hints[type].SetActive(true);
                else
                    return;
            }
                
        }
        /// <summary>
        ///  关闭消息提示
        /// </summary>
        /// <param name="type"></param>
        /// <param name="num">关闭显示数字类型的提示</param>
        /// <param name="isman">关闭显示满的提示</param>
        public void closeHint(int type,bool num=false,bool isman=false)
        {
            if(num)
                if(dic_hints_havenum.ContainsKey(type))
                    dic_hints_havenum[type][0].SetActive(false);
            if(isman)
                if (dic_hints_havenum.ContainsKey(type))
                    dic_hints_havenum[type][1].SetActive(false);
            if(num==false&&isman==false)
                if (dic_hints.ContainsKey(type))
                    dic_hints[type].SetActive(false);
        }
        public bool inituiisok = false;
        public List<hintinfo> lst_info = new List<hintinfo>();
        public void  initui()
        {
            if (lst_info.Count <= 0)
                return;
            else
            {
                for (int i=0;i< lst_info.Count;i++)
                {
                    showHint(lst_info[i].type, lst_info[i].FunctionOpenMgr_id, lst_info[i].num, lst_info[i].isman);
                }
            }

          
        }

        public static IconHintMgr _instance;
        public static IconHintMgr getInsatnce()
        {
            if (_instance == null)
                _instance = new IconHintMgr();
            return _instance;
        }



       
    }
     class hintinfo
    {
        public int type;
        public int FunctionOpenMgr_id;
        public int num;
        public bool isman;

    }
}
