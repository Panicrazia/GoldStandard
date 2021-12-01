using GoldStandard.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace GoldStandard.Projectiles
{
    abstract class BaseSpearBobber : BaseBobber
    {
		//should have an abstract thing to make the textures pull automatically fromthe actual fishing spears or whatever
		public bool tileStuck = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Base Spear Bobber (change this)");
		}

        public override void SetDefaults()
        {
            base.SetDefaults();
			drawOffsetX = -32;
			drawOriginOffsetX = 10;
			drawOriginOffsetY = -10;
		}

        public override bool WetMovement()
		{
			if (!tileStuck)
			{
				projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(45f);

				//simulate friction and gravity
				projectile.velocity.X = projectile.velocity.X * 0.995f;

				if (projectile.velocity.Y < 0)
				{
					projectile.velocity.Y *= 0.995f;
					projectile.velocity.Y = projectile.velocity.Y + 0.05f;
				}
				else
				{
					projectile.velocity.Y = projectile.velocity.Y + 0.25f;
				}
				//gravity cap
				if (projectile.velocity.Y > 11.9f)
				{
					projectile.velocity.Y = 11.9f;
				}

				/*
				if(conditions met)
				{
					return true;
				}
				*/
			}

			return false;
		}

		public override bool DryMovement()
		{
			//projectile.rotation += .04f;
			if(!tileStuck) 
			{
				projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(45f);

				//simulate friction and gravity
				if (projectile.velocity.Y < 0)
				{
					projectile.velocity.X *= 0.98f;
				}
				projectile.velocity.X = projectile.velocity.X * 0.997f;
				projectile.velocity.Y = projectile.velocity.Y + 0.2f;


				//projectile.velocity.X = 0;
				//projectile.velocity.Y = 0;


				//gravity cap
				if (projectile.velocity.Y > 15.9f)
				{
					projectile.velocity.Y = 15.9f;
				}
			}

			return false;
		}

		public override float GetReturnRotation()
		{
			Vector2 vector2 = new Vector2(projectile.position.X + (projectile.width * 0.5f), projectile.position.Y + (projectile.height * 0.5f));
			float num32 = Main.player[projectile.owner].position.X + (Main.player[projectile.owner].width / 2) - vector2.X;
			float num31 = Main.player[projectile.owner].position.Y + (Main.player[projectile.owner].height / 2) - vector2.Y;
			return (float)Math.Atan2(num31, num32) + MathHelper.ToRadians(225f);
		}

		public override void Bobb()
		{
			projectile.ai[0] = 1f;
		}

		public override FishingType GetFishingType()
		{
			return FishingType.Spear;
		}

		public override void UnhookTimerTick()
		{
			
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			//projectile.ai[0] = 1f;
			if (!tileStuck)
			{
				//this will make it look more sunken into the ground but may fuck it up against slopes
				//projectile.position += projectile.velocity;

				tileStuck = true;
				projectile.velocity.X = 0f;
				projectile.velocity.Y = 0f;
			}
			return false;
		}
	}
}
