using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace GoldStandard.Items
{
    class BaseFishItem : ModItem
	{
		private int strength;
		private string displayName;
		private string texturePath;

		public override bool CloneNewInstances => true;
		public override string Texture => texturePath;


		public override bool Autoload(ref string name)
		{
			return false;
		}

		public BaseFishItem(int baitNumber, int strength, string displayName, string texturePath)
		{
			if (baitNumber > 0)
			{
				item.bait = baitNumber;
			}
			this.strength = strength;
			this.displayName = displayName;
			this.texturePath = texturePath;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(displayName);
			if (item.bait > 0)
			{
				Tooltip.SetDefault("Use as bait to mooch a bigger catch");
			}
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Bass);
		}
		
		public int GetStrength()
        {
			return strength;
        }
	}
}
