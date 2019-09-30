using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

namespace MuGame
{
    class GRHero : GRAvatar
    {

       
        public GRHero(muGRClient ctrl)
            : base(ctrl)
        { }

        public static IObjectPlugin create(IClientBase m)
        {
            return new GRHero(m as muGRClient);
        }

        override protected void onLoadFin()
        {

            SkAniMeshImpl m = (m_gr as GRCharacter3D).skmesh as SkAniMeshImpl;
            if ((lgAvatar as LGAvatarHero).isUserOwnHero)
                m.addScript(this._fileName + "(Clone)", "FightAniUserTempSC");
            else
                m.addScript(this._fileName + "(Clone)", "FightAniTempSC");



            base.onLoadFin();
        }
    }
}
