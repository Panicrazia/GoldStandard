using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldStandard.Items.FishingPoles
{
    class WoodenFishingPole : BaseFishingPole
    {
        public override int GoldStandardFishingPower => 10;
        public override int LineOffsetX => 42;
        public override int LineOffsestY => 36;

        public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Wooden Fishing Pole");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.value = 60;
			item.shoot = mod.ProjectileType("WoodenBobber");
			item.shootSpeed = 8f;
		}
	}
}
