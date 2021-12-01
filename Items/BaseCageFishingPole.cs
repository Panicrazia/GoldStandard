using GoldStandard.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace GoldStandard.Items
{
	abstract class BaseCageFishingPole : BaseFishingPole
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Base Cage Fishing Rod (change this)");
			Tooltip.SetDefault("Drag along the bottom of the sea to catch something");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.shootSpeed = 5f;

			//these arent required to be set here but they should all be changed when another fishingpole is created
			item.value = 10000;
			item.shoot = mod.ProjectileType("BaseCageBobber");
		}
		public override FishingType GetFishingType()
		{
			return FishingType.Cage;
		}
	}
}
