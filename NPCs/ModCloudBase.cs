using GoldStandard.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace GoldStandard.NPCs
{
    //TODO: add slight movement based on wind speed (maybe? I dunno if it would end up just being annoying)
	//TODO: make graphic nothing after I find out how to make it so nothing appears on mouseover
    class ModCloudBase : ModNPC
	{
		int size = 500;
        /*
		static int sizeThreshhold1 = 100;
		static int sizeThreshhold2 = 250;
		static int sizeThreshhold3 = 500;
		*/

		//TODO: unfuck this
        readonly List<CloudCircle> circles = new List<CloudCircle>();

		public override void SetDefaults()
		{
			npc.width = 10;
			npc.height = 10;
			npc.aiStyle = -1;
			npc.damage = 0;
			npc.friendly = true;
			npc.dontTakeDamageFromHostiles = true;
			npc.lifeMax = 1000;
			npc.immortal = true; //i think this keeps it from drawing the stuff on hover?? if it doesnt then this isnt needed
			npc.npcSlots = 0;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.knockBackResist = 0; //this might need to be 1 instead? i dunno, dont have a reference atm
		}

		public override bool? CanBeHitByItem(Terraria.Player player, Item item)
		{
			return false;
		}
		
		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return false;
		}

		public override void AI()
		{
			if(circles.Any())
			{
				//yes this should probably be done in DrawEffects
				GenerateCloudDusts();
			}
			else
            {
				GenerateCircles();
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			return false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.spawnTileY <= Main.worldSurface &&
				NPC.CountNPCS(NPCType<ModCloudBase>()) < 2)
            {
				return .1f;
            }
			return 0f;
		}

		public override int SpawnNPC(int tileX, int tileY)
		{
			Tile tile = Main.tile[tileX, tileY];
			int counter = 0;
			while (counter < 50)
			{
				//these checks should probably be done in the spawnChance method but I belive this will have to be done in this method anyways since SpawnChance's parameter isnt a ref
				if (tile.type != 0 && tile.wall != 0 && tile.liquid == 0)
				{
					tile = Main.tile[tileX, tileY - counter - 1];
				}
                else
				{
					return NPC.NewNPC(tileX * 16 + 8, (tileY - counter - 50) * 16, NPCType<ModCloudBase>());
				}
				counter++;
			}
			return NPC.NewNPC(1, 1, 0);
		}

		public void GenerateCircles()
		{
			size = (Main.rand.Next(750) + 250) * (Main.rand.Next(750) + 250);
			//this is the hitbox which could be changed to be smaller than the spawnbox of the dusts so I can make cool dust fly out when a cloud bobber enters/leaves a cloud for the first time
			npc.width = (int)Math.Sqrt(size);
			npc.height = npc.width / 2;
			circles.Add(new CloudCircle());

			/*
			if (size >= sizeThreshhold1)
			{
				circles.Add(new CloudCircle());
				if (size >= sizeThreshhold2)
				{
					circles.Add(new CloudCircle());
					if (size >= sizeThreshhold3)
					{
						circles.Add(new CloudCircle());
					}
				}
			}*/

            switch (circles.Count)
            {
				case 1:
					circles[0].InitCircle((int)Math.Sqrt(size), 0, 0);	//i decided after the fact that the clouds all looked fine as just one oval was involved but i am still keeping this other stuff in the code on the off chance I do use it for some other cloud design or something
					break;
				case 2:
					if (Main.rand.Next(2) == 1)
					{
						circles[0].InitCircle((int)((size * .5) * (1 / .75)), 1, 1);	//bottom left
						circles[1].InitCircle((int)((size * .5) * (1 / .75)), -1, -1);	//top right
					}
					else
					{
						circles[0].InitCircle((int)((size * .5) * (1 / .75)), -1, 1);	//bottom right
						circles[1].InitCircle((int)((size * .5) * (1 / .75)), 1, -1);	//top left
					}
					break;
				case 3:
					circles[0].InitCircle((int)((size * .5) * (1 / .75)), 1, 1);		//bottom left
					circles[1].InitCircle((int)((size * .5) * (1 / .75)), -1, 1);		//bottom right
					circles[2].InitCircle((int)(size*.5), 0, -1);						//top center
					break;
				case 4:
					circles[0].InitCircle((int)((size * .5) * (1 / .75)), 1, 1);		//bottom left
					circles[1].InitCircle((int)((size * .5) * (1 / .75)), -1, 1);		//bottom right
					circles[2].InitCircle((int)(size * .25), 1, -1);					//top left
					circles[3].InitCircle((int)(size * .25), -1, -1);					//top right
					break;
			}
		}

		public void GenerateCloudDusts()
		{
			Microsoft.Xna.Framework.Vector2 offset;
			foreach (CloudCircle cloud in circles)
			{
				//magic numbers go brrr (what this does is translate the size of the cloud into a frequency to spawn the cloud particles that looks good)
				if (Main.rand.Next(20) <= (size * Math.PI) / (196250)) //(for smaller clouds a larger number might provide a better looking cloud due to the dust particles being so large)
				{
					offset = cloud.GetDustVector();
					if (offset != null)
					{
						Dust.NewDustPerfect(npc.Center + offset, DustType<CloudDust1>());
					}
				}
			}
		}

		/*
		public void SpawnDustOnCloud(int x, int y)
        {
			Dust.NewDustPerfect(new Microsoft.Xna.Framework.Vector2(x, y), DustType<CloudDust1>());
		}*/

		private class CloudCircle
		{
			int radius = 0;
			bool bottom = true; //determines if the bottom should get cut off for particles, and which direction the shrink should happen, shrink happens in both directions if x and y offset are 0
			int xOffset = 0;
			int yOffset = 0;

			/*
			public CloudCircle(int radius)
			{
				
			}
			*/

			public void InitCircle(int diameter, int xOffset, int yOffset)
			{
				this.radius = (int)(diameter * .5);

				if (yOffset < 0) //top
				{
					this.yOffset = -(int)(radius / Math.Sqrt(2));
					if (xOffset != 0)
					{
						this.xOffset = (int)(radius / Math.Sqrt(2));
					}
					bottom = false;
				}
				else if (yOffset > 0) //bottom
				{
					this.yOffset = (int)(radius / 2 * Math.Sqrt(3));
					if (xOffset != 0)
					{
						this.xOffset = radius / 2;
					}
				}
				if (xOffset > 0)
				{
					this.xOffset = -this.xOffset;
				}
			}

			public Microsoft.Xna.Framework.Vector2 GetDustVector()
			{
				double angle = Main.rand.NextDouble() * 2 * Math.PI;
				double radi = radius * Math.Sqrt(Main.rand.NextDouble());
				float x = (float)(radi * Math.Cos(angle));
				float y = (float)(radi * Math.Sin(angle));
				//TODO: remake this so it doesnt rely on a magic number and instead can be variable to control how thicc the clouds are
				y /= 2; //shrink the y upwards
				if (bottom)
                {
					//y -= radius / 2;
					//TODO: cut off dust that would spawn in the lower .25 of the cloud (ie return null)
				}
                else
                {
					y += radius / 2; //shrink y downwards
                }
				return new Microsoft.Xna.Framework.Vector2(xOffset + x, yOffset + y);
			}
		}
	}
}
