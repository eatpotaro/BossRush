﻿using BossRush.Common.Utils;
using BossRush.Contents.Items.Chest;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.NPCs {
	//To ensure the code is readable and consistent throughout the process of making
	//ai[0] => timer
	//ai[1] => ai switch
	//ai[2] => counter ( not to be confused with timer )
	//The code in this file must follow the above rule
	internal class LootBoxLord : ModNPC {
		public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<WoodenLootBox>();
		public override void SetDefaults() {
			NPC.lifeMax = 6000;
			NPC.damage = 20;
			NPC.defense = 20;
			NPC.width = 60;
			NPC.height = 60;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.npcSlots = 6f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.netAlways = true;
			NPC.knockBackResist = 0f;
			NPC.boss = true;
		}
		//Use NPC.ai[0] to delay attack
		//Use NPC.ai[1] to switch attack
		//Use NPC.ai[2] for calculation
		//Use NPC.ai[3] to do movement
		public override void AI() {
			Player player = Main.player[NPC.target];
			if (player.dead || !player.active) {
				NPC.active = false;
			}
			//Move above the player
			switch (NPC.ai[1]) {
				case 0:
					Move(player);
					break;
				case 1:
					ShootShortSword();
					break;
				case 2:
					ShootShortSword2();
					break;
				case 3:
					ShootBroadSword();
					break;
				case 4:
					ShootBroadSword2();
					break;
				case 5:
					ShootWoodBow();
					break;
				case 6:
					ShootWoodBow2();
					break;
				case 7:
					ShootStaff(player);
					break;
				case 8:
					ShootStaff2();
					break;
			}
			ActivateBroadSword();
		}
		/// <summary>
		/// This is a way to make NPC itself handle it own projectile and activate as will
		/// Make sure there are no junk data or overlapped attack
		/// </summary>
		private void ActivateBroadSword() {
			List<SwordBroadAttackOne> broadSwordProjectile = new List<SwordBroadAttackOne>();
			for (int i = 0; i < ProjectileWhoAmI.Count; i++) {
				Projectile projectile = Main.projectile[ProjectileWhoAmI[i]];
				if (projectile.ModProjectile is SwordBroadAttackOne swordProj && projectile.ai[1] >= 3) {
					broadSwordProjectile.Add(swordProj);
				}
			}
			if (broadSwordProjectile.Count >= TerrariaArrayID.AllOreBroadSword.Length) {
				foreach (SwordBroadAttackOne proj in broadSwordProjectile) {
					proj.CanProgressToAI3 = true;
				}
				//We clean junk data here
				//Since we have proven that all of them are here, and most likely this attack won't change in term of number
				//We should clear the list so we can reuse the list
				ProjectileWhoAmI.Clear();
			}
		}
		public override void OnKill() {
			foreach (var projIndex in ProjectileWhoAmI) {
				Projectile projectile = Main.projectile[projIndex];
				if (projectile == null) {
					continue;
				}
				if (projectile.active) {
					projectile.Kill();
				}
				ProjectileWhoAmI.Clear();
			}
		}
		List<int> ProjectileWhoAmI = new List<int>();
		private void Move(Player player) {
			if (BossDelayAttack(0, 0, 0)) {
				return;
			}
			Vector2 positionAbovePlayer = new Vector2(player.Center.X, player.Center.Y - 300);
			if (NPC.NPCMoveToPosition(positionAbovePlayer, 30f)) {
				NPC.ai[0] = 20;
				NPC.ai[1] = MoveSetHandle();
			}
		}
		private int MoveSetHandle() {
			int Move = (int)Math.Clamp(++NPC.ai[3], 1, 9);
			if (Move >= 9) {
				Move = 1;
				NPC.ai[3] = 1;
			}
			return Move;
		}
		private void ResetEverything() {
			NPC.ai[0] = 90;
			NPC.ai[1] = 0;
			NPC.ai[2] = 0;
		}
		private void ShootShortSword() {
			if (BossDelayAttack(10, 0, TerrariaArrayID.AllOreShortSword.Length - 1)) {
				return;
			}
			Vector2 vec = -Vector2.UnitY.Vector2DistributeEvenly(8, 120, (int)NPC.ai[2]) * 15f;
			int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<ShortSwordAttackOne>(), NPC.damage, 2);
			if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
				projectile.ItemIDtextureValue = TerrariaArrayID.AllOreShortSword[(int)NPC.ai[2]];
			Main.projectile[proj].owner = NPC.target;
			NPC.ai[2]++;
		}
		private void ShootShortSword2() {
			Vector2 positionAbovePlayer = new Vector2(Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y - 350);
			NPC.NPCMoveToPosition(positionAbovePlayer, 5f);
			if (NPC.ai[2] >= TerrariaArrayID.AllOreShortSword.Length - 1) {
				ResetEverything();
				return;
			}
			if (BossDelayAttack(20, 0, TerrariaArrayID.AllOreShortSword.Length - 1)) {
				return;
			}
			Vector2 vec = Vector2.UnitX * 20 * Main.rand.NextBool(2).BoolOne();
			int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<ShortSwordAttackTwo>(), NPC.damage, 2);
			if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
				projectile.ItemIDtextureValue = TerrariaArrayID.AllOreShortSword[(int)NPC.ai[2]];
			Main.projectile[proj].ai[1] = -20;
			Main.projectile[proj].ai[0] = 2;
			Main.projectile[proj].owner = NPC.target;
			Main.projectile[proj].rotation = Main.projectile[proj].velocity.ToRotation() + MathHelper.PiOver4;
			NPC.ai[2]++;
		}
		private void ShootBroadSword() {
			if (BossDelayAttack(0, 0, 0)) {
				return;
			}
			for (int i = 0; i < TerrariaArrayID.AllOreBroadSword.Length; i++) {
				Vector2 vec = -Vector2.UnitY.Vector2DistributeEvenlyPlus(TerrariaArrayID.AllOreBroadSword.Length, 160, i) * 20f;
				int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<SwordBroadAttackOne>(), NPC.damage, 2);
				if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
					projectile.ItemIDtextureValue = TerrariaArrayID.AllOreBroadSword[i];
				Main.projectile[proj].owner = NPC.target;
				if (Main.projectile[proj].ModProjectile is SwordBroadAttackOne swordProj) {
					swordProj.OnSpawnDirection = vec.X > 0 ? 1 : -1;
					swordProj.rememberThisPos = Main.player[NPC.target].Center + new Vector2(250 * swordProj.OnSpawnDirection, -120 + 40 * i);
					ProjectileWhoAmI.Add(proj);
				}
			}
			NPC.ai[2]++;
			BossDelayAttack(0, 0, 0, 30);
		}
		private void ShootBroadSword2() {
			NPC.ai[0] = 0;
			if (BossDelayAttack(0, 0, 0)) {
				return;
			}
			for (int i = 0; i < TerrariaArrayID.AllOreBroadSword.Length; i++) {
				Vector2 vec = -Vector2.UnitY.Vector2DistributeEvenlyPlus(TerrariaArrayID.AllOreBroadSword.Length, 180, i) * 50f;
				vec.Y = 5;
				int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<SwordBroadAttackTwo>(), NPC.damage, 2);
				Main.projectile[proj].ai[1] = 35;
				if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
					projectile.ItemIDtextureValue = TerrariaArrayID.AllOreBroadSword[i];
				Main.projectile[proj].owner = NPC.target;
			}
			for (int i = 0; i < TerrariaArrayID.AllOreBroadSword.Length; i++) {
				Vector2 vec = -Vector2.UnitY.Vector2DistributeEvenlyPlus(TerrariaArrayID.AllOreBroadSword.Length, 90, i) * 20f;
				int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<SwordBroadAttackTwo>(), NPC.damage, 2);
				Main.projectile[proj].ai[1] = 50;
				if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
					projectile.ItemIDtextureValue = TerrariaArrayID.AllOreBroadSword[i];
				Main.projectile[proj].owner = NPC.target;
			}
			NPC.ai[2]++;
			BossDelayAttack(0, 0, 0);

		}
		private void ShootWoodBow() {
			if (BossDelayAttack(0, 0, 0)) {
				return;
			}
			int length = TerrariaArrayID.AllWoodBowPHM.Length;
			for (int i = 0; i < length; i++) {
				if (i < length / 2) {
					Vector2 velocity = (Vector2.UnitX * 2).Vector2DistributeEvenlyPlus(3, 45, i);
					int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<WoodBowAttackOne>(), NPC.damage, 2);
					if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
						projectile.ItemIDtextureValue = TerrariaArrayID.AllWoodBowPHM[i];
					Main.projectile[proj].owner = NPC.target;
					if (i == length / 2 - 1)
						Main.projectile[proj].ai[1] = -MathHelper.PiOver4;
					if (i == 0)
						Main.projectile[proj].ai[1] = MathHelper.PiOver4;
				}
				else {
					Vector2 velocity = (-Vector2.UnitX * 2).Vector2DistributeEvenlyPlus(3, 45, i - 3);
					int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<WoodBowAttackOne>(), NPC.damage, 2);
					if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
						projectile.ItemIDtextureValue = TerrariaArrayID.AllWoodBowPHM[i];
					Main.projectile[proj].owner = NPC.target;
					if (i == length - 1)
						Main.projectile[proj].ai[1] = -MathHelper.PiOver4;
					if (i == length / 2)
						Main.projectile[proj].ai[1] = MathHelper.PiOver4;
				}
			}
			NPC.ai[2]++;
			BossDelayAttack(0, 0, 240);

		}
		private void ShootWoodBow2() {
			if (BossDelayAttack(5, 0, TerrariaArrayID.AllWoodBowPHM.Length - 1)) {
				return;
			}
			Vector2 vec = Vector2.UnitY.Vector2DistributeEvenlyPlus(TerrariaArrayID.AllWoodBowPHM.Length, 120, (int)NPC.ai[2]) * 3f;
			int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<WoodBowAttackTwo>(), NPC.damage, 2);
			if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
				projectile.ItemIDtextureValue = TerrariaArrayID.AllWoodBowPHM[(int)NPC.ai[2]];
			Main.projectile[proj].owner = NPC.target;
			NPC.ai[2]++;
		}
		private void ShootStaff(Player player) {
			BossCircleMovement(player, 5, TerrariaArrayID.AllGemStaffPHM.Length, out float percent);
			if (BossDelayAttack(5, 0, TerrariaArrayID.AllGemStaffPHM.Length - 1)) {
				return;
			}
			Vector2 vec = Vector2.UnitY.RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(0, 360, percent)));
			int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, vec, ModContent.ProjectileType<GemStaffAttackOne>(), NPC.damage, 2);
			if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
				projectile.ItemIDtextureValue = TerrariaArrayID.AllGemStaffPHM[(int)NPC.ai[2]];
			Main.projectile[proj].owner = NPC.target;
			if (Main.projectile[proj].ModProjectile is BaseHostileGemStaff gemstaffProj)
				gemstaffProj.ProjectileType = TerrariaArrayID.AllGemStafProjectilePHM[(int)NPC.ai[2]];
			NPC.ai[2]++;
		}
		private void ShootStaff2() {
			if (BossDelayAttack(120, 0, 0)) {
				return;
			}
			for (int i = 0; i < TerrariaArrayID.AllGemStaffPHM.Length; i++) {
				float lerpXPos = MathHelper.Lerp(-10, 10, i / (TerrariaArrayID.AllGemStaffPHM.Length - 1f));
				Vector2 PosTo = new Vector2(lerpXPos, -2);
				int proj = BossRushUtils.NewHostileProjectile(NPC.GetSource_FromAI(), NPC.Center, PosTo, ModContent.ProjectileType<GemStaffAttackTwo>(), NPC.damage, 2);
				if (Main.projectile[proj].ModProjectile is BaseHostileProjectile projectile)
					projectile.ItemIDtextureValue = TerrariaArrayID.AllGemStaffPHM[i];
				Main.projectile[proj].ai[2] = i;
				Main.projectile[proj].owner = NPC.target;
				Main.projectile[proj].rotation = MathHelper.PiOver4 + MathHelper.PiOver2;
				if (Main.projectile[proj].ModProjectile is BaseHostileGemStaff gemstaffProj)
					gemstaffProj.ProjectileType = TerrariaArrayID.AllGemStafProjectilePHM[i];
			}
			NPC.ai[2]++;
		}
		private void BossCircleMovement(Player player, int delayValue, int AttackEndValue, out float percent) {
			float total = delayValue * AttackEndValue;
			percent = Math.Clamp(((delayValue - NPC.ai[0] >= 0 ? delayValue - NPC.ai[0] : 0) + delayValue * NPC.ai[2]) / total, 0, 1f);
			float rotation = MathHelper.Lerp(0, 360, percent);
			Vector2 rotateAroundPlayerCenter = player.Center - Vector2.UnitY.RotatedBy(MathHelper.ToRadians(rotation)) * 350;
			NPC.Center = rotateAroundPlayerCenter;
		}

		/// <summary>
		/// This is to ensure boss have a certain delay
		/// </summary>
		/// <param name="delaytime">the delay between each attack, use if you want to shoot out projectile individually or have a space out</param>
		/// <param name="nextattack">Will set the next attack</param>
		/// <param name="whenAttackwillend">determined whenever the attack will end</param>
		/// <param name="additionalDelay">the post delay after the attack is done</param>
		/// <returns></returns>
		private bool BossDelayAttack(float delaytime, float nextattack, float whenAttackwillend, int additionalDelay = 0) {
			//This only run whenever a delay is given but only when the timer reach 0 or below
			if (NPC.ai[0] <= 0) {
				NPC.ai[0] += delaytime;
			}
			else {
				//The timer decrease
				NPC.ai[0]--;
				return true;
			}
			//this will check if the counter (NPC.ai[2]) reach the requirement to reset everything
			if (NPC.ai[2] > whenAttackwillend) {
				ResetEverything();
				NPC.ai[0] += additionalDelay;
				NPC.ai[1] = nextattack;
				return true;
			}
			return false;
		}
	}
	//This code did not follow the above rule and it should be change to follow the above rule
	public abstract class BaseHostileProjectile : ModProjectile {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public int ItemIDtextureValue = 1;
		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetVanillaTexture<Item>(ItemIDtextureValue)).Value;
			Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
	public abstract class BaseHostileShortSword : BaseHostileProjectile {
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 32;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}
		protected int OnSpawnDirection = 0;
		public override void OnSpawn(IEntitySource source) {
			OnSpawnDirection = Projectile.velocity.X > 0 ? 1 : -1;
			base.OnSpawn(source);
		}
	}
	class ShortSwordAttackOne : BaseHostileShortSword {
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if (Projectile.ai[0] == 1) {
				if (Projectile.timeLeft > 30)
					Projectile.velocity += (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
				if (Projectile.timeLeft > 160)
					Projectile.timeLeft = 160;
				return;
			}
			if (Projectile.velocity.IsLimitReached(3)) {
				Projectile.velocity -= Projectile.velocity * .05f;
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			}
			else {
				Projectile.ai[0] = 1;
			}
		}
	}
	class ShortSwordAttackTwo : BaseHostileShortSword {
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			Vector2 LeftOfPlayer = new Vector2(player.Center.X + 400 * OnSpawnDirection, player.Center.Y);
			if (Projectile.ai[1] < 0) {
				Projectile.ai[1]++;
				return;
			}
			if (Projectile.ai[1] == 0) {
				if (!Projectile.Center.IsCloseToPosition(LeftOfPlayer, 10f)) {
					Vector2 distance = LeftOfPlayer - Projectile.Center;
					float length = distance.Length();
					if (length > 5) {
						length = 5;
					}
					Projectile.velocity -= Projectile.velocity * .08f;
					Projectile.velocity += distance.SafeNormalize(Vector2.Zero) * length;
					Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
				}
				else {
					Projectile.velocity = -Vector2.UnitX * OnSpawnDirection;
					Projectile.timeLeft = 150;
					Projectile.ai[1] = 1;
					Projectile.Center = LeftOfPlayer;
				}
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			}
			else {
				Projectile.ai[1]++;
				if (Projectile.ai[1] >= 60) {
					if (Projectile.ai[1] >= 75) {
						Projectile.velocity -= Vector2.UnitX * 2 * OnSpawnDirection;
						return;
					}
					Projectile.velocity += Vector2.UnitX * OnSpawnDirection;
					return;
				}
			}
		}
	}
	public abstract class BaseHostileSwordBroad : BaseHostileProjectile {
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 36;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}
	}
	class SwordBroadAttackOne : BaseHostileSwordBroad {
		public int OnSpawnDirection = 0;
		public bool CanProgressToAI3 = false;
		public Vector2 rememberThisPos = Vector2.Zero;
		public override void AI() {
			if (Projectile.ai[1] == 2) {
				if (!Projectile.Center.IsCloseToPosition(rememberThisPos, 20f)) {
					Vector2 distance = rememberThisPos - Projectile.Center;
					float length = distance.Length();
					if (length > 2) {
						length = 2;
					}
					Projectile.velocity -= Projectile.velocity * .08f;
					Projectile.velocity += distance.SafeNormalize(Vector2.Zero) * length;
					Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
				}
				else {
					Projectile.Center = rememberThisPos;
					Projectile.velocity = Vector2.Zero;
					Projectile.ai[1]++;
				}
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
				return;
			}
			if (Projectile.ai[1] >= 3) {
				Vector2 newVel = Vector2.UnitX * OnSpawnDirection;
				if (CanProgressToAI3) {
					Projectile.ai[1]++;
					if (Projectile.ai[1] >= 30) {
						Projectile.velocity -= newVel * 2;
					}
					else if (Projectile.ai[1] >= 20) {
						Projectile.velocity += newVel;
					}
					if (Projectile.timeLeft > 40)
						Projectile.timeLeft = 120;
				}
				Projectile.rotation = newVel.ToRotation() + MathHelper.PiOver4 + MathHelper.Pi;
				return;
			}

			if (Projectile.velocity.IsLimitReached(7)) {
				Projectile.velocity -= Projectile.velocity * .05f;
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			}
			else {
				Projectile.ai[1] = 2;
				Projectile.velocity = Vector2.Zero;
			}
		}
	}
	class SwordBroadAttackTwo : BaseHostileSwordBroad {
		public override void AI() {
			Projectile.rotation = MathHelper.PiOver4 + MathHelper.PiOver2;
			if (Projectile.ai[1] == 1) {
				if (Projectile.timeLeft > 30)
					Projectile.timeLeft = 30;
				Projectile.velocity.Y = 50;
				Projectile.velocity.X = 0;
				return;
			}
			if (Projectile.ai[1] > 1) {
				Projectile.velocity.Y += -.5f;
				Projectile.ai[1]--;
				Projectile.velocity -= Projectile.velocity * .1f;
			}
			else {
				Projectile.ai[1] = 1;
				Projectile.velocity = Vector2.Zero;
			}
		}
	}
	public abstract class BaseHostileWoodBow : BaseHostileProjectile {
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 32;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
		}
	}
	class WoodBowAttackOne : BaseHostileWoodBow {
		public override void AI() {
			if (Projectile.timeLeft > 150)
				Projectile.timeLeft = 150;
			Projectile.rotation = -MathHelper.PiOver2 + MathHelper.Pi + Projectile.ai[1];
			if (++Projectile.ai[0] >= 35) {
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 10f, ProjectileID.WoodenArrowHostile, Projectile.damage, 1);
				Projectile.ai[0] = 0;
			}
		}
	}
	class WoodBowAttackTwo : BaseHostileWoodBow {
		Vector2 toPlayer = Vector2.Zero;
		public override void AI() {
			int Requirement = 35;
			if (Projectile.ai[1] <= 0)
				Projectile.rotation = Projectile.velocity.ToRotation();
			else {
				toPlayer = (Main.player[Projectile.owner].Center - Projectile.Center).SafeNormalize(Vector2.Zero);
				Projectile.rotation = toPlayer.ToRotation();
				Requirement += 15;
			}
			if (Projectile.timeLeft > 90)
				Projectile.timeLeft = 90;
			if (++Projectile.ai[0] >= Requirement) {
				if (Projectile.ai[1] <= 0) {
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 10f, ProjectileID.WoodenArrowHostile, Projectile.damage, 1);
				}
				else {
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, toPlayer * 10f, ProjectileID.WoodenArrowHostile, Projectile.damage, 1);
				}
				Projectile.ai[0] = 0;
				Projectile.ai[1]++;
			}
			Projectile.velocity -= Projectile.velocity * .05f;
		}
	}
	public abstract class BaseHostileGemStaff : BaseHostileProjectile {
		public int ProjectileType = ProjectileID.AmethystBolt;
		public override void SetDefaults() {
			Projectile.width = 40;
			Projectile.height = 42;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
		}
	}
	class GemStaffAttackOne : BaseHostileGemStaff {
		public override void AI() {
			if (Projectile.timeLeft > 180)
				Projectile.timeLeft = 180;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 + MathHelper.ToRadians(Projectile.ai[1] - 70);
			if (++Projectile.ai[0] >= 35) {
				BossRushUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 10f, ProjectileType, Projectile.damage, 1);
				Projectile.ai[0] = 0;
			}
			Projectile.ai[1] += 2;
		}
	}
	class GemStaffAttackTwo : BaseHostileGemStaff {
		//Projectile.ai[2] act as projectile index
		public override void AI() {
			if (Projectile.velocity.IsLimitReached(.1f)) {
				Projectile.velocity -= Projectile.velocity * .04f;
				return;
			}
			else {
				Projectile.velocity = Vector2.Zero;
			}
			Projectile.rotation = (Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.PiOver4;
			if (Projectile.timeLeft > 300)
				Projectile.timeLeft = 300;
			if (++Projectile.ai[0] >= 40) {
				BossRushUtils.NewHostileProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 10f, ProjectileType, Projectile.damage, 1);
				Projectile.ai[0] = 0;
				Projectile.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * -2f;
			}
		}
	}
}