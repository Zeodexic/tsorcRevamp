﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Melee {
    class ForgottenIceRod : ModItem {

        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Randomly casts ice 2.");
        }

        public override void SetDefaults() {
            item.rare = ItemRarityID.Blue;
            item.damage = 20;
            item.height = 26;
            item.knockBack = 4;
            item.melee = true;
            item.autoReuse = true;
            item.useAnimation = 27;
            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 21;
            item.value = PriceByRarity.Blue_1;
            item.width = 26;
        }

        public override bool UseItem(Player player) {
            if (Main.rand.Next(5) == 0) {
                Projectile.NewProjectile(player.position.X, player.position.Y, (float)(-40 + Main.rand.Next(80)) / 10, 14.9f, ModContent.ProjectileType<Projectiles.Ice2Ball>(), 20, 2.0f, player.whoAmI);
            }
            return true;
        }
    }
}
