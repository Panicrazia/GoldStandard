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
    abstract class BaseCloudFishingPole : BaseFishingPole
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Base Cloud Fishing Rod (change this)");
			Tooltip.SetDefault("Fish from the clouds of the world");
		}
		public override void SetDefaults()
		{
			//these arent required to be set here but they should all generally be changed when another fishingpole is created
			base.SetDefaults();
			item.value = 10000;
			item.shoot = mod.ProjectileType("BaseCloudBobber");
			item.shootSpeed = 7f;
		}
        public override FishingType GetFishingType()
        {
            return FishingType.Cloud;
        }
    }
}
