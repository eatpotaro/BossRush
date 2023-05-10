﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossRush.Contents.Items;
using System.Collections.Generic;
using BossRush.Contents.Items.Artifact;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.BuffAndDebuff;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using BossRush.Common.Utils;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BossRush.Common.Global
{
    internal class ArtifactSystem : ModSystem
    {
        public override void AddRecipes()
        {
            ArtifactRecipe();
        }
        private static void ArtifactRecipe()
        {
            foreach (var itemSample in ContentSamples.ItemsByType)
            {
                ModItem item = itemSample.Value.ModItem;
                if (item is IArtifactItem)
                {
                    if (item is SkillIssuedArtifact)
                    {
                        item.CreateRecipe()
                        .AddIngredient(ModContent.ItemType<BrokenArtifact>())
                        .AddIngredient(ModContent.ItemType<PowerEnergy>())
                        .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                        .AddIngredient(ModContent.ItemType<WoodenTreasureChest>())
                        .Register();
                        continue;
                    }
                    item.CreateRecipe()
                        .AddIngredient(ModContent.ItemType<BrokenArtifact>())
                        .Register();
                }
            }
        }
    }
    class ArtifactGlobalItem : GlobalItem
    {
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.ModItem is IArtifactItem)
            {
                if (item.consumable)
                {
                    return player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID < 1;
                }
            }
            return base.CanUseItem(item, player);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.ModItem is IArtifactItem)
            {
                if (item.consumable)
                {
                    tooltips.Add(new TooltipLine(Mod, "ArtifactCursed", "Only 1 artifact can be consume"));
                }
                if (Main.LocalPlayer.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID != 0)
                {
                    TooltipLine line = new TooltipLine(Mod, "ArtifactAlreadyConsumed", "You can't no longer consume anymore artifact");
                    line.OverrideColor = Color.DarkRed;
                    tooltips.Add(line);
                }
            }
        }
    }
    class ArtifactPlayerHandleLogic : ModPlayer
    {
        public int ArtifactDefinedID = 0;
        bool Greed = false;//ID = 1
        bool Pride = false;//ID = 2
        bool Vampire = false;//ID = 3
        bool Earth = false;
        bool FateDice = false;
        int EarthCD = 0;
        ChestLootDropPlayer chestmodplayer => Player.GetModPlayer<ChestLootDropPlayer>();
        public override void PreUpdate()
        {
            Greed = false;
            Pride = false;
            Vampire = false;
            Earth = false;
            FateDice = false;
            switch (ArtifactDefinedID)
            {
                case 1:
                    Greed = true;
                    break;
                case 2:
                    Pride = true;
                    break;
                case 3:
                    Vampire = true;
                    break;
                case 4:
                    Earth = true;
                    break;
                case 5:
                    FateDice = true;
                    break;
            }
        }
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            base.ModifyMaxStats(out health, out mana);
            if (Vampire)
            {
                health.Base = -(Player.statLifeMax * 0.65f);
            }
            if (Earth)
            {
                health.Base = 100 + Player.statLifeMax * 2;
            }
        }
        public override void PostUpdate()
        {
            if (Pride)
            {
                chestmodplayer.multiplier = true;
                chestmodplayer.amountModifier = .5f;
            }
            if (Greed)
            {
                chestmodplayer.amountModifier += 4;
            }
            if (FateDice)
            {
                chestmodplayer.amountModifier += 2;
            }
            if (Earth)
            {
                bool isOnCoolDown = EarthCD > 0;
                EarthCD -= isOnCoolDown ? 1 : 0;
                if (isOnCoolDown)
                {
                    int dust = Dust.NewDust(Player.Center, 0, 0, DustID.Blood);
                    Main.dust[dust].velocity = -Vector2.UnitY * 2f + Main.rand.NextVector2Circular(2, 2);
                }
            }
            FateDeciderEffect();
        }
        int RNGdecide = -1;
        int effectlasting = 0;

        bool CanCrit = false;
        bool CanBeHit = true;
        bool NPCdeal5TimeDamage = false;
        bool UnableToUseWeapon = false;
        bool ExtendingEffect = false;
        int OPEFFECT = 0;
        private void FateDeciderEffect()
        {
            effectlasting -= effectlasting > 0 ? 1 : 0;
            if (effectlasting <= 0)
            {
                RNGdecide = -1;
                CanCrit = false;
                CanBeHit = true;
                NPCdeal5TimeDamage = false;
                UnableToUseWeapon = false;
                ExtendingEffect = false;
                OPEFFECT = 0;
            }
            if (!FateDice)
            {
                return;
            }
            if (effectlasting <= 0 && Main.rand.NextBool(1000))
            {
                RNGdecide = Main.rand.Next(17);
            }
            SpamTextToScare();
            switch (RNGdecide)
            {
                case 0:
                    Player.velocity = Vector2.Zero;
                    Player.AddBuff(BuffID.Frozen, 60);
                    effectlasting = effectlasting > 0 ? effectlasting : 5;
                    break;
                case 1:
                    Player.AddBuff(BuffID.Weak, 300);
                    Player.AddBuff(BuffID.Slow, 300);
                    Player.AddBuff(BuffID.BrokenArmor, 300);
                    Player.AddBuff(BuffID.ManaSickness, 300);
                    Player.AddBuff(BuffID.Obstructed, 300);
                    effectlasting = effectlasting > 0 ? effectlasting : 30;
                    break;
                case 2:
                    if (effectlasting % 50 == 0)
                    {
                        BossRushUtils.SpawnBoulderOnTopPlayer(Player, 0, false);
                    }
                    if (effectlasting % 20 == 0)
                    {
                        BossRushUtils.SpawnHostileProjectileDirectlyOnPlayer(Player, 1000, 50, true, Vector2.Zero, ProjectileID.HellfireArrow, 100, 10f);
                    }
                    effectlasting = effectlasting > 0 ? effectlasting : 900;
                    break;
                case 3:
                    Player.AddBuff(BuffID.Confused, 600);
                    effectlasting = effectlasting > 0 ? effectlasting : 40;
                    break;
                case 4:
                    NPCdeal5TimeDamage = true;
                    effectlasting = effectlasting > 0 ? effectlasting : 210;
                    break;
                case 5:
                    if (effectlasting % 4 == 0)
                    {
                        NPC.SpawnOnPlayer(Player.whoAmI, Main.rand.Next(TerrariaArrayID.BAT));
                    }
                    effectlasting = effectlasting > 0 ? effectlasting : 100;
                    break;
                case 6:
                    UnableToUseWeapon = true;
                    int dust = Dust.NewDust(Player.Center, 0, 0, DustID.Blood);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity = Main.rand.NextVector2Circular(5, 5);
                    effectlasting = effectlasting > 0 ? effectlasting : 120;
                    break;
                case 7:
                    for (int i = 0; i < Player.CountBuffs(); i++)
                    {
                        Player.DelBuff(i);
                    }
                    effectlasting = effectlasting > 0 ? effectlasting : 1;
                    break;
                case 8:
                    if (Main.dayTime)
                    {
                        if (Main.hardMode)
                        {
                            Main.eclipse = true;
                        }
                    }
                    else
                    {
                        Main.bloodMoon = true;
                    }
                    RNGdecide = Main.rand.Next(8);
                    effectlasting = effectlasting > 0 ? effectlasting : 100;
                    break;
                case 9:
                    Effect9();
                    effectlasting = effectlasting > 0 ? effectlasting : 1;
                    break;
                case 10:
                    Effect10();
                    effectlasting = effectlasting > 0 ? effectlasting : 1;
                    break;
                case 11:
                    Effect11();
                    effectlasting = effectlasting > 0 ? effectlasting : 300;
                    break;
                case 12:
                    Effect12();
                    effectlasting = effectlasting > 0 ? effectlasting : 1;
                    break;
                case 13:
                    Effect13();
                    effectlasting = effectlasting > 0 ? effectlasting : 180;
                    break;
                case 14:
                    Effect14();
                    effectlasting = effectlasting > 0 ? effectlasting : 60;
                    break;
                case 15:
                    effectlasting = effectlasting > 0 ? effectlasting : 30;
                    if (OPEFFECT == 0)
                    {
                        Effect9();
                        Effect10();
                        Effect11();
                        Effect12();
                        Effect13();
                        Effect14();
                        Effect16(effectlasting);
                        OPEFFECT = 10;
                    }
                    OPEFFECT -= OPEFFECT > 0 ? 1 : 0;
                    break;
                case 16:
                    effectlasting = effectlasting > 0 ? effectlasting : 600;
                    Effect16(effectlasting);
                    break;
                default:
                    break;
            }
            if (Main.rand.NextBool(300) && !ExtendingEffect)
            {
                ExtendingEffect = true;
                effectlasting *= 5;
            }
        }
        private void SpamTextToScare()
        {
            if (Main.rand.NextBool(750))
            {
                int RandomTextChooser = Main.rand.Next(11);
                Color color = Color.White;
                string TextString;
                switch (RandomTextChooser)
                {
                    case 0:
                        TextString = "You feel an evil presence watching you...";
                        color = new Color(50, 255, 130);
                        break;
                    case 1:
                        TextString = "You feel vibrations from deep below...";
                        color = new Color(50, 255, 130);
                        break;
                    case 2:
                        TextString = "This is going to be a terrible night...";
                        color = new Color(50, 255, 130);
                        break;
                    case 3:
                        TextString = "The air is getting colder around you...";
                        color = new Color(50, 255, 130);
                        break;
                    case 4:
                        TextString = "Pirates are approaching from the west!";
                        color = new Color(175, 75, 255);
                        break;
                    case 5:
                        TextString = RandomName() + " has joined.";
                        break;
                    case 6:
                        TextString = RandomNameExit() + " has left";
                        break;
                    case 7:
                        TextString = "The weather forecast that there will be boulder rain soon !";
                        color = new Color(10, 10, 255);
                        break;
                    case 8:
                        TextString = "I see you, we gonna be together forever, forever chopping tree " + $"[i:{ItemID.LucyTheAxe}]" + $"[i:{ItemID.Heart}]";
                        color = new Color(100, 0, 0);
                        break;
                    case 9:
                        TextString = "Rare life form detected that there is a furry near by !";
                        color = new Color(10, 10, 255);
                        break;
                    case 10:
                        TextString = "To give yourself a zenith, please write in chat : \\Player::give[i:Zenith][1]";
                        color = new Color(10, 10, 255);
                        break;
                    default:
                        TextString = "The weather forecast that there will be boulder rain soon";
                        color = new Color(10, 10, 255);
                        break;
                }
                Main.NewText(TextString, color);
            }
        }
        private string RandomName()
        {
            switch (Main.rand.Next(10))
            {
                case 0:
                    return "God";
                case 1:
                    return Player.name;
                case 2:
                    return "Your mom";
                case 3:
                    return "Red";
                case 4:
                    return "skillissue";
                case 5:
                    return "FBI";
                case 6:
                    return "Guide";
                case 7:
                    return "Sans";
                case 8:
                    return "LQTXinim";
                case 9:
                    return "drugaddict";
                default:
                    return "asgfgfagasdf";
            }
        }
        private string RandomNameExit()
        {
            switch (Main.rand.Next(10))
            {
                case 0:
                    return "Your lover";
                case 1:
                    return Player.name;
                case 2:
                    return "Your father";
                case 3:
                    return "Anime";
                case 4:
                    return "Luck";
                case 5:
                    return "LQTMinix";
                case 6:
                    return "Ninja";
                case 7:
                    return "Sans";
                case 8:
                    return "FeelingLucky";
                case 9:
                    return "ImNotGud";
                default:
                    return "asgfgfagasdf";
            }
        }
        private void Effect9()
        {
            ChestLootDrop.GetWeapon(out int Weapon, out int amount);
            Player.QuickSpawnItem(null, Weapon, amount);
        }
        private void Effect10()
        {
            Player.Center.LookForHostileNPC(out List<NPC> npc, 1000);
            for (int i = 0; i < npc.Count; i++)
            {
                npc[i].StrikeNPC(npc[i].CalculateHitInfo(1000, 0));
                NetMessage.SendStrikeNPC(npc[i], npc[i].CalculateHitInfo(1000, 0));
            }
        }
        private void Effect11()
        {
            Vector2 position = new Vector2(Player.Center.X + Main.rand.NextFloat(-1000, 1000), Player.Center.Y - 1000);
            Vector2 velocity = new Vector2(0, 20);
            Projectile.NewProjectile(Player.GetSource_FromThis(), position, velocity, ProjectileID.StarWrath, Player.HeldItem.damage * 3, 10, Player.whoAmI);
        }
        private void Effect12()
        {
            Player.statLife += (int)Math.Clamp(Player.statLifeMax2 * .25f, 0, Player.statLifeMax2);
        }
        private void Effect13()
        {
            CanCrit = true;
        }
        private void Effect14()
        {
            CanBeHit = false;
        }
        private void Effect16(int effectlasting)
        {
            Player.AddBuff(BuffID.NebulaUpLife3, effectlasting);
            Player.AddBuff(BuffID.NebulaUpDmg3, effectlasting);
            Player.AddBuff(BuffID.NebulaUpMana3, effectlasting);
            Player.AddBuff(BuffID.Endurance, effectlasting);
            Player.AddBuff(BuffID.Wrath, effectlasting);
            Player.AddBuff(BuffID.Rage, effectlasting);
            Player.AddBuff(BuffID.Lifeforce, effectlasting);
            Player.AddBuff(BuffID.Regeneration, effectlasting);
            Player.AddBuff(BuffID.RapidHealing, effectlasting);
            Player.AddBuff(BuffID.Honey, effectlasting);
            Player.AddBuff(BuffID.BeetleEndurance3, effectlasting);
            Player.AddBuff(BuffID.BeetleMight3, effectlasting);
            Player.AddBuff(BuffID.BrainOfConfusionBuff, effectlasting);
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (Greed)
            {
                damage *= .65f;
            }
            if (Pride)
            {
                damage += .45f;
            }
        }
        int countRange = 0;
        private void LifeSteal(NPC target, int rangeMin = 1, int rangeMax = 3, float multiplier = 1)
        {
            if (target.lifeMax > 5 && !target.friendly && target.type != NPCID.TargetDummy)
            {
                int HP = (int)(Main.rand.Next(rangeMin, rangeMax) * multiplier);
                int HPsimulation = Player.statLife + HP;
                if (HPsimulation < Player.statLifeMax2)
                {
                    Player.Heal(HP);
                }
                else
                {
                    Player.statLife = Player.statLifeMax2;
                }
            }
        }
        public override bool CanUseItem(Item item)
        {
            if (FateDice)
            {
                return !UnableToUseWeapon;
            }
            if (Earth)
            {
                return EarthCD <= 0;
            }
            return base.CanUseItem(item);
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (FateDice & NPCdeal5TimeDamage)
            {
                info.Damage *= 5;
            }
            if (Earth)
            {
                EarthCD = 300;
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Vampire)
            {
                LifeSteal(target, 3, 6, Main.rand.NextFloat(1, 3));
            }
            if (CanCrit && FateDice)
            {
                hit.Crit = true;
                hit.Damage *= 5;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Vampire)
            {
                if (proj.DamageType == DamageClass.Melee)
                {
                    LifeSteal(target, 3, 6, Main.rand.NextFloat(1, 3));
                    return;
                }
                countRange++;
                if (countRange >= 3)
                {
                    LifeSteal(target);
                    countRange = 0;
                }
            }
            if (CanCrit && FateDice)
            {
                hit.Crit = true;
                hit.Damage *= 5;
            }
        }
        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            if (FateDice)
            {
                return !CanBeHit;
            }
            return base.ImmuneTo(damageSource, cooldownCounter, dodgeable);
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (!Player.HasBuff(ModContent.BuffType<SecondChance>()) && Vampire)
            {
                Player.Heal(Player.statLifeMax2);
                Player.AddBuff(ModContent.BuffType<SecondChance>(), 10800);
                return false;
            }
            return true;
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRushNetCodeHandle.MessageType.ArtifactRegister);
            packet.Write((byte)Player.whoAmI);
            packet.Write(ArtifactDefinedID);
            packet.Send(toWho, fromWho);
        }
        public override void SaveData(TagCompound tag)
        {
            tag["ArtifactDefinedID"] = ArtifactDefinedID;
        }
        public override void LoadData(TagCompound tag)
        {
            ArtifactDefinedID = (int)tag["ArtifactDefinedID"];
        }
    }
}