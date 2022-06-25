using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.NPCs.Enemies.SuperHardMode
{
    class HydrisNecromancer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydris Necromancer");
        }
        public override void SetDefaults()
        {
            NPC.npcSlots = 5;
            Main.npcFrameCount[NPC.type] = 15;
            AnimationType = 21;
            NPC.knockBackResist = 0.1f;
            NPC.aiStyle = 3; //was 3
            NPC.damage = 120;
            NPC.defense = 75; //was 135
            NPC.height = 40;
            NPC.width = 20;
            NPC.lifeMax = 6000;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 27050; //was 1600 souls
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Banners.HydrisNecromancerBanner>();

        }

        int deathStrikeDamage = 65;

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax / 2);
            NPC.damage = (int)(NPC.damage / 2);
            deathStrikeDamage = (int)(deathStrikeDamage * tsorcRevampWorld.SubtleSHMScale);
        }

        //Spawns in the Underground and Cavern before 3.5/10ths and after 7.5/10ths (Width). Does not Spawn in the Jungle, Meteor, or if there are Town NPCs.

        #region Spawn
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            Player P = spawnInfo.Player; //this shortens our code up from writing this line over and over.
            bool Hallow = P.ZoneHallow;
            bool oMagmaCavern = (spawnInfo.SpawnTileY >= (Main.maxTilesY * 0.6f) && spawnInfo.SpawnTileY < (Main.maxTilesY * 0.8f));
            bool oUnderworld = (spawnInfo.SpawnTileY >= (Main.maxTilesY * 0.8f));

            if (tsorcRevampWorld.SuperHardMode && (P.ZoneDirtLayerHeight || P.ZoneRockLayerHeight || oMagmaCavern))
            {
                if (Hallow && Main.rand.Next(20) == 1) return 1; //was 20
                if (spawnInfo.Player.ZoneGlowshroom && Main.rand.Next(20) == 1) return 1; //was 20
                if (spawnInfo.Player.ZoneUndergroundDesert && Main.rand.Next(20) == 1) return 1; //was 20
                if (Hallow && Main.bloodMoon && Main.rand.Next(6) == 1) return 1;
                if ((spawnInfo.SpawnTileX < Main.maxTilesX * 0.35f || spawnInfo.SpawnTileX > Main.maxTilesX * 0.75f) && Main.rand.Next(20) == 1) return 1; //was 10
                if (spawnInfo.SpawnTileType == TileID.BoneBlock && spawnInfo.Player.ZoneDungeon && Main.rand.Next(20) == 1)
                    return 0;
            }

            else if (tsorcRevampWorld.SuperHardMode && oUnderworld)
            {
                if (Main.rand.Next(60) == 1) return 1;
                if ((spawnInfo.SpawnTileX < Main.maxTilesX * 0.35f || spawnInfo.SpawnTileX > Main.maxTilesX * 0.75f) && Main.rand.Next(30) == 1) return 1;
                return 0;
            }
            return 0;
        }
        #endregion



        float strikeTimer = 0;
        float skeletonTimer = 0;
        public override void AI()
        {
            tsorcRevampAIs.FighterAI(NPC, 1.8f, 0.05f, canTeleport: true, lavaJumping: true);

            strikeTimer++;
            skeletonTimer++;
            bool lineOfSight = Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height);
            tsorcRevampAIs.SimpleProjectile(NPC, ref strikeTimer, 150, ModContent.ProjectileType<Projectiles.Enemy.EnemySpellSuddenDeathStrike>(), deathStrikeDamage, 8, lineOfSight && Main.rand.NextBool(), false, SoundID.Item17, 0);
            if (tsorcRevampAIs.SimpleProjectile(NPC, ref strikeTimer, 150, ModContent.ProjectileType<Projectiles.Enemy.EnemySpellEffectHealing>(), 0, 0, !lineOfSight, false, SoundID.Item17, 0))
            {
                NPC.life += 10;
                NPC.HealEffect(10);
                if (NPC.life > NPC.lifeMax) NPC.life = NPC.lifeMax;
            }

            //IF HIT BEFORE PINK DUST TELEGRAPH, RESET TIMER, BUT CHANCE TO BREAK STUN LOCK
            //(WORKS WITH 2 TELEGRAPH DUSTS IN DRAW, AT TIMER 60 AND 110)
            if (NPC.justHit && strikeTimer <= 109)
            {
                if (Main.rand.Next(3) == 0)
                {
                    strikeTimer = 110;
                }
                else
                {
                    strikeTimer = 0;
                }
            }
            if (NPC.justHit && Main.rand.Next(18) == 1)
            {
                tsorcRevampAIs.Teleport(NPC, 20, true);
                strikeTimer = 70f;
            }



            if (skeletonTimer > 300 && lineOfSight)
            {
                skeletonTimer = 0;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int spawnType;
                    if (Main.rand.NextBool())
                    {
                        spawnType = ModContent.NPCType<NPCs.Enemies.HollowSoldier>();
                    }
                    else
                    {
                        spawnType = ModContent.NPCType<NPCs.Enemies.SuperHardMode.HydrisElemental>(); //NPCID.ChaosElemental;
                    }

                    int spawnedNPC = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, spawnType, 0);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, spawnedNPC, 0f, 0f, 0f, 0);
                    }
                }
            }
        }

        #region DRAW
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //BlACK DUST is used to show stunlock worked, PINK is used to show unstoppable attack incoming
            //BLACK DUST
            if (strikeTimer >= 60)
            {
                Lighting.AddLight(NPC.Center, Color.WhiteSmoke.ToVector3() * 2f); //Pick a color, any color. The 0.5f tones down its intensity by 50%
                if (Main.rand.Next(2) == 1)
                {
                    //Dust.NewDust(npc.position, npc.width, npc.height, 41, npc.velocity.X, npc.velocity.Y); //41 wassss weird anti-gravity blue dust but now I'm seeing grass clippings; not sure what happened
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 54, (NPC.velocity.X * 0.2f), NPC.velocity.Y * 0.2f, 100, default, 1f); //54 is black smoke
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 54, (NPC.velocity.X * 0.2f), NPC.velocity.Y * 0.2f, 100, default, 2f);

                }
            }
            //PINK DUST
            if (strikeTimer >= 110)
            {
                Lighting.AddLight(NPC.Center, Color.WhiteSmoke.ToVector3() * 2f); //Pick a color, any color. The 0.5f tones down its intensity by 50%
                if (Main.rand.Next(2) == 1)
                {
                    int pink = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CrystalSerpent, NPC.velocity.X, NPC.velocity.Y, Scale: 1.5f);

                    Main.dust[pink].noGravity = true;
                }
            }
        }
        #endregion


        #region Gore
        public override void OnKill()
        {
            if (NPC.life <= 0)
            {
                if (!Main.dedServ)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Necromancer Gore 1").Type, 1.1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Necromancer Gore 2").Type, 1.1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Necromancer Gore 3").Type, 1.1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Necromancer Gore 2").Type, 1.1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Necromancer Gore 3").Type, 1.1f);
                }
            }
            Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<Items.DyingWindShard>(), 8 + Main.rand.Next(8));
        }
        #endregion
    }
}