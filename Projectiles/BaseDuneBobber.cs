using GoldStandard.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace GoldStandard.Projectiles
{
    abstract class BaseDuneBobber : BaseBobber
    {
		//TODO: add some custom post draw logic or shader logic i dunno which to make a rumble on the screen or something where the bobber currently is, 
		//what im thinking is like how the stardust codex looks before you pick it up as reference
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Base Dune Bobber (change this)");
			ProjectileID.Sets.DontAttachHideToAlpha[projectile.type] = true;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.tileCollide = false;
			projectile.hide = true;
		}

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
			//drawCacheProjsBehindNPCs.Add(index);
			drawCacheProjsBehindNPCsAndTiles.Add(index);
		}

        public override bool WetMovement()
		{
			//just floats on the water sideways so its clear that this doesnt work in water

			//set rotation to normal
			projectile.rotation = MathHelper.ToRadians(225f);

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

			return false;
		}

		public override bool DryMovement()
		{
			//if in sand then it keeps going down until its 4 or so blocks above a non sand block (or if its very deep in the sand like 20 blocks or something, can just check if the 19th block above is sand and if so do that stuff)
			



			int posX = (int)(projectile.position.X/16);
			int posY = (int)(projectile.position.Y/16);

			switch(IsTileEligible(posX, posY))
            {
				case -1://air

					Vector2 vector2 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num32 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector2.X;
					float num31 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector2.Y;
					projectile.rotation = (float)Math.Atan2((double)num31, (double)num32) + 1.57f;

					/*
                    if (JankCollisionCheck())
					{
						projectile.velocity.X = 0;
						projectile.velocity.Y = 0;
					}
					*/

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

					break;
				case 0: //solid
					projectile.rotation += (float)Math.PI;
					projectile.velocity.X = 0;
					projectile.velocity.Y = 0;

					break;
				default://sand

					//simulate friction
					projectile.velocity.X *= 0.97f;

					bool isNearBottom = false; 
					for(int i = 0; i <= 5; i++)
					{
						if(IsTileEligible(posX, posY + i) == 0)
                        {
							isNearBottom = true;
                        }
					}
					bool isNearTop = IsTileEligible(posX, posY - 15) <= 0;

					if(isNearBottom || !isNearTop)
                    {
						if(projectile.velocity.Y > 0)
                        {
							projectile.velocity.Y *= 0.75f;
						}
						projectile.velocity.Y = projectile.velocity.Y - 0.17f;

						return true;
					}
                    else
                    {
						projectile.velocity.Y = projectile.velocity.Y + 0.2f;
					}
					JankCollisionCheck();

					break;
            }

			return false;
		}
		public override void ReelInMovement()
		{
			if (projectile.ai[1] < 0f && projectile.localAI[1] != 0f)
			{
				//TODO: add sand particles that go up and a big sandy fishing sound because that would look dope as hell
				projectile.velocity.Y = -42;
			}
		}

		public override void Bobb()
		{
			if (projectile.velocity.X >= -4 && projectile.velocity.X <= 4)
			{
				projectile.velocity.X = (Main.rand.Next(0, 3) - 1) * Main.rand.Next(60, 100) * 0.09f;
			}
		}

		public override FishingType GetFishingType()
		{
			return FishingType.Dune;
		}

		/**
		 * <returns>-1 if the tile is passable (ie air or non solid blocks), 0 if the tile is a non eligible solid block, the tileID if it is an eligible block</returns>
		 */
		private int IsTileEligible(int x, int y)
		{
			Tile tile = Main.tile[x, y];
            if (tile.active() && Main.tileSolid[tile.type])
            {
				if (Main.tileSand[tile.type]) //returns gravity affected blocks
				{
					return tile.type;
				}
				return 0;
			}
			return -1;
			//gravity affected blocks:
			//53  sand
			//112 corrupted sand
			//116 hallowed sand
			//234 crimson sand
			//123 silt
			//224 slush
			//495 seashells (1.4)
		}

		private void JankCollisionCheck()
        {
			//this should instead be a check for if moving in X will stick you then bounce X and if moving in Y will stick you then bounce Y, and only make it bounce in sand
			int futurePosX = (int)((projectile.position.X + projectile.velocity.X) / 16);
			int futurePosY = (int)((projectile.position.Y + projectile.velocity.Y) / 16);
            if (IsTileEligible(futurePosX, (int)(projectile.position.Y / 16)) == 0)
			{
				projectile.velocity.X *= -3f;
				projectile.localAI[1] = 0f;
				projectile.ai[1] = 0f;
				projectile.netUpdate = true;
			}
			if (IsTileEligible((int)(projectile.position.X / 16), futurePosY) == 0)
			{
				projectile.velocity.X *= -.25f;
			}
			//return true if bounced on X? and then if it bounced on X the fish unhooks itself?? would have to be explained in game that thats how it works but seems like a good incentive to fish in big sandy areas
		}
	}
}
