﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Melee {
    class ForgottenGaiaSword : ModItem {

        public override void SetStaticDefaults() {
            Tooltip.SetDefault("A blade made to slay the Witchking.\n" + "Does 3x damage and dispels the defensive shield of the Witchking");
        }

        public override void SetDefaults() {
            item.rare = ItemRarityID.Red;
            item.autoReuse = true;
            item.damage = 185;
            item.height = 50;
            item.knockBack = 8;
            item.maxStack = 1;
            item.melee = true;
            item.scale = 1.1f;
            item.useAnimation = 21;
            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 21;
            item.value = PriceByRarity.Red_10;
            item.width = 50;
        }



        /*public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FallenStar, 120);
            recipe.AddIngredient(mod.GetItem("GuardianSoul"), 1);
            recipe.AddIngredient(mod.GetItem("WhiteTitanite"), 10);
            recipe.AddIngredient(mod.GetItem("DarkSoul"), 100000);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }*/  

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (target.type == ModContent.NPCType<NPCs.Bosses.SuperHardMode.Witchking>())
            {
                target.AddBuff(ModContent.BuffType<Buffs.DispelShadow>(), 36000);
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendData(MessageID.AddNPCBuff, number: target.whoAmI, number2: ModContent.BuffType<Buffs.DispelShadow>(), number3: 36000);
                    ModPacket shadowPacket = ModContent.GetInstance<tsorcRevamp>().GetPacket();
                    shadowPacket.Write((byte)tsorcPacketID.DispelShadow);
                    shadowPacket.Write(target.whoAmI);
                    shadowPacket.Send();
                }
            }
        }

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) {
            if (target.type == ModContent.NPCType<NPCs.Bosses.SuperHardMode.Witchking>()) { 
                damage *= 3;
            }
        }
    }
}
