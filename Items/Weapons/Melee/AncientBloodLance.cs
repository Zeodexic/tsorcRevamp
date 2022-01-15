using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Melee {
    public class AncientBloodLance : ModItem {

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ancient Blood Lance");
            Tooltip.SetDefault("Pierces multiple times on every hit.");
        }

        public override void SetDefaults() {
            item.damage = 33;
            item.knockBack = 6.5f;

            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 13;
            item.useTime = 4;
            item.shootSpeed = 8;
            //item.shoot = ProjectileID.DarkLance;
            
            item.height = 50;
            item.width = 50;

            item.melee = true;
            item.noMelee = true;
            item.noUseGraphic = true;

            item.value = PriceByRarity.Orange_3;
            item.rare = ItemRarityID.Orange;
            item.maxStack = 1;
            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<Projectiles.AncientBloodLance>();

        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DarkLance);
            recipe.AddIngredient(mod.GetItem("DarkSoul"), 6000);
            recipe.SetResult(this, 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.AddRecipe();
        }
    }
}
