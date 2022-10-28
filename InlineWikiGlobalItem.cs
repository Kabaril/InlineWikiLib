using Combinations;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace InlineWikiLib
{
    public sealed class InlineWikiGlobalItem : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string wiki_string = GetItemWikiString(item);
            if (wiki_string is not null) {
                string text = Language.GetTextValue("Mods.InlineWikiLib.Click", GetKeyBindString(InlineWikiLibModSystem.Instance.OpenItemWikiKeybind));
                tooltips.Add(new TooltipLine(Mod, "InlineWikiLib:Wiki", text)
                {
                    OverrideColor = Color.Green
                });
                if (InlineWikiLibModSystem.Instance.OpenItemWikiKeybind.JustPressed)
                {
                    InlineWikiLibModSystem.Instance.LoadWikiString(wiki_string);
                }
            }
            base.ModifyTooltips(item, tooltips);
        }

        private string GetKeyBindString(ModKeybind keybind)
        {
            if (Main.dedServ || keybind == null)
            {
                return null;
            }

            List<string> assignedKeys = keybind.GetAssignedKeys();
            if (assignedKeys.Count == 0)
            {
                return "[NONE]";
            }

            return string.Join('+', assignedKeys);
        }

        public override void Load()
        {
            WikiStringCache = new Dictionary<string, string>();
        }

        public override void Unload()
        {
            WikiStringCache = null;
        }

        private static Dictionary<string, string> WikiStringCache;

        internal static string GetItemWikiStringModItemField(ModItem mod_item)
        {
            FieldInfo field_info = mod_item.GetType().GetField("InlineWikiLibValue", BindingFlags.Public | BindingFlags.Static);
            if (field_info is null)
            {
                return null;
            }
            object wiki_value = field_info.GetValue(mod_item);
            if (wiki_value is null || wiki_value is not string)
            {
                return null;
            }
            return wiki_value as string;
        }

        internal static void AddItemWikiStringToCache(ModItem mod_item, string wiki_value)
        {
            if (WikiStringCache.ContainsKey(mod_item.FullName))
            {
                WikiStringCache.Remove(mod_item.FullName);
            }
            WikiStringCache.Add(mod_item.FullName, wiki_value);
        }

        internal static void AddVanillaItemWikiStringToCache(string name, string wiki_value)
        {
            string full_vanilla_name = "Terraria/" + name;
            if (WikiStringCache.ContainsKey(full_vanilla_name))
            {
                WikiStringCache.Remove(full_vanilla_name);
            }
            WikiStringCache.Add(full_vanilla_name, wiki_value);
        }

        private static string GetItemWikiString(Item item)
        {
            ModItem mod_item = item.ModItem;
            if (mod_item is not null)
            {
                if (WikiStringCache.ContainsKey(mod_item.FullName))
                {
                    return WikiStringCache[mod_item.FullName];
                }
                string wiki_value = GetItemWikiStringModItemField(mod_item);
                if(wiki_value is null)
                {
                    return null;
                }
                WikiStringCache.Add(mod_item.FullName, wiki_value);
                return WikiStringCache[mod_item.FullName];
            } else
            {
                string full_vanilla_name = "Terraria/" + item.Name;
                if (WikiStringCache.ContainsKey(full_vanilla_name))
                {
                    return WikiStringCache[full_vanilla_name];
                }
            }

            return null;
        }
    }
}
