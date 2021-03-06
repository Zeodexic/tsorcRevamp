﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Buffs
{
    class MiakodaCrescent : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Miakoda - Crescent Moon Form");
            Description.SetDefault("An ancient being freed from Gravelord Nito." +
                                   "\nIt is said to possess a divine smile");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<tsorcRevampPlayer>().MiakodaCrescent = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Pets.MiakodaCrescent>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + player.width / 2, player.position.Y + player.height / 2, 0f, 0f, ModContent.ProjectileType<Projectiles.Pets.MiakodaCrescent>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
            player.allDamageMult += 0.03f;
        }
    }
}
