using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldStandard.Items.FishingPoles
{
    class CrimsonFishingSpear : BaseSpearFishingPole
    {
        public override int GoldStandardFishingPower => 666;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Fleshy Fishing Spear");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.value = 6000;
            item.shoot = mod.ProjectileType("CrimsonSpearBobber");
            item.shootSpeed = 15f;
        }
    }
}
