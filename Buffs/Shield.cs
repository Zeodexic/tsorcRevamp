﻿using Terraria;
using Terraria.ModLoader;

namespace tsorcRevamp.Buffs
{
    class Shield : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Shield");
            Description.SetDefault("Defense is increased by 62!");
            Main.debuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 62;
            if (!ModContent.GetInstance<tsorcRevampConfig>().LegacyMode) {
                Lighting.AddLight(player.Center, .7f, .7f, .45f);
                Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), player.velocity.X, player.velocity.Y, mod.ProjectileType("Shield"), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}