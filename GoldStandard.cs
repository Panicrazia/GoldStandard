using GoldStandard.Items;
using GoldStandard.Managers;
using GoldStandard.Player;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GoldStandard
{
	public class GoldStandard : Mod
	{
		public override void AddRecipes()
		{
			//Vanilla fishing pole purge episode 1
			RecipeFinder finder = new RecipeFinder();
			BigDelete(finder, ItemID.WoodFishingPole);
			BigDelete(finder, ItemID.ReinforcedFishingPole);
			BigDelete(finder, ItemID.FisherofSouls);
			BigDelete(finder, ItemID.Fleshcatcher);
		}

		private void BigDelete(RecipeFinder finder, int itemID)
		{
			finder.SetResult(itemID);
			foreach (Recipe recipe in finder.SearchRecipes())
			{
				RecipeEditor editor = new RecipeEditor(recipe);
				editor.DeleteRecipe();
			}
		}

		public override void Load()
		{
			FishingManager.InitializeLists();
			GoldStandardPlayer.StaticlyInitialize();

			foreach (FishingType fishingType in Enum.GetValues(typeof(FishingType)))
			{
				//fish
				foreach (System.Collections.Generic.KeyValuePair<string, FishingManager.FishEntry> fish in FishingManager.fishies[fishingType])
				{
					if (fish.Value.MakeNew())
					{
						BaseFishItem item = new BaseFishItem(fish.Value.GetBaitNumber(), fish.Value.GetStrength(), fish.Key, fish.Value.GetFileName());
						AddItem(fish.Key.Trim(), item);
						fish.Value.SetItemNumber(item.item.type);
					}
				}
				//baits
				foreach (System.Collections.Generic.KeyValuePair<string, FishingManager.BaitEntry> bait in FishingManager.baities[fishingType])
				{
					if (bait.Value.MakeNew())
					{
						BaseBaitItem item = new BaseBaitItem(bait.Value.GetBait(), bait.Key, bait.Value.GetFileName());
						AddItem(bait.Key.Trim(), item);
					}
				}
			}
			
			//fishing lines
			InitializeFishingLines();

		}

		private void InitializeFishingLines()
        {
			//one is recieved from the normal high test fishing line
            for (int strength = 2; strength <= 4; strength++)
			{
				HigherTesterFishingLiner item = new HigherTesterFishingLiner(strength); 
				AddItem("Fishing_Line_Tier_"+strength, item);
			}
		}
	}
}