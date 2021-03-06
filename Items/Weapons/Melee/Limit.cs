﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Melee {
    class Limit : ModItem {

        public override void SetDefaults() {
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 24;
            item.height = 24;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.melee = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = ModContent.ProjectileType<Projectiles.Limit>();
            item.useAnimation = 40;
            item.useTime = item.useAnimation / 4;
            item.shootSpeed = 0f;
            item.damage = 150;
            item.knockBack = 6.5f;
            item.value = Item.sellPrice(0, 20);
            item.crit = 10;
            item.rare = ItemRarityID.Red;
            item.glowMask = 271;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            Vector2 mousePos = Main.MouseWorld;
            Vector2 playerToMouse = mousePos - player.Center;

            if (playerToMouse.Length() > 75f) {
                playerToMouse *= 75f / playerToMouse.Length();
                mousePos = player.Center + playerToMouse;
            }
            Projectile.NewProjectile(mousePos, new Vector2(18,  0), ModContent.ProjectileType<Projectiles.Limit>(), item.damage, item.knockBack, item.owner);
            return false;
        }
    }
}
