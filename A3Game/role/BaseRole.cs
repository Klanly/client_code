using System;
using UnityEngine;
using System.Collections;
using MuGame;
using Cross;
using System.Collections.Generic;

public enum AI_TYPE
{
    MAIT_NONE = 0,
    MAIT_BORN = 1,
    MAIT_SICK = 2,
    MAIT_ATTACK = 3,
}

public class BaseRole : INameObj
{
    public bool havefanjibuff=false;
    public static int VIEW_TYPE_NONE = 0;
    public static int VIEW_TYPE_NAV = 1;
    public static int VIEW_TYPE_ALL = 2;

    public BaseRole m_LockRole; //锁定的目标
    public float m_LockDis = 13; //锁定距离
    public int m_circle_type = -1; //锁定的光圈类型，默认为-1，1为boss
    public float m_circle_scale = 1;

    public bool isfake = false; //前端，本地的模拟物
    public uint m_unIID = 0;
    public string _strIID = "";
    public string strIID
    {
        get
        {
            if (_strIID == "")
                _strIID = isfake ? ("fake" + m_unIID) : m_unIID.ToString();
            return _strIID;
        }
    }

    public uint m_unCID = 0;
    //public FIGHT_A3_SIDE m_eFight_Side = FIGHT_A3_SIDE.FA3S_NONE;
    public PK_TYPE m_ePK_Type = PK_TYPE.PK_PKALL;
    public uint m_unPK_Param = 0;

    public uint m_unTeamID = 0;//队伍id
    public uint m_unLegionID = 0;//军团id

    public uint lvlsideid = 0;//阵营id

    protected bool m_bFlyMonster = false; //如果是有翅膀类的飞行怪物，转身要特殊处理

    public float m_fSkillShowTime;
    protected float m_fDisposeTime = 0f; //如果这个值大于0，表示还不能删除

    public GameObject m_curGameObj;
    public GameObject M_curGameObj => m_curGameObj;

    public Transform m_curModel;
    public Transform m_curPhy;
    public Animator m_curAni;

    public NavMeshAgent m_moveAgent;

    public float m_fAttackCount;
    public int m_curSkillId;

    public Transform m_LeftHand; //可能没有值
    public Transform m_RightHand; //可能没有值
    public Transform m_LeftFoot; //可能没有值
    public Transform m_RightFoot; //可能没有值

    //几种基本的位移影响 sp(skill pos技能对位置的影响)  1击飞 // 2拉近 3推远
    public int m_nSPWeight = 1; //体重 --》 影响位移技能的效果
    public int m_nSPLevel = 1; //影响的级别
    public int m_nSkillSP_up; //技能的特殊影响 需要独立一个类出来专门处理
    public int m_nSkillSP_fb; //技能前后拉动
    public float m_fSkillSPup_Value;
    //public float m_fSkillSPup_Keep;
    public float m_fSkillSPfb_Value;
    public Vector3 m_vSkillSP_dir;

    //头衔
    public int heroTitleID = 0;
    public bool heroTitle_isShow = false;

    public bool m_isMain = false;//是不是自己
    protected AI_TYPE m_eThinkType = AI_TYPE.MAIT_NONE;
    public Vector3 headOffset;
    public Vector3 headOffset_half;
    private string _roleName = "";
    public string roleName { get { return _roleName; } set { _roleName = value; } }
    public SXML tempXMl;
    private int _curhp = 100;
    public int curhp { get { return _curhp; } set { _curhp = value; } }
    private int _maxHp = 100;
    public int maxHp { get { return _maxHp; } set { _maxHp = value; } }
    public bool isDead = false;
    public bool canbehurt = false;
    private int _title_id = 0;
    public int title_id { get { return _title_id; } set { _title_id = value; } }
    private bool _isactive = true;
    public bool isactive
    {
        get
        {
            return isactive;
        }
        set
        {
            _isactive = value;
            if (_isactive == false) 
                _title_id = 0;
        }
    }
    private int _rednm = 0;
    public int rednm { get { return _rednm; } set { _rednm = value; } }

    private int _lvlsideid = 0;
    public int spost_lvlsideid { get { return _lvlsideid; } set { _lvlsideid = value; } }

    public uint _hidbacktime = 0;
    public uint hidbacktime { get { return _hidbacktime; } set { _hidbacktime = value; } }





    protected int m_nNavPriority = 50;
    protected float m_fNavSpeed = 0.125f;
    protected float m_fNavStoppingDis = 1.5f;

    protected string m_strModelPath;
    public int viewType = 0;

    public bool m_bHide_state;
    public float m_fHideTime = 0f;

    public RoleItemData m_roleDta = new RoleItemData();

    public bool viewInScene = false;
    public int m_layer;

    public bool disposed = false;
    protected bool attackAvailable = true;
    public bool m_isMarked { get; set; } = false;

    public  Animator zuoji_Ani;

    //protected void Init(string prefab_path, int layer, Vector3 pos, float roatate = 0, bool isMain = false)
    //{
    //    m_strModelPath = prefab_path;
    //    m_layer = layer;
    //    m_roleDta.pos = pos;
    //    m_roleDta.rotate = roatate;
    //    m_isMain = isMain;

    //    m_curGameObj = U3DAPI.DEF_GAMEOBJ;
    //    m_curModel = U3DAPI.DEF_TRANSFORM;
    //    m_curPhy = U3DAPI.DEF_TRANSFORM;
    //    m_curAni = U3DAPI.DEF_ANIMATOR;
    //}

    //protected void RefreshModel()
    //{
    //    GameObject obj_prefab = GAMEAPI.ABFight_LoadPrefab(m_strModelPath);
    //    if (obj_prefab == null)
    //    {
    //        Debug.LogError("not find model = " + m_strModelPath);
    //        obj_prefab = U3DAPI.U3DResLoad<GameObject>("def_role");
    //    }

    //    m_curGameObj = GameObject.Instantiate(obj_prefab) as GameObject;

    //    foreach (Transform tran in m_curGameObj.GetComponentsInChildren<Transform>())
    //    {
    //        //debug.Log("改变了layer " + tran.name);
    //        tran.gameObject.layer = m_layer;// 更改物体的Layer层
    //    }

    //    m_curModel = m_curGameObj.transform.FindChild("model");
    //    m_curPhy = m_curModel.transform.FindChild("physics");
    //    try
    //    {
    //        m_curPhy.gameObject.layer = EnumLayer.LM_BT_FIGHT;
    //    }
    //    catch (System.Exception ex)
    //    {

    //    }

    //    m_curAni = m_curModel.GetComponent<Animator>();

    //    CapsuleCollider CapsuleCollider = m_curPhy.GetComponent<CapsuleCollider>();
    //    headOffset = CapsuleCollider.center;
    //    headOffset.y += CapsuleCollider.height / 2;
    //    headOffset_half = CapsuleCollider.center;
    //    //4足位置（人左右手+左右脚  牛 4四只脚）
    //    m_LeftHand = m_curModel.FindChild("L_Finger1");
    //    m_RightHand = m_curModel.FindChild("R_Finger1");
    //    m_LeftFoot = m_curModel.FindChild("L_Toe0");
    //    m_RightFoot = m_curModel.FindChild("R_Toe0");


    //    if (m_LeftHand == null) m_LeftHand = U3DAPI.DEF_TRANSFORM;
    //    if (m_RightHand == null) m_RightHand = U3DAPI.DEF_TRANSFORM;
    //    if (m_LeftFoot == null) m_LeftFoot = U3DAPI.DEF_TRANSFORM;
    //    if (m_RightFoot == null) m_RightFoot = U3DAPI.DEF_TRANSFORM;

    //    refreshViewType(viewType);
    //    //如断头 断尾要加效果的话要特殊处理

    //    //SelfHitPoint shp = m_curModel.gameObject.AddComponent<SelfHitPoint>();
    //    //shp.m_selfRole = this;

    //    //m_curPhy = m_curModel.FindChild("physics");
    //    //HurtPoint htt = m_curPhy.gameObject.AddComponent<HurtPoint>();
    //    //htt.m_selfRole = this;

    //    //PetBirdMgr.Inst.AttachPetBird(m_curModel);

    //    if (m_layer != EnumLayer.LM_SELFROLE && !(this is CollectRole) && TEMP_SHADOW != null)
    //    {
    //        GameObject shadowgo = GameObject.Instantiate(TEMP_SHADOW) as GameObject;
    //        shadowgo.transform.SetParent(m_curModel.transform, false);
    //        //当模型是有旋转时，调整影子的旋转反向
    //        Quaternion q = m_curModel.transform.localRotation;
    //        shadowgo.transform.localRotation = Quaternion.Inverse(q);
    //    }
    //}



    public void initNavMesh()
    {
        if (m_moveAgent != null)
        {
            m_moveAgent.enabled = true;
            return;
        }

        m_moveAgent = m_curModel.GetComponent<NavMeshAgent>();
        if (m_moveAgent == null)
            m_moveAgent = m_curModel.gameObject.AddComponent<NavMeshAgent>();

        //Debug.LogError("initNavMesh initNavMesh initNavMesh");

        m_moveAgent.stoppingDistance = m_fNavStoppingDis; //需要根据体型来调整
        m_moveAgent.speed = m_fNavSpeed;
        m_moveAgent.avoidancePriority = m_nNavPriority;
        m_moveAgent.angularSpeed = 360f;
    }

    public float scale
    {
        set
        {
            m_roleDta.scale = new Vector3(value, value, value);
            if (m_curGameObj != null)
            {
                m_curModel.transform.localScale = m_roleDta.scale;
            }

            if (m_moveAgent != null && m_moveAgent.enabled)
            {
                m_moveAgent.radius = 0.5f * value;
            }
        }

        get { return m_roleDta.scale.x; }
    }

    public static GameObject TEMP_SHADOW = null;

    public void refreshViewType(int type = 0)
    {
        if (type != 999) return;

        type = 2;

        if (m_curModel == null)
        {
            viewType = type;
            return;
        }

        if (type == VIEW_TYPE_NAV)
        {
            //if (m_isMain && type == VIEW_TYPE_NONE)
            //{
            //    refreshViewType(VIEW_TYPE_ALL);
            //    return;
            //}

            //viewType = VIEW_TYPE_NAV;

            //initNavMesh();
            //setNavLay(m_roleDta.wallkableMask);
            //setPos(m_roleDta.pos);

            //m_moveAgent.speed = 8f;
            ////    m_curModel.gameObject.SetActive(false);
        }
        else if (type == VIEW_TYPE_ALL)
        {
            initNavMesh();

            viewType = VIEW_TYPE_ALL;

            setNavLay(m_roleDta.wallkableMask);
            setPos(m_roleDta.pos);

            //     m_curModel.gameObject.SetActive(true);

            viewInScene = true;
            if (!isfake)
            {
                m_curAni.SetBool(EnumAni.ANI_B_BORNED, true);
                canbehurt = true;
            }

            if (m_roleDta.rotate != 0)
                m_curModel.eulerAngles = new Vector3(m_curModel.eulerAngles.x, m_roleDta.rotate, m_curModel.eulerAngles.z);

            scale = m_roleDta.scale.x;
            //m_moveAgent.speed = 0.125f;
        }
        else
        {
            viewType = VIEW_TYPE_NONE;

            if (m_moveAgent)
            {
                m_moveAgent.baseOffset = 0f;
                m_moveAgent.enabled = false;
            }
            //    m_curModel.gameObject.SetActive(false);

            // m_curModel.gameObject.SetActive(false);
        }

        onRefresh_ViewType();
    }

    protected virtual void onRefresh_ViewType()
    {

    }

    public void setNavLay(int idx)
    {
        if (m_moveAgent == null || m_moveAgent.enabled == false)
            m_roleDta.wallkableMask = idx;
        else
            m_moveAgent.walkableMask = idx;
    }

    public int lastHeadPosTick = 0;
    public Vector3 lastHeadPos = Vector3.zero;
    public UnityEngine.Vector3 getHeadPos()
    {
        if (m_curPhy == null || SceneCamera.m_curCamera == null)
            return UnityEngine.Vector3.zero;

        float tempx = 0;
        if (SelfRole._inst.m_curPhy.position.x > m_curPhy.position.x)
            tempx = SelfRole._inst.m_curPhy.position.x - m_curPhy.position.x;
        else
            tempx = m_curPhy.position.x - SelfRole._inst.m_curPhy.position.x;

        float tempy = 0;
        if (SelfRole._inst.m_curPhy.position.y > m_curPhy.position.y)
            tempy = SelfRole._inst.m_curPhy.position.y - m_curPhy.position.y;
        else
            tempy = m_curPhy.position.y - SelfRole._inst.m_curPhy.position.y;

        float temp = tempx + tempy;

        if (temp > 10f)
            return UnityEngine.Vector3.zero;

        int curTime = TickMgr.tickNum;

        if (lastHeadPosTick == curTime)
            return lastHeadPos;

        lastHeadPosTick = curTime;

        Vector3 v3 = m_curPhy.position + headOffset;

        if ( this.zuoji_Ani != null && this.m_moveAgent != null )
        {
            v3 = v3 + new Vector3(0f, this.m_moveAgent.baseOffset + 0.3f , 0f);
        }

        //if (InterfaceMgr.ui_Camera_cam == null)
        //    return UnityEngine.Vector3.zero;

        //if (InterfaceMgr.ui_Camera_cam != null && !InterfaceMgr.ui_Camera_cam.gameObject.active)
        //    InterfaceMgr.ui_Camera_cam.gameObject.SetActive(true);

        v3 = SceneCamera.m_curCamera.WorldToScreenPoint(v3);

        //v3 *= SceneCamera.m_fGameScreenPow;//需要根据game_screen 来缩放

        v3.z = 0f;
        lastHeadPos = v3;
        // lastHeadpos = v3;
        return v3;
    }

    public virtual void refreshViewData(Variant v)
    {
        //if (viewType != VIEW_TYPE_ALL)
        //  refreshViewType(VIEW_TYPE_ALL);
    }

    public void TurnToPos(Vector3 pos)
    {
        if (pos != Vector3.zero)
        {
            float next_x = pos.x - m_curModel.position.x;
            float next_y = pos.z - m_curModel.position.z;
            Vector3 vdir = new Vector3(next_x, 0, next_y);
            if (vdir != Vector3.zero)
                m_curModel.forward = vdir;

        }
    }

    public void setRoleRoatate(float r)
    {
        Vector3 ve = m_curModel.eulerAngles;
        ve.y = r;
        m_curModel.eulerAngles = ve;
    }
    //bool值判断一下攻击的人是不是我，
    public void TurnToRole(BaseRole r, bool ismyself)
    {
        if (ismyself)
            if (PkmodelAdmin.RefreshLockRoleTransform(r) == null) return;
            else
                if (r == null) return;


        if (r.m_curModel != null)
        {
            Vector3 pos = r.m_curModel.position;
            TurnToPos(pos);
        }
    }

    public virtual void onServerHurt(int damage, int hp, bool dead, BaseRole frm = null, int isCrit = -1, bool miss = false, bool stagger = false)
    {

    }
    public virtual void PlaySkill(int id)
    {

    }

    public void OtherSkillShow()
    {
        if (m_moveAgent != null)
        {
            m_moveAgent.updateRotation = false;
            //m_moveAgent.updatePosition = false;
        }
        m_fSkillShowTime = 1.4f;
    }

    public void modHp(int hp)
    {
        curhp = hp;
        PlayerNameUIMgr.getInstance().refreshHp(this, curhp, maxHp);
    }

    public void pos_correct(Vector3 pos)
    {
        if (Vector3.Distance(pos, m_curModel.position) > 15f)
        {
            m_curModel.GetComponent<NavMeshAgent>().enabled = false;
            m_curModel.position = pos;
            m_roleDta.pos = pos;
            m_curModel.GetComponent<NavMeshAgent>().enabled = true;
        }
        else
        {
            SetDestPos(pos);

        }
    }



    public void SetDestPos(Vector3 pos)
    {
        //debug.Log("移动的相应位置");
        //Vector3 pos = SelfRole._inst.m_curModel.position;
        //pos.x += UnityEngine.Random.Range(-8f, 8f);
        //pos.z += UnityEngine.Random.Range(-8f, 8f);

        //if (this is MonsterRole)
        //{
        //    if ((this as MonsterRole).issummon)
        //    {
        //        if ( Vector3.Distance(pos, m_curModel.position) > 15f)
        //        {
        //            m_curModel.GetComponent<NavMeshAgent>().enabled = false;
        //            m_curModel.position = pos;
        //            m_roleDta.pos = pos;
        //            m_curModel.GetComponent<NavMeshAgent>().enabled = true;
        //        }
        //    }
        //}

        bool beginMove = Vector3.Distance(m_roleDta.pos, m_curModel.position) < 1.5f;
        //debug.Log("::" + Vector3.Distance(m_roleDta.pos, m_curModel.position) + " " + m_roleDta.pos + " " + m_curModel.position);
        m_roleDta.pos = pos;
        if (viewType == VIEW_TYPE_NONE || m_moveAgent == null)
            return;

        if (isfake && m_eThinkType == AI_TYPE.MAIT_BORN)
        {
            m_roleDta.pos = pos;
            return;
        }


        if (!isDead)
        {
            if (m_moveAgent && m_moveAgent.enabled && m_moveAgent.isOnNavMesh)
            {
                NavMeshPath path = new NavMeshPath();
                m_moveAgent.CalculatePath(m_roleDta.pos, path);

                //修改自动寻路时朝向为路径中第一个坐标点。避免绕圈的问题。path.corners[0]是初始位置
                if (path.corners.Length > 1)
                    TurnToPos(path.corners[1]);

                if (m_isMain)
                {
                    NavMeshHit hit;
                    NavMesh.SamplePosition(pos, out hit, 100f, m_layer);
                    m_moveAgent.SetDestination(hit.position);
                    //debug.Log("SetDestination:" + pos + "  " + hit.position + "  " + m_moveAgent.destination);

                    //生成小地图导航坐标 zjw
                    //NavMeshPath path = new NavMeshPath();

                    m_moveAgent.CalculatePath(hit.position, path);
                    if (worldmap.instance != null)
                    {
                        worldmap.instance.DrawMapImage(path);
                    }
                    else
                    {
                        worldmap.Desmapimg();//其他自动寻路的时候消失掉已有路径
                    }

                }
                else
                {
                    m_moveAgent.SetDestination( pos );
                }

                if (m_fSkillShowTime > 0f)
                    return;

                //if (beginMove)
                //{
                //    if (false == m_bFlyMonster)
                //        TurnToPos(path.corners[0]);
                //}
                //else
                //{
                //    Quaternion rot = Quaternion.LookRotation(m_curModel.forward - pos.normalized);
                //    if (rot.eulerAngles.y > 30f)
                //    {
                //        if (false == m_bFlyMonster) TurnToPos(path.corners[0]);
                //    }
                //    else
                //    {
                //        //debug.Log("::" + rot.eulerAngles.y);
                //    }
                //}

            }
        }
    }




    public void setPos( Vector3 vec )
    {
        if (viewType == VIEW_TYPE_NONE)
        {
            m_roleDta.pos = vec;
            return;
        }

        NavMeshHit hit;

        if (NavMesh.SamplePosition(vec, out hit , 100f, NavmeshUtils.allARE))
        {
            if (m_moveAgent)
            {
                m_moveAgent.enabled = false;
                m_moveAgent.transform.position = hit.position;
                m_moveAgent.enabled = true;
            }

        }  
        //if (!m_isMain)
        //{
        //    SetDestPos(vec);
        //}
    }


    public virtual void dispose()
    {
        disposed = true;
        PlayerNameUIMgr.getInstance().hide(this);
        GameObject.Destroy(m_curGameObj);
        m_curGameObj = null;

        //todo 要所有的LockRole都遍历下，清除设置为null
        if (SelfRole._inst != null && SelfRole._inst.m_LockRole == this)
        {
            SelfRole._inst.m_LockRole = null;
        }
    }

    private Vector3 old_move_pos = Vector3.zero;
    private int checkMoveTick = 0;
    private int m_nAnimActiveCount = -3;

    //private Vector3 initPos;
    public virtual void FrameMove(float delta_time)
    {   
        if (m_moveAgent == null || m_curPhy == null || isDead)
            return;

        if (m_nAnimActiveCount < 0)
        {
            var animm = m_curModel.GetComponent<Animator>();
            if (animm != null)
            {
                if (m_nAnimActiveCount == -2)
                {
                }
                if (m_nAnimActiveCount == -1)
                {
                    animm.Rebind();
                    if (this is ProfessionRole || this is MonsterPlayer)
                    {
                        if ( zuoji_Ani != null )
                        {
                            animm.SetFloat( EnumAni.ANI_F_FLY , 2f );
                        }
                        else if (m_roleDta.m_WindID > 0)
                        {
                            animm.SetFloat(EnumAni.ANI_F_FLY, 1f);
                        }
                        else
                        {
                            animm.SetFloat(EnumAni.ANI_F_FLY, 0f);
                        }
                    }
                }


            }
            //if (m_nAnimActiveCount == -2) m_curModel.gameObject.SetActive(false);
            // if (m_nAnimActiveCount == -1) m_curModel.gameObject.SetActive(true);
            m_nAnimActiveCount++;
        }

        if (m_isMain)
        {
            if (m_fSkillShowTime > 0f)
            {
                m_fSkillShowTime -= delta_time;
            }

        }
        else
        {
            if (m_nSkillSP_up == 1)
            {
                if (m_fSkillSPup_Value > 0f)
                {
                    m_moveAgent.baseOffset += m_fSkillSPup_Value * 0.5f * m_nSPLevel;
                    m_fSkillSPup_Value -= delta_time;
                }
                else
                {
                    //if (m_fSkillSPup_Keep > 0f)
                    //{
                    //    m_fSkillSPup_Keep -= delta_time;
                    //}
                    //else
                    //{
                    m_moveAgent.baseOffset += m_fSkillSPup_Value * 0.5f * m_nSPLevel;
                    m_fSkillSPup_Value -= delta_time;
                    if (m_moveAgent.baseOffset <= 0f)
                    {
                        m_moveAgent.baseOffset = 0f;
                        m_nSkillSP_up = 0;
                    }
                    //}
                }
            }

            if (m_nSkillSP_fb != 0)
            {
                if (m_nSkillSP_fb == -21)
                {
                    if (m_fSkillSPfb_Value > 0f)
                    {
                        Vector3 dir = m_curModel.position - m_vSkillSP_dir;
                        dir.Normalize();

                        m_curModel.position -= dir * m_nSPLevel * m_fSkillSPfb_Value;
                        m_fSkillSPfb_Value -= delta_time;

                        //Vector3 offpos = m_curModel.position - m_vSkillSP_pos;
                        if (m_fSkillSPfb_Value <= 0f)
                        {
                            m_nSkillSP_fb = 0;
                        }
                    }
                }
                else if (m_nSkillSP_fb == -31)
                {
                    if (m_fSkillSPfb_Value > 0f)
                    {
                        m_curAni.enabled = false;
                        m_fSkillSPfb_Value -= delta_time;
                    }

                    if (m_fSkillSPfb_Value <= 0f)
                    {
                        m_curAni.enabled = true;
                        m_nSkillSP_fb = 0;
                    }
                }
                else if (m_nSkillSP_fb == -41)
                {
                    if (m_fSkillSPfb_Value > 0f)
                    {
                        Vector3 dir = m_curModel.position - m_vSkillSP_dir;
                        dir.Normalize();

                        m_curModel.position -= dir * delta_time * 3;
                        m_fSkillSPfb_Value -= delta_time;

                        //Vector3 offpos = m_curModel.position - m_vSkillSP_pos;
                        if (m_fSkillSPfb_Value <= 0f)
                        {
                            m_nSkillSP_fb = 0;
                        }
                    }
                }
                else
                {
                    if (m_fSkillSPfb_Value > 0f)
                    {
                        m_curModel.position += m_vSkillSP_dir * m_nSPLevel * m_fSkillSPfb_Value * m_nSkillSP_fb;
                        m_fSkillSPfb_Value -= delta_time;

                        if (m_fSkillSPfb_Value <= 0f)
                        {
                            m_nSkillSP_fb = 0;
                        }
                    }
                }
            }

            if (m_fSkillShowTime > 0f)
            {
                m_moveAgent.updateRotation = false;
                m_fSkillShowTime -= delta_time;
                //m_curModel.position = m_curPhy.position;
                m_curAni.SetBool(EnumAni.ANI_RUN, false);
            }
            else
            {
                m_moveAgent.updateRotation = true;
                if (m_moveAgent.enabled == false)
                    return;

                if (m_moveAgent.isOnNavMesh == false)
                    return;


                if (viewType == BaseRole.VIEW_TYPE_ALL && m_eThinkType == AI_TYPE.MAIT_NONE)
                {
                    //float dis = m_moveAgent.remainingDistance;
                    float dis = Vector3.Distance(m_moveAgent.destination.ConvertToGamePosition(), m_curModel.transform.position.ConvertToGamePosition());
                    if (false == m_bFlyMonster && m_moveAgent.speed > 0.125f)
                        m_moveAgent.speed = 0.125f;

                    if (dis > 0.93f /* 50 ÷ 53.33 */ )
                    {
                        //if (dis <= 0.93f)
                        //{
                        //m_curAni.SetBool(EnumAni.ANI_RUN, false);       
                        //m_moveAgent.updateRotation = false;
                        //}
                        //else
                        //{
                        if (m_moveAgent != null && !m_moveAgent.updateRotation)
                        {
                            m_moveAgent.updateRotation = true;
                            //m_moveAgent.updatePosition = true;

                            if (false == m_bFlyMonster)
                                m_moveAgent.speed = 0.125f;
                        }

                        #region never use anymore
                        //由z轴引起的 距离 误传 导致 m_moveAgent.remainingDistance 过来，所以添加一个参数来修复怪物和角色绕圈的问题
                        //if (old_move_pos == m_curModel.position)
                        //{
                        //    checkMoveTick++;
                        //    m_curAni.SetBool(EnumAni.ANI_RUN, false);
                        //}
                        //else
                        //    m_curAni.SetBool(EnumAni.ANI_RUN, true);
                        //if (checkMoveTick > 5)
                        //{
                        //    checkMoveTick = 0;
                        //    Vector3 pos = m_moveAgent.pathEndPosition;
                        //    Quaternion rot = Quaternion.LookRotation(m_curModel.forward - m_roleDta.pos);
                        //    if (rot.eulerAngles.y > 180f)
                        //        TurnToPos(m_roleDta.pos);
                        //    pos.z = m_moveAgent.pathEndPosition.z + 0.3f;
                        //    m_moveAgent.SetDestination(m_curModel.position);
                        //}
                        //old_move_pos = m_curModel.position;
                        #endregion

                        if ( zuoji_Ani != null )
                        {
                            zuoji_Ani.SetBool( EnumAni.ANI_T_RIDERUN , true );
                        }
                        m_curAni.SetBool(EnumAni.ANI_RUN, true);

                        if (!m_isMain)
                            SetDestPos(m_moveAgent.destination);
                        //}
                    }
                    else
                    {
                        //if ( dis < 0.3f )
                        //{
                            if ( zuoji_Ani != null )
                            {
                                zuoji_Ani.SetBool( EnumAni.ANI_T_RIDERUN , false );
                            }
                            //SetDestPos(m_curModel.position);
                            m_curAni.SetBool( EnumAni.ANI_RUN , false );

                        //}

                        //m_moveAgent.SetDestination(m_curModel.position);
                        //m_moveAgent.updateRotation = false;
                    }
                }
            }
        }
    }

}


public class RoleItemData
{
    public Vector3 pos;
    public Vector3 scale = Vector3.one;
    public float rotate;

    public int m_Weapon_LID;
    public int m_Weapon_LFXID;
    public int m_Weapon_RID;
    public int m_Weapon_RFXID;
    public int m_WindID;
    public int m_WingFXID;
    public int m_BodyID;
    public int m_BodyFXID;
    public uint m_EquipColorID;
    public int wallkableMask;
    public int carr;
    public bool add_eff;
    public int activecount;
}

public static class PositionExt
{
    public static Vector3 ConvertToGamePosition(this Vector3 transform)
    {
        return new Vector3(transform.x, 0, transform.z);
    }
}