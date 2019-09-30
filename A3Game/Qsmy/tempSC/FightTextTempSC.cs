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
        public static int TYPE_TEXT = 0;
        public static int TYPE_ANI = 1;

        public static Vector3 vec_two = new Vector3(2, 2, 2);

        internal Animator ani;
        public List<FightTextTempSC> pool;
        public List<FightTextTempSC> playingPool;
        public Text txt;
        internal int _type;
        public INameObj _avatar;
        public float timer = 0;

        //private GRCamera3D camera;
        internal void init(int type)
        {
            _type = type;
            ani = GetComponent<Animator>();
            //camera = GRClient.instance.getGraphCamera();

            if (_type == TYPE_TEXT)
                txt = transform.FindChild("Text").GetComponent<Text>();
        }

        public void onAniOver()
        {
            gameObject.SetActive(false);
            playingPool.Remove(this);
            pool.Add(this);
        }
        internal void setActive(bool b)
        {
            gameObject.SetActive(b);
        }

        public int idx = 0;
       // public List<float> lRandom = new List<float> { 1.05f, 1.1f, 0.95f, 1.2f, 0.9f, 1.15f, 0.92f, 1f };
        internal void play(Vector3 pos, int num, bool criatk)
        {
            if (_type == TYPE_TEXT)
            {
                int temp = num;//(int)(num * lRandom[idx]);
                if (temp == 0) temp = 1;
                if (temp >= 99999999) temp = 99999999;//最大伤害上限
                txt.text = (criatk ? ContMgr.getCont("FightTextTempSC_txt") : "") + temp;
                //idx++;
                //if (idx >= lRandom.Count)
                //    idx = 0;
                if (criatk)
                    txt.GetComponent<RectTransform>().localScale = vec_two;
                else
                    txt.GetComponent<RectTransform>().localScale = Vector3.one;
            }

            //Vec3 vec=  camera.worldToScreenPoint(role.getPoss());

            RectTransform rec = this.GetComponent<RectTransform>();

            rec.position = pos;
        }

        //屏蔽战斗飘字跟随角色
        //void Update() {
        //    if (_avatar == null) return;
        //    if (_avatar.getHeadPos() == Vector3.zero) { this.gameObject.SetActive(false); }

        //    RectTransform rec = this.GetComponent<RectTransform>();
        //    rec.position = _avatar.getHeadPos();
        //}

    }
}
