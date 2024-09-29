﻿using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace BossRush.Common.Systems.ArgumentsSystem;

public class FireI : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Red;
	}
	public override void OnHitNPC(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.OnFire, BossRushUtils.ToSecond(Main.rand.Next(7, 10)));
	}
}
public class FireII : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Red;
	}
	public override void ModifyHitNPC(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			modifiers.SourceDamage += .2f;
		}
	}
}
public class FrostBurnI : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Cyan;
	}
	public override void OnHitNPC(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(Main.rand.Next(7, 10)));
	}
}
public class FrostBurnII : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Cyan;
	}
	public override void ModifyHitNPC(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2)) {
			modifiers.SourceDamage += .2f;
		}
	}
}
public class BerserkI : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.OrangeRed;
	}
	public override void ModifyHitNPC(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		float percentage = player.statLife / (float)player.statLifeMax2;
		modifiers.SourceDamage += .5f * percentage;
	}
}

public class True : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Yellow;
	}
	public override void ModifyHitNPC(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int damage = (int)(player.GetWeaponDamage(item) * .1f);
		modifiers.FinalDamage.Flat += damage;
	}
}

public class Terra : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Green;
	}
	public override void OnHitNPC(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		NPC.HitModifiers modifier = new NPC.HitModifiers();
		modifier.FinalDamage.Flat = player.GetWeaponDamage(item) * (hitInfo.Crit ? 2 : 1);
		modifier.FinalDamage *= 0;
		player.StrikeNPCDirect(npc, modifier.ToHitInfo(1, hitInfo.Crit, hitInfo.Knockback, true));
	}
}

public class TitanI : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Blue;
	}
	public override void ModifyHitNPC(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int damage = (int)player.GetWeaponKnockback(item);
		modifiers.SourceDamage.Base += damage;
	}
}

public class TitanII : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Blue;
	}
	public override void ModifyHitNPC(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int knockbackStrength = (int)(player.GetWeaponDamage(item) * .05f);
		modifiers.Knockback += knockbackStrength;
	}
}

public class CriticalI : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Orange;
	}
	public override void OnHitNPC(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		if (hitInfo.Crit) {
			player.Heal(Math.Clamp((int)(player.statLifeMax2 * 0.001f), 1, player.statLifeMax2));
		}
	}
}

public class VampireI : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkRed;
	}
	public override void UpdateHeld(Player player, Item item) {
		player.GetModPlayer<PlayerStatsHandle>().LifeSteal += 0.01f;
	}
}

public class VampireII : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkRed;
	}
	public override void UpdateHeld(Player player, Item item) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, Multiplicative: 1.5f);
	}
}

public class AlchemistI : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.BlueViolet;
	}
	public override void UpdateHeld(Player player, Item item) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.DebuffDamage, 1.06f);
	}
}

public class AlchemistII : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.BlueViolet;
	}
	public override void ModifyHitNPC(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int buffamount = target.buffType.Where(b => b != 0 && b != -1).Count();
		modifiers.FinalDamage += 0.06f * buffamount;
	}
}
