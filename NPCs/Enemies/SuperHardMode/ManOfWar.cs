﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.NPCs.Enemies.SuperHardMode {
    class ManOfWar : ModNPC {
        public override void SetDefaults() {
            npc.npcSlots = 1;
            npc.width = 26;
            npc.height = 26;
            Main.npcFrameCount[npc.type] = 7;
            animationType = NPCID.GreenJellyfish;
            npc.aiStyle = 18;
            npc.timeLeft = 750;
            npc.damage = 120;
            npc.defense = 40;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.lifeMax = 2000;
            npc.alpha = 20;
            npc.scale = .7f;
            npc.knockBackResist = 0.3f;
            npc.noGravity = true;
            npc.value = 1250;
            npc.buffImmune[BuffID.Confused] = true;
            npc.buffImmune[BuffID.Frozen] = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            if (tsorcRevampWorld.SuperHardMode && spawnInfo.water) {
                return 0.5f;
            }
            return 0;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit) {
            if (Main.rand.Next(2) == 0) {
                target.AddBuff(BuffID.PotionSickness, 3600); //evil! pure evil!
            }
        }
        public override void HitEffect(int hitDirection, double damage) {
            if (npc.life <= 0) {
                Dust.NewDust(npc.position, npc.width, npc.height, 71, 0.3f, 0.3f, 200, default, 1f);
                Dust.NewDust(npc.position, npc.height, npc.width, 71, 0.2f, 0.2f, 200, default, 2f);
                Dust.NewDust(npc.position, npc.width, npc.height, 71, 0.2f, 0.2f, 200, default, 2f);
                Dust.NewDust(npc.position, npc.height, npc.width, 71, 0.2f, 0.2f, 200, default, 3f);
                Dust.NewDust(npc.position, npc.height, npc.width, 71, 0.2f, 0.2f, 200, default, 2f);
                Dust.NewDust(npc.position, npc.width, npc.height, 71, 0.2f, 0.2f, 200, default, 2f);
                Dust.NewDust(npc.position, npc.height, npc.width, 71, 0.2f, 0.2f, 200, default, 2f);
                Dust.NewDust(npc.position, npc.height, npc.width, 71, 0.2f, 0.2f, 200, default, 2f);
                Dust.NewDust(npc.position, npc.height, npc.width, 71, 0.2f, 0.2f, 200, default, 2f);
            }
        }
    }
}
