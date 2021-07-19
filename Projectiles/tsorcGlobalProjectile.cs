﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Projectiles.Pets;

namespace tsorcRevamp.Projectiles
{
    class tsorcGlobalProjectile : GlobalProjectile
    {
        public override bool PreAI(Projectile projectile) {
            Player player = Main.player[projectile.owner];
            tsorcRevampPlayer modPlayer = player.GetModPlayer<tsorcRevampPlayer>();


            if (projectile.owner == Main.myPlayer && !projectile.hostile && modPlayer.MiakodaCrescentBoost && !(projectile.type == (int)ModContent.ProjectileType<MiakodaCrescent>() || projectile.type == (int)ModContent.ProjectileType<ShulletBellDark>() || projectile.type == (int)ModContent.ProjectileType<ShulletBellLight>() || projectile.type == (int)ModContent.ProjectileType<Bloodsign>())) {
                if (Main.rand.Next(2) == 0) {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 164, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 30, default(Color), 1f);
                    Main.dust[dust].noGravity = false;
                }
            }

            if (projectile.owner == Main.myPlayer && !projectile.hostile && modPlayer.MiakodaNewBoost && !(projectile.type == (int)ModContent.ProjectileType<MiakodaNew>() || projectile.type == (int)ModContent.ProjectileType<ShulletBellDark>() || projectile.type == (int)ModContent.ProjectileType<ShulletBellLight>() || projectile.type == (int)ModContent.ProjectileType<Bloodsign>())) {
                if (Main.rand.Next(2) == 0) {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 57, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 130, default(Color), 1f);
                    Main.dust[dust].noGravity = true;
                }
            }
            if (projectile.owner == Main.myPlayer && !projectile.hostile && projectile.melee)
            {
                if (modPlayer.MagicWeapon)
                {
                    Lighting.AddLight(projectile.position, 0.3f, 0.3f, 0.45f);
                    for (int i = 0; i < 4; i++)
                    {
                        int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 68, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 30, default(Color), .9f);
                        Main.dust[dust].noGravity = true;
                    }
                    {
                        int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 15, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 30, default(Color), .9f);
                        Main.dust[dust].noGravity = true;
                    }
                }

                if (modPlayer.GreatMagicWeapon)
                {
                    Lighting.AddLight(projectile.position, 0.3f, 0.3f, 0.55f);
                    for (int i = 0; i < 4; i++)
                    {
                        int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 30, default(Color), .9f);
                        Main.dust[dust].noGravity = true;
                    }
                    {
                        int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 68, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 30, default(Color), .9f);
                        Main.dust[dust].noGravity = true;
                    }
                    {
                        int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172, player.velocity.X * .2f, player.velocity.Y * 0.2f, 30, default(Color), 1.3f);
                        Main.dust[dust].noGravity = true;
                    }
                }

                if (modPlayer.CrystalMagicWeapon)
                {
                    Lighting.AddLight(projectile.position, 0.3f, 0.3f, 0.55f);
                    for (int i = 0; i < 2; i++)
                    {
                        int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 221, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 30, default(Color), .9f);
                        Main.dust[dust].noGravity = true;
                    }
                    {
                        int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 68, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 30, default(Color), .9f);
                        Main.dust[dust].noGravity = true;
                    }
                    {
                        int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172, player.velocity.X * .2f, player.velocity.Y * 0.2f, 30, default(Color), 1.3f);
                        Main.dust[dust].noGravity = true;
                    }
                }
            }
            return true;
        }
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit) {
            Player player = Main.player[projectile.owner];
            tsorcRevampPlayer modPlayer = player.GetModPlayer<tsorcRevampPlayer>();

            if (projectile.owner == Main.myPlayer && !projectile.hostile && modPlayer.MiakodaCrescentBoost && projectile.type != (int)ModContent.ProjectileType<MiakodaCrescent>()) {
                target.AddBuff(ModContent.BuffType<Buffs.CrescentMoonlight>(), 180); // Adds the ExampleJavelin debuff for a very small DoT
            }

            if (projectile.owner == Main.myPlayer && !projectile.hostile && modPlayer.MiakodaNewBoost && projectile.type != (int)ModContent.ProjectileType<MiakodaNew>()) {
                target.AddBuff(BuffID.Midas, 300);
            }

            if (projectile.owner == Main.myPlayer && (modPlayer.MagicWeapon || modPlayer.GreatMagicWeapon) && projectile.melee) {
                Main.PlaySound(SoundID.NPCHit44.WithVolume(0.3f), target.position);
            }
            if (projectile.owner == Main.myPlayer && modPlayer.CrystalMagicWeapon && projectile.melee)
            {
                Main.PlaySound(SoundID.Item27.WithVolume(0.3f), target.position);
            }
        }
    }
}
