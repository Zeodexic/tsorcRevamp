﻿using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.BossItems {
    class GrassyEgg : ModItem {

        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Summons The Hunter \n" + "You must sacrifice this at a Demon Altar in the Jungle far to the West");
        }

        public override void SetDefaults() {
            item.rare = ItemRarityID.LightRed;
            item.width = 12;
            item.height = 12;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.useAnimation = 5;
            item.useTime = 5;
            item.maxStack = 1;
            item.consumable = false;
        }


        public override bool UseItem(Player player) {
            bool zoneJ = player.ZoneJungle;
            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.TheHunter>())) {
                return false;
            }
            else if (!zoneJ) {
                Main.NewText("You can only use this in the Jungle.");
            }
            else {
                NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Bosses.TheHunter>());
            }
            return true;
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Stinger, 99);
            recipe.AddIngredient(ItemID.JungleSpores, 99);
            recipe.AddIngredient(ItemID.ShadowScale, 99);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
