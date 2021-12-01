using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using GoldStandard.Managers;

namespace GoldStandard.Projectiles
{
    abstract class BaseCageBobber : BaseBobber
	{
		public bool open = true;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Base Cage Bobber (change this)");
		}

		public override bool WetMovement()
		{
			projectile.rotation = 0;
			if (projectile.velocity.Y == 0f)
			{
				projectile.velocity.X = projectile.velocity.X * 0.6f;
			}
			//simulate water friction and gravity
			projectile.velocity.X = projectile.velocity.X * 0.97f;
			projectile.velocity.Y = projectile.velocity.Y + 0.1f;
			if (projectile.velocity.Y > 11.9f)
			{
				projectile.velocity.Y = 11.9f;
			}

			Vector2 projectileVector = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
			float distX = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - projectileVector.X;
			float distY = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - projectileVector.Y;
			float dist = (float)Math.Sqrt((double)(distX * distX + distY * distY));

			if (dist > 200f)
            {
				float torque = .4f;
				projectile.velocity.X += torque * (distX / dist);
				projectile.velocity.Y += torque * (distY / dist);
			}
			
			if(distY > -180 && projectile.velocity.Y < 0f)
			{
				projectile.velocity.Y = projectile.velocity.Y * 0.97f;
			}

			int xBobberCenterCoordinate = (int)(projectile.position.X / 16f);
			int yBobberCenterCoordinate = (int)(projectile.position.Y / 16f);

			if ((Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate + 1] != null && Main.tile[xBobberCenterCoordinate, yBobberCenterCoordinate + 1].collisionType == 1) && (projectile.velocity.X >= 1f || projectile.velocity.X <= -1f))
			{
				return true;
			}

			return false;
		}

		public override bool DryMovement()
		{
			projectile.rotation = 0;
			if (projectile.velocity.Y == 0f)
			{
				//then slow it horizontally
				projectile.velocity.X = projectile.velocity.X * 0.8f;
			}
			//simulate air friction and gravity
			projectile.velocity.X = projectile.velocity.X * 0.92f;
			projectile.velocity.Y = projectile.velocity.Y + 0.4f;
			//gravity cap
			if (projectile.velocity.Y > 15.9f)
			{
				projectile.velocity.Y = 15.9f;
			}

			Vector2 projectileVector = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
			float distX = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - projectileVector.X;
			float distY = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - projectileVector.Y;
			float dist = (float)Math.Sqrt((double)(distX * distX + distY * distY));

			if (dist > 200f)
			{
				float torque = .4f;
				projectile.velocity.X += torque * (distX / dist) * 2;
                if (projectile.velocity.Y > 0)
                {
					projectile.velocity.Y *= .94f;

				}
				projectile.velocity.Y += torque * (distY / dist);
			}

			if (distY > -180 && projectile.velocity.Y < 0f)
			{
				projectile.velocity.Y = projectile.velocity.Y * 0.96f;
			}

			return false;
		}

		public override void Bobb()
		{
            if (open)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 37, 1f, 0f);
				open = false;
            }
		}

        public override FishingType GetFishingType()
        {
            return FishingType.Cage;
        }

        public override bool CheckEnvironment()
		{
			return projectile.wet;
		}

		public override void UnhookTimerTick()
		{

		}
	}
}
