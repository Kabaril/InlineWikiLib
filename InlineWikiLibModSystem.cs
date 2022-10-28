using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Combinations.UI;
using Terraria.UI;
using System.Collections.Generic;

namespace Combinations
{
    public sealed class InlineWikiLibModSystem : ModSystem
    {
        public static InlineWikiLibModSystem Instance;
        private ItemWikiState ItemWikiState;
        private UserInterface UserInterface;
        public ModKeybind OpenItemWikiKeybind;

        public override void Load()
        {
            UserInterface = new UserInterface();
            OpenItemWikiKeybind = KeybindLoader.RegisterKeybind(Mod, "Open inline wiki", "P");
            Instance = this;
        }

        public void LoadWikiString(string text)
        {
            ItemWikiState = new ItemWikiState(text);
            ItemWikiState.Activate();
            UserInterface.ResetLasts();
            UserInterface.SetState(ItemWikiState);
            UserInterface.Recalculate();
        }

        public void UnloadWiki()
        {
            UserInterface.SetState(null);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            UserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "Combinations: Inline Wiki",
                    () => { UserInterface.Draw(Main.spriteBatch, new GameTime()); return true; },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
