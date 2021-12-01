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
	internal class YellowTangNPC : ModFishNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yellow Tang");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			//base.SetDefaults();
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

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.OverworldUnderwaterCritter.Chance * 0.5f;
		}
		
		public override void HitEffect(int hitDirection, double damage)
		{
			base.HitEffect(hitDirection, damage);
			if (npc.life <= 0)
			{
				//gore goes here
				//Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LavaSnailHead"), npc.scale);
				//Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LavaSnailShell"), npc.scale);
			}
		}
	}

	internal class YellowTangItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yellow Tang");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.autoReuse = true;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.maxStack = 999;
			item.consumable = true;
			item.width = 12;
			item.height = 12;
			//item.makeNPC = 360;
			item.noUseGraphic = true;
			//item.bait = 15;
			item.bait = 17;
			//no, i dont know why this line isnt working
			item.makeNPC = (short)NPCType<YellowTangNPC>();
		}
	}
}
