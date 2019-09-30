using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Cross;
using GameFramework;
using DG.Tweening;
namespace MuGame
{
    class MapEffMgr
    {
        public readonly static int TYPE_AUTO = 0;
        public readonly static int TYPE_LOOP = 1;

        public float interval = 0.2f;
        public float lifeCycle = 1f;
        float lastconbinedTime = 0f;


        private Dictionary<string, EffectItem> dEffectItem;
        private Dictionary<string, effData> dLifeTime;
        public static Dictionary<string, GameObject> dGo;

        private Transform con;
        TickItem process;
        public MapEffMgr()
        {
            dLifeTime = new Dictionary<string, effData>();
            dGo = new Dictionary<string, GameObject>();
            dEffectItem = new Dictionary<string, EffectItem>();
            GameObject go = new GameObject();
            go.name = "effects";
            con = go.transform;
            process = new TickItem(onUpdate);
            //con.SetParent(GameObject.Find("3DScene").transform, false);
            TickMgr.instance.addTick(process);

        }

        void onUpdate(float s)
        {
            List<EffectItem> l = new List<EffectItem>();
            foreach (EffectItem item in dEffectItem.Values)
            {
                item.update(s);
                if (item._disposed)
                    l.Add(item);
            }

            foreach (EffectItem item in l)
            {
                dEffectItem.Remove(item._id);
            }
        }

        public EffectItem addEffItem(string id, string path, int effType = 0, Transform effcon = null)
        {
            removeEffItem(id);
            float t;

            effData d = null;
            if (!dLifeTime.ContainsKey(path))
            {
                SXML xml = XMLMgr.instance.GetSXML("effect.eff", "id==" + id);
                if (xml == null) return null;

                d = new effData();
                d.path = xml.getString("file");

             
                    d.lifetime = xml.getInt("lenth") / 10;
     
                dLifeTime[path] = d;
            }
            else
            {
                d = dLifeTime[path];
            }
            t = d.lifetime;

            if (effType == TYPE_LOOP)
                t = -1;

            if (dEffectItem.ContainsKey(id))
                dEffectItem[id].dispose();

            EffectItem item = new EffectItem(id, d.path, t);
            dEffectItem[id] = item;

            if (effcon == null)
                item.setParent(con);
            else
                item.setParent(effcon);

            return item;
        }

        public void removeEffItem(string id)
        {
            if (dEffectItem.ContainsKey(id))
            {
                EffectItem item = dEffectItem[id];
                item.dispose();
                dEffectItem.Remove(id);
            }
        }

        public void clearAll()
        {
            foreach (EffectItem item in dEffectItem.Values)
            {
                item.dispose();
            }

            dEffectItem.Clear();

            for (int i = 0; i < con.childCount; i++)
            {
                GameObject.Destroy(con.GetChild(i).gameObject);
            }

        }







        public void playLineTo(string path, Vector3 begin, Vector3 end, int pointNum, float life = 0f)
        {
            Quaternion rot = Quaternion.LookRotation(end - begin);
            for (int i = 1; i < pointNum; i++)
            {
                Vector3 vec = Vector3.Lerp(begin, end, (float)i / (float)pointNum);
                //vec.x+= ConfigUtil.getRandom(-20,20)/10;
                //vec.z += ConfigUtil.getRandom(-20, 20) / 10;
                play(path, vec, rot, life);
            }
        }

        public void playMoveto(string path, Vector3 begin, Vector3 end, float life = 0f)
        {
            Vector3 vec = end - begin;
            if (vec == Vector3.zero)
                return;

            Quaternion rot = Quaternion.LookRotation(end - begin);

            play(path, begin, rot, life, (GameObject go) =>
            {
                go.transform.DOMove(end, life);
            });
        }

        public void play(string path, Transform tran, float life)
        {
            play(path, tran.position, tran.localEulerAngles, life);
        }

        public void play(string path, Transform tran, float rotY, float life)
        {
            Vector3 vec = tran.eulerAngles;
            vec.y = rotY;

            play(path, tran.position, vec, life);
        }

        public void play(string path, Vector3 pos, Quaternion qua, float life, Action<GameObject> handle = null)
        {
            getGameObject(path, (GameObject goa) =>
            {
                float lifetime = life;
                if (lifetime == 0f)
                    lifetime = dLifeTime[path].lifetime;

                initFadeInObj(goa, pos, qua, lifetime);
                if (handle != null)
                    handle(goa);
            });
        }

        public void play(string path, Vector3 pos, Vector3 eulerAngles, float life)
        {
            getGameObject(path, (GameObject goa) =>
                 {
                     float lifetime = life;
                     if (lifetime == 0f)
                         lifetime = dLifeTime[path].lifetime;

                     initFadeInObj(goa, pos, eulerAngles, lifetime);
                 });
        }




        private void getGameObject(string id, Action<GameObject> handle)
        {
            if (id == "" || id == null)
                return;
            if (!dLifeTime.ContainsKey(id))
            {
                Variant va = GraphManager.singleton.getEffectConf(id);
                if (va == null) return;

                effData d = new effData();
                d.path = va["file"];
                d.lifetime = va["lenth"]._float / 10;
                dLifeTime[id] = d;
            }

            string path = dLifeTime[id].path;

            if (!dGo.ContainsKey(id))
            {
                IAsset res_mesh = os.asset.getAsset<IAssetParticles>(path, (IAsset ast) =>
                {
                    GameObject res_obj = (ast as AssetParticlesImpl).assetObj;
                    dGo[id] = res_obj;
                    GameObject the_obj = GameObject.Instantiate(res_obj) as GameObject;
                    the_obj.transform.SetParent(con, false);
                    handle(the_obj);
                }, null,
                (IAsset ast, string err) =>
                {
                    //加载失败
                    Debug.LogError("加载特效失败" + id);
                });

                (res_mesh as AssetImpl).loadImpl(false);
            }
            else if (dGo[id] != null)
            {
                GameObject the_obj = GameObject.Instantiate(dGo[id]) as GameObject;
                the_obj.transform.SetParent(con, false);
                handle(the_obj);
            }
        }

        void initFadeInObj(GameObject go, Vector3 position, Vector3 eulerAngles, float life)
        {
            go.transform.position = position;
            go.transform.localEulerAngles = eulerAngles;
            fadeout fi = go.AddComponent<fadeout>();
            fi.lifeTime = life;
        }

        void initFadeInObj(GameObject go, Vector3 position, Quaternion qua, float life)
        {
            go.transform.position = position;
            go.transform.rotation = qua;
            fadeout fi = go.AddComponent<fadeout>();
            fi.lifeTime = life;
        }



        private static MapEffMgr _instacne;
        public static MapEffMgr getInstance()
        {
            if (_instacne == null)
                _instacne = new MapEffMgr();

            return _instacne;
        }
    }

    class effData
    {
        public string path;
        public float lifetime;
    }

    public class EffectItem
    {
        public static Dictionary<string, List<Action>> dLoading = new Dictionary<string, List<Action>>();

        public GameObject _goEffect;
        public string _id;
        public string _path;
        public bool _disposed = false;
        public Vector3 _pos = Vector3.zero;
        public Transform _parent;
        private float _scale = 1;

        private float _len = -1;

        public EffectItem(string id, string path, float len = -1)
        {
            _path = path;
            _id = id;
            if (len > 0)
                _len = Time.time + len;
            loadGo(path);
        }

        public float scale
        {
            get { return _scale; }
            set
            {
                if (_scale == value)
                    return;

                _scale = value;
                if (_goEffect != null)
                    _goEffect.transform.localScale = new Vector3(_scale, _scale, _scale);
            }
        }

        public bool isAutoRemove
        {
            get { return _len > 0; }
        }

        public Vector3 pos
        {
            get { return _pos; }
            set { if (_disposed)return; _pos = value; if (_goEffect != null)_goEffect.transform.position = value; }
        }

        public void setParent(Transform trans)
        {
            _parent = trans;
            if (_goEffect != null)
            {
                _goEffect.transform.SetParent(_parent, false);
            }
        }

        public void loadGo(string path)
        {
            MapEffMgr.dGo[path] = GAMEAPI.ABFight_LoadPrefab(path);
            onLoaded();
            //if (MapEffMgr.dGo.ContainsKey(path))
            //{
            //    onLoaded();
            //}
            //else if (dLoading.ContainsKey(path))
            //{
            //    dLoading[path].Add(onLoaded);
            //}
            //else
            //{
            //    dLoading[path] = new List<Action>();
            //    dLoading[path].Add(onLoaded);

            //    IAsset res_mesh = os.asset.getAsset<IAssetParticles>(path, (IAsset ast) =>
            //    {
            //        GameObject res_obj = (ast as AssetParticlesImpl).assetObj;
            //        MapEffMgr.dGo[path] = res_obj;

            //        List<Action> l = dLoading[path];
            //        foreach (Action a in l)
            //        {
            //            a();
            //        }
            //        dLoading.Remove(path);
            //    }, null,
            //       (IAsset ast, string err) =>
            //       {
            //           //加载失败
            //           Debug.LogError("加载特效失败" + _id + " " + path);
            //           dLoading.Remove(path);
            //       });


            //    (res_mesh as AssetImpl).loadImpl(false);
            //}
        }


        private void onLoaded()
        {
            if (_disposed)
                return;

            _goEffect = GameObject.Instantiate(MapEffMgr.dGo[_path]) as GameObject;
            _goEffect.transform.position = _pos;
            if (_parent != null)
                _goEffect.transform.SetParent(_parent, false);
            if (_scale != 1f)
                _goEffect.transform.localScale = new Vector3(_scale, _scale, _scale);
        }

        public void update(float s)
        {
            if (_len <= 0)
                return;

            if (_disposed)
                return;

            if (_len <= Time.time)
                dispose();
        }

        public void dispose()
        {
            if (_disposed) return;
            if (_goEffect)
                GameObject.Destroy(_goEffect);
            _disposed = true;
            _parent = null;
        }
    }


    public class fadeout : MonoBehaviour
    {
        private float _lifeTimer;
        float startTime;

        public float lifeTime
        {
            set
            {
                if (value <= 0) _lifeTimer = -1;
                _lifeTimer = Time.time + value;
            }
        }



        void Start()
        {
            startTime = Time.time;
            //    MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_lifeTimer == -1f)
                return;

            if (Time.time > _lifeTimer)
            {
                //DestroyImmediate(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
