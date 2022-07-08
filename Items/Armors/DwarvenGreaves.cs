﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    class DwarvenGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases minion damage by 12%\nIncreases your max number of minions by 1\nIncreases movement speed by 25%");
        }

        public override void SetDefaults()
        {
            Item.height = Item.width = 18;
            Item.defense = 15;
            Item.value = 24000;
            Item.rare = ItemRarityID.Pink;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.12f;
            player.maxMinions += 1;
            player.moveSpeed += 0.25f;
        }
        public override void AddRecipes()
        {
            Terraria.Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HallowedGreaves, 1);
            recipe.AddIngredient(Mod.Find<ModItem>("DarkSoul").Type, 7200);
            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }
    }
}
