﻿using System;
using Terraria;
using System.Linq;
using BossRush.Texture;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace BossRush
{
    public static partial class BossRushUtils
    {
        public static string GetTheSameTextureAsEntity<T>() where T : class
        {
            var type = typeof(T);
            string NameSpace = type.Namespace;
            if (NameSpace == null)
            {
                return BossRushTexture.MISSINGTEXTURE;
            }
            return NameSpace.Replace(".", "/") + "/" + type.Name;
        }
        public static string GetTheSameTextureAs<T>(string altName = "") where T : class
        {
            var type = typeof(T);
            if (string.IsNullOrEmpty(altName))
            {
                altName = type.Name;
            }
            string NameSpace = type.Namespace;
            if (NameSpace == null)
            {
                return BossRushTexture.MISSINGTEXTURE;
            }
            return NameSpace.Replace(".", "/") + "/" + altName;
        }
        public static string GetVanillaTexture<T>(int EntityType) where T : class
        {
            var type = typeof(T);
            if (type == typeof(NPC))
            {
                return "Terraria/Images/NPC_" + EntityType;
            }
            if (type == typeof(Item))
            {
                return "Terraria/Images/Item_" + EntityType;
            }
            if (type == typeof(Projectile))
            {
                return "Terraria/Images/Projectile_" + EntityType;
            }
            return BossRushTexture.MISSINGTEXTURE;
        }
        public static bool IsAnyVanillaBossAlive()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.boss && npc.active)
                {
                    return true;
                }
                else if ((npc.type == NPCID.EaterofWorldsBody
                    || npc.type == NPCID.EaterofWorldsHead
                    || npc.type == NPCID.EaterofWorldsTail)
                    && npc.active)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Use to order 2 values from smallest to biggest
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static (int, int) Order(float v1, float v2) => v1 < v2 ? ((int)v1, (int)v2) : ((int)v2, (int)v1);
        public static bool LookForHostileNPC(this Vector2 position, float distance)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active)
                {
                    if (CompareSquareFloatValue(position, Main.npc[i].Center, distance)) return true;
                }
            }
            return false;
        }
        public static Vector2 LookForHostileNPCPositionClosest(this Vector2 position, float distance)
        {
            List<Vector2> vector2List = new List<Vector2>();
            List<float> ListOfDistance = new List<float>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active 
                    && CompareSquareFloatValue(position, Main.npc[i].Center, distance)
                    && Main.npc[i].CanBeChasedBy()
                    && !Main.npc[i].friendly)
                {
                    vector2List.Add(Main.npc[i].Center);
                    ListOfDistance.Add(Vector2.DistanceSquared(position, Main.npc[i].Center));
                }
            }
            if (vector2List.Count > 0)
            {
                float smallNum = FloatSmallestInList(ListOfDistance);
                //idk why but IndexOf always return 0 so we are searching manually
                for (int i = 0; i < ListOfDistance.Count; i++)
                {
                    if (ListOfDistance[i] == smallNum)
                    {
                        return vector2List[i];
                    }
                }
            }
            return Vector2.Zero;
        }
        public static float FloatSmallestInList(List<float> flag)
        {
            List<float> finalflag = flag;
            for (int i = 0; i < flag.Count;)
            {
            float index = finalflag[i];
                for (int l = i + 1; l < flag.Count; ++l)
                {
                    if (index > flag[l])
                    {
                        index = flag[l];
                    }
                }
                return index;
            }
            return 0;
        }
        public static bool LookForHostileNPC(this Vector2 position, out NPC npc, float distance)
        {
            List<NPC> npcList = new List<NPC>();
            List<Vector2> vector2List = new List<Vector2>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active 
                    && CompareSquareFloatValue(Main.npc[i].Center, position, distance) 
                    && Main.npc[i].CanBeChasedBy()
                    && !Main.npc[i].friendly
                    )
                {
                    npcList.Add(Main.npc[i]);
                    vector2List.Add(position - Main.npc[i].position);
                }
            }
            if (npcList.Count > 0 || vector2List.Count > 0)
            {
                Vector2 closestPos = Vector2SmallestInList(vector2List);
                //idk why but IndexOf always return 0 so we are searching manually
                for (int i = 0; i < vector2List.Count; i++)
                {
                    if (vector2List[i] == closestPos)
                    {
                        npc = npcList[i];
                        return true;
                    }
                }
            }
            npc = null;
            return false;
        }
        public static void LookForHostileNPC(this Vector2 position, out List<NPC> npc, float distance)
        {
            List<NPC> localNPC = new List<NPC>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npcLocal = Main.npc[i];
                if (npcLocal.active 
                    && CompareSquareFloatValue(npcLocal.Center, position, distance) 
                    && npcLocal.CanBeChasedBy() 
                    && npcLocal.friendly)
                {
                    localNPC.Add(npcLocal);
                }
            }
            npc = localNPC;
        }
        public static float InExpo(float t) => (float)Math.Pow(2, 5 * (t - 1));
        public static float OutExpo(float t) => 1 - InExpo(1 - t);
        public static float InOutExpo(float t)
        {
            if (t < 0.5) return InExpo(t * 2) * .5f;
            return 1 - InExpo((1 - t) * 2) * .5f;
        }
        public static float InSine(float t) => (float)-Math.Cos(t * MathHelper.PiOver2);
        public static float OutSine(float t) => (float)Math.Sin(t * MathHelper.PiOver2);
        public static float InOutSine(float t) => (float)(Math.Cos(t * Math.PI) - 1) * -.5f;
        public static float InBack(float t)
        {
            float s = 1.70158f;
            return t * t * ((s + 1) * t - s);
        }
        public static float OutBack(float t) => 1 - InBack(1 - t);
        public static float InOutBack(float t)
        {
            if (t < 0.5) return InBack(t * 2) * .5f;
            return 1 - InBack((1 - t) * 2) * .5f;
        }
        /// <summary>
        /// Calculate square length of Vector2 and check if it is smaller than square max distance
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <param name="maxDistance"></param>
        /// <returns>
        /// Return true if length of Vector2 smaller than max distance<br/>
        /// Return false if length of Vector2 greater than max distance
        /// </returns>
        public static bool CompareSquareFloatValue(Vector2 pos1, Vector2 pos2, float maxDistance)
        {
            double value1X = pos1.X,
                value1Y = pos1.Y,
                value2X = pos2.X,
                value2Y = pos2.Y,
                DistanceX = value1X - value2X,
                DistanceY = value1Y - value2Y,
                maxDistanceDouble = maxDistance * maxDistance;
            return (DistanceX * DistanceX + DistanceY * DistanceY) < maxDistanceDouble;
        }
        public static void DrawTrail(this Projectile projectile, Color lightColor, float ManualScaleAccordinglyToLength = 0)
        {
            Main.instance.LoadProjectile(projectile.type);
            Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, projectile.rotation, origin, projectile.scale - k * ManualScaleAccordinglyToLength, SpriteEffects.None, 0);
            }
        }
        public static List<int> RemoveDupeInList(this List<int> flag)
        {
            HashSet<int> HashsetRemoveDup = new(flag);
            return HashsetRemoveDup.ToList();
        }
    }
}