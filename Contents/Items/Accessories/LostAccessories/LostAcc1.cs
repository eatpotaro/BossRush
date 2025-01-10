﻿using BossRush.Common.Systems;
using BossRush.Contents.Skill;
using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class LostAcc1 : ModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.Set_LostAccessory(32, 32);
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<LostAcc1_ModPlayer>().LostAcc1 = true;
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.EnergyCap, Base: -100);
		modplayer.AddStatsToPlayer(PlayerStats.EnergyRecharge, 1.11f);
	}
}
public class LostAcc1_ModPlayer : ModPlayer {
	public bool LostAcc1 = false;
	public override void ResetEffects() {
		LostAcc1 = false;
	}
	int delay = 0;
	const int maxdelay = 60;
	public override void UpdateEquips() {
		if (LostAcc1) {
			SkillHandlePlayer modplayer = Player.GetModPlayer<SkillHandlePlayer>();
			if (modplayer.Activate) {
				PlayerStatsHandle statplayer = Player.GetModPlayer<PlayerStatsHandle>();
				statplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.22f);
				statplayer.AddStatsToPlayer(PlayerStats.Defense, Base: 12);
			}
			if (++delay >= 60) {
				delay = 0;
				modplayer.Modify_EnergyAmount(1);
			}
		}
	}
}