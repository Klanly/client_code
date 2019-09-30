using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
namespace MuGame
{
    class GrMonsterAvatar : GRAvatar
    {
         public GrMonsterAvatar(muGRClient ctrl)
            : base(ctrl)
        { }
        public static IObjectPlugin create(IClientBase m)
        {
            return new GrMonsterAvatar(m as muGRClient);
        }

        override protected void onLoadFin()
        {
           
                SkAniMeshImpl m = (m_gr as GRCharacter3D).skmesh as SkAniMeshImpl;
                m.addScript(this._fileName + "(Clone)", "FightAniTempSC");
           
        }
    }
     
}
