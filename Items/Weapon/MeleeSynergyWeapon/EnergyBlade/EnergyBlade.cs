﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon.EnergyBlade
{
    internal class EnergyBlade : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("High energy vibration around the sword" +
                "\nmaking it sharp enough to even slash through the strongest mental like it is nothing");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 8));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.height = 62;
            Item.width = 64;

            Item.damage = 25;
            Item.knockBack = 5f;
            Item.useTime = 30;
            Item.useAnimation = 15;

            Item.shoot = ModContent.ProjectileType<EnergyBladeProjectile>();
            Item.shootSpeed = 0;

            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Orange;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 50);
            Item.useTurn = false;

            Item.UseSound = SoundID.Item1;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<EnergyBladeProjectile>()] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<EnergyBladeProjectile>()] < 1;
        }
    }
    public class EnergyBladeProjectile : ModProjectile
    {
        public override string Texture => "BossRush/Items/Weapon/MeleeSynergyWeapon/EnergyBlade/EnergyBlade";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 62;
            Projectile.penetrate = -1;
            Projectile.wet = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 0;
        }
        public override void AI()
        {
            frameCounter();
            Player player = Main.player[Projectile.owner];
            if (Projectile.timeLeft > player.itemAnimationMax)
            {
                Projectile.timeLeft = player.itemAnimationMax;
            }
            player.heldProj = Projectile.whoAmI;
            float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
            percentDone = BossRushUtils.InExpo(percentDone);
            Projectile.spriteDirection = player.direction;
            float baseAngle = player.GetModPlayer<MeleeOverhaulPlayer>().data.ToRotation();
            float angle = MathHelper.ToRadians(baseAngle + 90) * player.direction;
            float start = baseAngle + angle;
            float end = baseAngle - angle;
            float currentAngle = MathHelper.SmoothStep(start, end, percentDone);
            Projectile.rotation = currentAngle;
            Projectile.rotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
            Projectile.Center = player.MountedCenter + Vector2.UnitX.RotatedBy(currentAngle) * 42;
            player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);
        }
        public void frameCounter()
        {
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                Projectile.frame += 1;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }
    }
}