using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;
using MuGame.Qsmy.model;

namespace MuGame.role
{
    /// <summary>
    /// 飞鸟宠物类
    /// </summary>
    public class PetBird : MonoBehaviour
    {
        private GameObject _path;
        private Transform _curPath;
        private Tween _curPathTween;
        private Vector3 _ownerLastPos;

        private float _thinkTimer;
        private float _showTimer;

        private bool _mustFollow;
        private bool _canShow;

        private Animator _anim;
        private readonly float _thinkInterval = 2.0f;

        public GameObject Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public PetBird()
        {
            Path = null;
            _thinkTimer = 0;
            _showTimer = 0;
            _curPath = null;
            _curPathTween = null;
            _ownerLastPos = new Vector3(0, 0, 0);
            _mustFollow = true;
            _canShow = false;
        }

        void Start()
        {
            transform.position = Path.transform.position;
            _anim = this.GetComponent<Animator>();
            if (_anim == null)
            {
                _anim = U3DAPI.DEF_ANIMATOR;
            }
        }

        bool goto_dropitem = false;//是否有可拾取的东西
        Vector3 drop_pos = Vector3.zero;//要拾取的东西的位置
        void Update()
        {
            //判断一下鸟是不是自己的
            //看看有没有东西要我去捡，没得话在更着主人
            if (!PlayerModel.getInstance().havePet) return;            
            if (SelfRole._inst.m_myPetBird != this)
            {
                goto_dropitem = false;
                goto BIRD_ACTION;
            }
            pickup_drop();
            BIRD_ACTION:
            if (goto_dropitem)
            {
                transform.LookAt(drop_pos);
                transform.position = Vector3.Lerp(transform.position, drop_pos, Time.deltaTime * 2.0f);
            }
            else
            {                
                _thinkTimer += Time.deltaTime;
                if (_thinkTimer >= _thinkInterval)
                {
                    _thinkTimer = 0;
                    this.Think();
                }

                DoAction();
            }

            //  print("鸟和主人的距离：" + Vector3.Distance(SelfRole._inst.m_curModel.position, transform.position));
        }
        private Vector3 calc_ori, calc_tar, calc_cur;
        private DropItem pickTarget, nearest = null;
        private void pickup_drop()
        {
            #region if
            if (nearest == null || !nearest.gameObject)
            {
                DropItem drpitm;
                float min = float.MaxValue;
                Dictionary<uint, DropItem>.Enumerator etor = BaseRoomItem.instance.dDropItem_own.GetEnumerator();
                //if (!etor.MoveNext())
                // goto_dropitem = false;
                //所有物品都时间到了
                if (BaseRoomItem.instance.dDropItem.Count <= 0)
                {
                    goto_dropitem = false;
                    return;
                }
                while (etor.MoveNext())
                {
                    drpitm = etor.Current.Value;
                    if (drpitm != null && drpitm.gameObject)
                    {
                        //自己是否可拾取
                        bool hasOwner = drpitm.itemdta.ownerId != 0,
                             notMine = drpitm.itemdta.ownerId != PlayerModel.getInstance().cid
                             || (TeamProxy.getInstance().MyTeamData != null && drpitm.itemdta.ownerId == TeamProxy.getInstance().MyTeamData.teamId);                     
                        if (hasOwner && notMine)
                            continue;
                        //!--距离超过拾取范围
                        if (Vector3.Distance(drpitm.transform.position/*.ConvertToGamePosition()*/, transform.position) > StateInit.Instance.PickDistance)
                            continue;
                        //!--如果背包满且不是金币，不捡
                        if (a3_BagModel.getInstance().curi <= a3_BagModel.getInstance().getItems().Count && (
                            a3_BagModel.getInstance().getItemNumByTpid((uint)drpitm.itemdta.tpid) == 0 ||
                            a3_BagModel.getInstance().getItemNumByTpid((uint)drpitm.itemdta.tpid) >= a3_BagModel.getInstance().getItemDataById((uint)drpitm.itemdta.tpid).maxnum)
                            && drpitm.itemdta.tpid != 0)
                            continue;

                        if (A3_RollModel.getInstance().rollMapping.ContainsKey(drpitm.itemdta.dpid) && !A3_RollModel.getInstance().rollMapping[drpitm.itemdta.dpid].isCanPick) continue;

                        else if (drpitm.canPick && AutoPlayModel.getInstance().WillPick((uint)drpitm.itemdta.tpid))
                        {
                            pickTarget = drpitm;
                            if (nearest == null)
                                nearest = pickTarget;
                            else
                            {
                                float dis = Vector3.Distance(pickTarget.transform.position/*.ConvertToGamePosition()*/, transform.position);
                                if (dis < min)
                                {
                                    min = dis;
                                    nearest = pickTarget;
                                }
                            }
                        }
                        else
                            pickTarget = null;
                    }
                }
                calc_ori = transform.position;
                if (nearest != null)
                {
                    //SelfRole._inst.SetDestPos(nearest.transform.position);
                    if (goto_dropitem ^ true)
                        pickTarget = null;
                    goto_dropitem = true;
                    drop_pos = nearest.transform.position;
                    calc_tar = nearest.transform.position;

                   // if (A3_RollModel.getInstance().rollMapping.ContainsKey(nearest.itemdta.dpid))  // 该物品是roll点物品
                    //{
                    //    bool isCanPick = A3_RollModel.getInstance().rollMapping[nearest.itemdta.dpid].isCanPick;
                    //    if (!isCanPick)
                    //    {
                    //        nearest = null;
                    //        goto_dropitem = false;
                    //        calc_tar = Vector3.zero;
                    //        drop_pos = Vector3.zero;
                    //    }else
                    //    {
                    //        return;
                    //    }
                    //}
                    //else {
                        return;
                    //}
                }
                else
                {
                    goto_dropitem = false;
                    calc_tar = Vector3.zero;
                    drop_pos = Vector3.zero;
                }
            }
            #endregion
            #region else
            else
            {
                // print("小鸟和物品之间的距离是多少：" + Vector3.Distance(transform.position, nearest.transform.position/*.ConvertToGamePosition()*/));
                if (Vector3.Distance(transform.position, nearest.transform.position) < 0.5f)
                {
                    if (nearest.gameObject != null)
                    {
                        nearest.PickUpItem();
                        // SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, false);
                        goto_dropitem = false;
                        drop_pos = Vector3.zero;
                        nearest = null;
                    }
                    else
                    {
                        goto_dropitem = false;
                        drop_pos = Vector3.zero;
                        nearest = null;
                    }

                }
                // bool isOutOfTouch = Vector3.Distance(nearest.transform.position.ConvertToGamePosition(), transform.position) > StateInit.Instance.PickDistance;

                if (nearest != null)
                {
                    //print("物品和pet之间的距离：" + Vector3.Distance(nearest.transform.position, transform.position) + ".限定距离：" + StateInit.Instance.PickDistance);
                    if (Vector3.Distance(nearest.transform.position/*.ConvertToGamePosition()*/, transform.position) <= StateInit.Instance.PickDistance)
                    {
                        //SelfRole._inst.TurnToPos(nearest.transform.position);
                        //SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, true);
                        if (goto_dropitem ^ true)
                            pickTarget = null;
                        goto_dropitem = true;
                        drop_pos = nearest.transform.position;

                    }
                    else
                    {
                        nearest = null;
                        // SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, false);
                        goto_dropitem = false;
                        drop_pos = Vector3.zero;
                    }
                }
                calc_cur = Path.transform.position;
                if (nearest == null || nearest.gameObject == null)
                {
                    goto_dropitem = false;
                    drop_pos = Vector3.zero;
                    //SelfRole._inst.m_moveAgent.Stop();
                    //SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, false);
                    //if (SelfRole.fsm.Autofighting)
                    //    SelfRole.fsm.ChangeState(StateAttack.Instance);
                    //else
                    //    SelfRole.fsm.ChangeState(StateIdle.Instance);
                    //return;
                }
            }
            #endregion
        }
        private void FlyPath()
        {
            if (!Path) return;

            Transform ptrans = Path.transform;
            var cnt = ptrans.childCount;
            print("cnt:" + cnt);
            if (cnt <= 0) return;

            // 随机获取一条路径
            Random rd = new Random();
            var idx = rd.Next(cnt);
            _curPath = ptrans.GetChild(idx);

            cnt = _curPath.childCount;
            if (cnt <= 0) return;

            // 准备路径点
            Vector3[] waypoints = new Vector3[cnt];
            Transform t = null;
            for (int i = 0; i < cnt; i++)
            {
                t = _curPath.GetChild(i);
                waypoints[i] = t.position;
            }

            _curPathTween = transform.DOPath(waypoints, 0.5f, PathType.CatmullRom, PathMode.Full3D, 5)
                .OnWaypointChange(this.OnWayPointChange)
                .OnStart(this.OnPathStart)
                .OnComplete(this.OnPathComplete)
                .OnPause(this.OnPathPause)
                .OnKill(this.OnPathKill)
                .SetLookAt(0.01f)
                .SetEase(Ease.Linear)
                .SetOptions(false, AxisConstraint.None, AxisConstraint.X | AxisConstraint.Z)
                .SetSpeedBased();

            _anim.SetBool("stop", false);
        }

        private void Think()
        {
            // Vector3.Distance(SelfRole._inst.m_curModel.position, transform.position) < 5
            //!--主人走了吗?
            var ownPos = Path.transform.position;
            if (ownPos.Equals(_ownerLastPos))
            {//!--还没走，想想show点啥呢
                //print("主人没走");
                _mustFollow = false;
                _showTimer += _thinkInterval;
                if (_showTimer >= 35.0f)
                {
                    _canShow = true;
                    _showTimer = 0;
                }
            }
            else
            {//!--已经走了,我必须跟上,先别show了
             // print("主人走了");
                _mustFollow = true;
                _canShow = false;
                _showTimer = 0;
                _ownerLastPos.Set(ownPos.x, ownPos.y, ownPos.z);
            }
            //切换地图进副本，主角不动时，会一直往前飞（加一个距离限制）
            if (Vector3.Distance(SelfRole._inst.m_curModel.position, transform.position) >= 3)
                _mustFollow = true;
        }

        private void DoAction()
        {
            if (_mustFollow)
            {//跟上主人
                if (_curPathTween != null)
                {
                    _curPathTween.Kill(false);
                    _curPathTween = null;
                }

                Vector3 forwardDir = Path.transform.position - transform.position;
                float dis = forwardDir.magnitude;

                //比较近的距离，直接待机过去, 比较远的距离飞过去
                _anim.SetBool("stop", dis < 1.0f ? true : false);
                _anim.SetBool("fly", true);
                //Debug.LogError(SelfRole._inst.m_curModel.position+"：：：主人坐标::：宠物坐标：："+Path.transform.position);
                //调整朝向和位置
                forwardDir.y = 0;
                transform.forward = Vector3.Lerp(transform.forward, forwardDir, Time.deltaTime * 8.0f);
                transform.position = Vector3.Lerp(transform.position, Path.transform.position, Time.deltaTime * 1.0f);
            }
            else
            {
                if (_canShow)
                {
                    FlyPath();

                    //!--我show过了,下一次可以show的时候再来吧
                    _canShow = false;
                }
            }
        }

        #region DoTween Callback

        /// <summary>
        /// DOTween的路径点事件函数,需要注意的是wpIdx是下一个路径点,即行径中的下一个目标
        /// </summary>
        /// <param name="wpIdx">下一个路径点</param>
        private void OnWayPointChange(int wpIdx)
        {
            int last_idx = wpIdx - 1;
            if (last_idx >= _curPath.childCount
                || last_idx < 0)
                return;

            Transform curWP = _curPath.GetChild(last_idx);
            string wpName = curWP.name;
            if (wpName == null || wpName.Length <= 0)
                return;

            string[] para = wpName.Split('_');
            if (para[0] == "stop")
            {//!--该节点要待机一段时间
                _anim.SetBool("stop", true);
                StartCoroutine(OnPathPauseCoroutine(float.Parse(para[1])));
            }
            else if (para[0] == "land")
            {
                _anim.SetBool("fly", false);
                StartCoroutine(OnPathPauseCoroutine(float.Parse(para[1])));
            }
            else if (para[0] == "fly")
            {
                _anim.SetBool("fly", true);
            }
        }

        IEnumerator OnPathPauseCoroutine(float pauseTm)
        {
            _curPathTween.Pause();
            yield return new WaitForSeconds(pauseTm);

            _anim.SetBool("stop", false);
            _curPathTween.Play();

        }

        private void OnPathStart()
        {

        }

        private void OnPathComplete()
        {
            _anim.SetBool("stop", true);
            _curPathTween = null;
            _curPath = null;
        }

        private void OnPathPause()
        {

        }

        private void OnPathKill()
        {
            _curPathTween = null;
            _curPath = null;
        }
        #endregion
    }

    /// <summary>
    /// 飞鸟管理类
    /// </summary>
    public class PetBirdMgr
    {
        private GameObject _birdPrefab = null;
        private GameObject _pathPrefab = null;

        private ProfessionRole _owner = null;

        private static PetBirdMgr _inst = null;
        public static PetBirdMgr Inst
        {
            get
            {
                if (_inst != null)
                    return _inst;

                _inst = new PetBirdMgr();
                _inst._init();
                return _inst;
            }
        }

        private void OnPetStageChange()
        {
            Transform stop = SelfRole._inst.m_curModel.FindChild("birdstop");
            for (int i = 0; i < stop.childCount; i++)
            {
                GameObject.Destroy(stop.GetChild(i));
            }

            A3_PetModel petModel = A3_PetModel.getInstance();

            SXML stageXML = petModel.PetXML.GetNode("pet.stage", "stage==" + petModel.Stage.ToString());
            string avatar = stageXML.getString("avatar");

            GameObject birdPrefab = GAMEAPI.ABModel_LoadNow_GameObject("profession_" + avatar);
            GameObject pathPrefab = GAMEAPI.ABModel_LoadNow_GameObject("profession_birdpath");
            if (_birdPrefab == null || pathPrefab == null)
                return;

            GameObject bird = GameObject.Instantiate(_birdPrefab, stop.position, Quaternion.identity) as GameObject;
            GameObject path = GameObject.Instantiate(_pathPrefab, stop.position, Quaternion.identity) as GameObject;
            if (bird == null || path == null)
                return;

            path.transform.parent = stop;

            PetBird bd = null;
            bd = bird.AddComponent<PetBird>();
            bd.Path = path;
        }

        private void _init()
        {

        }
    }
}