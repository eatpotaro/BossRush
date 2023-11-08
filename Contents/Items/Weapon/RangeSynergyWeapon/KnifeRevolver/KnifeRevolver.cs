﻿using BossRush.Common.RoguelikeChange;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.KnifeRevolver {
	internal class KnifeRevolver : SynergyModItem, IRogueLikeRangeGun {
		public float OffSetPosition => 40;

		public float Spread { get; set; }

		public override void SetDefaults() {
			Item.BossRushDefaultRange(84, 24, 21, 3f, 30, 30, ItemUseStyleID.Shoot, ProjectileID.Bullet, 10, false, AmmoID.Bullet);
			Item.crit = 10;
			Item.scale = 0.85f;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.buyPrice(gold: 50);
			Spread = 1;
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-3, 4);
		}
		public override bool AltFunctionUse(Player player) {
			return true;
		}
		public override bool CanConsumeAmmo(Item ammo, Player player) {
			return player.altFunctionUse != 2;
		}

		public override bool? UseItem(Player player) {
			return player.ownedProjectileCounts[ModContent.ProjectileType<KnifeRevolverSpearProjectile>()] < 1;
		}
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (Main.mouseRight) {
				type = ModContent.ProjectileType<KnifeRevolverSpearProjectile>();
				damage *= 2;
				Item.noUseGraphic = true;
			}
			else {
				type = ProjectileID.Bullet;
				Item.noUseGraphic = false;
			}
			player.GetArmorPenetration(DamageClass.Ranged) += 10;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Revolver)
				.AddIngredient(ItemID.Gladius)
				.Register();
		}
	}
}
