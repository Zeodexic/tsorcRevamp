﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
namespace tsorcRevamp.Items.Weapons.Ranged {
    internal class BlizzardBlaster : ModItem {

        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Fires a blast of freezing wind,\n damaging up to 8 enemies in a cone.");
        }

        static readonly int RADIUS = 300;
        static readonly float WIDTH = (RADIUS / 500f); //idk it just works (tm)
        public override void SetDefaults() {
            item.damage = 91;
            item.useTime = 26;
            item.useAnimation = 26;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.ranged = true;
            item.shoot = ModContent.ProjectileType<BlizzardBlasterShot>();
            item.shootSpeed = 10f; //unused
            item.value = PriceByRarity.Pink_5;
            item.rare = ItemRarityID.Pink;
            //item.UseSound = SoundID.Item98;
        }

        public override Vector2? HoldoutOffset() {
            return new Vector2(-12f, 0f);
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 15000);
            recipe.AddIngredient(ItemID.CobaltBar, 12);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            float aimRotation = (player.Center - Main.MouseWorld).ToRotation();
            List<NPC> targetList = FindTargets(player);
            for (int i = 0; i < 24; i++) {
                float coneAngle = aimRotation + MathHelper.ToRadians(135) + Main.rand.NextFloat(-WIDTH, WIDTH);
                Dust dust = Dust.NewDustPerfect(player.Center + Vector2.One.RotatedBy(coneAngle) * 20f, 229, Vector2.One.RotatedBy(coneAngle) * Main.rand.NextFloat(3f, 24f), 0, default, 3f);
                dust.noGravity = true;
            }
            for (int i = 0; i < 6; i++) {
                float coneAngle = aimRotation + MathHelper.ToRadians(135) + (((i % 2) * 2) - 1);
                Dust dust = Dust.NewDustPerfect(player.Center + Vector2.One.RotatedBy(coneAngle) * 20f, 229, Vector2.One.RotatedBy(coneAngle) * Main.rand.NextFloat(3f, 24f), 0, default, 3f);
                dust.noGravity = true;
            }
            if (targetList.Count != 0) {
                targetList.Sort((NPC a, NPC b) => (int)(Vector2.Distance(a.Center, player.Center) - Vector2.Distance(b.Center, player.Center)));
                for (int j = 0; j < targetList.Count; j++) {
                    int projectile = Projectile.NewProjectile(player.Center, Vector2.Zero, type, damage, knockBack, player.whoAmI);
                    BlizzardBlasterShot BlizzardBlasterShot = Main.projectile[projectile].modProjectile as BlizzardBlasterShot;
                    BlizzardBlasterShot.target = targetList[j];
                    if (j >= 8) {
                        break;
                    }
                }
            }
            Main.PlaySound(SoundID.Item98);
            return false;
        }

        private List<NPC> FindTargets(Player player) {
            List<NPC> list = new List<NPC>();
            float aim = (player.Center - Main.MouseWorld).ToRotation();

            for (int i = 0; i < Main.maxNPCs; i++) {
                NPC n = Main.npc[i];
                if (n.active && !n.dontTakeDamage && !n.townNPC) {
                    if (ConicalCollision(player.Center, RADIUS, aim, WIDTH, n.Hitbox) && Collision.CanHitLine(player.Center, 1, 1, n.Center, 1, 1)) {
                        list.Add(n);
                        if (list.Count == 10) {
                            break;
                        }
                    }

                }
            }
            return list;
        }

        public static bool ConicalCollision(Vector2 origin, int radius, float angle, float width, Rectangle targetHitbox) {
            if (CheckPoint(origin, radius, targetHitbox.TopLeft(), angle, width)) {
                return true;
            }
            else if (CheckPoint(origin, radius, targetHitbox.TopRight(), angle, width)) {
                return true;
            }
            else if (CheckPoint(origin, radius, targetHitbox.BottomLeft(), angle, width)) {
                return true;
            }
            else if (CheckPoint(origin, radius, targetHitbox.BottomRight(), angle, width)) {
                return true;
            }

            return false;
        }

        private static bool CheckPoint(Vector2 origin, int radius, Vector2 target, float angle, float width) {
            //draw a line from origin to target, and get the rotation
            float rotationToTarget = (origin - target).ToRotation();
            //check if target is inside the cone by making sure the difference between aim and the rotation to the target is not greater than width radians
            bool check = false;
            if (Vector2.Distance(origin, target) <= (float)radius && rotationToTarget > angle - width) {
                if (rotationToTarget < angle + width) {
                    check = true;
                }
            }
            //border case handling
            if (!check) {
                if (angle > rotationToTarget) angle -= (float)(2 * Math.PI);
                else rotationToTarget -= (float)(2 * Math.PI);
            }
            if (Vector2.Distance(origin, target) <= (float)radius && rotationToTarget > angle - width) {
                if (rotationToTarget < angle + width) {
                    check = true;
                }
            }
            return check;
        }
    }

    internal class BlizzardBlasterShot : ModProjectile {

        public NPC target;
        public override string Texture => "tsorcRevamp/Projectiles/InvisibleProj";

        public override bool? CanHitNPC(NPC target) {
            return target == this.target;
        }

        public override void SetDefaults() {
            projectile.width = 1;
            projectile.height = 1;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.timeLeft = 3;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;
        }

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Blizzard Blaster Shot");
        }

        public override void AI() {
            if (target == null) projectile.Kill();
            else projectile.Center = target.Center;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            target.AddBuff(BuffID.Frostburn, 120);
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}

