﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Items.Pets;
using tsorcRevamp.NPCs.Bosses;
using tsorcRevamp.NPCs.Bosses.SuperHardMode;

namespace tsorcRevamp.Items.BossBags
{

    public abstract class BossBag : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");

        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = (Texture2D)Terraria.GameContent.TextureAssets.Item[Item.type];
            for (int i = 0; i < 4; i++)
            {
                Vector2 offsetPositon = Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * i) * 3;
                spriteBatch.Draw(texture, position + offsetPositon, null, Main.DiscoColor, 0, origin, scale, SpriteEffects.None, 0);
            }

            return true;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            //UsefulFunctions.DustRing(Item.Center, 32, DustID.ShadowbeamStaff);
            Texture2D texture = (Texture2D)Terraria.GameContent.TextureAssets.Item[Item.type];

            Lighting.AddLight(Item.Center, Main.DiscoColor.ToVector3());
            for (int i = 0; i < 4; i++)
            {
                Vector2 offsetPositon = Vector2.UnitY.RotatedBy(((Main.GameUpdateCount % 300) / 30f) + MathHelper.PiOver2 * i) * 5;
                spriteBatch.Draw(texture,offsetPositon + new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f),
                new Rectangle(0, 0, texture.Width, texture.Height), Main.DiscoColor, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0);
            }

            return true;
        }

    }

    #region PreHardMode

    public class OolacileDemonBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<AncientOolacileDemon>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player, false, true); //gives the player souls if they haven't opened the bag before            
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.BandOfGreatCosmicPower>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ItemID.CloudinaBottle, 1);

        }
    }
    public class SlograBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.Slogra>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player, false, true); //gives the player souls if they haven't opened the bag before
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.PoisonbiteRing>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.BloodbiteRing>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Ranged.DarkTrident>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.BurningStone>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<DarkSoul>(), (int)((700 + Main.rand.Next(300)) * tsorcRevampPlayer.CheckSoulsMultiplier(player)));
        }
    }
    public class GaibonBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.Gaibon>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player, false, true); //gives the player souls if they haven't opened the bag before
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.PoisonbiteRing>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.BloodbiteRing>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Ranged.DarkTrident>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.BurningAura>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<DarkSoul>(), (int)((700 + Main.rand.Next(300)) * tsorcRevampPlayer.CheckSoulsMultiplier(player)));
        }
    }
    public class JungleWyvernBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.JungleWyvern.JungleWyvernHead>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player, false, true); //gives the player souls if they haven't opened the bag before
            player.QuickSpawnItem(player.GetSource_Loot(), ItemID.NecroHelmet);
            player.QuickSpawnItem(player.GetSource_Loot(), ItemID.NecroBreastplate);
            player.QuickSpawnItem(player.GetSource_Loot(), ItemID.NecroGreaves);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Accessories.ChloranthyRing>());
            player.QuickSpawnItem(player.GetSource_Loot(), ItemID.Sapphire, Main.rand.Next(2, 10));
            player.QuickSpawnItem(player.GetSource_Loot(), ItemID.Ruby, Main.rand.Next(2, 10));
            player.QuickSpawnItem(player.GetSource_Loot(), ItemID.Topaz, Main.rand.Next(2, 10));
            player.QuickSpawnItem(player.GetSource_Loot(), ItemID.Diamond, Main.rand.Next(2, 10));
            player.QuickSpawnItem(player.GetSource_Loot(), ItemID.Emerald, Main.rand.Next(2, 10));
            player.QuickSpawnItem(player.GetSource_Loot(), ItemID.Amethyst, Main.rand.Next(2, 10));
        }
    }
    #endregion

    #region Hardmode
    public class TheHunterBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<TheHunter>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player); //gives the player souls if they haven't opened the bag before
            player.QuickSpawnItem(player.GetSource_Loot(), ItemID.WaterWalkingBoots, 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<CrestOfEarth>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ItemID.Drax);
        }
    }
    public class TheRageBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<TheRage>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player); //gives the player souls if they haven't opened the bag before
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<CrestOfFire>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ItemID.CobaltDrill);
        }
    }
    public class TheSorrowBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<TheSorrow>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player); //gives the player souls if they haven't opened the bag before
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<CrestOfWater>());
            player.QuickSpawnItem(player.GetSource_Loot(), ItemID.AdamantiteDrill);
        }
    }

    public class HeroofLumeliaBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<HeroofLumelia>();
        public override void OpenBossBag(Player player)
        {          
            tsorcRevampPlayer modPlayer = player.GetModPlayer<tsorcRevampPlayer>();
            
            //if not killed before
            if (!modPlayer.bagsOpened.Contains(BossBagNPC))
            {
                player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<DarkSoul>(), 10000); //Then drop the souls
                player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.CovetousSilverSerpentRing>());
                player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Ammo.ArrowOfBard>(), Main.rand.Next(10, 20));
            }
            //if the boss has been killed once
            else
            {
                player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<DarkSoul>(), 3500); //Then drop the souls
                player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Ammo.ArrowOfBard>(), Main.rand.Next(10, 15));
            }

            //This has to go last, because it marks the bag as opened
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player, false, true); //gives the player souls if they haven't opened the bag before
        }
    }

    public class AttraidiesBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.Okiku.FinalForm.Attraidies>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player); //gives the player souls if they haven't opened the bag before            
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Magic.BloomShards>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.TheEnd>());
            player.QuickSpawnItem(player.GetSource_Loot(), ItemID.Picksaw);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.SoulOfAttraidies>(), Main.rand.Next(15, 23));
        }
    }
    public class KrakenBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.Fiends.WaterFiendKraken>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player, true, true); //gives the player souls if they haven't opened the bag before
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.GoldenHairpin>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.DragonHorn>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Melee.BarrowBlade>());
        }
    }
    public class MarilithBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.Fiends.FireFiendMarilith>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player, true, true); //gives the player souls if they haven't opened the bag before
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Melee.ForgottenRisingSun>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Magic.Ice3Tome>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.FairyInABottle>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Potions.HolyWarElixir>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Melee.BarrowBlade>()); //Because the Shaman Elder says she drops them

            if (ModContent.GetInstance<tsorcRevampConfig>().AdventureModeItems)
            {
                player.QuickSpawnItem(player.GetSource_Loot(), ItemID.LargeSapphire);
            }
        }
    }
    public class LichBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.Fiends.EarthFiendLich>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player, true, true); //gives the player souls if they haven't opened the bag before
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Magic.Bolt3Tome>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.DragoonBoots>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Melee.ForgottenGaiaSword>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Potions.HolyWarElixir>(), 1);
        }
    }

    public class SerrisBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.Serris.SerrisX>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player, true, true); //gives the player souls if they haven't opened the bag before     
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Potions.DemonDrugPotion>(), 3 + Main.rand.Next(4));
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Potions.ArmorDrugPotion>(), 3 + Main.rand.Next(4));
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Magic.BarrierTome>(), 1);
        }
    }
    public class DeathBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.Death>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player, true); //gives the player souls if they haven't opened the bag before            
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Potions.HolyWarElixir>(), 4);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Magic.WallTome>(), 4);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Magic.BarrierTome>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ItemID.LivingRainbowDye, 5);
            player.QuickSpawnItem(player.GetSource_Loot(), ItemID.MidnightRainbowDye, 5);
        }
    }
    public class WyvernMageBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.WyvernMage.WyvernMage>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player, false, true); //gives the player souls if they haven't opened the bag before          
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Potions.HolyWarElixir>(), 2);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Melee.LionheartGunblade>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Magic.LampTome>(), 1);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.GemBox>(), 1);
        }
    }
    #endregion

    #region SuperHardMode
    public class GwynBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<Gwyn>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player); //gives the player souls if they haven't opened the bag before
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.DraxEX>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Epilogue>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.EssenceOfTerraria>());
        }
    }
    public class BlightBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<Blight>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player, true); //gives the player souls if they haven't opened the bag before           
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Magic.DivineSpark>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.SoulOfBlight>(), Main.rand.Next(3, 6));
        }
    }
    public class ChaosBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<Chaos>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player, true); //gives the player souls if they haven't opened the bag before              
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Armors.PowerArmorNUHelmet>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Armors.PowerArmorNUTorso>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Armors.PowerArmorNUGreaves>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Magic.FlareTome>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Ranged.ElfinBow>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Potions.HolyWarElixir>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.DarkSoul>(), 3000);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.SoulOfChaos>(), 3);
        }
    }
    public class MageShadowBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.SuperHardMode.GhostWyvernMage.WyvernMageShadow>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player); //gives the player souls if they haven't opened the bag before             
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Potions.HolyWarElixir>(), 4);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.GhostWyvernSoul>(), 8);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.RingOfPower>());
        }
    }

    public class OolacileSorcererBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<AbysmalOolacileSorcerer>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player, true); //gives the player souls if they haven't opened the bag before           
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Potions.HealingElixir>(), 10);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.DarkSoul>(), 5000);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.DuskCrownRing>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Humanity>());
            if (Main.rand.Next(1) == 0) player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.PurgingStone>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.RedTitanite>(), 5);
        }
    }
    public class ArtoriasBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<Artorias>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player, true); //gives the player souls if they haven't opened the bag before
            
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.DarkSoul>(), 5000);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.WolfRing>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.TheRingOfArtorias>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.SoulOfArtorias>(), 6);
            if (ModContent.GetInstance<tsorcRevampConfig>().AdventureModeItems)
            {
                player.QuickSpawnItem(player.GetSource_Loot(), ItemID.LargeAmethyst);
            }
        }
    }
    public class DarkCloudBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<DarkCloud>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player); //gives the player souls if they haven't opened the bag before
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Humanity>(), 3);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.ReflectionShift>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Melee.MoonlightGreatsword>());
        }
    }
    public class HellkiteBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.SuperHardMode.HellkiteDragon.HellkiteDragonHead>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player, true); //gives the player souls if they haven't opened the bag before            
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.DragonEssence>(), 22 + Main.rand.Next(6));
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.DarkSoul>(), 4000);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Melee.HiRyuuSpear>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.DragonStone>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.BossItems.HellkiteStone>(), 1);

        }
    }
    public class SeathBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.SuperHardMode.Seath.SeathTheScalelessHead>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player); //gives the player souls if they haven't opened the bag before
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.DragonEssence>(), 35 + Main.rand.Next(5));
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.DarkSoul>(), 7000);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.BequeathedSoul>(), 3);
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.BlueTearstoneRing>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.PurgingStone>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.DragonWings>());
        }
    }
    public class WitchkingBag : BossBag
    {
        public override int BossBagNPC => ModContent.NPCType<NPCs.Bosses.SuperHardMode.Witchking>();
        public override void OpenBossBag(Player player)
        {
            VanillaBossBag.AddBossBagSouls(BossBagNPC, player, true); //gives the player souls if they haven't opened the bag before
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.RingOfPower>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Armors.WitchkingHelmet>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Armors.WitchkingTop>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Armors.WitchkingBottoms>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Accessories.CovenantOfArtorias>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.Weapons.Melee.WitchkingsSword>());
            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<DarkSoul>(), 5000);
            if (!ModContent.GetInstance<tsorcRevampConfig>().AdventureModeItems)
            {
                player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<BrokenStrangeMagicRing>());
            }
        }
    }
    #endregion

    public class VanillaBossBag : GlobalItem
    {
        public static void AddBossBagSouls(int EnemyID, Player player, bool guardianSoul = false, bool staminaVessel = false)
        {
            tsorcRevampPlayer modPlayer = player.GetModPlayer<tsorcRevampPlayer>();
            if (!modPlayer.bagsOpened.Contains(EnemyID))
            {
                modPlayer.bagsOpened.Add(EnemyID);
            }
            else
            {
                return;
            }

            NPC npc = new NPC();
            npc.SetDefaults(EnemyID);
            float enemyValue = (int)npc.value / 25;
            float multiplier = tsorcRevampPlayer.CheckSoulsMultiplier(player);
            tsorcRevampWorld.Slain[EnemyID] = 1; //set the value to 1

            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData); //Slain only exists on the server. This tells the server to run NetSend(), which syncs this data with clients
            }

            int DarkSoulQuantity = (int)(multiplier * enemyValue);

            if (guardianSoul)
            {
                player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<GuardianSoul>());
            }
            if (staminaVessel)
            {
                player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<StaminaVessel>());
            }

            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<DarkSoul>(), DarkSoulQuantity);
        }

        public static void SoulsOnFirstBag(int EnemyID, Player player)
        {
            var Slain = tsorcRevampWorld.Slain;
            if (Slain.ContainsKey(EnemyID))
            {
                if (Slain[EnemyID] == 0)
                {
                    AddBossBagSouls(EnemyID, player);
                    Slain[EnemyID] = 1;
                }
            }
        }

        public static void StaminaVesselOnFirstBag(int EnemyID, Player player)
        {
            var Slain = tsorcRevampWorld.Slain;
            if (Slain.ContainsKey(EnemyID))
            {
                if (Slain[EnemyID] == 0)
                {
                    player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<StaminaVessel>());
                    //Don't set slain to 1, let SoulsOnFirstBag do that as they all run it
                }
            }
        }

        public static void EstusFlaskShardOnFirstBag(int EnemyID, Player player)
        {
            var Slain = tsorcRevampWorld.Slain;
            if (Slain.ContainsKey(EnemyID))
            {
                if (Slain[EnemyID] == 0 && player.GetModPlayer<tsorcRevampPlayer>().BearerOfTheCurse)
                {
                    player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<EstusFlaskShard>());
                    //Don't set slain to 1, let SoulsOnFirstBag do that as they all run it
                }
            }
        }

        public static void SublimeBoneDustOnFirstBag(int EnemyID, Player player)
        {
            var Slain = tsorcRevampWorld.Slain;
            if (Slain.ContainsKey(EnemyID))
            {
                if (Slain[EnemyID] == 0 && player.GetModPlayer<tsorcRevampPlayer>().BearerOfTheCurse)
                {
                    player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<SublimeBoneDust>());
                    //Don't set slain to 1, let SoulsOnFirstBag do that as they all run it
                }
            }
        }
        public override bool PreOpenVanillaBag(string context, Player player, int arg)
        {

            if (context == "bossBag" && arg == ItemID.KingSlimeBossBag)
            { //re-implement king slime bag to stop blacklisted items from dropping in adventure mode
                player.QuickSpawnItem(player.GetSource_Loot(), ItemID.RoyalGel);
                player.QuickSpawnItem(player.GetSource_Loot(), ItemID.Solidifier);
                player.QuickSpawnItem(player.GetSource_Loot(), ItemID.GoldCoin, 11);
                player.QuickSpawnItem(player.GetSource_Loot(), ItemID.Katana);
                if (Main.rand.Next(99) < 66) { player.QuickSpawnItem(player.GetSource_Loot(), ItemID.NinjaHood); }
                if (Main.rand.Next(99) < 66) { player.QuickSpawnItem(player.GetSource_Loot(), ItemID.NinjaShirt); }
                if (Main.rand.Next(99) < 66) { player.QuickSpawnItem(player.GetSource_Loot(), ItemID.NinjaPants); }
                if (Main.rand.Next(7) == 0) { player.QuickSpawnItem(player.GetSource_Loot(), ItemID.KingSlimeMask); }
                if (Main.rand.Next(10) == 0) { player.QuickSpawnItem(player.GetSource_Loot(), ItemID.KingSlimeTrophy); }
                if (Main.rand.Next(2) == 0) { player.QuickSpawnItem(player.GetSource_Loot(), ItemID.SlimeGun); }
                player.QuickSpawnItem(player.GetSource_Loot(), ItemID.SlimySaddle);
                if (Main.rand.Next(2) == 0) { player.QuickSpawnItem(player.GetSource_Loot(), ItemID.SlimeHook); }

                StaminaVesselOnFirstBag(NPCID.KingSlime, player);
                SoulsOnFirstBag(NPCID.KingSlime, player);
                return false;
            }
            if (context == "bossBag" && arg == ItemID.GolemBossBag)
            {
                //Picksaw drops from Attraidies who is Post-Golem now, and gates SuperHardMode content. We've gotta stop Golem from dropping it.
                if (!ModContent.GetInstance<tsorcRevampConfig>().AdventureModeItems)
                {
                    if (Main.rand.Next(3) == 0) { player.QuickSpawnItem(player.GetSource_Loot(), ItemID.Picksaw); }
                }
                else
                {
                    player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<Items.BrokenPicksaw>());
                }

                //Drops that work in the traditional way. Also, adds the Crest of Stone to its drops.
                player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<CrestOfStone>());
                player.QuickSpawnItem(player.GetSource_Loot(), ItemID.ShinyStone);
                if (Main.rand.Next(6) == 0) { player.QuickSpawnItem(player.GetSource_Loot(), ItemID.GolemMask); }
                if (Main.rand.Next(9) == 0) { player.QuickSpawnItem(player.GetSource_Loot(), ItemID.GolemTrophy); }
                player.QuickSpawnItem(player.GetSource_Loot(), ItemID.GreaterHealingPotion, 5 + Main.rand.Next(10));

                //Always drops one of these things, picked at random
                int drop = Main.rand.Next(6);
                switch (drop)
                {
                    case 0:
                        player.QuickSpawnItem(player.GetSource_Loot(), ItemID.Stynger);
                        player.QuickSpawnItem(player.GetSource_Loot(), ItemID.StyngerBolt, 60 + Main.rand.Next(39));
                        break;
                    case 1:
                        player.QuickSpawnItem(player.GetSource_Loot(), ItemID.PossessedHatchet);
                        break;
                    case 2:
                        player.QuickSpawnItem(player.GetSource_Loot(), ItemID.SunStone);
                        break;
                    case 3:
                        player.QuickSpawnItem(player.GetSource_Loot(), ItemID.EyeoftheGolem);
                        break;
                    case 4:
                        player.QuickSpawnItem(player.GetSource_Loot(), ItemID.HeatRay);
                        break;
                    case 5:
                        player.QuickSpawnItem(player.GetSource_Loot(), ItemID.StaffofEarth);
                        break;
                    case 6:
                        player.QuickSpawnItem(player.GetSource_Loot(), ItemID.GolemFist);
                        break;
                }

                SoulsOnFirstBag(NPCID.Golem, player);
                return false;
            }



            return base.PreOpenVanillaBag(context, player, arg);
        }
        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            var Slain = tsorcRevampWorld.Slain;
            if (context == "bossBag")
            {
                if (arg == ItemID.EyeOfCthulhuBossBag && !player.ZoneJungle)
                {
                    player.QuickSpawnItem(player.GetSource_Loot(), ItemID.HermesBoots);
                    player.QuickSpawnItem(player.GetSource_Loot(), ItemID.HerosHat);
                    player.QuickSpawnItem(player.GetSource_Loot(), ItemID.HerosPants);
                    player.QuickSpawnItem(player.GetSource_Loot(), ItemID.HerosShirt);
                    SublimeBoneDustOnFirstBag(NPCID.EyeofCthulhu, player);
                    SoulsOnFirstBag(NPCID.EyeofCthulhu, player);
                }
                if (arg == ItemID.EaterOfWorldsBossBag)
                {
                    SoulsOnFirstBag(NPCID.EaterofWorldsHead, player);
                }
                if (arg == ItemID.BrainOfCthulhuBossBag)
                {
                    StaminaVesselOnFirstBag(NPCID.BrainofCthulhu, player);
                    SoulsOnFirstBag(NPCID.BrainofCthulhu, player);
                }
                if (arg == ItemID.QueenBeeBossBag)
                {
                    if (Slain.ContainsKey(NPCID.QueenBee))
                    {
                        if (Slain[NPCID.QueenBee] == 0)
                        {
                            int enemyValue = 5000;
                            float multiplier = tsorcRevampPlayer.CheckSoulsMultiplier(player);

                            int DarkSoulQuantity = (int)(multiplier * enemyValue);

                            StaminaVesselOnFirstBag(NPCID.QueenBee, player);
                            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<DarkSoul>(), DarkSoulQuantity);
                            Slain[NPCID.QueenBee] = 1;
                        }
                    };
                }
                if (arg == ItemID.WallOfFleshBossBag)
                {
                    EstusFlaskShardOnFirstBag(NPCID.WallofFlesh, player);
                    SoulsOnFirstBag(NPCID.WallofFlesh, player);
                }
                if (arg == ItemID.SkeletronBossBag)
                {
                    SublimeBoneDustOnFirstBag(NPCID.SkeletronHead, player);
                    SoulsOnFirstBag(NPCID.SkeletronHead, player);
                    player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<MiakodaFull>());
                }
                if (arg == ItemID.DestroyerBossBag)
                {
                    SoulsOnFirstBag(NPCID.TheDestroyer, player);
                    player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<RTQ2>());
                    player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<CrestOfCorruption>(), 1);
                }
                if (arg == ItemID.TwinsBossBag)
                {
                    /* 
                    * picture the following:
                    * Twins are killed. Spazmatism is added to Slain, and the player opens a bag and receives souls
                    * then, Twins are killed again. Retinazer is added to slain this time, and the player opens a bag and gets souls again
                    * to prevent this, we need to make sure we haven't opened a bag from Spazmatism when we open a bag in Retinazer's context
                    */
                    if (Slain.ContainsKey(NPCID.Retinazer))
                    {
                        if (Slain[NPCID.Retinazer] == 0)
                        {
                            bool SpazmatismDowned = Slain.TryGetValue(NPCID.Spazmatism, out int value);
                            //if SpazmatismDowned evaluates to true, int value is set to the value pair of Spazmatism's key, which stores if a bag has been opened
                            if (!SpazmatismDowned || value == 0)
                            { //if Spazmatism is not in Slain, or no twins bag has been opened in Spazmatism's context
                                AddBossBagSouls(NPCID.Retinazer, player);
                                Slain[NPCID.Retinazer] = 1;
                            }
                        }
                    }
                    else if (Slain.ContainsKey(NPCID.Spazmatism))
                    { //dont need to check if Retinazer is downed, since this is only run if Retinazer is not in Slain
                        if (Slain[NPCID.Spazmatism] == 0)
                        {
                            AddBossBagSouls(NPCID.Spazmatism, player);
                            Slain[NPCID.Spazmatism] = 1;
                        }
                    }
                    player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<CrestOfSky>(), 1);
                }
                if (arg == ItemID.SkeletronPrimeBossBag)
                {
                    SublimeBoneDustOnFirstBag(NPCID.SkeletronPrime, player);
                    SoulsOnFirstBag(NPCID.SkeletronPrime, player);
                    player.QuickSpawnItem(player.GetSource_Loot(), ItemID.AngelWings);
                    player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<CrestOfSteel>(), 1);
                }
                if (arg == ItemID.PlanteraBossBag)
                {
                    player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<CrestOfLife>());
                    player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<SoulOfLife>(), 3);
                    SoulsOnFirstBag(NPCID.Plantera, player);
                }
                if (arg == ItemID.FishronBossBag)
                {
                    StaminaVesselOnFirstBag(NPCID.DukeFishron, player);
                    SoulsOnFirstBag(NPCID.DukeFishron, player);
                }
                if (arg == ItemID.MoonLordBossBag)
                {
                    if (Slain.ContainsKey(NPCID.MoonLordCore))
                    {
                        if (Slain[NPCID.MoonLordCore] == 0)
                        {
                            int enemyValue = 40000; // 1 platinum / 25
                            float multiplier = tsorcRevampPlayer.CheckSoulsMultiplier(player);

                            int DarkSoulQuantity = (int)(multiplier * enemyValue);

                            player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<DarkSoul>(), DarkSoulQuantity);
                            Slain[NPCID.MoonLordCore] = 1;
                        }
                    }
                }
            }
        }
    }
}
