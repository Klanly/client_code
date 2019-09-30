using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;





public class MonsterRole : BaseRole
{
    private bool m_bWaitingModel = true;
    private Vector3 bornPos;
    public Vector3 BornPos => bornPos;
    private bool canTestDis;//检测距离
    //public int roleId;
    public int monsterid;
    protected float m_fThinkTime = 0.25f;//UnityEngine.Random.Range(1f, 4f); //开始出生的时间

    protected float m_fFreezeTime = 0f;  //施放大招时的冻结时间
    protected float m_fFreezeSpeed = 0f;
    protected bool m_bdoFreeze = false;

    //protected int m_TurnToTargetCount = 0;
    //public AI_TYPE m_eThinkType = AI_TYPE.MAIT_NONE;

    public bool isBoos = false;
    public bool isBoss_c = false; //用来区分是否可以被击飞等受击

    protected float m_fAttackAngle = 16f; //如果这个怪前面90度都可以攻击到，那值是45 一般精确点，像60度的就是30

    public bool ismapfx = false;
    public Vector3 fxvec = Vector3.zero;
    public bool issummon = false;
    public int summonid = 0;
    public bool isDart = false;
    public int dartid = 0;
    //public BaseRole master = null;
    public uint masterid = 0;
    protected SkinnedMeshRenderer m_MonBody;
    protected SkinnedMeshRenderer m_MonBody1; //处理两个body的情况
    protected Color m_Main_Color;
    protected Color m_Rim_Color;
    protected float m_Rim_Width;
    private float m_fhitFlash_time = 0f;
    private bool m_bhitFlashGoUp = false;
    private bool m_bDoHitFlash = false;

    private bool m_bchangeDeadMtl = false;
    private bool m_bchangeLiveMtl = false;
    public int born_type = 0;
    private float m_dead_Burn = 0f;
    private float m_live_Burn = 1f;
    private Material m_mat;
    public Material body_m_mat {
        get { return m_mat; }
    }
    public bool m_remove_after_dead = false; //死亡溶解播放结束后是否移除

    private float height;
    private float radius;
    private bool isDartRole = false;//用来判断镖车
    public string ownerName;

    //这里存一些初始化的数据，由于异步加载后初始化存在问题，得先保存一些协议数据，待模型加载完成后初始化
    public bool invisible = true;
    public bool to_moving = false;
    public float to_x = 0;
    public float to_y = 0;

    virtual public void Init(string prefab_path, int layer, Vector3 pos, float roatate = 0)
    {
        m_strModelPath = prefab_path;
        m_layer = layer;
        m_roleDta.pos = pos;
        m_roleDta.rotate = roatate;
        m_isMain = false;

        m_curGameObj = U3DAPI.DEF_GAMEOBJ;
        m_curModel = U3DAPI.DEF_TRANSFORM;
        m_curPhy = U3DAPI.DEF_TRANSFORM;
        m_curAni = U3DAPI.DEF_ANIMATOR;

        GAMEAPI.ABModel_LoadGameObject(m_strModelPath, Model_LoadedOK, null);
    }

    virtual protected void Model_Loaded_Over()
    {

    }

    private void Model_LoadedOK(UnityEngine.Object model_obj, System.Object data)
    {
        if (!MonsterMgr._inst.m_mapMonster.ContainsKey(m_unIID) && !MonsterMgr._inst.m_mapFakeMonster.ContainsKey(m_unIID))
            return;

        m_bWaitingModel = false;

        GameObject obj_prefab = model_obj as GameObject;
        if (obj_prefab == U3DAPI.DEF_GAMEOBJ || obj_prefab == null)
        {
            Debug.LogError("not find model = " + m_strModelPath);
            obj_prefab = U3DAPI.U3DResLoad<GameObject>("def_role");
        }

        m_curGameObj = GameObject.Instantiate(obj_prefab) as GameObject;

        foreach (Transform tran in m_curGameObj.GetComponentsInChildren<Transform>())
        {
            //debug.Log("改变了layer " + tran.name);
            tran.gameObject.layer = m_layer;// 更改物体的Layer层
        }

        m_curModel = m_curGameObj.transform.FindChild("model");
        m_curPhy = m_curModel.transform.FindChild("physics");
        try
        {
            m_curPhy.gameObject.layer = EnumLayer.LM_BT_FIGHT;
        }
        catch (System.Exception ex)
        {

        }

        m_curAni = m_curModel.GetComponent<Animator>();

        CapsuleCollider CapsuleCollider = m_curPhy.GetComponent<CapsuleCollider>();
        headOffset = CapsuleCollider.center;
        headOffset.y += CapsuleCollider.height / 2;
        headOffset_half = CapsuleCollider.center;

        //4足位置（人左右手+左右脚  牛 4四只脚）
        m_LeftHand = m_curModel.FindChild("L_Finger1");
        m_RightHand = m_curModel.FindChild("R_Finger1");
        m_LeftFoot = m_curModel.FindChild("L_Toe0");
        m_RightFoot = m_curModel.FindChild("R_Toe0");

        if (m_LeftHand == null) m_LeftHand = U3DAPI.DEF_TRANSFORM;
        if (m_RightHand == null) m_RightHand = U3DAPI.DEF_TRANSFORM;
        if (m_LeftFoot == null) m_LeftFoot = U3DAPI.DEF_TRANSFORM;
        if (m_RightFoot == null) m_RightFoot = U3DAPI.DEF_TRANSFORM;

        //refreshViewType(viewType);
        //如断头 断尾要加效果的话要特殊处理

        //SelfHitPoint shp = m_curModel.gameObject.AddComponent<SelfHitPoint>();
        //shp.m_selfRole = this;

        //m_curPhy = m_curModel.FindChild("physics");
        //HurtPoint htt = m_curPhy.gameObject.AddComponent<HurtPoint>();
        //htt.m_selfRole = this;

        //PetBirdMgr.Inst.AttachPetBird(m_curModel);

        if (m_layer != EnumLayer.LM_SELFROLE && !(this is CollectRole) && TEMP_SHADOW != null)
        {
            GameObject shadowgo = GameObject.Instantiate(TEMP_SHADOW) as GameObject;
            shadowgo.transform.SetParent(m_curModel.transform, false);
            //当模型是有旋转时，调整影子的旋转反向
            Quaternion q = m_curModel.transform.localRotation;
            shadowgo.transform.localRotation = Quaternion.Inverse(q);
        }



        m_unLegionID = (uint)LOGION_DEF.LNDF_MONSTER;
        //军团ID=怪物（uint=2）
        m_curGameObj.SetActive(true);

        MonHurtPoint mhtt = m_curPhy.gameObject.AddComponent<MonHurtPoint>();
        mhtt.m_monRole = this;

        setNavLay(NavmeshUtils.allARE);

        //  m_curAgent.enabled = false;

        if (this is MonsterPlayer)
            PlayerNameUIMgr.getInstance().show(this);

        Transform mon_body = m_curModel.FindChild("body");
        if (mon_body != null)
        {
            m_MonBody = mon_body.GetComponent<SkinnedMeshRenderer>();
            if (m_MonBody.material.HasProperty(EnumShader.SPI_COLOR))
                m_Main_Color = m_MonBody.material.GetColor(EnumShader.SPI_COLOR);
            if (m_MonBody.material.HasProperty(EnumShader.SPI_RIMCOLOR))
                m_Rim_Color = m_MonBody.material.GetColor(EnumShader.SPI_RIMCOLOR);
            if (m_MonBody.material.HasProperty(EnumShader.SPI_RIMWIDTH))
                m_Rim_Width = m_MonBody.material.GetFloat(EnumShader.SPI_RIMWIDTH);

            m_mat = m_MonBody.material;
        }

        if (m_MonBody == null) m_MonBody = U3DAPI.DEF_SKINNEDMESHRENDERER;
        Transform mon_body1 = m_curModel.FindChild("body1");
        if (mon_body1 != null)
        {
            m_MonBody1 = mon_body1.GetComponent<SkinnedMeshRenderer>();
        }

        //m_fNavStoppingDis = 0.125f;
        if (tempXMl != null && tempXMl.getInt("born_eff") > 0)
        {
            //出生时材质变化
            onBornStart(tempXMl.getInt("born_eff"));
        }
        height = m_curModel.FindChild("physics").GetComponent<CapsuleCollider>().height;
        radius = m_curModel.FindChild("physics").GetComponent<CapsuleCollider>().radius;

        refreshViewType(999);

        //协议过来的一些初始化内容
        m_curGameObj.SetActive(invisible & m_curGameObj.activeSelf);
        if (to_moving)
        {
            NavMeshHit hit;
            Vector3 vec = new Vector3(to_x * 32 / GameConstant.PIXEL_TRANS_UNITYPOS, 0f, to_y * 32 / GameConstant.PIXEL_TRANS_UNITYPOS);
            NavMesh.SamplePosition(vec, out hit, 100f, m_layer);
            SetDestPos(hit.position);
        }

        Model_Loaded_Over();
    }

    public virtual void onClick()
    {

    }

    public bool hasBorned
    {
        get { return m_eThinkType != AI_TYPE.MAIT_BORN; }
    }


    public HitData Build_PRBullet(uint skillid, float t, GameObject original, Vector3 position, Quaternion rotation)
    {
        GameObject bult = GameObject.Instantiate(original, position, rotation) as GameObject;
        bult.transform.SetParent(U3DAPI.FX_POOL_TF, false);
        HitData hd = bult.gameObject.AddComponent<HitData>();

        hd.m_hdRootObj = bult;
        hd.m_CastRole = this;
        hd.m_vBornerPos = m_curModel.position;

        hd.m_ePK_Type = m_ePK_Type;
        switch (m_ePK_Type)
        {
            case PK_TYPE.PK_TEAM: hd.m_unPK_Param = m_unTeamID; break;
            case PK_TYPE.PK_LEGION: hd.m_unPK_Param = m_unLegionID; break;
            case PK_TYPE.PK_PKALL: hd.m_unPK_Param = m_unCID; break;
        }

        hd.m_unSkillID = skillid;
        hd.m_nDamage = 100;
        hd.m_nHitType = 0;

        bult.layer = EnumLayer.LM_BT_FIGHT;

        if (t > 0f)
            GameObject.Destroy(bult, t);

        //可以用一个float 来记录最后一个子弹的时间，以后释放role的时候，可以通过这个考虑延时
        if (m_fDisposeTime < t) m_fDisposeTime = t;

        return hd;
    }
    public HitData Link_PRBullet(uint skillid, float t, GameObject root, Transform linker)
    {
        HitData hd = linker.gameObject.AddComponent<HitData>();

        hd.m_hdRootObj = root;
        hd.m_CastRole = this;
        hd.m_vBornerPos = m_curModel.position;

        hd.m_ePK_Type = m_ePK_Type;
        switch (m_ePK_Type)
        {
            case PK_TYPE.PK_TEAM: hd.m_unPK_Param = m_unTeamID; break;
            case PK_TYPE.PK_LEGION: hd.m_unPK_Param = m_unLegionID; break;
            case PK_TYPE.PK_PKALL: hd.m_unPK_Param = m_unCID; break;
        }

        hd.m_unSkillID = skillid;
        hd.m_nDamage = 100;
        hd.m_nHitType = 0;

        linker.gameObject.layer = EnumLayer.LM_BT_FIGHT;
        GameObject.Destroy(root, t);

        //可以用一个float 来记录最后一个子弹的时间，以后释放role的时候，可以通过这个考虑延时
        if (m_fDisposeTime < t) m_fDisposeTime = t;

        return hd;
    }
    public override void onServerHurt(int damage, int hp, bool dead, BaseRole frm = null, int isCrit = -1, bool miss = false, bool stagger = false)
    {
        if (isDead) return;
        curhp = hp;

        //if (ConfigUtil.getRandom(1, 10) > 3)
        //    PlayHurt();



        if (dead)
        {
            onDead();
        }

        if (!miss)
        {
            PlayerNameUIMgr.getInstance().refreshHp(this, curhp, maxHp);
            if (this is MS0000 && (this as MS0000).owner_cid == PlayerModel.getInstance().cid)
            {
                if (a3_herohead.instance)
                {
                    a3_herohead.instance.refresh_sumHp(curhp, maxHp);
                }
            }
        }

        if (frm != null && SelfRole._inst.m_unIID == frm.m_unIID)
        {
            FightText.CurrentCauseRole = frm;
            switch (isCrit)
            {
                case -1: FightText.play(FightText.userText, getHeadPos(), damage); break;
                case 0: break;
                case 1: FightText.play(FightText.userText, getHeadPos(), damage, false, -1, null, this); break;
                case 2: FightText.play(FightText.IMG_TEXT, getHeadPos(), damage, false, isCrit, null, this); break;
                case 3: FightText.play(FightText.userText, getHeadPos(), damage, false, -1, null, this); break;
                case 4: FightText.play(FightText.userText, getHeadPos(), damage, false, -1, null, this); break;
                case 5: FightText.play(FightText.userText, getHeadPos(), damage, false, -1, null, this); break;
                case 6: FightText.play(FightText.IMG_TEXT_2, getHeadPos(), damage, false, isCrit, null, this); break;
                default: FightText.play(FightText.IMG_TEXT, getHeadPos(), damage, false, isCrit, null, this); break;
            }
        }
        if (frm is MS0000 && ((MS0000)frm).owner_cid == PlayerModel.getInstance().cid)
        {
            FightText.CurrentCauseRole = frm;
            switch (isCrit)
            {
                case -1: FightText.play(FightText.userText, getHeadPos(), damage, false, -1, null, this); break;
                case 0: break;
                case 1: FightText.play(FightText.ZHS_TEXT_PG, getHeadPos(), damage, false, -1, null, this); break;
                case 2: FightText.play(FightText.ZHS_TEXT_ZM, getHeadPos(), damage, false, isCrit, null, this); break;
                case 3: FightText.play(FightText.ZHS_TEXT_PG, getHeadPos(), damage, false, -1, null, this); break;
                case 4: FightText.play(FightText.ZHS_TEXT_PG, getHeadPos(), damage, false, -1, null, this); break;
                case 5: FightText.play(FightText.ZHS_TEXT_PG, getHeadPos(), damage, false, -1, null, this); break;
                case 6: FightText.play(FightText.ZHS_TEXT_PG, getHeadPos(), damage, false, -1, null, this); break;
                default: FightText.play(FightText.IMG_TEXT, getHeadPos(), damage, false, isCrit, null, this); break;
            }
        }
    }

    public void flyskill(string skill)
    {
        FightText.play(FightText.ZHS_TEXT_JN, this.getHeadPos(), 0, false, -1, skill, this);
    }

    public void setHitFlash(HitData hd)  //受伤变色
    {
        if (!(this is MonsterPlayer))
        {
            m_bhitFlashGoUp = true;
            m_bDoHitFlash = true;

            //setColor(hitxml.mainColor, hitxml.rimColor);

            m_MonBody.material.SetColor(EnumShader.SPI_COLOR, hd.m_Color_Main);
            m_MonBody.material.SetColor(EnumShader.SPI_RIMCOLOR, hd.m_Color_Rim);

            if (m_MonBody1 != null)
            {
                m_MonBody1.material.SetColor(EnumShader.SPI_COLOR, hd.m_Color_Main);
                m_MonBody1.material.SetColor(EnumShader.SPI_RIMCOLOR, hd.m_Color_Rim);
            }
        }
    }

    public void AI_Sick() //追击主角
    {
        SetDestPos(SelfRole._inst.m_curModel.position);

        if (m_moveAgent.isOnNavMesh && m_moveAgent.remainingDistance > m_fNavStoppingDis)
        {

        }
        else
        {
            m_eThinkType = AI_TYPE.MAIT_ATTACK;
            m_curAni.SetBool(EnumAni.ANI_RUN, false);

            m_moveAgent.updateRotation = false;
            //  m_curAgent.speed = 0f;
        }

        //SetDestPos(SelfRole._inst.m_curModel.position);
        //if (m_moveAgent.remainingDistance < m_fNavStoppingDis)
        //{
        //    attackAvailable = true;
        //    m_eThinkType = AI_TYPE.MAIT_ATTACK;
        //    m_curAni.SetBool(EnumAni.ANI_RUN, false);
        //    m_moveAgent.updateRotation = false;
        //    return;
        //}

        // -- 正常这里应该有一个追击行为,但和BaseRole中的FrameMove末尾部分有冲突
        // -- 所以暂时注释,若今后对BaseRole重构时,考虑此部分的代码
        // -- BaseRole.cs line 785:m_curAni.SetBool(EnumAni.ANI_RUN, true);
        //else if (m_moveAgent.remainingDistance > MonsterMgr._inst.GetTraceRange(monsterid))
        //{
        //    attackAvailable = false;
        //    m_moveAgent.Stop();
        //    m_eThinkType = AI_TYPE.MAIT_NONE;
        //    SetDestPos(bornPos);
        //    if (!m_curAni.GetBool(EnumAni.ANI_RUN))
        //        m_curAni.SetBool(EnumAni.ANI_RUN, true);            
        //}

    }




    virtual protected void EnterAttackState() //进入攻击状态
    {
        m_curAni.SetBool(EnumAni.ANI_ATTACK, true);
    }

    virtual protected void LeaveAttackState() //离开攻击状态
    {
        m_curAni.SetBool(EnumAni.ANI_ATTACK, false);
    }

    public void AI_Attack(float delta_time)//攻击
    {
        if (!attackAvailable)
            return;
        Vector3 dir = SelfRole._inst.m_curModel.position - m_curModel.position;

        if (dir.magnitude > 4f)
        {
            m_eThinkType = AI_TYPE.MAIT_SICK;

            m_curAni.SetBool(EnumAni.ANI_RUN, true);
            return;
        }

        //if (dir.magnitude > 1f && !isfake)
        //{
        //    m_eThinkType = AI_TYPE.MAIT_SICK;
        //    //    m_curAgent.updateRotation = true;
        //    //   m_curAgent.speed = m_fNavSpeed;
        //    //if (dir.magnitude > 3f)
        //    //{
        //    //}
        //    m_curAni.SetBool(EnumAni.ANI_RUN, true); //Modified
        //    //LeaveAttackState();

        //    //m_curAni.SetBool(EnumAni.ANI_TURN_L, false);
        //    //m_curAni.SetBool(EnumAni.ANI_TURN_R, false);
        //    return;
        //}
        //if (m_bFlyMonster)
        //{

        //}
        //else
        //{
        //TurnToRole(SelfRole._inst);

        Vector3 fw = m_curModel.forward;
        fw.y = 0;

        float fangle = Vector3.Angle(fw, dir);
        bool bcannot_attack = true;


        //屏蔽掉转身
        if (fangle < m_fAttackAngle) //如果这个怪前面90度都可以攻击到是45 一般精确点，像60度的就是30
        {
            //debug.Log("前");
            EnterAttackState();
            m_fAttackCount = 1f;
            bcannot_attack = false;
        }

        if (bcannot_attack)
        {
            Vector3 fr = m_curModel.right;
            fr.y = 0;

            //转身也有动作控制，就解决这里的问题了，测试下
            LeaveAttackState();

            fangle = Vector3.Angle(fr, dir);
            if (fangle < 90f)
            {
                //debug.Log("右");
                m_curModel.transform.Rotate(Vector3.up, 256f * delta_time);
            }
            else
            {
                //debug.Log("左");
                m_curModel.transform.Rotate(Vector3.up, -256f * delta_time);
            }
        }
    }
    #region
    //EnterAttackState();
    //m_fAttackCount = 1f;


    //  dir.y = 0;

    //m_curAni.SetBool(EnumAni.ANI_RUN, false);
    ////dir.Normalize();

    //Vector3 fw = m_curModel.forward;
    //fw.y = 0;
    ////fw.Normalize();

    //float fangle = Vector3.Angle(fw, dir);
    //bool bcannot_attack = true;
    //if (fangle < m_fAttackAngle) //如果这个怪前面90度都可以攻击到是45 一般精确点，像60度的就是30
    //{
    //    //debug.Log("前");
    //    EnterAttackState();
    //    bcannot_attack = false;
    //}

    //if (bcannot_attack)
    //{
    //    Vector3 fr = m_curModel.right;
    //    fr.y = 0;

    //    //转身也有动作控制，就解决这里的问题了，测试下

    //    LeaveAttackState();
    //    fangle = Vector3.Angle(fr, dir);
    //    if (fangle < 90f)
    //    {
    //        //debug.Log("右");
    //        m_TurnToTargetCount = 1;
    //    }
    //    else
    //    {
    //        //debug.Log("左");
    //        m_TurnToTargetCount = -1;
    //    }
    //}

    //if (m_TurnToTargetCount > 0)
    //{
    //    m_TurnToTargetCount--;

    //    if (m_bFlyMonster)
    //    {
    //        m_curModel.transform.Rotate(Vector3.up, 128f * delta_time);
    //    }
    //    else
    //    {
    //        m_curAni.SetBool(EnumAni.ANI_TURN_R, true);
    //    }
    //}
    //else
    //{
    //    m_curAni.SetBool(EnumAni.ANI_TURN_R, false);
    //}

    //if (m_TurnToTargetCount < 0)
    //{
    //    m_TurnToTargetCount++;

    //    if (m_bFlyMonster)
    //    {
    //        m_curModel.transform.Rotate(Vector3.up, -128f * delta_time);
    //    }
    //    else
    //    {
    //        m_curAni.SetBool(EnumAni.ANI_TURN_L, true);
    //    }
    //}
    //else
    //{
    //    m_curAni.SetBool(EnumAni.ANI_TURN_L, false);
    //}
    #endregion


    public HitData BuildBullet(uint skillid, float t, GameObject original, Vector3 position, Quaternion rotation)
    {
        GameObject bult = GameObject.Instantiate(original, position, rotation) as GameObject;
        bult.transform.SetParent(U3DAPI.FX_POOL_TF, false);

        HitData hd = bult.gameObject.AddComponent<HitData>();
        hd.m_vBornerPos = m_curModel.position;
        hd.m_CastRole = this;
        hd.m_ePK_Type = PK_TYPE.PK_LEGION;
        hd.m_unPK_Param = m_unLegionID;
        hd.m_unSkillID = skillid;
        hd.m_nDamage = 100;
        hd.m_nHitType = 0;

        bult.layer = EnumLayer.LM_BT_FIGHT;
        GameObject.Destroy(bult, t);

        //可以用一个float 来记录最后一个子弹的时间，以后释放role的时候，可以通过这个考虑延时
        if (m_fDisposeTime < t) m_fDisposeTime = t;

        return hd;
    }
    //TickItem BulletFly;
    public HitData longBullet(uint skillid, float t, GameObject original, Transform linker)
    {
        HitData hd = linker.gameObject.AddComponent<HitData>();
        hd.m_hdRootObj = original;
        hd.m_vBornerPos = m_curModel.position;
        hd.m_CastRole = this;
        hd.m_ePK_Type = PK_TYPE.PK_LEGION;
        hd.m_unPK_Param = m_unLegionID;
        hd.m_unSkillID = skillid;
        hd.m_nDamage = 100;
        hd.m_nHitType = 0;

        linker.gameObject.layer = EnumLayer.LM_BT_FIGHT;
        GameObject.Destroy(original, t);
        //if (BulletFly == null && original!= null)
        //{
        //    BulletFly = new TickItem(onfly);
        //    TickMgr.instance.addTick(BulletFly);
        //}
        if (m_fDisposeTime < t) m_fDisposeTime = t;

        return hd;
    }
    void onfly(float s)
    {

    }

    public void onHurt(HitData hd)
    {
        if (isDead)
            return;

        //受伤变色
        setHitFlash(hd);

        //播放受击特效
        if (hd.m_nHurtFX > 0 && hd.m_nHurtFX < 10)
        {
            if (hd.m_nHurtFX == 6)
            {//受击为击退和眩晕时特效特殊处理
                Vector3 pos = Vector3.zero;
                pos.y = pos.y + headOffset.y;
                GameObject fx_inst = GameObject.Instantiate(SceneTFX.m_HFX_Prefabs[hd.m_nHurtFX], pos, m_curModel.rotation) as GameObject;
                fx_inst.transform.SetParent(m_curModel, false);
                GameObject.Destroy(fx_inst, 2f);
            }
            else
            {
                GameObject fx_inst = GameObject.Instantiate(SceneTFX.m_HFX_Prefabs[hd.m_nHurtFX], m_curModel.position, m_curModel.rotation) as GameObject;
                fx_inst.transform.SetParent(U3DAPI.FX_POOL_TF, false);
                GameObject.Destroy(fx_inst, 2f);
            }

        }

        if (hd.m_nDamage == 0)
            return;

        curhp -= hd.m_nDamage;
        if (curhp < 0)
            curhp = 0;

        PlayerNameUIMgr.getInstance().refreshHp(this, curhp, maxHp);
        if (this is MS0000 && (this as MS0000).owner_cid == PlayerModel.getInstance().cid)
        {
            if (a3_herohead.instance)
            {
                a3_herohead.instance.refresh_sumHp(curhp, maxHp);
            }
        }

        if (SelfRole._inst.m_unIID == hd.m_CastRole.m_unIID)
        {
            FightText.play(FightText.userText, lastHeadPos, hd.m_nDamage);
        }


        if (curhp == 0)
        {
            //去掉碰撞体
            m_curPhy.gameObject.SetActive(false);

            onDead();
        }
        else
        {
            PlayHurtFront();
        }
    }

    public virtual void onDeadEnd()//替换死亡后的材质
    {
        if (m_MonBody != null)
        {
            SkinnedMeshRenderer[] curSMRs = m_MonBody.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < curSMRs.Length; i++)
            {
                Material mtl_inst = GameObject.Instantiate(gameST.DEAD_MTL) as Material;
                Texture tex = curSMRs[i].material.GetTexture(gameST.MTL_Main_Tex);
                mtl_inst.SetTexture(gameST.MTL_Dead_Tex, tex);

                curSMRs[i].material = mtl_inst;
            }

            m_bchangeDeadMtl = true;
        }
    }

    //出生的材质变化
    public virtual void onBornStart(int type)
    {
        if (m_MonBody != null)
        {
            Material born_mtl = U3DAPI.U3DResLoad<Material>("mtl/born_mtl" + type);
            SkinnedMeshRenderer[] curSMRs = m_MonBody.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < curSMRs.Length; i++)
            {
                Material mtl_inst = GameObject.Instantiate(born_mtl) as Material;
                Texture tex = curSMRs[i].material.GetTexture(gameST.MTL_Main_Tex);
                mtl_inst.SetTexture(gameST.MTL_Dead_Tex, tex);

                curSMRs[i].material = mtl_inst;
            }
            m_MonBody.material.SetFloat(gameST.DEAD_MT_AMOUNT, m_live_Burn);
            m_bchangeLiveMtl = true;
        }
    }

    //出生材质显示效果结束后重置材质
    public void onResetMTL()
    {
        if (m_MonBody != null)
        {
            SkinnedMeshRenderer[] curSMRs = m_MonBody.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < curSMRs.Length; i++)
            {
                curSMRs[i].material = m_mat;
            }
        }

        if (m_Main_Color != null)
            m_MonBody.material.SetColor(EnumShader.SPI_COLOR, m_Main_Color);
        else
            m_MonBody.material.SetColor(EnumShader.SPI_COLOR, Color.white);
        if (m_Rim_Color != null)
            m_MonBody.material.SetColor(EnumShader.SPI_RIMCOLOR, m_Rim_Color);
        else
            m_MonBody.material.SetColor(EnumShader.SPI_RIMCOLOR, Color.black);

        m_MonBody.material.SetFloat(EnumShader.SPI_RIMWIDTH, m_Rim_Width);
    }


    public void onDead()
    {

        if (isDead == false)
        {
            isDead = true;
            if (isfake)
            {
                int len = ConfigUtil.getRandom(1, 3);
                if (len > 0)
                {
                    List<DropItemdta> l = new List<DropItemdta>();
                    for (int i = 0; i < len; i++)
                    {
                        DropItemdta d = new DropItemdta();
                        d.dpid = 0;
                        d.count = ConfigUtil.getRandom(1, 10);
                        l.Add(d);
                    }
                    // BaseRoomItem.instance.showDropItem(m_curModel.position, l, true);
                }
            }

            if (m_curAni)
            {
                m_curAni.enabled = true;
                m_curAni.SetBool(EnumAni.ANI_B_DIE, true);
            }

            if (m_curPhy)
                m_curPhy.gameObject.SetActive(false);

            if (m_moveAgent)
            {
                m_moveAgent.baseOffset = 0f;
                m_moveAgent.enabled = false;
            }

            PlayerNameUIMgr.getInstance().hide(this);

            //记录杀敌数
            if (MapModel.getInstance().curLevelId != 0)
            {
                if (MapModel.getInstance().dFbDta.ContainsKey((int)MapModel.getInstance().curLevelId))
                {
                    MapModel.getInstance().dFbDta[(int)MapModel.getInstance().curLevelId].kmNum++;
                }
            }
            if (GameRoomMgr.getInstance().curRoom != null) GameRoomMgr.getInstance().curRoom.onMonsterDied(this);
        }
    }

    public void onBorned()
    {
        PlayerNameUIMgr.getInstance().show(this);
        if (m_eThinkType != AI_TYPE.MAIT_NONE)
            m_eThinkType = AI_TYPE.MAIT_SICK;
        canbehurt = true;
        if (m_moveAgent != null)
            m_moveAgent.enabled = true;
        //SetDestPos(m_roleDta.pos);
        bornPos = m_moveAgent.transform.position;//m_roleDta.pos;
    }

    public void SleepAI(float time)
    {
        m_fThinkTime = time;
    }

    public void FreezeAni(float time, float speed)
    {
        m_fFreezeTime = time;
        m_fFreezeSpeed = speed;
        m_bdoFreeze = true;

    }

    public override void PlaySkill(int id)
    {
        if (m_curSkillId != 0)
            return;
        m_fAttackCount = 0.5f;
        if ((id > 10000 && id < 15000)||id==1)
        {//普攻技能
            //OtherSkillShow();
            EnterAttackState();
        }
        else
        {
            m_curSkillId = id;
            m_curAni.SetInteger(EnumAni.ANI_I_SKILL, id);
        }
    }

    protected override void onRefresh_ViewType()
    {
        if(/*viewType == VIEW_TYPE_ALL &&*/ isfake && m_eThinkType == AI_TYPE.MAIT_NONE)
        {
            m_eThinkType = AI_TYPE.MAIT_BORN;

            m_moveAgent.baseOffset = 0f;
            m_moveAgent.enabled = false;
            canbehurt = false;
        }

        if (m_moveAgent)
        {
            m_moveAgent.radius = radius * 0.8f;
            m_moveAgent.height = height;
        }
    }


    public virtual void FrameMove(float delta_time) //处理死亡的溶解变化
    {
        //if (m_bWaitingModel) return;

        base.FrameMove(delta_time);

        if (disposed)
            return;

        if (m_moveAgent == null || m_curPhy == null)
            return;

        if (m_bchangeDeadMtl)
        {
            if (m_dead_Burn < 1f)
            {
                m_dead_Burn += delta_time * 0.75f;
                m_MonBody.material.SetFloat(gameST.DEAD_MT_AMOUNT, m_dead_Burn);
            }
            else
            {
                m_remove_after_dead = true;
                //MonsterMgr._inst.RemoveMonster(this);
            }
        }

        if (m_bchangeLiveMtl)
        {
            if (m_live_Burn > 0f)
            {
                m_live_Burn -= delta_time * 0.75f;
                m_MonBody.material.SetFloat(gameST.DEAD_MT_AMOUNT, m_live_Burn);
            }
            else
            {
                m_bchangeLiveMtl = false;

                onResetMTL();
            }
        }

        //处理hurt的颜色变化
        if (m_bDoHitFlash)
        {
            if (m_bhitFlashGoUp)
            {
                m_fhitFlash_time += delta_time * 10f;

                if (m_fhitFlash_time > 0.25f)
                {
                    m_fhitFlash_time = 0.25f;
                    m_bhitFlashGoUp = false;
                }

                m_MonBody.material.SetFloat(EnumShader.SPI_RIMWIDTH, m_fhitFlash_time * 6f);
            }
            else
            {
                if (m_nSkillSP_fb == -31)
                {//受到冰封技能时特殊处理
                    m_MonBody.material.SetFloat(EnumShader.SPI_RIMWIDTH, 1.5f);
                }
                else
                {
                    m_fhitFlash_time -= delta_time;
                    if (m_fhitFlash_time <= 0.0f)
                    {
                        m_fhitFlash_time = 0f;
                        m_bDoHitFlash = false;

                        if (m_Main_Color != null)
                            m_MonBody.material.SetColor(EnumShader.SPI_COLOR, m_Main_Color);
                        else
                            m_MonBody.material.SetColor(EnumShader.SPI_COLOR, Color.white);
                        if (m_Rim_Color != null)
                            m_MonBody.material.SetColor(EnumShader.SPI_RIMCOLOR, m_Rim_Color);
                        else
                            m_MonBody.material.SetColor(EnumShader.SPI_RIMCOLOR, Color.black);

                        m_MonBody.material.SetFloat(EnumShader.SPI_RIMWIDTH, m_Rim_Width);
                        if (m_MonBody1 != null)
                        {
                            m_MonBody1.material.SetColor(EnumShader.SPI_COLOR, m_Main_Color);
                            m_MonBody1.material.SetColor(EnumShader.SPI_RIMCOLOR, m_Rim_Color);
                            m_MonBody1.material.SetFloat(EnumShader.SPI_RIMWIDTH, m_Rim_Width);
                        }
                    }
                    else
                    {
                        m_MonBody.material.SetFloat(EnumShader.SPI_RIMWIDTH, m_fhitFlash_time * 6f);
                        if (m_MonBody1 != null)
                        {
                            m_MonBody1.material.SetFloat(EnumShader.SPI_RIMWIDTH, m_fhitFlash_time * 6f);
                        }
                    }
                }
            }
        }

        if (m_fAttackCount > 0f)
        {
            m_fAttackCount -= delta_time;

            if (m_fAttackCount <= 0f)
            {
                m_curSkillId = 0;
                LeaveAttackState();
                m_curAni.SetInteger(EnumAni.ANI_I_SKILL, 0);
            }
        }
        //修正怪物位置  服务器怪物坐标
        if (m_moveAgent != null && m_moveAgent.enabled && m_moveAgent.isOnNavMesh)
        {
            if (isDartRole) return;
            if (m_moveAgent.isOnNavMesh && m_moveAgent.remainingDistance > 1)
            {
                if (canTestDis)
                {
                    m_moveAgent.avoidancePriority = 50 - (int)m_moveAgent.remainingDistance;
                    canTestDis = false;
                }
            }
            else
            {
                m_moveAgent.avoidancePriority = 10;
                canTestDis = true;
            }
            if (m_curModel.GetComponent<M00000_Default_Event>()?.m_monRole is MDC000)
            {
                m_moveAgent.avoidancePriority = 0;//镖车不受其他控制
                isDartRole = true;
                debug.Log("isDartRole");
            }

        }


        if (isDead)
            return;

        m_fFreezeTime -= delta_time;
        if (m_fFreezeTime > 0)
        {
            //m_curAni.enabled = false;
            m_curAni.speed = m_fFreezeSpeed;
        }
        else if (m_fFreezeTime < 0 && m_bdoFreeze)
        {
            m_fFreezeTime = 0;
            //m_curAni.enabled = true;
            m_curAni.speed = 1f;
            m_bdoFreeze = false;
        }

        if (m_eThinkType == AI_TYPE.MAIT_NONE)
            return;

        if (SelfRole._inst.m_bHide_state)
            return;

        m_fThinkTime -= delta_time;

        //debug.Log(m_curAgent.destination.ToString());
        if (m_fThinkTime <= 0f)
        {
            m_fThinkTime = 0.05f;

            switch (m_eThinkType)
            {
                case AI_TYPE.MAIT_BORN:
                    {

                    }
                    break;
                case AI_TYPE.MAIT_SICK:
                    {
                        AI_Sick();
                    }
                    break;
                case AI_TYPE.MAIT_ATTACK:
                    {
                        AI_Attack(delta_time);
                    }
                    break;
            }
        }
    }

    public void PlayHurtFront()
    {
        if (m_moveAgent != null && m_moveAgent.isOnNavMesh && m_moveAgent.remainingDistance < 1f)
            m_curAni.SetTrigger(EnumAni.ANI_HURT_FRONT);
    }

    public void PlayHurtBack()
    {
        m_curAni.SetTrigger(EnumAni.ANI_HURT_BACK);
    }

    public void PlayFallFront()
    {
        m_curAni.SetTrigger(EnumAni.ANI_FALL_FRONT);
    }

    public void PlayFallBack()
    {
        m_curAni.SetTrigger(EnumAni.ANI_FALL_BACK);
    }

}
