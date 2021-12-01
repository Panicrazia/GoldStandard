using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldStandard.Items.FishingPoles
{
    class WoodenCloudFishingPole : BaseCloudFishingPole
	{
		public override int GoldStandardFishingPower => 10;
		public override int LineOffsetX => 50;
		public override int LineOffsestY => 30;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Wooden Cloud Fishing Rod");
		}
		public override void SetDefaults()
		{
			//these arent required to be set here but they should all generally be changed when another fishingpole is created
			base.SetDefaults();
			item.value = 100;
			item.shoot = mod.ProjectileType("WoodenCloudBobber");
			item.shootSpeed = 7f;
		}
	}
}
