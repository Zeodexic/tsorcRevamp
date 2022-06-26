using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Magic
{
    public class MagicBarrier : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Barrier");
            Tooltip.SetDefault("A lost tome for artisans\n" +
                                "Casts Barrier on the user, which adds 20 defense for 20 seconds\n" +
                                "\nDoes not stack with Fog, Wall or Shield spells");

        }
        public override void SetDefaults()
        {
            Item.stack = 1;
            Item.width = 34;
            Item.height = 10;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Pink;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.mana = 130;
            Item.UseSound = SoundID.Item21;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.value = PriceByRarity.Pink_5;

        }

        public override void AddRecipes()
        {
            Terraria.Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpellTome, 1);
            recipe.AddIngredient(ItemID.SoulofSight, 10);
            recipe.AddIngredient(ItemID.SoulofLight, 20);
            recipe.AddIngredient(Mod.Find<ModItem>("DarkSoul").Type, 20000);
            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<Buffs.Barrier>(), 1200, false);
            // Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("Barrier"), 0, 0f, player.whoAmI, 0f, 0f);
            return true;
        }
        public override bool CanUseItem(Player player)
        {

            if (player.HasBuff(ModContent.BuffType<Buffs.ShieldCooldown>()))
            {
                return false;
            }

            if (player.HasBuff(ModContent.BuffType<Buffs.Fog>()) || player.HasBuff(ModContent.BuffType<Buffs.Wall>()) || player.HasBuff(ModContent.BuffType<Buffs.Shield>()))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
