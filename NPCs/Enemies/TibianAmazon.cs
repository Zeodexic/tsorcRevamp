﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.NPCs.Enemies
{
    class TibianAmazon : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tibian Amazon");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Skeleton];
        }

        public int throwingKnifeDamage = 8;

        public override void SetDefaults()
        {



            AnimationType = NPCID.Skeleton;
            NPC.aiStyle = -1;
            NPC.height = 40;
            NPC.width = 20;
            NPC.lifeMax = 85;
            NPC.damage = 20;
            NPC.scale = 1f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = .6f;
            NPC.value = 250;
            NPC.defense = 2;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.TibianAmazonBanner>();

            if (Main.hardMode)
            {
                NPC.lifeMax = 180;
                NPC.defense = 16;
                NPC.value = 320;
                NPC.damage = 50;
                throwingKnifeDamage = 20;
            }
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ItemID.Torch);
            Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ItemID.ThrowingKnife, Main.rand.Next(20, 50));
            if (!Main.hardMode && Main.rand.Next(10) == 0) Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<Items.Armors.RedMageTunic>());
            if (!Main.hardMode && Main.rand.Next(10) == 0) Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<Items.Armors.RedMagePants>());
            if (!Main.hardMode && Main.rand.Next(10) == 0) Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<Items.Armors.RedMageHat>());
            if (!Main.hardMode && Main.rand.Next(10) == 0) Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<Items.Weapons.Melee.OldDoubleAxe>(), 1, false, -1);
            if (Main.rand.Next(20) == 0) Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<Items.DeadChicken>());
        }




        //Spawns on the Surface and into the Underground. Does not spawn in the Dungeon, Hardmode, Meteor, or if there are Town NPCs.

        #region Spawn

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float chance = 0;

            if (spawnInfo.Invasion)
            {
                chance = 0;
                return chance;
            }

            if (spawnInfo.Player.townNPCs > 0f || tsorcRevampWorld.SuperHardMode || spawnInfo.Player.ZoneDungeon) chance = 0f;
            if (!tsorcRevampWorld.SuperHardMode && (spawnInfo.Player.ZoneOverworldHeight || spawnInfo.Player.ZoneDirtLayerHeight || spawnInfo.Player.ZoneRockLayerHeight))
            {
                if (!(spawnInfo.Player.ZoneCorrupt || spawnInfo.Player.ZoneCrimson)) return 0.05f;
                if (!(spawnInfo.Player.ZoneCorrupt || spawnInfo.Player.ZoneCrimson) && !Main.dayTime) return 0.055f;
                if (!(spawnInfo.Player.ZoneCorrupt || spawnInfo.Player.ZoneCrimson) && Main.dayTime) return 0.0534f;
                if (spawnInfo.Player.ZoneMeteor && !Main.dayTime) return 0.0725f;
            }

            return chance;
        }
        #endregion

        float knifeTimer = 0;
        public override void AI()
        {
            tsorcRevampAIs.FighterAI(NPC, 1.8f, 0.15f, enragePercent: 0.2f, enrageTopSpeed: 2.2f);
            tsorcRevampAIs.SimpleProjectile(NPC, ref knifeTimer, 90, ModContent.ProjectileType<Projectiles.Enemy.EnemyThrowingKnife>(), throwingKnifeDamage, 8, shootSound: SoundID.Item17);

            if (NPC.justHit)
            {
                knifeTimer = 60;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (knifeTimer >= 60)
            {
                Texture2D knifeTexture = (Texture2D)Mod.Assets.Request<Texture2D>("NPCs/Enemies/TibianAmazon_Knife");
                SpriteEffects effects = NPC.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                if (NPC.spriteDirection == -1)
                {
                    spriteBatch.Draw(knifeTexture, NPC.Center - Main.screenPosition, new Rectangle(NPC.frame.X, NPC.frame.Y, 60, 56), drawColor, NPC.rotation, new Vector2(30, 32), NPC.scale, effects, 0);
                }
                else
                {
                    spriteBatch.Draw(knifeTexture, NPC.Center - Main.screenPosition, new Rectangle(NPC.frame.X, NPC.frame.Y, 60, 56), drawColor, NPC.rotation, new Vector2(30, 32), NPC.scale, effects, 0);
                }
            }
        }

        #region Gore
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 25; i++)
            {
                int DustType = 5;
                int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.06f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.06f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                dust.noGravity = true;
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, Main.rand.Next(-3, 3), Main.rand.Next(-3, 3), 70, default(Color), 1f);
                }

                if (!Main.dedServ)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Tibian Amazon Gore 1").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Tibian Amazon Gore 2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Tibian Amazon Gore 3").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Tibian Amazon Gore 2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Tibian Amazon Gore 3").Type, 1f);
                }
            }
        }
        #endregion
    }
}