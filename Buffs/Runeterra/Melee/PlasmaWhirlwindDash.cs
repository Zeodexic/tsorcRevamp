﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Items.Weapons.Melee.Runeterra;
using tsorcRevamp.Projectiles.Melee.Runeterra;

namespace tsorcRevamp.Buffs.Runeterra.Melee
{
    public class PlasmaWhirlwindDash : ModBuff
    {
        public Vector2 DashVelocity;
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.CoralTorch, Scale: 3f);
            dust.noGravity = true;

            player.immune = true;

            if (player.buffTime[buffIndex] == (int)(PlasmaWhirlwind.DashDuration * 60 * 2))
            {
                SoundEngine.PlaySound(new SoundStyle("tsorcRevamp/Sounds/Runeterra/Melee/PlasmaWhirlwind/Dash") with { Volume = 1f }, player.Center);
                DashVelocity = player.DirectionTo(player.GetModPlayer<tsorcRevampPlayer>().SweepingBladePosition) * 17;
                Projectile DashHitbox = Projectile.NewProjectileDirect(Projectile.GetSource_None(), player.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaWhirlwindDashHitbox>(), PlasmaWhirlwind.BaseDamage, 0, player.whoAmI);
                DashHitbox.OriginalCritChance = SteelTempest.CritChance;
            }
            if (player.buffTime[buffIndex] >= (int)(PlasmaWhirlwind.DashDuration * 60))
            {
                player.velocity = DashVelocity;
            }
        }
    }
}
