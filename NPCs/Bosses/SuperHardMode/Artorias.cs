﻿using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.NPCs.Bosses.SuperHardMode
{
    [AutoloadBossHead]
    class Artorias : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 15;
        }
        public override void SetDefaults()
        {
            NPC.knockBackResist = 0;
            NPC.damage = 200;
            NPC.defense = 0;
            NPC.height = 40;
            NPC.width = 30;
            NPC.lifeMax = 150000;
            NPC.scale = 1.1f;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 700000;
            NPC.boss = true;
            NPC.lavaImmune = true;
            bossBag = ModContent.ItemType<Items.BossBags.ArtoriasBag>();
            despawnHandler = new NPCDespawnHandler("Artorias, the Abysswalker stands victorious...", Color.Gold, DustID.GoldFlame);
        }

        public int holdBallDamage = 20;
        public int energyBallDamage = 30;
        public int lightPillarDamage = 75;
        public int blackBreathDamage = 35;
        public int lightning3Damage = 25;
        public int ice3Damage = 25;
        public int phantomSeekerDamage = 40;
        public int lightning4Damage = 40;
        public int shardsDamage = 40;
        public int iceStormDamage = 30;
        //This attack does damage equal to 25% of your max health no matter what, so its damage stat is irrelevant and only listed for readability.
        public int gravityBallDamage = 0;

        bool defenseBroken = false;


        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.damage = (int)(NPC.damage / 2);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {

            int expertScale = 1;
            if (Main.expertMode) expertScale = 2;

            if (Main.rand.Next(4) == 0)
            {
                target.AddBuff(BuffID.BrokenArmor, 180 / expertScale, false);
                target.AddBuff(BuffID.Poisoned, 3600 / expertScale, false);
                target.AddBuff(BuffID.Cursed, 300 / expertScale, false);
                target.AddBuff(ModContent.BuffType<Buffs.CurseBuildup>(), 18000, false);
            }
        }
        float customAi1;

        float customspawn2;
        NPCDespawnHandler despawnHandler;


        //PROJECTILE HIT LOGIC
        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            tsorcRevampAIs.RedKnightOnHit(NPC, true);
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            tsorcRevampAIs.RedKnightOnHit(NPC, projectile.melee);
        }


        public override void AI()
        {


            Player player = Main.player[NPC.target];

            if (NPC.Distance(player.Center) < 600)
            {

                player.AddBuff(ModContent.BuffType<Buffs.TornWings>(), 60, false);
                player.AddBuff(ModContent.BuffType<Buffs.GrappleMalfunction>(), 60, false);

            }


            despawnHandler.TargetAndDespawn(NPC.whoAmI);
            if (NPC.HasBuff(ModContent.BuffType<Buffs.DispelShadow>()))
            {
                defenseBroken = true;
            }

            int num58;
            int num59;


            int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 32, NPC.velocity.X - 3f, NPC.velocity.Y, 150, Color.Yellow, 1f);
            Main.dust[dust].noGravity = true;

            bool flag2 = false;
            int num5 = 60;
            bool flag3 = true;
            if (NPC.velocity.Y == 0f && (NPC.velocity.X == 0f && NPC.direction < 0))
            {
                NPC.velocity.Y -= 8f;
                NPC.velocity.X -= 2f;
            }
            else if (NPC.velocity.Y == 0f && (NPC.velocity.X == 0f && NPC.direction > 0))
            {
                NPC.velocity.Y -= 8f;
                NPC.velocity.X += 2f;
            }
            if (NPC.velocity.Y == 0f && ((NPC.velocity.X > 0f && NPC.direction < 0) || (NPC.velocity.X < 0f && NPC.direction > 0)))
            {
                flag2 = true;
            }
            if (NPC.position.X == NPC.oldPosition.X || NPC.ai[3] >= (float)num5 || flag2)
            {
                NPC.ai[3] += 1f;
            }
            else
            {
                if ((double)Math.Abs(NPC.velocity.X) > 0.9 && NPC.ai[3] > 0f)
                {
                    NPC.ai[3] -= 1f;
                }
            }
            if (NPC.ai[3] > (float)(num5 * 10))
            {
                NPC.ai[3] = 0f;
            }
            if (NPC.justHit)
            {
                NPC.ai[3] = 0f;
            }
            if (NPC.ai[3] == (float)num5)
            {
                NPC.netUpdate = true;
            }

            if (NPC.velocity.X == 0f)
            {
                if (NPC.velocity.Y == 0f)
                {
                    NPC.ai[0] += 1f;
                    if (NPC.ai[0] >= 2f)
                    {
                        NPC.direction *= -1;
                        NPC.spriteDirection = NPC.direction;
                        NPC.ai[0] = 0f;
                    }
                }
            }
            else
            {
                NPC.ai[0] = 0f;
            }
            if (NPC.direction == 0)
            {
                NPC.direction = 1;
            }
            if (NPC.velocity.X < -1.5f || NPC.velocity.X > 1.5f)
            {
                if (NPC.velocity.Y == 0f)
                {
                    NPC.velocity *= 0.8f;
                }
            }
            else
            {
                if (NPC.velocity.X < 1.5f && NPC.direction == 1)
                {
                    NPC.velocity.X = NPC.velocity.X + 0.07f;
                    if (NPC.velocity.X > 1.5f)
                    {
                        NPC.velocity.X = 1.5f;
                    }
                }
                else
                {
                    if (NPC.velocity.X > -1.5f && NPC.direction == -1)
                    {
                        NPC.velocity.X = NPC.velocity.X - 0.07f;
                        if (NPC.velocity.X < -1.5f)
                        {
                            NPC.velocity.X = -1.5f;
                        }
                    }
                }
            }
            bool flag4 = false;
            if (NPC.velocity.Y == 0f)
            {
                int num29 = (int)(NPC.position.Y + (float)NPC.height + 8f) / 16;
                int num30 = (int)NPC.position.X / 16;
                int num31 = (int)(NPC.position.X + (float)NPC.width) / 16;
                for (int l = num30; l <= num31; l++)
                {
                    if (Main.tile[l, num29] == null)
                    {
                        return;
                    }
                    if (Main.tile[l, num29].HasTile && Main.tileSolid[(int)Main.tile[l, num29].TileType])
                    {
                        flag4 = true;
                        break;
                    }
                }
            }
            if (flag4)
            {
                int num32 = (int)((NPC.position.X + (float)(NPC.width / 2) + (float)(15 * NPC.direction)) / 16f);
                int num33 = (int)((NPC.position.Y + (float)NPC.height - 15f) / 16f);
                if (Main.tile[num32, num33] == null)
                {
                    Main.tile[num32, num33].ClearTile();
                }
                if (Main.tile[num32, num33 - 1] == null)
                {
                    Main.tile[num32, num33 - 1].ClearTile();
                }
                if (Main.tile[num32, num33 - 2] == null)
                {
                    Main.tile[num32, num33 - 2].ClearTile();
                }
                if (Main.tile[num32, num33 - 3] == null)
                {
                    Main.tile[num32, num33 - 3].ClearTile();
                }
                if (Main.tile[num32, num33 + 1] == null)
                {
                    Main.tile[num32, num33 + 1].ClearTile();
                }
                if (Main.tile[num32 + NPC.direction, num33 - 1] == null)
                {
                    Main.tile[num32 + NPC.direction, num33 - 1].ClearTile();
                }
                if (Main.tile[num32 + NPC.direction, num33 + 1] == null)
                {
                    Main.tile[num32 + NPC.direction, num33 + 1].ClearTile();
                }
                if (Main.tile[num32, num33 - 1].HasTile && Main.tile[num32, num33 - 1].TileType == 10 && flag3)
                {
                    NPC.ai[2] += 1f;
                    NPC.ai[3] = 0f;
                    if (NPC.ai[2] >= 60f)
                    {
                        NPC.velocity.X = 0.5f * (float)(-(float)NPC.direction);
                        NPC.ai[1] += 1f;
                        NPC.ai[2] = 0f;
                        bool flag5 = false;
                        if (NPC.ai[1] >= 10f)
                        {
                            flag5 = true;
                            NPC.ai[1] = 10f;
                        }
                        WorldGen.KillTile(num32, num33 - 1, true, false, false);
                        if ((Main.netMode != NetmodeID.MultiplayerClient || !flag5) && flag5 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            if (NPC.type == NPCID.GoblinPeon)
                            {
                                WorldGen.KillTile(num32, num33 - 1, false, false, false);
                                if (Main.netMode == NetmodeID.Server)
                                {
                                    NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, (float)num32, (float)(num33 - 1), 0f, 0);
                                }
                            }
                            else
                            {
                                bool flag6 = WorldGen.OpenDoor(num32, num33, NPC.direction);
                                if (!flag6)
                                {
                                    NPC.ai[3] = (float)num5;
                                    NPC.netUpdate = true;
                                }
                                if (Main.netMode == NetmodeID.Server && flag6)
                                {
                                    NetMessage.SendData(MessageID.ChangeDoor, -1, -1, null, 0, (float)num32, (float)num33, (float)NPC.direction, 0);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if ((NPC.velocity.X < 0f && NPC.spriteDirection == -1) || (NPC.velocity.X > 0f && NPC.spriteDirection == 1))
                    {
                        if (Main.tile[num32, num33 - 2].HasTile && Main.tileSolid[(int)Main.tile[num32, num33 - 2].TileType])
                        {
                            if (Main.tile[num32, num33 - 3].HasTile && Main.tileSolid[(int)Main.tile[num32, num33 - 3].TileType])
                            {
                                NPC.velocity.Y = -8f;
                                NPC.netUpdate = true;
                            }
                            else
                            {
                                NPC.velocity.Y = -7f;
                                NPC.netUpdate = true;
                            }
                        }
                        else
                        {
                            if (Main.tile[num32, num33 - 1].HasTile && Main.tileSolid[(int)Main.tile[num32, num33 - 1].TileType])
                            {
                                NPC.velocity.Y = -6f;
                                NPC.netUpdate = true;
                            }
                            else
                            {
                                if (Main.tile[num32, num33].HasTile && Main.tileSolid[(int)Main.tile[num32, num33].TileType])
                                {
                                    NPC.velocity.Y = -5f;
                                    NPC.netUpdate = true;
                                }
                                else
                                {
                                    if (NPC.directionY < 0 && (!Main.tile[num32, num33 + 1].HasTile || !Main.tileSolid[(int)Main.tile[num32, num33 + 1].TileType]) && (!Main.tile[num32 + NPC.direction, num33 + 1].HasTile || !Main.tileSolid[(int)Main.tile[num32 + NPC.direction, num33 + 1].TileType]))
                                    {
                                        NPC.velocity.Y = -8f;
                                        NPC.velocity.X = NPC.velocity.X * 1.5f;
                                        NPC.netUpdate = true;
                                    }
                                    else
                                    {
                                        if (flag3)
                                        {
                                            NPC.ai[1] = 0f;
                                            NPC.ai[2] = 0f;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (flag3)
                {
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                }
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                customAi1 += (Main.rand.Next(2, 5) * 0.1f) * NPC.scale;
                if (customAi1 >= 10f)
                {

                    if ((customspawn2 < 24) && Main.rand.Next(1950) == 1)
                    {
                        int Spawned = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<Enemies.LothricBlackKnight>(), 0); // Spawns Lothric Black Knight
                        Main.npc[Spawned].velocity.Y = -8;
                        Main.npc[Spawned].velocity.X = Main.rand.Next(-10, 10) / 10;
                        NPC.ai[0] = 20 - Main.rand.Next(80);
                        customspawn2 += 1f;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, Spawned, 0f, 0f, 0f, 0);
                        }
                    }
                    if (Main.rand.Next(220) == 1)
                    {
                        Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2));
                        float rotation = (float)Math.Atan2(vector8.Y - (Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)), vector8.X - (Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)));
                        NPC.velocity.X = (float)(Math.Cos(rotation) * 14) * -1;
                        NPC.velocity.Y = (float)(Math.Sin(rotation) * 14) * -1;
                        NPC.ai[1] = 1f;
                        NPC.netUpdate = true;
                    }
                    if (Main.rand.Next(400) == 1 && NPC.Distance(player.Center) > 100)
                    {
                        float num48 = 8f;
                        Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y - 100 + (NPC.height / 2));
                        float speedX = ((Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)) - vector8.X) + Main.rand.Next(-20, 0x15);
                        float speedY = ((Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)) - vector8.Y) + Main.rand.Next(-20, 0x15);
                        if (((speedX < 0f) && (NPC.velocity.X < 0f)) || ((speedX > 0f) && (NPC.velocity.X > 0f)))
                        {
                            float num51 = (float)Math.Sqrt((double)((speedX * speedX) + (speedY * speedY)));
                            num51 = num48 / num51;
                            speedX *= num51;
                            speedY *= num51;
                            int type = ModContent.ProjectileType<Projectiles.Enemy.EnemySpellHoldBall>();
                            int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y, speedX, speedY, type, holdBallDamage, 0f, Main.myPlayer);
                            Main.projectile[num54].timeLeft = 105;
                            Main.projectile[num54].aiStyle = 1;
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCHit3 with { Volume = 0.2f, Pitch = -0.7f }, NPC.Center); //MAGIC INTERCEPT


                            NPC.ai[1] = 1f;





                        }
                        NPC.netUpdate = true;
                    }
                    if (Main.rand.Next(150) == 1 && NPC.Distance(player.Center) > 100)
                    {
                        float num48 = 10f;
                        Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2));
                        float speedX = ((Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)) - vector8.X) + Main.rand.Next(-10, 20);
                        float speedY = ((Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)) - vector8.Y) + Main.rand.Next(-10, 30);
                        if (((speedX < 0f) && (NPC.velocity.X < 0f)) || ((speedX > 0f) && (NPC.velocity.X > 0f)))
                        {
                            float num51 = (float)Math.Sqrt((double)((speedX * speedX) + (speedY * speedY)));
                            num51 = num48 / num51;
                            speedX *= num51;
                            speedY *= num51;
                            int type = ModContent.ProjectileType<Projectiles.Enemy.EnemySpellGreatEnergyBall>();
                            int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y, speedX, speedY, type, energyBallDamage, 0f, Main.myPlayer);
                            Main.projectile[num54].timeLeft = 100;
                            Main.projectile[num54].aiStyle = 1;
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item95 with { Volume = 0.3f, Pitch = 0.8f }, NPC.Center); //xenopopper
                                                                                                                                   //Terraria.Audio.SoundEngine.PlaySound(3, (int)npc.position.X, (int)npc.position.Y, 3, 0.2f, .1f); //magic intercept
                            customAi1 = 1f;
                        }
                        NPC.netUpdate = true;
                    }
                    /* removed because it looks broken and can't be fairly dodged
					if (Main.rand.Next(40) == 1) {
						Vector2 vector8 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height / 2));
						float speedX = ((Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5f)) - vector8.X) + Main.rand.Next(-20, 0x15);
						float speedY = ((Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5f)) - vector8.Y) + Main.rand.Next(-20, 0x15);
						if (((speedX < 0f) && (npc.velocity.X < 0f)) || ((speedX > 0f) && (npc.velocity.X > 0f))) {
							speedX *= 0;
							speedY *= 0;
							int type = ModContent.ProjectileType<Projectiles.Enemy.EnemySpellLightPillarBall>();
							int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y, speedX, speedY, type, lightPillarDamage, 0f, Main.myPlayer, npc.direction);
							Main.projectile[num54].timeLeft = 300;
							Main.projectile[num54].aiStyle = 1;
							Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 0x11);
							customAi1 = 1f;
						}
						npc.netUpdate = true;
					}
					
					if (Main.rand.Next(200) == 1 && npc.Distance(player.Center) > 100) {
						float num48 = 8f;
						Vector2 vector8 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height / 2));
						float speedX = ((Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5f)) - vector8.X) + Main.rand.Next(-20, 0x15);
						float speedY = ((Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5f)) - vector8.Y) + Main.rand.Next(-20, 0x15);
						if (((speedX < 0f) && (npc.velocity.X < 0f)) || ((speedX > 0f) && (npc.velocity.X > 0f))) {
							float num51 = (float)Math.Sqrt((double)((speedX * speedX) + (speedY * speedY)));
							num51 = num48 / num51;
							speedX *= num51;
							speedY *= num51;
							int type = ModContent.ProjectileType<Projectiles.Enemy.BlackBreath>();
							int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y, speedX, speedY, type, blackBreathDamage, 0f, Main.myPlayer);
							Main.projectile[num54].timeLeft = 10;
							Main.projectile[num54].aiStyle = 1;
							Terraria.Audio.SoundEngine.PlaySound(3, (int)npc.position.X, (int)npc.position.Y, 52, 0.2f, .1f); //shadowflame apparition
							customAi1 = 1f;
						}
						npc.netUpdate = true;
					}
					*/
                    if (Main.rand.Next(250) == 1 && NPC.Distance(player.Center) > 100)
                    {
                        float num48 = 9f;
                        Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2));
                        float speedX = ((Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)) - vector8.X) + Main.rand.Next(-20, 0x15);
                        float speedY = ((Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)) - vector8.Y) + Main.rand.Next(-20, 0x15);
                        if (((speedX < 0f) && (NPC.velocity.X < 0f)) || ((speedX > 0f) && (NPC.velocity.X > 0f)))
                        {
                            float num51 = (float)Math.Sqrt((double)((speedX * speedX) + (speedY * speedY)));
                            num51 = num48 / num51;
                            speedX *= num51;
                            speedY *= num51;
                            int type = ModContent.ProjectileType<Projectiles.Enemy.EnemySpellLightning3Ball>();
                            int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y, speedX, speedY, type, lightning3Damage, 0f, Main.myPlayer);
                            Main.projectile[num54].timeLeft = 300;
                            Main.projectile[num54].aiStyle = 1;
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item93 with { Volume = 0.1f, Pitch = 0.2f }, NPC.Center); //electric zap
                            customAi1 = 1f;
                        }
                        NPC.netUpdate = true;
                    }
                    if (Main.rand.Next(220) == 1 && NPC.Distance(player.Center) > 100)
                    {
                        float num48 = 8f;
                        Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y - 650 + (NPC.height / 2));
                        float speedX = ((Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)) - vector8.X) + Main.rand.Next(-20, 0x15);
                        float speedY = ((Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)) - vector8.Y) + Main.rand.Next(-20, 0x15);
                        if (((speedX < 0f) && (NPC.velocity.X < 0f)) || ((speedX > 0f) && (NPC.velocity.X > 0f)))
                        {
                            float num51 = (float)Math.Sqrt((double)((speedX * speedX) + (speedY * speedY)));
                            num51 = num48 / num51;
                            speedX *= num51;
                            speedY *= num51;
                            int type = ModContent.ProjectileType<Projectiles.Enemy.EnemySpellIce3Ball>();
                            int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y, speedX, speedY, type, ice3Damage, 0f, Main.myPlayer);
                            Main.projectile[num54].timeLeft = 40;
                            Main.projectile[num54].aiStyle = 1;
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item71 with { Volume = 0.2f, Pitch = 0.1f }, NPC.Center); //death cicle
                            customAi1 = 1f;
                        }
                        NPC.netUpdate = true;
                    }
                    if (Main.rand.Next(600) == 1 && NPC.Distance(player.Center) > 300)
                    {
                        num58 = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.position.X + 20, NPC.position.Y + 50, Main.rand.Next(-5, 5), Main.rand.Next(-5, 5), ModContent.ProjectileType<Projectiles.PhantomSeeker>(), phantomSeekerDamage, 0f, Main.myPlayer);
                        Main.projectile[num58].timeLeft = 400;
                        Main.projectile[num58].rotation = Main.rand.Next(700) / 100f;
                        Main.projectile[num58].ai[0] = NPC.target;

                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item93 with { Volume = 0.1f, Pitch = 0.6f }, NPC.Center); //electric zap

                        customAi1 = 1f;

                        NPC.netUpdate = true;
                    }
                    if (Main.rand.Next(650) == 1)
                    {
                        float num48 = 8f;
                        Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y - 400 + (NPC.height / 2));
                        float speedX = ((Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)) - vector8.X) + Main.rand.Next(-20, 0x15);
                        float speedY = ((Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)) - vector8.Y) + Main.rand.Next(-20, 0x15);
                        if (((speedX < 0f) && (NPC.velocity.X < 0f)) || ((speedX > 0f) && (NPC.velocity.X > 0f)))
                        {
                            float num51 = (float)Math.Sqrt((double)((speedX * speedX) + (speedY * speedY)));
                            num51 = num48 / num51;
                            speedX *= num51;
                            speedY *= num51;
                            int type = ModContent.ProjectileType<Projectiles.Enemy.EnemySpellLightning4Ball>();
                            int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y, speedX, speedY, type, lightning4Damage, 0f, Main.myPlayer);
                            Main.projectile[num54].timeLeft = 300;
                            Main.projectile[num54].aiStyle = 1;
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item92 with { Volume = 0.2f, Pitch = -0.2f }, NPC.Center); //electrosphere launch
                            customAi1 = 1f;
                        }
                        NPC.netUpdate = true;
                    }



                    if (Main.rand.Next(350) == 1)
                    {
                        float num48 = 8f;
                        Vector2 vector9 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y - 520 + (NPC.height / 2));
                        float speedX = ((Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)) - vector9.X) + Main.rand.Next(-20, 0x15);
                        float speedY = ((Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)) - vector9.Y) + Main.rand.Next(-20, 0x15);
                        if (((speedX < 0f) && (NPC.velocity.X < 0f)) || ((speedX > 0f) && (NPC.velocity.X > 0f)))
                        {
                            float num51 = (float)Math.Sqrt((double)((speedX * speedX) + (speedY * speedY)));
                            num51 = num48 / num51;
                            speedX *= num51;
                            speedY *= num51;
                            int type = ModContent.ProjectileType<Projectiles.Enemy.Okiku.MassiveCrystalShardsSpell>();
                            int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector9.X, vector9.Y, speedX, speedY, type, shardsDamage, 0f, Main.myPlayer);
                            Main.projectile[num54].timeLeft = 100;
                            Main.projectile[num54].aiStyle = 4;
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item30 with { Volume = 0.2f, Pitch = -0.3f }, NPC.Center); //ice materialize - good
                            NPC.ai[3] = 0; ;
                        }
                        NPC.netUpdate = true;
                    }


                    if (Main.rand.Next(500) == 1 && NPC.Distance(player.Center) > 300)
                    {
                        num59 = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.position.X + 20, NPC.position.Y + 50, Main.rand.Next(-5, 5), Main.rand.Next(-5, 5), ModContent.ProjectileType<Projectiles.PhantomSeeker>(), phantomSeekerDamage, 0f, Main.myPlayer);
                        Main.projectile[num59].timeLeft = 500;
                        Main.projectile[num59].rotation = Main.rand.Next(700) / 100f;
                        Main.projectile[num59].ai[0] = NPC.target;

                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item93 with { Volume = 0.1f, Pitch = -0.1f }, NPC.Center); //electric zap
                        customAi1 = 1f;

                        NPC.netUpdate = true;

                    }
                }





                if (Main.rand.Next(350) == 1 && NPC.Distance(player.Center) > 100)
                {
                    float num48 = 8f;
                    Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2));
                    float speedX = ((Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)) - vector8.X) + Main.rand.Next(-20, 0x15);
                    float speedY = ((Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)) - vector8.Y) + Main.rand.Next(-20, 0x15);
                    if (((speedX < 0f) && (NPC.velocity.X < 0f)) || ((speedX > 0f) && (NPC.velocity.X > 0f)))
                    {
                        float num51 = (float)Math.Sqrt((double)((speedX * speedX) + (speedY * speedY)));
                        num51 = num48 / num51;
                        speedX *= num51;
                        speedY *= num51;
                        int type = ModContent.ProjectileType<Projectiles.Enemy.EnemySpellIcestormBall>();
                        int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y, speedX, speedY, type, iceStormDamage, 0f, Main.myPlayer);
                        Main.projectile[num54].timeLeft = 1;
                        Main.projectile[num54].aiStyle = 1;
                        //Terraria.Audio.SoundEngine.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 120, 0.3f, .1f); //ice mist howl sounds crazy
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item30 with { Volume = 0.2f, Pitch = 0.3f }, NPC.Center); //ice materialize - good
                        NPC.ai[1] = 1f;
                    }
                    NPC.netUpdate = true;
                }



                if (Main.rand.Next(205) == 1)
                {
                    float num48 = 9f;
                    Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2));
                    float speedX = ((Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)) - vector8.X) + Main.rand.Next(-20, 0x15);
                    float speedY = ((Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)) - vector8.Y) + Main.rand.Next(-20, 0x15);
                    if (((speedX < 0f) && (NPC.velocity.X < 0f)) || ((speedX > 0f) && (NPC.velocity.X > 0f)))
                    {
                        float num51 = (float)Math.Sqrt((double)((speedX * speedX) + (speedY * speedY)));
                        num51 = num48 / num51;
                        speedX *= num51;
                        speedY *= num51;
                        int type = ModContent.ProjectileType<Projectiles.Enemy.EnemySpellGreatEnergyBall>();
                        int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y, speedX, speedY, type, energyBallDamage, 0f, Main.myPlayer);
                        Main.projectile[num54].timeLeft = 300;
                        Main.projectile[num54].aiStyle = 1;
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item113 with { Volume = 0.2f, Pitch = 0.2f }, NPC.Center); //deadly sphere
                        customAi1 = 1f;
                    }
                    NPC.netUpdate = true;
                }
                if (Main.rand.Next(500) == 1 && NPC.Distance(player.Center) > 100)
                {
                    float num48 = 7f;
                    Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2));
                    float speedX = ((Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)) - vector8.X) + Main.rand.Next(-20, 0x15);
                    float speedY = ((Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)) - vector8.Y) + Main.rand.Next(-20, 0x15);
                    if (((speedX < 0f) && (NPC.velocity.X < 0f)) || ((speedX > 0f) && (NPC.velocity.X > 0f)))
                    {
                        float num51 = (float)Math.Sqrt((double)((speedX * speedX) + (speedY * speedY)));
                        num51 = num48 / num51;
                        speedX *= num51;
                        speedY *= num51;
                        int type = ModContent.ProjectileType<Projectiles.Enemy.EnemySpellGravity4Ball>();
                        int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y, speedX, speedY, type, gravityBallDamage, 0f, Main.myPlayer);
                        Main.projectile[num54].timeLeft = 60;
                        Main.projectile[num54].aiStyle = 1;
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item113 with { Volume = 0.2f, Pitch = -0.2f }, NPC.Center); //deadly sphere
                        customAi1 = 1f;
                    }
                    NPC.netUpdate = true;
                }
            }
            if ((Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height)))
            {
                NPC.noTileCollide = false;
                NPC.noGravity = false;
            }
            if ((!Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height)))
            {
                NPC.noTileCollide = true;
                NPC.noGravity = true;
                NPC.velocity.Y = 0f;
                if (NPC.position.Y > Main.player[NPC.target].position.Y)
                {
                    NPC.velocity.Y -= 3f;
                }
                if (NPC.position.Y < Main.player[NPC.target].position.Y)
                {
                    NPC.velocity.Y += 8f;
                }
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            if (NPC.HasBuff(ModContent.BuffType<Buffs.DispelShadow>()))
            {
                defenseBroken = true;
            }
            if (defenseBroken)
            {
                writer.Write(true);
            }
            else
            {
                writer.Write(false);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            bool recievedBrokenDef = reader.ReadBoolean();
            if (recievedBrokenDef == true)
            {
                defenseBroken = true;
                NPC.defense = 0;
            }
        }
        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (item.type == ModContent.ItemType<Items.Weapons.Melee.BarrowBlade>() || item.type == ModContent.ItemType<Items.Weapons.Melee.ForgottenGaiaSword>())
            {
                defenseBroken = true;
            }
            if (!defenseBroken)
            {
                damage = 1;
            }
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (!defenseBroken)
            {
                damage = 1;
            }
        }


        public override bool CheckActive()
        {
            return false;
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        #region Gore
        public override void OnKill()
        {
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Gores/Easterling Gore 1").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Gores/Easterling Gore 2").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Gores/Easterling Gore 3").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Gores/Easterling Gore 2").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Gores/Easterling Gore 3").Type, 1f);

            if (Main.expertMode)
            {
                NPC.DropBossBags();
            }
            else
            {
                Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<Items.GuardianSoul>());
                Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<Items.DarkSoul>(), 5000);
                Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<Items.Accessories.WolfRing>());
                Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<Items.Accessories.TheRingOfArtorias>());
                Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<Items.SoulOfArtorias>(), 4);
                Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<Items.BossItems.DarkMirror>());
            }
        }
        #endregion

        public override void FindFrame(int currentFrame)
        {
            int num = 1;
            if (!Main.dedServ)
            {
                num = TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type];
            }
            if (NPC.velocity.Y == 0f)
            {
                if (NPC.direction == 1)
                {
                    NPC.spriteDirection = 1;
                }
                if (NPC.direction == -1)
                {
                    NPC.spriteDirection = -1;
                }
                if (NPC.velocity.X == 0f)
                {
                    NPC.frame.Y = 0;
                    NPC.frameCounter = 0.0;
                }
                else
                {
                    NPC.frameCounter += (double)(Math.Abs(NPC.velocity.X) * .5f);
                    //npc.frameCounter += 1.0;
                    if (NPC.frameCounter > 2)
                    {
                        NPC.frame.Y = NPC.frame.Y + num;
                        NPC.frameCounter = 0;
                    }
                    if (NPC.frame.Y / num >= Main.npcFrameCount[NPC.type])
                    {
                        NPC.frame.Y = num * 1;
                    }
                }
            }
            else
            {
                NPC.frameCounter = 1.5;
                NPC.frame.Y = num;
                NPC.frame.Y = 0;
            }
        }
    }
}
