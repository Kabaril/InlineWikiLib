# Inline Wiki Lib
Inline Wiki Lib is a Mod for Terraria TModLoader

Inline Wiki Lib handles GUI for Inline Wiki Entries


The easiest way to add support for your Mod is to add a field called 'InlineWikiLibValue' to your ModItem

The field must be public and static for this to work:


> //Example from Combinations Mod
>
> public static string InlineWikiLibValue = @"
>
> \#\# Aglet Of The Wind ![Combinations/Items/AgletOfTheWind/AgletOfTheWind]t4
>
> The Aglet Of The Wind grants 12 % increased movement speed.
>
> The Aglet Of The Wind can be further upgraded into the Lightning Boots, like its parts.
>
> ...
>
> ";


If you need Localisation support you can do the following:


> public static readonly string InlineWikiLibValue;
>
>
> public override void SetStaticDefaults()
> {
>
> 	InlineWikiLibValue = Language.GetTextValue("Mods.YourMod.YourItem.InlineWiki");
>
> }

You can also add a entry with the Mod Call API:


>Mod inlineWiki = ModLoader.GetMod("InlineWikiLib");
>
>//Wood is Terraria Wood Item
>
>inlineWiki?.Call("AddWikiEntry", "Wood", 
>
>@"## Wood ![Terraria/Images/Item_9]
>
>
>Wood is a type of block that is generally obtained by cutting down trees using an axe or a chainsaw.");


This is usefull if you want to add wiki entries for vanilla or other mods.


The content of the wiki string is parsed similar to Markdown.
The following syntax is understood:

\n is interpreted as line break

\# at start of line for large font size

\#\# at start of line for medium-large font size

\#\#\# at start of line for medium font size

\#\#\#\# at start of line for small font size


example:
## Aglet Of The Wind

~0 for color red

~1 for color green

~2 for color blue

~3 for color black

~4 for color yellow

~5 for color violet

~6 for color orange

~7 for color cyan

~8 for color brown

~9 for color pink

example:

~1Green

or

##~1Big Green


![PATH/TO/TEXTURE] to display a texture

![PATH/TO/TEXTURE]t-x to display a texture with x pixel top offset

example:

![Combinations/Items/AgletOfTheWind/AgletOfTheWind]t4

loads the texture with 4 pixel offset from the top (can also be t-4)