using Terraria;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Weapon;
using BossRush.Common.RoguelikeChange.Prefixes;
using BossRush.Contents.Items.Accessories.TrinketAccessories;
using BossRush.Contents.Items.Consumable.Potion;

namespace BossRush {
	public partial class BossRush : Mod {
		public static BossRush Instance { get; private set; }
		public override void Load() {
			Instance = this;
			base.Load();
		}
	}
	public class BossRushModSystem : ModSystem {
		public static bool[] IsFireBuff;
		public static bool[] IsPoisonBuff;
		public static bool[] CanBeAffectByLastingVile;
		public static bool[] AdvancedRPGItem;
		public static Dictionary<int, List<int>> WeaponRarityDB { get; private set; }
		public static Dictionary<int, List<int>> AccRarityDB { get; private set; }
		public static Dictionary<int, List<int>> HeadArmorRarityDB { get; private set; }
		public static Dictionary<int, List<int>> BodyArmorRarityDB { get; private set; }
		public static Dictionary<int, List<int>> LegsArmorRarityDB { get; private set; }
		public static List<Item> SynergyItem { get; private set; }
		public static List<Item> LostAccessories { get; private set; }
		public static List<Item> TrinketAccessories { get; private set; }
		public static List<Item> RPGItem { get; private set; }

		public static List<int> ListLootboxType;
		public override void OnModLoad() {
			TrinketAccessories = new();
			LostAccessories = new();
			SynergyItem = new();
			RPGItem = new();
			WeaponRarityDB = new();
			ListLootboxType = new();
			HeadArmorRarityDB = new();
			BodyArmorRarityDB = new();
			LegsArmorRarityDB = new();
			AccRarityDB = new();
		}
		public override void OnModUnload() {
			SynergyItem = null;
			LostAccessories = null;
			RPGItem = null;
			TrinketAccessories = null;
			ListLootboxType = null;
			WeaponRarityDB = null;
			IsFireBuff = null;
			IsPoisonBuff = null;
			HeadArmorRarityDB = null;
			BodyArmorRarityDB = null;
			LegsArmorRarityDB = null;
			AccRarityDB = null;
		}
		public override void PostSetupContent() {
			IsFireBuff = BuffID.Sets.Factory.CreateBoolSet(BuffID.OnFire, BuffID.OnFire3, BuffID.ShadowFlame, BuffID.Frostburn, BuffID.Frostburn2, BuffID.CursedInferno);
			IsPoisonBuff = BuffID.Sets.Factory.CreateBoolSet(BuffID.Poisoned, BuffID.Venom);
			CanBeAffectByLastingVile = BuffID.Sets.Factory.CreateBoolSet(true, ModContent.BuffType<LastingVileBuff>());
			AdvancedRPGItem = ItemID.Sets.Factory.CreateBoolSet();
			List<Item> cacheitemList = ContentSamples.ItemsByType.Values.ToList();
			for (int i = 0; i < cacheitemList.Count; i++) {
				Item item = cacheitemList[i];
				if (item.ModItem is LootBoxBase) {
					ListLootboxType.Add(item.type);
					continue;
				}
				if (item.ModItem is SynergyModItem) {
					SynergyItem.Add(item);
					continue;
				}
				if (item.headSlot > 0) {
					if (!HeadArmorRarityDB.ContainsKey(item.rare)) {
						HeadArmorRarityDB.Add(item.rare, new List<int> { item.type });
					}
					else {
						HeadArmorRarityDB[item.rare].Add(item.type);
					}
					continue;
				}
				else if (item.bodySlot > 0) {
					if (!BodyArmorRarityDB.ContainsKey(item.rare)) {
						BodyArmorRarityDB.Add(item.rare, new List<int> { item.type });
					}
					else {
						BodyArmorRarityDB[item.rare].Add(item.type);
					}
					continue;
				}
				else if (item.legSlot > 0) {
					if (!LegsArmorRarityDB.ContainsKey(item.rare)) {
						LegsArmorRarityDB.Add(item.rare, new List<int> { item.type });
					}
					else {
						LegsArmorRarityDB[item.rare].Add(item.type);
					}
					continue;
				}
				GlobalItemHandle globalitem = item.GetGlobalItem<GlobalItemHandle>();
				if (item.accessory) {
					if (item.ModItem is BaseTrinket) {
						TrinketAccessories.Add(item);
						continue;
					}
					if (globalitem.LostAccessories) {
						LostAccessories.Add(item);
						continue;
					}
					if (!AccRarityDB.ContainsKey(item.rare)) {
						AccRarityDB.Add(item.rare, new List<int> { item.type });
					}
					else {
						AccRarityDB[item.rare].Add(item.type);
					}
					continue;
				}
				if (globalitem.RPGItem) {
					if (globalitem.AdvancedBuffItem) {
						AdvancedRPGItem[item.type] = true;
					}
					RPGItem.Add(item);
					continue;
				}
				if (!item.IsAWeapon()) {
					continue;
				}
				if (!WeaponRarityDB.ContainsKey(item.rare)) {
					WeaponRarityDB.Add(item.rare, new List<int> { item.type });
				}
				else {
					WeaponRarityDB[item.rare].Add(item.type);
				}
			}
		}
		public override void PostAddRecipes() {
			if (ModLoader.TryGetMod("PrefixImproved", out Mod PrefixImproved)) {
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Vampiric>()).Name, (byte)4);
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Jumpy>()).Name, (byte)4);
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Evasive>()).Name, (byte)4);
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Cunning>()).Name, (byte)2);
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Stealthy>()).Name, (byte)2);
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Spiky>()).Name, (byte)1);
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Alchemic>()).Name, (byte)2);
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Energetic>()).Name, (byte)2);
			}
		}
	}
}

