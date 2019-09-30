using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
namespace MuGame
{
    class SkillProxy:BaseProxy<SkillProxy>
    {
        public static uint EVENT_SKILL_UP = 0;
        public SkillProxy()
            :base()
        {
            //addProxyListener(PKG_NAME.S2C_SETUP_SKILL, onSetUpSkill);
            //addProxyListener(PKG_NAME.S2C_LEARN_SKILL, onSkillUp);
        }
    
        public void sendSkillUp(int id)
        {
            Variant msg = new Variant();
            msg["skill_id"] = id;
            sendRPC(87, msg);
        }
        void onSkillUp(Variant data)
        {
            int id = data["skid"];
            int lv = data["sklvl"];
            //int spt = data["spt"];

            Variant skill = new Variant();
            skill["skill_id"] = id;
            skill["skill_level"] = lv;
            SkillModel.getInstance().changeSkillList(skill);
        }
        void onSetUpSkill(Variant data)
        {
            int res = data["res"];
            if (res != 1)
            {
                Globle.err_output(res);
                return;
            }
            Variant skill = new Variant();
            skill["skill_id"] = data["skill_id"];
            skill["skill_level"] = data["skill_level"];
            SkillModel.getInstance().changeSkillList(skill);

            dispatchEvent(GameEvent.Create(EVENT_SKILL_UP, this, skill));
        }

        //public void sendSkexpUp(uint skillID, uint tp)
        //{
        //    Variant info = new Variant();
        //    info["skid"] = skillID;
        //    if (tp != 0)
        //    {
        //        info["tp"] = tp;
        //    }

        //    sendRPC(90, info);
        //}

        //public Variant skill_list = new Variant();

        //public Variant GetSkillData(uint skid)
        //{
        //    foreach (Variant obj in skill_list._arr)
        //    {
        //        if (skid == obj["skid"])
        //        {
        //            return obj;
        //        }
        //    }
        //    return null;
        //}

        //private Variant _manualSkills = null;
        //public Boolean IsManualSkill(uint sid)
        //{//判断是不是手动释放技能
        //    if (_manualSkills == null)
        //    {
        //        _manualSkills = muCLientConfig.instance.localGeneral.GetManualSkill();
        //    }

        //    for (int i = 0; i < _manualSkills.Length; ++i)
        //    {
        //        if (_manualSkills[i] == sid) return true;
        //    }
        //    return false;
        //}

        //public void SetSkillCD(Variant data, Variant cds)
        //{
        //    float curtm = muNetCleint.instance.CurServerTimeStampMS;
        //    float endtm = curtm + data["cdtm"] * 100;
        //    foreach (Variant skill in skill_list._arr)
        //    {
        //        if (cds.ContainsKey(skill["skid"]._str))
        //        {
        //            skill["cd"] = cds[skill["skid"]]["start_tm"] + cds[skill["skid"]]["cd_tm"] * 100;
        //        }
        //        else
        //        {
        //            skill["cd"] = endtm;
        //        }
        //    }
        //}
        //public Variant GetSkillById(uint skid)
        //{
        //    return skill_list[skid];
        //}
    }
}
