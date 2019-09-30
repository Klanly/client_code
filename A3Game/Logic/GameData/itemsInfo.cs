using System;
using System.Collections.Generic;
using GameFramework;  using Cross; 

namespace MuGame
{
	public class itemsInfo:LGDataBase
	{
		private const uint SOLD_TP_PACKAGE		= 0;
		private const uint SOLD_TP_SELLED		= 1;
		private const uint SOLD_TP_STORAGE		= 2;
		private const uint SOLD_TP_STORAGE_TEMP = 3;
		public itemsInfo( muNetCleint m )
			: base(m)
		{
			
		}

		public static IObjectPlugin create( IClientBase m )
        {           
            return new itemsInfo( m as muNetCleint );
        }

		/*
			    
		 *		□ maxi:可选，背包空间上限, 
				□ ci:[], 
				□ nci:[],
				□ eqp:[]
		 */
		private Variant _itemInfo;
		private Variant _packageItems = new Variant();
		private Variant _packageItemsMap = new Variant();
		private bool _packageItemsRefreshFlag = true;
		override public void init()
		{//
		//	m_mgr.addEventListener(GAME_EVENT.S2C_GET_ITEMS_RES, onItemRes );
		//	m_mgr.addEventListener(GAME_EVENT.S2C_ITEM_CHANGE, onItemChange);
		}
		private Variant itemInfo
		{
			get {
				if (_itemInfo == null)
				{
					_itemInfo = new Variant();
					_itemInfo = GameConstant.INFO_LOADING;
					Variant msg = new Variant();					
					msg["sold"] = 0;
					//sendRpc(PKG_CMD.C2S_GET_ITEMS, msg);
					return null;
				}
				else if ( _itemInfo.isInteger &&  _itemInfo == GameConstant.INFO_LOADING )
				{					 
					return null;
				}
				else 
				{
					return _itemInfo;
				}
			}
		}


		public Variant packageItems
		{
			get {
				if (this.itemInfo == null) return null;
				refreshPackageItems();
				return _packageItems;
			}
		
		}
		//是否开启随身仓库
		public bool isPrepoAct
		{
			get {
				if (this.itemInfo == null) return false;
				return this.itemInfo["prepo"]._bool;
			}
		}

		//背包空间上限
		public uint packageMaxGrid
		{
			get
			{
				if (this.itemInfo == null) return 0;
				return this.itemInfo["info"]["maxi"]._uint;
			}
		}


		private void refreshPackageItems()
		{
			if ( !_packageItemsRefreshFlag ) return;
						 
			_packageItemsRefreshFlag = false;
			_packageItems._arr.Clear();
			_packageItems._arr.AddRange(this.itemInfo["info"]["ci"]._arr);
			_packageItems._arr.AddRange(this.itemInfo["info"]["nci"]._arr);
			_packageItems._arr.AddRange(this.itemInfo["info"]["eqp"]._arr);
			
			foreach( Variant itm in _packageItems._arr )
			{
				_packageItemsMap[itm["id"]._uint] = itm;
			}
		}

		/*
		 
		○ sold: 
			§ =1表示获取已出售道具，
			§ =0表示获取背包道具，
			§ =2表示仓库道具，
			§ =3表示临时仓库道具,
		○ prepo:是否开启随身仓库，=true为开启,
		○ info:道具信息，根据sold不同而不同，
			§ sold=0时为：
				□ maxi:可选，背包空间上限, 
				□ ci:[], 
				□ nci:[],
				□ eqp:[]
			§ sold=1时为：
				□ maxi:出售道具空间上限, 
				□ itm:[]
			§ sold=2、3时为：
				□ maxi: 仓库空间或临时仓库空间空间, 
				□ total_cnt:可选，仓库中道具总数, 
				□ begin_idx:可选，开始索引值, 
				□ itm:[]

		 */
		private void onItemRes( GameEvent e )
		{
			//Variant data = e.data;
			//uint sold = data["sold"];
			//if (sold != SOLD_TP_PACKAGE) return;

			//_itemInfo = data;

			
			//dispatchEvent(
			//	GameEvent.Create( GAME_EVENT.S2C_GET_ITEMS_RES, this, null )
			//);
		}


 

 

		/*
		 
		○ id:可选，道具实例id，存在时，则表示某个道具实例发生了变化（可能是已装备的或背包中的），可能的变化字段如下：
			§ expire：可选，过期时间变化,
			§ bnd：可选，绑定属性变化,
			§ tpid：可选，道具类型变化,
		○ add:可选，新增道具数组
			§ id:道具实例id, 
			§ tpid: 道具类型id,
			§ bnd:=true表示已绑定,
			§ cnt:可选，道具数量,
			§ flvl:可选，锻造等级,
			§ fpt:可选，锻造加成百分比,
			§ stn:可选，已镶嵌宝石数组
				□ id:宝石id,
				□ tpid:宝石道具id,
				□ bnd:宝石是否绑定,
			§ dura:可选，耐久度,
			§ expire:可选，过期时间（UNIX时间戳，单位秒）,
			§ wh:可选，已绑定武魂对象，参加武魂对象说明,
			§ bcnt:可选，击碎武器数量,
			§ kcnt:可选，杀敌数量,
			§ brk:可选，=true表示处于被击碎状态,
		○ modcnts:可选，数量变化道具数组
			§ id:道具实例id, 
			§ cnt:道具数量,
		○ rmvids:可选，删除道具实例id数组,

		 */
		private void onItemChange(GameEvent e)
		{
			_packageItemsRefreshFlag = true;
			Variant data = e.data;
            uint itemid = 0;
			if ( data.ContainsKey( "id" ) )
			{ 
				
			}
			else if( data.ContainsKey( "add" ) )
			{
				foreach ( Variant itm in data["add"]._arr )
				{
					_packageItems._arr.Add( itm );
					_packageItemsMap[ itm[ "id" ]._uint ] = itm;

                    dispatchEvent(
				        GameEvent.Create( GAME_EVENT.ITEM_ADD, this, itm )
			        );

				}
			}
			else if ( data.ContainsKey( "rmv" ) )
			{
				foreach ( Variant itm in data[ "rmv" ]._arr )
				{
					_packageItems._arr.Remove( itm );
					_packageItemsMap._dct.Remove( (itm[ "id" ]._uint).ToString() );
                     
                    dispatchEvent(
				        GameEvent.Create( GAME_EVENT.ITEM_RMV, this, itm )
			        );
				}
			}
			else if ( data.ContainsKey( "mod" ) )
			{
				foreach ( Variant itm in data[ "mod" ]._arr )
				{
                    itemid = itm[ "id" ]._uint;
					if( !_packageItemsMap.ContainsKey( itemid.ToString() ) )
				    {//do err!
                        
                    }
                    else
                    {
                        Variant itmCache = _packageItemsMap[ itemid ];
                        foreach( string key in itm.Keys )
                        {
                            itmCache[ key ] = itm[ key ];
                        }
                        dispatchEvent(
				            GameEvent.Create( GAME_EVENT.ITEM_MOD, this, itm )
			            );
                    }
                }
			}

		}




	}
}
 