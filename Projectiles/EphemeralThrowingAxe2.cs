﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Projectiles {
    class EphemeralThrowingAxe2 : ModProjectile {

        public override string Texture => "tsorcRevamp/Items/Weapons/Melee/EphemeralThrowingAxe";

        public override void SetDefaults() {
            projectile.aiStyle = 2;
            projectile.friendly = true;
            projectile.height = 22;
            projectile.penetrate = 1;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.width = 22;
            projectile.timeLeft = 50;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            //todo add mod NPCs to this list
            if (target.type == NPCID.Tim
                || target.type == NPCID.DarkCaster
                || target.type == NPCID.GoblinSorcerer
                //|| target.type == ModContent.NPCType<UndeadCaster>()
                //|| target.type == ModContent.NPCType<MindflayerServant>()
                //|| target.type == ModContent.NPCType<DungeonMage>()
                //|| target.type == ModContent.NPCType<DemonSpirit>()
                //|| target.type == ModContent.NPCType<CrazedDemonSpirit>()
                //|| target.type == ModContent.NPCType<ShadowMage>()
                //|| target.type == ModContent.NPCType<AttraidiesIllusion>()
                //|| target.type == ModContent.NPCType<AttraidiesManifestation>()
                //|| target.type == ModContent.NPCType<MindflayerKing>()
                //|| target.type == ModContent.NPCType<DarkShogunMask>()
                //|| target.type == ModContent.NPCType<DarkDragonMask>()
                //|| target.type == ModContent.NPCType<BrokenOkiku>()
                //|| target.type == ModContent.NPCType<Okiku>()
                //|| target.type == ModContent.NPCType<WyvernMage>()
                //|| target.type == ModContent.NPCType<LichKingDisciple>()
                //|| target.type == ModContent.NPCType<Attraidies>()
                //|| target.type == ModContent.NPCType<GhostOfTheForgottenKnight>()
                //|| target.type == ModContent.NPCType<GhostOfTheForgottenWarrior>()
                //|| target.type == ModContent.NPCType<BarrowWight>()
                ) {
                damage *= 2;
            }
        }
        public override void AI() {
            Color color = new Color();
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 57, 0f, 0f, 80, color, 1f);
            Main.dust[dust].noGravity = true;
        }
        public override void Kill(int timeLeft) {

            if (!projectile.active) {
                return;
            }
            projectile.timeLeft = 0;
            {
                Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y, 1);
                for (int i = 0; i < 10; i++) {
                    Vector2 arg_92_0 = new Vector2(projectile.position.X, projectile.position.Y);
                    int arg_92_1 = projectile.width;
                    int arg_92_2 = projectile.height;
                    int arg_92_3 = 7;
                    float arg_92_4 = 0f;
                    float arg_92_5 = 0f;
                    int arg_92_6 = 0;
                    Color newColor = default(Color);
                    Dust.NewDust(arg_92_0, arg_92_1, arg_92_2, arg_92_3, arg_92_4, arg_92_5, arg_92_6, newColor, 1f);
                }
            }
            projectile.active = false;

        }
    }
}
