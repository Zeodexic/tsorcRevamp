﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Armors {
    [AutoloadEquip(EquipType.Head)]
    class ShellHelmet : ModItem {

        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Armor made from the shell of a legenadry creature." +
                "\nSet bonus: Archery skill and +19% ranged crit activates when health falls below 160" +
                "\n+11% ranged crit otherwise");
        }
        public override void SetDefaults() {
            item.width = 18;
            item.height = 12;
            item.value = 35000;
            item.rare = ItemRarityID.Orange;
            item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) {
            return body.type == ModContent.ItemType<ShellArmor>() && legs.type == ModContent.ItemType<ShellGreaves>();
        }

        public override void UpdateArmorSet(Player player) {
            if (player.statLife <= 160) {
                player.archery = true;
                player.rangedCrit += 19;

                int dust = Dust.NewDust(new Vector2((float)player.position.X, (float)player.position.Y), player.width, player.height, 6, (player.velocity.X) + (player.direction * 1), player.velocity.Y, 100, Color.Black, 1.0f);
                Main.dust[dust].noGravity = true;
            }

            else {
                player.rangedCrit += 11;

            }
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.NecroHelmet);
            recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 3000);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
