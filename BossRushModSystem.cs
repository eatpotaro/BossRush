﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Items;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace BossRush
{
    internal class BossRushModSystem : ModSystem
    {
        List<int> list = new List<int>();
        public override void AddRecipeGroups()
        {
            foreach (var item in ContentSamples.ItemsByType)
            {
                if (item.Value.ModItem is ISynergyItem)
                {
                    list.Add(item.Key);
                }
            }
            RecipeGroup SynergyItem = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<SynergyEnergy>())}", list.ToArray());
            RecipeGroup.RegisterGroup("Synergy Item", SynergyItem);

            RecipeGroup WoodSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.WoodenSword)}", new int[]
            {
                ItemID.WoodenSword,
                ItemID.BorealWoodSword,
                ItemID.RichMahoganySword,
                ItemID.ShadewoodSword,
                ItemID.EbonwoodSword,
                ItemID.PalmWoodSword,
                ItemID.PearlwoodSword,
            });
            RecipeGroup.RegisterGroup("Wood Sword", WoodSword);

            RecipeGroup WoodBow = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.WoodenBow)}", new int[]
            {
                ItemID.WoodenBow,
                ItemID.BorealWoodBow,
                ItemID.RichMahoganyBow,
                ItemID.ShadewoodBow,
                ItemID.EbonwoodBow,
                ItemID.PalmWoodBow,
                ItemID.PearlwoodBow,
            });
            RecipeGroup.RegisterGroup("Wood Bow", WoodBow);

            RecipeGroup OreShortSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CopperShortsword)}", new int[]
            {
                ItemID.CopperShortsword,
                ItemID.TinShortsword,
                ItemID.IronShortsword,
                ItemID.LeadShortsword,
                ItemID.SilverShortsword,
                ItemID.TungstenShortsword,
                ItemID.GoldShortsword,
                ItemID.PlatinumShortsword,
            });
            RecipeGroup.RegisterGroup("OreShortSword", OreShortSword);

            RecipeGroup OreBroadSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CopperBroadsword)}", new int[]
            {
                ItemID.CopperBroadsword,
                ItemID.TinBroadsword,
                ItemID.IronBroadsword,
                ItemID.LeadBroadsword,
                ItemID.SilverBroadsword,
                ItemID.TungstenBroadsword,
                ItemID.GoldBroadsword,
                ItemID.PlatinumBroadsword,
            });
            RecipeGroup.RegisterGroup("OreBroadSword", OreBroadSword);

            RecipeGroup OreBow = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CopperBow)}", new int[]
            {
                ItemID.CopperBow,
                ItemID.TinBow,
                ItemID.IronBow,
                ItemID.LeadBow,
                ItemID.SilverBow,
                ItemID.TungstenBow,
                ItemID.GoldBow,
                ItemID.PlatinumBow,
            });
            RecipeGroup.RegisterGroup("OreBow", OreBow);
        }
        public override void PostAddRecipes()
        {
            if (!ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                return;
            }
            for (int i = 0; i < Main.recipe.Length; i++)
            {
                if (Main.recipe[i].HasResult(ItemID.FlamingArrow) 
                    || Main.recipe[i].HasResult(ItemID.FrostburnArrow) )
                {
                    Main.recipe[i].DisableRecipe();
                }
            }
        }
        static int ChallengeR = 100, ChallengeB = 100;
        public static Color ChallangeGodColor => new Color(ChallengeR, 0, ChallengeB);
        int ColorStyle = 0;
        public override void PostUpdateEverything()
        {
            //Pulsing Purple
            if (ColorStyle != 1)
            {
                if (ChallengeR < 255)
                {
                    Math.Clamp(++ChallengeR, 100, 255);
                    Math.Clamp(++ChallengeB, 100, 255);
                }
                else
                {
                    ColorStyle = 1;
                }
            }
            else
            {
                if (ChallengeR > 100)
                {
                    Math.Clamp(--ChallengeR, 100, 255);
                    Math.Clamp(--ChallengeB, 100, 255);
                }
                else
                {
                    ColorStyle = 0;
                }
            }
        }
    }
}
