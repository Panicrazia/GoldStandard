using GoldStandard.Managers;
using GoldStandard.Projectiles;
using Microsoft.Xna.Framework;
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
    abstract class BaseFishingPole : ModItem
    {
        //why
        //why is my BaseBobber occasionally a spore
        //how does this even happen terraria
        //why
		HashSet<BaseBobber> bobbers = new HashSet<BaseBobber>();
        public abstract int GoldStandardFishingPower { get; }
        public abstract int LineOffsetX { get; }
        public abstract int LineOffsestY { get; }
        /*
         * To find the x and y offset for the line (atleast in paint.net): 
         * 1. Take the texture and drag a box from the top right point of where the line is attatching
         * 2. Pull a box completely to the bottom left
         * 3. The X and Y dimensions of that box are the line offsets
         */

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Cast your line into the waters of the world");
        }

        public override void SetDefaults()
		{
            item.CloneDefaults(ItemID.WoodFishingPole);
            item.fishingPole = 0; //this needs to be 0, must use custom fishingValue in order for vanilla terraria code to not fuck with functionality
            item.width = 24;    //width and height might need to be changed per pole if it matters, but im not actually sure how much it matters for these
			item.height = 28;
			item.useTime = 8;
			item.useAnimation = 8;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.rare = ItemRarityID.White;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;

            //these arent required to be set here but they should all generally be changed when another fishingpole is created
            item.value = 10000;
            item.shoot = mod.ProjectileType("BaseBobber");
            item.shootSpeed = 15f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            //Might be better to override this on the other types and just use magic strings
            tooltips.Insert(1, new TooltipLine(mod, "GoldStandardFishingPower", GoldStandardFishingPower + " " + FishingManager.GetFishingTypeString(this.GetFishingType()) + " Fishing Power"));
        }

        //behold my stupid ass bobber connected to a fishing pole system
        public void AddBobber(BaseBobber bobby)
        {
			bobbers.Add(bobby);
        }
        public bool HasBobbers()
        {
            this.PurgeBobbers();
            return bobbers.Count() != 0;
        }
        public HashSet<BaseBobber> GetBobbers()
        {
            this.PurgeBobbers(); //this check might not be nessecary
            return bobbers;
        }
        public void PurgeBobbers()
        {
            //could make a seprate hashset of bobbers to remove but it wouldnt (shouldnt) be a gain worth doing
            foreach (BaseBobber bobby in this.bobbers.ToList())
            {
                if (!bobby.projectile.active || !ThingIsActuallyABobberAndNotASporeBecauseTerrariaSourceCodeIsDumb(bobby))
                {
                    bobbers.Remove(bobby);
                }
            }
            bobbers.TrimExcess();
        }
        /**
         * <summary>Yep</summary>
         */
        private bool ThingIsActuallyABobberAndNotASporeBecauseTerrariaSourceCodeIsDumb(BaseBobber bobby)
        {
            //should be better but fukit
            return bobby.projectile.bobber;
        }

        public override bool Shoot(Terraria.Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (this.HasBobbers())
            {
                UseItemButItsJankAF(player);
                //its also probably not supposed to be called here, but it doesnt work when its called from the right place (UseItem)
                return false;
            }
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        /**
         * <summary>Also known as 'Reel in'</summary>
         */
        public bool UseItemButItsJankAF(Terraria.Player player)
        {
            bool doneShit = false;
            foreach (BaseBobber bobby in this.GetBobbers())
            {
                //if AI[0] is 0 (not getting reeled in)
                if (bobby.projectile.ai[0] == 0f)
                {
                    //set it to 1 (setting it as reeling in)
                    bobby.projectile.ai[0] = 1f;

                    bobby.ReelInMovement();

                    //mark a packet to be sent since the AI has changed
                    bobby.projectile.netUpdate2 = true;

                    //if the bobber is bobbing and something is on the line
                    if (bobby.projectile.ai[1] < 0f && bobby.projectile.localAI[1] != 0f)
                    {
                        //TODO: this should get redone somewhat, could out the ai change and do the line snapping stuff in the bait method, can also ensure bait gets consumed when line snaps then
                        //but i shall wait until 1.4 since that actually changes what is done here
                        int specificBaitID;
                        bool baitFound = FishingManager.ConsumeBait(player, this.GetFishingType(), out specificBaitID);
                        if (baitFound)
                        {
                            
                            //TODO: make this seprate for a boss/monster spawning function in FishingManager? idk, this shit will change alot in 1.4 anyways, my spearfishing will be mob summons anyways
                            if (specificBaitID == 2673)//truffle worm
                            {
                                //spawn the duke
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    NPC.SpawnOnPlayer(player.whoAmI, 370);
                                }
                                else
                                {
                                    NetMessage.SendData(MessageID.SpawnBoss, -1, -1, null, player.whoAmI, 370f, 0f, 0f, 0, 0, 0);
                                }
                                //and snap the line i guess? never realized this also happened when duke spawns
                                bobby.projectile.ai[0] = 2f;
                            }
                            else if (FishingManager.WillLineSnap(player, (int)(bobby.projectile.localAI[1])))
                            {
                                bobby.projectile.ai[0] = 2f;
                            }
                            //else lock in the catch from localai[1] to ai[1]
                            else
                            {
                                bobby.projectile.ai[1] = bobby.projectile.localAI[1];
                            }
                            //mark a packet to be sent
                            bobby.projectile.netUpdate = true;
                        }
                    }
                }
                doneShit = true;
            }
            return doneShit;
        }

        public virtual FishingType GetFishingType()
        {
            return FishingType.Normal;
        }

        public override void HoldStyle(Terraria.Player player)
        {
            item.holdStyle = 0;
            if (player.itemTime == 0 && player.itemAnimation == 0)
            {
                if (this.HasBobbers())
                {
                    item.holdStyle = 1;
                    player.itemLocation.X -= 18f * player.direction;
                    player.itemLocation.Y += 2f;
                }
            }
        }
    }
}
