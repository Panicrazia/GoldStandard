using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace GoldStandard.Managers
{
    //needed for futureproofing
    class BiomeManager
    {
        public enum BiomeType
        {
            none,
            purity,
            snow,
            desert,
            jungle
        }

        //make methods for each biome check, so that for each biome check you can short circuit and just check if the player is still in the same biome, reducing many calculations later on
        //also a check for what world type the lpayer is currently in and iterate through subbiomes of that before others, and then dont check them later with via bool flag
        public static BiomeType GetBiomeType(Terraria.Player player)
        {
            if (player.ZoneJungle)
            {
                return BiomeType.jungle;
            }
            if (player.ZoneSnow)
            {
                return BiomeType.snow;
            }
            if (player.ZoneDesert)
            {
                return BiomeType.desert;
            }
            return BiomeType.purity;
        }
    }
}
