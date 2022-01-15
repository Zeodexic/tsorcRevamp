
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Melee {
    public class CelestialLance : ModItem {

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Celestial Lance");
            Tooltip.SetDefault("Celestial lance fabled to hold sway over the world.\nGains 50% attack damage while falling. \nAlso has random chance to cast a healing spell on each strike.");
        }


        public override void SetDefaults() {
            item.damage = 500;
            item.knockBack = 10f;

            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 11;
            item.useTime = 1;
            item.shootSpeed = 8;
            //item.shoot = ProjectileID.DarkLance;

            item.height = 50;
            item.width = 50;

            item.melee = true;
            item.noMelee = true;
            item.noUseGraphic = true;

            item.value = PriceByRarity.Red_10;
            item.rare = ItemRarityID.Red;
            item.maxStack = 1;
            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<Projectiles.CelestialLance>();

        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
            if (player.gravDir == 1f && player.velocity.Y > 0 || player.gravDir == -1f && player.velocity.Y < 0) {
                mult = 1.5f;
            }

        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("Longinus"), 1);
            recipe.AddIngredient(mod.GetItem("WhiteTitanite"), 20);
            recipe.AddIngredient(mod.GetItem("CursedSoul"), 100);
            recipe.AddIngredient(ItemID.FallenStar, 20);
            recipe.AddIngredient(mod.GetItem("DarkSoul"), 240000);
            recipe.SetResult(this, 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.AddRecipe();
        }
    }
}
