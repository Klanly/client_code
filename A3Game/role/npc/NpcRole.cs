using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MuGame
{
    public class NpcRole : MonoBehaviour, INameObj
    {
        public static GameObject GO_TASK_FINISH;
        public static GameObject GO_TASK_UNFINISH;
        public static GameObject GO_TASK_GET;

        public int id;
        public string name;
        public string openid;
        public bool nav = true;
        public List<string> lDesc;
        public List<string> newDesc;
        public Action handle;

        public List<int> listTaskId;
        //public Dictionary<int, Action> dicNpcHandle;

        public Vector3 talkOffset;
        public Vector3 talkScale;
        public Transform headNub;
        public Vector3 headOffset;
        public string roleName { get { return name; } set { name = value; } }
        private int _curhp = 100;
        public int curhp { get { return _curhp; } set { _curhp = value; } }
        private int _maxHp = 100;
        public int maxHp { get { return _maxHp; } set { _maxHp = value; } }
        public bool isDead = false;
        public bool canbehurt = false;
        private int _title_id = 0;
        public int title_id { get { return _title_id; } set { _title_id = value; } }
        private int _rednm = 0;
        public int rednm { get { return _rednm; } set { _rednm = value; } }
        public uint _hidbacktime = 0;

        private int _lvlsideid = 0;
        public int spost_lvlsideid { get { return _lvlsideid; } set { _lvlsideid = value; } }
        public uint hidbacktime { get { return _hidbacktime; } set { _hidbacktime = value; } }
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
        public int lastHeadPosTick = 0;
        public Vector3 lastHeadPos = Vector3.zero;
        public UnityEngine.Vector3 getHeadPos()
        {
            if (SceneCamera.m_curCamera == null)
                return UnityEngine.Vector3.zero;

            float tempx = 0;
            if (SelfRole._inst.m_curPhy.position.x > transform.position.x)
                tempx = SelfRole._inst.m_curPhy.position.x - transform.position.x;
            else
                tempx = transform.position.x - SelfRole._inst.m_curPhy.position.x;

            float tempy = 0;
            if (SelfRole._inst.m_curPhy.position.y > transform.position.y)
                tempy = SelfRole._inst.m_curPhy.position.y - transform.position.y;
            else
                tempy = transform.position.y - SelfRole._inst.m_curPhy.position.y;

            float temp = tempx + tempy;

            if (temp > 9f)
                return UnityEngine.Vector3.zero;




            int curTime = TickMgr.tickNum;

            if (lastHeadPosTick == curTime)
                return lastHeadPos;

            lastHeadPosTick = curTime;


            Vector3 v3;
            if (headNub != null)
                v3 = headNub.position;
            else
                v3= transform.position + headOffset;

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

        Animator anim;

        string m_strModelPath;
        void Start()
        {
            int npc_id = -1;
            int.TryParse(gameObject.name, out npc_id);
            if (NpcMgr.instance.xNpc.ContainsKey(npc_id))
            {
                m_strModelPath = NpcMgr.instance.xNpc[npc_id].model;
                GAMEAPI.ABModel_LoadGameObject(m_strModelPath, Model_LoadedOK, null);
            }
            SXML ncpxml = XMLMgr.instance.GetSXML("npcs");
            SXML xml =  ncpxml.GetNode("npc", "id==" + id);
            if (xml != null) {
                name = xml.getString("name");
            }


                        BoxCollider box = GetComponent<BoxCollider>();
            if (box == null)
            {
                box = gameObject.AddComponent<BoxCollider>();
                box.center = new Vector3(0, 1.2f, 0);
                box.size = new Vector3(1, 2, 1);
            }

            headOffset = box.center;
            headOffset.y = headOffset.y + (box.size.y / 2);

            if (nav)
            {
                NavMeshAgent move = GetComponent<NavMeshAgent>();
                if (move == null)
                    move = gameObject.AddComponent<NavMeshAgent>();
                move.speed = 0.1f;
                move.avoidancePriority = 0;
                move.walkableMask = NavmeshUtils.allARE;
            }

            gameObject.layer = EnumLayer.LM_NPC;

            NpcMgr.instance.addRole(this);

            headNub = transform.Find("headnub");
            if (headNub == null)
                headNub = transform.Find("Bip001 HeadNub");


            PlayerNameUIMgr.getInstance().show(this);  
        }

        private void Model_LoadedOK(UnityEngine.Object model_obj, System.Object data)
        {
            GameObject obj_prefab = model_obj as GameObject;
            if (obj_prefab == U3DAPI.DEF_GAMEOBJ || obj_prefab == null)
            {
                Debug.LogError("not find model = " + m_strModelPath);
                model_obj = U3DAPI.U3DResLoad<GameObject>("def_role");
            }

            GameObject npcObj = GameObject.Instantiate(model_obj) as GameObject;
            npcObj.transform.SetParent(gameObject.transform, false);
            npcObj.transform.SetAsFirstSibling();
            foreach (Transform tran in npcObj.gameObject.GetComponentsInChildren<Transform>())
            {
                if(model_obj.name == "npc_111")
                    tran.gameObject.layer = EnumLayer.LM_SELFROLE;
                else
                    tran.gameObject.layer = EnumLayer.LM_NPC;
            }

            anim = npcObj.transform.FindChild("model").GetComponent<Animator>();
            anim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        }

        public void createAvatar()
        {

        }
        public void disposeAvatar()
        {

        }


        private GameObject taskIcon;
        public void refreshTaskIcon(NpcTaskState state)
        {
            clearTaskIcon();
            switch (state)
            {
                case NpcTaskState.NONE:
                    break;
                case NpcTaskState.UNREACHED:
                    GO_TASK_GET = GAMEAPI.ABFight_LoadPrefab("FX_comFX_fx_renwu_FX_com_tanhao");
                    taskIcon = GameObject.Instantiate(GO_TASK_GET) as GameObject;
                    taskIcon.transform.SetParent(transform, false);
                    break;
                case NpcTaskState.REACHED:
                    GO_TASK_UNFINISH = GAMEAPI.ABFight_LoadPrefab("FX_comFX_fx_renwu_FX_com_wenhao_red");
                    taskIcon = GameObject.Instantiate(GO_TASK_UNFINISH) as GameObject;
                    taskIcon.transform.SetParent(transform, false);
                    break;
                case NpcTaskState.UNFINISHED:
                    GO_TASK_UNFINISH = GAMEAPI.ABFight_LoadPrefab("FX_comFX_fx_renwu_FX_com_wenhao_red");
                    taskIcon = GameObject.Instantiate(GO_TASK_UNFINISH) as GameObject;
                    taskIcon.transform.SetParent(transform, false);
                    break;
                case NpcTaskState.FINISHED:
                    GO_TASK_FINISH = GAMEAPI.ABFight_LoadPrefab("FX_comFX_fx_renwu_FX_com_wenhao_yellow");
                    taskIcon = GameObject.Instantiate(GO_TASK_FINISH) as GameObject;
                    taskIcon.transform.SetParent(transform, false);
                    break;
                default:
                    break;
            }

            if (taskIcon != null)
            {
                Vector3 vec = headOffset;
                vec.y = vec.y + 1;

                //屏蔽角度
                //if (SceneCamera.m_curCamGo != null)
                //    taskIcon.transform.forward = -SceneCamera.m_curCamGo.transform.forward;


                taskIcon.transform.localPosition = vec;

            }
        }

        public void clearTaskIcon()
        {
            if (taskIcon != null)
                GameObject.Destroy(taskIcon);
            taskIcon = null;
        }

        public void dispose()
        {
            PlayerNameUIMgr.getInstance().hide(this);
        }
        
        public void onClick()
        {          
            if (!NpcMgr.instance.can_touch)
                return;

            anim.SetTrigger("talk");
            if (handle != null)
            {
                Action temp = handle;
                dialog.showTalk(newDesc, temp, this);
                handle = null;
                return;
            }

            if (listTaskId != null || openid != "")
            {
                List<string> ldialog = new List<string>() { ContMgr.getCont("NpcRole") };
                dialog.showTalk(ldialog, null, this);
            }

            //if (openid != "")
            //{
            //    InterfaceMgr.getInstance().open(openid);
            //}
            else
            {
                dialog.showTalk(lDesc, null, this);
            }
            a1_gamejoy.canClick = false;
        }

        public void OnRefreshTitle()
        {
            PlayerNameUIMgr.getInstance().refreshTitlelv(this, title_id);
        }

        public void playSkill()
        {
            anim.SetTrigger("skill");
        }
    }
}
