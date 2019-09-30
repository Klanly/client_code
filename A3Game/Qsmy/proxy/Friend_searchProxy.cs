using System.Text;
using GameFramework;
using Cross;
using System.Collections.Generic;

namespace MuGame
{
    class Friend_searchProxy:BaseProxy<Friend_searchProxy>
    {
        public static uint HAVE_FRIEND = 0;
        public static uint ON_FRIEND = 1;
        public static uint RECOMMENTFRIEND = 2;
        public static uint ADDFRIEND = 3;
		public static uint LOOKFRIEND = 11; 
        
        public Friend_searchProxy()
        {
            //172
            addProxyListener(PKG_NAME.C2S_ON_LOAD_SRARCH_FRIEND, onLoadSearchFriend);
            //175
            addProxyListener(PKG_NAME.C2S_ON_LOAD_RECOMMENT_FRIEND, onLoadrecommentFriend);
            //170
            addProxyListener(PKG_NAME.C2S_ON_ADDFRIEND_BUTTON, onAddfriendButton);
        }


        public void sendSearchFriend(string name)
        {
            Variant msg = new Variant();
            msg["name"] = name;
            sendRPC(PKG_NAME.C2S_ON_LOAD_SRARCH_FRIEND, msg);
        }
        public void sendrecommentFriend()
        {
            sendRPC(PKG_NAME.C2S_ON_LOAD_RECOMMENT_FRIEND, null);
        }
        public void sendaddfriendButton(int id)
        {
            Variant msg = new Variant();
            msg["cid"] = id;
			sendRPC(PKG_NAME.C2S_ON_ADDFRIEND_BUTTON, msg);
		}
		public void sendgetplayerinfo(uint cid) {
			Variant msg = new Variant();
			msg["buddy_cmd"] = 11;
			msg["cid"] = cid;
			sendRPC(PKG_NAME.C2S_ON_ADDFRIEND_BUTTON, msg);
		}







        public void onLoadSearchFriend(Variant data)
        {
            debug.Log("搜索好友：" + data.dump());
            int res = data["res"];
            if (res == 1)
            {
                dispatchEvent(GameEvent.Create(HAVE_FRIEND, this, data)); 
            }
            else
            {
                dispatchEvent(GameEvent.Create(ON_FRIEND, this, data));
            }
        }
        public void onLoadrecommentFriend(Variant data)
        {
            debug.Log("推荐好友：" + data.dump());
            dispatchEvent(GameEvent.Create(RECOMMENTFRIEND, this, data));

        }
        public void onAddfriendButton(Variant data)
        {
            debug.Log("好友操作：" + data.dump());
			int res = data["res"];
			//查看好友
            if (res == 11)
            {
				dispatchEvent(GameEvent.Create(LOOKFRIEND, this, data));
            }


            
        }
    }
}
