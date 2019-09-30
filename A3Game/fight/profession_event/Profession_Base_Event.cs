using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;
public class Profession_Base_Event : MonoBehaviour
{
    public ProfessionRole m_linkProfessionRole;
    public MonsterPlayer ohter_linkProfessionRole;
    //加入不能转身的时间no turn
    private void onNT(float time)
    {
        if(m_linkProfessionRole != null)
        m_linkProfessionRole.m_fSkillShowTime = time;
        if(ohter_linkProfessionRole != null)
        ohter_linkProfessionRole.m_fSkillShowTime = time;
    }
    //加入该技能的释放时间不能进行其他操作 onND
    private void onND(float time)
    {
        if (m_linkProfessionRole != null)
            m_linkProfessionRole.m_fAttackCount = time;
        if (ohter_linkProfessionRole != null)
            ohter_linkProfessionRole.m_fAttackCount = time;
    }

    private void onSL() //蓄力技能
    {
        if (m_linkProfessionRole != null)
            m_linkProfessionRole.m_nKeepSkillCount++;
        if (ohter_linkProfessionRole != null)
            ohter_linkProfessionRole.m_nKeepSkillCount++;
    }

    //加入翅膀的煽动变化
    public void onWing(float time)
    {
        if (m_linkProfessionRole != null)
            m_linkProfessionRole.FlyWing(time);
        if (ohter_linkProfessionRole != null)
            ohter_linkProfessionRole.FlyWing(time);
    }

    //加入地形特效
    private void onTFX(int id)
    {
        if (id < 100) //小于100的常规地形特效，100以上是怪物自身的地形特效需要特殊制作
        {
            if (m_linkProfessionRole.m_roleDta.m_WindID > 0)
                return;

            Vector3 born_pos;
            if (id >= 11 && id <= 20) //左脚
            {
                born_pos = m_linkProfessionRole.m_LeftFoot.position;
            }
            else if (id >= 21 && id <= 30) //右脚
            {
                born_pos = m_linkProfessionRole.m_RightFoot.position;
            }
            else if (id >= 31 && id <= 40) //左手
            {
                born_pos = m_linkProfessionRole.m_LeftHand.position;
            }
            else if (id >= 41 && id <= 50) //右手
            {
                born_pos = m_linkProfessionRole.m_RightHand.position;
            }
            else
            {
                born_pos = this.transform.position;
            }

            if (SceneTFX.m_TFX_Prefabs[id % 10] == null)
                return;

            if ( m_linkProfessionRole.invisible == false )
            {
                GameObject fx_inst = GameObject.Instantiate(SceneTFX.m_TFX_Prefabs[id % 10], born_pos, transform.rotation) as GameObject;
                fx_inst.transform.SetParent( U3DAPI.FX_POOL_TF , false );
                GameObject.Destroy( fx_inst , 1f );
            }
          
        }
    }

    //公用特效
    private void onFX(int id)
    {

    }

    //加入碰撞点
    public void onBullet(int id)
    {
        ////debug.Log("记录攻击点，这里可以做很多事情 id = " + id);
    }

    public void onShake(string param)
    {
        if (m_linkProfessionRole != null)
            if (!m_linkProfessionRole.m_isMain)
            return;

        string[] arr = param.Split(',');
        if (arr.Length < 3)
            return;

        SceneCamera.cameraShake(float.Parse(arr[0]), int.Parse(arr[1]), float.Parse(arr[2]));
    }

    public void onSound(string path)
    {
        if (m_linkProfessionRole != null)
        {
            if (m_linkProfessionRole.m_isMain)
                MediaClient.instance.PlaySoundUrl("audio_skill_" + path, false, null);
            else
            {
                MediaClient.instance.PlaySoundUrl("audio_skill_" + path, false, null, 0.4f);
            }
        }
    }

    public void onShow(int id)
    {
        if (m_linkProfessionRole != null)
            m_linkProfessionRole.ShowAll();
    }

    public void onHide(int id)
    {
        if (m_linkProfessionRole != null)
            m_linkProfessionRole.HideAll();
    }

    //自身特效
    private void onSFX(int id)
    {

    }
}
