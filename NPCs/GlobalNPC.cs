﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using tsorcRevamp.Items;
using tsorcRevamp.Items.Weapons.Ranged;
using tsorcRevamp.Items.Weapons.Throwing;

namespace tsorcRevamp.NPCs
{
    public class tsorcRevampGlobalNPC : GlobalNPC
    {


        float enemyValue;
        float multiplier = 1f;
        float divisorMultiplier = 1f;
        int DarkSoulQuantity;

        //Whatever custom expert scaling we want goes here. For reference 1 eliminates all expert mode doubling, and 2 is normal expert mode scaling.
        public static double expertScale = 2;

        public override bool InstancePerEntity => true;
        public bool DarkInferno = false;
        public bool CrimsonBurn = false;
        public bool ToxicCatDrain = false;
        public bool ResetToxicCatBlobs = false;
        public bool ViruCatDrain = false;
        public bool ResetViruCatBlobs = false;
        public bool BiohazardDrain = false;
        public bool ResetBiohazardBlobs = false;
        public bool ElectrocutedEffect = false;
        public bool PolarisElectrocutedEffect = false;
        public bool CrescentMoonlight = false;
        public bool Soulstruck = false;
        public bool PhazonCorruption = false;


        public override void ResetEffects(NPC npc)
        {
            DarkInferno = false;
            CrimsonBurn = false;
            ToxicCatDrain = false;
            ResetToxicCatBlobs = false;
            ViruCatDrain = false;
            ResetViruCatBlobs = false;
            BiohazardDrain = false;
            ResetBiohazardBlobs = false;
            ElectrocutedEffect = false;
            PolarisElectrocutedEffect = false;
            CrescentMoonlight = false;
            Soulstruck = false;
            PhazonCorruption = false;
        }


        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {            
            if (tsorcRevampWorld.TheEnd)
            {
                pool.Clear(); //stop NPC spawns in The End 
            }

            if (Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].wall == WallID.StarlitHeavenWallpaper)
            {
                pool.Clear();
                pool.Add(ModContent.NPCType<Enemies.HumanityPhantom>(), 10f);
            }

            //VANILLA NPC SPAWN EDITS
            if (spawnInfo.player.ZoneJungle && !tsorcRevampWorld.SuperHardMode)
            {
                //pool.Add(the type of the npc, what chance you want it to spawn with);
                pool.Add(NPCID.LostGirl, 0.02f);
            }

            if (spawnInfo.marble)
            {
                //pool.Add(NPCID.SolarCorite, 0.5f);
            }

            if (spawnInfo.spawnTileType == TileID.LihzahrdBrick && spawnInfo.lihzahrd) 
            {
                pool.Add(NPCID.DiabolistRed, 0.2f);
            }

            //SUPER HARD MODE SECTION
            if (spawnInfo.player.ZoneGlowshroom && tsorcRevampWorld.SuperHardMode)
            {
                pool.Add(NPCID.DD2LightningBugT3, 0.3f);
            }

            if (spawnInfo.player.ZoneUnderworldHeight && tsorcRevampWorld.SuperHardMode)
            {
                //pool.Add(NPCID.SolarSpearman, 1f);
                pool.Add(NPCID.SolarCrawltipedeHead, 0.2f); //.1 is 3%
                //pool.Add(NPCID.SolarDrakomire, 0.2f);
                pool.Add(NPCID.SolarSroller, 0.5f); //.5 is 16%
                pool.Add(NPCID.SolarCorite, 0.1f);
                //pool.Add(NPCID.SolarSolenian, 1f);
            }

            if (spawnInfo.spawnTileType == TileID.BoneBlock && spawnInfo.player.ZoneDungeon && tsorcRevampWorld.SuperHardMode)
            {
                
                pool.Add(NPCID.NebulaBrain, 0.2f); //.1 is 3%
                
            }

            if ((Math.Abs(spawnInfo.spawnTileX - Main.spawnTileX) > Main.maxTilesX / 3) && tsorcRevampWorld.SuperHardMode)
            //spawn tile is on one of the outer thirds of the map
            {

                //pool.Add(NPCID.GoblinShark, 0.2f); //.1 is 3%

            }
           

            if (spawnInfo.player.ZoneUnderworldHeight && spawnInfo.player.ZoneDungeon && tsorcRevampWorld.SuperHardMode)
            {
                pool.Add(NPCID.SolarSpearman, 1f);
                //pool.Add(NPCID.SolarCrawltipedeHead, 0.1f);
                pool.Add(NPCID.SolarDrakomire, 0.2f);
                //pool.Add(NPCID.SolarSroller, 0.5f);
                //pool.Add(NPCID.SolarCorite, 0.1f);
                pool.Add(NPCID.SolarSolenian, 2f);
            }
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.GetModPlayer<tsorcRevampPlayer>().BossZenBuff)
            {
                maxSpawns = 0;
            }


            //Peace candles do not activate if there is a) an invasion and b) the player is near the center of the world.
            if((Main.invasionType == 0 || player.Center.X > 82016 || player.Center.X < 74560 || player.Center.Y > 16000))
            {
                if (player.HasBuff(BuffID.PeaceCandle))
                {
                    maxSpawns = 0;
                }
            }
            else
            {
                if(Main.invasionType == 1)
                {
                    player.buffImmune[BuffID.PeaceCandle] = true;
                    player.ZonePeaceCandle = false;
                    spawnRate /= 2;
                    maxSpawns *= 3;
                }
            }

            if(player.ZoneTowerSolar || player.ZoneTowerNebula || player.ZoneTowerStardust || player.ZoneTowerVortex)
            {
                spawnRate /= 2;
                maxSpawns = (int)(maxSpawns * 1.5);
            }

            if (Main.tile[(int)player.position.X / 16, (int)player.position.Y / 16].wall == WallID.StarlitHeavenWallpaper)
            {
                spawnRate /= 10; //Origin of the Abyss. All spawns blocked other than Humanity Phantoms
            }

        }

        //vanilla npc changes moved to separate file

        public override void NPCLoot(NPC npc)
        {
            if (npc.boss)
            {
                foreach (Player player in Main.player)
                {
                    if (!player.active) { continue; }
                    player.GetModPlayer<tsorcRevampPlayer>().bossMagnet = true;
                    player.GetModPlayer<tsorcRevampPlayer>().bossMagnetTimer = 300; //5 seconds of increased grab range, check GlobalItem::GrabStyle and GrabRange
                }
            }

            #region Dark Souls & Consumable Souls Drops

            if (Soulstruck)
            {
                divisorMultiplier = 0.9f; //10% increase
            }

            if (npc.lifeMax > 5 && npc.value >= 10f || npc.boss)
            { //stop zero-value souls from dropping (the 'or boss' is for expert mode support)

                if (npc.netID != NPCID.JungleSlime)
                {
                    if (Main.expertMode)
                    { //npc.value is the amount of coins they drop
                        enemyValue = (int)npc.value / (divisorMultiplier * 25); //all enemies drop more money in expert mode, so the divisor is larger to compensate
                    }
                    else
                    {
                        enemyValue = (int)npc.value / (divisorMultiplier * 10);
                    }
                }

                if (npc.netID == NPCID.JungleSlime) //jungle slimes drop 10 souls
                {
                    if (Main.expertMode)
                    {
                        enemyValue = (int)npc.value / (divisorMultiplier * 125);
                    }
                    else
                    {
                        enemyValue = (int)npc.value / (divisorMultiplier * 50);
                    }
                }


                multiplier = tsorcRevampPlayer.CheckSoulsMultiplier(Main.LocalPlayer);

                DarkSoulQuantity = (int)(multiplier * enemyValue);

                #region Bosses drop souls once
                if (npc.boss)
                {
                    if (npc.type == NPCID.MoonLordCore)
                    { //moon lord does not drop coins in 1.3, so his value is 0, but in 1.4 he has a value of 1 plat
                        DarkSoulQuantity = 100000; //1 plat / 10
                    }

                    if (tsorcRevampWorld.Slain.ContainsKey(npc.type))
                    {
                        DarkSoulQuantity = 0;
                        return;
                    }
                    else
                    {
                        if (Main.netMode == NetmodeID.SinglePlayer)
                        {
                            Main.NewText("The souls of " + npc.GivenOrTypeName + " have been released!", 175, 255, 75); 
                            if (((npc.type == NPCID.EaterofWorldsHead) || (npc.type == NPCID.EaterofWorldsBody) || (npc.type == NPCID.EaterofWorldsTail)) && Main.invasionType == 0)
                            {
                                Main.StartInvasion();
                            }
                        }
                        else if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("The souls of " + npc.GivenOrTypeName + " have been released!"), new Color(175, 255, 75));
                            if(((npc.type == NPCID.EaterofWorldsHead) || (npc.type == NPCID.EaterofWorldsBody) || (npc.type == NPCID.EaterofWorldsTail)) && Main.invasionType == 0)
                            {
                                Main.StartInvasion();
                            }
                        }
                                                
                        tsorcRevampWorld.Slain.Add(npc.type, 0);
                        
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendData(MessageID.WorldData); //Slain only exists on the server. This tells the server to run NetSend(), which syncs this data with clients
                        }

                        if (Main.expertMode)
                        {
                            DarkSoulQuantity = 0;
                        }
                    }
                }
                #endregion

                #region EoW drops souls in a unique way
                if (((npc.type == NPCID.EaterofWorldsHead) || (npc.type == NPCID.EaterofWorldsBody) || (npc.type == NPCID.EaterofWorldsTail)))
                {

                    DarkSoulQuantity = 110;

                    if (Main.expertMode)
                    {
                        //EoW has 5 more segments in Expert mode, so its drops per segment is reduced slightly to keep it consistent. 
                        DarkSoulQuantity = 102;
                    }
                    if (NPC.downedBoss2)
                    {
                        //EoW still drops this many souls per segment even after the first kill. The difference between normal and expert is small enough it would get rounded away at this point.
                        DarkSoulQuantity = 10;
                    }

                    Item.NewItem(npc.getRect(), mod.ItemType("DarkSoul"), DarkSoulQuantity);
                    DarkSoulQuantity = 0;
                }
                #endregion

                if (DarkSoulQuantity > 0)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<DarkSoul>(), DarkSoulQuantity);
                }


                // Consumable Soul drops ahead - Current numbers give aprox. +20% souls

                float chance = 0.01f + (0.0005f * Main.LocalPlayer.GetModPlayer<tsorcRevampPlayer>().ConsSoulChanceMult);
                //Main.NewText(chance);

                if (!(npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.EaterofWorldsHead) && !ModContent.GetInstance<tsorcRevampConfig>().LegacyMode)
                {

                    if ((enemyValue >= 1) && (enemyValue <= 200) && (Main.rand.NextFloat() < chance)) // 1% chance of all enemies between enemyValue 1 and 200 dropping FadingSoul aka 1/75
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<FadingSoul>(), 1); // Zombies and eyes are 6 and 7 enemyValue, so will only drop FadingSoul
                    }

                    if ((enemyValue >= 15) && (enemyValue <= 2000) && (Main.rand.NextFloat() < chance)) // 1% chance of all enemies between enemyValue 10 and 2000 dropping LostUndeadSoul aka 1/75
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<LostUndeadSoul>(), 1); // Most pre-HM enemies fall into this category
                    }

                    if ((enemyValue >= 55) && (enemyValue <= 10000) && (Main.rand.NextFloat() < chance)) // 1% chance of all enemies between enemyValue 50 and 10000 dropping NamelessSoldierSoul aka 1/75
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<NamelessSoldierSoul>(), 1); // Most HM enemies fall into this category
                    }

                    if ((enemyValue >= 150) && (enemyValue <= 10000) && (Main.rand.NextFloat() < chance) && Main.hardMode) // 1% chance of all enemies between enemyValue 150 and 10000 dropping ProudKnightSoul aka 1/75
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<ProudKnightSoul>(), 1);
                    }
                }
                //End consumable souls drops
            }
            #endregion


            #region Event saving and custom drops code
            if(tsorcScriptedEvents.ActiveEvents != null && tsorcScriptedEvents.ActiveEvents.Count > 0) {
                foreach (ScriptedEvent thisEvent in tsorcScriptedEvents.ActiveEvents)
                {
                    if (thisEvent.spawnedNPC != null && thisEvent.spawnedNPC.active && thisEvent.spawnedNPC.whoAmI == npc.whoAmI)
                    {
                        thisEvent.npcDead = true;
                        if (thisEvent.CustomDrops != null && thisEvent.CustomDrops.Count > 0)
                        {
                            for (int j = 0; j < thisEvent.CustomDrops.Count; j++)
                            {
                                Item.NewItem(npc.Center, thisEvent.CustomDrops[j], thisEvent.DropAmounts[j]);
                            }
                        }
                    }
                    if (thisEvent.spawnedNPCs != null && thisEvent.spawnedNPCs.Count > 0)
                    {
                        for (int i = 0; i < thisEvent.spawnedNPCs.Count; i++)
                        {
                            if (thisEvent.spawnedNPCs[i].active && thisEvent.spawnedNPCs[i].whoAmI == npc.whoAmI)
                            {
                                thisEvent.deadNPCs[i] = true;
                                if (thisEvent.CustomDrops != null && thisEvent.CustomDrops.Count > 0)
                                {
                                    if (!thisEvent.onlyLastEnemy)
                                    {
                                        for (int j = 0; j < thisEvent.CustomDrops.Count; j++)
                                        {
                                            Item.NewItem(npc.Center, thisEvent.CustomDrops[j], thisEvent.DropAmounts[j]);
                                        }
                                    }
                                    else
                                    {
                                        bool oneAlive = false;
                                        foreach(bool thisBool in thisEvent.deadNPCs)
                                        {
                                            if(thisBool == false)
                                            {
                                                oneAlive = true;
                                            }
                                        }

                                        if (!oneAlive)
                                        {
                                            for (int j = 0; j < thisEvent.CustomDrops.Count; j++)
                                            {
                                                Item.NewItem(npc.Center, thisEvent.CustomDrops[j], thisEvent.DropAmounts[j]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.player[projectile.owner].GetModPlayer<tsorcRevampPlayer>().ConditionOverload)
            {
                int debuffCounter = 1;
                foreach (int buffType in npc.buffType)
                {

                    if (Main.debuff[buffType])
                    {
                        debuffCounter++;
                    }
                }
                damage = (int)Math.Pow(1.2, debuffCounter - 1);
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (player.GetModPlayer<tsorcRevampPlayer>().ConditionOverload)
            {
                int debuffCounter = 1;
                foreach (int buffType in npc.buffType)
                {

                    if (Main.debuff[buffType])
                    {
                        debuffCounter++;
                    }
                }
                damage = (int)Math.Pow(1.2, debuffCounter - 1);
            }
        }

        public override bool PreNPCLoot(NPC npc)
        {
            Player player = Main.LocalPlayer;

            if (ModContent.GetInstance<tsorcRevampConfig>().AdventureModeItems)
            {
                if (npc.type == NPCID.ChaosElemental)
                {
                    NPCLoader.blockLoot.Add(ItemID.RodofDiscord); //we dont want any sequence breaks, do we
                }
                if (npc.type == NPCID.KingSlime)
                {
                    NPCLoader.blockLoot.Add(ItemID.SlimeHook);
                    NPCLoader.blockLoot.Add(ItemID.SlimySaddle); //no lol
                }
                if (npc.type == NPCID.Golem)
                {
                    NPCLoader.blockLoot.Add(ItemID.Picksaw); //Goodnight sweet prince
                }
            }

            if (player.GetModPlayer<tsorcRevampStaminaPlayer>().staminaResourceCurrent < player.GetModPlayer<tsorcRevampStaminaPlayer>().staminaResourceMax2)
            {
                if (Main.rand.Next(2) == 0)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Items.StaminaDroplet>(), 1);
                }

                if (Main.rand.Next(12) == 0)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Items.StaminaDroplet>(), 1);
                }
            }

            return base.PreNPCLoot(npc);
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (DarkInferno)
            {

                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 16;
                if (damage < 2)
                {
                    damage = 2;
                }

                var N = npc;
                for (int j = 0; j < 6; j++)
                {
                    int dust = Dust.NewDust(N.position, N.width / 2, N.height / 2, 54, (N.velocity.X * 0.2f), N.velocity.Y * 0.2f, 100, default, 1f);
                    Main.dust[dust].noGravity = true;

                    int dust2 = Dust.NewDust(N.position, N.width / 2, N.height / 2, 58, (N.velocity.X * 0.2f), N.velocity.Y * 0.2f, 100, default, 1f);
                    Main.dust[dust2].noGravity = true;
                }
            }

            if (PhazonCorruption)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen = -8;

                int dust = Dust.NewDust(npc.position, npc.width, npc.height, 185, (npc.velocity.X * 0.2f), npc.velocity.Y * 0.2f, 100, default, 1f);
                Main.dust[dust].noGravity = true;

                int dust2 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.FireworkFountain_Blue, (npc.velocity.X * 0.2f), npc.velocity.Y * 0.2f, 100, default, 1f);
                Main.dust[dust2].noGravity = true;                
            }

            if (CrimsonBurn)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 24;
                if (Main.hardMode) npc.lifeRegen -= 24;
                if (damage < 2)
                {
                    damage = 2;
                }

            }

            if (ToxicCatDrain)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }

                int ToxicCatShotCount = 0;

                for (int i = 0; i < 1000; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p.active && p.type == ModContent.ProjectileType<Projectiles.ToxicCatShot>() && p.ai[0] == 1f && p.ai[1] == npc.whoAmI)
                    {
                        ToxicCatShotCount++;
                    }
                }
                if (ToxicCatShotCount >= 4)
                { //this is to make it worth the players time stickying more than 3 times
                    npc.lifeRegen -= ToxicCatShotCount * 3 * 2; //Use 1st N for damage, second N can be used to make it tick faster.
                    if (damage < ToxicCatShotCount * 1)
                    {
                        damage = ToxicCatShotCount * 1;
                    }
                }
                else
                {
                    npc.lifeRegen -= ToxicCatShotCount * 2 * 2;
                    if (damage < ToxicCatShotCount * 1)
                    {
                        damage = ToxicCatShotCount * 1;
                    }
                }
            }

            if (ViruCatDrain)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }

                int ViruCatShotCount = 0;

                for (int i = 0; i < 1000; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p.active && p.type == ModContent.ProjectileType<Projectiles.VirulentCatShot>() && p.ai[0] == 1f && p.ai[1] == npc.whoAmI)
                    {
                        ViruCatShotCount++;
                    }
                }
                if (ViruCatShotCount >= 4)
                {
                    npc.lifeRegen -= ViruCatShotCount * 3 * 5; //I use 1st N for damage, second N can be used to make it tick faster.
                    if (damage < ViruCatShotCount * 1)
                    {
                        damage = ViruCatShotCount * 1;
                    }
                }
                else
                {
                    npc.lifeRegen -= ViruCatShotCount * 2 * 5;
                    if (damage < ViruCatShotCount * 1)
                    {
                        damage = ViruCatShotCount * 1;
                    }
                }
            }

            if (BiohazardDrain)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }

                int BiohazardShotCount = 0;

                for (int i = 0; i < 1000; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p.active && p.type == ModContent.ProjectileType<Projectiles.BiohazardShot>() && p.ai[0] == 1f && p.ai[1] == npc.whoAmI)
                    {
                        BiohazardShotCount++;
                    }
                }
                if (BiohazardShotCount >= 4)
                {
                    npc.lifeRegen -= BiohazardShotCount * 9 * 4; //I use 1st N for damage, second N can be used to make it tick faster.
                    if (damage < BiohazardShotCount * 1)
                    {
                        damage = BiohazardShotCount * 1;
                    }
                }
                else
                {
                    npc.lifeRegen -= BiohazardShotCount * 6 * 4;
                    if (damage < BiohazardShotCount * 1)
                    {
                        damage = BiohazardShotCount * 1;
                    }
                }
            }

            if (ElectrocutedEffect)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 12;
                if (damage < 2)
                {
                    damage = 2;
                }
            }

            if (PolarisElectrocutedEffect)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 70;
                if (damage < 10)
                {
                    damage = 10;
                }
            }

            if (CrescentMoonlight)
            {
                if (!Main.hardMode)
                {
                    if (npc.lifeRegen > 0)
                    {
                        npc.lifeRegen = 0;
                    }
                    npc.lifeRegen -= 14;
                    if (damage < 2)
                    {
                        damage = 2;
                    }
                }
                else
                { //double the DoT in HM
                    if (npc.lifeRegen > 0)
                    {
                        npc.lifeRegen = 0;
                    }
                    npc.lifeRegen -= 28;
                    if (damage < 2)
                    {
                        damage = 2;
                    }
                }
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Merchant && !ModContent.GetInstance<tsorcRevampConfig>().LegacyMode)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Bottle); //despite being able to find the archeologist right after (who sells bottled water), it's nice to have
                nextSlot++;
            }
            if (type == NPCID.SkeletonMerchant && !ModContent.GetInstance<tsorcRevampConfig>().LegacyMode)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Firebomb>());
                shop.item[nextSlot].shopCustomPrice = 5;
                shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<EternalCrystal>());
                shop.item[nextSlot].shopCustomPrice = 2000;
                shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
                nextSlot++;
            }
            if (type == NPCID.GoblinTinkerer && !ModContent.GetInstance<tsorcRevampConfig>().LegacyMode)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Pulsar>());
                shop.item[nextSlot].shopCustomPrice = 800;
                shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ModContent.ItemType<ToxicCatalyzer>());
                shop.item[nextSlot].shopCustomPrice = 800;
                shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
                nextSlot++;
            }
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (npc.GetGlobalNPC<tsorcRevampGlobalNPC>().ToxicCatDrain && (projectile.type == ModContent.ProjectileType<Projectiles.ToxicCatDetonator>() || projectile.type == ModContent.ProjectileType<Projectiles.ToxicCatExplosion>()))
            {
                npc.GetGlobalNPC<tsorcRevampGlobalNPC>().ResetToxicCatBlobs = true;
                int tags;

                for (int i = 0; i < 1000; i++)
                {
                    tags = 0;
                    Projectile p = Main.projectile[i];
                    if (p.active && p.type == ModContent.ProjectileType<Projectiles.ToxicCatShot>() && p.ai[0] == 1f && p.timeLeft > 2 && p.ai[1] == npc.whoAmI)
                    {
                        for (int q = 0; q < 1000; q++)
                        {
                            Projectile ñ = Main.projectile[q];
                            if (ñ.active && ñ.type == ModContent.ProjectileType<Projectiles.ToxicCatShot>() && ñ.ai[0] == 1f && ñ.ai[1] == npc.whoAmI)
                            {
                                tags++;
                            }
                        }
                        float volume = (tags * 0.3f) + 0.7f;
                        float pitch = tags * 0.08f;
                        Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, SoundID.Item74.Style, volume, -pitch);

                        p.timeLeft = 2;

                        Projectile.NewProjectile(p.Center, npc.velocity, ModContent.ProjectileType<Projectiles.ToxicCatExplosion>(), (int)(projectile.damage * 1.8f), projectile.knockBack, projectile.owner, tags, 0);

                        int buffindex = npc.FindBuffIndex(ModContent.BuffType<Buffs.ToxicCatDrain>());

                        if (buffindex != -1)
                        {
                            npc.DelBuff(buffindex);
                        }
                    }
                }
            }

            if (npc.GetGlobalNPC<tsorcRevampGlobalNPC>().ViruCatDrain && (projectile.type == ModContent.ProjectileType<Projectiles.VirulentCatDetonator>() || projectile.type == ModContent.ProjectileType<Projectiles.VirulentCatExplosion>()))
            {
                npc.GetGlobalNPC<tsorcRevampGlobalNPC>().ResetViruCatBlobs = true;
                int tags;

                for (int i = 0; i < 1000; i++)
                {
                    tags = 0;
                    Projectile p = Main.projectile[i];
                    if (p.active && p.type == ModContent.ProjectileType<Projectiles.VirulentCatShot>() && p.ai[0] == 1f && p.timeLeft > 2 && p.ai[1] == npc.whoAmI)
                    {
                        for (int q = 0; q < 1000; q++)
                        {
                            Projectile ñ = Main.projectile[q];
                            if (ñ.active && ñ.type == ModContent.ProjectileType<Projectiles.VirulentCatShot>() && ñ.ai[0] == 1f && ñ.ai[1] == npc.whoAmI)
                            {
                                tags++;
                            }
                        }
                        float volume = (tags * 0.3f) + 0.7f;
                        float pitch = tags * 0.08f;
                        Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, SoundID.Item74.Style, volume, -pitch);

                        //Main.NewText(pitch);
                        p.timeLeft = 2;

                        Projectile.NewProjectile(p.Center, npc.velocity, ModContent.ProjectileType<Projectiles.VirulentCatExplosion>(), (projectile.damage * 2), projectile.knockBack, projectile.owner, tags, 0);

                        int buffindex = npc.FindBuffIndex(ModContent.BuffType<Buffs.ViruCatDrain>());

                        if (buffindex != -1)
                        {
                            npc.DelBuff(buffindex);
                        }
                    }
                }
            }

            if (npc.GetGlobalNPC<tsorcRevampGlobalNPC>().BiohazardDrain && (projectile.type == ModContent.ProjectileType<Projectiles.BiohazardDetonator>() || projectile.type == ModContent.ProjectileType<Projectiles.BiohazardExplosion>()))
            {
                npc.GetGlobalNPC<tsorcRevampGlobalNPC>().ResetBiohazardBlobs = true;
                int tags;

                for (int i = 0; i < 1000; i++)
                {
                    tags = 0;
                    Projectile p = Main.projectile[i];
                    if (p.active && p.type == ModContent.ProjectileType<Projectiles.BiohazardShot>() && p.ai[0] == 1f && p.timeLeft > 2 && p.ai[1] == npc.whoAmI)
                    {
                        for (int q = 0; q < 1000; q++)
                        {
                            Projectile ñ = Main.projectile[q];
                            if (ñ.active && ñ.type == ModContent.ProjectileType<Projectiles.BiohazardShot>() && ñ.ai[0] == 1f && ñ.ai[1] == npc.whoAmI)
                            {
                                tags++;
                            }
                        }
                        float volume = (tags * 0.3f) + 0.7f;
                        float pitch = tags * 0.08f;
                        Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, SoundID.Item74.Style, volume, -pitch);

                        p.timeLeft = 2;

                        Projectile.NewProjectile(p.Center, npc.velocity, ModContent.ProjectileType<Projectiles.BiohazardExplosion>(), (projectile.damage * 2), projectile.knockBack, projectile.owner, tags, 0);

                        int buffindex = npc.FindBuffIndex(ModContent.BuffType<Buffs.BiohazardDrain>());

                        if (buffindex != -1)
                        {
                            npc.DelBuff(buffindex);
                        }
                    }
                }
            }
        }



        //This method lets us scale the stats of NPC's in expert mode.
        public override void ScaleExpertStats(NPC npc, int numPlayers, float bossLifeScale)
        {
            //If it's not one of ours, don't mess with it.
            if ((npc.modNPC == null) || (npc.modNPC.mod != this.mod))
            {
                base.ScaleExpertStats(npc, numPlayers, bossLifeScale);
                return;
            }

            //If it's a boss, do nothing. Bosses will get their own scaling.
            if (npc.boss)
            {
                return;
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (ElectrocutedEffect)
            {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, 226, npc.velocity.X * 0f, npc.velocity.Y * 0f, 100, default(Color), .4f);
                Main.dust[dust].noGravity = true;
            }

            if (PolarisElectrocutedEffect)
            {
                for (int i = 0; i < 2; i++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, 226, npc.velocity.X * 0f, npc.velocity.Y * 0f, 100, default(Color), .4f);
                    Main.dust[dust].noGravity = true;
                }
                if (Main.rand.Next(2) == 0)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, 226, npc.velocity.X * 0f, npc.velocity.Y * 0f, 100, default(Color), .4f);
                    Main.dust[dust].noGravity = false;
                }
            }

            if (ToxicCatDrain)
            {
                drawColor = Color.LimeGreen;
                Lighting.AddLight(npc.position, 0.125f, 0.23f, 0.065f);

                if (Main.rand.Next(10) == 0)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, 74, npc.velocity.X * 0f, npc.velocity.Y * 0f, 100, default(Color), .8f); ;
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity += npc.velocity;
                    Main.dust[dust].fadeIn = 1f;
                }
            }

            if (ViruCatDrain)
            {
                drawColor = Color.LimeGreen;
                Lighting.AddLight(npc.position, 0.125f, 0.23f, 0.065f);

                if (Main.rand.Next(6) == 0)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, 74, npc.velocity.X * 0f, npc.velocity.Y * 0f, 100, default(Color), .8f); ;
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity += npc.velocity;
                    Main.dust[dust].fadeIn = 1f;
                }
            }

            if (BiohazardDrain)
            {
                drawColor = Color.LimeGreen;
                Lighting.AddLight(npc.position, 0.125f, 0.23f, 0.065f);

                if (Main.rand.Next(2) == 0)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, 74, npc.velocity.X * 0f, -2f, 100, default(Color), .8f); ;
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity += npc.velocity;
                    Main.dust[dust].fadeIn = 1f;
                }
            }

            if (CrescentMoonlight)
            {
                drawColor = Color.White;

                int dust = Dust.NewDust(npc.position, npc.width, npc.height, 164, npc.velocity.X * 0f, 0f, 100, default(Color), 1f); ;
                Main.dust[dust].velocity *= 0f;
                Main.dust[dust].noGravity = false;
                Main.dust[dust].velocity += npc.velocity;
            }

            if (Soulstruck)
            {
                Lighting.AddLight(npc.Center, .4f, .4f, .850f);

                if (Main.rand.Next(6) == 0)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, 68, 0, 0, 30, default(Color), 1.25f);
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity += npc.velocity;
                }
            }
        }

        //AIWorm(NPC npc, int headType, int[] bodyTypes, int tailType, int wormLength = 3, float partDistanceAddon = 0f, float maxSpeed = 8f, float gravityResist = 0.07f, bool fly = false, bool split = false, bool ignoreTiles = false, bool spawnTileDust = true, bool soundEffects = true)

        /*
                 * A cleaned up (and edited) copy of Worm AI.

                 * headType/tailType : the type of the head, body, and tail of the worm, respectively.
                 * bodyTypes: An array of the body types. NOTE: Array must at least be as long as the body length - 2!
                 * wormLength : the total length of the worm.
                 * partDistanceAddon : and addon to the distance between parts of the worm.
                 * maxSpeed : the fastest the worm can accellerate to.
                 * gravityResist : how much resistance on the X axis the worm has when it is out of tiles. was 0.07f
                    //higher values cause the wvyern's 'gravity' towards the player to increase
                    //lower values basically == longer passes
                 * fly : If true, acts like a Wvyern.
                 * split : If true, worm will split when parts of it die.
                 * ignoreTiles : If true, Allows the worm to move outside of tiles as if it were in them. (ignored if fly is true)
                 * spawnTileDust : If true, worm will spawn tile dust when it digs through tiles.
                 * soundEffects : If true, will produce a digging sound when nearing the player.

                 * that array works like this: say you have a worm that is 5 segments long
                 * you would make the body array have 3 ids in it and they would go in order they would appear on the worm from the head
                 * the array *must* be 2 less than the total length of the worm or it will not work
        */


        //ai[0] = ID of piece behind it
        //ai[1] = ID of piece ahead of it
        //ai[2] = Relates to length of worms
        //ai[3] = ID of worm head
        //npc.localAI[0] = place in the queue to sync itself, used to spread the syncing out
        #region AIWorm
        public static void AIWorm(NPC npc, int headType, int[] bodyTypes, int tailType, int wormLength = 3, float partDistanceAddon = 0f, float maxSpeed = 8f, float gravityResist = 0.07f, bool fly = false, bool split = false, bool ignoreTiles = false, bool spawnTileDust = true, bool soundEffects = true)
        {
            //Flip sprite so it's always facing the right way            
            if (npc.type == headType)
            {
                if (npc.velocity.X < 0f || Math.Abs(npc.velocity.X) < 0.1f)
                {
                    npc.spriteDirection = 1;
                }
                else if (npc.velocity.X > 0f)
                {
                    npc.spriteDirection = -1;
                }
            }
            else
            {
                if (npc.position.X > Main.npc[(int)npc.ai[1]].position.X || Math.Abs(npc.position.X - Main.npc[(int)npc.ai[1]].position.X) < 0.1f)
                {
                    npc.spriteDirection = 1;
                }
                if (npc.position.X < Main.npc[(int)npc.ai[1]].position.X)
                {
                    npc.spriteDirection = -1;
                }
            }
            //If it splits, ignore the health of the head and keep its own healthbar
            //If it doesn't, set its real health to the health of the head
            if (split)
            {
                npc.realLife = -1;
            }
            else if (npc.ai[3] > 0f) { 
                npc.realLife = (int)npc.ai[3];
            }

            //Don't do *any* spawning if we're a multiplayer client
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                //Tick down the sync counter, and if it hits 1 then sync them.
                if(npc.localAI[0] == 1 && npc.localAI[0] > 0)
                {
                    npc.netUpdate = true;
                    npc.localAI[0] = -1;
                }
                else
                {
                    npc.localAI[0]--;
                }

                //And the piece behind it does not exist
                if (npc.ai[0] == 0f)
                {
                    //If we're a head and flying type, spawn the rest of the worm
                    if (fly && npc.type == headType)
                    {
                        //Set its the head's head id, actual health, and ID to itself
                        npc.ai[3] = (float)npc.whoAmI;
                        npc.realLife = npc.whoAmI;

                        //Store the head's index in npcID. This will get updated as we go through each piece.
                        int npcID = npc.whoAmI;

                        //Spawn the rest of the worm. For each piece...
                        for (int m = 0; m < wormLength - 1; m++)
                        {
                            //If we're the last piece, make the worm type the tail. If not, make it the body type corrosponding to its position on the list
                            int npcType = (m == wormLength - 2 ? tailType : bodyTypes[m]);

                            //Spawn the npc
                            int newnpcID = NPC.NewNPC((int)(npc.Center.X), (int)(npc.Center.Y), npcType, npc.whoAmI);

                            //Set the new piece's Head ID to the head
                            Main.npc[newnpcID].ai[3] = (float)npc.whoAmI;

                            //Set its real health to the head's
                            Main.npc[newnpcID].realLife = npc.whoAmI;

                            //Set its "previous piece id" to the id of the previous spawned piece
                            Main.npc[newnpcID].ai[1] = (float)npcID;

                            //Set the previous piece's "next piece id" to the id of the newly spawned piece
                            Main.npc[npcID].ai[0] = (float)newnpcID;

                            //Set their localAI to a number that grows as each segment is spawned
                            Main.npc[npcID].localAI[0] = 2 + (m * 2);

                            //Ask the server to sync it right away (might be triggering the net spam limit and causing the issues!!)
                            //NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, newnpcID);

                            //Store the current piece's ID in npcID, so that the next piece can use it
                            npcID = newnpcID;
                        }
                        //Immediately update
                        npc.netUpdate = true;
                    }
                    //If we're a grounded type and not the tail, just spawn the piece behind itself
                    else if (npc.type != tailType)
                    {
                        if (npc.type == headType)
                        {
                            if (!split)
                            {
                                npc.ai[3] = (float)npc.whoAmI;
                                npc.realLife = npc.whoAmI;
                            }
                            npc.ai[2] = (float)(wormLength - 2);
                            int nextPiece = (bodyTypes.Length == 0 ? tailType : bodyTypes[0]);
                            npc.ai[0] = (float)NPC.NewNPC((int)(npc.Center.X), (int)(npc.Center.Y), nextPiece, npc.whoAmI);
                        }
                        else
                        if ((npc.type != headType && npc.type != tailType) && npc.ai[2] > 0f)
                        {
                            npc.ai[0] = (float)NPC.NewNPC((int)(npc.Center.X), (int)(npc.Center.Y), bodyTypes[wormLength - 3 - (int)npc.ai[2]], npc.whoAmI);
                        }
                        else
                        {
                            npc.ai[0] = (float)NPC.NewNPC((int)(npc.Center.X), (int)(npc.Center.Y), tailType, npc.whoAmI);
                        }
                        if (!split)
                        {
                            Main.npc[(int)npc.ai[0]].ai[3] = npc.ai[3];
                            Main.npc[(int)npc.ai[0]].realLife = npc.realLife;
                        }
                        Main.npc[(int)npc.ai[0]].ai[1] = (float)npc.whoAmI;
                        Main.npc[(int)npc.ai[0]].ai[2] = npc.ai[2] - 1f;
                        npc.netUpdate = true;
                    }
                }

                //if npc can split, check if pieces are dead and if so split.
                if (split)
                {
                    //If the piece in front and behind it are dead, then die too
                    if (!Main.npc[(int)npc.ai[1]].active && !Main.npc[(int)npc.ai[0]].active)
                    {
                        npc.life = 0;
                        npc.HitEffect(0, 10.0);
                        npc.active = false;
                    }

                    //If it's a head and the piece behind it dies, then die
                    if (npc.type == headType && !Main.npc[(int)npc.ai[0]].active)
                    {
                        npc.life = 0;
                        npc.HitEffect(0, 10.0);
                        npc.active = false;
                    }

                    //If it's a tail and the piece in front of it dies, then die
                    if (npc.type == tailType && !Main.npc[(int)npc.ai[1]].active)
                    {
                        npc.life = 0;
                        npc.HitEffect(0, 10.0);
                        npc.active = false;
                    }

                    //If the piece isn't a head or tail, and the piece in front of it dies, then become a head
                    if ((npc.type != headType && npc.type != tailType) && !Main.npc[(int)npc.ai[1]].active)
                    {
                        npc.type = headType;
                        int npcID = npc.whoAmI;
                        float lifePercent = (float)npc.life / (float)npc.lifeMax;
                        float lastPiece = npc.ai[0];
                        npc.SetDefaults(npc.type, -1f);
                        npc.life = (int)((float)npc.lifeMax * lifePercent);
                        npc.ai[0] = lastPiece;
                        npc.netUpdate = true;
                        npc.whoAmI = npcID;
                    }

                    //If the piece isn't a head or tail, and the piece behind it dies, then become a head
                    else if ((npc.type != headType && npc.type != tailType) && !Main.npc[(int)npc.ai[0]].active)
                    {
                        npc.type = tailType;
                        int npcID = npc.whoAmI;
                        float lifePercent = (float)npc.life / (float)npc.lifeMax;
                        float lastPiece = npc.ai[1];
                        npc.SetDefaults(npc.type, -1f);
                        npc.life = (int)((float)npc.lifeMax * lifePercent);
                        npc.ai[1] = lastPiece;
                        npc.netUpdate = true;
                        npc.whoAmI = npcID;
                    }
                }

                //If it can't split, die if it is incomplete 
                else
                {
                    //If it's not a head and the piece in front of it is dead (or the wrong aiStyle, just in-case a new npc took its slot) then die
                    if (npc.type != headType && (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].aiStyle != npc.aiStyle))
                    {
                        npc.life = 0;
                        npc.HitEffect(0, 10.0);

                        npc.active = false;
                    }

                    //If it's not a tail and the piece behind it is dead then die
                    if (npc.type != tailType && (!Main.npc[(int)npc.ai[0]].active || Main.npc[(int)npc.ai[0]].aiStyle != npc.aiStyle))
                    {
                        npc.life = 0;
                        npc.HitEffect(0, 10.0);

                        npc.active = false;
                    }
                }
                /**
                if (!npc.active && Main.netMode == NetmodeID.Server) 
                { 
                    NetMessage.SendData(28, -1, -1, "", npc.whoAmI, 1, 0f, 0f, -1); 
                }**/
            }
            int tileX = (int)(npc.position.X / 16f) - 1;
            int tileCenterX = (int)((npc.Center.X) / 16f) + 2;
            int tileY = (int)(npc.position.Y / 16f) - 1;
            int tileCenterY = (int)((npc.Center.Y) / 16f) + 2;
            if (tileX < 0) { tileX = 0; }
            if (tileCenterX > Main.maxTilesX) { tileCenterX = Main.maxTilesX; }
            if (tileY < 0) { tileY = 0; }
            if (tileCenterY > Main.maxTilesY) { tileCenterY = Main.maxTilesY; }
            bool canMove = false;
            if (fly || ignoreTiles) { canMove = true; }


            if (!canMove || spawnTileDust)
            {
                for (int tX = tileX; tX < tileCenterX; tX++)
                {
                    for (int tY = tileY; tY < tileCenterY; tY++)
                    {
                        if (Main.tile[tX, tY] != null && ((Main.tile[tX, tY].active() && (Main.tileSolid[(int)Main.tile[tX, tY].type] || (Main.tileSolidTop[(int)Main.tile[tX, tY].type] && Main.tile[tX, tY].frameY == 0))) || Main.tile[tX, tY].liquid > 64))
                        {
                            Vector2 tPos;
                            tPos.X = (float)(tX * 16);
                            tPos.Y = (float)(tY * 16);
                            if (npc.position.X + (float)npc.width > tPos.X && npc.position.X < tPos.X + 16f && npc.position.Y + (float)npc.height > tPos.Y && npc.position.Y < tPos.Y + 16f)
                            {
                                canMove = true;
                                if (spawnTileDust && (Main.rand.Next(100)) == 0 && Main.tile[tX, tY].active())
                                {
                                    WorldGen.KillTile(tX, tY, true, true, false);
                                }
                            }
                        }
                    }
                }
            }


            if (!canMove && npc.type == headType)
            {
                Rectangle rectangle = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                int playerCheckDistance = 1000;
                bool canMove2 = true;
                for (int m3 = 0; m3 < 255; m3++)
                {
                    if (Main.player[m3].active)
                    {
                        Rectangle rectangle2 = new Rectangle((int)Main.player[m3].position.X - playerCheckDistance, (int)Main.player[m3].position.Y - playerCheckDistance, playerCheckDistance * 2, playerCheckDistance * 2);
                        if (rectangle.Intersects(rectangle2))
                        {
                            canMove2 = false;
                            break;
                        }
                    }
                }
                if (canMove2) { canMove = true; }
            }
            


            Vector2 npcCenter = npc.Center;
            float playerCenterX = Main.player[npc.target].Center.X;
            float playerCenterY = Main.player[npc.target].Center.Y;
            playerCenterX = (float)((int)(playerCenterX / 16f) * 16); playerCenterY = (float)((int)(playerCenterY / 16f) * 16);
            npcCenter.X = (float)((int)(npcCenter.X / 16f) * 16); npcCenter.Y = (float)((int)(npcCenter.Y / 16f) * 16);
            playerCenterX -= npcCenter.X; playerCenterY -= npcCenter.Y;
            float dist = (float)Math.Sqrt((double)(playerCenterX * playerCenterX + playerCenterY * playerCenterY));
            if (npc.ai[1] > 0f && npc.ai[1] < (float)Main.npc.Length)
            {
                
                npcCenter = npc.Center;
                float offsetX = Main.npc[(int)npc.ai[1]].Center.X - npcCenter.X;
                float offsetY = Main.npc[(int)npc.ai[1]].Center.Y - npcCenter.Y;
                
                npc.rotation = (float)Math.Atan2((double)offsetY, (double)offsetX) + 1.57f;
                dist = (float)Math.Sqrt((double)(offsetX * offsetX + offsetY * offsetY));
                dist = (dist - (float)npc.width - (float)partDistanceAddon) / dist;
                offsetX *= dist;
                offsetY *= dist;
                npc.velocity = default(Vector2);
                npc.position.X = npc.position.X + offsetX;
                npc.position.Y = npc.position.Y + offsetY;                
            }
            else
            {
                if (!canMove)
                {
                    npc.velocity.Y = npc.velocity.Y + 0.11f;
                    if (npc.velocity.Y > maxSpeed) { npc.velocity.Y = maxSpeed; }
                    if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)maxSpeed * 0.4)
                    {
                        if (npc.velocity.X < 0f) { npc.velocity.X = npc.velocity.X - gravityResist * 1.1f; } else { npc.velocity.X = npc.velocity.X + gravityResist * 1.1f; }
                    }
                    else
                    if (npc.velocity.Y == maxSpeed)
                    {
                        if (npc.velocity.X < playerCenterX) { npc.velocity.X = npc.velocity.X + gravityResist; }
                        else
                        if (npc.velocity.X > playerCenterX) { npc.velocity.X = npc.velocity.X - gravityResist; }
                    }
                    else
                    if (npc.velocity.Y > 4f)
                    {
                        if (npc.velocity.X < 0f) { npc.velocity.X = npc.velocity.X + gravityResist * 0.9f; } else { npc.velocity.X = npc.velocity.X - gravityResist * 0.9f; }
                    }
                }
                else
                {
                    if (soundEffects && npc.soundDelay == 0)
                    {
                        float distSoundDelay = dist / 40f;
                        if (distSoundDelay < 10f) { distSoundDelay = 10f; }
                        if (distSoundDelay > 20f) { distSoundDelay = 20f; }
                        npc.soundDelay = (int)distSoundDelay;
                        Main.PlaySound(SoundID.Roar, (int)npc.position.X, (int)npc.position.Y, 1);
                    }
                    dist = (float)Math.Sqrt((double)(playerCenterX * playerCenterX + playerCenterY * playerCenterY));
                    float absPlayerCenterX = Math.Abs(playerCenterX);
                    float absPlayerCenterY = Math.Abs(playerCenterY);
                    float newSpeed = maxSpeed / dist;
                    playerCenterX *= newSpeed;
                    playerCenterY *= newSpeed;
                    bool dontFall = false;
                    if (fly)
                    {
                        if (((npc.velocity.X > 0f && playerCenterX < 0f) || (npc.velocity.X < 0f && playerCenterX > 0f) || (npc.velocity.Y > 0f && playerCenterY < 0f) || (npc.velocity.Y < 0f && playerCenterY > 0f)) && Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) > gravityResist / 2f && dist < 300f)
                        {
                            dontFall = true;
                            if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < maxSpeed) { npc.velocity *= 1.1f; }
                        }
                    }
                    if (!dontFall)
                    {
                        if ((npc.velocity.X > 0f && playerCenterX > 0f) || (npc.velocity.X < 0f && playerCenterX < 0f) || (npc.velocity.Y > 0f && playerCenterY > 0f) || (npc.velocity.Y < 0f && playerCenterY < 0f))
                        {
                            if (npc.velocity.X < playerCenterX) { npc.velocity.X = npc.velocity.X + gravityResist; }
                            else
                            if (npc.velocity.X > playerCenterX) { npc.velocity.X = npc.velocity.X - gravityResist; }
                            if (npc.velocity.Y < playerCenterY) { npc.velocity.Y = npc.velocity.Y + gravityResist; }
                            else
                            if (npc.velocity.Y > playerCenterY) { npc.velocity.Y = npc.velocity.Y - gravityResist; }
                            if ((double)Math.Abs(playerCenterY) < (double)maxSpeed * 0.2 && ((npc.velocity.X > 0f && playerCenterX < 0f) || (npc.velocity.X < 0f && playerCenterX > 0f)))
                            {
                                if (npc.velocity.Y > 0f) { npc.velocity.Y = npc.velocity.Y + gravityResist * 2f; } else { npc.velocity.Y = npc.velocity.Y - gravityResist * 2f; }
                            }
                            if ((double)Math.Abs(playerCenterX) < (double)maxSpeed * 0.2 && ((npc.velocity.Y > 0f && playerCenterY < 0f) || (npc.velocity.Y < 0f && playerCenterY > 0f)))
                            {
                                if (npc.velocity.X > 0f) { npc.velocity.X = npc.velocity.X + gravityResist * 2f; } else { npc.velocity.X = npc.velocity.X - gravityResist * 2f; }
                            }
                        }
                        else
                        if (absPlayerCenterX > absPlayerCenterY)
                        {
                            if (npc.velocity.X < playerCenterX) { npc.velocity.X = npc.velocity.X + gravityResist * 1.1f; }
                            else
                            if (npc.velocity.X > playerCenterX) { npc.velocity.X = npc.velocity.X - gravityResist * 1.1f; }

                            if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)maxSpeed * 0.5)
                            {
                                if (npc.velocity.Y > 0f) { npc.velocity.Y = npc.velocity.Y + gravityResist; } else { npc.velocity.Y = npc.velocity.Y - gravityResist; }
                            }
                        }
                        else
                        {
                            if (npc.velocity.Y < playerCenterY) { npc.velocity.Y = npc.velocity.Y + gravityResist * 1.1f; }
                            else
                            if (npc.velocity.Y > playerCenterY) { npc.velocity.Y = npc.velocity.Y - gravityResist * 1.1f; }
                            if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)maxSpeed * 0.5)
                            {
                                if (npc.velocity.X > 0f) { npc.velocity.X = npc.velocity.X + gravityResist; } else { npc.velocity.X = npc.velocity.X - gravityResist; }
                            }
                        }
                    }
                }
                npc.rotation = (float)Math.Atan2((double)npc.velocity.Y, (double)npc.velocity.X) + 1.57f;
                if (npc.type == headType)
                {
                    if (canMove)
                    {
                        if (npc.localAI[0] != 1f) { npc.netUpdate = true; }
                        npc.localAI[0] = 1f;
                    }
                    else
                    {
                        if (npc.localAI[0] != 0f) { npc.netUpdate = true; }
                        npc.localAI[0] = 0f;
                    }
                    if (((npc.velocity.X > 0f && npc.oldVelocity.X < 0f) || (npc.velocity.X < 0f && npc.oldVelocity.X > 0f) || (npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f) || (npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f)) && !npc.justHit)
                    {
                        npc.netUpdate = true;
                        return;
                    }
                }
            }
        }

        #endregion

        


    }

    public class PortedAIs
    {
        ///<summary> 
        ///A port of the teleporter AI from Omnir's mod, which is itself an edited custom version of Grox's teleporter AI.
        ///</summary>         
        ///<param name="ai">A float array that stores AI data. (Note this array should be synced!)</param>
        ///<param name="immobile">Whether or not this NPC should move while its teleporting.</param>
        ///<param name="tpRadius">Radius around the player where the NPC will try to move.</param>
        ///<param name="distFromPlayer">Minimum distance to keep from the player as the NPC teleports.</param>
        ///<param name="tpInterval">How often the NPC will try to teleport, tied to npc.ai[3].</param>
        ///<param name="aerial">Whether or not an NPC will try to move to an airborne position.</param>
        ///<param name="tpEffects">The effect that the NPC will create as it moves.</param>
        ///<param name="tpConstant">If true, the NPC will constantly teleport.  If false, it will only teleport if bored. NOTE: If an NPC has tpConstant = false and does not have fighter AI, it will never teleport!</param>
        public static void teleporterAI(NPC npc, ref float[] ai, bool immobile = true, int tpRadius = 20, int distFromPlayer = 4, int tpInterval = 650, bool aerial = true, Action<bool> tpEffects = null, bool tpConstant = false)
        {
            npc.TargetClosest(true);
            Vector2 telePos = new Vector2(0, 0);
            if (immobile)
            {
                npc.velocity.X *= 0.93f;
                if ((double)Math.Abs(npc.velocity.X) > 0.1) npc.velocity.X = 0f;
            }
            if (tpConstant) ai[3]++;
            if (ai[3] >= tpInterval)
            {
                int playerTileX = (int)Main.player[npc.target].position.X / 16;
                int playerTileY = (int)Main.player[npc.target].position.Y / 16;
                int tileX = (int)npc.position.X / 16;
                int tileY = (int)npc.position.Y / 16;
                int tpCheck = 0;
                bool hasTP = false;
                if (Vector2.Distance(npc.Center, Main.player[npc.target].Center) > 2000f)
                {
                    tpCheck = 100;
                    hasTP = true;
                }
                while (!hasTP && tpCheck < 100)
                {
                    tpCheck++;
                    //Syncedrand would go here but... RIP
                    //It does netupdate after teleporting, which should keep the desyncs small
                    int tpTileX = Main.rand.Next(playerTileX - tpRadius, playerTileX + tpRadius);
                    int tpTileY = Main.rand.Next(playerTileY - tpRadius, playerTileY + tpRadius);
                    for (int tpY = tpTileY; tpY < playerTileY + tpRadius; tpY++)
                    {
                        if ((tpY < playerTileY - distFromPlayer || tpY > playerTileY + distFromPlayer || tpTileX < playerTileX - distFromPlayer || tpTileX > playerTileX + distFromPlayer) && (tpY < tileY - 1 || tpY > tileY + 1 || tpTileX < tileX - 1 || tpTileX > tileX + 1) && Main.tile[tpTileX, tpY].active())
                        {
                            bool safe = true;
                            if (Main.tile[tpTileX, tpY - 1].lava() && !npc.lavaImmune) safe = false;
                            if (safe && (Main.tileSolid[(int)Main.tile[tpTileX, tpY].type] || aerial) && !Collision.SolidTiles(tpTileX - 1, tpTileX + 1, tpY - 4, tpY - 1))
                            {
                                telePos.X = (float)(tpTileX * 16f - (float)(npc.width / 2) + 8f);
                                telePos.Y = (aerial) ? (float)(tpY * 16f - (float)npc.height) - 65 : (float)(tpY * 16f - (float)npc.height);
                                hasTP = true;
                                ai[3] = -120f;
                                break;
                            }
                        }
                    }
                }
                npc.netUpdate = true;
            }
            if (ai[3] == -120f && telePos != new Vector2(0, 0))
            {
                if (tpEffects != null) { tpEffects(true); }
                npc.position.X = telePos.X;
                npc.position.Y = telePos.Y;
                npc.velocity *= 0f;
                ai[3] = 0f;
                if (tpEffects != null) { tpEffects(false); }
            }
        }




        /*
         * A heavily edited custom version of aiStyle 3 (parenthesis indicate default values)
         * 
         * ai : 				    A float array that stores AI data. (Note this array should be synced!)
         * nocturnal (false) :	    If true, flees when it is daytime.
         * focused (false) : 	    If true, npc wont get interrupted when hit or confused.
         * boredom (60) : 		    The amount of ticks until the npc gets 'bored' following a target.
         * knockPower (0) :		    0 == do not interact with doors, attempt to open the doors by this value, negative numbers will break instead
         * accel (0.07f):   	    The rate velocity X increases by when moving.
         * topSpeed (1f) :	 	    The maximum velocity on the X axis.
         * leapReq (0) :	   	    -1 npc wont jump over gaps, more than 0 npc will leap at players
         * leapSpeed (3) :          The max tiles it can jump across. 
         * leapHeight (4) :         The max tiles it can jump over. 
         * leapRangeX (100) :       The X distance from a player before the npc initiates leap. 
         * leapRangeY (50) :  	    The Y distance from a player before the npc initiates leap. 
         * shotType (0) : 		   	If higher than 0, allows an npc to fire a projectile.
         * shotRate (70) : 			The rate of fire of the projectile, if there is one.
         * shotPow (-1) : 			The projectile's damage, if -1 it will use the projectile's default.
         * shotSpeed (11) : 		The projectile's velocity.
         */

        ///<summary> 
        ///A port of the fighterAI from Omnir's mod, which is itself a heavily edited custom version of vanilla aiStyle 3.
        ///</summary>         
        ///<param name="ai">A float array that stores AI data. (Note this array should be synced!)</param>
        ///<param name="nocturnal">If true, flees when it is daytime.</param>
        ///<param name="focused">If true, npc wont get interrupted when hit or confused.</param>
        ///<param name="boredom">The amount of ticks until the npc gets 'bored' following a target.</param>
        ///<param name="knockPower">0 == do not interact with doors, attempt to open the doors by this value, negative numbers will break instead</param>
        ///<param name="accel">The rate velocity X increases by when moving.</param>
        ///<param name="topSpeed">The maximum velocity on the X axis.</param>
        ///<param name="leapReq">-1 npc wont jump over gaps, more than 0 npc will leap at players</param>
        ///<param name="leapSpeed">The max tiles it can jump across. </param>
        ///<param name="leapHeight">The max tiles it can jump over. </param>
        ///<param name="leapRangeX">The X distance from a player before the npc initiates leap. </param>
        ///<param name="leapRangeY">The Y distance from a player before the npc initiates leap. </param>
        ///<param name="shotType">If higher than 0, allows an npc to fire a projectile.</param>
        ///<param name="shotRate">The rate of fire of the projectile, if there is one.</param>
        ///<param name="shotPow">The projectile's damage, if -1 it will use the projectile's default.</param>
        ///<param name="shotSpeed">The projectile's velocity.</param>
        public static void fighterAI(NPC npc, ref float[] ai, bool nocturnal = true, bool focused = false, int boredom = 60, int knockPower = 0, float accel = 0.07f, float topSpeed = 1f, float leapReq = 0, float leapSpeed = 3, float leapHeight = 4, float leapRangeX = 100, float leapRangeY = 50, int shotType = 0, float shotRate = 70, int shotPow = -1, float shotSpeed = 11)
        {
            bool moonwalking = false;
            //This block of code checks for major X velocity/directional changes as well as periodically updates the npc.
            if (npc.velocity.Y == 0f && ((npc.velocity.X > 0f && npc.direction < 0) || (npc.velocity.X < 0f && npc.direction > 0)))
                moonwalking = true;
            if (shotType <= 0 || npc.ai[2] <= 0f)  //  loop to set ai[3] (boredom)
            {
                if (npc.position.X == npc.oldPosition.X || ai[3] >= (float)boredom || moonwalking)
                { ai[3] += 1f; }
                else if ((double)Math.Abs(npc.velocity.X) > 0.9 && ai[3] > 0f)
                { ai[3] -= 1f; }
                if ((npc.justHit && !focused) || ai[3] > (float)(boredom * 10)) //focused NPCs don't reset their boredom/teleport timer when hit
                { ai[3] = 0f; }
                if (ai[3] == (float)boredom)
                { npc.netUpdate = true; }
            }
            if (!focused && (npc.justHit || npc.confused))
            {
                if (shotType > 0)
                { npc.localAI[0] = (float)shotRate / 3; } // shot on .5 sec cooldown
                npc.ai[2] = 0;
            }
            bool notBored = ai[3] < (float)boredom;
            //if npc does not flee when it's day, if is night, or npc is not on the surface and it hasn't updated projectile pass, update target.
            if ((!nocturnal || !Main.dayTime || (double)npc.position.Y > Main.worldSurface * 16.0) && notBored)
            { npc.TargetClosest(true); }
            else if (shotType <= 0 || ai[2] <= 0f)//if 'bored'
            {
                if (nocturnal && Main.dayTime && (double)(npc.position.Y / 16f) < Main.worldSurface && npc.timeLeft > 10)
                { npc.timeLeft = 10; }
                if (npc.velocity.X == 0f)
                {
                    if (npc.velocity.Y == 0f)
                    {
                        if (npc.ai[0] == 0f)
                        { npc.ai[0] = 1f; }
                        else
                        {
                            npc.direction *= -1;
                            npc.spriteDirection = npc.direction;
                            npc.ai[0] = 0f;
                        }
                    }
                }
                else
                { ai[0] = 0f; }
                if (npc.direction == 0)
                { npc.direction = 1; }
            }
            //if velocity is less than -1 or greater than 1...
            if (shotType <= 0 || (npc.ai[2] <= 0f && !npc.confused))  //  melee attack/movement. shotType > 0s only use while not aiming
            {
                if (Math.Abs(npc.velocity.X) > topSpeed)  //  running/flying faster than top speed
                {
                    if (npc.velocity.Y == 0f)  //  and not jump/fall
                    { npc.velocity *= .8f; }  //  decelerate
                }
                else if ((npc.velocity.X < topSpeed && npc.direction == 1) || (npc.velocity.X > -topSpeed && npc.direction == -1))
                {  //  running slower than top speed (forward), can be jump/fall
                    npc.velocity.X = npc.velocity.X + (float)npc.direction * accel;  //  accellerate fwd; can happen midair
                    if ((float)npc.direction * npc.velocity.X > topSpeed)
                    { npc.velocity.X = (float)npc.direction * topSpeed; }  //  but cap at top speed
                }  //  END running slower than top speed (forward), can be jump/fall
            } // END shotType <= 0 or not aiming
            if (shotType > 0)
            {
                if (!npc.confused)
                {
                    float distance = npc.Distance(Main.player[npc.target].Center);
                    Vector2 angle = Main.player[npc.target].Center - npc.Center;
                    angle.Y = angle.Y - (Math.Abs(angle.X) * .1f);
                    angle.X += (float)Main.rand.Next(-40, 41);
                    angle.Y += (float)Main.rand.Next(-40, 41);
                    angle.Normalize();
                    angle *= shotSpeed;
                    if (npc.localAI[0] > 0f)
                    { npc.localAI[0] -= 1f; } // decrement fire & reload counter
                    if (npc.ai[2] > 0f) // if aiming: adjust aim and fire if needed
                    {
                        npc.TargetClosest(true); // target and face closest player
                        if (npc.localAI[0] == (float)(shotRate / 2))  //  fire at halfway through; first half of delay is aim, 2nd half is cooldown
                        {
                            int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, angle.X, angle.Y, shotType, shotPow, 0f, Main.myPlayer, -1);
                            Main.projectile[proj].netUpdate = true;
                            if (Math.Abs(angle.Y) > Math.Abs(angle.X) * 2f) // target steeply above/below NPC
                            {
                                if (angle.Y > 0f)
                                { npc.ai[2] = 1f; } // aim downward
                                else
                                { npc.ai[2] = 5f; } // aim upward
                            }
                            else if (Math.Abs(angle.X) > Math.Abs(angle.Y) * 2f) // target on level with NPC
                            { npc.ai[2] = 3f; }  //  aim straight ahead
                            else if (angle.Y > 0f) // target is below NPC
                            { npc.ai[2] = 2f; }  //  aim slight downward
                            else // target is not below NPC
                            { npc.ai[2] = 4f; }  //  aim slight upward
                        } // END firing
                        if (npc.velocity.Y != 0f || npc.localAI[0] <= 0f) // jump/fall or firing reload
                        {
                            npc.ai[2] = 0f; // not aiming
                            npc.localAI[0] = 0f; // reset firing/reload counter (necessary? nonzero maybe)
                        }
                        else // no jump/fall and no firing reload
                        {
                            npc.velocity.X *= 0.9f; // decelerate to stop & shoot
                            npc.spriteDirection = npc.direction; // match animation to facing
                        }
                    } // END if aiming: adjust aim and fire if needed
                    if (npc.ai[2] <= 0f && npc.velocity.Y == 0f && npc.localAI[0] <= 0f && !Main.player[npc.target].dead && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height) && distance < 700f)
                    {
                        npc.netUpdate = true;
                        npc.velocity.X *= 0.5f;
                        npc.ai[2] = 3f;
                        npc.localAI[0] = (float)shotRate;
                    } // END start aiming
                } // END not confused
            }
            bool standingOnSolid = false;
            if (npc.velocity.Y == 0f) // no jump/fall
            {
                int yBelow = (int)(npc.position.Y + (float)npc.height + 7f) / 16;
                int xLeft = (int)npc.position.X / 16;
                int xRight = (int)(npc.position.X + (float)npc.width) / 16;
                for (int l = xLeft; l <= xRight; l++) // check every block under feet
                {
                    if (Main.tile[l, yBelow] == null) // null tile means ??
                        return;

                    if (Main.tile[l, yBelow].nactive() && Main.tileSolid[(int)Main.tile[l, yBelow].type]) // tile exists and is solid
                    {
                        standingOnSolid = true;
                        break; // one is enough so stop checking
                    }
                } // END traverse blocks under feet
            } // END no jump/fall
            if (npc.velocity.Y >= 0f)
            {
                int offset = 0;
                if (npc.velocity.X < 0f) offset = -1;
                if (npc.velocity.X > 0f) offset = 1;
                Vector2 pos = npc.position;
                pos.X += npc.velocity.X;
                int tileX = (int)((pos.X + (float)(npc.width / 2) + (float)((npc.width / 2 + 1) * offset)) / 16f);
                int tileY = (int)((pos.Y + (float)npc.height - 1f) / 16f);
                if (Main.tile[tileX, tileY] == null) Main.tile[tileX, tileY] = new Tile();
                if (Main.tile[tileX, tileY - 1] == null) Main.tile[tileX, tileY - 1] = new Tile();
                if (Main.tile[tileX, tileY - 2] == null) Main.tile[tileX, tileY - 2] = new Tile();
                if (Main.tile[tileX, tileY - 3] == null) Main.tile[tileX, tileY - 3] = new Tile();
                if (Main.tile[tileX, tileY + 1] == null) Main.tile[tileX, tileY + 1] = new Tile();
                if (Main.tile[tileX - offset, tileY - 3] == null) Main.tile[tileX - offset, tileY - 3] = new Tile();
                if ((float)(tileX * 16) < pos.X + (float)npc.width && (float)(tileX * 16 + 16) > pos.X && ((Main.tile[tileX, tileY].nactive() && !Main.tile[tileX, tileY].topSlope() && !Main.tile[tileX, tileY - 1].topSlope() && Main.tileSolid[(int)Main.tile[tileX, tileY].type] && !Main.tileSolidTop[(int)Main.tile[tileX, tileY].type]) || (Main.tile[tileX, tileY - 1].halfBrick() && Main.tile[tileX, tileY - 1].nactive())) && (!Main.tile[tileX, tileY - 1].nactive() || !Main.tileSolid[(int)Main.tile[tileX, tileY - 1].type] || Main.tileSolidTop[(int)Main.tile[tileX, tileY - 1].type] || (Main.tile[tileX, tileY - 1].halfBrick() && (!Main.tile[tileX, tileY - 4].nactive() || !Main.tileSolid[(int)Main.tile[tileX, tileY - 4].type] || Main.tileSolidTop[(int)Main.tile[tileX, tileY - 4].type]))) && (!Main.tile[tileX, tileY - 2].nactive() || !Main.tileSolid[(int)Main.tile[tileX, tileY - 2].type] || Main.tileSolidTop[(int)Main.tile[tileX, tileY - 2].type]) && (!Main.tile[tileX, tileY - 3].nactive() || !Main.tileSolid[(int)Main.tile[tileX, tileY - 3].type] || Main.tileSolidTop[(int)Main.tile[tileX, tileY - 3].type]) && (!Main.tile[tileX - offset, tileY - 3].nactive() || !Main.tileSolid[(int)Main.tile[tileX - offset, tileY - 3].type]))
                {
                    float tileWorldY = (float)(tileY * 16);
                    if (Main.tile[tileX, tileY].halfBrick())
                    { tileWorldY += 8f; }
                    if (Main.tile[tileX, tileY - 1].halfBrick())
                    { tileWorldY -= 8f; }
                    if (tileWorldY < pos.Y + (float)npc.height)
                    {
                        float tileWorldYHeight = pos.Y + (float)npc.height - tileWorldY;
                        float heightNeeded = 16.1f;
                        // if (wallCrawler)
                        // heightNeeded += 8f;
                        if (tileWorldYHeight <= heightNeeded)
                        {
                            npc.gfxOffY += npc.position.Y + (float)npc.height - tileWorldY;
                            npc.position.Y = tileWorldY - (float)npc.height;
                            npc.stepSpeed = (double)tileWorldYHeight >= 9.0 ? 2f : 1f;
                        }
                    }
                }
            }
            if (standingOnSolid)  //  if standing on solid tile
            {
                int x2 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f); // 15 pix in front of center of mass
                int y2 = (int)((npc.position.Y + (float)npc.height - 15f) / 16f); // 15 pix above feet
                if (leapReq > 1)
                { x2 = (int)((npc.position.X + (float)(npc.width / 2) + (float)((npc.width / 2 + 16) * npc.direction)) / 16f); } // 16 pix in front of edge
                if (Main.tile[x2, y2] == null)
                    Main.tile[x2, y2] = new Tile();
                if (Main.tile[x2, y2 - 1] == null)
                    Main.tile[x2, y2 - 1] = new Tile();
                if (Main.tile[x2, y2 - 2] == null)
                    Main.tile[x2, y2 - 2] = new Tile();
                if (Main.tile[x2, y2 - 3] == null)
                    Main.tile[x2, y2 - 3] = new Tile();
                if (Main.tile[x2, y2 + 1] == null)
                    Main.tile[x2, y2 + 1] = new Tile();
                if (Main.tile[x2 + npc.direction, y2 - 1] == null)
                    Main.tile[x2 + npc.direction, y2 - 1] = new Tile();
                if (Main.tile[x2 + npc.direction, y2 + 1] == null)
                    Main.tile[x2 + npc.direction, y2 + 1] = new Tile();
                Main.tile[x2, y2 + 1].halfBrick();
                if (Main.tile[x2, y2 - 1].nactive() && (Main.tile[x2, y2 - 1].type == 10 || Main.tile[x2, y2 - 1].type == 388) && knockPower != 0)
                {
                    npc.localAI[2] += 1f; // inc knock countdown
                    npc.ai[3] = 0f; // not bored if working on breaking a door
                    if (npc.localAI[2] >= 60f)  //  knock once per second
                    {
                        npc.velocity.X = 0.5f * (float)(-(float)npc.direction); //  slight recoil from hitting it
                        npc.localAI[1] += Math.Abs(knockPower);  //  increase door damage counter
                        npc.localAI[2] = 0f;  //  knock finished; start next knock
                        bool doorBuster = false;  //  door break flag
                        if (npc.localAI[1] >= 10f)  //  at 10 damage, set door as breaking (and cap at 10)
                        {
                            doorBuster = true;
                            npc.localAI[1] = 10f;
                        }
                        WorldGen.KillTile(x2, y2 - 1, true, false, false);  //  knock sound
                        if ((Main.netMode != 1 || !doorBuster) && doorBuster && Main.netMode != 1)  //  server and door breaking
                        {
                            if (knockPower < 0)  //  breaks doors rather than attempt to open
                            {
                                WorldGen.KillTile(x2, y2 - 1, false, false, false);  //  kill door
                                if (Main.netMode == 2) // server
                                    NetMessage.SendData(17, -1, -1, null, 0, (float)x2, (float)(y2 - 1), 0f, 0); // ?? tile breaking and/or item drop probably
                            }
                            else  //  try to open without breaking
                            {
                                if (Main.tile[x2, y2 - 1].type == 10)
                                {
                                    bool openDoor = WorldGen.OpenDoor(x2, y2 - 1, npc.direction);
                                    if (!openDoor)  //  door not opened successfully
                                    {
                                        npc.ai[3] = (float)boredom;  //  bored if door is stuck
                                        npc.netUpdate = true;
                                        // npc.velocity.X = 0; // cancel recoil so boredom wall reflection can trigger
                                    }
                                    if (Main.netMode == 2 && openDoor) // is server & door was just opened
                                        NetMessage.SendData(19, -1, -1, null, 0, (float)x2, (float)(y2 - 1), (float)npc.direction, 0, 0, 0);
                                }
                                if (Main.tile[x2, y2 - 1].type == 388)
                                {
                                    bool openDoor = WorldGen.ShiftTallGate(x2, y2 - 1, false);  //  open the door
                                    if (!openDoor)
                                    {
                                        npc.ai[3] = (float)boredom;
                                        npc.netUpdate = true;
                                    }
                                    if (Main.netMode == 2 && openDoor)
                                        NetMessage.SendData(19, -1, -1, null, 4, (float)x2, (float)(y2 - 1), 0f, 0, 0, 0);
                                }
                            }
                        }  //  END server and door breaking
                    } // END knock on door
                } // END trying to break door
                else // standing on solid tile but not in front of a passable door
                {
                    if ((npc.velocity.X < 0f && npc.spriteDirection == -1) || (npc.velocity.X > 0f && npc.spriteDirection == 1))
                    {  //  moving forward
                        if (npc.height >= 32 && Main.tile[x2, y2 - 2].nactive() && Main.tileSolid[(int)Main.tile[x2, y2 - 2].type])
                        { // 3 blocks above ground level(head height) blocked
                            if (Main.tile[x2, y2 - 3].nactive() && Main.tileSolid[(int)Main.tile[x2, y2 - 3].type])
                            { // 4 blocks above ground level(over head) blocked
                                npc.velocity.Y = -8f; // jump with power 8 (for 4 block steps)
                                npc.netUpdate = true;
                            }
                            else
                            {
                                npc.velocity.Y = -7f; // jump with power 7 (for 3 block steps)
                                npc.netUpdate = true;
                            }
                        } // for everything else, head height clear:
                        else if (Main.tile[x2, y2 - 1].nactive() && Main.tileSolid[(int)Main.tile[x2, y2 - 1].type])
                        { // 2 blocks above ground level(mid body height) blocked
                            npc.velocity.Y = -6f; // jump with power 6 (for 2 block steps)
                            npc.netUpdate = true;
                        }
                        else if (npc.position.Y + (float)npc.height - (float)(y2 * 16) > 20f && Main.tile[x2, y2].nactive() && !Main.tile[x2, y2].topSlope() && Main.tileSolid[(int)Main.tile[x2, y2].type])
                        { // 1 block above ground level(foot height) blocked
                            npc.velocity.Y = -5f; // jump with power 5 (for 1 block steps)
                            npc.netUpdate = true;
                        }
                        else if (leapReq > -1 && npc.directionY < 0 && (!Main.tile[x2, y2 + 1].nactive() || !Main.tileSolid[(int)Main.tile[x2, y2 + 1].type]) && (!Main.tile[x2 + npc.direction, y2 + 1].nactive() || !Main.tileSolid[(int)Main.tile[x2 + npc.direction, y2 + 1].type]))
                        { // rising? & jumps gaps & no solid tile ahead to step on for 2 spaces in front
                            npc.velocity.Y = -8f; // jump with power 8
                            npc.velocity.X *= 1.5f; // jump forward hard as well; we're trying to jump a gap
                            npc.netUpdate = true;
                        }
                        else if (knockPower != 0) // standing on solid tile but not in front of a passable door, moving forward, didnt jump.  I assume recoil from hitting door is too small to move passable door out of range and trigger this
                        {
                            npc.localAI[1] = 0f;  //  reset door dmg counter
                            npc.localAI[2] = 0f;  //  reset knock counter
                        }
                        if (npc.velocity.Y == 0f && !npc.justHit && npc.ai[3] == 1f)
                        { npc.velocity.Y = -5f; }
                    } // END moving forward, still: standing on solid tile but not in front of a passable door
                    if (leapReq > 1 && npc.velocity.Y == 0f && Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) < leapRangeX && Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y) < leapRangeY && (float)npc.direction * npc.velocity.X >= leapReq)
                    { // type that leaper & no jump/fall & near target & moving forward fast enough: hop code
                        npc.velocity.X *= 2f; // burst forward
                        if ((float)npc.direction * npc.velocity.X > leapSpeed) // but cap at leapSpeed
                        { npc.velocity.X = (float)npc.direction * leapSpeed; }
                        npc.velocity.Y = -leapHeight; // and jump of course
                        npc.netUpdate = true;
                    }
                }
            }
            else if (knockPower != 0)  //  not standing on a solid tile & can open/break doors
            {
                npc.localAI[1] = 0f;  //  reset door damage counter
                npc.localAI[2] = 0f;  //  reset knock counter
            }
        }
    }

    ///<summary> 
    ///Handles boss despawning and targeting.
    ///This exists to simplify AI code.
    ///Create an instance of this class in SetDefaults, call targetAndDespawn(npcID) at the start of their AI, and removing any existing targeting or despawning.
    ///</summary>
    public class NPCDespawnHandler
    {
        ///<summary> 
        ///Handles all targeting and despawning.
        ///</summary> 
        ///<param name="despawnFlavorText">The custom text this boss displays when it despawns</param>
        ///<param name="textColor">The color of the despawn text</param>
        ///<param name="dustType">The ID of the dust this NPC should create an explosion of upon despawning</param>
        public NPCDespawnHandler(string despawnFlavorText, Color textColor, int dustType)
        {
            despawnText = despawnFlavorText;
            despawnTextColor = textColor;
            despawnDustType = dustType;
        }

        ///<summary> 
        ///Handles all targeting and despawning.
        ///</summary> 
        ///<param name="dustType">The ID of the dust this NPC should create an explosion of upon despawning</param>
        public NPCDespawnHandler(int dustType)
        {
            despawnDustType = dustType;
        }

        readonly string despawnText;
        readonly Color despawnTextColor;
        readonly int despawnDustType;
        bool hasTargeted = false;
        int targetCount = 0;
        readonly int[] targetIDs = new int[256];
        readonly bool[] targetAlive = new bool[256];
        int despawnTime = -1;

        ///<summary> 
        ///Handles all targeting and despawning.
        ///</summary>         
        ///<param name="npcID">The ID of the NPC in question.</param>
        public bool TargetAndDespawn(int npcID)
        {

            //When despawning, we set timeLeft to 240. If that's been done, we don't need to check for players or target anyone anymore.
            if (despawnTime < 0)
            {
                //Only run this once. Gets all active players and throws them into these arrays so we can track their status.
                if (!hasTargeted)
                {
                    foreach (Player player in Main.player)
                    {
                        //For some reason, Main.player always has 255 entries. This ensures we're only pulling real players from it.
                        if (player.active)
                        {
                            targetIDs[targetCount] = player.whoAmI;
                            targetAlive[targetCount] = true;
                            targetCount++;
                        }
                    }
                    hasTargeted = true;
                }
                else
                {
                    //Go through the target list. If everyone has died once, despawn. Else, target the closest one that has not yet died.
                    //It's important that it only targets players who haven't died, because otherwise one living player could hide far away while the other repeatedly respawned and fought the boss.
                    //With this, it will intentionally seek out those it has not yet killed instead.
                    bool viableTarget = false;
                    float closestPlayerDistance = 999999;
                    //Iterate through all tracked players in the array
                    for (int i = 0; i < targetCount; i++)
                    {
                        //For each of them, check if they're dead. If so, mark it down in targetAlive.
                        if (Main.player[targetIDs[i]].dead && targetAlive[i])
                        {
                            targetAlive[i] = false;
                        }
                        else if (targetAlive[i] && Main.player[targetIDs[i]].active)
                        {
                            //If it found a player that hasn't been killed yet, then don't despawn
                            viableTarget = true;
                            //Check if they're the closest one, and if so target them
                            float distance = Vector2.DistanceSquared(Main.player[targetIDs[i]].position, Main.npc[npcID].position);
                            if (distance < closestPlayerDistance)
                            {
                                closestPlayerDistance = distance;
                                Main.npc[npcID].target = targetIDs[i];

                            }
                        }
                    }
                    //If there's no player that has not yet died, then despawn.
                    if (!viableTarget)
                    {
                        if (despawnText != null)
                        {
                            if (Main.netMode == NetmodeID.SinglePlayer)
                            {
                                Main.NewText(despawnText, despawnTextColor);
                            }
                            if (Main.netMode == NetmodeID.Server)
                            {
                                UsefulFunctions.ServerText(despawnText, despawnTextColor);
                            }
                        }
                        despawnTime = 240;
                    }
                }
            }
            else
            {
                //Adios
                if (despawnTime == 0)
                {
                    for (int i = 0; i < 60; i++)
                    {
                        int dustID = Dust.NewDust(Main.npc[npcID].position, Main.npc[npcID].width, Main.npc[npcID].height, despawnDustType, Main.rand.Next(-12, 12), Main.rand.Next(-12, 12), 150, default, 7f);
                        Main.dust[dustID].noGravity = true;
                    }
                    Main.npc[npcID].active = false;
                }
                else
                {
                    int dustID = Dust.NewDust(Main.npc[npcID].position, Main.npc[npcID].width, Main.npc[npcID].height, despawnDustType, Main.rand.Next(-12, 12), Main.rand.Next(-12, 12), 150, default, 1f);
                    Main.dust[dustID].noGravity = true;
                    despawnTime--;
                }

                //The frame before despawning, we return true to let the NPC's AI know it's about to get despawned. This allows it to do anything it needs to with that information (like re-actuating the pyramid)
                if (despawnTime == 1)
                {
                    return true;
                }
            }
            return false;
        }

        //Originally used modified fighter/archer AI by GrtAndPwrflTrtl as a base (http://www.terrariaonline.com/members/grtandpwrfltrtl.86018/)
        public void FighterAI(NPC npc, float top_speed = 1f, float acceleration = .07f, float braking_power = .2f, bool can_pass_doors = false, bool can_teleport = false, bool hates_light = false, int sound_type = 0, int sound_frequency = 1000, float enrage_percentage = 0, bool hops = false)
        {
            float door_break_pow = 10;
            


            BaseAI(npc);
        }

        public void ArcherAI(NPC npc, int projectile_type, int projectile_damage, int projectile_velocity, int shot_rate, float top_speed = 1f, float acceleration = .07f, float braking_power = .2f,  bool can_pass_doors = false, bool can_teleport = false, bool hates_light = false, int sound_type = 0, int sound_frequency = 1000, float enrage_percentage = 0)
        {
            BaseAI(npc, top_speed, acceleration, braking_power, can_pass_doors, can_teleport, hates_light, sound_type, sound_frequency);

            //First half is firing time, second half is cooldown. So we just double it to simplify.
            shot_rate *= 2;

            if (npc.confused)
            {
                npc.ai[2] = 0f; // won't try to stop & aim if confused
            }
            else
            {
                if (npc.ai[1] > 0f)
                    npc.ai[1] -= 1f; // decrement fire & reload counter

                if (npc.justHit || npc.velocity.Y != 0f || npc.ai[1] <= 0f) // was just hit?
                {
                    npc.ai[1] = shot_rate; //Reset firing time
                    npc.ai[2] = 0f; //Not aiming
                }

                //Target the closest player and check if they're in range
                npc.TargetClosest(true);
                if (Vector2.Distance(npc.Center, Main.player[npc.target].Center) < 700f)
                {
                    //If it's not aiming yet, then slow down, aim, and start its cooldown
                    if (npc.ai[2] == 0)
                    {
                        //Aim at them, and start the shot cooldown
                        npc.velocity.X = npc.velocity.X * 0.5f;
                        npc.ai[2] = 3f;
                        npc.ai[1] = shot_rate;
                    }

                    npc.velocity.X = npc.velocity.X * 0.9f; // decelerate to stop & shoot
                    npc.spriteDirection = npc.direction; // match animation to facing

                    Vector2 projVelocity = UsefulFunctions.GenerateTargetingVector(npc.Center, Main.player[npc.target].Center, projectile_velocity);
                    projVelocity.Y -= projVelocity.X * 0.1f; //Overshoot to compensate for gravity
                    //Fire at halfway through: first half of delay is aim, 2nd half is cooldown
                    if (npc.ai[1] == (shot_rate / 2))
                    {
                        projVelocity += Main.rand.NextVector2Circular(4, 4); //Add randomness
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, projVelocity.X, projVelocity.Y, projectile_type, projectile_damage, 0f, Main.myPlayer);
                        }
                    }

                    if (Math.Abs(projVelocity.Y) > Math.Abs(projVelocity.X) * 2f) // target steeply above/below NPC
                    {
                        if (projVelocity.Y > 0f)
                            npc.ai[2] = 1f; // aim downward
                        else
                            npc.ai[2] = 5f; // aim upward
                    }
                    else if (Math.Abs(projVelocity.X) > Math.Abs(projVelocity.Y) * 2f) // target on level with NPC
                        npc.ai[2] = 3f;  //  aim straight ahead
                    else if (projVelocity.Y > 0f) // target is below NPC
                        npc.ai[2] = 2f;  //  aim slight downward
                    else // target is not below NPC
                        npc.ai[2] = 4f;  //  aim slight upward                    
                }
                //If not, don't aim at them
                else
                {
                    npc.ai[2] = 0;
                }
            }
        }

        public void BaseAI(NPC npc, float topSpeed = 1f, float acceleration = .07f, float braking_power = .2f, bool can_pass_doors = false, bool can_teleport = false, bool hates_light = false, int sound_type = 0, int sound_frequency = 1000, float enrage_percentage = 0, bool lavaJumping = false)
        {
            bool bored = false;
            int boredom_time = 60;
            int boredomCooldown = boredom_time * 10;

            float hop_velocity = 1f; // forward velocity needed to initiate hopping; usually 1
            float hop_range_x = 100; // less than this is 'close to target'; usually 100
            float hop_range_y = hop_range_x / 2; // less than this is 'close to target'; usually 50
            float hop_power = 4; // how hard/high offensive hops are; usually 4
            float hop_speed = 3; // how fast hops can accelerate vertically; usually 3 (2xSpd is 4 for Hvy Skel & Werewolf so they're noticably capped)
           

            if (npc.life < (float)npc.lifeMax * enrage_percentage)
            {
                acceleration *= 2;
                topSpeed *= 2;
            }

            BasicMovement(npc, topSpeed, acceleration);

            if (can_teleport)
            {
                TeleportEffects(npc);
            }
            //if (forceJumpGaps)
            {
                JumpGaps(npc);
            }
            if (lavaJumping)
            {
                LavaJumping(npc);
            }
        }

        private void BasicMovement(NPC npc, float topSpeed, float acceleration)
        {
            //Jump if stuck
            if (npc.velocity.Y == 0f && (npc.velocity.X == 0f && npc.direction < 0))
            {
                npc.velocity.Y -= 8f;
                npc.velocity.X -= 2f;
            }
            else if (npc.velocity.Y == 0f && (npc.velocity.X == 0f && npc.direction > 0))
            {
                npc.velocity.Y -= 8f;
                npc.velocity.X += 2f;
            }

            //If more than max speed then slow down
            if (npc.velocity.X < -topSpeed || npc.velocity.X > topSpeed)
            {
                if (npc.velocity.Y == 0f)
                {
                    npc.velocity *= 0.8f;
                }
            }
            //Otherwise, accelerate
            else
            {
                if (npc.velocity.X < topSpeed && npc.direction == 1)
                {
                    npc.velocity.X = npc.velocity.X + acceleration;
                    if (npc.velocity.X > topSpeed)
                    {
                        npc.velocity.X = topSpeed;
                    }
                }
                else
                {
                    if (npc.velocity.X > -topSpeed && npc.direction == -1)
                    {
                        npc.velocity.X = npc.velocity.X - acceleration;
                        if (npc.velocity.X < -topSpeed)
                        {
                            npc.velocity.X = -topSpeed;
                        }
                    }
                }
            }
        }

        private void TeleportEffects(NPC npc)
        {
            if (npc.ai[3] == -120f)  //  boredom goes negative? I think this makes disappear/arrival effects after it just teleported
            {
                npc.velocity *= 0f; // stop moving
                npc.ai[3] = 0f; // reset boredom to 0
                Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);
                Vector2 vector = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f); // current location
                float num6 = npc.oldPos[2].X + (float)npc.width * 0.5f - vector.X; // direction to where it was 3 frames ago?
                float num7 = npc.oldPos[2].Y + (float)npc.height * 0.5f - vector.Y; // direction to where it was 3 frames ago?
                float num8 = (float)Math.Sqrt((double)(num6 * num6 + num7 * num7)); // distance to where it was 3 frames ago?
                num8 = 2f / num8; // to normalize to 2 unit long vector
                num6 *= num8; // direction to where it was 3 frames ago, vector normalized
                num7 *= num8; // direction to where it was 3 frames ago, vector normalized
                for (int j = 0; j < 20; j++) // make 20 dusts at current position
                {
                    int num9 = Dust.NewDust(npc.position, npc.width, npc.height, 71, num6, num7, 200, default(Color), 2f);
                    Main.dust[num9].noGravity = true; // floating
                    Dust expr_19EE_cp_0 = Main.dust[num9]; // make a dust handle?
                    expr_19EE_cp_0.velocity.X = expr_19EE_cp_0.velocity.X * 2f; // faster in x direction
                }
                for (int k = 0; k < 20; k++) // more dust effects at old position
                {
                    int num10 = Dust.NewDust(npc.oldPos[2], npc.width, npc.height, 71, -num6, -num7, 200, default(Color), 2f);
                    Main.dust[num10].noGravity = true;
                    Dust expr_1A6F_cp_0 = Main.dust[num10];
                    expr_1A6F_cp_0.velocity.X = expr_1A6F_cp_0.velocity.X * 2f;
                }
            }
        }

        private void JumpGaps(NPC npc)
        {
            if (npc.velocity.Y == 0f && (npc.velocity.X == 0f && npc.direction < 0))
            {
                npc.velocity.Y -= 8f;
                npc.velocity.X -= 2f;
            }
            else if (npc.velocity.Y == 0f && (npc.velocity.X == 0f && npc.direction > 0))
            {
                npc.velocity.Y -= 8f;
                npc.velocity.X += 2f;
            }            
        }

        private void LavaJumping(NPC npc)
        {
            if (npc.lavaWet)
            {
                npc.velocity.Y -= 2;
            }
        }
    }
}
