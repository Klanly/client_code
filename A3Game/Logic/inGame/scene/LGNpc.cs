using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;  using Cross;
namespace MuGame
{
	
	
	class LGNpc : LGAvatar
	{
		public LGNpc( gameManager m ):base(m)
		{
		}
		
		private uint _npcid;
		public uint npcid
		{
			get{
				return _npcid;
			}
			set{
				_npcid = value;
			}
		
		}
		override public Variant viewInfo
		{//子类重写
			get{
				return null;
			}
		} 
	}
}
