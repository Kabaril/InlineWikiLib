using Terraria.ID;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace InlineWikiLib
{
    public sealed class InlineWikiLib : Mod
    {
        // Automatically set by tModLoader
        public static InlineWikiLib Instance;

        public override void Load()
        {
            Logger.Info("Initializing InlineWikiLib");
        }

        public override void Unload()
        {
            Logger.Info("Unloading InlineWikiLib");
        }

        public override object Call(params object[] args)
        {
            if(args.Length < 3 || args[0] is not string || args[1] is not string || args[2] is not string) {
                return @"Syntax Error: Invalid Call
Possible Calls:
'AddWikiEntry', 'Mod/Item', 'Wiki String', (optional force_overwrite, default = false)false|true";
            }
            switch(args[0] as string) {
                case "AddWikiEntry":
                    {
                        try
                        {
                            if(ItemID.Search.ContainsName(args[1] as string))
                            {
                                InlineWikiGlobalItem.AddVanillaItemWikiStringToCache(args[1] as string, args[2] as string);
                                return "Success";
                            }
                            ModItem mod_item = ModContent.Find<ModItem>(args[1] as string);
                            string wiki_string = InlineWikiGlobalItem.GetItemWikiStringModItemField(mod_item);
                            if(wiki_string is null)
                            {
                                InlineWikiGlobalItem.AddItemWikiStringToCache(mod_item, args[2] as string);
                                return "Success";
                            }
                            if(args.Length >= 4 && args[3] is bool force_overwrite) {
                                if (force_overwrite)
                                {
                                    InlineWikiGlobalItem.AddItemWikiStringToCache(mod_item, args[2] as string);
                                    return "Success";
                                }
                            }
                            return "Item " + args[1] + " wiki was not added because it already exists and the overwrite option has not been set";
                        } catch (KeyNotFoundException)
                        {
                            return "Could not find item " + args[1];
                        }
                    }
                default:
                    {
                        return "Error: First argument must be one of: 'AddWikiToEntry'";
                    }
            }
        }
    }
}
