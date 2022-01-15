﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Magic {
    class EsunaTome : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("A lost tome known to cure all but the rarest of ailments.");
        }
        public override void SetDefaults() {
            item.height = 10;
            item.knockBack = 4;
            item.rare = ItemRarityID.Cyan;
            item.magic = true;
            item.noMelee = true;
            item.mana = 40;
            item.UseSound = SoundID.Item21;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useTime = 10;
            item.useAnimation = 10;
            item.value = PriceByRarity.Cyan_9;
            item.width = 34;
        }

        public override bool UseItem(Player player) {
            int buffIndex = 0;

            foreach (int buffType in player.buffType) {

                if ((buffType == BuffID.Bleeding) 
                    || (buffType == BuffID.Poisoned) 
                    || (buffType == BuffID.Confused) 
                    || (buffType == BuffID.BrokenArmor)
                    || (buffType == BuffID.Darkness)
                    || (buffType == BuffID.OnFire)
                    || (buffType == BuffID.Slow)
                    || (buffType == BuffID.Weak)
                    || (buffType == BuffID.CursedInferno)
                    || (buffType == BuffID.Cursed) // why are these in here? you can't use this item if you have cursed or silenced 
                    || (buffType == BuffID.Silenced) // the original tsorc mod has these in here, so im leaving them, but im *well* aware of how stupid this is
                    ) {
                    player.buffTime[buffIndex] = 0;
                }
                buffIndex++;
            }
            return true;
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpellTome);
            recipe.AddIngredient(mod.GetItem("GuardianSoul"), 1);
            recipe.AddIngredient(mod.GetItem("HealingElixir"), 10);
            recipe.AddIngredient(mod.GetItem("DarkSoul"), 70000);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
