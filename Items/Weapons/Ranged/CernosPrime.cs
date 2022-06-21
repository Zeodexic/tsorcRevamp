﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Ranged
{
    public class CernosPrime : ModItem
    {

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires three arrows. \nHold FIRE to charge. \nArrows are faster and more accurate when the bow is charged.");
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ModContent.ProjectileType<Projectiles.CernosPrimeHeld>();
            Item.channel = true;

            Item.damage = 795;
            Item.width = 24;
            Item.height = 48;
            Item.useTime = 48;
            Item.useAnimation = 48;
            Item.reuseDelay = 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 4f;
            Item.value = PriceByRarity.Purple_11;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item7;
            Item.useAmmo = AmmoID.Arrow;
            Item.shootSpeed = 24f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.CernosPrimeHeld>(), damage, knockback, player.whoAmI, type);
            return false;
        }

        public override void AddRecipes()
        {
            Terraria.Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SagittariusBow>(), 1);
            recipe.AddIngredient(ModContent.ItemType<FlameOfTheAbyss>(), 15);
            recipe.AddIngredient(ModContent.ItemType<GhostWyvernSoul>(), 1);
            recipe.AddIngredient(ModContent.ItemType<CursedSoul>(), 30);
            recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 300000);
            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }

    }
}
