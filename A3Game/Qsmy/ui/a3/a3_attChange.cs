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
using DG.Tweening;

namespace MuGame
{
    class a3_attChange : LoadingUI
    {
       List< Dictionary<uint, int>> change_att = new List<Dictionary<uint, int>> ();
        private GameObject item;
        private Transform con;
        private GameObject bg;
        private GameObject veiw;
        private bool canshow = false;
        private float itemsize;
        public static a3_attChange instans;
        //Animator ani;
        public override void init()
        {
            instans = this;
            bg = transform.FindChild("bg").gameObject;
            this.gameObject.SetActive(false);
            item = transform.FindChild("scrollview/item").gameObject;
            con = transform.FindChild("scrollview/con");
            veiw = transform.FindChild("scrollview").gameObject;
            itemsize = con.GetComponent<GridLayoutGroup>().cellSize.y;
            bg.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            //ani = bg.GetComponent<Animator>();
        }
        public override void onShowed()
        {
            //transform.FindChild("ig_bg_bg").gameObject.SetActive(false); 
            //tame = waiteTime;
            //time_over = waiteTime_h;
            //if (uiData == null)
                //return;
            //if (uiData.Count != 0)
            //{
                //change_att = (Dictionary<uint, int>)uiData[0];
            //}

            for (int i = 0; i < con.childCount; i++)
            {
                Destroy(con.GetChild(i).gameObject);
            }
        }

        private bool onshow = false;
        public void runTxt( Dictionary<uint ,int> l)
        {

            if (!a3_EquipModel.getInstance().Attchange_wite)
            {
                if (!onshow)
                {
                    canshow = true;
                    show_bg = true;
                    time_over = waiteTime_h;
                    this.gameObject.SetActive(true);
                }

                onshow = true;
            }
            if (l != null)
            {
                change_att.Add(l);
                if (change_att.Count >= 3)
                {
                    change_att.Remove(change_att[1]);
                }
            }
        }

        private uint curtype = 0;

        private float viewhight = 0;
        void showAtt()
        {
            foreach ( uint type in change_att[0].Keys)
            {
                curtype = type; 
                GameObject clon = Instantiate(item);
                clon.SetActive(true);
                int nowAtt = PlayerModel.getInstance().attr_list[type];
                clon.transform.FindChild ("attValue").GetComponent<Text>().text = Globle.getAttrAddById((int)type, nowAtt - change_att[0][type]);
                int ss = 0;
                DOTween.To(() => ss, (float s) => {
                    ss = (int)s;
                    clon.transform.FindChild("addValue").GetComponent<Text>().text = Globle.getAttrAddById_value((int)type,ss, true);
                }, (float)nowAtt, 1f);               
                clon.transform.SetParent(con, false);
                //change_att[0].Remove(curtype);
                // break;
            }
            wite();
        }

        void setbg()
        {            
            int ss = 0;
            viewhight = change_att[0].Count * itemsize;
            Tweener tween1= DOTween.To(() => ss, (float s) => {
                ss = (int)s;
                bg.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ss);
            }, (float)(change_att[0].Count * itemsize)+20f,0.5f);
            veiw.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, change_att[0].Count * itemsize);
            tween1.OnComplete(delegate ()
            {
                showAtt();
            });
        }


        void setbg_over()
        {
            int ss = 0;
            Tweener tween1 = DOTween.To(() => ss, (float s) => {
            }, (float)0, 0.5f);
            tween1.OnComplete(delegate ()
            {
                bg.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);  
                for (int i = 0; i < con.childCount; i++)
                {
                    Destroy(con.GetChild(i).gameObject);
                }
                if (change_att.Count <= 0)
                {
                    this.gameObject.SetActive(false);
                    onshow = false;
                    canshow = false;
                    show_bg = false;
                }
                else
                {
                    canshow = true;
                    show_bg = true;
                }
            });
        }
        void wite()
        {
            canshow = false;
            int ss = 0;
            Tweener tween1 = DOTween.To(() => ss, (float s) => {
            }, (float)0, 1.5f);
            tween1.OnComplete(delegate ()
            {

                for (int i = 0; i < con.childCount; i++)
                {
                    Destroy(con.GetChild(i).gameObject);
                }
                bg.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
                change_att.Remove(change_att[0]);
                if (change_att.Count > 0)
                {
                    show_bg = true;
                    canshow = true;
                }
                else if (change_att == null || change_att.Count<=0)
                {
                    this.gameObject.SetActive(false);
                    onshow = false;
                    canshow = false;
                    show_bg = false;
                }
            });
        }

        private float waiteTime = 0.5f;
        //private float tame = 0; // 每个att间计时器
        private float time_over = 0; // 结束停顿计时器
        private float waiteTime_h = 0.8f;
        //private bool showtext = false;
        private bool show_bg = true;
        private bool onOver = false;
        void Update()
        {
            if (!canshow) return;
            if (change_att.Count <= 0) return;
            //if (change_att)
            //{

            //}
            if (show_bg)
            {
                setbg();
                show_bg = false;
            }
            //if ( change_att[0] != null && change_att[0].Count > 0)
            //{
                //tame -= Time.deltaTime;
                //if (tame <= 0)
                //{
             //       showAtt(); 
                    //tame = waiteTime;
                //}
           // }
            //else if (change_att[0] == null || change_att[0].Count <= 0)
           // {
            //    change_att.Remove(change_att[0]);
            //    if (change_att.Count >0)
           //     { 
              //      wite();
             //   }
            //    show_bg = true;
           // }

            //if(change_att == null || change_att.Count<=0)
            //{
            //    if (canshow)
            //    {
            //        setbg_over();
            //        canshow = false;
            //    }
            //}
        }

        public override void onClosed()
        {

        }
    }


}
