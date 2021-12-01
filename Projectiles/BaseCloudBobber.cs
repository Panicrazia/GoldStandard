using GoldStandard.NPCs;
using System.Linq;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using GoldStandard.Managers;
using System;

namespace GoldStandard.Projectiles
{
    abstract class BaseCloudBobber : BaseBobber
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Base Cloud Bobber (change this)");
		}

		public override bool WetMovement()
		{
			//TODO: add check for lavawet and if it is then turn it into a puff of smoke
			//TODO: see if i can make it wobble back and forth on the X axis when its going upwards

			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

			//simulate water friction
			projectile.velocity.X = projectile.velocity.X * 0.9f;

			//simulate water bouyancy
			if (projectile.velocity.Y > 0f)
			{
				projectile.velocity.Y = projectile.velocity.Y * 0.5f;
			}

			projectile.velocity.Y = projectile.velocity.Y - 0.2f;

			if (projectile.velocity.Y < -16f)
			{
				projectile.velocity.Y = -16f;
			}

			return false;
		}

		public override bool DryMovement()
		{
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

			//if it is not moving vertically
			if (projectile.velocity.Y == 0f)
			{
				//then slow it horizontally
				projectile.velocity.X = projectile.velocity.X * 0.95f;
			}
			//or if it is not moving horizontally
			else if (projectile.velocity.X == 0f)
			{
				//then slow it vertically
				projectile.velocity.Y = projectile.velocity.Y * 0.95f;
			}

			//simulate air friction
			projectile.velocity.X = projectile.velocity.X * 0.98f;
			projectile.velocity.Y = projectile.velocity.Y * 0.98f;

			//gravity cap
			if (projectile.velocity.Y < -10f)
			{
				projectile.velocity.Y = -10f;
			}

			if ((double)projectile.velocity.Y >= -0.01 && (double)projectile.velocity.Y <= 0.01 && (double)projectile.velocity.X >= -0.01 && (double)projectile.velocity.X <= 0.01)
			{
				return true;
			}
			return false;
		}

		public override void Bobb()
		{
			if ((double)projectile.velocity.Y >= -.2 && (double)projectile.velocity.Y <= .2 && (double)projectile.velocity.X >= -.2 && (double)projectile.velocity.X <= .2)
			{
				//in vanilla fishing innately provides a sound to the bobb by the bobber going in and out of the water, otherwise one must be added
				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 35, 1f, 0f); //filler bell sound effect that should be applied if a specific fairy bell accessory is worn
				projectile.velocity.Y = (float)Main.rand.Next(-50, 51) * 0.015f;
				projectile.velocity.X = (float)Main.rand.Next(-50, 51) * 0.015f;
				projectile.wet = false;
				projectile.lavaWet = false;
				projectile.honeyWet = false;
			}
		}

		public override FishingType GetFishingType()
		{
			return FishingType.Cloud;
		}

		public override bool CheckEnvironment()
        {
			foreach (NPC npc in Main.npc.Where(n => n.active && n.type == NPCType<ModCloudBase>() && n.Hitbox.Intersects(projectile.Hitbox)))
			{
				return true;
			}
			return false;
        }

		public override void UnhookTimerTick()
		{
			float number = (float)Main.rand.Next(1, 50) - 45f;
			if (number > 0)
            {
				projectile.ai[1] += number;
			}

			if (projectile.ai[1] >= 0f)
			{
				projectile.ai[1] = 0f;
				projectile.localAI[1] = 0f;
				projectile.netUpdate = true;
			}
		}
	}
}
