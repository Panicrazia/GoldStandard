using GoldStandard.Items;
using GoldStandard.Managers;
using GoldStandard.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GoldStandard.Projectiles
{
    abstract class BaseBobber : ModProjectile
    {
		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.penetrate = -1;
			projectile.bobber = true;
		}

		/*How the bobber operates in vanilla
		private void AI_061_FishingBobber()
		{
			//keeps the fishing bobber going permanently
			projectile.timeLeft = 60;
			//checks for if the bobber should not exist anymore
			if (Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].fishingPole == 0 || Main.player[projectile.owner].CCed || Main.player[projectile.owner].noItems)
			{
				projectile.Kill();
			}
			else if (Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].shoot != projectile.type)
			{
				projectile.Kill();
			}
			else if (Main.player[projectile.owner].pulley)
			{
				projectile.Kill();
			}
			else if (Main.player[projectile.owner].dead)
			{
				projectile.Kill();
			}
			//.ai[1] being over 0 means an item is on the line being pulled, localAI[1] being over 0 means theres a fish
			if (projectile.ai[1] > 0f && projectile.localAI[1] >= 0f)
			{
				projectile.localAI[1] = -1f;
				//if it is in water then do a big splash and the fishing sound?
				if (!projectile.lavaWet && !projectile.honeyWet)
				{
					//create 100 dust particles for water, happens when the bobber is reeled in successfully
					for (int i = 0; i < 100; i++)
					{
						int num46 = Dust.NewDust(new Vector2(projectile.position.X - 6f, projectile.position.Y - 10f), projectile.width + 12, 24, Dust.dustWater(), 0f, 0f, 0, default(Color), 1f);
						Dust expr_17D_cp_0 = Main.dust[num46];
						expr_17D_cp_0.velocity.Y = expr_17D_cp_0.velocity.Y - 4f;
						Dust expr_19A_cp_0 = Main.dust[num46];
						expr_19A_cp_0.velocity.X = expr_19A_cp_0.velocity.X * 2.5f;
						Main.dust[num46].scale = 0.8f;
						Main.dust[num46].alpha = 100;
						Main.dust[num46].noGravity = true;
					}
					//bobber reel in sound
					Main.PlaySound(SoundID.Splash, (int)projectile.position.X, (int)projectile.position.Y, 0, 1f, 0f);
				}
			}
			//if being currently reeled in
			if (projectile.ai[0] >= 1f)
			{
				//if line is going to snap
				if (projectile.ai[0] == 2f)
				{
					projectile.ai[0] += 1f;
					Main.PlaySound(SoundID.Item17, projectile.position); //speculating that this is the sound for the bobber breaking

					//if it is in water then do a big splash and the fishing sound?
					if (!projectile.lavaWet && !projectile.honeyWet)
					{
						for (int j = 0; j < 100; j++)
						{
							int num45 = Dust.NewDust(new Vector2(projectile.position.X - 6f, projectile.position.Y - 10f), projectile.width + 12, 24, Dust.dustWater(), 0f, 0f, 0, default(Color), 1f);
							Dust expr_2E0_cp_0 = Main.dust[num45];
							expr_2E0_cp_0.velocity.Y = expr_2E0_cp_0.velocity.Y - 4f;
							Dust expr_2FD_cp_0 = Main.dust[num45];
							expr_2FD_cp_0.velocity.X = expr_2FD_cp_0.velocity.X * 2.5f;
							Main.dust[num45].scale = 0.8f;
							Main.dust[num45].alpha = 100;
							Main.dust[num45].noGravity = true;
						}
						Main.PlaySound(SoundID.Splash, (int)projectile.position.X, (int)projectile.position.Y, 0, 1f, 0f);
					}
				}
				//brings localAI[0] to 100f slowly
				if (projectile.localAI[0] < 100f)
				{
					projectile.localAI[0] += 1f;
				}
				projectile.tileCollide = false;
				int num44 = 10;
				Vector2 vector3 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num43 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector3.X;
				float num42 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector3.Y;
				float num41 = (float)Math.Sqrt((double)(num43 * num43 + num42 * num42));
				//if the bobber is like super far away from the player just straight up murder it
				if (num41 > 3000f)
				{
					projectile.Kill();
				}
				//a small ass number, gets smaller the farther the player is away from the bobber
				num41 = 15.9f / num41;
				//turn these numbers smol
				num43 *= num41;
				num42 *= num41;
				//bobber speeds up as its closer to player
				projectile.velocity.X = (projectile.velocity.X * (float)(num44 - 1) + num43) / (float)num44;
				projectile.velocity.Y = (projectile.velocity.Y * (float)(num44 - 1) + num42) / (float)num44;
				//if player who is rendering the bobber is the owner of said bobber
				if (Main.myPlayer == projectile.owner)
				{
					//hitbox of bobby
					Rectangle rectangle = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
					//hitbox of daddy
					Rectangle value = new Rectangle((int)Main.player[projectile.owner].position.X, (int)Main.player[projectile.owner].position.Y, Main.player[projectile.owner].width, Main.player[projectile.owner].height);
					//if bobby and daddy are on top of each other
					if (rectangle.Intersects(value))
					{
						//if there is something on the line (ie bobber didnt break, something was actually fished up)
						if (projectile.ai[1] > 0f)
						{
							//.ai[1] gets converted to the item being fished up
							int num37 = (int)projectile.ai[1];
							Item item = new Item();
							item.SetDefaults(num37, false);
							//bombfish
							if (num37 == 3196)
							{
								//code resulting in more fishing power equalling more bombfish
								int num36 = Main.player[projectile.owner].FishingLevel();
								int minValue3 = (num36 / 20 + 3) / 2;
								int num35 = (num36 / 10 + 6) / 2;
								if (Main.rand.Next(50) < num36)
								{
									num35++;
								}
								if (Main.rand.Next(100) < num36)
								{
									num35++;
								}
								if (Main.rand.Next(150) < num36)
								{
									num35++;
								}
								if (Main.rand.Next(200) < num36)
								{
									num35++;
								}
								int stack3 = item.stack = Main.rand.Next(minValue3, num35 + 1);
							}
							//frostdagger fish
							if (num37 == 3197)
							{
								//code resulting in more fishingpower equalling more frostdagger fish
								int num34 = Main.player[projectile.owner].FishingLevel();
								int minValue2 = (num34 / 4 + 15) / 2;
								int num33 = (num34 / 2 + 30) / 2;
								if (Main.rand.Next(50) < num34)
								{
									num33 += 4;
								}
								if (Main.rand.Next(100) < num34)
								{
									num33 += 4;
								}
								if (Main.rand.Next(150) < num34)
								{
									num33 += 4;
								}
								if (Main.rand.Next(200) < num34)
								{
									num33 += 4;
								}
								int stack2 = item.stack = Main.rand.Next(minValue2, num33 + 1);
							}
							//modloader method that is responsible for the number of items in a fishing stack (probably invalidates the above stuff for bombfish/frostdagger fish)
							ItemLoader.CaughtFishStack(item);
							//i think newAndShiny is what makes the item have that glow when you havent hovered it yet in your inventory?
							item.newAndShiny = true;
							//(try to) give the player the item... (or maybe its the other way around?)
							if (Main.player[projectile.owner].GetItem(projectile.owner, item, false, false).stack > 0)
							{
								//i dunno what this is supposed to do
								int number = Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, num37, 1, false, 0, true, false);
								if (Main.netMode == NetmodeID.MultiplayerClient)
								{
									NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f, 0f, 0f, 0, 0, 0);
								}
							}
							//if you cant give them (inventory full, quest fish, any other reason) then drop it as an item in world
							else
							{
								item.position.X = projectile.Center.X - (float)(item.width / 2);
								item.position.Y = projectile.Center.Y - (float)(item.height / 2);
								item.active = true;
								ItemText.NewText(item, 0, false, false);
							}
						}
						//after the bobber hits and delivers the items kill it
						projectile.Kill();
					}
				}
				//rotate bobber in whatever direction it is travelling in
				projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			}
			else
			{
				//bobberEligibleForLoot represents the bobber being able to catch something (on the water surface)
				bool bobberEligibleForLoot = false;
				//if the projectile is far away from the player then begin returning (.ai[0] being >= 1 is setting it to return to player)
				Vector2 vector2 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num32 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector2.X;
				float num31 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector2.Y;
				projectile.rotation = (float)Math.Atan2((double)num31, (double)num32) + 1.57f;
				if ((float)Math.Sqrt((double)(num32 * num32 + num31 * num31)) > 900f)
				{
					projectile.ai[0] = 1f;
				}
				//if the projectile is in liquid, move accordingly
				if (projectile.wet)
				{
					//set rotation to normal
					projectile.rotation = 0f;
					//simulate water friction
					projectile.velocity.X = projectile.velocity.X * 0.9f;
					//center coordinates are set, X is including the direction that it is moving into
					int xBobberCenterCoordinate = (int)(projectile.Center.X + (float)((projectile.width / 2 + 8) * projectile.direction)) / 16;
					int yBobberCenterCoordinate = (int)(projectile.Center.Y / 16f);
					float num57 = projectile.position.Y / 16f;                 //num57 is just unused?
					//if my head math is correct then num 28 is effectively the lowest point on the bobber
					int num28 = (int)((projectile.position.Y + (float)projectile.height) / 16f);
					//i have literally no idea what these 2 if statements purpose is for
					if (Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate] == null)
					{
						Tile[,] tile = Main.tile;
						int num47 = xBobberCenterCoordinate;
						int num48 = yBobberCenterCoordinate;
						Tile tile2 = new Tile();
						tile[num47, num48] = tile2;
					}
					if (Main.tile[xBobberCenterCoordinate, num28] == null)
					{
						Tile[,] tile3 = Main.tile;
						int num49 = xBobberCenterCoordinate;
						int num50 = num28;
						Tile tile4 = new Tile();
						tile3[num49, num50] = tile4;
					}
					//simulate water bouyancy
					if (projectile.velocity.Y > 0f)
					{
						projectile.velocity.Y = projectile.velocity.Y * 0.5f;
					}
					//exact center coordinates set for where the bobber currently is
					xBobberCenterCoordinate = (int)(projectile.Center.X / 16f);
					yBobberCenterCoordinate = (int)(projectile.Center.Y / 16f);
					float waterHeight = projectile.position.Y + (float)projectile.height;
					//i have literally no idea what these 3 if statements purpose is for
					if (Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate - 1] == null)
					{
						Tile[,] tile5 = Main.tile;
						int num51 = xBobberCenterCoordinate;
						int num52 = yBobberCenterCoordinate - 1;
						Tile tile6 = new Tile();
						tile5[num51, num52] = tile6;
					}
					if (Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate] == null)
					{
						Tile[,] tile7 = Main.tile;
						int num53 = xBobberCenterCoordinate;
						int num54 = yBobberCenterCoordinate;
						Tile tile8 = new Tile();
						tile7[num53, num54] = tile8;
					}
					if (Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate + 1] == null)
					{
						Tile[,] tile9 = Main.tile;
						int num55 = xBobberCenterCoordinate;
						int num56 = yBobberCenterCoordinate + 1;
						Tile tile10 = new Tile();
						tile9[num55, num56] = tile10;
					}
					//if there is any liquid in the tiles above, at, or below the tiles of the bobber then waterHeight will be the ylevel of said water, or the highest point of the tile above the bobber if they are all full
					if (Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate - 1].liquid > 0)
					{
						waterHeight = (float)(yBobberCenterCoordinate * 16);
						waterHeight -= (float)((int)Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate - 1].liquid / 16);
					}
					else if (Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate].liquid > 0)
					{
						waterHeight = (float)((yBobberCenterCoordinate + 1) * 16);
						waterHeight -= (float)((int)Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate].liquid / 16);
					}
					else if (Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate + 1].liquid > 0)
					{
						waterHeight = (float)((yBobberCenterCoordinate + 2) * 16);
						waterHeight -= (float)((int)Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate + 1].liquid / 16);
					}
					//if the center of it is under the water level then go up until you hit the surface
					if (projectile.Center.Y > waterHeight)
					{
						projectile.velocity.Y = projectile.velocity.Y - 0.1f;
						if (projectile.velocity.Y < -8f)
						{
							projectile.velocity.Y = -8f;
						}
						if (projectile.Center.Y + projectile.velocity.Y < waterHeight)
						{
							projectile.velocity.Y = waterHeight - projectile.Center.Y;
						}
					}
					//else set the velocity to the difference between the current height and the water height
					else
					{
						projectile.velocity.Y = waterHeight - projectile.Center.Y;
					}
					//if the bobber is essentially not moving on the water surface then bobberEligibleForLoot is set to true
					if ((double)projectile.velocity.Y >= -0.01 && (double)projectile.velocity.Y <= 0.01)
					{
						bobberEligibleForLoot = true;
					}
				}
				//if it is not wet, follow physics normally
				else
				{
					//if it is not moving vertically
					if (projectile.velocity.Y == 0f)
					{
						//then slow it horizontally
						projectile.velocity.X = projectile.velocity.X * 0.95f;
					}
					//simulate air friction and gravity
					projectile.velocity.X = projectile.velocity.X * 0.98f;
					projectile.velocity.Y = projectile.velocity.Y + 0.2f;
					//gravity cap
					if (projectile.velocity.Y > 15.9f)
					{
						projectile.velocity.Y = 15.9f;
					}
				}
				//essentially if you have bait which will summon a boss find a way to make the players fishing level -1
				if (Main.myPlayer == projectile.owner)
				{
					switch (Main.player[projectile.owner].FishingLevel())
					{
						case -1:
							Main.player[projectile.owner].displayedFishingInfo = Language.GetTextValue("GameUI.FishingWarning");
							break;
					}
				}
				//.ai[1] being negative means the bobber is bobbing
				if (projectile.ai[1] != 0f)
				{
					bobberEligibleForLoot = true;
				}
				//self explanatory
				if (bobberEligibleForLoot)
				{
					//this is the section of code that gets hit once and determines what is waiting on the line, if anything, also handles lava/honey fishing
					if (projectile.ai[1] == 0f && Main.myPlayer == projectile.owner)
					{
						int fishingLevel = Main.player[projectile.owner].FishingLevel();
						//i have no idea what would make the player have -9000 fishing level, or why this code is a thing, i can only imagine its for testing purposes and left over
						if (fishingLevel == -9000)
						{
							projectile.localAI[1] += 5f;
							projectile.localAI[1] += (float)Main.rand.Next(1, 3);
							if (projectile.localAI[1] > 660f)
							{
								projectile.localAI[1] = 0f;
								projectile.FishingCheck();
							}
						}
						else
						{
							//the higher your fishing level is the faster you catch
							if (Main.rand.Next(300) < fishingLevel)
							{
								projectile.localAI[1] += (float)Main.rand.Next(1, 3);
							}
							projectile.localAI[1] += (float)(fishingLevel / 30);
							projectile.localAI[1] += (float)Main.rand.Next(1, 3);
							if (Main.rand.Next(60) == 0)
							{
								projectile.localAI[1] += 60f;
							}

							//once threshhold hits then the loot check happens
							if (projectile.localAI[1] > 660f)
							{
								projectile.localAI[1] = 0f;
								//this method sets .ai[1] to a negative number while the fish is on the line, and .localAI[1] to the id of the fish caught
								projectile.FishingCheck();
							}
						}
					}
					//if a fish is currently on the line, bobbing the bobber
					else if (projectile.ai[1] < 0f)
					{
						//if its not moving OR it is essentially not moving in honey
						if (projectile.velocity.Y == 0f || (projectile.honeyWet && (double)projectile.velocity.Y >= -0.01 && (double)projectile.velocity.Y <= 0.01))
						{
							//bobb the bobber
							projectile.velocity.Y = (float)Main.rand.Next(100, 500) * 0.015f;
							projectile.velocity.X = (float)Main.rand.Next(-100, 101) * 0.015f;
							projectile.wet = false;
							projectile.lavaWet = false;
							projectile.honeyWet = false;
						}
						//a slow auto reset, when .ai[1] hits positive numbers the fish unhooks itself and is lost
						projectile.ai[1] += (float)Main.rand.Next(1, 5);
						if (projectile.ai[1] >= 0f)
						{
							projectile.ai[1] = 0f;
							projectile.localAI[1] = 0f;
							projectile.netUpdate = true;
						}
					}
				}
			}
		}*/

		public override void AI()
		{
			/* AI chart
			 *				|  negative				|	zero	|  positive
			 * ai[0]		|						|			|	currently being reeled in, 2 if the line is going to break, 3 if it has already broken
			 * ai[1]		|  fish unhook timer	|			|	fish value locked in and on the line
			 *				|						|			|
			 * localAi[0]	|						|			|	???
			 * localAi[1]	|						|			|	fish value waiting to be reeled in (if ai[1] is negative)
			 *
			 */
			if (projectile.timeLeft > 60) //should only fire once per bobber on its creation, adding it to the relevant fishingpole
			{
				if (Main.myPlayer == projectile.owner)
				{
					Item fishingPole = Main.player[projectile.owner].HeldItem;
					if(fishingPole.modItem is BaseFishingPole baseFishingPole)
                    {
						baseFishingPole.AddBobber(this);
                    }
                    else
                    {
						//BaseBobber created with no BaseFishingPole to attatch to, this should never happen
						projectile.Kill();
					}
				}
			}
			projectile.timeLeft = 60;

            if (BobberShouldKill())
            {
				projectile.Kill();
            }
			
			//.ai[1] being over 0 means an item is on the line being pulled, localAI[1] being over 0 means theres a fish waiting to be reeled in
			if (projectile.ai[1] > 0f && projectile.localAI[1] >= 0f)
			{
				projectile.localAI[1] = -1f;
				ReelInSplash();
			}
			Vector2 projectileVector = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
			float distanceX = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - projectileVector.X;
			float distanceY = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - projectileVector.Y;
			float distance = (float)Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));
			//if being currently reeled in
			if (projectile.ai[0] >= 1f)
			{
				if (distance > 3000f)
				{
					projectile.Kill();
				}

				//TODO: this should be its own method since breaksounds may not always be that
				if (projectile.ai[0] == 2f) //if line is going to snap
				{
					projectile.ai[0] += 1f;
					Main.PlaySound(SoundID.Item17, projectile.position); //speculating that this is the sound for the bobber breaking
					ReelInSplash();
				}

				projectile.tileCollide = false;

				//brings localAI[0] to 100f slowly, i dont know why this is important or if it is even needed, but it was in the vanilla code
				//************this influences the string with the bobber when it is getting pulled in
				if (projectile.localAI[0] < 100f)
				{
					projectile.localAI[0] += 1f;
				}

				int num44 = 10;

				distance = 15.9f / distance;
				distanceX *= distance;
				distanceY *= distance;
				projectile.velocity.X = (projectile.velocity.X * (float)(num44 - 1) + distanceX) / (float)num44;
				projectile.velocity.Y = (projectile.velocity.Y * (float)(num44 - 1) + distanceY) / (float)num44;

				//if player who is rendering the bobber is the owner of said bobber
				if (Main.myPlayer == projectile.owner)
				{
					Rectangle bobberHitbox = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
					Rectangle playerHitbox = new Rectangle((int)Main.player[projectile.owner].position.X, (int)Main.player[projectile.owner].position.Y, Main.player[projectile.owner].width, Main.player[projectile.owner].height);
					if (bobberHitbox.Intersects(playerHitbox))
					{
						//if there is something on the line (ie bobber didnt break, something was actually fished up)
						if (projectile.ai[1] > 0f)
						{
							//.ai[1] gets converted to the item being fished up
							int catchID = (int)projectile.ai[1];
							Item item = new Item();
							item.SetDefaults(catchID, false); //TODO: LENGTH DETERMINATION GOES HERE IF I PLAN TO DO THAT
							//modloader method that is responsible for the number of items in a fishing stack
							ItemLoader.CaughtFishStack(item);
							//i think newAndShiny is what makes the item have that glow when you havent hovered it yet in your inventory?
							item.newAndShiny = true;

							//(try to) give the player the item... (or maybe its the other way around?)
							if (Main.player[projectile.owner].GetItem(projectile.owner, item, false, false).stack > 0)
							{
								//i dunno what this is supposed to do
								int number = Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, catchID, 1, false, 0, true, false);
								if (Main.netMode == NetmodeID.MultiplayerClient)
								{
									NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 1f, 0f, 0f, 0, 0, 0);
								}
							}
							//if you cant give them (inventory full, quest fish, any other reason) then drop it as an item in world
							else
							{
								item.position.X = projectile.Center.X - (float)(item.width / 2);
								item.position.Y = projectile.Center.Y - (float)(item.height / 2);
								item.active = true;
								ItemText.NewText(item, 0, false, false);
							}
						}
						projectile.Kill();
					}
				}
				projectile.rotation = GetReturnRotation();
			}
			else
			{
				if (distance > 900f)
				{
					projectile.ai[0] = 1f;
				}

				bool bobberEligibleForLoot = projectile.wet ? WetMovement() : DryMovement();

				if (projectile.ai[1] == 0f && bobberEligibleForLoot && Main.myPlayer == projectile.owner)
				{
					BiteChance();
				}
				else if(projectile.ai[1] < 0f)
				{
					Bobb();
					UnhookTimerTick();
				}
			}
		}

		virtual public float GetReturnRotation()
        {
			return (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
		}

		/**
		 * <summary>Decides if the bobber should dissapear if the player hits various conditions</summary>
		 */
		virtual public bool BobberShouldKill()
        {
			return Main.player[projectile.owner].CCed
				|| Main.player[projectile.owner].noItems
				|| Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].shoot != projectile.type
				|| Main.player[projectile.owner].pulley
				|| Main.player[projectile.owner].dead;
		}

		/**
		 * <summary>Causes the splash particles and sound when the bobber is reeled in</summary>
		 */
		virtual public void ReelInSplash()
        {
			if (!projectile.lavaWet && !projectile.honeyWet)
			{
				//create 100 dust particles for water
				for (int i = 0; i < 100; i++)
				{
					int num46 = Dust.NewDust(new Vector2(projectile.position.X - 6f, projectile.position.Y - 10f), projectile.width + 12, 24, Dust.dustWater(), 0f, 0f, 0, default(Color), 1f);
					Dust expr_17D_cp_0 = Main.dust[num46];
					expr_17D_cp_0.velocity.Y -= 4f;
					Dust expr_19A_cp_0 = Main.dust[num46];
					expr_19A_cp_0.velocity.X *= 2.5f;
					Main.dust[num46].scale = 0.8f;
					Main.dust[num46].alpha = 100;
					Main.dust[num46].noGravity = true;
				}
				//sound
				Main.PlaySound(SoundID.Splash, (int)projectile.position.X, (int)projectile.position.Y, 0, 1f, 0f);
			}
		}

		/**
		 * <summary>Movement for when the bobber is wet, returns true if the bobber should be eligible for loot checking</summary>
		 */
		virtual public bool WetMovement()
        {
			bool bobberEligibleForLoot = false;

			//currently all just vanilla fishing junk, im not sure how much of it is actually useful

			//set rotation to normal
			projectile.rotation = 0f;

			//simulate water friction
			projectile.velocity.X = projectile.velocity.X * 0.9f;
			//simulate water bouyancy
			if (projectile.velocity.Y > 0f)
			{
				projectile.velocity.Y = projectile.velocity.Y * 0.5f;
			}

			//exact center coordinates set for where the bobber currently is
			int xBobberCenterCoordinate = (int)(projectile.Center.X / 16f);
			int yBobberCenterCoordinate = (int)(projectile.Center.Y / 16f);
			float waterHeight = projectile.position.Y + (float)projectile.height;


			//if there is any liquid in the tiles above, at, or below the tiles of the bobber then waterHeight will be the ylevel of said water, or the highest point of the tile above the bobber if they are all full
			if (Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate - 1].liquid > 0)
			{
				waterHeight = (float)(yBobberCenterCoordinate * 16);
				waterHeight -= (float)((int)Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate - 1].liquid / 16);
			}
			else if (Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate].liquid > 0)
			{
				waterHeight = (float)((yBobberCenterCoordinate + 1) * 16);
				waterHeight -= (float)((int)Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate].liquid / 16);
			}
			else if (Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate + 1].liquid > 0)
			{
				waterHeight = (float)((yBobberCenterCoordinate + 2) * 16);
				waterHeight -= (float)((int)Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate + 1].liquid / 16);
			}

			//if the center of it is under the water level then go up until you hit the surface
			if (projectile.Center.Y > waterHeight)
			{
				projectile.velocity.Y = projectile.velocity.Y - 0.1f;
				if (projectile.velocity.Y < -8f)
				{
					projectile.velocity.Y = -8f;
				}
				if (projectile.Center.Y + projectile.velocity.Y < waterHeight)
				{
					projectile.velocity.Y = waterHeight - projectile.Center.Y;
				}
			}
			//else set the velocity to the difference between the current height and the water height
			else
			{
				projectile.velocity.Y = waterHeight - projectile.Center.Y;
			}
			//if the bobber is essentially not moving on the water surface then bobberEligibleForLoot is set to true
			if ((double)projectile.velocity.Y >= -0.01 && (double)projectile.velocity.Y <= 0.01)
			{
				bobberEligibleForLoot = true;
			}
			return bobberEligibleForLoot;
		}

		/**
		 * <summary>Movement for when the bobber is dry, returns true if the bobber should be eligible for loot checking</summary>
		 */
		virtual public bool DryMovement()
		{
			Vector2 vector2 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
			float num32 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector2.X;
			float num31 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector2.Y;
			projectile.rotation = (float)Math.Atan2((double)num31, (double)num32) + 1.57f;

			//if it is not moving vertically
			if (projectile.velocity.Y == 0f)
			{
				//then slow it horizontally
				projectile.velocity.X = projectile.velocity.X * 0.95f;
			}
			//simulate air friction and gravity
			projectile.velocity.X = projectile.velocity.X * 0.98f;
			projectile.velocity.Y = projectile.velocity.Y + 0.2f;
			//gravity cap
			if (projectile.velocity.Y > 15.9f)
			{
				projectile.velocity.Y = 15.9f;
			}

			return false;
		}

		/**
		 * <summary>Movement for when the bobber is reeled in, fires once when immediately reeled in</summary>
		 */
		virtual public void ReelInMovement()
		{
			if (projectile.velocity.Y > -10)
			{
				projectile.velocity.Y = -10;
			}
		}

		/**
		 * <summary>Bobb the bobber</summary>
		 */
		virtual public void Bobb()
		{
			if (projectile.velocity.Y == 0f || (projectile.honeyWet && (double)projectile.velocity.Y >= -0.01 && (double)projectile.velocity.Y <= 0.01))
			{
				projectile.velocity.Y = (float)Main.rand.Next(100, 500) * 0.015f;
				projectile.velocity.X = (float)Main.rand.Next(-100, 101) * 0.015f;
				projectile.wet = false;
				projectile.lavaWet = false;
				projectile.honeyWet = false;
			}
		}

		/**
		 * <summary>Decide how the unhooking timer is calculated</summary>
		 */
		virtual public void UnhookTimerTick()
        {
			projectile.ai[1] += (float)Main.rand.Next(1, 5);
			if (projectile.ai[1] >= 0f)
			{
				projectile.ai[1] = 0f;
				projectile.localAI[1] = 0f;
				projectile.netUpdate = true;
			}
		}

		/** <summary>Determines when FishingCheck should happen</summary> 
		 */
		public void BiteChance() 
        {
			int fishingLevel = Main.player[projectile.owner].GetModPlayer<GoldStandardPlayer>().GetGoldStandardFishingPower();
			//the higher your fishing level is the faster you catch
			if (Main.rand.Next(300) < fishingLevel)
			{
				projectile.localAI[1] += (float)Main.rand.Next(1, 3);
			}
			projectile.localAI[1] += (float)(fishingLevel / 30);
			projectile.localAI[1] += (float)Main.rand.Next(1, 3);
			if (Main.rand.Next(60) == 0)
			{
				projectile.localAI[1] += 60f;
			}

			//once threshhold hits then the loot check happens
			if (projectile.localAI[1] > 660f)
			{
				projectile.localAI[1] = 0f;
				//this method sets .ai[1] to a negative number to time while the fish is on the line, and .localAI[1] to the id of the fish caught
				FishingCheck(fishingLevel);
			}
		}

		/**
		 * <summary>Fired when the bobber decides what should be on the line, if anything (the only time for nothing is if there is no eligible fish)</summary>
		 */
		public void FishingCheck(int fishingLevel)
		{
			if (fishingLevel != 0) //precautionary check leftover from vanilla that fishing level is not zero
			{
				//display fishing level
				Main.player[projectile.owner].displayedFishingInfo = Language.GetTextValue("GameUI.FishingPower", fishingLevel);
				if (fishingLevel < 0)
				{
					//warning display
					if (fishingLevel == -1)
					{
						Main.player[projectile.owner].displayedFishingInfo = Language.GetTextValue("GameUI.FishingWarning");
					}
				}
				//maybe the check environment check should be in the movement portions and baked into the non moving portion of some of them?
				else if (CheckEnvironment())
				{
					int catchID = FishingManager.GetFish(Main.player[projectile.owner], GetFishingType());

					if(catchID > 0)
                    {
						LockInCatch(catchID, fishingLevel);
					}

					//just to remember how to check for crate potion: Main.player[projectile.owner].cratePotion

					//TODO: add a hook for other modders to alter fishing result 
					// ^ for conventional fish and whatever though there should be hooks to add them to the fish population hashes
				}
			}
		}

		virtual public FishingType GetFishingType()
        {
			return FishingType.Normal;
        }

		/**
		 * <summary>Returns true if the bobber should attempt to hook a fish</summary>
		 */
		virtual public bool CheckEnvironment()
        {
			//this probably could be phased out and not used
			return true;
        }

		/**
		 * <summary>Method responsible for locking the fishing loot into the bobbers AI, also handles sonar potion at the moment</summary>
		 */
		public void LockInCatch(int catchID, int fishingLevel)
        {
			//sonar potion effect
			if (Main.player[projectile.owner].sonarPotion)
			{
				Item item = new Item();
				item.SetDefaults(catchID, false);
				item.position = projectile.position;
				ItemText.NewText(item, 1, true, false);
			}

			//TODO: this will get weird with how high my fishing values can get, atm is just what vanilla was so this should probs be altered in some way
			//ai[1] is set to a random negative value, lowered further by the fishing level, is how long the fish will stay hooked
			projectile.ai[1] = (float)Main.rand.Next(-240, -90) - fishingLevel;
			//localAI[1] stores the catchID, later transfered into .ai[0]on reel in
			projectile.localAI[1] = (float)catchID;
			projectile.netUpdate = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.X == 0)
			{
				projectile.velocity.X = -oldVelocity.X * .5f;
			}
			if (projectile.velocity.Y == 0)
			{
				projectile.velocity.Y = -oldVelocity.Y * .5f;
			}
			return false;
		}

		/* vanilla code for fishing check
		public void FishingCheck() //Getting the catch
		{
			int xTileCoord = (int)(projectile.Center.X / 16f);
			int yTileCoord = (int)(projectile.Center.Y / 16f);
			//if there is no liquid in the tile grabbed then grab the tile below it
			if (Main.tile[xTileCoord, yTileCoord].liquid < 0)
			{
				yTileCoord++;
			}
			bool lavaFishing = false;
			bool honeyFishing = false;
			int xTileCoordAgain = xTileCoord;
			int xTileCoordAgainAgain = xTileCoord;
			//counter to see how wide the fishing pool is to the left
			while (xTileCoordAgain > 10 && Main.tile[xTileCoordAgain, yTileCoord].liquid > 0 && !WorldGen.SolidTile(xTileCoordAgain, yTileCoord))
			{
				xTileCoordAgain--;
			}
			//counter to see how wide the fishing pool is to the right
			while (xTileCoordAgainAgain < Main.maxTilesX - 10 && Main.tile[xTileCoordAgainAgain, yTileCoord].liquid > 0 && !WorldGen.SolidTile(xTileCoordAgainAgain, yTileCoord)){
				xTileCoordAgainAgain++;
			}
			int totalWaterTiles = 0;
			//for loop to go through the entire pond to check the depth of each x coord, adds to the total tiles every new tile
			for (int i = xTileCoordAgain; i <= xTileCoordAgainAgain; i++)
			{
				int currentDepth = yTileCoord;
				while (Main.tile[i, currentDepth].liquid > 0 && !WorldGen.SolidTile(i, currentDepth) && currentDepth < Main.maxTilesY - 10)
				{
					totalWaterTiles++;
					currentDepth++;
					//check to see if the current liquid is lava (there is no reason these checks need to be called every time right? this could be done once before this potentially massive loop?)
					if (Main.tile[i, currentDepth].lava())
					{
						lavaFishing = true;
					}
					//check to see if the current liquid is honey (there is no reason these checks need to be called every time right? this could be done once before this potentially massive loop?)
					else if (Main.tile[i, currentDepth].honey())
					{
						honeyFishing = true;
					}
				}
			}
			//artificially inflate totalWaterTiles if fishing in honey
			if (honeyFishing)
			{
				totalWaterTiles = (int)((double)totalWaterTiles * 1.5);
			}
			int fishingLevel;
			int catchID;
			 * Layer values:
			 * 0: sky islands
			 * 1: surface
			 * 2: underground
			 * 3: caverns
			 * 4: underworld
		
			int layer;
			bool common;
			bool uncommon;
			bool rare;
			bool veryRare;
			bool extremelyRare;
			int anglerFishID;
			bool junk;
			//if there are not enough tiles the players fishing info tells them so and the method ends
			if (totalWaterTiles < 75)
			{
				Main.player[projectile.owner].displayedFishingInfo = Language.GetTextValue("GameUI.NotEnoughWater");
			}
			else
			{
				fishingLevel = Main.player[projectile.owner].FishingLevel();
				if (fishingLevel != 0) //looks like a precautionary check that fishing level is not zero
				{
					//display fishing level
					Main.player[projectile.owner].displayedFishingInfo = Language.GetTextValue("GameUI.FishingPower", fishingLevel);
					//if fishing power is negative
					if (fishingLevel < 0)
					{
						//if fishing power is negative 1 then display the warning
						if (fishingLevel == -1)
						{
							Main.player[projectile.owner].displayedFishingInfo = Language.GetTextValue("GameUI.FishingWarning");
							//ocean biome check
							if (xTileCoord >= 380 && xTileCoord <= Main.maxTilesX - 380)
							{
								return;
							}
							//1000 water tiles and duke fishron is not currently alive then do that stuff and return
							if (totalWaterTiles > 1000 && !NPC.AnyNPCs(370))
							{
								projectile.ai[1] = (float)(Main.rand.Next(-180, -60) - 100);
								projectile.localAI[1] = (float)fishingLevel;
								projectile.netUpdate = true;
							}
						}
					}
					else
					{
						int idealWaterTiles = 300;
						//worldSize will be 1 for small worlds, 1.5 for medium and 2 for large worlds
						float worldSize = (float)(Main.maxTilesX / 4200);
						//worldSize will be 1 for small worlds, 2.25 for medium and 4 for large worlds
						worldSize *= worldSize;
						//i dont know wtf this calculation is supposed to matter for, but it seems that it gets smaller the lower in a world you are?
						float worldHeightAdjustment = (float)((double)(projectile.position.Y / 16f - (60f + 10f * worldSize)) / (Main.worldSurface / 6.0));
						if ((double)worldHeightAdjustment < 0.25)
						{
							worldHeightAdjustment = 0.25f;
						}
						if (worldHeightAdjustment > 1f)
						{
							worldHeightAdjustment = 1f;
						}
						//idealWaterTiles is anywhere from 75 to 300, based on world size and where the projectile is in the world vertially, with higher being closer to 300 (forealz tho why)
						idealWaterTiles = (int)((float)idealWaterTiles * worldHeightAdjustment);
						//it must be the fishingpower reduction for smaller pools and just noone knew that the maximum pool size is smaller the lower you are? wtf?
						float fishingPowerReduction = (float)totalWaterTiles / (float)idealWaterTiles;
						if (fishingPowerReduction < 1f)
						{
							fishingLevel = (int)((float)fishingLevel * fishingPowerReduction);
						}
						fishingPowerReduction = 1f - fishingPowerReduction;
						if (totalWaterTiles < idealWaterTiles)
						{
							Main.player[projectile.owner].displayedFishingInfo = Language.GetTextValue("GameUI.FullFishingPower", fishingLevel, 0.0 - Math.Round((double)(fishingPowerReduction * 100f)));
						}
						int biteChance = (fishingLevel + 75) / 2;
						//if the player is not wet and the bite chance succeeds
						if (!Main.player[projectile.owner].wet && Main.rand.Next(100) <= biteChance)
						{
							catchID = 0;
							layer = ((!((double)yTileCoord < Main.worldSurface * 0.5)) ? (((double)yTileCoord < Main.worldSurface) ? 1 : ((!((double)yTileCoord < Main.rockLayer)) ? ((yTileCoord >= Main.maxTilesY - 300) ? 4 : 3) : 2)) : 0);
							int commonChance = 150 / fishingLevel;
							int uncommonChance = 150 * 2 / fishingLevel;
							int rareChance = 150 * 7 / fishingLevel;
							int veryRareChance = 150 * 15 / fishingLevel;
							int extremelyRareChance = 150 * 30 / fishingLevel;
							if (commonChance < 2)
							{
								commonChance = 2;
							}
							if (uncommonChance < 3)
							{
								uncommonChance = 3;
							}
							if (rareChance < 4)
							{
								rareChance = 4;
							}
							if (veryRareChance < 5)
							{
								veryRareChance = 5;
							}
							if (extremelyRareChance < 6)
							{
								extremelyRareChance = 6;
							}
							common = false;
							uncommon = false;
							rare = false;
							veryRare = false;
							extremelyRare = false;
							//setting fish rarity for this catch
							if (Main.rand.Next(commonChance) == 0)
							{
								common = true;
							}
							if (Main.rand.Next(uncommonChance) == 0)
							{
								uncommon = true;
							}
							if (Main.rand.Next(rareChance) == 0)
							{
								rare = true;
							}
							if (Main.rand.Next(veryRareChance) == 0)
							{
								veryRare = true;
							}
							if (Main.rand.Next(extremelyRareChance) == 0)
							{
								extremelyRare = true;
							}
							int crateChance = 10;
							if (Main.player[projectile.owner].cratePotion)
							{
								crateChance += 10;
							}
							anglerFishID = Main.anglerQuestItemNetIDs[Main.anglerQuest];
							if (Main.player[projectile.owner].HasItem(anglerFishID))
							{
								anglerFishID = -1;
							}
							if (Main.anglerQuestFinished)
							{
								anglerFishID = -1;
							}
							junk = false;
							//lavafishing
							if (lavaFishing)
							{
								//check to see if the pole the player has equipted can fish in lava
								if (ItemID.Sets.CanFishInLava[Main.player[projectile.owner].HeldItem.type])
								{
									if (extremelyRare)
									{
										//obsidian sword fish
										catchID = 2331;
									}
									else if (veryRare)
									{
										//flarefin koi
										catchID = 2312;
									}
									else if (rare)
									{
										//obsidifish
										catchID = 2315;
									}
									goto lockInCatch;
								}
								return;
							}
							//honeyfishing
							if (honeyFishing)
							{
								if (rare || (uncommon && Main.rand.Next(2) == 0))
								{
									//honeyfin
									catchID = 2314;
								}
								else if (uncommon && anglerFishID == 2451)
								{
									//bumblebee angler fish
									catchID = 2451;
								}
								goto lockInCatch;
							}
							//junk
							if (Main.rand.Next(50) > fishingLevel && Main.rand.Next(50) > fishingLevel && totalWaterTiles < idealWaterTiles)
							{
								catchID = Main.rand.Next(2337, 2340);
								junk = true;
								goto lockInCatch;
							}
							//crates
							if (Main.rand.Next(100) < crateChance)
							{
								catchID = ((!(veryRare | extremelyRare)) ? ((!rare || !Main.player[projectile.owner].ZoneCorrupt) ? ((!rare || !Main.player[projectile.owner].ZoneCrimson) ? ((!rare || !Main.player[projectile.owner].ZoneHoly) ? ((!rare || !Main.player[projectile.owner].ZoneDungeon) ? ((!rare || !Main.player[projectile.owner].ZoneJungle) ? ((!rare || layer != 0) ? ((!uncommon) ? 2334 : 2335) : 3206) : 3208) : 3205) : 3207) : 3204) : 3203) : 2336);
								goto lockInCatch;
							}
							//frogleg
							if (extremelyRare && Main.rand.Next(5) == 0)
							{
								catchID = 2423;
								goto lockInCatch;
							}
							//balloon pufferfish
							if (extremelyRare && Main.rand.Next(5) == 0)
							{
								catchID = 3225;
								goto lockInCatch;
							}
							//zephyr fish
							if (extremelyRare && Main.rand.Next(10) == 0)
							{
								catchID = 2420;
								goto lockInCatch;
							}
							//bombfish
							if (((!extremelyRare && !veryRare) & uncommon) && Main.rand.Next(5) == 0)
							{
								catchID = 3196;
								goto lockInCatch;
							}
							bool inCorruption = Main.player[projectile.owner].ZoneCorrupt;
							bool inCrimson = Main.player[projectile.owner].ZoneCrimson;
							//if player is in both corruption and crimson then randomly choose one of them
							if (inCorruption & inCrimson)
							{
								if (Main.rand.Next(2) == 0)
								{
									inCrimson = false;
								}
								else
								{
									inCorruption = false;
								}
							}
							//corruption
							if (inCorruption)
							{
								if (extremelyRare && Main.hardMode && Main.player[projectile.owner].ZoneSnow && layer == 3 && Main.rand.Next(3) != 0)
								{
									catchID = 2429;
								}
								else if (extremelyRare && Main.hardMode && Main.rand.Next(2) == 0)
								{
									catchID = 3210;
								}
								else if (rare)
								{
									catchID = 2330;
								}
								else if (uncommon && anglerFishID == 2454)
								{
									catchID = 2454;
								}
								else if (uncommon && anglerFishID == 2485)
								{
									catchID = 2485;
								}
								else if (uncommon && anglerFishID == 2457)
								{
									catchID = 2457;
								}
								else if (uncommon)
								{
									catchID = 2318;
								}
							}
							//crimson
							else if (inCrimson)
							{
								if (extremelyRare && Main.hardMode && Main.player[projectile.owner].ZoneSnow && layer == 3 && Main.rand.Next(3) != 0)
								{
									catchID = 2429;
								}
								else if (extremelyRare && Main.hardMode && Main.rand.Next(2) == 0)
								{
									catchID = 3211;
								}
								else if (uncommon && anglerFishID == 2477)
								{
									catchID = 2477;
								}
								else if (uncommon && anglerFishID == 2463)
								{
									catchID = 2463;
								}
								else if (uncommon)
								{
									catchID = 2319;
								}
								else if (common)
								{
									catchID = 2305;
								}
							}
							//hallow
							else if (Main.player[projectile.owner].ZoneHoly)
							{
								if (extremelyRare && Main.hardMode && Main.player[projectile.owner].ZoneSnow && layer == 3 && Main.rand.Next(3) != 0)
								{
									catchID = 2429;
								}
								else if (extremelyRare && Main.hardMode && Main.rand.Next(2) == 0)
								{
									catchID = 3209;
								}
								else if (layer > 1 & veryRare)
								{
									catchID = 2317;
								}
								else if ((layer > 1 & rare) && anglerFishID == 2465)
								{
									catchID = 2465;
								}
								else if ((layer < 2 & rare) && anglerFishID == 2468)
								{
									catchID = 2468;
								}
								else if (rare)
								{
									catchID = 2310;
								}
								else if (uncommon && anglerFishID == 2471)
								{
									catchID = 2471;
								}
								else if (uncommon)
								{
									catchID = 2307;
								}
							}
							//snow
							if (catchID == 0 && Main.player[projectile.owner].ZoneSnow)
							{
								if ((layer < 2 & uncommon) && anglerFishID == 2467)
								{
									catchID = 2467;
								}
								else if ((layer == 1 & uncommon) && anglerFishID == 2470)
								{
									catchID = 2470;
								}
								else if ((layer >= 2 & uncommon) && anglerFishID == 2484)
								{
									catchID = 2484;
								}
								else if ((layer > 1 & uncommon) && anglerFishID == 2466)
								{
									catchID = 2466;
								}
								else
								{
									if (common && Main.rand.Next(12) == 0)
									{
										goto frostDaggerFish;
									}
									if (uncommon && Main.rand.Next(6) == 0)
									{
										goto frostDaggerFish;
									}
									if (uncommon)
									{
										catchID = 2306;
									}
									else if (common)
									{
										catchID = 2299;
									}
									else if (layer > 1 && Main.rand.Next(3) == 0)
									{
										catchID = 2309;
									}
								}
							}
							goto electricBoogaloo;
						}
					}
				}
			}
			return;
		frostDaggerFish:
			catchID = 3197;
			goto electricBoogaloo;
		lockInCatch:
			//hooks for modders to modify fishing result
			PlayerHooks.CatchFish(Main.player[projectile.owner], Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem], fishingLevel, lavaFishing ? 1 : (honeyFishing ? 2 : 0), totalWaterTiles, layer, anglerFishID, ref catchID, ref junk);
			//if there is a result to the catch
			if (catchID > 0)
			{
				//sonar potion effect
				if (Main.player[projectile.owner].sonarPotion)
				{
					Item item = new Item();
					item.SetDefaults(catchID, false);
					item.position = projectile.position;
					ItemText.NewText(item, 1, true, false);
				}
				float floatedFishingLevel = (float)fishingLevel;
				//apparently ai[1] is set to a random negative value, lowered further by the fishing level, probably how long the fish will stay hooked
				projectile.ai[1] = (float)Main.rand.Next(-240, -90) - floatedFishingLevel;
				//localAI[1] stores the catchID
				projectile.localAI[1] = (float)catchID;
				projectile.netUpdate = true;
			}
			return;
		electricBoogaloo:
			//jungle
			if (catchID == 0 && Main.player[projectile.owner].ZoneJungle)
			{
				if ((layer == 1 & uncommon) && anglerFishID == 2452)
				{
					catchID = 2452;
				}
				else if ((layer == 1 & uncommon) && anglerFishID == 2483)
				{
					catchID = 2483;
				}
				else if ((layer == 1 & uncommon) && anglerFishID == 2488)
				{
					catchID = 2488;
				}
				else if ((layer >= 1 & uncommon) && anglerFishID == 2486)
				{
					catchID = 2486;
				}
				else if (layer > 1 & uncommon)
				{
					catchID = 2311;
				}
				else if (uncommon)
				{
					catchID = 2313;
				}
				else if (common)
				{
					catchID = 2302;
				}
			}
			//mushies
			if (((catchID == 0 && Main.shroomTiles > 200) & uncommon) && anglerFishID == 2475)
			{
				catchID = 2475;
			}
			//other
			if (catchID == 0)
			{
				//ocean
				if (layer <= 1 && (xTileCoord < 380 || xTileCoord > Main.maxTilesX - 380) && totalWaterTiles > 1000)
				{
					catchID = ((!veryRare || Main.rand.Next(2) != 0) ? ((!veryRare) ? ((!rare || Main.rand.Next(5) != 0) ? ((!rare || Main.rand.Next(2) != 0) ? ((!uncommon || anglerFishID != 2480) ? ((!uncommon || anglerFishID != 2481) ? ((!uncommon) ? ((!common || Main.rand.Next(2) != 0) ? ((!common) ? 2297 : 2300) : 2301) : 2316) : 2481) : 2480) : 2332) : 2438) : 2342) : 2341);
				}
				//might be an unused desert biome case??
				else
				{
					int sandTile = Main.sandTiles;
				}
			}
			//all surface/sky islands/underground/caverns fishing
			if (catchID == 0)
			{
				catchID = ((!(layer < 2 & uncommon) || anglerFishID != 2461) ? ((!(layer == 0 & uncommon) || anglerFishID != 2453) ? ((!(layer == 0 & uncommon) || anglerFishID != 2473) ? ((!(layer == 0 & uncommon) || anglerFishID != 2476) ? ((!(layer < 2 & uncommon) || anglerFishID != 2458) ? ((!(layer < 2 & uncommon) || anglerFishID != 2459) ? ((!(layer == 0 & uncommon)) ? ((!((layer > 0 && layer < 3) & uncommon) || anglerFishID != 2455) ? ((!(layer == 1 & uncommon) || anglerFishID != 2479) ? ((!(layer == 1 & uncommon) || anglerFishID != 2456) ? ((!(layer == 1 & uncommon) || anglerFishID != 2474) ? ((!(layer > 1 & rare) || Main.rand.Next(5) != 0) ? ((!(layer > 1 & extremelyRare)) ? ((!(layer > 1 & veryRare) || Main.rand.Next(2) != 0) ? ((!(layer > 1 & rare)) ? ((!(layer > 1 & uncommon) || anglerFishID != 2478) ? ((!(layer > 1 & uncommon) || anglerFishID != 2450) ? ((!(layer > 1 & uncommon) || anglerFishID != 2464) ? ((!(layer > 1 & uncommon) || anglerFishID != 2469) ? ((!(layer > 2 & uncommon) || anglerFishID != 2462) ? ((!(layer > 2 & uncommon) || anglerFishID != 2482) ? ((!(layer > 2 & uncommon) || anglerFishID != 2472) ? ((!(layer > 2 & uncommon) || anglerFishID != 2460) ? ((!(layer > 1 & uncommon) || Main.rand.Next(4) == 0) ? ((layer <= 1 || (!(uncommon | common) && Main.rand.Next(4) != 0)) ? ((!uncommon || anglerFishID != 2487) ? ((!(totalWaterTiles > 1000 & common)) ? 2290 : 2298) : 2487) : ((Main.rand.Next(4) != 0) ? 2309 : 2303)) : 2303) : 2460) : 2472) : 2482) : 2462) : 2469) : 2464) : 2450) : 2478) : 2321) : 2320) : 2308) : ((!Main.hardMode || Main.rand.Next(2) != 0) ? 2436 : 2437)) : 2474) : 2456) : 2479) : 2455) : 2304) : 2459) : 2458) : 2476) : 2473) : 2453) : 2461);
			}
			goto lockInCatch;
		}
		*/

		public override bool PreDrawExtras(SpriteBatch spriteBatch) //this draws the fishing line
		{
			Terraria.Player player = Main.player[projectile.owner];

			if (projectile.bobber &&
				Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].modItem is BaseFishingPole fishingPole && 
				(Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].holdStyle > 0 ||
				Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].modItem is BaseSpearFishingPole))
			{ 
				float pPosX = player.MountedCenter.X;
				float pPosY = player.MountedCenter.Y;
				pPosY += Main.player[projectile.owner].gfxOffY;
				float gravDir = Main.player[projectile.owner].gravDir;


				pPosX += (float)(fishingPole.LineOffsetX * Main.player[projectile.owner].direction);
				if (Main.player[projectile.owner].direction < 0)
				{
					pPosX -= 13f; //i think this is supposed to be related somehow to player.width???
				}
				pPosY -= fishingPole.LineOffsestY * gravDir;


				if (gravDir == -1f)
				{
					pPosY -= 12f;
				}

				Vector2 value = new Vector2(pPosX, pPosY);
				value = Main.player[projectile.owner].RotatedRelativePoint(value + new Vector2(8f), true) - new Vector2(8f);
				float projPosX = projectile.position.X + (float)projectile.width * 0.5f - value.X;
				float projPosY = projectile.position.Y + (float)projectile.height * 0.5f - value.Y;


				bool flag2 = true;
				if (projPosX == 0f && projPosY == 0f)
				{
					flag2 = false;
				}

				else
				{
					float projPosXY = (float)Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
					projPosXY = 12f / projPosXY;
					projPosX *= projPosXY;
					projPosY *= projPosXY;
					value.X -= projPosX;
					value.Y -= projPosY;
					projPosX = projectile.position.X + (float)projectile.width * 0.5f - value.X;
					projPosY = projectile.position.Y + (float)projectile.height * 0.5f - value.Y;
				}
				while (flag2)
				{
					float num = 12f;
					float num2 = (float)Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
					float num3 = num2;
					if (float.IsNaN(num2) || float.IsNaN(num3))
					{
						flag2 = false;
					}
					else
					{
						if (num2 < 20f)
						{
							num = num2 - 8f;
							flag2 = false;
						}
						num2 = 12f / num2;
						projPosX *= num2;
						projPosY *= num2;
						value.X += projPosX;
						value.Y += projPosY;

						/* info to make the line attach in a cool way for spears
						
						new Microsoft.Xna.Framework.Color(105, 31, 55, 100) //color for the crimson fishing spear line (could be lighter??)

						projectile.rotation %= (float)(Math.PI * 2); //<--see if its ok to take this out, it should be, but i want to watch anime instead of more of this

						//lower curvature works for spears aswell

						double s2 = Math.Cos(projectile.rotation + MathHelper.ToRadians(-45f));
						double s3 = -44 * s2;

						double y2 = Math.Sin(projectile.rotation + MathHelper.ToRadians(-45f));
						double y3 = -46 * y2;

						float adjustedX = projectile.position.X + (float)(s3);
						float adjustedY = projectile.position.Y + (float)(y3);

						
						projPosX = adjustedX + (float)projectile.width * 0.0f - value.X;
						projPosY = adjustedY + (float)projectile.height * 0.1f - value.Y;

						*/

						projPosX = projectile.position.X + (float)projectile.width * 0.0f - value.X;
						projPosY = projectile.position.Y + (float)projectile.height * 0.1f - value.Y;


						//curvature
						if (num3 > 12f)
						{
							float curvature = 0.3f;
							float movementCurvature = Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y);
							if (movementCurvature > 16f)
							{
								movementCurvature = 16f;
							}
							movementCurvature = 1f - movementCurvature / 16f;
							curvature *= movementCurvature;
							movementCurvature = num3 / 80f;
							if (movementCurvature > 1f)
							{
								movementCurvature = 1f;
							}
							curvature *= movementCurvature;
							if (curvature < 0f)
							{
								curvature = 0f;
							}
							movementCurvature = 1f - projectile.localAI[0] / 100f;
							curvature *= movementCurvature;
							if (projPosY > 0f)
							{
								projPosY *= 1f + curvature;
								projPosX *= 1f - curvature;
							}
							else
							{
								movementCurvature = Math.Abs(projectile.velocity.X) / 3f;
								if (movementCurvature > 1f)
								{
									movementCurvature = 1f;
								}
								movementCurvature -= 0.5f;
								curvature *= movementCurvature;
								if (curvature > 0f)
								{
									curvature *= 2f;
								}
								projPosY *= 1f + curvature;
								projPosX *= 1f - curvature;
							}

						}

						float rotation2 = (float)Math.Atan2((double)projPosY, (double)projPosX) - 1.57f;
						Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)(value.X / 16), (int)(value.Y / 16), new Microsoft.Xna.Framework.Color(255, 249, 183, 100));

						if(fishingPole is BaseDuneFishingPole)
						{
							//if the segment of line being drawn starts inside a block then the line is colored black, could instead break the while loop and stop drawing the line
							//in edge cases (the line is coming out of a block into a non solid tile) then the line will appear black for a short segment, 
							//unless I get a decent bit of bug reports about it this is good enough for me at the moment
							//(if it does get to that point though I can just check if any of R/G/B are below 80 or something and if so they are set to 0)
							Tile tile = Main.tile[(int)((value.X + 6f) / 16), (int)((value.Y - 8f) / 16)];
							if (tile.active() && Main.tileSolid[tile.type]) 
							{
								color2.R = 0;
								color2.G = 0;
								color2.B = 0;
							}
						}

						Main.spriteBatch.Draw(Main.fishingLineTexture,
							new Vector2(value.X - Main.screenPosition.X + (float)Main.fishingLineTexture.Width * 0.5f, value.Y - Main.screenPosition.Y + (float)Main.fishingLineTexture.Height * 0.5f),
							new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.fishingLineTexture.Width, (int)num)),
							color2,
							rotation2,
							new Vector2((float)Main.fishingLineTexture.Width * 0.5f, 0f), 
							1f,
							SpriteEffects.None, 
							0f);
					}
				}
			}
			return false;
		}
	}
}
