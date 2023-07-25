﻿using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.NPCs
{
    internal class Servant : ModNPC
    {
        public override string Texture => BossRushTexture.DIAMONDSWOTAFFORB;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.DebuffImmunitySets.Add(NPC.type, new NPCDebuffImmunityData() { ImmuneToAllBuffsThatAreNotWhips = true });
            NPCID.Sets.TrailCacheLength[NPC.type] = 50;
        }
        public override void SetDefaults()
        {
            NPC.lifeMax = 90000;
            NPC.damage = 400;
            NPC.defense = 50;
            NPC.friendly = false;
            NPC.width = NPC.height = 30;
            NPC.lavaImmune = true;
            NPC.trapImmune = true;
            NPC.knockBackResist = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.scale = 2f;
            NPC.color = Color.Black;
        }
        public override void AI()
        {
            for (int i = 0; i < 4; i++)
            {
                int dust = Dust.NewDust(NPC.Center + Main.rand.NextVector2Circular(50, 50), 0, 0, DustID.Granite, 0, 0, 0, Color.Black, Main.rand.NextFloat(1.5f, 3f));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].color = Color.Black;
            }
            Player player = Main.player[NPC.target];
            NPC.velocity = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 2;
            if (!player.active || player.dead)
            {
                return;
            }
            if (NPC.ai[1] >= 100)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity, ModContent.ProjectileType<ServantProjectile>(), NPC.damage, 40, -1, NPC.target);
                NPC.ai[1] = 0;
            }
            else
            {
                NPC.ai[1]++;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return true;
        }
    }
    public class ServantProjectile : ModProjectile
    {
        public override string Texture => BossRushTexture.DIAMONDSWOTAFFORB;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.penetrate = 1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            Projectile.velocity += Projectile.velocity * .01f;
            if (Projectile.ai[0] >= 0)
            {
                Projectile.velocity += (Main.player[(int)Projectile.ai[0]].Center - Projectile.Center).SafeNormalize(Vector2.Zero) * .1f;
            }
            for (int i = 0; i < 4; i++)
            {
                int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(30, 30), 0, 0, DustID.Granite);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].color = Color.Black;
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 150; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Granite, 0, 0, 0, Color.Black, Main.rand.NextFloat(.5f, 2f));
                Vector2 vel = Main.rand.NextVector2Circular(5, 5);
                Main.dust[dust].velocity = vel;
                Main.dust[dust].color = Color.Black;
            }
            base.Kill(timeLeft);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Black;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(Color.Black, .02f);
            return true;
        }
    }
}
