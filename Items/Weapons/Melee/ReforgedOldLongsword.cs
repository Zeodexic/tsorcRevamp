﻿using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Melee {
    class ReforgedOldLongsword : ModItem {

        public override string Texture => "tsorcRevamp/Items/Weapons/Melee/OldLongsword";
        public override void SetDefaults() {
            item.damage = 12;
            item.width = 44;
            item.height = 44;
            item.knockBack = 4;
            item.maxStack = 1;
            item.melee = true;
            item.scale = .9f;
            item.useAnimation = 19;
            item.rare = ItemRarityID.White;
            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 21;
            item.value = 7000;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("OldLongsword"));
            recipe.AddTile(mod.GetTile("SweatyCyclopsForge"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
