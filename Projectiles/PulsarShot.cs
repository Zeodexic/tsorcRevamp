﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;


namespace tsorcRevamp.Projectiles
{
    class PulsarShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 8;
        }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.aiStyle = 0;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 90;
            projectile.penetrate = -1;

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
           if (projectile.owner == Main.myPlayer && projectile.timeLeft <= 50)
           {
                if (Main.rand.Next(4) == 0)
                {
                    target.AddBuff(mod.BuffType("ElectrocutedBuff"), 180);
                }
                projectile.timeLeft = 2;
           }

           else
           {
                projectile.tileCollide = true;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
                projectile.width = 10;
                projectile.height = 10;
                projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
                projectile.damage = (int)(originalDamage * 1f);
                projectile.knockBack = 2f;
                projectile.ranged = true;

                if (Main.rand.Next(4) == 0)
                {
                    target.AddBuff(mod.BuffType("ElectrocutedBuff"), 120);
                }

                projectile.timeLeft = 0;
           }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, projectile.frame * 32, 32, 32), Color.White, projectile.rotation, new Vector2(16, 16), projectile.scale, SpriteEffects.None, 0);

            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.owner == Main.myPlayer && projectile.timeLeft >= 52)
            {
                projectile.timeLeft = 0;

                if (Main.netMode != NetmodeID.Server)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/PulsarBump").WithVolume(.6f).WithPitchVariance(.3f), projectile.Center);
                }
            }
            else //(projectile.owner == Main.myPlayer && projectile.timeLeft <= 51)
            {
                projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 2 frames
                projectile.alpha = 255;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
                projectile.width = 60;
                projectile.height = 60;
                projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
                projectile.damage = (int)(originalDamage * 1.5f);
                projectile.knockBack = 6f;
                projectile.ranged = true;
                projectile.timeLeft = 0;

                if (Main.netMode != NetmodeID.Server)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/PulsarBoom").WithVolume(.6f).WithPitchVariance(.3f), projectile.Center);
                }
                for (int i = 0; i < 110; i++)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 30, default(Color), .4f);
                    Main.dust[dust].noGravity = true;
                }
                for (int i = 0; i < 60; i++)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 70, default(Color), .6f);
                    Main.dust[dust].noGravity = true;
                }
                for (int i = 0; i < 110; i++)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 70, default(Color), 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= Main.rand.NextFloat(2f, 3.5f);
                    Main.dust[dust].scale = 0.2f + (float)Main.rand.Next(5) * 0.1f;
                }

            }
            return false;
        }

        public int pulsardusttimer;
        public int originalDamage = 0;
        public bool spawned = false;

        public override void AI()
        {
            Lighting.AddLight(projectile.position, 0.0452f, 0.21f, 0.073f);
            //projectile.rotation += 0.12f;

            //Change these two variables to affect the rotation of your projectile
            float rotationsPerSecond = 1.2f;
            bool rotateClockwise = true;
            //The rotation is set here
            projectile.rotation += (rotateClockwise ? 1 : -1) * MathHelper.ToRadians(rotationsPerSecond * 6f);


            if (!spawned)
            {
                spawned = true;
                originalDamage = projectile.damage;
            }

            //DUST SPAWNING

            if (Main.rand.Next(4) == 0)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 100, default(Color), .4f);
                Main.dust[dust].noGravity = true;
            }
            pulsardusttimer++;
            if (pulsardusttimer >= 15 && pulsardusttimer <= 16)
            {
                for (int a = 0; a < 4; a++)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width - 9, projectile.height - 9, 226, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 100, default(Color), .4f);
                    Main.dust[dust].noGravity = true;
                }
            }
            if (pulsardusttimer >= 34 && pulsardusttimer <= 35)
            {
                for (int a = 0; a < 4; a++)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width - 9, projectile.height - 9, 226, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 100, default(Color), .4f);
                    Main.dust[dust].noGravity = true;
                }
            }
            if (pulsardusttimer >= 53 && pulsardusttimer <= 54)
            {
                for (int a = 0; a < 4; a++)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width - 9, projectile.height - 9, 226, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 100, default(Color), .4f);
                    Main.dust[dust].noGravity = true;
                }
            }
            if (pulsardusttimer >= 72 && pulsardusttimer <= 73)
            {
                for (int a = 0; a < 4; a++)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width - 9, projectile.height - 9, 226, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 100, default(Color), .4f);
                    Main.dust[dust].noGravity = true;
                }
            }

            //ANIMATION
            if (pulsardusttimer > 90)
            {
                pulsardusttimer = 0;
            }

            if (++projectile.frameCounter >= 10) //ticks spent on each frame
            {
                projectile.frameCounter = 0;
                if (projectile.timeLeft >= 52)
                {
                    if (++projectile.frame == 4)
                    {
                        projectile.frame = 0;
                    }
                }
                if (projectile.timeLeft <= 51)
                {
                    projectile.frameCounter = 5;
                    if (++projectile.frame >= 8)
                    {
                        projectile.frame = 5;
                    }
                }
            }

            Vector2 oldSize = projectile.Size;

            if (projectile.owner == Main.myPlayer && projectile.timeLeft <= 50)
            {
                projectile.tileCollide = true;
                projectile.width = 22;
                projectile.height = 22;
                projectile.damage = (int)(originalDamage * 1.5f);
                projectile.knockBack = 6f;
                projectile.ranged = true;
            }

            if (projectile.owner == Main.myPlayer && projectile.timeLeft == 51)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/PulsarGrow").WithVolume(.6f).WithPitchVariance(.3f), projectile.Center);
                }
            }

            projectile.position = projectile.position - (projectile.Size - oldSize) / 2f;

            if (projectile.owner == Main.myPlayer && projectile.timeLeft == 1)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/PulsarBoom").WithVolume(.6f).WithPitchVariance(.3f), projectile.Center);
                }
                for (int i = 0; i < 110; i++)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 30, default(Color), .4f);
                    Main.dust[dust].noGravity = true;
                }
                for (int i = 0; i < 60; i++)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 70, default(Color), .6f);
                    Main.dust[dust].noGravity = true;
                }
                for (int i = 0; i < 110; i++)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 70, default(Color), 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= Main.rand.NextFloat(2f, 3.5f);
                    Main.dust[dust].scale = 0.2f + (float)Main.rand.Next(5) * 0.1f;
                }
            }

            if (projectile.owner == Main.myPlayer && projectile.timeLeft <= 2)
            {
                projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 2 frames
                projectile.alpha = 255;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
                projectile.width = 60;
                projectile.height = 60;
                projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
                projectile.damage = (int)(originalDamage * 1.5f);
                projectile.knockBack = 6f;
                projectile.ranged = true;
            }
            if (projectile.wet)
            {
                projectile.tileCollide = false;
                // Set to transparent. This projectile technically lives as  transparent for about 2 frames
                projectile.alpha = 255;
                // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
                projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
                projectile.width = 80;
                projectile.height = 80;
                projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
                projectile.damage = (int)(originalDamage * 1.5f);
                projectile.knockBack = 6f;
                projectile.ranged = true;

                if (Main.netMode != NetmodeID.Server)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/PulsarBoom").WithVolume(.6f).WithPitchVariance(.3f), projectile.Center);
                }

                for (int i = 0; i < 110; i++)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 30, default(Color), .4f);
                    Main.dust[dust].noGravity = true;
                }
                for (int i = 0; i < 60; i++)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 70, default(Color), .6f);
                    Main.dust[dust].noGravity = true;
                }
                for (int i = 0; i < 110; i++)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 70, default(Color), 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= Main.rand.NextFloat(2f, 3.5f);
                    Main.dust[dust].scale = 0.2f + (float)Main.rand.Next(5) * 0.1f;
                    projectile.timeLeft = 0;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 50; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, projectile.velocity.X * 0f, projectile.velocity.Y * 0f, 70, default(Color), .6f);
                Main.dust[dust].noGravity = true;
            }
        }
    }
}