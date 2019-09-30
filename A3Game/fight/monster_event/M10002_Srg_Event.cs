using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;


namespace MuGame
{
    class M10002_Srg_Event : Monster_Base_Event
    {
        public void onBullet(int type)
        {
            Vector3 pos = transform.position + transform.forward * 2f;
            pos.y += 1f;
            if (m_monRole.isfake)
            {
                HitData hd = m_monRole.BuildBullet(1, 0.125f, SceneTFX.m_Bullet_Prefabs[1], pos, transform.rotation);
                hd.m_nDamage = 1;
            }
           
        }

    }
}
