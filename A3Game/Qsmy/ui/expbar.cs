using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Cross;

namespace MuGame
{
    class expbar : FloatUi
    {
        public static expbar instance;
      
        public override void init()
        {
            alain();
            instance = this;
            //if (cemaraRectTran == null)
            //    cemaraRectTran = GameObject.Find("canvas").GetComponent<RectTransform>();

            //RectTransform cv = cemaraRectTran;

            //RectTransform bg = transform.FindChild("ig_bg").gameObject.GetComponent<RectTransform>();
            //RectTransform bar = transform.FindChild("exp_bar").gameObject.GetComponent<RectTransform>();

            //bg.sizeDelta = new Vector2(cv.rect.width, bg.sizeDelta.y);
            //bar.sizeDelta = new Vector2(cv.rect.width, bg.sizeDelta.y);

          

            refreshExp(null);
        }
        public void refreshExp(GameEvent e)
        {
            SXML s_xml = XMLMgr.instance.GetSXML("role_attribute.growthup", "id==" + PlayerModel.getInstance().lvl);
            int cost_exp = s_xml.getInt("cost_exp");
            //transform.FindChild("Text").gameObject.GetComponent<Text>().text = PlayerModel.getInstance().exp + "/" + cost_exp;
            transform.FindChild("exp_bar").gameObject.GetComponent<Image>().fillAmount = (float)PlayerModel.getInstance().exp / (float)cost_exp;
        }
    }



}
