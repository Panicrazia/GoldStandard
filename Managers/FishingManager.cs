using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using GoldStandard.Items;
using static GoldStandard.Managers.BiomeManager;
using GoldStandard.Player;

namespace GoldStandard.Managers
{
    public enum BaitType
    {
        Artificial = 7,
        Live = 9,
        Mooch = 11,
        Both = 63
    }

    public enum FishingType
    {
        Normal = 0,
        Cloud = 1,
        Cage = 2,
        Spear = 3,
        Dune = 4
    }

    static class FishingManager
    {
        //TODO: Spear fishing to make an actually good use of chum, aswell as making their promise of new fishing styles actually true with chum??
        //TODO: Dune fishing because it sounds cool

        public static readonly Dictionary<FishingType, Dictionary<string, FishEntry>> fishies = new Dictionary<FishingType, Dictionary<string, FishEntry>>();
        //Bait values subject to change (AS OF RIGHT NOW: if you want the bait value accurately you must get it from the BaitEntry, not the item, if possible I should change the bait values of the vanilla items so this is no longer nessecary)
        public static readonly Dictionary<FishingType, Dictionary<string, BaitEntry>> baities = new Dictionary<FishingType, Dictionary<string, BaitEntry>>();
        /* Break glass incase of 1.4 so I dont have to type out the new vanilla baits again
        //Hell
        baities.Add("Hell Butterfly", new BaitEntry(BaitType.Live, 100, null));
        baities.Add("Magma Snail", new BaitEntry(BaitType.Live, 100, null));
        baities.Add("Lavafly", new BaitEntry(BaitType.Live, 100, null));
        //Dragonflies (cloud)
        baities.Add("Gold Dragonfly", new BaitEntry(BaitType.Live, 25, null));
        baities.Add("Black Dragonfly", new BaitEntry(BaitType.Live, 25, null));
        baities.Add("Blue Dragonfly", new BaitEntry(BaitType.Live, 25, null));
        baities.Add("Green Dragonfly", new BaitEntry(BaitType.Live, 25, null));
        baities.Add("Orange Dragonfly", new BaitEntry(BaitType.Live, 25, null));
        baities.Add("Red Dragonfly", new BaitEntry(BaitType.Live, 25, null));
        baities.Add("Yellow Dragonfly", new BaitEntry(BaitType.Live, 25, null));
        //Ladybug
        baities.Add("Ladybug", new BaitEntry(BaitType.Live, 25, null));
        baities.Add("Gold Ladybug", new BaitEntry(BaitType.Live, 100, null));
        //Water Strider
        baities.Add("Water Strider", new BaitEntry(BaitType.Live, 25, null));
        baities.Add("Gold Water Strider", new BaitEntry(BaitType.Live, 100, null));
        //Maggot
        baities.Add("Maggot", new BaitEntry(BaitType.Live, 25, null));
        */

        public static BaitEntry GetBaitiesEntry(string identifier, out FishingType fishingType)
        {
            foreach (FishingType fishingTypes in Enum.GetValues(typeof(FishingType)))
            {
                if (baities[fishingTypes].ContainsKey(identifier))
                {
                    fishingType = fishingTypes;
                    return baities[fishingTypes][identifier];
                }
            }

            fishingType = FishingType.Normal;
            return null;
        }

        public static void InitializeLists()
        {
            foreach (FishingType fishingType in Enum.GetValues(typeof(FishingType)))
            {
                fishies.Add(fishingType, new Dictionary<string, FishEntry>());
                baities.Add(fishingType, new Dictionary<string, BaitEntry>());
            }

            /* to add specific catch parameters instead of null use 
             * 
             * (maybe turn it into needing to send in a player as a thing?)
            
            () => 
            {
                return true; 
            }

            and put whatever the conditions are in that lambda code bracket

            
            fishiesInit.Add("Fish Name", new FishEntry(
            BaitType.Both, 
            new HashSet<string> { "specific bait" }, 
            new HashSet<BiomeType> { BiomeType.snow }, 
            min fishing power, 
            max fishing power, 
            catch weight, 
            specific catch parameters, 
            "File_Name", 
            "Replacement Fish", 
            strength, 
            bait power));

             */
            Dictionary<string, FishEntry> fishiesInit = fishies[FishingType.Normal];
            fishiesInit.Add("Great Atlantic Cod", new FishEntry(BaitType.Both, null, new HashSet<BiomeType> { BiomeType.snow }, 5, 9999, 40, null, "Great_Atlantic_Cod", "Atlantic Cod", 3, 45));

            fishiesInit = fishies[FishingType.Cloud];
            fishiesInit.Add("Flying Great Atlantic Cod", new FishEntry(BaitType.Both, null, new HashSet<BiomeType> { BiomeType.snow }, 5, 9999, 40, null, "Great_Atlantic_Cod", "Atlantic Cod", 3, 45));
            fishiesInit.Add("Flying Great Jungle Cod", new FishEntry(BaitType.Both, null, new HashSet<BiomeType> { BiomeType.jungle }, 5, 9999, 40, null, "Great_Atlantic_Cod", "Atlantic Cod", 3, 45));
            fishiesInit.Add("Flying Great Desert Cod", new FishEntry(BaitType.Both, null, new HashSet<BiomeType> { BiomeType.desert }, 5, 9999, 40, null, "Great_Atlantic_Cod", "Atlantic Cod", 3, 45));
            fishiesInit.Add("Flying Great Plains Cod", new FishEntry(BaitType.Both, null, new HashSet<BiomeType> { BiomeType.purity }, 5, 9999, 40, null, "Great_Atlantic_Cod", "Atlantic Cod", 3, 45));

            fishiesInit = fishies[FishingType.Cage];
            
            fishiesInit = fishies[FishingType.Dune];
            fishiesInit.Add("Sand Skipper", new FishEntry(BaitType.Both, null, new HashSet<BiomeType> { BiomeType.desert }, 5, 9999, 40, null, "Great_Atlantic_Cod", "Atlantic Cod", 3, 45));
            
            fishiesInit = fishies[FishingType.Spear];


            //------------------------
            Dictionary<string, BaitEntry> baitiesInit = baities[FishingType.Normal];
            //Stupid bait (seriously relogic these three baits, but more specifically master bait just ruins bait as a concept in vanilla)
            //and also calamity just makes it way worse by letting you buy them
            baitiesInit.Add("Apprentice Bait", new BaitEntry(BaitType.Artificial, 10, null));
            baitiesInit.Add("Journeyman Bait", new BaitEntry(BaitType.Artificial, 25, null));
            baitiesInit.Add("Master Bait", new BaitEntry(BaitType.Artificial, 45, null));
            //Wormies
            baitiesInit.Add("Worm", new BaitEntry(BaitType.Live, 25, null));
            baitiesInit.Add("Enchanted Nightcrawler", new BaitEntry(BaitType.Live, 50, null));
            baitiesInit.Add("Gold Worm", new BaitEntry(BaitType.Live, 100, null));
            baitiesInit.Add("Truffle Worm", new BaitEntry(BaitType.Live, 666, null));
            //Grasshoppers
            baitiesInit.Add("Grasshopper", new BaitEntry(BaitType.Live, 25, null));
            baitiesInit.Add("Gold Grasshopper", new BaitEntry(BaitType.Live, 100, null));
            //Underground
            baitiesInit.Add("Snail", new BaitEntry(BaitType.Live, 25, null));
            baitiesInit.Add("Glowing Snail", new BaitEntry(BaitType.Live, 100, null));
            //Jungle
            baitiesInit.Add("Grubby", new BaitEntry(BaitType.Live, 25, null));
            baitiesInit.Add("Sluggy", new BaitEntry(BaitType.Live, 35, null));
            baitiesInit.Add("Buggy", new BaitEntry(BaitType.Live, 45, null));
            //Ocean
            baitiesInit.Add("Blue Jellyfish", new BaitEntry(BaitType.Live, 10, null));
            baitiesInit.Add("Green Jellyfish", new BaitEntry(BaitType.Live, 20, null));
            baitiesInit.Add("Pink Jellyfish", new BaitEntry(BaitType.Live, 30, null));

            baitiesInit = baities[FishingType.Cloud];
            //Butterflies
            baitiesInit.Add("Monarch Butterfly", new BaitEntry(BaitType.Live, 5, null));
            baitiesInit.Add("Sulphur Butterfly", new BaitEntry(BaitType.Live, 10, null));
            baitiesInit.Add("Zebra Swallowtail Butterfly", new BaitEntry(BaitType.Live, 15, null));
            baitiesInit.Add("Ulysses Butterfly", new BaitEntry(BaitType.Live, 20, null));
            baitiesInit.Add("Julia Butterfly", new BaitEntry(BaitType.Live, 25, null));
            baitiesInit.Add("Red Admiral Butterfly", new BaitEntry(BaitType.Live, 30, null));
            baitiesInit.Add("Purple Emperor Butterfly", new BaitEntry(BaitType.Live, 35, null));
            baitiesInit.Add("Tree Nymph Butterfly", new BaitEntry(BaitType.Live, 50, null));
            baitiesInit.Add("Gold Butterfly", new BaitEntry(BaitType.Live, 100, null));
            //Bioluminescents
            baitiesInit.Add("Firefly", new BaitEntry(BaitType.Live, 35, null));
            baitiesInit.Add("Lightning Bug", new BaitEntry(BaitType.Live, 45, null));

            baitiesInit = baities[FishingType.Cage];

            baitiesInit = baities[FishingType.Dune];
            //Desert
            baitiesInit.Add("Scorpion", new BaitEntry(BaitType.Live, 25, null));
            baitiesInit.Add("Black Scorpion", new BaitEntry(BaitType.Live, 100, null));

            baitiesInit = baities[FishingType.Spear];

        }

        /** 
         * <returns>The item ID of the fish caught, 0 if no fish were eligible</returns>
         */
        public static int GetFish(Terraria.Player player, FishingType fishingType)
        {
            int fishingPower = player.GetModPlayer<GoldStandardPlayer>().GetGoldStandardFishingPower();
            BaitEntry type = FindBait(player, fishingType, out int baitSlot); //may want to save the bait entry instead if deciding to go through with bait families idea
            if(type == null)
            {
                return 0;
            }
            string baitName = player.inventory[baitSlot].Name;
            Dictionary<string, FishEntry> baseFishList = fishies[fishingType];
            HashSet<FishEntry> goodFishies = new HashSet<FishEntry>();
            int totalChance = 0;

            //the results of this loop could theoretically be cached and reused if the same params are input, ill only do that if this gets heavy in certain circumstances
            foreach (FishEntry entry in baseFishList.Values) 
            {
                if(entry.HasBiome(BiomeManager.GetBiomeType(player)) && entry.IsBaitCorrect(type.GetBaitType(), baitName))
                {
                    int chance = entry.GetChance(fishingPower);
                    if(chance > 0)
                    {
                        goodFishies.Add(entry);
                        totalChance += chance;
                    }
                }
            }

            totalChance = Main.rand.Next(0, totalChance);

            foreach (FishEntry entry in goodFishies)
            {
                totalChance -= entry.GetChance(fishingPower);
                if(totalChance <= 0)
                {
                    if (entry.ExtraCatchParameters())
                    {
                        return entry.GetItemNumber();
                    }
                    else
                    {
                        return baseFishList[entry.GetReplacementFish()].GetItemNumber();
                    }
                }
            }

            return 0;
        }

        public static bool WillLineSnap(Terraria.Player player, int fish)
        {
            Item item = new Item();
            item.SetDefaults(fish, false);
            if (item.modItem is BaseFishItem fishItem)
            {
                return GetPlayerLineStrength(player) <= fishItem.GetStrength();
            }
            return false;
        }

        /**
         * <summary>Here for convience</summary>
         */
        public static int GetPlayerLineStrength(Terraria.Player player)
        {
            return player.GetModPlayer<GoldStandardPlayer>().LineStrength;
        }

        public static BaitEntry FindBait(Terraria.Player player, FishingType fishingType, out int inventorySlot)
        {
            //needs to be somewhat revamped if I want to do the bait slot/tackle box idea I was thinking of
            //also should be changed to read ammo slots first
            //Im also not sure if returning the baitEntry is nessecary or if just returning the inventorySlot is enough
            int slot = -1;
            while (slot < 58)
            {
                //find an item with bait > 0
                slot++;
                if (player.inventory[slot].stack <= 0 || player.inventory[slot].bait <= 0)
                {
                    continue;
                }

                inventorySlot = slot;

                if (baities[fishingType].ContainsKey(player.inventory[slot].Name))
                {
                    return baities[fishingType][player.inventory[slot].Name];
                }
            }
            inventorySlot = -1;
            return null;
        }

        public static bool ConsumeBait(Terraria.Player player, FishingType fishingType, out int baitID) {
            bool consumeBait = false;
            float consumeChance = FindBait(player, fishingType, out int inventorySlot).GetConsumeChance();
            if (inventorySlot != -1)
            {
                if (player.accTackleBox)
                {
                    //TODO: make the tackle box actually do something, thinking that it reduces the chance for non-mooch constumpion by 5%? seems lame but idk
                }
                if (Main.rand.Next(100) <= consumeChance)
                {
                    consumeBait = true;
                }

                //when localai[1] is negative (???)(it must be when summoning a boss, has to be, ill have to look later to sanity check it)
                /*
                if (bobby.projectile.localAI[1] < 0f)
                {
                    consumeBait = true;
                }
                */
                baitID = player.inventory[inventorySlot].type;
                if (consumeBait)
                {
                    if (ItemLoader.ConsumeItem(player.inventory[inventorySlot], player))
                    {
                        player.inventory[inventorySlot].stack--;
                    }
                    if (player.inventory[inventorySlot].stack <= 0)
                    {
                        player.inventory[inventorySlot].SetDefaults(0, false);
                    }
                }
                return true;
            }
            else
            {
                //player is fishing without bait
                baitID = -1;
                return false;
            }
        }

        public static string GetFishingTypeString(FishingType type)
        {
            switch (type)
            {
                case FishingType.Cloud:
                    return "Cloud";
                case FishingType.Cage:
                    return "Cage";
                default:
                    return "Normal";
            }
        }

        public class FishEntry
        {
#pragma warning disable IDE0044 // Add readonly modifier
            int baitNeeded; //use % when determining (Artificial = 7, Both = 63, Live = 9, Mooch = 11)
            HashSet<string> specificBaitNeeded; //maybe should instead be bait family enum, so i dont have to store that many strings and can have something like desert specific bait for most occasions, and have this as an option still or just roll it into extraCatchParameters?? 
            HashSet<BiomeType> biomes;
            int minimumFishingPower;
            int maximumFishingPower;
            int chance;
            string fileName = string.Empty;
            int itemNumber = 2290; //bass by default
            string replacementFish = string.Empty;
            Func<bool> extraCatchParameters = () => true;
            int strength;
            int baitNumber;
#pragma warning restore IDE0044 // Add readonly modifier

            /** 
             * <summary>Pass in null for the extraCatchParameters if there are no extra catch parameters, replacement fish is the fish given when the extraCatchParameters returns false</summary>
             */
            public FishEntry(BaitType baitNeeded, HashSet<string> specificBaitNeeded, HashSet<BiomeType> biomes, int minimumFishingPower, int maximumFishingPower, int chance, Func<bool> extraCatchParameters, string fileName = "", string replacementFish = "", int strength = 0, int baitNumber = 0)
            {
                this.baitNeeded = (int)baitNeeded;
                this.specificBaitNeeded = specificBaitNeeded;
                this.biomes = biomes;
                this.minimumFishingPower = minimumFishingPower;
                this.maximumFishingPower = maximumFishingPower;
                this.chance = chance;
                if (extraCatchParameters != null)
                {
                    this.extraCatchParameters = extraCatchParameters;
                }
                this.fileName = fileName;
                this.replacementFish = replacementFish;
                this.strength = strength;
                this.baitNumber = baitNumber;
            }

            public void SetItemNumber(int num)
            {
                itemNumber = num;
            }

            public int GetItemNumber()
            {
                return itemNumber;
            }

            public int GetStrength()
            {
                return strength;
            }

            public int GetBaitNumber()
            {
                return baitNumber;
            }

            public String GetFileName()
            {
                return "GoldStandard/Items/FishPicsh/" + fileName;
            }

            /**
             * <summary>Returns true if a new item needs to be created, false otherwise</summary>
             */
            public bool MakeNew()
            {
                return !string.IsNullOrEmpty(fileName);
            }

            public bool IsBaitCorrect(BaitType type, String bait)
            {
                if (baitNeeded % (int)type == 0)
                {
                    if(specificBaitNeeded != null && specificBaitNeeded.Count() > 0)
                    {
                        if (specificBaitNeeded.Contains(bait))
                        {
                            return true;
                        }
                        return false;
                    }
                    return true;
                }
                return false;
            }

            public bool HasBiome(BiomeType biomeToCheck)
            {
                return biomes.Contains(biomeToCheck);
            }

            public int GetChance(int fishingPower)
            {
                if(fishingPower > minimumFishingPower)
                {
                    if(fishingPower > maximumFishingPower)
                    {
                        return chance - (fishingPower - maximumFishingPower);
                    }
                    return chance;
                }
                return 0;
            }

            /** 
             * <summary>I dont know if this is big or smol brain, but I dont want to make 1000 different files for some fish to have special catching parameters</summary>
             */
            internal bool ExtraCatchParameters()
            {
                if (extraCatchParameters != null)
                {
                    return this.extraCatchParameters();
                }
                return true;
            }

            public string GetReplacementFish()
            {
                return replacementFish;
            }
        }

        public class BaitEntry
        {
#pragma warning disable IDE0044 // Add readonly modifier
            private BaitType category;
            private int bait;
            string fileName;
#pragma warning disable IDE0044 // Add readonly modifier

            public BaitEntry(BaitType category, int bait, string fileName)
            {
                this.category = (BaitType)category;
                this.bait = bait;
                this.fileName = fileName;
            }

            public int GetBait()
            {
                return bait;
            }

            public BaitType GetBaitType()
            {
                return category;
            }

            public String GetBaitTypeForTooltip()
            {
                if(category == BaitType.Artificial)
                {
                    return "Artificial (人工)"; 
                }
                return "Live";
            }

            /** 
             * <summary>Returns true if a new item needs to be created, false otherwise</summary>
             */
            public bool MakeNew()
            {
                return !string.IsNullOrEmpty(fileName);
            }

            public int GetConsumeChance()
            {
                switch (category)
                {
                    case BaitType.Artificial:
                        return 10;
                    case BaitType.Live:
                        return 75;
                    default:
                        return 100;
                }
            }

            public String GetFileName()
            {
                return "GoldStandard/Items/BaitPics/" + fileName;
            }
        }
    }
}
