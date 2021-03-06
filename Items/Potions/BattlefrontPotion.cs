﻿using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Potions {
    public class BattlefrontPotion : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Increases damage by 20%, critical strike chance " +
                             "\nby 5%, defense by 8, and swing speed by 20%." + 
                             "\nAlso gives Thorns and Battle potion effects.");

        }

        public override void SetDefaults() {
            item.width = 24;
            item.height = 30;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.useAnimation = 15;
            item.useTime = 15;
            item.useTurn = true;
            item.UseSound = SoundID.Item3;
            item.maxStack = 30;
            item.consumable = true;
            item.rare = ItemRarityID.Blue;
            item.value = 5000;
            item.buffType = ModContent.BuffType<Buffs.Battlefront>();
            item.buffTime = 28800;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BattlePotion, 1);
            recipe.AddIngredient(ItemID.ThornsPotion, 1);
            recipe.AddIngredient(ItemID.IronskinPotion, 1);
            recipe.AddIngredient(ItemID.ArcheryPotion, 1);
            recipe.AddIngredient(mod.GetItem("BoostPotion"), 1);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
