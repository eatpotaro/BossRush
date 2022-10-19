﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush
{
    public abstract class WeaponDataStoreValue : ModPlayer
    {
        private float numOfProjectile = 1;
        private Vector2 vec2ToRotate;
        private bool rotateByRandom;
        public float SpreadModify { get => SpreadModify1; set => SpreadModify1 = value; }
        public float NumOfProjectile { get => numOfProjectile; set => numOfProjectile = value; }
        public Vector2 Vec2ToRotate { get => vec2ToRotate; set => vec2ToRotate = value; }
        public bool RotateByRandom { get => rotateByRandom; set => rotateByRandom = value; }

        private float spreadModify = 1;
        public float SpreadModify1 { get => spreadModify; set => spreadModify = value; }

    }

    public abstract class WeaponTemplate : ModItem
    {
        protected WeaponDataStoreValue WeaponData;
        public float ModifySpread(float TakeFloat) => WeaponData.SpreadModify <= 0 ? 0 : TakeFloat += WeaponData.SpreadModify;

        public Vector2 RotateRandom(float ToRadians)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            return WeaponData.Vec2ToRotate.RotatedByRandom(rotation);
        }

        public Vector2 RotateCode(float ToRadians, float time = 0)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            if (WeaponData.NumOfProjectile > 1)
            {
                return WeaponData.Vec2ToRotate.RotatedBy(MathHelper.Lerp(rotation / 2f, -rotation / 2f, time / (WeaponData.NumOfProjectile - 1f)));
            }
            return WeaponData.Vec2ToRotate;
        }

        public Vector2 RandomSpread(Vector2 ToRotateAgain, int Spread, float additionalMultiplier = 1)
        {
            ToRotateAgain.X += (Main.rand.Next(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            ToRotateAgain.Y += (Main.rand.Next(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            return ToRotateAgain;
        }
    }

    abstract class GlobalWeaponModify : GlobalItem
    {
        protected WeaponDataStoreValue WeaponData;
        public float ModifiedProjAmount()
        {
            return WeaponData.NumOfProjectile += 5;
        }
        public float ModifySpread(float TakeFloat) => WeaponData.SpreadModify <= 0 ? 0 : TakeFloat += WeaponData.SpreadModify;

        public Vector2 RotateRandom(float ToRadians)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            return WeaponData.Vec2ToRotate.RotatedByRandom(rotation);
        }

        public Vector2 RotateCode(float ToRadians, float time = 0)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            if (WeaponData.NumOfProjectile > 1)
            {
                return WeaponData.Vec2ToRotate.RotatedBy(MathHelper.Lerp(rotation / 2f, -rotation / 2f, time / (WeaponData.NumOfProjectile - 1f)));
            }
            return WeaponData.Vec2ToRotate;
        }

        public Vector2 PositionOFFSET(Vector2 position, Vector2 ProjectileVelocity,float offSetBy)
        {
            Vector2 OFFSET = ProjectileVelocity.SafeNormalize(Vector2.UnitX) * offSetBy;
            if(Collision.CanHitLine(position,0,0,position + OFFSET,0,0))
            {
                return position += OFFSET;
            }
            return position;
        }

        public Vector2 RandomSpread(Vector2 ToRotateAgain, int Spread, float additionalMultiplier = 1)
        {
            ToRotateAgain.X += (Main.rand.Next(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            ToRotateAgain.Y += (Main.rand.Next(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            return ToRotateAgain;
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (item.type == ItemID.RedRyder && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 20);
                velocity = RotateRandom(6);
            }
            if (item.type == ItemID.Minishark && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 10);
                velocity = RotateRandom(10);
            }
            if (item.type == ItemID.Gatligator && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 20);
                velocity = RandomSpread(RotateRandom(35), 10);
            }
            if (item.type == ItemID.Handgun && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 10);
                velocity = RotateRandom(15);
            }
            if (item.type == ItemID.PhoenixBlaster && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 10);
                velocity = RotateRandom(10);
            }
            if (item.type == ItemID.Musket && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 20);
                velocity = RotateRandom(4);
            }
            if (item.type == ItemID.TheUndertaker && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 5);
                velocity = RotateRandom(12);
            }
            if (item.type == ItemID.FlintlockPistol && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 5);
                velocity = RotateRandom(20);
            }
            if (item.type == ItemID.Revolver && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 5);
                velocity = RotateRandom(10);
            }
            if (item.type == ItemID.ClockworkAssaultRifle && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 15);
                velocity = RotateRandom(17);
            }
            if (item.type == ItemID.Megashark && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 25);
                velocity = RotateRandom(6);
            }
            if (item.type == ItemID.Uzi && AppliesToEntity(item, false))
            {
                velocity = RotateRandom(14);
            }
            if (item.type == ItemID.VenusMagnum && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 25);
                velocity = RotateRandom(14);
            }
            if (item.type == ItemID.SniperRifle && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 35);
                velocity = RotateRandom(2);
            }
            if (item.type == ItemID.ChainGun && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 35);
                velocity = RotateRandom(33);
            }
            if (item.type == ItemID.VortexBeater && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 35);
                velocity = RotateRandom(20);
            }
            if (item.type == ItemID.SDMG && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 35);
                velocity = RotateRandom(4);
            }
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.type == ItemID.Boomstick && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 25);
                WeaponData.NumOfProjectile += Main.rand.Next(2, 5);
                for (int i = 0; i < ModifiedProjAmount(); i++)
                {
                    Projectile.NewProjectile(source, position, RandomSpread(RotateRandom(18), 35, 0.04f), type, damage, knockback, player.whoAmI);
                }
                return false;
            }
            if (item.type == ItemID.QuadBarrelShotgun && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 25);
                WeaponData.NumOfProjectile += 5;
                for (int i = 0; i < ModifiedProjAmount(); i++)
                {
                    Projectile.NewProjectile(source, position, RotateRandom(65), type, damage, knockback, player.whoAmI);
                }
                return false;
            }
            if (item.type == ItemID.Shotgun && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 35);
                WeaponData.NumOfProjectile += Main.rand.Next(2, 5);
                for (int i = 0; i < ModifiedProjAmount(); i++)
                {
                    Projectile.NewProjectile(source, position, RandomSpread(RotateRandom(30), 10, 0.5f), type, damage, knockback, player.whoAmI);
                }
                return false;
            }
            if (item.type == ItemID.OnyxBlaster && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 35);
                WeaponData.NumOfProjectile += 3;
                for (int i = 0; i < ModifiedProjAmount(); i++)
                {
                    Projectile.NewProjectile(source, position, RandomSpread(RotateRandom(15), 5, 0.5f), type, damage, knockback, player.whoAmI);
                }
                Projectile.NewProjectile(source, position, velocity, ProjectileID.BlackBolt, damage * 2, knockback, player.whoAmI);
                return false;
            }
            if (item.type == ItemID.TacticalShotgun && AppliesToEntity(item, false))
            {
                PositionOFFSET(position, velocity, 35);
                WeaponData.NumOfProjectile += 5;
                for (int i = 0; i < ModifiedProjAmount(); i++)
                {
                    Projectile.NewProjectile(source, position, RandomSpread(RotateRandom(18), 3, 0.76f), type, damage, knockback, player.whoAmI);
                }
                return false;
            }
            return true;
        }
    }
}