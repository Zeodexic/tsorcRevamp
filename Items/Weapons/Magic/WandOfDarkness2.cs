﻿using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Magic {
    class WandOfDarkness2 : ModItem {
        public override string Texture => "tsorcRevamp/Items/Weapons/Magic/WandOfDarkness";
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Wand of Darkness II");
            Tooltip.SetDefault("Greater damage and higher knockback");
            Item.staff[item.type] = true;
        }
        public override void SetDefaults() {
            item.autoReuse = true;
            item.width = 12;
            item.height = 17;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 25;
            item.useTime = 25;
            item.damage = 18;
            item.knockBack = 2.5f;
            if (!ModContent.GetInstance<tsorcRevampConfig>().LegacyMode) item.mana = 5;
            if (ModContent.GetInstance<tsorcRevampConfig>().LegacyMode) item.mana = 4;
            item.UseSound = SoundID.Item8;
            item.shootSpeed = 7;
            item.noMelee = true;
            item.value = PriceByRarity.Green_2;
            item.rare = ItemRarityID.Green;
            item.magic = true;
            item.shoot = ModContent.ProjectileType<Projectiles.ShadowBall>();
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("WandOfDarkness"), 1);
            recipe.AddIngredient(mod.GetItem("DarkSoul"), 2700);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
