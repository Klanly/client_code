using System;
using UnityEngine;
using System.Collections;
using MuGame;
using System.Collections.Generic;
using GameFramework;

public class ProfessionAvatar
{
    private A3_PROFESSION m_eProfession = A3_PROFESSION.None;

    private bool m_bDisposed = false;
    private string m_strAvatarPath;
    private string m_strEquipEffPath;
    private string m_strLow_or_High;
    public int m_nLayer;
    private Material m_fxLvMtl;

    private int m_nCurFXID;

    private Transform m_Weapon_LBone;
    private GameObject m_Weapon_LObj;
    private Renderer m_Weapon_LDraw;
    private Material m_Weapon_LMtl;
    public int m_Weapon_LID = -1;
    public int m_Weapon_LFXLV = -1;

    private Transform m_Weapon_RBone;
    private GameObject m_Weapon_RObj;
    private Renderer m_Weapon_RDraw;
    private Material m_Weapon_RMtl;
    public int m_Weapon_RID = -1;
    public int m_Weapon_RFXLV = -1;

    private Transform m_WingBone;
    public GameObject m_WingObj;
    private Renderer m_Wing_Draw;
    private Material m_Wing_Mtl;

    private Renderer m_zuoji_Draw;
    private Material m_zuoji_Mtl;

    private Renderer m_zuoji_Draw2;
    private Material m_zuoji_Mtl2;

    public int m_WindID = -1;
    public int m_Wing_FXLV = -1;

    private List<GameObject> m_Euiqp_oldEff = new List<GameObject>();
    public int m_Equip_Eff_id = -1;

    private List<GameObject> m_Euiqp_Eff_New = new List<GameObject>();

    public SkinnedMeshRenderer m_BodySkin; //将Body和Shoulder分开的避免骨骼数过多
    public SkinnedMeshRenderer m_ShoulderSkin;
    private Material [] m_Body_Mtl;
    private Material [] m_Shoulder_Mtl;
    public int m_BodyID = -1;
    public int m_BodyFXLV = -1;
    public uint m_EquipColorID = 0;  //0表示默认颜色

    public Transform m_curModel;

    private bool m_bAnimRebind = false;
    private Transform m_L_UpperArm;
    private Transform m_R_UpperArm;
    private Transform m_Pelvis;
    private Transform m_Spine;
    private Transform m_model;

    public AnimationState m_Wing_Animstate;

    public GameObject rideGo;
    private Animator playerAni;
    private Transform bip01;
    private Action frameMoveCallBack;
    private Transform yingzi;

    private int randomTime  = UnityEngine.Random.Range(5,10);
    private  int idlecurrTime = 0;
    private  bool lastState;
    private  int  rideId;

    public Action weaponL_CallBack = null;
    public Action weaponR_CallBack = null;
    public Action wing_CallBack = null;
    public Action bodyS_CallBack = null;
    public Action body_CallBack = null;   // 异步操作回调
    public bool invisible = false;
    public int speed=0;

    private List<GameObject> fEquip_FXLst = new List<GameObject>();
    private Dictionary<string,Transform> bonesMapping = new Dictionary<string,Transform>();

    public void Init_PA( A3_PROFESSION profession , string avatar_path , string lh , int layer , Material fxlv , Transform cur_model , string equipEff_path = "" )
    {
        m_eProfession = profession;
        m_strAvatarPath = avatar_path;
        m_strEquipEffPath = equipEff_path;
        m_strLow_or_High = lh;
        m_nLayer = layer;
        m_fxLvMtl = fxlv;

        //角色的换装
        m_Weapon_LBone = cur_model.FindChild( "Weapon_L" );
        m_Weapon_RBone = cur_model.FindChild( "Weapon_R" );
        m_WingBone = cur_model.FindChild( "Plus_B" );
        m_BodySkin = cur_model.FindChild( "body" ).GetComponent<SkinnedMeshRenderer>();
        m_ShoulderSkin = cur_model.FindChild( "shoulder" ).GetComponent<SkinnedMeshRenderer>();

        if ( m_BodySkin == null ) m_BodySkin = U3DAPI.DEF_SKINNEDMESHRENDERER;
        if ( m_ShoulderSkin == null ) m_ShoulderSkin = U3DAPI.DEF_SKINNEDMESHRENDERER;

        m_BodyID = -1;
        m_Weapon_LID = -1;
        m_Weapon_RID = -1;
        m_WindID = -1;
        
        //角色装备特效
        m_L_UpperArm = cur_model.FindChild( "L_UpperArm" );
        m_R_UpperArm = cur_model.FindChild( "R_UpperArm" );
        m_Pelvis = cur_model.FindChild( "Pelvis" );
        m_Spine = cur_model.FindChild( "Spine" );
        m_model = cur_model;

        //cur_model.gameObject.SetActive(false);
        m_curModel = cur_model;

        ///  zmh

        bip01 =  cur_model.FindChild( "Bip01" );
        yingzi = cur_model.FindChild( "yingzi" );

        if ( yingzi != null )
        {
            yingzi.gameObject.SetActive( false );
        }

        playerAni = cur_model.GetComponent<Animator>();

        BuilBonesMapping( cur_model.gameObject);
    }

    public void dispose()
    {
        remove_weaponl();
        remove_weaponr();
        remove_wing();
        remove_Ride();

        playerAni=null;

        m_fxLvMtl = null;
        m_Weapon_LBone = null;
        m_Weapon_LObj = null;
        m_Weapon_LDraw = null;
        m_Weapon_LMtl = null;

        m_Weapon_RBone = null;
        m_Weapon_RObj = null;
        m_Weapon_RDraw = null;
        m_Weapon_RMtl = null;

        m_WingBone = null;
        m_WingObj = null;
        m_Wing_Draw = null;
        m_Wing_Mtl = null;

        clear_oldeff();
        clear_eff_new();
        clear_fequip_fx();
        bonesMapping.Clear();

        m_BodySkin = null;
        m_ShoulderSkin = null;
        m_Body_Mtl = null;
        m_Shoulder_Mtl = null;

        bip01=null;
        m_curModel = null;

        m_L_UpperArm = null;
        m_R_UpperArm = null;
        m_Pelvis = null;
        m_Spine = null;
        m_model = null;

        m_bDisposed = true;

        frameMoveCallBack = null;

        weaponL_CallBack = null;
        weaponR_CallBack = null;
        wing_CallBack = null;
        bodyS_CallBack = null;
        body_CallBack = null;  
       
    }

    public void FrameMove()
    {
        if (m_bAnimRebind && m_curModel != null)
        {
            //unity 5.4 保证动画的正确
            var animm = m_curModel.GetComponent<Animator>();

            if (animm != null)
            {
                animm.Rebind();

                if ( rideGo != null  )
                {
                    animm.SetFloat( EnumAni.ANI_F_FLY , 2f );
                }
                else if (m_WindID > 0)
                {
                    animm.SetFloat(EnumAni.ANI_F_FLY, 1f);
                }
                else
                {
                    animm.SetFloat(EnumAni.ANI_F_FLY, 0f);
                }

                if ( frameMoveCallBack != null )
                {
                    frameMoveCallBack();
                }
            }

            m_bAnimRebind = false;
        }

        //坐骑随机动画idle 状态下

        RideRandomAni();

        ResetPosition();

    }

    public void push_fx(int id, bool mustdo = false)
    {
        if (m_bDisposed) return;

        if (m_nCurFXID <= 0 || mustdo)
        {
            m_nCurFXID = id;

            Material fx_mtl = EnumMaterial.EMT_SKILL_HIDE;

            if (m_Body_Mtl != null)
            {
                // 时装原因 可能会存在两个材质球    之前的旧模型 skin 都是一个材质球 （material）

                Material[] materials = new Material[m_Body_Mtl.Length];

                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = fx_mtl;
                }

                m_BodySkin.materials = materials;

            }

            if (m_Shoulder_Mtl != null)
            {
                Material[] materials = new Material[m_Body_Mtl.Length];

                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = fx_mtl;
                }

                m_ShoulderSkin.materials = materials;
            }

            if (m_Weapon_LDraw != null)
            {
                m_Weapon_LDraw.material = fx_mtl;
            }

            if (m_Weapon_RDraw != null)
            {
                m_Weapon_RDraw.material = fx_mtl;
            }

            if (m_Wing_Draw != null)
            {
                m_Wing_Draw.material = fx_mtl;
            }

            if ( m_zuoji_Draw != null )
            {
                m_zuoji_Draw.material = fx_mtl;
            }

            if ( m_zuoji_Mtl2 != null )
            {
                m_zuoji_Draw.materials = new Material[] { fx_mtl , fx_mtl };
            }


            //隐身加的特效

            if (a3_lowblood.instance != null)
                a3_lowblood.instance.begin_assassin_fx();
        }
    }

    public void pop_fx()
    {
        if (m_bDisposed) return;

        if (m_nCurFXID > 0)
        {
            m_nCurFXID = 0;

            if (m_Body_Mtl != null)
            {
                m_BodySkin.materials = m_Body_Mtl;
            }

            if (m_Shoulder_Mtl != null)
            {
                m_ShoulderSkin.materials = m_Shoulder_Mtl;
            }

            if (m_Weapon_LDraw != null)
            {
                m_Weapon_LDraw.material = m_Weapon_LMtl;
            }

            if (m_Weapon_RDraw != null)
            {
                m_Weapon_RDraw.material = m_Weapon_RMtl;
            }

            if (m_Wing_Draw != null)
            {
                m_Wing_Draw.material = m_Wing_Mtl;
            }

            if ( m_zuoji_Draw != null )
            {
                m_zuoji_Draw.material = m_zuoji_Mtl;
            }

            if ( m_zuoji_Mtl2 != null )
            {
                m_zuoji_Draw.materials = new Material[] { m_zuoji_Mtl , m_zuoji_Mtl2 };
            }

            if (a3_lowblood.instance != null)
                a3_lowblood.instance.end_assassin_fx();
        }
    }

    private void weaponl_lfx_callback(UnityEngine.Object ac, System.Object data)
    {
        if (m_bDisposed) return;

        GameObject parent = data as GameObject;
        Texture2D tex = ac as Texture2D;
        if (tex != null && m_Weapon_LObj == parent)
        {
            Transform tf_w = m_Weapon_LObj.transform.FindChild("weapon");
            if (tf_w != null)
            {
                SkinnedMeshRenderer smr = tf_w.GetComponent<SkinnedMeshRenderer>();
                if (smr != null)
                {
                    smr.material = set_strength_shader(smr.material, tex, m_Weapon_LID, m_Weapon_LFXLV);
                    m_Weapon_LDraw = smr;
                    m_Weapon_LMtl = smr.material;
                }
                else
                {
                    MeshRenderer mr = tf_w.GetComponent<MeshRenderer>();
                    if (mr != null)
                    {
                        mr.material = set_strength_shader(mr.material, tex, m_Weapon_LID, m_Weapon_LFXLV);
                        m_Weapon_LDraw = mr;
                        m_Weapon_LMtl = mr.material;
                    }
                }
            }
        }
    }

    private void weaponl_model_callback(UnityEngine.Object ac, System.Object data)
    {
        if (m_bDisposed) return;
        if (ac == null || ac == U3DAPI.DEF_GAMEOBJ) return;
        if (m_Weapon_LID != (int)data) return;

        m_Weapon_LObj = GameObject.Instantiate(ac) as GameObject;
        m_Weapon_LObj.transform.SetParent(m_Weapon_LBone, false);
        foreach (Transform tran in m_Weapon_LObj.GetComponentsInChildren<Transform>())
        {
            //todo 特效不能包含其中，特效最后通过一个管理器加入，低配的玩家可以关闭
            tran.gameObject.layer = m_nLayer ;// 更改物体的Layer层

        }

        //强化效果
        Transform tf_w = m_Weapon_LObj.transform.FindChild("weapon");
        if (tf_w != null)
        {
            SkinnedMeshRenderer smr = tf_w.GetComponent<SkinnedMeshRenderer>();

            if (smr != null)
            {
                if (m_Weapon_LFXLV > 0)
                {
                    Load_Mtl_Texture(smr.material, m_Weapon_LObj, weaponl_lfx_callback);
                }

                m_Weapon_LDraw = smr;
                m_Weapon_LMtl = smr.material;
            }
            else
            {
                MeshRenderer mr = tf_w.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    if (m_Weapon_LFXLV > 0)
                    {
                        Load_Mtl_Texture(mr.material, m_Weapon_LObj, weaponl_lfx_callback);
                    }

                    m_Weapon_LDraw = mr;
                    m_Weapon_LMtl = mr.material;
                }
            }
        }


        if ( weaponL_CallBack != null )
        {
            weaponL_CallBack();
        }

        if ( rideGo !=null )
        {
            int carrer = GetCarrer() ;

            SetPlayerWeaponTransform( carrer );
        }
      
    }

    private void remove_weaponl()
    {
        if (m_Weapon_LObj != null)
        {
            GameObject.Destroy(m_Weapon_LObj);
            m_Weapon_LObj = null;
        }
    }


    public void set_weaponl(int id, int fxlevel )
    {
        if (m_eProfession == A3_PROFESSION.Warrior) return;
        if (m_Weapon_LID == id && m_Weapon_LFXLV == fxlevel) return;

        m_Weapon_LID = id;
        m_Weapon_LFXLV = fxlevel;

        remove_weaponl();

        if (m_Weapon_LID == -1)
        {
            return;
        }

        if (m_Weapon_LID >= 0)
        {
            string h_l = m_strLow_or_High;

            //if (this.m_nLayer == EnumLayer.LM_ROLE_INVISIBLE )
            //{
            //    h_l = "h_";
            //}

            GAMEAPI.ABModel_LoadGameObject(m_strAvatarPath + "weaponl_" + h_l + m_Weapon_LID.ToString(), weaponl_model_callback, m_Weapon_LID);
        }
    }

    internal void Set_Ride(string avatarName)
    {
        throw new NotImplementedException();
    }

    private void weaponr_fx_callback(UnityEngine.Object ac, System.Object data)
    {
        if (m_bDisposed) return;

        GameObject parent = data as GameObject;
        Texture2D tex = ac as Texture2D;
        if (tex != null && m_Weapon_RObj == parent)
        {
            Transform tf_w = m_Weapon_RObj.transform.FindChild("weapon");
            if (tf_w != null)
            {
                SkinnedMeshRenderer smr = tf_w.GetComponent<SkinnedMeshRenderer>();
                if (smr != null)
                {
                    smr.material = set_strength_shader(smr.material, tex, m_Weapon_RID, m_Weapon_RFXLV);
                    m_Weapon_RDraw = smr;
                    m_Weapon_RMtl = smr.material;
                }
                else
                {
                    MeshRenderer mr = tf_w.GetComponent<MeshRenderer>();
                    if (mr != null)
                    {
                        mr.material = set_strength_shader(mr.material, tex, m_Weapon_RID, m_Weapon_RFXLV);
                        m_Weapon_RDraw = mr;
                        m_Weapon_RMtl = mr.material;
                    }
                }
            }
        }
    }

    private void weaponr_model_callback(UnityEngine.Object ac, System.Object data)
    {
        if (m_bDisposed) return;
        if (ac == null || ac == U3DAPI.DEF_GAMEOBJ) return;
        if (m_Weapon_RID != (int)data) return;

        m_Weapon_RObj = GameObject.Instantiate(ac) as GameObject;
        m_Weapon_RObj.transform.SetParent(m_Weapon_RBone, false);

        foreach (Transform tran in m_Weapon_RObj.GetComponentsInChildren<Transform>())
        {
            //todo 特效不能包含其中，特效最后通过一个管理器加入，低配的玩家可以关闭
            tran.gameObject.layer = m_nLayer;// 更改物体的Layer层
        }

        //强化效果
        Transform tf_w = m_Weapon_RObj.transform.FindChild("weapon");
        if (tf_w != null)
        {
            SkinnedMeshRenderer smr = tf_w.GetComponent<SkinnedMeshRenderer>();

            if (smr != null)
            {
                if (m_Weapon_RFXLV > 0)
                {
                    Load_Mtl_Texture(smr.material, m_Weapon_RObj, weaponr_fx_callback);
                }

                m_Weapon_RDraw = smr;
                m_Weapon_RMtl = smr.material;
            }
            else
            {
                MeshRenderer mr = tf_w.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    if (m_Weapon_RFXLV > 0)
                    {
                        Load_Mtl_Texture(mr.material, m_Weapon_RObj, weaponr_fx_callback);
                    }

                    m_Weapon_RDraw = mr;
                    m_Weapon_RMtl = mr.material;
                }
            }

        }

        if ( weaponR_CallBack != null )
        {
            weaponR_CallBack();
        }

        if ( rideGo !=null )
        {
            int carrer = GetCarrer() ;

            SetPlayerWeaponTransform( carrer );

        }  //异步问题
    }

    internal void ChangeAni()
    {
        throw new NotImplementedException();
    }

    private void remove_weaponr()
    {
        if (m_Weapon_RObj != null)
        {
            GameObject.Destroy(m_Weapon_RObj);
            m_Weapon_RObj = null;
        }
    }

    public void set_weaponr(int id, int fxlevel , Action<ProfessionRole> _callBck = null )
    {
        if (m_eProfession == A3_PROFESSION.Mage) return;
        if (m_Weapon_RID == id && m_Weapon_RFXLV == fxlevel) return;

        m_Weapon_RID = id;
        m_Weapon_RFXLV = fxlevel;

        remove_weaponr();

        if (m_Weapon_RID == -1)
        {
            return;
        }

        if (m_Weapon_RID >= 0)
        {
            GAMEAPI.ABModel_LoadGameObject(m_strAvatarPath + "weaponr_" + m_strLow_or_High + m_Weapon_RID.ToString(), weaponr_model_callback, m_Weapon_RID);
        }
    }

    private void body_fx_callback(UnityEngine.Object ac, System.Object data)
    {
        if (m_bDisposed) return;

        SkinnedMeshRenderer parent = data as SkinnedMeshRenderer;
        Texture2D tex = ac as Texture2D;
        if (tex != null && m_BodySkin == parent)
        {
            m_BodySkin.material = set_strength_shader(m_BodySkin.material, tex, m_BodyID, m_BodyFXLV);
            m_Body_Mtl = m_BodySkin.materials;
        }
    }

    private void shoulder_fx_callback(UnityEngine.Object ac, System.Object data)
    {
        if (m_bDisposed) return;

        SkinnedMeshRenderer parent = data as SkinnedMeshRenderer;
        Texture2D tex = ac as Texture2D;
        if (tex != null && m_ShoulderSkin == parent)
        {
            m_ShoulderSkin.material = set_strength_shader(m_ShoulderSkin.material, tex, m_BodyID, m_BodyFXLV);
            m_Shoulder_Mtl = m_ShoulderSkin.materials;
        }
    }

    private void body_model_callback(UnityEngine.Object ac, System.Object data)
    {
        if (m_bDisposed) return;
        if (ac == null || ac == U3DAPI.DEF_GAMEOBJ) return;
        if (m_BodyID != (int)data) return;

        GameObject obj_prefab = ac as GameObject;
        if (obj_prefab == null || m_BodySkin == null) return;

        SkinnedMeshRenderer t_smr = obj_prefab.GetComponent<SkinnedMeshRenderer>();
        if (t_smr != null)
        {
            m_BodySkin.sharedMesh = t_smr.sharedMesh;
            m_BodySkin.sharedMaterials = t_smr.sharedMaterials;

            m_bAnimRebind = true;
        }

        if (m_BodyFXLV > 0)
        {
            Load_Mtl_Texture(m_BodySkin.material, m_BodySkin, body_fx_callback);
        }

        m_Body_Mtl = m_BodySkin.materials;

        clear_fequip_fx();

        SetFEquipFxGo( obj_prefab );

        if ( body_CallBack != null )
        {
            body_CallBack();
        }

    }

    private void shoulder_model_callback(UnityEngine.Object ac, System.Object data)
    {
        if (m_bDisposed) return;
        if (ac == null || ac == U3DAPI.DEF_GAMEOBJ) return;
        if (m_BodyID != (int)data) return;

        GameObject obj_prefab = ac as GameObject;
        if (obj_prefab == null) return;

        SkinnedMeshRenderer t_smr = obj_prefab.GetComponent<SkinnedMeshRenderer>();
        if (t_smr != null)
        {
            m_ShoulderSkin.sharedMesh = t_smr.sharedMesh;
            m_ShoulderSkin.sharedMaterials = t_smr.sharedMaterials;

            m_bAnimRebind = true;
        }

        if (m_BodyFXLV > 0)
        {
            Load_Mtl_Texture(m_ShoulderSkin.material, m_ShoulderSkin, shoulder_fx_callback);
        }

        m_Shoulder_Mtl = m_ShoulderSkin.materials;

        if ( bodyS_CallBack != null )
        {
            bodyS_CallBack();
        }
    }


    public void set_body(int id, int fxlevel)
    {
        if (m_BodyID == id && m_BodyFXLV == fxlevel) return;

        m_BodyID = id;
        m_BodyFXLV = fxlevel;

        if (m_BodyID == -1)
        {
            if (m_BodySkin != null)
            {
                m_BodySkin.sharedMesh = null;
                m_ShoulderSkin.sharedMesh = null;
            }
            return;
        }
        debug.Log("模型的路径是：" + m_strAvatarPath + "  " + m_strLow_or_High + "  " + m_BodyID);
        GAMEAPI.ABModel_LoadGameObject(m_strAvatarPath + "body_" + m_strLow_or_High + m_BodyID.ToString(), body_model_callback, m_BodyID);
        GAMEAPI.ABModel_LoadGameObject(m_strAvatarPath + "shoulder_" + m_strLow_or_High + m_BodyID.ToString(), shoulder_model_callback, m_BodyID);
    }

    public void set_equip_eff(int eff_lvl)
    {
        clear_eff_new();

        if (eff_lvl > 0)
        {
            GameObject obj_prefab1 = GAMEAPI.ABFight_LoadPrefab("FX_armourFX_FX_qh_buff_Lv" + eff_lvl);
            if (obj_prefab1 != null)
            {
                GameObject go = GameObject.Instantiate(obj_prefab1) as GameObject;
                Transform eqpeff_Con = m_model.FindChild("eqpEffect");
                if(eqpeff_Con!= null)
                    go.transform.SetParent(eqpeff_Con, false);
                else 
                    go.transform.SetParent(m_model, false);
                //foreach (Transform tran in go.GetComponentsInChildren<Transform>())
                //{
                    //todo 特效不能包含其中，特效最后通过一个管理器加入，低配的玩家可以关闭
                   // tran.gameObject.layer = m_nLayer;// 更改物体的Layer层
               // }
                m_Euiqp_Eff_New.Add(go);

                SetFXPosition( rideGo != null );

            }
        }
    }
    public void clear_eff_new()
    {
        for (int i = 0; i < m_Euiqp_Eff_New.Count; ++i)
        {
            GameObject.Destroy(m_Euiqp_Eff_New[i]);
        }
        m_Euiqp_Eff_New.Clear();
    }

    public void showEquipEff()
    {
        foreach (GameObject go in m_Euiqp_oldEff)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in m_Euiqp_Eff_New)
        {
            go.SetActive(true);
        }
    }
    public void hideEquipEff()
    {
        foreach (GameObject go in m_Euiqp_oldEff)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in m_Euiqp_Eff_New)
        {
            go.SetActive(false);
        }
    }

    public void clear_oldeff()
    {
        for (int i = 0; i < m_Euiqp_oldEff.Count; ++i)
        {
            GameObject.Destroy(m_Euiqp_oldEff[i]);
        }
        m_Euiqp_oldEff.Clear();
    }

    public void clear_fequip_fx()
    {
        for ( int i = 0 ; i < fEquip_FXLst.Count ; ++i )
        {
            GameObject.Destroy( fEquip_FXLst[ i ] );
        }

        fEquip_FXLst.Clear();
    }

    public void set_equip_eff(int id, bool high)
    {
        if (m_bDisposed) return;

        clear_oldeff();

        m_Equip_Eff_id = id;
        if (m_Equip_Eff_id > 0)
        {
            GameObject obj_prefab1 = GAMEAPI.ABFight_LoadPrefab(m_strEquipEffPath + id + "_FX_L_UpperArm_" + id);
            GameObject obj_prefab2 = GAMEAPI.ABFight_LoadPrefab(m_strEquipEffPath + id + "_FX_Pelvis_" + id);
            GameObject obj_prefab3 = GAMEAPI.ABFight_LoadPrefab(m_strEquipEffPath + id + "_FX_R_UpperArm_" + id);
            GameObject obj_prefab4 = GAMEAPI.ABFight_LoadPrefab(m_strEquipEffPath + id + "_FX_Spine_" + id);

            if (obj_prefab1 != null)
            {
                add_equip_eff(obj_prefab1, m_L_UpperArm, high);
            }
            if (obj_prefab2 != null)
            {
                add_equip_eff(obj_prefab2, m_Pelvis, high);
            }
            if (obj_prefab3 != null)
            {
                add_equip_eff(obj_prefab3, m_R_UpperArm, high);
            }
            if (obj_prefab4 != null)
            {
                add_equip_eff(obj_prefab4, m_Spine, high);
            }
        }
    }
    private void add_equip_eff(GameObject child, Transform parent, bool high)
    {
        if (m_bDisposed) return;

        GameObject go = GameObject.Instantiate(child) as GameObject;
        if (high)
        {
            Transform go_hide = go.transform.FindChild("hide");
            if (go_hide != null)
                go_hide.gameObject.SetActive(true);
        }
        go.transform.SetParent(parent, false);

        foreach (Transform tran in go.GetComponentsInChildren<Transform>())
        {
            //todo 特效不能包含其中，特效最后通过一个管理器加入，低配的玩家可以关闭
            tran.gameObject.layer = m_nLayer;// 更改物体的Layer层
        }

        m_Euiqp_oldEff.Add(go);
    }

    private void wing_model_callback(UnityEngine.Object ac, System.Object data)
    {
        if (m_bDisposed) return;
        if (ac == null || ac == U3DAPI.DEF_GAMEOBJ) return;
        if (m_WindID != (int)data) return;

        m_WingObj = GameObject.Instantiate(ac) as GameObject;
        m_WingObj.transform.SetParent(m_WingBone, false);
        foreach (Transform tran in m_WingObj.GetComponentsInChildren<Transform>())
        {
            //todo 特效不能包含其中，特效最后通过一个管理器加入，低配的玩家可以关闭
            tran.gameObject.layer = m_nLayer;// 更改物体的Layer层
        }

        //强化效果
        Transform tf_w = m_WingObj.transform.FindChild("wing");
        if (tf_w != null)
        {
            SkinnedMeshRenderer smr = tf_w.GetComponent<SkinnedMeshRenderer>();

            if (smr != null)
            {
                //if (fxlevel > 0)
                //{
                //    Material mtl_inst = GameObject.Instantiate(m_fxLvMtl) as Material;
                //    Material cur_mat = smr.material;
                //    Texture tex = cur_mat.GetTexture(EnumShader.SPI_MAINTEX);
                //    mtl_inst.SetTexture(EnumShader.SPI_MAINTEX, tex);

                //    smr.material = mtl_inst;
                //}

                m_Wing_Draw = smr;
                m_Wing_Mtl = smr.material;
            }
            else
            {
                MeshRenderer mr = tf_w.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    //if (fxlevel > 0)
                    //{
                    //    Material mtl_inst = GameObject.Instantiate(m_fxLvMtl) as Material;

                    //    Material cur_mat = mr.material;

                    //    Texture tex = cur_mat.GetTexture(EnumShader.SPI_MAINTEX);
                    //    mtl_inst.SetTexture(EnumShader.SPI_MAINTEX, tex);

                    //    mr.material = mtl_inst;
                    //}

                    m_Wing_Draw = mr;
                    m_Wing_Mtl = mr.material;
                }
            }
        }

        Animation wing_anim = m_WingObj.GetComponent<Animation>();
        if (wing_anim != null)
        {
            m_Wing_Animstate = wing_anim["wg"];
        }

        if ( wing_CallBack != null )
        {
            wing_CallBack();
        }
    }

    private void remove_wing()
    {
        if (m_WingObj != null)
        {
            GameObject.Destroy(m_WingObj);
            m_WingObj = null;
        }
    }

    public void set_wing(int id, int fxlevel)
    {
        //翅膀没有 wing_0 的资源。这里先这样修改
        // (id == 0) return;
        if (m_WindID == id && m_Wing_FXLV == fxlevel) return;

        m_WindID = id;
        m_Wing_FXLV = fxlevel;

        remove_wing();

        if (m_WindID == -1)
        {
            return;
        }

        if (m_WindID > 0)
        {
            string h_l = m_strLow_or_High;

            //if (this.m_nLayer == EnumLayer.LM_ROLE_INVISIBLE)
            //{
            //    h_l = "h_";
            //}

            GAMEAPI.ABModel_LoadGameObject(m_strAvatarPath + "wing_" + h_l + m_WindID.ToString(), wing_model_callback, m_WindID);
        }
    }

    public void Load_Mtl_Texture(Material ml, System.Object parent, System.Action<UnityEngine.Object, System.Object> call_back)
    {
        Texture tex = ml.GetTexture(EnumShader.SPI_MAINTEX);
        if (tex != null)
        {
            string tex1_name = tex.name.Substring(0, tex.name.Length - 6) + "q";
            GAMEAPI.ABModel_LoadTexture2D("mtl_" + tex1_name, call_back, parent);
        }
    }

    public Material set_strength_shader(Material ml, Texture2D tex1, int id, int fxlevel)
    {
        //Material cur_mat = ml;
        Texture tex = ml.GetTexture(EnumShader.SPI_MAINTEX);

        ////例如   mage_skin0_diff_h 到 mage_skin0_q
        //string tex1_name = tex.name.Substring(0, tex.name.Length - 6) + "q";
        ////debug.Log("材质是"+ tex1_name);

        ////Texture tex1 = GAMEAPI.ABFight_LoadTexture2D("mtl_" + tex1_name);
        //Texture tex1 = null;
        Material mtl_inst = GameObject.Instantiate(m_fxLvMtl) as Material;
        mtl_inst.SetTexture(EnumShader.SPI_MAINTEX, tex);

        if (tex1 != null)
            mtl_inst.SetTexture(EnumShader.SPI_SUBTEX, tex1);

        SXML xml = XMLMgr.instance.GetSXML("item.stage", "stage_level==" + fxlevel);
        if (xml == null)
            return mtl_inst;

        xml = xml.GetNode("stage_info", "itemid==" + id);
        if (xml == null)
            return mtl_inst;

        xml = xml.GetNode("intensify_light", null);
        if (xml == null)
            return mtl_inst;

        string[] high_light = xml.getString("high_light").Split(',');
        string[] strength_light = xml.getString("strength_light").Split(',');
        string[] color = xml.getString("color").Split(',');

        Vector4 splRim = new Vector4(float.Parse(high_light[0]), float.Parse(high_light[1]), float.Parse(high_light[2]), float.Parse(high_light[3]));
        Vector4 strAnim = new Vector4(float.Parse(strength_light[0]), float.Parse(strength_light[1]), float.Parse(strength_light[2]), float.Parse(strength_light[3]));
        Color strColor = new Color(float.Parse(color[0]) / 255, float.Parse(color[1]) / 255, float.Parse(color[2]) / 255);
        mtl_inst.SetVector(EnumShader.SPI_SPLRIM, splRim);
        mtl_inst.SetVector(EnumShader.SPI_STRANIM, strAnim);
        mtl_inst.SetColor(EnumShader.SPI_STRCOLOR, strColor);
        return mtl_inst;
    }

    public void set_equip_color(uint id)
    {
        if (m_bDisposed) return;
        if (m_BodySkin == null || m_ShoulderSkin == null) return;

        m_EquipColorID = id;

        if (m_BodyID == -1)
        {
            return;
        }

        if (id == 0)
        {
            m_BodySkin.material.SetColor(EnumShader.SPI_CHANGECOLOR, new Color(1, 1, 1, 0));
            m_ShoulderSkin.material.SetColor(EnumShader.SPI_CHANGECOLOR, new Color(1, 1, 1, 0));
            return;
        }

        Color col = a3_EquipModel.EQUIP_COLOR[id];

        m_BodySkin.material.SetColor(EnumShader.SPI_CHANGECOLOR, col);
        m_ShoulderSkin.material.SetColor(EnumShader.SPI_CHANGECOLOR, col);

        //if (m_Weapon_LObj != null)
        //{
        //    Transform tf_l = m_Weapon_LObj.transform.FindChild("weapon");
        //    if (tf_l != null)
        //    {
        //        SkinnedMeshRenderer smr = tf_l.GetComponent<SkinnedMeshRenderer>();
        //        if (smr != null)
        //        {
        //            smr.material.SetColor(EnumShader.SPI_CHANGECOLOR, col);
        //        }
        //        else
        //        {
        //            MeshRenderer mr = tf_l.GetComponent<MeshRenderer>();
        //            if (mr != null)
        //            {
        //                mr.material.SetColor(EnumShader.SPI_CHANGECOLOR, col);
        //            }
        //        }
        //    }
        //}
        //if (m_Weapon_RObj != null)
        //{
        //    Transform tf_r = m_Weapon_RObj.transform.FindChild("weapon");
        //    if (tf_r != null)
        //    {
        //        SkinnedMeshRenderer smr = tf_r.GetComponent<SkinnedMeshRenderer>();
        //        if (smr != null)
        //        {
        //            smr.material.SetColor(EnumShader.SPI_CHANGECOLOR, col);
        //        }
        //        else
        //        {
        //            MeshRenderer mr = tf_r.GetComponent<MeshRenderer>();
        //            if (mr != null)
        //            {
        //                mr.material.SetColor(EnumShader.SPI_CHANGECOLOR, col);
        //            }
        //        }
        //    }
        //}

        //if (m_WingObj != null)
        //{
        //    Transform tf_w = m_WingObj.transform.FindChild("wing");
        //    if (tf_w != null)
        //    {
        //        SkinnedMeshRenderer smr = tf_w.GetComponent<SkinnedMeshRenderer>();
        //        if (smr != null)
        //        {
        //            smr.material.SetColor(EnumShader.SPI_CHANGECOLOR, col);
        //        }
        //        else
        //        {
        //            MeshRenderer mr = tf_w.GetComponent<MeshRenderer>();
        //            if (mr != null)
        //            {
        //                mr.material.SetColor(EnumShader.SPI_CHANGECOLOR, col);
        //            }
        //        }
        //    }
        //}      
    }

    private bool isUIShow = false;

    public void Set_Ride( int rideId , Action<float> callBack = null , bool isUIShow = false ) {

        this.rideId = rideId;

        this.isUIShow = isUIShow;

        string avatarName = m_strLow_or_High + A3_RideModel.getInstance().GetValueByType<RideConfigVo>(rideId).avatar;

        GAMEAPI.ABModel_LoadGameObject( avatarName , ( go , data ) => {

            remove_Ride();

            rideGo = GameObject.Instantiate( go ) as  GameObject;

            rideGo.name = "zuoji";

            SkinnedMeshRenderer  smr = rideGo.GetComponentInChildren<SkinnedMeshRenderer>();

            if ( smr != null )
            {
                m_zuoji_Draw = smr;
                m_zuoji_Mtl = smr.materials[0];

                if ( smr.materials.Length > 1 )
                {
                    m_zuoji_Mtl2 = smr.materials[ 1 ];
                }
                
            }

            int layer = EnumLayer.LM_DEFAULT;

            if ( m_nLayer != EnumLayer.LM_ROLE_INVISIBLE && GRMap.curSvrConf.ContainsKey( "id" ) && GRMap.curSvrConf.getValue( "id" )._int == 10 )
            {
                layer  =  m_nLayer == EnumLayer.LM_SELFROLE ? EnumLayer.LM_SELFRIDE : EnumLayer.LM_OTHERRIDE;

            }

            else {

                layer = m_nLayer;

            }  //美术提的特殊需求 主城场景光太亮 单独一盏灯处理

            if ( SceneCamera.m_nModelDetail_Level != 1 && this.m_nLayer != EnumLayer.LM_SELFROLE && isUIShow == false )
            {
                layer = EnumLayer.LM_DEFAULT;

            } //系统设置不显示其他玩家模型

            foreach ( var item in rideGo.GetComponentsInChildren<Transform>() )
            {
                item.gameObject.layer = layer;
            }

            rideGo.transform.SetParent( bip01 );
            rideGo.transform.localPosition=Vector3.zero;
            rideGo.transform.localEulerAngles=Vector3.zero;

           if (  yingzi ) yingzi.transform.SetParent( rideGo.transform);

            int carrer = GetCarrer() ;

            if ( playerAni ) playerAni.SetFloat( EnumAni.ANI_F_FLY , 2f );

            SetPlayerModelTransform( carrer , rideId , callBack );

            frameMoveCallBack = () =>
            {
                if ( playerAni ) {

                    playerAni.SetFloat( EnumAni.ANI_F_FLY , 2f );

                }

                frameMoveCallBack=null;

            }; // FrameMove  Rebind （可能比这里慢  所以加了个回调）；

        } , avatarName );

    }

    // career  对应的职业  1.刺客 2.法师 3.战士

    public void SetPlayerModelTransform( int carrer , int rideId , Action<float> callBack = null ) {

        if ( carrer == 0  )
        {
            Debug.LogError("没有匹配到职业");

            return;
        }

        RideConfigVo vo = A3_RideModel.getInstance().GetValueByType<RideConfigVo>( rideId );
        TransformConfigVo trans = vo.transformMapping[ carrer ];

        rideGo.transform.localPosition=new Vector3( trans.postionMapping[ "x" ] , trans.postionMapping[ "y" ] , trans.postionMapping[ "z" ] );
        rideGo.transform.localEulerAngles  = new Vector3( trans.rotationMapping[ "x" ] , trans.rotationMapping[ "y" ] , trans.rotationMapping[ "z" ] );
        rideGo.transform.localScale = new Vector3( trans.scaleMapping[ "x" ] , trans.scaleMapping[ "y" ] , trans.scaleMapping[ "z" ] );

        SetPlayerWeaponTransform( carrer );

        if ( m_nLayer == EnumLayer.LM_ROLE_INVISIBLE && isUIShow )
        {
            m_curModel.localPosition = Vector3.zero;
            m_curModel.localPosition = m_curModel.localPosition + new Vector3( 0 , trans.baseoffest + 0.15f , 0 );

        }

        if ( callBack != null )
        {
            callBack( trans.baseoffest );
            SetFXPosition(true);
        }

    }

    public void SetShadowgoScale( int rideId , GameObject yingzi , float baseoffest ) {

        if ( yingzi == null )
        {
            return;
        }

        Transform _yingzi = yingzi.transform;

        RideConfigVo vo = A3_RideModel.getInstance().GetValueByType<RideConfigVo>( rideId );

        TransformConfigVo trans = vo.transformMapping[ GetCarrer() ];

        _yingzi.localPosition = Vector3.zero;
        _yingzi.localScale = Vector3.one;

        _yingzi.localPosition = _yingzi.localPosition + new Vector3( 0 , -baseoffest , trans.yinzipostionMapping["z"] );

        _yingzi.localScale = new Vector3( trans.yinziscaleMapping[ "x" ] , trans.yinziscaleMapping[ "y" ] , trans.yinziscaleMapping[ "z" ] );

    }

    public void SetPlayerWeaponTransform( int carrer ) {

        var config = A3_RideModel.getInstance().GetWeaponVoByCarrer( carrer );

        if ( config == null )
        {
            return;
        }

        if ( carrer == 1|| carrer == 3 )
        {
            if ( m_Weapon_RObj == null ) return;
           
            var posMapping_right = config.rightVo.postionMapping;
            var roatMapping_right = config.rightVo.rotationMapping;
            m_Weapon_RObj.transform.localPosition = new Vector3( posMapping_right[ "x" ] , posMapping_right[ "y" ] , posMapping_right[ "z" ] );
            m_Weapon_RObj.transform.localEulerAngles = new Vector3( roatMapping_right[ "x" ] , roatMapping_right[ "y" ] , roatMapping_right[ "z" ] );
        }


        if ( carrer == 1 || carrer == 2 )
        {
            if ( m_Weapon_LObj == null ) return;

            var posMapping_left = config.leftVo.postionMapping;
            var roatMapping_left = config.leftVo.rotationMapping;
            m_Weapon_LObj.transform.localPosition = new Vector3( posMapping_left[ "x" ] , posMapping_left[ "y" ] , posMapping_left[ "z" ] );
            m_Weapon_LObj.transform.localEulerAngles = new Vector3( roatMapping_left[ "x" ] , roatMapping_left[ "y" ] , roatMapping_left[ "z" ] );
        }
    
    }

    public void SetResetTransform()
    {
        if ( m_nLayer == EnumLayer.LM_ROLE_INVISIBLE && this.isUIShow )
        {
            return;
        }

        if ( m_Weapon_RObj )
        {
            m_Weapon_RObj.transform.localPosition = Vector3.zero;
            m_Weapon_RObj.transform.localEulerAngles = Vector3.zero;
        }

        if ( m_Weapon_LObj )
        {
            m_Weapon_LObj.transform.localPosition = Vector3.zero;
            m_Weapon_LObj.transform.localEulerAngles = Vector3.zero;
        }

        SetFXPosition( false );

        if ( playerAni != null )
        {
            playerAni.Rebind();

            if ( m_WindID > 0 )
            {
                playerAni.SetFloat( EnumAni.ANI_F_FLY , 1f );
            }
            else
            {
                playerAni.SetFloat( EnumAni.ANI_F_FLY , 0f );
            }
          
        }

    }

    public void remove_Ride() {
            
        if ( rideGo != null )
        {
            if ( yingzi )
            {
                yingzi.transform.SetParent( m_curModel.transform );
            }

            m_zuoji_Draw = null;
            m_zuoji_Mtl  = null;

            m_zuoji_Draw2 = null;
            m_zuoji_Mtl2  = null;

            GameObject.Destroy( rideGo );

            SetResetTransform();

            rideGo= null;
            
        }

    }

    public void RideRandomAni() {

        if ( rideGo == null  || rideGo.GetComponent<Animator>().GetBool( EnumAni.ANI_T_RIDERUN ) )
        {

            idlecurrTime = 0;

            randomTime=0;

            return;

        } 

        if ( idlecurrTime == 0 )
        {
            idlecurrTime = NetClient.instance.CurServerTimeStamp;

            randomTime  = UnityEngine.Random.Range( 5 , 10 );

        }

        if ( idlecurrTime != 0 &&NetClient.instance.CurServerTimeStamp > randomTime + idlecurrTime )
        {
            var  _zuojiAni = rideGo.GetComponent<Animator>();

            var maxCount =  _zuojiAni.runtimeAnimatorController.animationClips;

            int lastValue = (int)_zuojiAni.GetFloat(EnumAni.ANI_T_RANDOMVA);

            if (lastValue < 0.01f) lastValue = 1;

            lastValue = lastValue >= ( maxCount.Length - 2 ) ? 1 : ++lastValue;

            _zuojiAni.SetFloat( EnumAni.ANI_T_RANDOMVA , lastValue);

            _zuojiAni.SetTrigger( EnumAni.ANI_T_RIDERANDOM );

            idlecurrTime = 0;

            randomTime=0;

        }


    }
    
    public void ResetPosition() {

        if ( rideGo == null  || rideGo.GetComponent<Animator>().GetBool( EnumAni.ANI_T_RIDERUN ) ==  lastState  )
        {
            return;
        }

        lastState = rideGo.GetComponent<Animator>().GetBool( EnumAni.ANI_T_RIDERUN );

        //Debug.LogError(lastState);

        if ( lastState )
        {
            float speed = (float)( this.speed - 259 )/100;

            speed = speed < 1f ? 1f : speed;

            playerAni.speed = speed > 1.6f ? 1.6f : speed;   //最大1.6f

        }
        else {

            playerAni.speed = 1f;
        }

        RideConfigVo vo = A3_RideModel.getInstance().GetValueByType<RideConfigVo>( this.rideId );

        if ( vo == null )
        {
            return;
        }

        TransformConfigVo trans = vo.transformMapping[ GetCarrer() ];

        if ( trans.runpostionMapping == null )
        {
            return;
        }

        if ( lastState  )
        {
            rideGo.transform.localPosition = new Vector3( trans.runpostionMapping["x"], trans.runpostionMapping[ "y" ], trans.runpostionMapping[ "z" ]);
        }
        else
        {
            rideGo.transform.localPosition = new Vector3( trans.postionMapping[ "x" ] , trans.postionMapping[ "y" ] , trans.postionMapping[ "z" ] );
        }

    }

    public int GetCarrer() {

        int carrer = 0 ;

        if ( this.m_eProfession  == A3_PROFESSION.Warrior )
        {
            carrer = 3;
        }
        else if ( this.m_eProfession  == A3_PROFESSION.Mage )
        {
            carrer = 2;
        }
        else if ( this.m_eProfession  == A3_PROFESSION.Assassin )
        {
            carrer = 1;
        }

        return carrer;
    }

    private void SetFXPosition( bool ishaveRide ) {

        if ( ishaveRide )
        {
            var  meshAgent = this.m_curModel.GetComponent<NavMeshAgent>();

            if ( meshAgent == null )
            {
                return;
            }

            foreach ( var fxGo in m_Euiqp_Eff_New )
            {
                fxGo.transform.localPosition = Vector3.zero + new Vector3( 0 , -meshAgent.baseOffset , -0.2f );
                fxGo.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
            }

        }

        else {

            foreach ( var fxGo in m_Euiqp_Eff_New )
            {
                fxGo.transform.localPosition = Vector3.zero;
                fxGo.transform.localScale = Vector3.one;
            }
        }

    }

    private void BuilBonesMapping(GameObject parent)
    {
        var count =  parent.transform.childCount;

        for ( int i = 0 ; i < count ; i++ )
        {
            Transform _childTrans =  parent.transform.GetChild(i);

            bonesMapping.Add(_childTrans .gameObject.name, _childTrans );

        }
    }

    private void SetFEquipFxGo( GameObject go) {

        int fxCount = go.transform.childCount;

        for ( int i = 0 ; i < fxCount ; i++ )

        {
            var _childGo = go.transform.GetChild( i );

            string name = _childGo.name;

            Vector3 position = _childGo.transform.localPosition;
            Vector3 rotation = _childGo.transform.localEulerAngles;
            Vector3 scala = _childGo.transform.localScale;

            if ( bonesMapping.ContainsKey( name ) == false )
            {
                Debug.LogError( " 该时装特效命名错误" + "    name :  " + name );

                continue;
            }
            
            var fxGo = GameObject.Instantiate( _childGo );
            fxGo.gameObject.layer = this.m_nLayer;
            fxGo.name  = name;
            fxGo.SetParent( bonesMapping[ name ] );
            fxGo.localPosition =position;
            fxGo.localEulerAngles =rotation;
            fxGo.localScale =scala;
            fEquip_FXLst.Add( fxGo.gameObject );
        }

    }
}

