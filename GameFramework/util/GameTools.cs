using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Cross;
namespace GameFramework
{

    public class GameTools
    {
        public const uint CASEINSENSITIVE = 1;
        //降序
        public const uint DESCENDING = 2;
        public const uint UNIQUESORT = 4;
        public const uint RETURNINDEXEDARRAY = 8;
        public const uint NUMERIC = 16;

        private static Random _random;

        public GameTools()
        {
        }
        static private GameTools _t;
        static public GameTools inst
        {
            get
            {
                if (_t == null) _t = new GameTools();
                return _t;
            }
        }
        public double pixelToUnit(double val)
        {
            return val / GameConstantDef.UNIT_TRANS_PIXEL;
        }
        public double unitTopixel(double val)
        {
            return val * GameConstantDef.UNIT_TRANS_PIXEL;
        }
        public static Random randomInst
        {
            get
            {
                if (_random == null)
                    _random = new Random();
                return _random;
            }
        }

        public const uint ClassArray = 0;
        public const uint ClassDictionary = 1;
        static public Variant array2Map(Variant ary, String keyName, uint type = ClassDictionary)
        {
            Variant retObj = new Variant();
            if (ary != null && ary._arr != null)
            {
                //防止ary不是数组会报错
                if (ClassArray == type)
                {
                    foreach (Variant val in ary._arr)
                    {
                        if(val.ContainsKey(keyName))
                        retObj[int.Parse(val[keyName]._str)] = val;
                    }
                    return retObj;
                }
                else if (ClassDictionary == type)
                {
                    foreach (Variant val in ary._arr)
                    {
                        retObj[val[keyName]] = val;
                    }
                    return retObj;
                }
                
            }
            return ary;

        }
        static  public  Variant mergeSimpleObject(Variant src, Variant dest, bool clone = false, bool cuseSrc = true)
        {
            Variant retObj = new Variant();
            Variant conflictSolvObj = src;
            if ( !cuseSrc )
            {
                conflictSolvObj = dest;
            }
            if (clone)
            {
                if (src.isArr)
                {
                    foreach (string key in src._arr)
                    {
                        if (dest.ContainsKey(key))
                        {
                            if ((dest[key].isArr || dest[key].isDct) && (src[key].isArr || src[key].isDct))
                            {
                                retObj[key] = mergeSimpleObject(src[key], dest[key]);
                            }
                            else
                            {
                                retObj[key] = conflictSolvObj[key].clone();
                            }
                        }
                        else
                        {
                            retObj[key] = src[key].clone();
                        }
                    }
                }
                if (src.isDct)
                {
                    foreach (string key in src.Keys)
                    {
                        if (dest.ContainsKey(key))
                        {
                            if ((dest[key].isArr || dest[key].isDct) && (src[key].isArr || src[key].isDct))
                            {
                                retObj[key] = mergeSimpleObject(src[key], dest[key]);
                            }
                            else
                            {
                                retObj[key] = conflictSolvObj[key].clone();
                            }
                        }
                        else
                        {
                            retObj[key] = src[key].clone();
                        }
                    }
                }
                if (dest.Count > 0)
                {
                    foreach (string key in dest.Keys)
                    {
                        if (retObj.ContainsKey(key))
                            continue;
                        retObj[key] = dest[key].clone();
                    }
                }
            }
            else
            {
            //    if (src.isArr)
            //    {
            //        foreach (string key in src._arr)
            //        {
            //            if (dest.ContainsKey(key))
            //            {
            //                if ((dest[key].isArr || dest[key].isDct) && (src[key].isArr || src[key].isDct))
            //                {
            //                    retObj[key] = mergeSimpleObject(src[key], dest[key]);
            //                }
            //                else
            //                {
            //                    retObj[key] = conflictSolvObj[key];
            //                }
            //            }
            //            else
            //            {
            //                retObj[key] = src[key];
            //            }
            //        }
            //    }
                 if (src.isDct)
                {
                    foreach (string key in src.Keys)
                    {
                        if (dest.ContainsKey(key))
                        {
                            if ((dest[key].isArr || dest[key].isDct) && (src[key].isArr || src[key].isDct))
                            {
                                retObj[key] = mergeSimpleObject(src[key], dest[key]);
                            }
                            else
                            {
                                retObj[key] = conflictSolvObj[key];
                            }
                        }
                        else
                        {
                            retObj[key] = src[key];
                        }
                    }
                }
                if (dest.Count > 0)
                {
                    foreach (string key in dest.Keys)
                    {
                        if (retObj.ContainsKey(key))
                            continue;
                        retObj[key] = dest[key];
                    }
                }
                
            }
            return retObj;
        }

        /// <summary>
        /// 将字符串切割成数组。
        /// </summary>
        /// <param name="str">需要被切割的字符串。</param>
        /// <param name="substr">用来切割的字符串。</param>
        /// <param name="type">
        /// 转换数组中值的数据类型，
        /// GameConstant.SPLIT_TYPE_STRING为字符串，
        /// GameConstant.SPLIT_TYPE_INT为int型。
        /// </param>
        /// <returns>Variant</returns>
        static public Variant split(string str, string substr, uint type = GameConstantDef.SPLIT_TYPE_STRING)
        {
            Variant ary = new Variant();
            string[] stringSeparators = new string[] { substr };
            string[] temp = str.Split(stringSeparators, StringSplitOptions.None);
            for (int i = 0; i < temp.Length; i++)
            {
                string t = temp[i];
                switch (type)
                {
                    case GameConstantDef.SPLIT_TYPE_INT:
                        if (t == "")
                            ary._arr.Add(0);
                        else
                            ary._arr.Add(Int32.Parse(t));
                        break;
                    case GameConstantDef.SPLIT_TYPE_STRING:
                        ary._arr.Add(t);
                        break;
                }
            }
            return ary;
        }

		static public void PrintCrash( string msg )
        {			
			DebugTrace.add(Define.DebugTrace.DTT_ERR, " < =========================== Crash  =========================== > \n" + msg );
		}
		static public void PrintError( string msg )
        {
			DebugTrace.add(Define.DebugTrace.DTT_ERR, " < =========================== ERROR  =========================== > \n" +msg );
		}

		static public void PrintDetial( string msg )
        {
			DebugTrace.add(Define.DebugTrace.DTT_DTL, msg );
		}

		static public void PrintSystem( string msg )
        {
			DebugTrace.add(Define.DebugTrace.DTT_SYS, " < =========================== SYSTEM  =========================== > \n" +msg );
		}

		static public void PrintNotice( string msg )
        {
			DebugTrace.add(Define.DebugTrace.DTT_SYS, " < =========================== Notice  =========================== > \n" +msg );
		}

		static public void PrintProfile( string msg )
        {
			DebugTrace.add(Define.DebugTrace.DTT_SYS, " < =========================== Profile  =========================== > \n" +msg );
		}

        static public Variant GetTmchkAbs(string date)
        {
            if ("" == date || null == date)
            {
                return null;
            }
            Variant tba = split(date, " ");
            Variant tby = split(tba[0], "-");
            Variant tbt = split(tba[1], ":");
            Variant ar = new Variant();
            ar["y"] = (int)tby[0];
            ar["mon"] = (int)tby[1];
            ar["d"] = (int)tby[2];
            ar["h"] = (int)tbt[0];
            ar["min"] = (int)tbt[1];
            ar["s"] = (int)tbt[2];
            return ar;
        }
        /// <summary>
        /// 获取当前函数路径str
        /// </summary>
        /// <returns></returns>
        public static string GetMethodInfo(Variant data = null)
        {
            string str = "";
            ////取得当前方法命名空间
            //str += "命名空间名:" + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "\n";
            ////取得当前方法类全名 包括命名空间
            //str += "类名:" + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + "\n";
            ////取得当前方法名
            //str += "方法名:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\n";
            //str += "\n";

            StackTrace ss = new StackTrace(true);
            MethodBase mb = ss.GetFrame(1).GetMethod();
            ////取得父方法命名空间
            //str += mb.DeclaringType.Namespace + "\n";
            ////取得父方法类名
            //str += mb.DeclaringType.Name + "\n";
            //取得父方法类全名
            str += mb.DeclaringType.FullName + ".";
            //取得父方法名
            str += mb.Name + ":";
            if (null != data)
                str += data.dump();
            str += "\n";
            return str;
        }
        static public Variant CreateSwitchData(string caseStr, Variant data)
        {
            //Variant var = new Variant();
            //var["case"] = caseStr;
            //var["data"] = data;
            //return data;
            return createGroup("case", caseStr, "data", data);
        }
        /// <summary>
        /// 实现{npcid:npcid,trid:trid} group(strint str1,int int1,string str2,variant var1 ....) 参数为偶数
        /// </summary>
        /// <param name="vals"></param>
        /// <returns></returns>
        static public Variant createGroup(params Variant[] vals)
        {
            Variant data = new Variant();
            for (int i = 0; i < vals.Length; i += 2)
            {
                if (vals[i] != null && vals[i + 1] != null)
                    data[vals[i]._str] = vals[i + 1];
            }
            return data;
        }
        static public Variant createGroup(params object[] vals)
        {
            //重载
            Variant data = new Variant();
            for (int i = 0; i < vals.Length; i += 2)
            {
                if (vals[i] != null && vals[i + 1] != null)
                {
                    data[(string)vals[i]] = new Variant();//vals[i + 1];
                    data[(string)vals[i]]._val = vals[i + 1];
                }
            }
            return data;
        }
        static public Variant createArray(params Variant[] vals)
        {
            Variant data = new Variant();
            for (int i = 0; i < vals.Length; i++)
            {
                data.pushBack(vals[i]);
            }
            return data;
        }
        static public long getTimer()
        {
            return CCTime.getCurTimestampMS();
        }
        static public void assignProp(Variant obj, Variant prop)
        {
            foreach (string key in prop.Keys)
            {
                if (!(obj.ContainsKey(key)))
                {
                    continue;
                }
                obj[key] = prop[key];
            }
        }
        //格式化文本
        /// <summary>
        /// UIGlobalFunctions.FormatStr  格式化文本
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format"></param>
        /// <param name="clkFun"></param>
        /// <param name="clkPar"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        static public String FormatStr(String str, string format, String clkFun = null, String clkPar = null, string style = "")
        {
            if (str == "") return "";

            String formatStr = "";
            //加样式会有问题，不用了
            string styleStr = style == "" ? "" : (" style=\"" + style + "\"");
            if (clkFun == null && clkPar == null)
            {
                formatStr += "<txt text=\"" + str + "\" format=\"" + format + "\"" + styleStr + "/>";
            }
            else if (clkFun != null && clkPar == null)
            {
                formatStr += "<txt text=\"" + str + "\" format=\"" + format + "\"" + styleStr + " onclick=\"" + clkFun + "\"/>";
            }
            else if (clkFun != null && clkPar != null)
            {
                formatStr += "<txt text=\"" + str + "\" format=\"" + format + "\"" + styleStr + " onclick=\"" + clkFun + "\" clickpar=\"" + clkPar + "\"/>";
            }
            return formatStr;
        }

        //最终文本（添加 <supertext> </supertext>）
        /// <summary>
        /// UIGlobalFunctions.GetFinFormatStr  最终文本（添加 <supertext> </supertext>）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public String GetFinFormatStr(String str)
        {
            String formatStr = "<supertext>";
            formatStr += str;
            formatStr += "</supertext>";
            return formatStr;
        }

        //清空superTxt
        /// <summary>
        /// UIGlobalFunctions.clearSuperTxtStr 清空superTxt
        /// </summary>
        /// <returns></returns>
        static public String clearSuperTxtStr()
        {
            return GetFinFormatStr("");
        }

        /// <summary>
        /// UIGlobalFunctions.GetSuperHtmlStr   获取html文本
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format"></param>
        /// <param name="clkFun"></param>
        /// <param name="clkPar"></param>
        /// <returns></returns>
        static public String GetSuperHtmlStr(string str, string format, string clkFun = null, string clkPar = null)
        {
            string formatStr = FormatStr(str, format, clkFun, clkPar);
            formatStr = GetFinFormatStr(formatStr);
            return formatStr;
        }
        static public int get_day(int sec)
        {
            int day = sec / 86400;
            return day;
        }

		static public T pop<T>( List<T> arr )
        {
			if( arr == null || arr.Count <=0 ) return default(T);
			int endidx = arr.Count-1;
			T val = arr[ endidx ];
			arr.RemoveAt( endidx );
			return val;
		}
		 
		static public Comparison<Variant> sortFun( string key, uint option = GameTools.NUMERIC )
        {
			return (left, right) =>
            {
                int ret = 0;
                int oi = -1;
                if (left.ContainsKey(key) && right.ContainsKey(key))
                {
                    if (left[key].isStr && !int.TryParse(left[key]._str, out oi))
                    {//纯字符串排序，不包含数字字符串
                        ret = left[key]._str.CompareTo(right[key]);
                    }
                    else
                    {
                        if (left[key] > right[key])
                            ret = 1;
                        else if (left[key] == right[key])
                            ret = 0;
                        else
                            ret = -1;
                    }
                    
                }
                else
                {
                    if (!left.ContainsKey(key) && !right.ContainsKey(key))
                        ret = 0;
                    if (!left.ContainsKey(key))
                        ret = -1;
                    if (!right.ContainsKey(key))
                        ret = 1;
                }
                if (option == GameTools.DESCENDING)
                {
                    ret = -ret;
                }

                return ret;

            };
		}
        /// <summary>
        /// 排序,默认升序。降序参数：option = GameTools.DESCENDING
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="option"></param>
        static public void Sort(Variant data, string key, uint option = GameTools.NUMERIC)
        {
            data._arr.Sort( GameTools.sortFun( key, option ) );

        }
        /// <summary>
        /// 可以传多个关键字、多个排序选项
        /// </summary>
        /// <param name="data"></param>
        /// <param name="keys：关键字"></param>
        /// <param name="options：排序选项"></param>
        static public void Sort(Variant data, List<Variant> keys = null, List<Variant> options = null)
        {
            if (keys == null)
                data._arr.Sort();
            else
                data._arr.Sort((left, right) =>
                {
                    return sortConparison(left, right, keys, options);
                });
        }
        /// <summary>
        /// 排序比较返回值，左与右比较，默认升序
        /// 大于0：左大于右
        /// 等于0：左等于右
        /// 小于0：左小于右
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="keys"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        static private int sortConparison(Variant left, Variant right, List<Variant> keys, List<Variant> opts)
        {
            int ret = 0;
            if (keys != null && keys.Count > 0)
            {
                int oi = -1;
                if (left.ContainsKey(keys[0]._str) && right.ContainsKey(keys[0]._str))
                {
                    if (left[keys[0]].isStr && !int.TryParse(left[keys[0]]._str, out oi))
                    {//纯字符串排序，不包含数字字符串
                        ret = left[keys[0]]._str.CompareTo(right[keys[0]]);
                    }
                    else
                    {
                        if (left[keys[0]] > right[keys[0]])
                            ret = 1;
                        else if (left[keys[0]] == right[keys[0]])
                            ret = 0;
                        else
                            ret = -1;
                    }
                }
                else
                {
                    if (!left.ContainsKey(keys[0]._str) && !right.ContainsKey(keys[0]._str))
                        ret = 0;
                    if (!left.ContainsKey(keys[0]._str))
                        ret = -1;
                    if (!right.ContainsKey(keys[0]._str))
                        ret = 1;
                }

                if (ret == 0)
                {//相等时，比较下一个关键字
                    List<Variant> opt = null;
                    if (opts != null && opts.Count > 1)
                    {
                        opt = opts.GetRange(1, opts.Count - 1);
                    }
                    ret = sortConparison(left, right, keys.GetRange(1, keys.Count - 1), opt);
                }
            }

            if (opts != null && opts.Count > 0)
            {
                switch (opts[0]._uint)
                {
                    case GameTools.DESCENDING:
                        ret = -ret;
                        break;
                }
            }

            return ret;

        }
        /**
		 * @ param symbol 是否以‘：’代替 分秒
		 */
        static public string get_time_ydms(uint sec, bool symbol = false)//symbol参数为空时候  默认symbol=false
        {
            uint day = sec / 86400;
            uint hour = (sec - day * 86400) / 3600;
            uint min = (sec - day * 86400 - hour * 3600) / 60;
            uint second = sec - day * 86400 - hour * 3600 - min * 60;

            string str = "";
            string sday = ":";
            string shour = ":";
            string smin = ":";
            string ssec = "";

            if (!symbol)
            {
                sday = LanguagePack.getLanguageText("time", "day");
                shour = LanguagePack.getLanguageText("time", "hour");
                smin = LanguagePack.getLanguageText("time", "min");
                ssec = LanguagePack.getLanguageText("time", "sec");
            }

            if (day > 0)
                str += day.ToString() + sday;
            if (hour > 0)
                str += hour.ToString() + shour;
            if (min > 0)
                str += min.ToString() + smin;
            if (second > 0)
                str += second.ToString() + ssec;
            return str;
        }
        static public Variant getByKeyVal(List<Variant> list, string key, uint val)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Variant tmp = list[i];
                if (tmp[key] == val)
                {
                    return tmp;
                }
            }
            return null;
        }
        static public Variant getByKeyVal(List<Variant> list, string key, int val)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Variant tmp = list[i];
                if (tmp[key] == val)
                {
                    return tmp;
                }
            }
            return null;
        }
        static public Variant getByKeyVal(List<Variant> list, string key, string val)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Variant tmp = list[i];
                if (tmp[key] == val)
                {
                    return tmp;
                }
            }
            return null;
        }
        /// <summary>
        /// 删除数组第一个元素并返回该元素
        /// </summary>
        static public Variant shift(ref Variant arr)
        {
            Variant res = arr[0];
            arr._arr.RemoveAt(0);
            return res;
        }
        /// <summary>
        /// 将元素插入到数组首位
        /// </summary>
        static public void unshift(ref Variant arr, Variant obj)
        {
            Variant res = new Variant();
            res._arr.Add(obj);
            res._arr.AddRange(arr._arr);
            arr = res;
        }

		static public int random( int a, int b )
		{
            return a + GameTools.randomInst.Next(0, (b - a));
		}
		static public int random( int a )
		{
            return GameTools.randomInst.Next(0, a);
		}
		


    }
}