﻿using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Magic {
    class FreezeBolt3 : ModItem {

        public override string Texture => "tsorcRevamp/Items/Weapons/Magic/FreezeBolt2";
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Freeze Bolt III");
            Tooltip.SetDefault("Casts a super fast-moving bolt of ice");
        }

        public override void SetDefaults() {
            item.width = 24;
            item.height = 28;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 12;
            item.useTime = 12;
            item.damage = 150;
            item.knockBack = 8;
            item.autoReuse = true;
            item.UseSound = SoundID.Item21;
            item.rare = ItemRarityID.Cyan;
            item.shootSpeed = 11;
            item.mana = 12;
            item.value = PriceByRarity.Cyan_9;
            item.magic = true;
            item.shoot = ModContent.ProjectileType<Projectiles.FreezeBolt>();
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("FreezeBolt2"), 1);
            recipe.AddIngredient(mod.GetItem("SoulOfAttraidies"), 1);
            recipe.AddIngredient(mod.GetItem("BlueTitanite"), 10);
            recipe.AddIngredient(mod.GetItem("DarkSoul"), 150000);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
