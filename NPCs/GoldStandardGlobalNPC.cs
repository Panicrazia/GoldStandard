using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GoldStandard.NPCs
{
    class GoldStandardGlobalNPC : GlobalNPC
    {
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if(type == NPCID.Mechanic)
            {
                for (int itemIndex = 0; itemIndex < shop.item.Length; itemIndex++)
                {
                    //Vanilla fishing pole purge episode 2: Electric Boogaloo
                    if (shop.item[itemIndex].type == ItemID.MechanicsRod)
                    {
                        for (int shopIndex = itemIndex + 1; shopIndex < shop.item.Length; shopIndex++)
                        {
                            shop.item[shopIndex - 1] = shop.item[shopIndex];
                        }
                        nextSlot--;
                        break;
                    }
                }
            }
        }

        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
            for (int itemIndex = 0; itemIndex < shop.Length; itemIndex++)
            {
                //Vanilla fishing pole purge episode 3: Rivers of Blood
                if (shop[itemIndex] == ItemID.SittingDucksFishingRod)
                {
                    for (int shopIndex = itemIndex + 1; shopIndex < shop.Length; shopIndex++)
                    {
                        shop[shopIndex - 1] = shop[shopIndex];
                    }
                    nextSlot--;
                    break;
                }
            }
        }
    }
}
