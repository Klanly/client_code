using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;

namespace MuGame
{
    public class ConfigUtil
    {
        private static Random _random;
        public ConfigUtil()
        {

        }

        static public Type getType(string name)
        {
            return System.Type.GetType(name);

        }


        public static void SetTimeout(double interval, Action action)
        {
            System.Timers.Timer timer = new System.Timers.Timer(interval);
            timer.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
           {
               timer.Enabled = false;
               action();
           };
            timer.Enabled = true;
        }



        static public bool attchk(Variant attchk, Variant atts)
        {
            if (attchk == null)
                return true;
            if (atts == null)
                return true;
            for (int i = 0; i < attchk.Count; ++i)
            {
                Variant att = attchk[i];
                if (att["name"]._str == "carr")
                {
                    int loop = 8;
                    //每4位为一个职业
                    bool carrcheck = false;
                    int carr = att["and"];
                    for (int j = 0; j < loop; ++j)
                    {
                        if (((carr >> (j * 4)) & 0x01) != 0)
                        {
                            int lvl = (carr >> (j * 4 + 1)) & 0x07;
                            int car = j + 1;
                            if (atts["carr"]._int == car && atts.ContainsKey("carrlvl") && atts["carrlvl"]._int >= lvl)
                            {
                                carrcheck = true;
                                break;
                            }
                        }
                    }
                    if (!carrcheck)
                        return false;
                }
                else
                {
                    if (att.ContainsKey("equal"))
                    {
                        if (atts.ContainsKey(att["name"]._str))
                        {
                            if (atts[att["name"]]._int != att["equal"]._int)
                                return false;
                        }
                    }
                    else
                    {
                        if (att.ContainsKey("min"))
                        {
                            if (atts.ContainsKey(att["name"]._str))
                            {
                                if (atts[att["name"]]._int < att["min"]._int)
                                    return false;
                            }
                        }
                        if (att.ContainsKey("max"))
                        {
                            if (atts.ContainsKey(att["name"]._str))
                            {
                                if (atts[att["name"]]._int > att["max"]._int)
                                    return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public static Variant get_att(Variant attchk, Variant atts)
        {
            for (int i = 0; i < attchk.Count; ++i)
            {
                Variant att = attchk[i]["attchk"][0];
                if (att.ContainsKey("equal"))
                {
                    if (atts.ContainsKey("name") && atts["name"] == att["name"])
                    {
                        if (atts["equal"]._int == att["equal"]._int)
                            return attchk[i];
                    }
                }
                else
                {
                    if (atts.ContainsKey("min") && atts["name"] == att["name"])
                    {
                        if (atts.ContainsKey(att["name"]._str))
                        {
                            if (atts["min"]._int > att["min"]._int)
                                return attchk[i];
                        }
                    }
                    if (att.ContainsKey("max"))
                    {
                        if (atts.ContainsKey("name") && atts["name"] == att["name"])
                        {
                            if (atts["max"]._int < att["max"]._int)
                                return attchk[i];
                        }
                    }
                }
            }

            return null;
        }

        /**
         * 是否在时效内 
         * @param current_tm
         * @param tmchk
         * @return 
         * 
         */
        static public bool check_tm(double tm_now, Variant tmchk, double firstopentm = 0, double cbtm = 0)
        {
            if (tmchk == null)
            {
                return true;
            }
            if (tmchk.isArr)
            {
                foreach (Variant chk in tmchk._arr)
                {
                    if (check_tm_impl(tm_now, chk, firstopentm, cbtm)) return true;
                }
                return false;
            }
            else
            {
                return check_tm_impl(tm_now, tmchk, firstopentm, cbtm);
            }
        }
        static private bool check_tm_impl(double tm_now, Variant tmchk, double firstopentm = 0, double cbtm = 0)
        {


            if (cbtm > 0)
            {
                if (tmchk.ContainsKey("cb_optm"))
                {
                    if ((tm_now / 1000 - cbtm) < tmchk["cb_optm"]._int * 86400)
                    {
                        return false;
                    }
                }
                if (tmchk.ContainsKey("cb_cltm"))
                {
                    if ((tm_now / 1000 - cbtm) > (tmchk["cb_cltm"]._int + 1) * 86400)
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (tmchk.ContainsKey("cb_optm") || tmchk.ContainsKey("cb_cltm"))
                {
                    return false;
                }
            }
            if (firstopentm > 0)
            {
                if (tmchk.ContainsKey("optm"))
                {
                    if ((tm_now / 1000 - firstopentm) < tmchk["optm"]._int * 86400)
                    {
                        return false;
                    }
                }
                if (tmchk.ContainsKey("cltm"))
                {
                    if ((tm_now / 1000 - firstopentm) > (tmchk["cltm"]._int + 1) * 86400)
                    {
                        return false;
                    }
                }
            }

            double _start_tm = 0;
            double _end_tm = 0;
            if (tmchk.ContainsKey("tb"))
            {
                //绝对开始时间
                Variant tb = tmchk["tb"];
                TZDate tbdate = TZDate.createByYMDHMS(tb["y"], tb["mon"]._int - 1, tb["d"], tb["h"], tb["min"], tb["s"]);
                _start_tm = tbdate.time;
                if (tm_now < _start_tm)
                {
                    return false;
                }
            }
            if (tmchk.ContainsKey("te"))
            {
                Variant te = tmchk["te"];
                TZDate tedate = TZDate.createByYMDHMS(te["y"], te["mon"] - 1, te["d"], te["h"], te["min"], te["s"]);
                _end_tm = tedate.time;
                if (tm_now > _end_tm)
                {
                    return false;
                }
            }


            if (tmchk.ContainsKey("dtb") && tmchk.ContainsKey("dte"))
            {
                //一天中的开始时间
                TZDate dtbd;
                TZDate dted;

                TZDate data_now = new TZDate(tm_now);
                int day = data_now.getDay();

                if (day == 0)
                    day = 7;

                if (tmchk.ContainsKey("wtb") && tmchk.ContainsKey("wte"))
                {
                    //一周中的开始时间
                    int wtb = tmchk["wtb"];
                    int wte = tmchk["wte"] - 1;

                    if (day > wte || day < wtb)
                        return false;

                }
                else if (tmchk.ContainsKey("wd"))
                {
                    string wd = tmchk["wd"];
                    Variant wda = GameTools.split(wd, ",");

                    bool inday = false;
                    for (int i = 0; i < wda.Count; ++i)
                    {
                        int dday = wda[i];
                        if (dday == day)
                        {
                            inday = true;
                            break;
                        }
                    }
                    if (!inday)
                        return false;
                }

                dtbd = new TZDate(tm_now);
                dted = new TZDate(tm_now);

                Variant dtb = tmchk["dtb"];
                Variant dte = tmchk["dte"];

                dtbd.setHours(dtb["h"], dtb["min"], dtb["s"], 0);

                dted.setHours(dte["h"], dte["min"], dte["s"], 0);

                _start_tm = dtbd.time;
                _end_tm = dted.time;

                if (tm_now < _start_tm || tm_now > _end_tm)
                    return false;
            }
            return true;
        }

        /**
         * 下次有效期开启的时刻 ，如果已在时效内，返回0
         * @param current_tm
         * @param tmchk
         * @return 
         * 
         */
        static public double next_start_tm(double tm_now, Variant tmchk, double firstopentm)
        {
            if (check_tm(tm_now, tmchk, firstopentm))
                return 0;

            if (tmchk.isArr)
            {
                double temp = 0;
                double tm = 0;
                foreach (Variant chk in tmchk._arr)
                {
                    temp = next_start(tm_now, chk, firstopentm);

                    if (temp > 0)
                    {
                        if (0 == tm)
                        {
                            tm = temp;
                        }
                        else if (tm > temp)
                        {
                            tm = temp;
                        }
                    }
                }
                return tm;
            }
            else
            {
                return next_start(tm_now, tmchk, firstopentm);
            }
        }
        static private double next_start(double tm_now, Variant tmchk, double firstopentm = 0)
        {
            bool calc_notopen = false;
            if (firstopentm > 0 && tmchk.ContainsKey("optm"))
            {
                if ((tm_now / 1000 - firstopentm) < tmchk["optm"]._int * 86400)
                {
                    tm_now = (tmchk["optm"]._int) * 86400 * 1000 + (firstopentm * 1000);
                    calc_notopen = true;
                }
            }

            double _start_tm = 0;
            double _end_tm = 0;
            string wd = "";
            Variant wda = null;
            if (tmchk.ContainsKey("tb") && tmchk.ContainsKey("te"))
            {
                //绝对开始时间
                Variant tb = tmchk["tb"];
                Variant te = tmchk["te"];

                TZDate tbdate = TZDate.createByYMDHMS(tb["y"], tb["mon"]._int - 1, tb["d"], tb["h"], tb["min"], tb["s"]);
                _start_tm = tbdate.time;

                TZDate tedate = TZDate.createByYMDHMS(te["y"], te["mon"] - 1, te["d"], te["h"], te["min"], te["s"]);
                _end_tm = tedate.time;

                //绝对开始时间如果已过开始时间，那么不会再次开启了
                if (_start_tm < tm_now)
                    return 0;
                else
                    return _start_tm;
            }
            else if (tmchk.ContainsKey("dtb") && tmchk.ContainsKey("dte"))
            {
                bool haswchk = false;

                TZDate data_now = new TZDate(tm_now);
                int day = data_now.getDay();

                if (day == 0)
                    day = 7;

                Variant dtb = tmchk["dtb"];
                Variant dte = tmchk["dte"];

                //一天中的开始时间
                TZDate dtbd;
                TZDate dted;

                //一周中的开始时间
                int wtb = 0;
                int wte = 0;
                if (tmchk.ContainsKey("wtb") && tmchk.ContainsKey("wte"))
                {
                    wtb = tmchk["wtb"];
                    wte = tmchk["wte"] - 1;
                    haswchk = true;
                }
                else if (tmchk.ContainsKey("wd"))
                {
                    wd = tmchk["wd"];
                    wda = GameTools.split(wd, ",");
                }

                dtbd = new TZDate(tm_now);
                dted = new TZDate(tm_now);

                dtbd.setHours(dtb["h"], dtb["min"], dtb["s"], 0);
                dted.setHours(dte["h"], dte["min"], dte["s"], 0);

                _start_tm = dtbd.time;
                _end_tm = dted.time;

                //今天是否未开启，如果有周限制，且今天不在限制内，那么标为非未开启.否则查看今天的时间是否已开启
                bool notopen = true;
                if (haswchk && (day > wte || day < wtb))
                {
                    notopen = true;
                }
                else if (tmchk.ContainsKey("wd"))
                {
                    wd = tmchk["wd"];
                    wda = GameTools.split(wd, ",");

                    notopen = true;
                    for (int j = 0; j < wda.Count; j++)
                    {
                        int wds = wda[j];
                        if (wds == day)
                        {
                            if (tm_now > _start_tm && tm_now < _end_tm)
                                notopen = false;
                            break;
                        }
                    }
                }
                else
                {
                    if (tm_now < _start_tm)
                    {
                        notopen = true;
                    }
                    else
                        notopen = false;
                }

                int dc;
                if (notopen || calc_notopen)
                {
                    if (tmchk.ContainsKey("wd"))
                    {
                        //返回下一天的开启时间
                        wd = tmchk["wd"];
                        wda = GameTools.split(wd, ",");

                        int next_day = -1;
                        int min_day = -1;
                        for (int i = 0; i < wda.Count; ++i)
                        {
                            int wdp = wda[i];

                            if (min_day < 0 || wdp < min_day)
                                min_day = wdp;

                            if (day > wdp || (day == wdp) && (tm_now > _end_tm))
                                continue;

                            if (next_day < 0 || wdp < next_day)
                                next_day = wdp;
                        }

                        if (next_day < 0)
                        {
                            //本周已过，取下周第一天
                            return _start_tm + 86400000 * (7 - day + min_day);
                        }
                        else
                        {
                            return _start_tm + 86400000 * (next_day - day);
                        }
                    }
                    else if (!haswchk)
                    {
                        //返回今天的开启时间
                        return _start_tm;
                    }
                    else
                    {
                        //如果有周限制，则要判断下一个周限制的自然日

                        if (day < wtb || day > wte)
                        {
                            dc = wtb - day;
                            if (dc < 0)
                                dc += 7;

                            return _start_tm + 86400000 * dc;
                        }
                        else
                        {
                            return _start_tm /* + 86400000 */;
                        }
                    }
                }
                else
                {
                    //返回下一天的开启时间
                    if (tmchk.ContainsKey("wd"))
                    {
                        //
                        return 0;
                    }
                    else if (!haswchk)
                    {
                        //如果没有周限制，则为下一个自然日
                        return _start_tm + 86400000;

                    }
                    else
                    {
                        //如果有周限制，则要判断下一个周限制的自然日
                        ++day;
                        if (day > 6)
                            day = 0;

                        if (day < wtb || day > wte)
                        {
                            dc = wtb - day;
                            if (dc < 0)
                                dc += 7;

                            return _start_tm + 86400000 * dc;
                        }
                        else
                        {
                            return _start_tm + 86400000;
                        }
                    }
                }
            }
            else
                return 0;
        }
        /**
         * 时效结束的时间，如果不再时效内，返回0 
         * @param current_tm
         * @param tmchk
         * @return 
         * 
         */
        static public double next_end_tm(double tm_now, Variant tmchk, double firstracttmt = 0, double combracttm = 0)
        {
            if (!check_tm(tm_now, tmchk, firstracttmt, combracttm))
                return 0;

            double _end_tm = 0;

            if (tmchk.ContainsKey("tb") && tmchk.ContainsKey("te"))
            {
                Variant te = tmchk["te"];

                TZDate tedate = TZDate.createByYMDHMS(te["y"], te["mon"] - 1, te["d"], te["h"], te["min"], te["s"]);
                _end_tm = tedate.time;
            }
            else if (tmchk.ContainsKey("dtb") && tmchk.ContainsKey("dte"))
            {
                Variant dte = tmchk["dte"];
                TZDate dted = TZDate.createByYMDHMS((int)tm_now);
                dted.setHours(dte["h"], dte["min"], dte["s"], 0);
                _end_tm = dted.time;
            }
            return _end_tm;
        }


        static public Variant GetTodayActiveTime(double tm_now, Variant tmchk, double firstracttmt, double combracttm)
        {
            Variant ret = new Variant();
            ret["begin"] = 0;
            ret["end"] = 0;
            TZDate tmpDate;
            bool todayHasAct = true;//今天是否有活动
            if (tmchk.ContainsKey("tb") && tmchk.ContainsKey("te"))
            {
                Variant tb = tmchk["tb"];
                Variant te = tmchk["te"];

                tmpDate = TZDate.createByYMDHMS(tb["y"], tb["mon"] - 1, tb["d"], tb["h"], tb["min"], tb["s"]);


                ret["begin"] = tmpDate.time;
                //				var today:TZDate = new TZDate(tm_now);
                tmpDate.setHours(0, 0, 0, 0);
                if (tm_now < tmpDate.time)
                {
                    todayHasAct = false;
                    ret["begin"] = 0;
                }

                tmpDate = TZDate.createByYMDHMS(te["y"], te["mon"] - 1, te["d"], te["h"], te["min"], te["s"]);
                ret["end"] = tmpDate.time;
                tmpDate.setHours(23, 59, 59, 0);
                if (tm_now >= tmpDate.time)
                {
                    todayHasAct = false;
                    ret["begin"] = 0;
                }
                return ret;
            }
            else if (tmchk.ContainsKey("dtb") && tmchk.ContainsKey("dte"))
            {
                tmpDate = new TZDate(tm_now);
                int today = tmpDate.getDay();
                if (today == 0) today = 7;//星期天 为 7


                if (tmchk.ContainsKey("wtb") && tmchk.ContainsKey("wte"))
                {
                    todayHasAct = today >= tmchk["wtb"]._int && today <= tmchk["wte"]._int - 1;
                }
                else if (tmchk.ContainsKey("wd"))
                {
                    todayHasAct = false;
                    Variant wds = GameTools.split(tmchk["wd"]._str, ",");
                    for (int i = 0; i < wds.Count; ++i)
                    {
                        if (wds[i]._int == today)
                        {
                            todayHasAct = true;
                            break;
                        }
                    }
                }

                if (todayHasAct)
                {
                    if (tmchk.ContainsKey("optm"))
                    {
                        if ((tm_now / 1000 - firstracttmt) < tmchk["optm"]._int * 86400)
                        {
                            todayHasAct = false;
                        }
                    }
                    if (tmchk.ContainsKey("cltm"))
                    {
                        if ((tm_now / 1000 - firstracttmt) > (tmchk["cltm"]._int + 1) * 86400)
                        {
                            todayHasAct = false;
                        }
                    }
                    if (tmchk.ContainsKey("cb_optm"))
                    {
                        if ((tm_now / 1000 - combracttm) < tmchk["cb_optm"]._int * 86400)
                        {
                            todayHasAct = false;
                        }
                    }
                    if (tmchk.ContainsKey("cb_cltm"))
                    {
                        if ((tm_now / 1000 - combracttm) > (tmchk["cb_cltm"]._int + 1) * 86400)
                        {
                            todayHasAct = false;
                        }
                    }
                }

                if (todayHasAct)
                {
                    Variant dtb = tmchk["dtb"];
                    Variant dte = tmchk["dte"];

                    tmpDate.setHours(dtb["h"], dtb["min"], dtb["s"], 0);
                    ret["begin"] = tmpDate.time;

                    tmpDate.setHours(dte["h"], dte["min"], dte["s"], 0);
                    ret["end"] = tmpDate.time;
                }
            }
            else if ((tmchk.ContainsKey("optm") && tmchk.ContainsKey("cltm")) ||
                (tmchk.ContainsKey("cb_optm") && tmchk.ContainsKey("cb_cltm")))
            {
                if (tmchk.ContainsKey("optm"))
                {
                    if ((tm_now / 1000 - firstracttmt) >= tmchk["optm"]._int * 86400 && ((tm_now / 1000 - firstracttmt) < (tmchk["cltm"]._int + 1) * 86400))
                    {
                        ret["begin"] = firstracttmt + tmchk["optm"]._int * 86400;
                        ret["end"] = firstracttmt + (tmchk["cltm"]._int + 1) * 86400;
                    }
                }
                else if (tmchk.ContainsKey("cb_optm"))
                {
                    if ((tm_now / 1000 - combracttm) >= tmchk["cb_optm"]._int * 86400 && (tm_now / 1000 - combracttm) < (tmchk["cb_cltm"]._int + 1) * 86400)
                    {
                        ret["begin"] = firstracttmt + tmchk["cb_optm"]._int * 86400;
                        ret["end"] = firstracttmt + (tmchk["cb_cltm"]._int + 1) * 86400;
                    }

                }
            }

            return ret;
        }
        /**
         * 通过方向获取起始弧度
         * */
        static public double GetRadianByOctori(int ori)
        {
            //0-w,1-sw,2-s,3-se,4-e,5-ne,6-n,7-nw
            double oriRadian = 0.25 * Math.PI;//45°
            switch (ori)//范围120
            {
                case 0: oriRadian = oriRadian * 4; break;//180°
                case 1: oriRadian = oriRadian * 5; break;//225°
                case 2: oriRadian = oriRadian * 6; break;//270°
                case 3: oriRadian = oriRadian * 7; break;//315°
                case 4: oriRadian = 0; break;//0°
                case 5: oriRadian = oriRadian * 1; break;//45°
                case 6: oriRadian = oriRadian * 2; break;//90°
                case 7: oriRadian = oriRadian * 3; break;//135°
            }
            oriRadian = oriRadian - 90 / 180 * Math.PI;
            return oriRadian;
        }

        static public Variant GetTmchkAbs(string date)
        {
            if ("" == date || null == date)
            {
                return null;
            }
            Variant tba = GameTools.split(date, " ");
            Variant tby = GameTools.split(tba[0], "-");
            Variant tbt = GameTools.split(tba[1], ":");

            Variant ret = new Variant();
            ret["y"] = (int)(tby[0]);
            ret["mon"] = (int)(tby[1]);
            ret["d"] = (int)(tby[2]);
            ret["h"] = (int)(tbt[0]);
            ret["min"] = (int)(tbt[1]);
            ret["s"] = (int)(tbt[2]);

            return ret;
        }
        /**
         * 随机数
         * */
        static public int getRandom(int sInt, int eInt)
        {
            if (sInt > eInt || eInt < 0)
            {
                return -1;
            }

            return (int)(random.NextDouble() * (eInt - sInt + 1)) + sInt;
        }
        /// <summary>
        /// 伪随机生成器实例
        /// </summary>
        static public Random random
        {
            get
            {
                if (_random == null)
                    _random = new Random();
                return _random;
            }

        }

        static public string get_time(int sec)
        {
            int hour = sec / 3600;
            int min = (sec - hour * 3600) / 60;
            int second = sec - hour * 3600 - min * 60;

            string str = "";
            if (hour < 10)
            {
                str += "0";
            }
            str += hour.ToString();

            str += ":";

            if (min < 10)
                str += "0";
            str += min.ToString();

            str += ":";

            if (second < 10)
                str += "0";
            str += second.ToString();

            return str;
        }

        /**
         * 检测角色创建时间是否在时效内
         */
        static public bool check_crttm(float tm_now, Variant crttmchk, Variant self)
        {
            if (crttmchk == null)
            {
                return true;
            }
            if (crttmchk.isArr)
            {
                foreach (Variant chk in crttmchk._arr)
                {
                    if (check_crttm_impl(tm_now, chk, self))
                        return true;
                }
                return false;
            }
            else
            {
                return check_crttm_impl(tm_now, crttmchk, self);
            }
        }

        static public bool check_crttm_impl(float tm_now, Variant crttmchk, Variant self)
        {
            if (crttmchk.ContainsKey("optm"))
            {
                if (tm_now / 1000 - self["crttm"]._int < crttmchk["optm"]._int * 86400)
                {
                    return false;
                }
            }

            if (crttmchk.ContainsKey("cltm"))
            {
                if (tm_now / 1000 - self["crttm"]._int > (crttmchk["cltm"]._int + 1) * 86400)
                {
                    return false;
                }
            }

            return true;
        }
        /**
         * 根据tmchk返回开始/结束的时间戳 
         */
        static public Variant get_tmchk_time(double tm_now, Variant tmchk, double firstopentm = 0, double cbtm = 0)
        {
            double s_tm = 0;
            double e_tm = 0;
            if (firstopentm > 0)
            {
                if (tmchk.ContainsKey("optm"))
                {
                    s_tm = (firstopentm + tmchk["optm"]._int * 86400) * 1000;
                }
                if (tmchk.ContainsKey("cltm"))
                {
                    e_tm = (firstopentm + (tmchk["cltm"]._int + 1) * 86400) * 1000;
                }
            }
            else if (cbtm > 0)
            {
                if (tmchk.ContainsKey("cb_optm"))
                {
                    s_tm = (cbtm + tmchk["cb_optm"]._int * 86400) * 1000;
                }
                if (tmchk.ContainsKey("cb_cltm"))
                {
                    e_tm = (cbtm + (tmchk["cb_cltm"]._int + 1) * 86400) * 1000;
                }
            }
            if (tmchk.ContainsKey("tb"))
            {
                //绝对开始时间
                Variant tb = tmchk["tb"];
                TZDate tbdate = TZDate.createByYMDHMS(tb["y"], tb["mon"]._int - 1, tb["d"], tb["h"], tb["min"], tb["s"]);
                s_tm = tbdate.time;
            }
            if (tmchk.ContainsKey("te"))
            {
                Variant te = tmchk["te"];
                TZDate tedate = TZDate.createByYMDHMS(te["y"], te["mon"]._int - 1, te["d"], te["h"], te["min"], te["s"]);
                e_tm = tedate.time;
            }


            if (tmchk.ContainsKey("dtb") && tmchk.ContainsKey("dte"))
            {
                //一天中的开始时间
                TZDate dtbd;
                TZDate dted;

                dtbd = new TZDate(tm_now);
                dted = new TZDate(tm_now);

                Variant dtb = tmchk["dtb"];
                Variant dte = tmchk["dte"];

                dtbd.setHours(dtb["h"], dtb["min"], dtb["s"], 0);

                dted.setHours(dte["h"], dte["min"], dte["s"], 0);

                s_tm = dtbd.time;
                e_tm = dted.time;

            }
            Variant ret = new Variant();
            ret["stm"] = s_tm;
            ret["etm"] = e_tm;
            return ret;
        }

        /**
         * 检测充值、消费抽奖时间是否有效
         * @param usetp : =2是充值，=3是消费
         */
        static public bool check_lott_tm(float now_tm, int usetp, Variant lottm = null, Variant costLottm = null)
        {
            if (usetp == 2 && lottm != null)
            {
                if (now_tm > lottm["sttm"]._int * 1000 && now_tm < lottm["edtm"]._int * 1000)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (usetp == 3 && costLottm != null)
            {
                if (now_tm > costLottm["sttm"]._int * 1000 && now_tm < costLottm["edtm"]._int * 1000)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}