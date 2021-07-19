﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace tsorcRevamp.Items {
    public class tsorcGlobalItem : GlobalItem {

        public override void MeleeEffects(Item item, Player player, Rectangle hitbox) {
            tsorcRevampPlayer modPlayer = player.GetModPlayer<tsorcRevampPlayer>();

            if (modPlayer.MiakodaCrescentBoost) {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 164, player.velocity.X * 1.2f, player.velocity.Y * 1.2f, 80, default(Color), 1.2f);
                Main.dust[dust].noGravity = true;
            }

            if (modPlayer.MiakodaNewBoost) {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 57, player.velocity.X * 1.2f, player.velocity.Y * 1.2f, 120, default(Color), 1.2f);
                Main.dust[dust].noGravity = true;
            }

			if (modPlayer.MagicWeapon) {
				Lighting.AddLight(new Vector2(hitbox.X, hitbox.Y), 0.3f, 0.3f, 0.45f);
				for (int i = 0; i < 4; i++)
				{
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 68, player.velocity.X * 1, player.velocity.Y * 1, 30, default(Color), .9f);
					Main.dust[dust].noGravity = true;
				}
				{
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 15, player.velocity.X * 1, player.velocity.Y * 1, 30, default(Color), .9f);
					Main.dust[dust].noGravity = true;
				}
			}

			if (modPlayer.GreatMagicWeapon)
			{
				Lighting.AddLight(new Vector2(hitbox.X, hitbox.Y), 0.3f, 0.3f, 0.55f);
				for (int i = 0; i < 3; i++)
				{
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 172, player.velocity.X * 1, player.velocity.Y * 1, 30, default(Color), .9f);
					Main.dust[dust].noGravity = true;
				}
				{
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 68, player.velocity.X * 1, player.velocity.Y * 1, 30, default(Color), .9f);
					Main.dust[dust].noGravity = true;
				}
                {
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 172, player.velocity.X * 1, player.velocity.Y * 1, 30, default(Color), 1.3f);
					Main.dust[dust].noGravity = true;
				}
			}

			if (modPlayer.CrystalMagicWeapon)
			{
				Lighting.AddLight(new Vector2(hitbox.X, hitbox.Y), 0.3f, 0.3f, 0.55f);
				for (int i = 0; i < 2; i++)
				{
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 221, player.velocity.X * 1, player.velocity.Y * 1, 30, default(Color), .9f);
					Main.dust[dust].noGravity = true;
				}
				{
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 68, player.velocity.X * 1, player.velocity.Y * 1, 30, default(Color), .9f);
					Main.dust[dust].noGravity = true;
				}
				{
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 172, player.velocity.X * 1, player.velocity.Y * 1, 30, default(Color), 1.3f);
					Main.dust[dust].noGravity = true;
				}
			}
		}

        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit) {
            tsorcRevampPlayer modPlayer = player.GetModPlayer<tsorcRevampPlayer>();

            if (modPlayer.MiakodaCrescentBoost) {
                target.AddBuff(ModContent.BuffType<Buffs.CrescentMoonlight>(), 240);
            }

            if (modPlayer.MiakodaNewBoost) {
                target.AddBuff(BuffID.Midas, 300);
            }

			if (modPlayer.MagicWeapon || modPlayer.GreatMagicWeapon) {
				Main.PlaySound(SoundID.NPCHit44.WithVolume(.3f), target.position);
            }

			if (modPlayer.CrystalMagicWeapon)
			{
				Main.PlaySound(SoundID.Item27.WithVolume(.3f), target.position);
			}
		}

		public override void ModifyWeaponDamage(Item item, Player player, ref float add, ref float mult, ref float flat)
		{
			tsorcRevampPlayer modPlayer = player.GetModPlayer<tsorcRevampPlayer>();

			if (item.melee)
			{
				if (modPlayer.MagicWeapon)
				{
					add += (player.magicDamage - player.magicDamageMult) * 0.5f /*- (player.meleeDamageMult * 0.05f)*/;
				}
				if (modPlayer.GreatMagicWeapon)
				{
					add += (player.magicDamage - player.magicDamageMult) * .75f /*- (player.meleeDamageMult * 0.1f)*/; 
				}
				if (modPlayer.CrystalMagicWeapon)
				{
					add += (player.magicDamage - player.magicDamageMult) * 1f /*- (player.meleeDamageMult * 0.15f)*/; //scales same as melee damage bonus would
				}
			}
        }

		#region PrefixChance (taken from Example Mod, leaving most original comments in)

		public override bool? PrefixChance(Item item, int pre, UnifiedRandom rand)
		{
			// pre: The prefix being applied to the item, or the roll mode
			// -1 is when an item is naturally generated in a chest, crafted, purchased from an NPC, looted from a grab bag (excluding presents), or dropped by a slain enemy
			// -2 is when an item is rolled in the tinkerer
			// -3 determines if an item can be placed in the tinkerer slot

			// To prevent putting an item in the tinkerer slot, return false when pre is -3
			/*if (pre == -3 && item.type == ItemID.LaserRifle)
			{
				// This will make the Laser Rifle not be reforgeable at all (useful if you want your item to preserve its custom name color)
				return false;
			}*/

			// To make an item reset its prefix when reforging
			/*if (pre == -2)
			{
				if (Main.LocalPlayer.HasBuff(BuffID.Tipsy))
				{
					// If the player is drunk, make it remove the prefix
					return false;
				}
			}*/

			// To prevent rolling of a prefix on spawn, return false when pre is -1
			if (pre == -1)
			{
				if (item.modItem?.mod == mod)
				{
					// All weapons/accesories from tsorcRevamp can have a prefix when they are crafted, bought, taken from a generated chest, opened, or dropped by an enemy
					return true;
				}
			}

			// For the following code, this is useful to know (from the terraria wiki):
			// Nearly all weapons and accessories have a 75% chance of receiving a random modifier upon the item's creation
			// (naturally generated in a chest, crafted, purchased from an NPC, looted from a grab bag (excluding presents), or dropped by a slain enemy).

			// To change the chance of a prefix being rolled or not, return true or false depending on some condition
			/*if (pre == -1 && item.type == ItemID.Shackle)
			{
				// Force rolling
				// return true;

				// When using random numbers, make sure to use the rand object passed into this method, and not Main.rand.
				// This will make it consistent with worldgen should this item be spawned in a chest
				if (rand.NextFloat() < 0.5f)
				{
					// Increase the chance of not receiving any prefix on spawn by 50%
					return false;
				}
				// Keep in mind that if the code arrives here, there is still a 25% chance that it won't get a modifier.
				// If you want a more controlled approach, return true in an else block
			}*/

			return null;
		}

        #endregion

    }
}