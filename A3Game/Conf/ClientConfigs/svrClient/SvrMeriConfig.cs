using System;
using GameFramework;  using Cross;

using System.Collections.Generic;

namespace MuGame
{
    public class SvrMeriConfig : configParser
    {
        public SvrMeriConfig( ClientConfig m ):base(m)
        {
            
        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new SvrMeriConfig(m as ClientConfig);
        }

        //-----------------------穴位相关配置          start-------------------------
        //门派1的筋脉信息
        private Variant meri_arr1 = new Variant();
        //门派2的筋脉信息
        private Variant meri_arr2 = new Variant();
        //门派3的筋脉信息
        private Variant meri_arr3 = new Variant();
        //门派4的筋脉信息
        private Variant meri_arr4 = new Variant();
        //门派5的筋脉信息
        private Variant meri_arr5 = new Variant();
        /**
         *获得门派穴位数组 
         */
        private Variant get_carr_meri_arr(int carr)
        {
            if (carr == 1)
            {
                return meri_arr1;
            }
            else if (carr == 2)
            {
                return meri_arr2;
            }
            else if (carr == 3)
            {
                return meri_arr3;
            }
            else if (carr == 4)
            {
                return meri_arr4;
            }
            else if (carr == 5)
            {
                return meri_arr5;
            }
            return null;
        }

        /**
         *通过门派获得门派下的编号。 
         *@cacupArr 门派编号
         */
        public Variant get_meri_data_carr(int carr)
        {
            Variant meir_arr = get_carr_meri_arr(carr);

            if (meir_arr.Count == 0)
            {
                //如果门派数据没有构造。那么。把门派有的穴位数据。替换默认的数据
                //获得门派对应的编号
                Variant carrObj = get_carr_meri(carr);
                //获得默认的筋脉数据
                Variant meriArr = get_meri_data();
                Variant c_meir_arr = new Variant();
                c_meir_arr = meriArr.clone();
                //c_meir_arr = Clone(meriArr);
                meir_arr = c_meir_arr;

                if (carr == 1)
                {
                    meri_arr1 = meir_arr;
                }
                else if (carr == 2)
                {
                    meri_arr2 = meir_arr;
                }
                else if (carr == 3)
                {
                    meri_arr3 = meir_arr;
                }
                else if (carr == 4)
                {
                    meri_arr4 = meir_arr;
                }
                else if (carr == 5)
                {
                    meri_arr5 = meir_arr;
                }
                if (carrObj == null || carrObj["meri"] == null)
                {
                    //如果没有默认为空的
                    return meir_arr;
                }

                //默认的穴位
                Variant acupArr = null;
                //门派对应的穴位
                Variant cacupArr = null;
                foreach (string i in carrObj["meri"]._arr)
                {
                    if (carrObj["meri"][i] == null || carrObj["meri"][i]["acup"] == null)
                    {
                        continue;
                    }
                    cacupArr = carrObj["meri"][i]["acup"];
                    acupArr = meir_arr[i]["acup"];
                    foreach (string m in cacupArr._arr)
                    {
                        acupArr[m] = cacupArr[m];
                    }
                }
            }

            return meir_arr;
        }

        public Variant Clone()
        {
            return null;
        }

        /**
         *获得默认的筋脉数据 
         */
        public Variant get_meri_data()
        {
            if (m_conf == null)
                return null;
            if (m_conf["meri"] == null)
                return null;

            return m_conf["meri"];
        }

        /**
         *获得本门派的筋脉信息 
         */
        public Variant get_meri_carr_data_by_id(int merid, int carr)
        {
            Variant arr = get_meri_data_carr(carr);
            Variant mobj = null;
            foreach (string i in arr._arr)
            {
                mobj = arr[i];
                if (mobj["id"] == merid)
                {
                    return mobj;
                }
            }
            return null;
        }

        /**
         *获得开通队列的对应信息
         * @param cnt 队列信息 
         */
        public Variant get_meri_cdcnt(int cnt)
        {
            if (m_conf == null)
                return null;

            if (m_conf["cdcnt"] == null)
                return null;

            Variant obj = m_conf["cdcnt"];

            Variant oo = null;
            foreach (string i in obj._arr)
            {
                oo = obj[i];
                if (oo["cnt"] == cnt)
                    return oo;
            }

            return null;
        }

        /**
         *获得可开通队列的长度 
         */
        public int get_meri_cdcnt_cnt()
        {
            if (m_conf == null)
                return 0;

            if (m_conf["cdcnt"] == null)
                return 0;

            Variant obj = m_conf["cdcnt"];

            int index = 0;

            foreach (string i in obj._arr)
            {
                index++;
            }

            return index;
        }

        /**
         *按照筋脉编号获得筋脉信息(默认的)
         * @merid 筋脉编号。 
         */
        public Variant get_meri_data_by_id(int merid)
        {
            Variant arr = get_meri_data();
            Variant mobj = new Variant();
            foreach (string i in arr._arr)
            {
                mobj = arr[i];
                if (mobj["id"] == merid)
                {
                    return mobj;
                }
            }
            return null;
        }

        public Variant get_carr_meri(int carr)
        {
            if (m_conf == null)
                return null;
            Variant meri_carr = m_conf["carr"];

            return meri_carr[carr];
        }

        public Variant get_acup_lvl(int lvl)
        {
            if (m_conf["acup_lvl"] == null)
                return null;
            Variant acup_lvl = m_conf["acup_lvl"];

            if (lvl >= acup_lvl.Count)
                return null;

            return acup_lvl[lvl];
        }

        public int get_stage_apt(int stage)
        {

            int level = 1;
            int apt = 0;

            for (; ; )
            {

                Variant data = get_acup_lvl(++level);

                if (data == null)
                    break;

                apt += data["att_per_min"];

                if ((apt / 100) > stage)
                    break;
            }

            return apt;
        }

        //获取从1级 到指定级别的 总增加值
        public int get_total_apt_by_level(int level)
        {
            int total = 0;
            if (m_conf["acup_lvl"] != null)
            {
                Variant acup_lvl = m_conf["acup_lvl"];

                if (level > acup_lvl.Count) level = acup_lvl.Count;

                for (int i = 1; i < level; i++)
                {
                    Variant data = acup_lvl[i];
                    total += data["att_per_min"]._int;
                }
            }
            return total;
        }

        public int get_add_apt_by_level(int level)
        {
            Variant bdata = get_acup_lvl(level);
            if (bdata == null)
                return 0;

            return bdata["att_per_min"];
        }
        public int get_acup_level_by_apt(int apt)
        {

            int level = 1;

            while (apt > 0)
            {

                Variant data = get_acup_lvl(level);

                if (data == null)
                    break;

                level++;

                apt -= data["att_per_min"];
            }

            return level;
        }

        public int max_acup_pt()
        {

            if (m_conf["acup_lvl"] == null)
                return 0;

            int apt = 0;

            Variant acup_lvl = m_conf["acup_lvl"];

            for (int i = 1; i < acup_lvl["length"]; i++)
            {
                apt += acup_lvl[i]["att_per_min"];
            }

            return apt;
        }

        /**
         *穴位可以升到最高的等级 
         */
        public int max_acup_lvl()
        {

            if (m_conf["acup_lvl"] == null)
                return 0;

            Variant acup_lvl = m_conf["acup_lvl"];

            Variant ao = acup_lvl[acup_lvl.Count - 1];
            if (ao == null)
                return 0;

            return ao["lvl"];
        }

        /**
         *根据筋脉编号和穴位编号获得 穴位信息(默认的)
         * @merid 筋脉编号
         * @aid 穴位编号 
         */
        public Variant get_acup_by_meri(int merid, int aid)
        {
            Variant merobj = get_meri_data_by_id(merid);

            if (merobj == null)
                return null;

            Variant acup = merobj["acup"];
            Variant acupobj = null;
            foreach (string i in acup._arr)
            {
                acupobj = acup[i];
                if (acupobj["aid"] == aid)
                {
                    return acupobj;
                }
            }

            return acupobj;
        }

        /**
         *根据门派筋脉编号和穴位编号获得 穴位信息
         * @merid 筋脉编号
         * @aid 穴位编号
         * @carr 门派编号。 
         */
        public Variant get_carr_acup_by_meri(int merid, int aid, int carr)
        {
            Variant merobj = this.get_meri_carr_data_by_id(merid, carr);

            if (merobj == null)
                return null;

            Variant acup = merobj["acup"];
            if (aid >= acup.Count)
            {
                return null;
            }
            Variant acupobj = null;
            foreach (string i in acup._arr)
            {
                acupobj = acup[i];
                if (acupobj["aid"] == aid)
                {
                    return acupobj;
                }
            }

            return acupobj;
        }
        //-----------------------穴位相关配置          end-------------------------
    }
}
