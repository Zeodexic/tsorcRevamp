using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.NPCs.Enemies
{
    class Warlock : ModNPC
    {
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 15;
            AnimationType = 21;
            NPC.knockBackResist = 0.2f;
            NPC.aiStyle = 3;
            NPC.damage = 65;
            NPC.npcSlots = 5;
            NPC.defense = 5;
            NPC.height = 40;
            NPC.width = 20;
            NPC.lifeMax = 2600;
            NPC.scale = 1f;
            NPC.HitSound = SoundID.NPCHit37;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 6700;
            NPC.rarity = 3;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.WarlockBanner>();
            if (!Main.hardMode)
            {
                NPC.lifeMax = 2000;
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax / 2);
            NPC.damage = (int)(NPC.damage / 2);
            greatEnergyBeamDamage = (int)(greatEnergyBeamDamage / 2);
            energyBallDamage = (int)(energyBallDamage / 2);
        }

        int greatEnergyBeamDamage = 35;
        int energyBallDamage = 35;



        //Spawn in the Cavern, mostly before 3/10th and after 7/10th (Width). Does not spawn in the Dungeon, Jungle, Meteor, or if there are Town NPCs
        #region Spawn

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!NPC.downedBoss1)
            {
                return 0;
            }

            Player P = spawnInfo.Player; //These are mostly redundant with the new zone definitions, but it still works.
            bool Meteor = P.ZoneMeteor;
            bool Jungle = P.ZoneJungle;
            bool Dungeon = P.ZoneDungeon;
            bool Corruption = (P.ZoneCorrupt || P.ZoneCrimson);
            bool Hallow = P.ZoneHallow;
            bool AboveEarth = P.ZoneOverworldHeight;
            bool oUnderground = P.ZoneDirtLayerHeight;
            bool oCavern = P.ZoneRockLayerHeight;
            bool InHell = P.ZoneUnderworldHeight;
            bool Ocean = spawnInfo.SpawnTileX < 3600 || spawnInfo.SpawnTileX > (Main.maxTilesX - 100) * 16;
            // P.townNPCs > 0f // is no town NPCs nearby

            //if (spawnInfo.Player.townNPCs > 0f || spawnInfo.Player.ZoneMeteor) return 0;
            if (!Main.hardMode && oCavern)
            {
                if (Main.rand.Next(1000) == 1) return 1;
                else if ((spawnInfo.SpawnTileX < Main.maxTilesX * 0.3f || spawnInfo.SpawnTileX > Main.maxTilesX * 0.7f) && Main.rand.Next(400) == 1)
                {
                    UsefulFunctions.BroadcastText("A Warlock is near... ", 175, 75, 255);
                    return 1;
                }

            }
            if (Main.hardMode && (oCavern || oUnderground || Jungle))
            {
                if (Main.rand.Next(400) == 1) return 1;
                else if ((spawnInfo.SpawnTileX < Main.maxTilesX * 0.3f || spawnInfo.SpawnTileX > Main.maxTilesX * 0.7f) && Main.rand.Next(200) == 1)
                {
                    UsefulFunctions.BroadcastText("A Warlock is hunting you... ", 175, 75, 255);
                    return 1;
                }
            }
            return 0;
        }
        #endregion

        float attackTimer = 0;
        float bigAttackTimer = 0;
        public override void AI()
        {
            bigAttackTimer++;
            tsorcRevampAIs.FighterAI(NPC, 1.8f, 0.03f, .2f, canTeleport: true, lavaJumping: true);

            bool clearShot = Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height) && Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) <= 1000;

            if (tsorcRevampAIs.SimpleProjectile(NPC, ref attackTimer, 140, ModContent.ProjectileType<Projectiles.Enemy.EnemySpellGreatEnergyBall>(), energyBallDamage, 8, clearShot && Main.rand.NextBool()))
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item28 with { Volume = 0.2f, Pitch = -0.8f }, NPC.Center);
            }
            if (tsorcRevampAIs.SimpleProjectile(NPC, ref bigAttackTimer, 250, ModContent.ProjectileType<Projectiles.Enemy.EnemySpellGreatEnergyBeamBall>(), greatEnergyBeamDamage, 8, clearShot, false))
            { //Terraria.Audio.SoundEngine.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 75, 0.2f, 0.2f);

            }

            if (tsorcRevampAIs.SimpleProjectile(NPC, ref attackTimer, 140, ModContent.ProjectileType<Projectiles.Enemy.EnemySpellEffectHealing>(), 1, 0, !clearShot, false, shootSound: SoundID.Item17))
            {
                NPC.life += 10;
                NPC.HealEffect(10);
                if (NPC.life > NPC.lifeMax) NPC.life = NPC.lifeMax;
            }
            //TELEGRAPHS
            //BlACK DUST is used to show stunlock worked, PINK is used to show unstoppable attack incoming
            //BLACK DUST
            if (attackTimer >= 60)
            {

                if (Main.rand.Next(2) == 1)
                {
                    int black = Dust.NewDust(NPC.position, NPC.width, NPC.height, 54, (NPC.velocity.X * 0.2f), NPC.velocity.Y * 0.2f, 100, default, 1f); //54 is black smoke
                    Main.dust[black].noGravity = true;

                }
            }
            //PINK DUST
            if (attackTimer >= 110)
            {
                Lighting.AddLight(NPC.Center, Color.WhiteSmoke.ToVector3() * 2f); //Pick a color, any color. The 0.5f tones down its intensity by 50%
                if (Main.rand.Next(2) == 1)
                {
                    int pink = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CrystalSerpent, NPC.velocity.X, NPC.velocity.Y, Scale: 1.5f);

                    Main.dust[pink].noGravity = true;
                }
            }

            //IF HIT BEFORE PINK DUST TELEGRAPH, RESET TIMER, BUT CHANCE TO BREAK STUN LOCK
            //(WORKS WITH 2 TELEGRAPH DUSTS, AT 60 AND 110)
            if (NPC.justHit && attackTimer <= 109)
            {
                if (Main.rand.Next(3) == 0)
                {
                    attackTimer = 110;
                }
                else
                {
                    attackTimer = 0;
                }
            }
            if (NPC.justHit && Main.rand.Next(8) == 1)
            {
                tsorcRevampAIs.Teleport(NPC, 20, true);
                attackTimer = 70f;
                NPC.netUpdate = true;
            }

            //Transparency. Higher alpha = more invisible
            if (NPC.justHit)
            {
                NPC.alpha = 0;
                NPC.netUpdate = true;
            }
            if (Main.rand.Next(230) == 1)
            {
                NPC.alpha = 0;
                NPC.netUpdate = true;
            }
            if (Main.rand.Next(150) == 1)
            {
                NPC.alpha = 230;
                NPC.netUpdate = true;
            }

            Lighting.AddLight((int)NPC.position.X / 16, (int)NPC.position.Y / 16, 0.4f, 0.4f, 0.4f);

            if (Main.rand.Next(600) == 0 && NPC.CountNPCS(NPCID.IlluminantBat) < 5)
            {
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCID.IlluminantBat);
            }

            //BIG ATTACK DUST CIRCLE
            if (bigAttackTimer > 180)
            {
                for (int j = 0; j < 10; j++)
                {
                    Vector2 dir = Main.rand.NextVector2CircularEdge(32, 32);
                    Vector2 dustPos = NPC.Center + dir;
                    Vector2 dustVel = new Vector2(5, 0).RotatedBy(dir.ToRotation() + MathHelper.Pi / 2);
                    Dust.NewDustPerfect(dustPos, DustID.MagicMirror, dustVel, 200).noGravity = true;
                }
            }
        }

        #region Gore
        public override void OnKill()
        {
            if (!Main.dedServ)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Warlock Gore 1").Type, 1.1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Warlock Gore 2").Type, 1.1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Warlock Gore 3").Type, 1.1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Warlock Gore 2").Type, 1.1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Warlock Gore 3").Type, 1.1f);
            }
            Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ItemID.LifeforcePotion, 1);
            Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ItemID.MagicPowerPotion, 1);
            Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<Items.Potions.Lifegem>(), 3);
        }
        #endregion
    }
}