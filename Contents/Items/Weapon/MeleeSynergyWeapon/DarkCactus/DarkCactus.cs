﻿using BossRush.Common.RoguelikeChange.ItemOverhaul;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.DarkCactus {
	internal class DarkCactus : SynergyModItem {
		public override void Synergy_SetStaticDefaults() {
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.BatScepter);
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.BladeofGrass);
		}
		public override void SetDefaults() {
			Item.BossRushSetDefault(58, 78, 29, 5f, 60, 20, ItemUseStyleID.Swing, true);
			Item.DamageType = DamageClass.Melee;

			Item.shoot = ModContent.ProjectileType<CactusBall>();
			Item.shootSpeed = 15;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.buyPrice(gold: 50);

			Item.UseSound = SoundID.Item1;

			if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem))
				meleeItem.SwingType = BossRushUseStyle.GenericSwingDownImprove;
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			if (SynergyBonus_System.Check_SynergyBonus(Type,ItemID.BatScepter)) {
				tooltips.Add(new TooltipLine(Mod, "DarkCactus_BatScepter", $"[i:{ItemID.BatScepter}] Bat now spawn on each swing, rolling cactus also spawn bat"));
			}
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.BladeofGrass)) {
				tooltips.Add(new TooltipLine(Mod, "DarkCactus_BladeOfGrass", $"[i:{ItemID.BladeofGrass}] Increase weapon size by 150% and shoot out leaf blade"));
			}
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.BatScepter)) {
				Vector2 UnitXvelocity = Vector2.UnitX * player.direction;
				for (int i = 0; i < 4; i++) {
					Projectile.NewProjectile(source, position, UnitXvelocity.Vector2DistributeEvenly(4, 40, i).Vector2RotateByRandom(10f), ProjectileID.Bat, damage, knockback, player.whoAmI);
				}
			}
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.BladeofGrass)) {
				for (int i = 0; i < 3; i++) {
					Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10f), ProjectileID.BladeOfGrass, damage, knockback, player.whoAmI);
				}
			}
			CanShootItem = true;
		}
		public override void ModifyItemScale(Player player, ref float scale) {
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.BladeofGrass)) {
				scale += 1.5f;
			}
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) {
			for (int i = 0; i < 2; i++) {
				Vector2 getPos2 = new Vector2(40 * player.direction + Main.rand.Next(-50, 50), -700) + player.Center;
				Vector2 aimto2 = new Vector2(player.Center.X + 60 * player.direction, player.Center.Y) - getPos2;
				Vector2 safeAim = aimto2.SafeNormalize(Vector2.Zero) * 10f;
				Projectile.NewProjectile(Item.GetSource_FromThis(), getPos2, safeAim, ProjectileID.Bat, (int)(hit.Damage * 0.75), hit.Knockback, player.whoAmI);
			}
			if (target.lifeMax > 5 && !target.friendly && target.type != NPCID.TargetDummy) {
				int healAmount = Main.rand.Next(1, 7);
				player.Heal(healAmount);
			}
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.CactusSword)
				.AddIngredient(ItemID.BatBat)
				.Register();
		}
	}
	internal class CactusBall : SynergyModProjectile {
		public override void SetDefaults() {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.tileCollide = true;
			Projectile.penetrate = 10;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.damage *= 3;
		}
		public override void SynergyPostAI(Player player, PlayerSynergyItemHandle modplayer) {
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.BatScepter)) {
				if (!Main.rand.NextBool(10)) {
					return;
				}
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, -Projectile.velocity.Vector2RotateByRandom(5f), ProjectileID.Bat, (int)(Projectile.damage * .5f), 0, Projectile.owner);
			}
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] > 10) {
				Projectile.netUpdate = true;
				Projectile.rotation += 0.5f;

				if (Projectile.velocity.Y <= 20) Projectile.velocity.Y += 0.3f;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0) {
				Projectile.Kill();
			}
			else {
				Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
				if (Projectile.velocity.X != oldVelocity.X) {
					Projectile.velocity.X = -oldVelocity.X * 0.85f;
				}
				if (Projectile.velocity.Y != oldVelocity.Y) {
					Projectile.velocity.Y = -oldVelocity.Y * 0.85f;
				}
			}
			return false;
		}
	}
}
