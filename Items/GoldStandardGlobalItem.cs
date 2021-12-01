using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using GoldStandard.Managers;
using static GoldStandard.Managers.FishingManager;

namespace GoldStandard.Items
{
    class GoldStandardGlobalItem : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if(item.bait > 0) // && item.type < 3930 to get vanilla items  number needs to be changed to 5045 when updating to 1.4 (its the point when item ids are no longer from terraria)
            {
                foreach(TooltipLine tooltip in tooltips.ToList())
                {
                    if(tooltip.Name == "BaitPower")
                    {
                        tooltips.Remove(tooltip);
                        break;
                    }
                }
                BaitEntry entry = FishingManager.GetBaitiesEntry(item.Name, out FishingType type);
                if(entry != null)
                {
                    tooltips.Insert(1, new TooltipLine(mod, "GoldStandardBaitType", entry.GetBaitTypeForTooltip() + " " + type + " Bait"));
                    tooltips.Insert(1, new TooltipLine(mod, "GoldStandardBaitPower", "Bait Power of " + entry.GetBait()));
                }
                else
                {
                    TooltipLine errorLine = new TooltipLine(mod, "GoldStandardBaitError", "Bait incompatible with gold standard fishing, ask mod author");
                    errorLine.isModifier = true;
                    errorLine.isModifierBad = true;
                    tooltips.Insert(1, errorLine);
                }
            }
        }
    }
}
