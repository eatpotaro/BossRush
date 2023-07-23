﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles
{
    internal class SandProjectile : ModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.SandBallGun);
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.SandBallGun);
            Projectile.aiStyle = -1;
        }
        public override void AI()
        { 
            if (Projectile.ai[0] >= 50)
            {
                if (Projectile.velocity.Y < 16)
                {
                    Projectile.velocity.Y += .25f;
                }
            }
            Projectile.ai[0]++;
        }
    }
}