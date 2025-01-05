﻿using Terraria;
using Terraria.ModLoader;
using BossRush.Contents.Perks;
using Terraria.ID;
using BossRush.Contents.Projectiles;
using Microsoft.Xna.Framework;

namespace BossRush.Common.Systems.WeaponUpgrade.Content;
public class EnhancedMagicStaff_GlobalItem : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (!UpgradePlayer.Check_Upgrade(Main.CurrentPlayer, WeaponUpgradeID.EnhancedMagicStaff)) {
			return;
		}
		switch (entity.type) {
			case ItemID.AmethystStaff:
				entity.shoot = ModContent.ProjectileType<AmethystMagicalBolt>();
				entity.useTime = 3;
				entity.useAnimation = 15;
				entity.reuseDelay = 30;
				entity.mana = 6;
				entity.shootSpeed = 1;
				break;
			case ItemID.TopazStaff:
				entity.shoot = ModContent.ProjectileType<TopazMagicalBolt>();
				entity.useTime = 3;
				entity.useAnimation = 18;
				entity.reuseDelay = 33;
				entity.mana = 6;
				entity.shootSpeed = 1;
				break;
		}
	}
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (!UpgradePlayer.Check_Upgrade(player, WeaponUpgradeID.EnhancedMagicStaff)) {
			return;
		}
		switch (item.type) {
			case ItemID.AmethystStaff:
				velocity = velocity.Vector2RotateByRandom(10);
				position = position.PositionOFFSET(velocity, 50);
				break;
			case ItemID.TopazStaff:
				velocity = velocity.Vector2RotateByRandom(15) * Main.rand.NextFloat(.75f, 1.25f);
				position = position.PositionOFFSET(velocity, 50);
				break;
		}
	}
}
internal class EnhancedMagicStaff : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		list_category.Add(PerkCategory.WeaponUpgrade);
	}
	public override void OnChoose(Player player) {
		UpgradePlayer.Add_Upgrade(player, WeaponUpgradeID.EnhancedMagicStaff);
		BossRushUtils.Reflesh_GlobalItem(Mod, player);
	}
}
