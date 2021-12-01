using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace GoldStandard.Dusts
{
    class CloudDust1 : ModDust
    {
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.frame = new Rectangle(0, 0, 18, 18);
			dust.alpha = 0;
			dust.scale = 5f;
		}

		public override bool Update(Dust dust)
		{
			//half rotate one way, half the other
			dust.rotation += 0.005f * (dust.dustIndex % 2 == 0 ? -1 : 1);
			//ideally have them grow at first then once they hit a point they shrink back down, possibly change alpha along with that\
			dust.alpha++;
			if (dust.alpha > 240)
			{
				dust.active = false;
			}
			return false;
		}
	}
}
