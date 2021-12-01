using GoldStandard.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldStandard.Items
{
    abstract class BaseSpearFishingPole : BaseFishingPole
	{
		public override int LineOffsetX => 0;
		public override int LineOffsestY => 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Base Spear Fishing Rod (change this)");
			Tooltip.SetDefault("Spear and pull up monsters of the seas");
		}
		public override void SetDefaults()
		{
			//these arent required to be set here but they should all generally be changed when another fishingpole is created
			base.SetDefaults();
			item.value = 10000;
			item.shoot = mod.ProjectileType("BaseSpearBobber");
			item.shootSpeed = 7f;
		}
		public override FishingType GetFishingType()
		{
			return FishingType.Spear;
		}
		public override void HoldStyle(Terraria.Player player)
		{
			item.holdStyle = 0;
		}
	}
}
