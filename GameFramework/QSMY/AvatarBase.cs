using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Cross;
namespace GameFramework
{
    public class AvatarBase
    {
        public GameObject _avatar;
        public Animation m_animator;

        string m_strCurPlayAnim;

        private GameObject con;
        public bool disposed = false;
        protected Dictionary<string, string> m_animPath = new Dictionary<string, string>();

        public AvatarBase(string id)
        {
            con = new GameObject();

            Variant conf = GraphManager.singleton.getCharacterConf(id);


            IAsset res_mesh = os.asset.getAsset<IAssetMesh>(conf["file"]._str, (IAsset ast) =>
            {
                if (disposed)
                    return;

                GameObject res_obj = (ast as AssetMeshImpl).assetObj;

                _avatar = GameObject.Instantiate(res_obj) as GameObject;
                m_animator = _avatar.GetComponent<Animation>();
                if (m_animator==null)
                    m_animator = _avatar.AddComponent<Animation>();
                _avatar.transform.SetParent(con.transform, false);

                if (conf.ContainsKey("ani"))
                {
                    foreach (Variant aniConf in conf["ani"].Values)
                    {
                        string aniName = null;
                        if (aniConf.ContainsKey("name"))
                            aniName = aniConf["name"]._str;
                        if (aniName == null || !aniConf.ContainsKey("file"))
                            Debug.LogError("表错误" + aniName);
                        if (aniName != null)
                            m_animPath[aniName] = aniConf["file"];
                    }
                    playAni("idle");
                }
            }, null,
    (IAsset ast, string err) =>
    {

        Debug.LogError("加载失败::" + conf["file"]._str);
    });
            (res_mesh as AssetImpl).loadImpl(false);


        }

        public void setParent(Transform trans)
        {
            con.transform.SetParent(trans, false);
        }

        public void setActive(bool b)
        {
            con.SetActive(b);
        }



        public void playAni(string anim)
        {
            if (m_animator == null) return;

            if (m_animator.GetClip(anim) == null)
            {

                if (!m_animPath.ContainsKey(anim))
                    return;

                IAsset res_mesh = os.asset.getAsset<IAssetSkAnimation>(m_animPath[anim], (IAsset ast) =>
                {
                    if (disposed)
                        return;

                    AnimationClip ani = (ast as AssetSkAnimationImpl).anim;
                    addAnim(anim, ani);
                    m_strCurPlayAnim = anim;

                    m_animator.Play(anim);
                }, null,
(IAsset ast, string err) =>
{

    Debug.LogError("加载失败::" + m_animPath[anim]);
});
                (res_mesh as AssetImpl).loadImpl(false);


                return;

            }
            else
            {
                if (m_strCurPlayAnim == anim)
                {
                    m_animator.Play(anim);
                }
                else
                {
                    m_animator.CrossFade(anim, .3f);
                    m_strCurPlayAnim = anim;
                }
            }
        }

        public void addAnim(string id, AnimationClip clip)
        {
            if (m_animator.GetClip(id) != null)
                m_animator.RemoveClip(id);

            m_animator.AddClip(clip, id);
        }

        public void dispose()
        {
            disposed = true;
            GameObject.Destroy(con);
        }

    }
}
