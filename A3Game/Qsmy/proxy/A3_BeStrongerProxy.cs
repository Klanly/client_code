using GameFramework;

namespace MuGame
{
    class A3_BeStrongerProxy:BaseProxy<A3_BeStrongerProxy>
    {
        /// <summary>
        /// 变强目前没有自己的协议,只依赖于其它各模块的协议所触发的事件
        /// </summary>
        public A3_BeStrongerProxy()
        {            
            addEventListener(PKG_NAME.S2C_GET_ITEMS_RES, CheckItem);
            addEventListener(PKG_NAME.S2C_SELL_ITEM_RES, CheckItem);
            addEventListener(PKG_NAME.S2C_USE_UITEM_RES, CheckItem);
            addEventListener(PKG_NAME.S2C_ITEM_CHANGE, CheckItem);
            addEventListener(PKG_NAME.S2C_BAGITEM_CDTIME, CheckItem);
            addEventListener(PKG_NAME.S2C_LVL_UP, CheckItem);
            addEventListener(PKG_NAME.C2S_A3_SKILL, CheckItem);
        }

        void CheckItem(GameEvent e = null) => A3_BeStronger.Instance?.CheckUpItem();        
    }
}
