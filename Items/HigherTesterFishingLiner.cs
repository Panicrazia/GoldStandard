using GoldStandard.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GoldStandard.Items
{
    class HigherTesterFishingLiner : ModItem
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

		public HigherTesterFishingLiner(int strength)
		{
			this.strength = strength;
            switch (strength)
            {
				case 2:
					this.displayName = "tier 2 line (immersify this)";
					break;
				case 3:
					this.displayName = "tier 3 line (immersify this)";
					break;
				case 4:
					this.displayName = "tier 4 line (immersify this)";
					break;
			}
			this.texturePath = "GoldStandard/Items/FishingLinePics/Fishing_Line_Tier_" + strength;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(displayName);
			Tooltip.SetDefault("Line strength of " + strength);
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.accessory = true;
			item.value = Item.sellPrice(silver: 30);
			item.rare = ItemRarityID.Blue;
		}

        public override void UpdateAccessory(Terraria.Player player, bool hideVisual)
        {
			//there has to be a way to simplify this and im just too stupid to realize it
            if (player.GetModPlayer<GoldStandardPlayer>().LineStrength < strength)
            {
				player.GetModPlayer<GoldStandardPlayer>().LineStrength = strength;
			}
		}
	}
}
