using GoldStandard.Managers;
using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using GoldStandard.Items;
using Terraria.ID;
using static GoldStandard.Managers.FishingManager;

namespace GoldStandard.Player
{
    class GoldStandardPlayer : ModPlayer
    {
        public int LineStrength { get; set; }

        public static Dictionary<FishingType, int> additionalFishingPower = new Dictionary<FishingType, int>();
        public int cachedFishingPower = 0;

        /** 
         * <summary>Due to the fact that I have to make my fishing poles have 0 fishingPole (and there should not be any other items with fishingPole > 0 in the players inventory),
         * in order for them to not get hit with a bunch of vanilla effects this is nessecary,
         * as a result the normal tmodloader function for altering the fishing level does not occur</summary>
         */
        public int GetGoldStandardFishingPower()
        {
            int fishingPower = 0;

            if(player.inventory[player.selectedItem].modItem is BaseFishingPole fishingPole)
            {
                BaitEntry entry = FishingManager.FindBait(player, fishingPole.GetFishingType(), out int inventorySlot);
                if (entry != null)
                {
                    //some special logic will be needed for spear fishing is needed due to its bait being chum
                    fishingPower += fishingPole.GoldStandardFishingPower;
                    fishingPower += entry.GetBait();

                    for (int i = 0; i < 8 + player.extraAccessorySlots; i++)
                    {
                        switch (player.armor[i].type)
                        {
                            case 2367:  //angler hat
                            case 2368:  //angler chest
                            case 2369:  //angler pants
                                fishingPower += 5; //should have some sort of set bonus aswell?
                                break;
                            case 2374:  //fishing hook accessory
                            case 3721:  //that grand pumbah fishing accessory
                                fishingPower += 10; //assuming these accessories stay as is cuz right now they are both kinda lame
                                break;
                            default:
                                break;
                        }
                    }

                    //check potions, this shouldnt be needed later when the potion rework goes in
                    for (int j = 0; j < Terraria.Player.MaxBuffs; j++)
                    {
                        if (player.buffType[j] == 121)  //vanilla fishing potion buff
                        {
                            fishingPower += 15;
                        }
                    }

                    fishingPower += additionalFishingPower[fishingPole.GetFishingType()];
                }
            }

            cachedFishingPower = fishingPower;
            return fishingPower;
        }

        public static void StaticlyInitialize()
        {
            foreach (FishingType fishingType in Enum.GetValues(typeof(FishingType)))
            {
                additionalFishingPower.Add(fishingType, 0);
            }
        }

        /**
         * <summary>Use this to alter the fishing power of the player for a given FishingType</summary>
         */
        public static void AlterFishingPower(ref Dictionary<FishingType, int> additionalFishingPower) { }


        public override void AnglerQuestReward(float rareMultiplier, List<Item> rewardItems)
        {
            //Vanilla fishing pole purge episode 4: Mars Needs Moms
            foreach (Item reward in rewardItems)
            {
                if( reward.type == ItemID.HotlineFishingHook || reward.type == ItemID.GoldenFishingRod)
                {
                    rewardItems.Remove(reward);
                }
            }
        }

        public override void ResetEffects()
        {
            LineStrength = 0;
            foreach (FishingType fishingType in Enum.GetValues(typeof(FishingType)))
            {
                additionalFishingPower[fishingType] = 0;
            }
        }
    }
}
