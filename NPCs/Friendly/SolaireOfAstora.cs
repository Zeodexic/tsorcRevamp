﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using tsorcRevamp.Items.Armors;
using tsorcRevamp.Items.Weapons.Melee;

namespace tsorcRevamp.NPCs.Friendly
{
    [AutoloadHead]
    class SolaireOfAstora : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warrior of Sunlight");
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 40;
            NPCID.Sets.AttackType[NPC.type] = 3;
            NPCID.Sets.AttackTime[NPC.type] = 18;
            NPCID.Sets.AttackAverageChance[NPC.type] = 10;
            NPCID.Sets.HatOffsetY[NPC.type] = 4;
        }
        public override List<string> SetNPCNameList() {
            return new List<string> { "Solaire" };
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 90;
            NPC.defense = 15;
            NPC.lifeMax = 1000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.DyeTrader;
        }
        public override void AI()
        {
            if ((NPC.velocity.X == 0 && NPC.velocity.Y == 0) && Main.dayTime)
            {
                Lighting.AddLight(NPC.Center, .850f, .850f, .450f);
                if (Main.rand.Next(10) == 0)
                {
                    int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 57, NPC.velocity.X * 0f, -1f, 30, default(Color), 1.5f);
                    Main.dust[dust].noGravity = true;
                }
            }
        }

        public override string GetChat()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();
            chat.Add("Dark Souls possess great powers, but also great responsibilities.");
            chat.Add("Praise the sun!");
            chat.Add("Oh, hello there. I will stay behind, to gaze at the sun. The sun is a wondrous body. Like a magnificent father! If only I could be so grossly incandescent!");
            chat.Add("Of course, we are not the only ones engaged in this.");
            chat.Add("Would you like to buy some of my wares?");
            chat.Add("The way I see it, our fates appear to be intertwined. In a land brimming with Hollows, could that really be mere chance? So, what do you say? Why not help one another on this lonely journey?");
            if (tsorcRevampWorld.TheEnd)
            {
                chat.Add("You have done well, indeed you have. You've a strong arm, strong faith, and most importantly, a strong heart.", 1.5);
            }
            return chat;
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
                return;
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<OldSabre>());
            shop.item[nextSlot].shopCustomPrice = 80;
            shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<OldRapier>());
            shop.item[nextSlot].shopCustomPrice = 80;
            shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<OldAxe>());
            shop.item[nextSlot].shopCustomPrice = 180;
            shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<OldLongsword>());
            shop.item[nextSlot].shopCustomPrice = 140;
            shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<OldMace>());
            shop.item[nextSlot].shopCustomPrice = 80;
            shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<OldBroadsword>());
            shop.item[nextSlot].shopCustomPrice = 140;
            shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<OldMorningStar>());
            shop.item[nextSlot].shopCustomPrice = 200;
            shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<OldChainCoif>());
            shop.item[nextSlot].shopCustomPrice = 60;
            shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<OldChainArmor>());
            shop.item[nextSlot].shopCustomPrice = 120;
            shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<OldChainGreaves>());
            shop.item[nextSlot].shopCustomPrice = 60;
            shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<AncientBrassHelmet>());
            shop.item[nextSlot].shopCustomPrice = 120;
            shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<AncientBrassArmor>());
            shop.item[nextSlot].shopCustomPrice = 180;
            shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<AncientBrassGreaves>());
            shop.item[nextSlot].shopCustomPrice = 120;
            shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<CorruptedTooth>());
            shop.item[nextSlot].shopCustomPrice = 420;
            shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<ForgottenMetalKnuckles>());
            shop.item[nextSlot].shopCustomPrice = 100;
            shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.ItemCrates.ThrowingAxeCrate>());
            shop.item[nextSlot].shopCustomPrice = 8;
            shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
            nextSlot++;
            if (NPC.downedBoss1)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<OldTwoHandedSword>());
                shop.item[nextSlot].shopCustomPrice = 300;
                shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
                nextSlot++;
            }
            if (NPC.downedBoss2)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<ForgottenLongSword>());
                shop.item[nextSlot].shopCustomPrice = 1200;
                shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<ForgottenKaiserKnuckles>());
                shop.item[nextSlot].shopCustomPrice = 600;
                shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
                nextSlot++;
            }
            if (NPC.downedBoss3)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<ForgottenKotetsu>());
                shop.item[nextSlot].shopCustomPrice = 1500;
                shop.item[nextSlot].shopSpecialCurrency = tsorcRevamp.DarkSoulCustomCurrencyId;
                nextSlot++;
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (base.NPC.life <= 0)
            {
                if (!Main.dedServ)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Tibian Female Knight Gore 1").Type);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Tibian Female Knight Gore 2").Type);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Tibian Female Knight Gore 2").Type);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Tibian Female Knight Gore 3").Type);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), Mod.Find<ModGore>("Tibian Female Knight Gore 3").Type);
                }
            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 30;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void DrawTownAttackSwing(ref Texture2D item, ref int itemSize, ref float scale, ref Vector2 offset)
        {
            item = (Texture2D)TextureAssets.Item[ModContent.ItemType<OldBroadsword>()];
            scale = .8f;
            itemSize = 36;
        }

        public override void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight)
        {
            itemWidth = 36;
            itemHeight = 36;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            foreach (Player p in Main.player)
            {
                if (!p.active)
                {
                    continue;
                }
                if (p.statDefense > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool CanGoToStatue(bool toKingStatue)
        {
            return true;
        }
    }
}
