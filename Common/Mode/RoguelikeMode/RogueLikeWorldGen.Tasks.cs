﻿using Terraria;
using Terraria.ID;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossRush.Common.Utils;
using Terraria.WorldBuilding;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;
using BossRush.Common.General;
using System;

namespace BossRush.Common.WorldGenOverhaul;
public class PlayerBiome : ModPlayer {
	HashSet<short> CurrentBiome = new HashSet<short>();
}
public class BiomeAreaID {
	public const short None = 0;
	public const short Forest = 1;
	public const short Jungle = 2;
	public const short Tundra = 3;
	public const short Desert = 4;
	public const short Crimson = 5;
	public const short Corruption = 6;
	public const short Dungeon = 7;
	public const short BlueShroom = 8;
	public const short Granite = 9;
	public const short Marble = 10;
	public const short Slime = 11;
	public const short FleshRealm = 12;
	public const short Beaches = 13;
	public const short Underground = 14;
	public const short BeeNest = 15;
	public const short Hallow = 16;
	public const short Ocean = 17;
}

public partial class RogueLikeWorldGen : ModSystem {
	public static Dictionary<short, string> BiomeID;
	public override void OnModLoad() {
		BiomeID = new();
		FieldInfo[] field = typeof(BiomeAreaID).GetFields();
		for (int i = 0; i < field.Length; i++) {
			object? obj = field[i].GetValue(null);
			if (obj == null) {
				continue;
			}
			if (obj.GetType() != typeof(short)) {
				continue;
			}
			short objvalue = (short)obj;
			BiomeID.Add(objvalue, field[i].Name);
		}

	}
	public override void OnModUnload() {
		BiomeID = null;
	}

	public static int GridPart_X = Main.maxTilesX / 24;
	public static int GridPart_Y = Main.maxTilesY / 24;
	public static float WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
	public static float WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
		if (ModContent.GetInstance<RogueLikeConfig>().WorldGenTest) {
			//tasks.ForEach(g => g.Disable());
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Chests")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Buried Chests")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Surface Chests")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Chests Placement")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Water Chests")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Trees")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Gems In Ice Biome")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Random Gems")));
			//tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Shinies")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Spider Caves")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Living Trees")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Wood Tree Walls")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Floating Islands")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Floating Island Houses")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Life Crystals")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Shinies")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Pyramids")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Altars")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Hives")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Chests")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Buried Chests")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Surface Chests")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Chests Placement")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Water Chests")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Trees")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Temple")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Micro Biomes")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Marble")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Granite")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Mushrooms")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Moss")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Surface Ore and Stone")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Planting Trees")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Larva")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Cactus, Palm Trees, & Coral")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Gems In Ice Biome")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Random Gems")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Vines")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Piles")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Traps")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Statues")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Shell Piles")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Oasis")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Water Plants")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Flowers")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle Plants")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Wavy Caves")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Rock Layer Caves")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Weeds")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Webs And Honey")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Clay")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Herbs")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Dye Plants")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Dirt Layer Caves")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Moss Grass")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Hellforge")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Pots")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Place Fallen Log")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Mushroom Patches")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Glowing Mushrooms and Jungle Plants")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Small Holes")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Remove Broken Traps")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Jungle")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Full Desert")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Wall Variety")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Gem Caves")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Cave Walls")));
			tasks.RemoveAt(tasks.FindIndex(GenPass => GenPass.Name.Equals("Temple")));
			tasks.AddRange(((ITaskCollection)this).Tasks);
		}
	}
	public static Dictionary<short, List<Rectangle>> Biome;
	public override void SaveWorldData(TagCompound tag) {
		if (Biome == null) {
			return;
		}
		tag["BiomeType"] = Biome.Keys.ToList();
		tag["BiomeArea"] = Biome.Values.ToList();
	}
	public override void LoadWorldData(TagCompound tag) {
		var Type = tag.Get<List<short>>("BiomeType");
		var Area = tag.Get<List<List<Rectangle>>>("BiomeArea");
		if (Type == null || Area == null) {
			return;
		}
		Biome = Type.Zip(Area, (k, v) => new { Key = k, Value = v }).ToDictionary(x => x.Key, x => x.Value);
	}
}
public partial class RogueLikeWorldGen : ITaskCollection {
	[Task]
	public void AddAltar() {
		GridPart_X = Main.maxTilesX / 24;//small world : 175
		GridPart_Y = Main.maxTilesY / 24;//small world : 50
		WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
		WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
		bool RNG = false;
		bool PlacedSlimeShrine = false;
		for (int i = 1; i < Main.maxTilesX - 1; i++) {
			for (int j = 1; j < Main.maxTilesY - 1; j++) {
				if (WorldGen.genRand.NextBool(10000)) {
					//StructureHelper.Generator.GenerateStructure("Assets/Structures/SlimeShrine", new(i, j), Mod);
					//WorldGen.PlaceChest()
				}
				if (!RNG) {
					if (WorldGen.genRand.NextBool(1500)) {
						RNG = true;
					}
				}
				else {
					int pass = 0;
					for (int offsetX = -1; offsetX <= 1; offsetX++) {
						for (int offsetY = -1; offsetY <= 1; offsetY++) {
							if (offsetX == 0 && offsetY == 0) continue;
							if (offsetY == 1 && offsetX == 0) continue;
							if (!WorldGen.InWorld(i + offsetX, j + offsetY)) continue;
							if (!WorldGen.TileEmpty(i + offsetX, j + offsetY)) {
								j = Math.Clamp(j + 1, 0, Main.maxTilesY);
								break;
							}
							else {
								pass++;
							}
						}
					}
					if (pass >= 7) {
						WorldGen.PlaceTile(i, j, Main.rand.Next(TerrariaArrayID.Altar));
						RNG = false;
					}
				}

				if (!PlacedSlimeShrine) {
					if (WorldGen.genRand.NextBool(10500)) {
						Generate_SlimeBossAltar(i, j);
					}
					if (WorldGen.genRand.NextBool(10500)) {
						Generate_EoCAltar(i, j);
					}
				}
			}
		}
	}
	public void Generate_SlimeBossAltar(int X, int Y) {
		GenerationHelper.PlaceStructure("Shrine/SlimeShrine", new Rectangle(X, Y, 11, 12));
		WorldGen.PlaceTile(X + 5, Y + 6, ModContent.TileType<SlimeBossAltar>());
	}
	public void Generate_EoCAltar(int X, int Y) {
		GenerationHelper.PlaceStructure("Shrine/FleshShrine", new Rectangle(X, Y, 11, 12));
		WorldGen.PlaceTile(X + 5, Y + 6, ModContent.TileType<EoCBossAltar>());
	}
	[Task]
	public void GenerateSlimeZone() {
		Rectangle rect = new();
		rect = GenerationHelper.GridPositionInTheWorld24x24(7, 10, 3, 3);
		Point counter = new();
		int count = -1;
		bool IsUsingHorizontal = false;
		int offsetcount = 0;
		int additionaloffset = -1;
		counter -= new Point(64, 64);
		while (counter.X < rect.Width || counter.Y < rect.Height) {
			ImageData template;
			IsUsingHorizontal = ++count % 2 == 0;
			if (IsUsingHorizontal) {
				template = ImageStructureLoader.Get_Tempate("WG_TemplateHorizontal" + WorldGen.genRand.Next(1, 8));
			}
			else {
				template = ImageStructureLoader.Get_Tempate("WG_TemplateVertical" + WorldGen.genRand.Next(1, 9));
			}
			if (++additionaloffset >= 2) {
				counter.X += 32;
				additionaloffset = 0;
			}
			template.EnumeratePixels((a, b, color) => {
				a += rect.X + counter.X;
				b += rect.Y + counter.Y;
				if (a > rect.Right || b > rect.Bottom) {
					return;
				}
				if (a < rect.Left || b < rect.Top) {
					return;
				}
				GenerationHelper.FastRemoveTile(a, b);
				if (color.R == 255 && color.B == 0 && color.G == 0) {
					GenerationHelper.FastPlaceTile(a, b, TileID.SlimeBlock);
				}
				GenerationHelper.FastPlaceWall(a, b, WallID.Slime);
			});
			if (counter.X < rect.Width) {
				counter.X += template.Width;
			}
			else {
				offsetcount++;
				counter.X = 0 - 32 * offsetcount;
				counter.Y += 32;
				count = 1;
				additionaloffset = -1;
			}
		}
		//Biome.Add(BiomeAreaID.Slime, new List<Rectangle> { rect });
	}
	[Task]
	public void GenerateFleshZone() {

		//Biome.Add(BiomeAreaID.Slime, new List<Rectangle> { rect });
	}
	//[Task]
	//public void SetUp() {
	//	Biome = new Dictionary<short, List<Rectangle>>();
	//	GridPart_X = Main.maxTilesX / 24;
	//	GridPart_Y = Main.maxTilesY / 24;
	//	Main.worldSurface = 0;
	//	Main.rockLayer = 0;
	//	Main.spawnTileX = (int)(Main.maxTilesX / 2);
	//	Main.spawnTileY = (int)(Main.maxTilesY * .22f);
	//	WorldWidthHeight_Ratio = Main.maxTilesX / (float)Main.maxTilesY;
	//	WorldHeightWidth_Ratio = Main.maxTilesX / (float)Main.maxTilesX;
	//	GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(0, 0, 24, 22),
	//		(i, j) => {
	//			GenerationHelper.FastPlaceTile(i, j, TileID.Dirt);
	//		}
	//	);
	//	//GenerationHelper.ForEachInRectangle(
	//	//	0,
	//	//	0,
	//	//	Main.maxTilesX,
	//	//	Main.maxTilesY,
	//	//	(i, j) => {
	//	//		for (int a = 1; a < 24; a++) {
	//	//			if (i == GridPart_X * a || j == GridPart_Y * a) {
	//	//				GenerationHelper.FastPlaceTile(i, j, TileID.LihzahrdBrick);
	//	//				break;
	//	//			}
	//	//		}
	//	//	}
	//	//);
	//	//small world  : x = 4200 | y = 1200
	//	//medium world : x = 6400 | y = 1800
	//	//large world  : x = 8400 | y = 2400
	//}
	//[Task]
	//public void Create_Hell() {
	//	GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(0, 23, 24, 1),
	//	(i, j) => {
	//		if (j == GridPart_Y * 23) {
	//			GenerationHelper.FastPlaceTile(i, j, TileID.AshGrass);
	//		}
	//		else {
	//			GenerationHelper.FastPlaceTile(i, j, TileID.Ash);
	//		}
	//	});
	//}
	//[Task]
	//public void Empty_AreaAroundPlayer() {
	//	GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(0, 0, 24, 5),
	//	(i, j) => {
	//		GenerationHelper.FastRemoveTile(i, j);
	//	});
	//	GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(11, 5, 2, 1),
	//	(i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.Grass);
	//	});
	//	GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(11, 7, 2, 1),
	//	(i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.Stone);
	//		GenerationHelper.FastPlaceWall(i, j, WallID.Stone);
	//	});
	//}
	//[Task]
	//public void Create_Jungle() {//17 -> 21
	//	List<Rectangle> rectList = new List<Rectangle>();
	//	rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(15, 5, 2, 6));
	//	GenerationHelper.ForEachInRectangle(rectList[0],
	//	(i, j) => {
	//		if (i <= rectList[0].PointOnRectX(.01f) || i >= rectList[0].PointOnRectX(.99f)) {
	//			//only place block if I is between 20% and 80% of rect width
	//			GenerationHelper.FastPlaceTile(i, j, TileID.JungleGrass);
	//		}
	//		else if (j <= rectList[0].PointOnRectY(.01f) || j >= rectList[0].PointOnRectY(.99f)) {
	//			//only place block if J is between 20% and 80% of rect height
	//			{
	//				GenerationHelper.FastPlaceTile(i, j, TileID.JungleGrass);
	//			}
	//		}
	//		else {
	//			GenerationHelper.FastPlaceTile(i, j, TileID.Mud);
	//		}
	//	});
	//	//Biome.Add(RogueLike_BiomeAreaID.Jungle, rectList);
	//}
	//[Task]
	//public void Create_Tundra() {
	//	List<Rectangle> rectList = new List<Rectangle>();
	//	rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(9, 5, 2, 6));
	//	GenerationHelper.ForEachInRectangle(rectList[0],
	//	(i, j) => {
	//		if (i <= rectList[0].PointOnRectX(.05f) || i >= rectList[0].PointOnRectX(.95f)) {
	//			//only place block if I is between 20% and 80% of rect width
	//			GenerationHelper.FastPlaceTile(i, j, TileID.SnowBlock);
	//		}
	//		else if (j <= rectList[0].PointOnRectY(.05f) || j >= rectList[0].PointOnRectY(.95f)) {
	//			//only place block if J is between 20% and 80% of rect height
	//			{
	//				GenerationHelper.FastPlaceTile(i, j, TileID.SnowBlock);
	//			}
	//		}
	//		else {
	//			GenerationHelper.FastPlaceTile(i, j, TileID.IceBlock);
	//		}
	//	});
	//	//Biome.Add(RogueLike_BiomeAreaID.Tundra, rectList);
	//}
	//[Task]
	//public void Create_Crimson() {
	//	List<Rectangle> rectList = new List<Rectangle>();
	//	rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(4, 5, 3, 7));
	//	GenerationHelper.ForEachInRectangle(rectList[0],
	//	(i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.Crimstone);
	//	});
	//	//Biome.Add(RogueLike_BiomeAreaID.Crimson, rectList);
	//}
	//[Task]
	//public void Create_Corruption() {
	//	List<Rectangle> rectList = new List<Rectangle>();
	//	rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(17, 5, 3, 7));
	//	GenerationHelper.ForEachInRectangle(rectList[0],
	//	(i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.Ebonstone);

	//	});
	//	//Biome.Add(RogueLike_BiomeAreaID.Corruption, rectList);
	//}

	//[Task]
	//public void Create_Hallowed() {
	//	List<Rectangle> rectList = new List<Rectangle>();
	//	rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(20, 5, 1, 7));
	//	GenerationHelper.ForEachInRectangle(rectList[0],
	//	(i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.Pearlstone);

	//	});
	//	rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(3, 5, 1, 7));
	//	GenerationHelper.ForEachInRectangle(rectList[1],
	//	(i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.Pearlstone);

	//	});
	//	//Biome.Add(RogueLike_BiomeAreaID.Corruption, rectList);
	//}
	//[Task]
	//public void Create_Desert() {
	//	List<Rectangle> rectList = new List<Rectangle>();
	//	rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(13, 5, 2, 7));
	//	GenerationHelper.ForEachInRectangle(rectList[0],
	//	(i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.Sandstone);
	//	});
	//	//Biome.Add(RogueLike_BiomeAreaID.Desert, rectList);
	//}
	//[Task]
	//public void Create_BlueShroom() {
	//	List<Rectangle> rectList = new List<Rectangle>();
	//	rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(18, 14, 3, 3));
	//	GenerationHelper.ForEachInRectangle(rectList[0],
	//	(i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.MushroomGrass);
	//	});
	//	//Biome.Add(RogueLike_BiomeAreaID.BlueShroom, rectList);
	//}
	//[Task]
	//public void Create_BigGranite() {
	//	List<Rectangle> rectList = new List<Rectangle>();
	//	rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(17, 12, 3, 1));
	//	GenerationHelper.ForEachInRectangle(rectList[0],
	//	(i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.Granite);
	//		GenerationHelper.FastPlaceWall(i, j, WallID.GraniteUnsafe);
	//	});
	//	//Biome.Add(RogueLike_BiomeAreaID.Granite, rectList);
	//}
	//[Task]
	//public void Create_Marble() {
	//	List<Rectangle> rectList = new List<Rectangle>();
	//	rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(13, 12, 2, 2));
	//	GenerationHelper.ForEachInRectangle(rectList[0],
	//	(i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.Marble);
	//		GenerationHelper.FastPlaceWall(i, j, WallID.MarbleUnsafe);
	//	});
	//	//Biome.Add(RogueLike_BiomeAreaID.Marble, rectList);
	//}
	//[Task]
	//public void Create_Dungeon() {
	//	GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(9, 11, 2, 5),
	//	(i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.BlueDungeonBrick);
	//		GenerationHelper.FastPlaceWall(i, j, WallID.BlueDungeonUnsafe);
	//	});
	//}
	//[Task]
	//public void Create_JungleTemple() {
	//	GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(15, 11, 2, 5),
	//	(i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.LihzahrdBrick);
	//		GenerationHelper.FastPlaceWall(i, j, WallID.LihzahrdBrickUnsafe);
	//	});
	//}
	//public void Create_BeeNest() {
	//	GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(15, 11, 2, 5),
	//	(i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.BeeHive);
	//		GenerationHelper.FastPlaceWall(i, j, WallID.Hive);
	//	});
	//}
	//[Task]//Test
	//public void Create_MinerParadise() {
	//	//Minor Biome or soon to be
	//	int[] oreIDarr = { TileID.Copper, TileID.Tin, TileID.Iron, TileID.Lead, TileID.Silver, TileID.Tungsten, TileID.Gold, TileID.Platinum, TileID.Palladium, TileID.Cobalt, TileID.Orichalcum, TileID.Mythril, TileID.Adamantite, TileID.Titanium };
	//	GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(11, 11, 2, 5),
	//	(i, j) => {
	//		int oreID = WorldGen.genRand.Next(oreIDarr);
	//		GenerationHelper.FastPlaceTile(i, j, Main.rand.NextFloat() >= .45f ? TileID.Stone : oreID);
	//		GenerationHelper.FastPlaceWall(i, j, WallID.Stone);
	//	});
	//}
	//[Task]
	//public void Create_CloudBiome() {
	//	//Minor Biome or soon to be
	//	GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(10, 0, 4, 3),
	//	(i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.Cloud);
	//		GenerationHelper.FastPlaceWall(i, j, WallID.Cloud);
	//	});
	//}
	//[Task]
	//public void Create_SlimeWorld() {
	//	//Minor Biome or soon to be
	//	List<Rectangle> rectList = new List<Rectangle>();
	//	rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(11, 8, 2, 3));
	//	GenerationHelper.ForEachInRectangle(rectList[0],
	//	(i, j) => {
	//		GenerationHelper.FastPlaceWall(i, j, WallID.Slime);
	//		GenerationHelper.FastPlaceTile(i, j, TileID.SlimeBlock);
	//	});
	//	//Biome.Add(RogueLike_BiomeAreaID.BlueSlime, rectList);
	//}
	//[Task]
	//public void Create_FleshRealm() {
	//	//Minor Biome or soon to be
	//	List<Rectangle> rectList = new List<Rectangle>();
	//	rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(4, 12, 3, 3));
	//	GenerationHelper.ForEachInRectangle(rectList[0], (i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.FleshBlock);
	//		GenerationHelper.FastPlaceWall(i, j, WallID.Flesh);
	//	});
	//	//Biome.Add(RogueLike_BiomeAreaID.FleshRealm, rectList);
	//}
	//[Task]
	//public void Create_TheBoneZone() {
	//	//Minor Biome or soon to be
	//	GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld24x24(0, 22, 3, 2),
	//	(i, j) => {
	//		GenerationHelper.FastPlaceTile(i, j, TileID.BoneBlock);
	//		GenerationHelper.FastPlaceWall(i, j, WallID.Bone);
	//	});
	//}
	//[Task]
	//public void Create_Beach() {
	//	List<Rectangle> rectList = new List<Rectangle>();
	//	rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(0, 5, 3, 2));
	//	GenerationHelper.ForEachInRectangle(rectList[0],
	//	(i, j) => {
	//		if (i <= rectList[0].PointOnRectX(.4f) && j <= rectList[0].PointOnRectY(.4f)) {
	//			GenerationHelper.FastRemoveTile(i, j);
	//		}
	//		else {
	//			GenerationHelper.FastPlaceTile(i, j, TileID.Sand);
	//		}
	//	});
	//	rectList.Add(GenerationHelper.GridPositionInTheWorld24x24(21, 5, 3, 2));
	//	GenerationHelper.ForEachInRectangle(rectList[1],
	//	(i, j) => {
	//		if (i >= rectList[1].PointOnRectX(.6f) && j <= rectList[1].PointOnRectY(.4f)) {
	//			GenerationHelper.FastRemoveTile(i, j);
	//		}
	//		else {
	//			GenerationHelper.FastPlaceTile(i, j, TileID.Sand);
	//		}
	//	});
	//}
	//[Task]
	//public void Create_Path() {
	//	//GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld48x48(0, 23, 23, 1), GenerationHelper.FastRemoveTile);
	//	//GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld48x48(24, 22, 24, 1), GenerationHelper.FastRemoveTile);
	//	//GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld48x48(22, 0, 1, 24), GenerationHelper.FastRemoveTile);
	//	//GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld48x48(26, 23, 1, 22), GenerationHelper.FastRemoveTile);
	//	////Create path from hel-la-va-tor through slime biome to miner paradise
	//	//GenerationHelper.ForEachInRectangle(GenerationHelper.GridPositionInTheWorld48x48(0, 36, 27, 1), GenerationHelper.FastRemoveTile);
	//}
	//[Task]
	//public void Final_CleanUp() {
	//}

}
