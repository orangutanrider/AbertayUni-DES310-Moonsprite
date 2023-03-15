This folder is for tilesets, as in files that will be placed into unity, via a grid+tilemap combo.

When importing a tileset, click on it, and in the inspector set the filtering setting to "Point, no filter"
Then set the sprite mode to multiple.
Then go into the sprite editor, by clicking the sprite editor button.
In that window, click slice and set the type to grid by cell size.
The standard resoloution for most tiles is 64 by 64 px, set the pixel size values to those (unless this file doesn't follow that standard size)
Exit the window and apply the changes.

Set the PPU to 64 (if the file is following the standard tile size).

Next, add the file to the "TilePaletteAtlas".
Do this by clicking on that file (it's in this folder)
Then in the inspector there is a "Objects for Packing" list of items
At the bottom of that list, click the plus and then select the asset you're adding.

Finally, you can now add it to a tile palette
Either add it to an existing one (by simply dragging it into that window) or create a new tile palette
To create a new tile palette, right click and then Create>2D>TilePalette>Rectangular
BUT WAIT
These tile files should be created in


Why is this needed? As in, why do we need a sprite atlas?
Answer: 
	The sprite atlas solves the issue of seam flickering.
	https://www.youtube.com/watch?v=Wf98KrAyB2I
	This is where, when moving around, you'll sometimes get lines flashing it where tiles meet.