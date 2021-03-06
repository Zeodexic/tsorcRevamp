using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Melee {
    public class FieryZweihander : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Fiery Zweihander");

        }

        public override void SetDefaults() {

            item.rare = ItemRarityID.Orange;
            item.damage = 37;
            item.width = 56;
            item.height = 56;
            item.knockBack = 7;
            item.maxStack = 1;
            item.melee = true;
            item.scale = (float)1.3;
            item.useAnimation = 37;
            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 21;
            item.value = 28000;
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.HellstoneBar, 31);
            recipe.AddIngredient(mod.GetItem("DarkSoul"), 1400);
            recipe.AddIngredient(mod.GetItem("DarkSoul"), 1000);

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        public override void OnHitNPC(Player player, NPC npc, int damage, float knockBack, bool crit) {
            if (Main.rand.Next(2) == 0) {
                npc.AddBuff(BuffID.OnFire, 360, false); //Light 'em on fire! 
            }
        }

        public override void MeleeEffects(Player player, Rectangle rectangle) {
            Color color = new Color();
            //This is the same general effect done with the Fiery Greatsword
            int dust = Dust.NewDust(new Vector2(rectangle.X, rectangle.Y), rectangle.Width, rectangle.Height, 6, player.velocity.X * 0.2f + player.direction * 3, player.velocity.Y * 0.2f, 100, color, 1.9f);
            Main.dust[dust].noGravity = true;
        }

    }
}
