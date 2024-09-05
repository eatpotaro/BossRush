﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using BossRush.Contents.Projectiles;
using BossRush.Contents.Items.BuilderItem;
using BossRush.Contents.Items.Accessories.LostAccessories;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow;

namespace BossRush.Common;
internal class RoguelikeGlobalProjectile : GlobalProjectile {
	public override bool InstancePerEntity => true;

	public int Source_ItemType = -1;
	public string Source_CustomContextInfo = string.Empty;
	public bool Source_FromDeathScatterShot = false;
	public int OnKill_ScatterShot = -1;
	public override void OnSpawn(Projectile projectile, IEntitySource source) {
		if (source is EntitySource_ItemUse parent) {
			Source_ItemType = parent.Item.type;
		}
		if (source is EntitySource_Misc parent2 && parent2.Context == "OnKill_ScatterShot") {
			Source_FromDeathScatterShot = true;
		}
		Source_CustomContextInfo = source.Context;
	}
	public override void OnKill(Projectile projectile, int timeLeft) {
		if (Source_FromDeathScatterShot
			|| OnKill_ScatterShot <= 0
			|| projectile.hostile
			|| !projectile.friendly
			|| projectile.minion
			|| projectile.aiStyle == 4
			|| projectile.aiStyle == 19
			|| projectile.aiStyle == 39
			|| projectile.aiStyle == 46
			|| projectile.aiStyle == 75
			|| projectile.aiStyle == 99
			|| projectile.aiStyle == 101
			|| projectile.minion
			|| projectile.sentry
			|| projectile.type == ProjectileID.PhantasmArrow
			|| projectile.type == ProjectileID.IchorDart
			|| projectile.type == ProjectileID.ExplosiveBunny
			|| projectile.type == ProjectileID.FinalFractal
			|| projectile.type == ProjectileID.PortalGun
			|| projectile.type == ProjectileID.PortalGunBolt
			|| projectile.type == ProjectileID.PortalGunGate
			|| projectile.type == ProjectileID.LightsBane
			|| projectile.type == ModContent.ProjectileType<LeafProjectile>()
			|| projectile.type == ModContent.ProjectileType<MagicBullet>()//This is to prevent lag
			|| projectile.type == ModContent.ProjectileType<DiamondGemP>()
			|| projectile.type == ModContent.ProjectileType<ArenaMakerProj>()
			|| projectile.type == ModContent.ProjectileType<NeoDynamiteExplosion>()
			|| projectile.type == ModContent.ProjectileType<TowerDestructionProjectile>()) {
			return;
		}
		if (!projectile.velocity.IsLimitReached(1)) {
			projectile.velocity *= Main.rand.NextFloat(5, 7);
		}
		for (int i = 0; i < OnKill_ScatterShot; i++) {
			Projectile.NewProjectile(projectile.GetSource_Misc("OnKill_ScatterShot"), projectile.Center, projectile.velocity.Vector2RotateByRandom(360), projectile.type, (int)(projectile.damage * .65f), projectile.knockBack * .55f, projectile.owner);
		}
	}
}