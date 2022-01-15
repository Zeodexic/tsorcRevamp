using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Ranged {
    public class AntimatRifle : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Antimat Rifle");
            Tooltip.SetDefault("Unbelievable damage at the cost of a 2.5 second cooldown between shots \n" +
                                "Fires piercing high-velocity rounds that punch through thin walls\n" +
                                "Damage increases with enemy armor");
        }

        public override void SetDefaults() {

            //item.prefixType=96;
            item.autoReuse = true;
            item.damage = 3000;
            item.width = 78;
            item.height = 26;
            item.knockBack = 5;
            item.maxStack = 1;
            item.noMelee = true;
            item.rare = ItemRarityID.Red;
            item.scale = (float)0.9;
            item.useAmmo = AmmoID.Bullet;
            item.ranged = true;
            item.shoot = AmmoID.Bullet;
            item.shootSpeed = 10;
            //item.pretendType=96;
            item.useAnimation = 150;
            item.useTime = 150;
            item.UseSound = SoundID.Item36;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = PriceByRarity.Red_10;
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.SniperRifle, 1);
            recipe.AddIngredient(mod.GetItem("DestructionElement"), 1);
            recipe.AddIngredient(mod.GetItem("SoulOfChaos"), 1);
            recipe.AddIngredient(mod.GetItem("Humanity"), 20);
            recipe.AddIngredient(mod.GetItem("CursedSoul"), 100);
            recipe.AddIngredient(mod.GetItem("DarkSoul"), 240000);

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        public override void HoldItem(Player player)
        {
            player.scope = true;
        }

        public override void UseStyle(Player player) {
            float backX = 24f; // move the weapon back
            float downY = 0f; // and down
            float cosRot = (float)Math.Cos(player.itemRotation);
            float sinRot = (float)Math.Sin(player.itemRotation);
            //Align
            player.itemLocation.X = player.itemLocation.X - backX * cosRot * player.direction - downY * sinRot * player.gravDir;
            player.itemLocation.Y = player.itemLocation.Y - backX * sinRot * player.direction + downY * cosRot * player.gravDir;
        }
        public override bool Shoot(Player P, ref Vector2 Pos, ref float speedX, ref float speedY, ref int type, ref int DMG, ref float KB) {

            type = ModContent.ProjectileType<Projectiles.AntiMaterialRound>();
            Projectile.NewProjectile(Pos.X, Pos.Y, speedX, speedY, type, DMG, KB, P.whoAmI);
            return false;
        }

    }
}
