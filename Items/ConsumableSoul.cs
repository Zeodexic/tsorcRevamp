﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items
{

    public abstract class ConsumableSoul : ModItem // These can be placed on little breakable corpses found around the map like in DS, or just placed in chests or rare enemy drops.
    {
        //all consumable souls found here. It's odd how the item quantity is consumed at a random tick during item use. They behave quite oddly but they work.
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemIconPulse[item.type] = true; // Makes item pulsate in world.
            ItemID.Sets.ItemNoGravity[item.type] = true; // Makes item float in world.
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.SoulofSight);
            item.width = 14;
            item.height = 22;
            item.maxStack = 999;
            item.autoReuse = true;
            item.useTurn = true;
            item.value = 1;
            item.consumable = true;
            item.useAnimation = 61; // Needs to be 1 tick more than use time for it to work properly. Not sure why.
            item.useTime = 60;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.rare = ItemRarityID.Green; // Mainly for colour consistency.
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, 0.15f, 0.42f, 0.05f);
        }

        public override bool UseItem(Player player) // Won't consume item without this
        {
            return true;
        }
    }

    public class FadingSoul : ConsumableSoul
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Fading soul");
            Tooltip.SetDefault("Consume to gain a mere 50 souls");
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, 0.05f, 0.18f, 0.02f);
        }
        public override void UseStyle(Player player)
        {
            // Each frame, make some dust
            if (Main.rand.NextFloat() < .3f)
            {
                Dust.NewDust(player.BottomLeft, player.width, player.height - 40, 89, 0f, -5f, 100, default(Color), .75f);
            }


            // This sets up the itemTime correctly.
            if (player.itemTime == 0)
            {
                player.itemTime = (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(player, item));
            }

            else if (player.itemTime == (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(player, item)) / 2)
            {
                // This code runs once halfway through the useTime of the item. 
                Main.PlaySound(SoundID.NPCDeath52.WithVolume(.25f).WithPitchVariance(.3f), player.position); // Plays sound.
                player.QuickSpawnItem(mod.ItemType("DarkSoul"), 50); // Gives player souls.

                for (int d = 0; d < 10; d++)
                {
                    Dust.NewDust(player.BottomLeft, player.width, player.height - 40, 89, 0f, -5f, 80, default(Color), .75f); // player.Bottom if offset to the right for some reason, player.BottomLeft is centered
                }
            }
        }
    }
    public class LostUndeadSoul : ConsumableSoul
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Soul of a Lost Undead");
            Tooltip.SetDefault("Consume to gain 200 souls");
        }
        public override void UseStyle(Player player)
        {
            // Each frame, make some dust
            if (Main.rand.NextBool())
            {
                Dust.NewDust(player.BottomLeft, player.width, player.height-40, 89, 0f, -5f, 80, default(Color), .8f);
            }

            // This sets up the itemTime correctly.
            if (player.itemTime == 0)
            {
                player.itemTime = (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(player, item));
            }

            else if (player.itemTime == (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(player, item)) / 2)
            {
                // This code runs once halfway through the useTime of the item. 
                Main.PlaySound(SoundID.NPCDeath52.WithVolume(.35f).WithPitchVariance(.3f), player.position); // Plays sound.
                player.QuickSpawnItem(mod.ItemType("DarkSoul"), 200); // Gives player souls.

                for (int d = 0; d < 30; d++)
                {
                     Dust.NewDust(player.BottomLeft, player.width, player.height-40, 89, 0f, -5f, 50, default(Color), .8f); // player.Bottom if offset to the right for some reason, player.BottomLeft is centered
                }
            }
        }
    }

    public class NamelessSoldierSoul : ConsumableSoul
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Soul of a Nameless Soldier");
            Tooltip.SetDefault("Consume to gain 800 souls");
        }
        public override void UseStyle(Player player)
        {
            // Each frame, make some dust
            if (Main.rand.NextBool())
            {
                Dust.NewDust(player.BottomLeft, player.width, player.height - 40, 89, 0f, -5f, 50, default(Color), .8f);
            }

            if (Main.rand.NextFloat() < .3f)
            {
                Dust.NewDust(player.BottomLeft, player.width, player.height - 40, 89, 0f, -5f, 100, default(Color), .75f);
            }

            // This sets up the itemTime correctly.
            if (player.itemTime == 0)
            {
                player.itemTime = (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(player, item));
            }

            else if (player.itemTime == (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(player, item)) / 2)
            {
                // This code runs once halfway through the useTime of the item. 
                Main.PlaySound(SoundID.NPCDeath52.WithVolume(.5f).WithPitchVariance(.3f), player.position); // Plays sound.
                player.QuickSpawnItem(mod.ItemType("DarkSoul"), 800); // Gives player souls.

                for (int d = 0; d < 60; d++)
                {
                    Dust.NewDust(player.BottomLeft, player.width, player.height - 40, 89, 0f, -5f, 50, default(Color), .8f); // player.Bottom if offset to the right for some reason, player.BottomLeft is centered
                }

                for (int d = 0; d < 15; d++) // Left
                {
                    Dust.NewDust(player.BottomLeft, player.width, player.height - 55, 89, -6f, -4f, 50, default(Color), .65f); // player.Bottom if offset to the right for some reason, player.BottomLeft is centered
                }

                for (int d = 0; d < 15; d++) // Right
                {
                    Dust.NewDust(player.BottomLeft, player.width, player.height - 55, 89, 6f, -4f, 50, default(Color), .65f); // player.Bottom if offset to the right for some reason, player.BottomLeft is centered
                }
            }
        }
    }

    public class ProudKnightSoul : ConsumableSoul
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Soul of a Proud Knight");
            Tooltip.SetDefault("Consume to gain 2000 souls");
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, 0.25f, 0.60f, 0.15f);
        }
        public override void UseStyle(Player player)
        {
            // Each frame, make some dust
            if (Main.rand.NextBool())
            {
                Dust.NewDust(player.BottomLeft, player.width, player.height - 40, 89, 0f, -5f, 50, default(Color), .8f);
            }

            if (Main.rand.NextFloat() < .5f)
            {
                Dust.NewDust(player.BottomLeft, player.width, player.height - 40, 89, 0f, -5f, 100, default(Color), .75f);
            }

            // This sets up the itemTime correctly.
            if (player.itemTime == 0)
            {
                player.itemTime = (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(player, item));
            }

            else if (player.itemTime == (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(player, item)) / 2)
            {
                // This code runs once halfway through the useTime of the item. 
                Main.PlaySound(SoundID.NPCDeath52.WithVolume(.75f).WithPitchVariance(.3f), player.position); // Plays sound.
                player.QuickSpawnItem(mod.ItemType("DarkSoul"), 2000); // Gives player souls.
                Projectile.NewProjectile(player.Top, player.velocity, mod.ProjectileType("Soulsplosion"), 200, 15);

                for (int d = 0; d < 90; d++) // Upwards
                {
                    Dust.NewDust(player.BottomLeft, player.width, player.height - 40, 89, 0f, -5f, 30, default(Color), .8f); // player.Bottom if offset to the right for some reason, player.BottomLeft is centered
                }

                for (int d = 0; d < 30; d++) // Left
                {
                    Dust.NewDust(player.BottomLeft, player.width, player.height - 55, 89, -6f, -4f, 30, default(Color), .7f); // player.Bottom if offset to the right for some reason, player.BottomLeft is centered
                }

                for (int d = 0; d < 30; d++) // Right
                {
                    Dust.NewDust(player.BottomLeft, player.width, player.height - 55, 89, 6f, -4f, 30, default(Color), .7f); // player.Bottom if offset to the right for some reason, player.BottomLeft is centered
                }
            }
        }

    }

    public class BraveWarriorSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Soul of a Brave Warrior");
            Tooltip.SetDefault("Consume to gain 5000 souls");
            ItemID.Sets.ItemIconPulse[item.type] = true; // Makes item pulsate in world.
            ItemID.Sets.ItemNoGravity[item.type] = true; // Makes item float in world.
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.SoulofSight);
            item.width = 14;
            item.height = 22;
            item.maxStack = 999;
            item.autoReuse = true;
            item.useTurn = true;
            item.value = 1;
            item.consumable = true;
            item.useAnimation = 121; // Needs to be 1 tick more than use time for it to work properly. Not sure why.
            item.useTime = 120;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.rare = ItemRarityID.Green; // Mainly for colour consistency.
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, 0.2f, 0.52f, 0.08f);
        }

        public override bool UseItem(Player player) // Won't consume item without this
        {
            return true;
        }

        public override void UseStyle(Player player)
        {
            // Each frame, make some dust
            if (Main.rand.NextBool())
            {
                Dust.NewDust(player.BottomLeft, player.width, player.height - 40, 89, 0f, -5f, 50, default(Color), .8f);
            }

            if (Main.rand.NextFloat() < .75f)
            {
                Dust.NewDust(player.BottomLeft, player.width, player.height - 40, 89, 0f, -5f, 100, default(Color), .75f);
            }

            // This sets up the itemTime correctly.
            if (player.itemTime == 0)
            {
                player.itemTime = (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(player, item));
            }

            else if (player.itemTime == (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(player, item)) / 2)
            {
                // This code runs once halfway through the useTime of the item. 
                Main.PlaySound(SoundID.NPCDeath52.WithVolume(.9f).WithPitchVariance(.3f), player.position); // Plays sound.
                player.QuickSpawnItem(mod.ItemType("DarkSoul"), 5000); // Gives player souls.
                Projectile.NewProjectile(player.Top, player.velocity, mod.ProjectileType("Soulsplosion"), 200, 15);

                for (int d = 0; d < 100; d++) // Upwards
                {
                    Dust.NewDust(player.BottomLeft, player.width, player.height - 40, 89, 0f, -5f, 30, default(Color), .8f); // player.Bottom if offset to the right for some reason, player.BottomLeft is centered
                }

                for (int d = 0; d < 40; d++) // Left
                {
                    Dust.NewDust(player.BottomLeft, player.width, player.height - 55, 89, -6f, -3f, 30, default(Color), .7f); // player.Bottom if offset to the right for some reason, player.BottomLeft is centered
                }

                for (int d = 0; d < 40; d++) // Right
                {
                    Dust.NewDust(player.BottomLeft, player.width, player.height - 55, 89, 6f, -3f, 30, default(Color), .7f); // player.Bottom if offset to the right for some reason, player.BottomLeft is centered
                }

                for (int d = 0; d < 40; d++) // Left
                {
                    Dust.NewDust(player.BottomLeft, player.width, player.height - 55, 89, -6f, -6f, 30, default(Color), .7f); // player.Bottom if offset to the right for some reason, player.BottomLeft is centered
                }

                for (int d = 0; d < 40; d++) // Right
                {
                    Dust.NewDust(player.BottomLeft, player.width, player.height - 55, 89, 6f, -6f, 30, default(Color), .7f); // player.Bottom if offset to the right for some reason, player.BottomLeft is centered
                }
            }
        }

    }

    public class HeroSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Soul of a Hero");
            Tooltip.SetDefault("Consume to gain 10000 souls");
            ItemID.Sets.ItemIconPulse[item.type] = true; // Makes item pulsate in world.
            ItemID.Sets.ItemNoGravity[item.type] = true; // Makes item float in world.
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.SoulofSight);
            item.width = 14;
            item.height = 22;
            item.maxStack = 999;
            item.autoReuse = true;
            item.useTurn = true;
            item.value = 1;
            item.consumable = true;
            item.useAnimation = 241; // Needs to be 1 tick more than use time for it to work properly. Not sure why.
            item.useTime = 240;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.rare = ItemRarityID.Green; // Mainly for colour consistency.
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, 0.25f, 0.6f, 0.10f);
        }

        public override bool UseItem(Player player) // Won't consume item without this
        {
            return true;
        }

        public override void UseStyle(Player player)
        {
            // Each frame, make some dust
            if (Main.rand.NextBool())
            {
                Dust.NewDust(player.BottomLeft, player.width, player.height - 40, 89, 0f, -5f, 50, default(Color), .8f);
            }

            if (Main.rand.NextFloat() < 1f)
            {
                Dust.NewDust(player.BottomLeft, player.width, player.height - 40, 89, 0f, -5f, 50, default(Color), .75f);
            }

            // This sets up the itemTime correctly.
            if (player.itemTime == 0)
            {
                player.itemTime = (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(player, item));
            }

            else if (player.itemTime == (int)(item.useTime / PlayerHooks.TotalUseTimeMultiplier(player, item)) / 2)
            {
                // This code runs once halfway through the useTime of the item. 
                Main.PlaySound(SoundID.NPCDeath52.WithVolume(1f).WithPitchVariance(.3f), player.position); // Plays sound.
                player.QuickSpawnItem(mod.ItemType("DarkSoul"), 10000); // Gives player souls.
                Projectile.NewProjectile(player.Top, player.velocity, mod.ProjectileType("SoulsplosionLarge"), 1000, 15);

                for (int d = 0; d < 100; d++) // Upwards
                {
                    Dust.NewDust(player.BottomLeft, player.width, player.height - 40, 89, 0f, -5f, 30, default(Color), .8f); // player.Bottom if offset to the right for some reason, player.BottomLeft is centered
                }

                for (int d = 0; d < 70; d++) // Left
                {
                    Dust.NewDust(player.BottomLeft, player.width, player.height - 55, 89, -6f, -3f, 30, default(Color), .7f); // player.Bottom if offset to the right for some reason, player.BottomLeft is centered
                }

                for (int d = 0; d < 70; d++) // Right
                {
                    Dust.NewDust(player.BottomLeft, player.width, player.height - 55, 89, 6f, -3f, 30, default(Color), .7f); // player.Bottom if offset to the right for some reason, player.BottomLeft is centered
                }

                for (int d = 0; d < 1000; d++) // Left
                {
                    int dust = Dust.NewDust(player.BottomLeft, player.width, player.height - 55, 89, Main.rand.NextFloat(-3.5f, 3.5f), Main.rand.NextFloat(-3f, -10f), 30, default(Color), Main.rand.NextFloat(.5f, .8f));
                }
            }
        }

    }
}