using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using UnityEngine;
using DG.Tweening;
using GameFramework;

namespace MuGame
{
    class AttackPointMgr
    {



        //   static internal int comboNum = 0;
        static public AttackPointMgr instacne;
        static public AttackPointMgr init()
        {
            if (instacne == null)
                instacne = new AttackPointMgr();
            return instacne;
        }
        //      private int curSkillId = 0;
        //    private List<HurtData> lHurtD;

        private Dictionary<BaseRole, lHurt> dHurt;

        TickItem process;
        public AttackPointMgr()
        {

            dHurt = new Dictionary<BaseRole, lHurt>();
            process = new TickItem(onUpdate);

            TickMgr.instance.addTick(process);
        }

        public void setHurt(BaseRole from, BaseRole to, hurtInfo hurtD, bool useCombo = false)
        {
            //  debug.Log("setHurt!!! " + hurtD["dmg"] + "" + hurtD["frm_iid"] + " " + hurtD["to_iid"]);

            if (!dHurt.ContainsKey(from))
                dHurt[from] = new lHurt();
            HurtData d = new HurtData(to, hurtD, useCombo);

            dHurt[from].add(d);

        }

        public void clear()
        {
            FightText.clear();

            dHurt.Clear();
        }

        private int curTick = 0;
        void onUpdate(float s)
        {
            curTick++;
            if (curTick < 40)
                return;

            float t = Time.time;
            BaseRole needRemoveKey = null;
            foreach (BaseRole go in dHurt.Keys)
            {
                lHurt lHurtD = dHurt[go];
                doAtt(lHurtD);

                if (needRemoveKey == null && lHurtD.Count == 0 && t - lHurtD.lastTimer > 5f)
                {
                    needRemoveKey = go;
                }

            }

            if (needRemoveKey != null)
                dHurt.Remove(needRemoveKey);
        }

        private void doAtt(lHurt lHurtD)
        {
            if (lHurtD.Count == 0)
                return;
            float t = Time.time;
            curTick = 0;
            List<HurtData> list = lHurtD.l;
            for (int i = 0; i < list.Count; i++)
            {
                HurtData d = list[i];
                if (t - d.timer > 2.0f)
                {
                    bool needdel = d.play(lHurtD.maxComboNum);
                    if (needdel)
                    {
                     
                        lHurtD.l.Remove(d);
                        i--;
                    }

                }
            }
        }

        public void onAttackHanle(BaseRole go, int skillid)
        {
            BaseRole temp = go;
            if (!dHurt.ContainsKey(temp))
                return;
            lHurt lHurtD = dHurt[temp];
            if (lHurtD.Count == 0)
                return;
            List<HurtData> list = lHurtD.l;
            for (int i = 0; i < list.Count; i++)
            {
                HurtData d = list[i];
                bool needdel = d.play(lHurtD.maxComboNum);
                if (needdel)
                {
                    i--;
                    list.Remove(d);
                }

            }
            //if (lHurtD.Count == 0)
            //    lHurtD.maxComboNum = 0;

            //if (lHurtD.maxComboNum > 0)
            //    lHurtD.maxComboNum--;
        }

        public void onAttackShake(float time, int count, float str)
        {
            GRMap.GAME_CAM_CAMERA.DOShakePosition(time, str, count);
        }



        public void onAttackBeginHanle(BaseRole go, int num)
        {
            // debug.Log("onAttackBeginHanle!!!" + num);
            BaseRole temp = go;
            if (!dHurt.ContainsKey(temp))
                dHurt[temp] = new lHurt();
            //  List<HurtData> list = dHurt[temp].l;
            //if (list.Count > 0)
            //{
            //    for (int i = 0; i < list.Count; i++)
            //    {
            //        HurtData d = list[i];
            //        d.playRest();
            //    }
            //    list.Clear();
            //}


            dHurt[temp].maxComboNum = num;
        }

    }

    class hurtInfo
    {
        public int dmg;
        public bool isdie = false;
    }

    class lHurt
    {
        public List<HurtData> l;
        private int _maxComboNum=0;
        public float lastTimer;
        public lHurt()
        {
            l = new List<HurtData>();
        }

        public int maxComboNum
        {
            set
            {
                lastTimer = Time.time;
                _maxComboNum = value;
                foreach (HurtData d in l)
                {
                    if (d.maxComboNum == 0)
                        d.maxComboNum = _maxComboNum;
                }


            }
            get { return _maxComboNum; }
        }

        public void add(HurtData d)
        {
            float t = Time.time;

            if (t - lastTimer > 0.7)
                _maxComboNum = 0;
            d.timer = t;
            d.maxComboNum = _maxComboNum;
            l.Add(d);
        }

        public int Count
        {
            get { return l.Count; }
        }

    }

    class HurtData
    {
        public List<float> lRandom = new List<float> { 1.05f, 1.01f, 0.96f, 1.02f, 0.95f, 1.03f, 0.99f, 1f };

        public int maxComboNum;
        public float timer;
        public BaseRole _ava;
        public hurtInfo _hurtD;
        public int singleAttack;
        public int comboNum = 0;
        public bool criatk = false;
        public bool useCombo = false;

        private bool inited = false;

        public HurtData(BaseRole ava, hurtInfo hurtD, bool combo)
        {
            _ava = ava;
            _hurtD = hurtD;
            useCombo = combo;
        }

        public void playRest()
        {
            if (comboNum == 0)
                return;

            _hurtD.dmg = (int)((float)_hurtD.dmg * comboNum);

            if (_hurtD.isdie == true && !_hurtD.isdie)
            {
             //   _ava.Die(_hurtD);
            }
          //  _ava.onHurt(_hurtD);

            comboNum = 0;
        }

        public bool play(int max)
        {
            if (!inited)
            {
                inited = true;
                maxComboNum = max;
            }

            if (maxComboNum >= 1 && comboNum == 0)
            {

                comboNum = maxComboNum;
                //  singleAttack = _hurtD["dmg"] / maxComboNum;

                //  debug.Log("new play:" + comboNum + " " + _hurtD["dmg"] + " " + singleAttack);
            }

            //if (useCombo)
            //    combo_txt.play();

         //   MediaClient.instance.PlaySoundUrl("media/skill/beattack", false, null);

            if (comboNum > 0)
            {
           //     _hurtD["dmg"] = singleAttack;
                doHurt();
                comboNum--;

                if (comboNum == 0)
                    return true;
                return false;
            }
            else
            {
                doHurt();
                return true;
            }
        }

        private int idx = 0;

        private void doHurt()
        {
            if (comboNum > 0)
            {
                idx++;
                if (idx >= lRandom.Count)
                {
                    idx = 0;
                }
              
                //if (comboNum ==1)
                //{
               
                //    if (_hurtD["isdie"] == true && !_ava.IsDie())
                //    {
                //        _ava.Die(_hurtD);
                //    }
                //}
              
                //_hurtD["dmg"] = (int)((float)_hurtD["dmg"] * lRandom[idx]);
            }
            //else if (_hurtD["isdie"] == true && !_ava.IsDie())
            //{
            //    _ava.Die(_hurtD);
            //}
            //_ava.grAvatar.onHurt(_hurtD);

        }
    }
}
