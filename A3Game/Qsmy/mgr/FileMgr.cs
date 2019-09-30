using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;
namespace MuGame
{
    class FileMgr
    {
        public static string TYPE_MAIL = "mail";
        public static string TYPE_CHAT = "chat";
        public static string TYPE_NEWBIE = "newbie";
        public static string TYPE_DRESS = "dress";
        public static string TYPE_AUTO = "auto";

        /** 后缀常量字符 */
        public const string SUFFIX = ".txt";
        const string PREFIX = "file://";
        const string FORMAT = ".unity3d";
        public static string RESROOT = Application.persistentDataPath + "/";


        public static void saveString(string type,string fileName,string cacheStr)
        {
            string path = RESROOT+PlayerModel.getInstance().cid + "_" + type + "_" + fileName;
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Flush();
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            sw.Write(cacheStr);
            sw.Close();
        }

        public static string loadString(string type, string fileName)
        {
             
            try
            {
                string path = RESROOT + PlayerModel.getInstance().cid + "_" + type + "_" + fileName;
                if (!File.Exists(path)) 
                    return "";
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
                fs.Close();
                List<string> mCacheFriend = new List<string>(); 
               return Encoding.UTF8.GetString(bytes);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
            return "";
        }

        public static void removeFile(string type, string fileName)
        {
            try
            {
                string path = RESROOT + PlayerModel.getInstance().cid + "_" + type + "_" + fileName;
                if (!File.Exists(path)) return ;
                File.Delete(path);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }


        public static string GetStreamingAssetsPath(string p_filename)
        {
            string _strPath = "";
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                _strPath = "file://" + Application.streamingAssetsPath + "/" + p_filename + ".unity3d";
            else if (Application.platform == RuntimePlatform.Android)
                _strPath = Application.streamingAssetsPath + "/" + p_filename + ".unity3d";
            else if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.IPhonePlayer)
                _strPath = "file://" + Application.streamingAssetsPath + "/" + p_filename + ".unity3d";

            return _strPath;
        }



        public static string GetOSDataPath(string p_filename)
        {
            string path;
            path = "";

            if (Application.platform == RuntimePlatform.OSXEditor)
                path = Application.persistentDataPath + p_filename;

            if (Application.platform == RuntimePlatform.IPhonePlayer)
                path = RESROOT + p_filename;


            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
                path = Application.dataPath + "/cache/" + p_filename;


            if (Application.platform == RuntimePlatform.Android)
                path = RESROOT + p_filename;

            //    Debug.LogWarning("===========path:"+path);
            return path;
        }

        public static string GetURLPath(string p_filename, bool needPreFix, bool needFormat)
        {
            string path;
            path = "";

            if (Application.platform == RuntimePlatform.OSXEditor)
                path = Application.persistentDataPath + "/" + p_filename;

            if (Application.platform == RuntimePlatform.IPhonePlayer)
                path = RESROOT + p_filename;


            if (Application.platform == RuntimePlatform.WindowsEditor)
                path = Application.dataPath + "/cache/" + p_filename;

            if (Application.platform == RuntimePlatform.WindowsPlayer)
                path = Application.dataPath + "/cache/" + p_filename;

            if (Application.platform == RuntimePlatform.Android)
                path = RESROOT + p_filename;

            if (needPreFix) path = PREFIX + path;
            if (needFormat) path = path + FORMAT;
            //Debug.LogWarning("===========path:"+path);
            return path;
        }

        public static string getFileName(string path)
        {

            string[] _list = path.Split(new char[] { '/' });



            if (_list.Length > 0) return _list[_list.Length - 1];
            else
                return "";

        }

        public static string getFileDir(string path)
        {
            path = path.Replace("\\", "/");
            path = path.Substring(0, path.LastIndexOf("/"));
            return path;
        }

        public static void CreateDirIfNotExists(string path)
        {
            string dir = getFileDir(path);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
        }
    }
}
