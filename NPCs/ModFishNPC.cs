using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace GoldStandard.NPCs
{
	/*
	 * This is the base for all npc critter fish
	 * TODO: add custom catching system for spearfishing
	 * TODO: add parameters for swim height max and swim height min
	 * TODO: add code so they dont run into walls and instead will turn before hitting a wall
	 * TODO: make this an abstract class
	 */
	internal class ModFishNPC : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mod Fish");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.noGravity = true;
			npc.width = 20;
			npc.height = 18;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.5f;
			npc.friendly = true;
			npc.aiStyle = -1;
		}

		public override bool? CanBeHitByItem(Terraria.Player player, Item item)
		{
			return true;
		}

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0;
		}
		
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int i = 0; i < 6; i++)
				{
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, 200, 2 * hitDirection, -2f);
					if (Main.rand.NextBool(2))
					{
						Main.dust[dust].noGravity = true;
						Main.dust[dust].scale = 1.2f * npc.scale;
					}
					else
					{
						Main.dust[dust].scale = 0.7f * npc.scale;
					}
				}
			}
		}

		public override void AI()
		{
			
			if (npc.direction == 0)
			{
				npc.TargetClosest(true);
			}
			
			if (npc.wet)
			{
				if (npc.collideX)
				{
					npc.velocity.X = npc.velocity.X * -1f;
					npc.direction *= -1;
					npc.netUpdate = true;
				}
				if (npc.collideY)
				{
					npc.netUpdate = true;
					if (npc.velocity.Y > 0f)
					{
						npc.velocity.Y = Math.Abs(npc.velocity.Y) * -1f;
						npc.directionY = -1;
						npc.ai[0] = -1f;
					}
					else if (npc.velocity.Y < 0f)
					{
						npc.velocity.Y = Math.Abs(npc.velocity.Y);
						npc.directionY = 1;
						npc.ai[0] = 1f;
					}
				}

				npc.velocity.X += (float)npc.direction * .1f;
				if (npc.velocity.X < -1f || npc.velocity.X > 1f)
				{
					npc.velocity.X = npc.velocity.X * 0.95f;
				}
				if (npc.ai[0] == -1f)
				{
					if ((double)npc.velocity.Y > -.3)
					{
						npc.velocity.Y = npc.velocity.Y - 0.01f;
					}
				}
				else
				{
					if ((double)npc.velocity.Y < .3)
					{
						npc.velocity.Y = npc.velocity.Y + 0.01f;
					}
				}
				if(Main.rand.Next(0,600) < 1 && ((double)npc.velocity.Y >= .3 || (double)npc.velocity.Y <= -.3))
				{
					if (Main.rand.Next(0, 2) < 1)
					{
						npc.velocity.X = npc.velocity.X * -.3f;
						npc.direction *= -1;
						npc.netUpdate = true;
					}
					else
					{
						npc.ai[0] *= -1f;
					}
				}


				//* what this does is make it so creatures that swim will go upwards instead of going downwards too much and hit the bottom
				int num3227 = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
				int num3226 = (int)(npc.position.Y + (float)(npc.height / 2)) / 16;
				if (Main.tile[num3227, num3226 - 1] == null)
				{
					Tile[,] tile3 = Main.tile;
					int num3552 = num3227;
					int num3553 = num3226 - 1;
					Tile tile4 = new Tile();
					tile3[num3552, num3553] = tile4;
				}
				if (Main.tile[num3227, num3226 + 1] == null)
				{
					Tile[,] tile5 = Main.tile;
					int num3554 = num3227;
					int num3555 = num3226 + 1;
					Tile tile6 = new Tile();
					tile5[num3554, num3555] = tile6;
				}
				if (Main.tile[num3227, num3226 + 2] == null)
				{
					Tile[,] tile7 = Main.tile;
					int num3556 = num3227;
					int num3557 = num3226 + 2;
					Tile tile8 = new Tile();
					tile7[num3556, num3557] = tile8;
				}
				if (Main.tile[num3227, num3226 - 1].liquid > 128)
				{
					if (Main.tile[num3227, num3226 + 1].active())
					{
						npc.ai[0] = -1f;
					}
					else if (Main.tile[num3227, num3226 + 2].active())
					{
						npc.ai[0] = -1f;
					}
				}
				if (((double)npc.velocity.Y > 0.4 || (double)npc.velocity.Y < -0.4))
				{
					npc.velocity.Y = npc.velocity.Y * 0.95f;
				}
			}
			//when out of the water bounce around
			else
			{
				if (npc.velocity.Y == 0f)
				{
					npc.velocity.Y = (float)Main.rand.Next(-50, -20) * .1f;
					npc.velocity.X = (float)Main.rand.Next(-20, 20) * 0.1f;
				}
				npc.velocity.Y = npc.velocity.Y + 0.3f;
				if (npc.velocity.Y > 10f)
				{
					npc.velocity.Y = 10f;
				}
				npc.ai[0] = 1f;
			}
			npc.rotation = npc.velocity.Y * (float)npc.direction * 0.1f;
			if ((double)npc.rotation < -0.2)
			{
				npc.rotation = -0.2f;
			}
			if ((double)npc.rotation > 0.2)
			{
				npc.rotation = 0.2f;
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
			npc.frameCounter += 1.0;
			if (npc.wet)
			{
				if (npc.frameCounter < 6.0)
				{
					npc.frame.Y = 0;
				}
				else if (npc.frameCounter < 12.0)
				{
					npc.frame.Y = frameHeight;
				}
				else if (npc.frameCounter < 18.0)
				{
					npc.frame.Y = frameHeight * 2;
				}
				else if (npc.frameCounter < 24.0)
				{
					npc.frame.Y = frameHeight * 3;
				}
				else
				{
					npc.frameCounter = 0.0;
				}
			}
			else if (npc.frameCounter < 6.0)
			{
				npc.frame.Y = frameHeight * 4;
			}
			else if (npc.frameCounter < 12.0)
			{
				npc.frame.Y = frameHeight * 5;
			}
			else
			{
				npc.frameCounter = 0.0;
			}
		}
	}

	/* this stuff below will have to be added to any catchable fish
	
	internal class YellowTangItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yellow Tang");
		}

		public override void SetDefaults()
		{
			//item.useStyle = 1;
			//item.autoReuse = true;
			//item.useTurn = true;
			//item.useAnimation = 15;
			//item.useTime = 10;
			//item.maxStack = 999;
			//item.consumable = true;
			//item.width = 12;
			//item.height = 12;
			//item.makeNPC = 360;
			//item.noUseGraphic = true;
			//item.bait = 15;

			item.CloneDefaults(ItemID.Goldfish);
			item.bait = 17;
			item.makeNPC = (short)NPCType<YellowTangNPC>();
		}
	}

	*/
}
