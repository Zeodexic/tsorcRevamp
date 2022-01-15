using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Ranged {
    public class FocusedEnergyBeam : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Uses Tesla Bolts for ammo\n" +
                        "Fires a single high damage beam that obliterates most enemies on contact...\n" +
                        "A weapon from a civilization not of this world...");

        }

        public override void SetDefaults() {
            item.ranged = true;
            item.shoot = ProjectileID.PurificationPowder;

            item.damage = 750;
            item.width = 52;
            item.height = 22;
            item.knockBack = 9.0f;
            item.maxStack = 1;
            item.noMelee = true;
            item.autoReuse = true;
            item.rare = ItemRarityID.Red;
            item.useAmmo = mod.ItemType("TeslaBolt");
            item.shootSpeed = 30;
            item.useAnimation = 60;
            item.useTime = 60;
            item.UseSound = SoundID.Item12;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = PriceByRarity.Red_10;
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("CompactFrame"));
            recipe.AddIngredient(mod.GetItem("DestructionElement"));
            recipe.AddIngredient(ItemID.SpaceGun, 1);
            recipe.AddIngredient(mod.GetItem("DarkSoul"), 150000);

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        public override Vector2? HoldoutOffset() {
            return new Vector2(-6, 0);
        }
    }
}
