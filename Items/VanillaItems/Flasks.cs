﻿using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.VanillaItems {
    class Flasks : GlobalItem {

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (item.type == ItemID.FlaskofVenom
                || item.type == ItemID.FlaskofCursedFlames
                || item.type == ItemID.FlaskofFire
                || item.type == ItemID.FlaskofGold
                || item.type == ItemID.FlaskofIchor
                || item.type == ItemID.FlaskofNanites
                || item.type == ItemID.FlaskofParty
                || item.type == ItemID.FlaskofPoison
                ) {
                tooltips.Insert(3, new TooltipLine(mod, "", "Adds 10% melee damage"));
            }
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddIngredient(ItemID.Deathweed, 1);
            recipe.AddIngredient(ItemID.Fireblossom, 4);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(ItemID.FlaskofFire, 1);
            recipe.AddRecipe();
        }
    }
}
