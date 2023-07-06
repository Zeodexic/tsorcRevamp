﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Debug
{
    public class DamageDebugTome : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.UseSound = SoundID.Item11;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.Master;
        }
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<tsorcRevampPlayer>().NoDamageSpread = !player.GetModPlayer<tsorcRevampPlayer>().NoDamageSpread;
            return true;
        }
    }
}