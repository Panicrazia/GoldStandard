using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace GoldStandard.Items.FishingPoles
{
    class ReinforcedCageFishingPole : BaseCageFishingPole
	{
		public override int GoldStandardFishingPower => 25;
		public override int LineOffsetX => 50;
		public override int LineOffsestY => 30;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Cage Fishing Chain");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.shoot = mod.ProjectileType("ReinforcedCageBobber");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Starfury);
			recipe.AddRecipeGroup("IronBar", 5);
			recipe.AddTile(TileID.FireflyinaBottle);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
