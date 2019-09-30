using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public static class U3DAPI
{
    public static GameObject DEF_GAMEOBJ = new GameObject("DEF_GAMEOBJ");
    public static Transform DEF_TRANSFORM = DEF_GAMEOBJ.transform;
    public static Animator DEF_ANIMATOR = null;
    public static Texture2D DEF_TEX2D = null;
    public static Sprite DEF_SPRITE = null;
    public static SkinnedMeshRenderer DEF_SKINNEDMESHRENDERER;

    public static GameObject FX_POOL_OBJ = new GameObject("FX_POOL");
    public static Transform FX_POOL_TF = FX_POOL_OBJ.transform;

    public static void Init_DEFAULT()
    {
        DEF_GAMEOBJ.name = "DEF_GAMEOBJ";

        GameObject obj_prefab = U3DAPI.U3DResLoad<GameObject>("default/skinned_mesh_renderer");
        DEF_SKINNEDMESHRENDERER = obj_prefab.GetComponent<SkinnedMeshRenderer>();
        DEF_ANIMATOR = obj_prefab.GetComponent<Animator>();

        DEF_TEX2D = new Texture2D(4, 4);

        DEF_SPRITE = Sprite.Create(DEF_TEX2D, new Rect(0f, 0f, 4f, 4f), new Vector2(0f, 0f));
    }

    private static bool s_functionbar_mode = false;
    private static LightProbes s_curLightPro = null;
    private static Color s_curAmLight;
    public static void functionbar_ChangeTo()
    {
        if (s_functionbar_mode == false)
        {
            s_functionbar_mode = true;
            s_curAmLight = RenderSettings.ambientLight;
            RenderSettings.ambientLight = new Color(0f, 0f, 0f);
        }
    }


    public static void functionbar_BackFrom()
    {
        //if (s_curLightPro != null)
        //{
        //    LightmapSettings.lightProbes = s_curLightPro;
        //    RenderSettings.ambientLight = s_curAmLight;
        //    s_curLightPro = null;
        //}

        if (s_functionbar_mode == true)
        {
            s_functionbar_mode = false;
            RenderSettings.ambientLight = s_curAmLight;
        }
    }

    //设置一些Unity的常用接口，为热更新做准备
    public static T U3DResLoad<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }

}

namespace MuGame
{
    static class IOS_ADDFUN
    {
        public static void QSTDOrderBy(this Dictionary<int, TaskData> dic)
        {
            List<KeyValuePair<int, TaskData>> my_list = new List<KeyValuePair<int, TaskData>>(dic);


            my_list.Sort(delegate (KeyValuePair<int, TaskData> first, KeyValuePair<int, TaskData> next)
            {
                int first_s = first.Key;
                int next_s = next.Key;

                //if (first.Value.isEnd == true) first_s -= 100000;
                //if (next.Value.isEnd == true ) next_s -= 100000;

                return first_s.CompareTo(next_s);

            });

            dic.Clear();

            foreach (KeyValuePair<int, TaskData> pair in my_list)
            {
                dic.Add(pair.Key, pair.Value);
            }
        }

        //public static void QSEQOrderBy(this Dictionary<string, BagItemData> dic)
        //{
        //    List<KeyValuePair<string, BagItemData>> my_list = new List<KeyValuePair<string, BagItemData>>(dic);

        //    my_list.Sort(delegate(KeyValuePair<string, BagItemData> first, KeyValuePair<string, BagItemData> next)
        //    {
        //        int first_s = int.Parse(first.Key);
        //        int next_s = int.Parse(next.Key);
        //        return first_s.CompareTo(next_s);

        //    });

        //    dic.Clear();

        //    foreach (KeyValuePair<string, BagItemData> pair in my_list)
        //    {
        //        dic.Add(pair.Key, pair.Value);
        //    }
        //}
        //邮箱
        public static void mailOderBy(this List<mailData> list)
        {
            list.Sort(delegate (mailData first, mailData next)
            {
                int t1 = first.seconds;
                int t2 = next.seconds;
                return t1.CompareTo(t2);
            });
        }
        ////家族贡献
        //public static void donateOderBy(this List<donateData> list)
        //{
        //    list.Sort(delegate(donateData first, donateData next)
        //    {
        //        int d1 = (int)first.donate;
        //        int d2 = (int)next.donate;
        //        return d1.CompareTo(d2);
        //    });
        //}

        //成就加称号
        //public static void AchieveOrderBy(this List<Data> list)
        //{
        //    //List<Data> my_list = new List<Data>();
        //    list.Sort(delegate(Data first, Data next)
        //    {
        //        int first_s = first.category;
        //        int next_s = next.category;

        //        if (first_s > next_s)
        //        {
        //            return 1;
        //        }
        //        else if (first_s < next_s)
        //        {
        //            return -1;
        //        }
        //        else
        //        {
        //            if (first.id > next.id)
        //            {
        //                return 1;
        //            }
        //            else if (first.id == next.id)
        //            {
        //                return 0;
        //            }
        //            else
        //            {
        //                return -1;
        //            }
        //        }
        //    });
        //}

        //public static void titileOrderBy(this List<DataTitle> list)
        //{
        //    list.Sort(delegate(DataTitle first, DataTitle next)
        //    {
        //        int first_s = first.para;
        //        int next_s = next.para;
        //        return first_s.CompareTo(next_s);
        //    });

        //}
        ////好友
        //public static void friendOrderBy(this List<setFriend> list)
        //{
        //    list.Sort(delegate(setFriend first, setFriend next)
        //    {
        //        int first_s = first.online;
        //        int next_s = next.online;

        //        if (first_s > next_s)
        //        {
        //            return 1;
        //        }
        //        else if (first_s < next_s)
        //        {
        //            return -1;
        //        }
        //        else
        //        {
        //            if (first.level<next.level)
        //            {
        //                return 1;
        //            }
        //            else if (first.level > next.level)
        //            {
        //                return -1;
        //            }
        //            else
        //            {
        //                if (first.cid > next.cid)
        //                {
        //                    return 1;
        //                }
        //                else if (first.cid < next.cid)
        //                {
        //                    return -1;
        //                }
        //                else
        //                {
        //                    return 0;
        //                }
        //            }

        //        }
        //    });
        //}
        //比武场：
        //public static void arenaOrderBy(this List<Datas> list)
        //{
        //    list.Sort(delegate(Datas frist, Datas next)
        //    {
        //        int first_s = frist.rank;
        //        int next_s = next.rank;
        //        return first_s.CompareTo(next_s);
        //    });
        //}
        //7天乐之等级礼包
        //public static void active3OrderBy(this List<lv_Reward> list)
        //{
        //    list.Sort(delegate(lv_Reward frist, lv_Reward next)
        //    {
        //        int first_s = frist.lv;
        //        int next_s = next.lv;
        //        return first_s.CompareTo(next_s);
        //    });
        //}
        //7天乐之礼包
        //public static void active4OrderBy(this List<Package_Promotion> list)
        //{
        //    list.Sort(delegate(Package_Promotion frist, Package_Promotion next)
        //    {
        //        int first_s = frist.id;
        //        int next_s = next.id;
        //        return first_s.CompareTo(next_s);
        //    });
        //}


    }
}
