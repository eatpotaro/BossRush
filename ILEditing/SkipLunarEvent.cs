﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.ILEditing;
public class SkipLunarEvent : ModSystem {

	public override void Load() {

		IL_NPC.DoDeathEvents += HookDoDeathEvents;

	}

	private void HookDoDeathEvents(ILContext il) {
		try {

			ILCursor c = new ILCursor(il);
			c.GotoNext(i => i.MatchCall(typeof(WorldGen).GetMethod("TriggerLunarApocalypse")));
			c.Remove();
		}
		catch (Exception e) {

			MonoModHooks.DumpIL(ModContent.GetInstance<BossRush>(), il);
		}
	}


}