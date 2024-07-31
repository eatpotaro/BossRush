﻿using BossRush.Common.Systems;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.Trinket;
internal class Trinket7 : BaseTrinket{
	public override void UpdateTrinket(Player player, TrinketPlayer modplayer) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, 1.35f);
	}
}
