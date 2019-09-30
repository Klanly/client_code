using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace MuGame
{
    public class EffMgr
    {
        public static EffMgr instance = new EffMgr();

        List<EffItm> lEff = new List<EffItm>();
        public static Dictionary<int, GameObject> dRuneEff = new Dictionary<int, GameObject>();
        private static SXML runeXml;
        public static GameObject getRuneEff(int tuneid)
        {
            if (runeXml == null)
                runeXml = XMLMgr.instance.GetSXML("rune");

            if (!dRuneEff.ContainsKey(tuneid))
            {
                SXML xml = runeXml.GetNode("rune", "id==" + tuneid);
                if (xml == null)
                    return null;
                string str = xml.getString("eff");
                if (str == "null")
                    dRuneEff[tuneid] = null;
                else
                    dRuneEff[tuneid] = GAMEAPI.ABFight_LoadPrefab("FX_comFX_fuwenFX_" + str);
            }

            return dRuneEff[tuneid];
        }


        TickItem tick;
        public EffMgr()
        {
            tick = new TickItem(onUpdate);
            TickMgr.instance.addTick(tick);
        }

        public void addEff(BaseRole frm, BaseRole to, GameObject eff, float sec)
        {
            EffItm itm = new EffItm();
            itm.to = to;
            itm.frm = frm;
            itm.eff = eff;
            itm.sec = sec;
            eff.transform.SetParent(U3DAPI.FX_POOL_TF, false);
            lEff.Add(itm);
        }


        public void removeEff(EffItm itm)
        {
            lEff.Remove(itm);

            itm.dispose();
        }

        void onUpdate(float s)
        {
            List<EffItm> delItem = null;
            foreach (EffItm itm in lEff)
            {
                itm.update(s);
                if (itm.sec < 0)
                {
                    if (delItem == null)
                        delItem = new List<EffItm>();
                    delItem.Add(itm);
                }
            }

            if (delItem != null)
            {
                foreach (EffItm itm in delItem)
                {
                    removeEff(itm);
                }
            }
        }

    }

    public class EffItm
    {
        public BaseRole to;
        public BaseRole frm;
        public GameObject eff;

        public float sec;

        public void update(float s)
        {
            if (to == null || frm == null || to.disposed || frm.disposed || to.isDead || frm.isDead)
            {
                sec = -1;
                return;
            }

            sec = sec - s;
            Vector3 pos0 = to.m_curModel.position;
            Vector3 pos1 = frm.m_curModel.transform.position;
            Quaternion rot = Quaternion.LookRotation(pos0 - pos1);
            eff.transform.localRotation = rot;

            Vector3 tempos = pos1;
            tempos.y = frm.headOffset_half.y + tempos.y;
            eff.transform.position = tempos;//Vector3.Lerp(pos0, pos1, 0.5f);


            eff.transform.localScale = new Vector3(1f, 1f, Vector3.Distance(pos0, pos1));
        }

        public void dispose()
        {
            GameObject.Destroy(eff);
        }
    }
}
