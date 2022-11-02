﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Chest
{
    class LihzahrdTreasureChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Right click to open chest to get the following items\n5 weapons from pre boss to post golem\n2 of random combat/movement accessories\nhead to dungeon to end the run or fight duke fishron, EoL 5 time lol\nGood Luck !");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 10;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            ChestLootDrop Chest = new ChestLootDrop(player);
            for (int i = 0; i < 2; i++)
            {
                int Accessory = Main.rand.Next(new int[] { ItemID.MasterNinjaGear, ItemID.FireGauntlet, ItemID.NecromanticScroll, ItemID.CelestialEmblem, ItemID.CelestialShell, ItemID.AvengerEmblem, ItemID.CharmofMyths, ItemID.DestroyerEmblem, ItemID.SniperScope, ItemID.StarCloak, ItemID.StarVeil, ItemID.CelestialCuffs });
                player.QuickSpawnItem(entitySource, Accessory);
            }
            int wing = Main.rand.Next(new int[] { ItemID.BeeWings, ItemID.BeetleWings, ItemID.BoneWings, ItemID.BatWings, ItemID.MothronWings, ItemID.ButterflyWings, ItemID.Hoverboard, ItemID.FlameWings, ItemID.GhostWings, ItemID.FestiveWings, ItemID.SpookyWings, ItemID.TatteredFairyWings });
            player.QuickSpawnItem(entitySource, wing);
            Chest.GetAmount(out int amount, out int _, out int _, player);
            for (int i = 0; i < amount; i++)
            {
                Chest.GetWeapon(out int weapon, out int specialAmount, true, Chest.RNGManage( 25, 25, 25, 25, 0));
                Chest.AmmoForWeapon(out int ammo, out int num, weapon, 3.5f);
                player.QuickSpawnItem(entitySource, weapon, specialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            for (int i = 0; i < 3; i++)
            {
                Chest.GetAccessory(out int Accessory, true, true, true, false, false);
                player.QuickSpawnItem(entitySource, Accessory);
            }
            player.QuickSpawnItem(entitySource, ItemID.GoldenFishingRod);
        }
    }
}