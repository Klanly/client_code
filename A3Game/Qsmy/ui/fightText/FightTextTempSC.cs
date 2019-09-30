using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
    [AddComponentMenu("QSMY/FightText")]
    public class FightTextTempSC : MonoBehaviour
    {
        internal Animator ani;
        public List<FightTextTempSC> pool;
        public Text txt;

        private GRCamera3D camera;
        internal void init()
        {
            ani = GetComponent<Animator>();
            camera = GRClient.instance.getGraphCamera();
            txt = transform.FindChild("Text").GetComponent<Text>();
        }

        public void onAniOver()
        {
            gameObject.SetActive(false);
            pool.Add(this);
        }
        internal void setActive(bool b)
        {
            gameObject.SetActive(b);
        }


        internal void play(Vector3 pos, int num)
        {
            txt.text = num + "";
            //Vec3 vec=  camera.worldToScreenPoint(role.getPoss());

            RectTransform rec = this.GetComponent<RectTransform>();

            rec.position = pos;
        }
    }
}
