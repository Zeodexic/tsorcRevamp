﻿using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Ranged {
    class ReforgedOldCrossbow : ModItem {
        public override string Texture => "tsorcRevamp/Items/Weapons/Ranged/Crossbow";
        public override void SetDefaults() {
            item.damage = 19;
            item.width = 28;
            item.height = 14;
            item.knockBack = 4;
            item.maxStack = 1;
            item.ranged = true;
            item.scale = 1;
            item.crit = 16;
            item.useAnimation = 45;
            item.useTime = 45;
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item5;
            item.useAmmo = mod.ItemType("Bolt");
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 10;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = PriceByRarity.Blue_1;
            item.noMelee = true;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("OldCrossbow"));
            recipe.AddTile(mod.GetTile("SweatyCyclopsForge"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
