﻿using BossRush.Common.Systems;
using BossRush.Contents.Items.Weapon;
using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class StimPack : ModItem{
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<StimPackPlayer>().StimPack = true;
	}
}
public class StimPackPlayer : ModPlayer {
	public bool StimPack = false;
	public override void ResetEffects() {
		StimPack = false;
	}
	public override void UpdateEquips() {
		if(!Player.ComparePlayerHealthInPercentage(.4f) && StimPack) {
			PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.RegenHP, 1.5f, Flat: 10);
			modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 1.12f);
		}
	}
}
