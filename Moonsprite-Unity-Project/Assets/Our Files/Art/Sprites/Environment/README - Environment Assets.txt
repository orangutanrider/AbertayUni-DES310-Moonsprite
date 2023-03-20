Standard PPU is 64
PPU stands for pixels per unit, if it isn't set to the correct number, then it won't be to the correct scale.

Create a prefab for the sprites you place.
On that prefab, add a "PositionalYSorter" script component.
If the asset is a building then add colliders to it aswell.

Tile maps
(as in ones that'll be added and placed via a tile palette) 
Have additional rules that are detailed in a README in the tilemaps folder