using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomb_Finder
{
    public class Tilemap
    {
        public Tile[,] gameMap;

        private Texture2D Tileset;

        public Vector2 Position { get; set; }

        public int TilesFlagged { get; private set; } = 0;
        public int TilesOpened { get; private set; } = 0;
        public int TotalTiles { get; private set; } = 0;
        public int ClosedTilesLeft { get; private set; } = 0;

        public int TotalBombs { get; private set; } = 0;
        public int BombsLeftUnflagged { get; private set; } = 0;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }


        private List<Tile> openNextUpdate = new List<Tile>();


        public Tilemap(Vector2 position, Point size, int bombCount) 
        {
            Position = position;

            gameMap = new Tile[size.X, size.Y];
            Generate();
        }
        public Tilemap(Vector2 position, int tileWidth, int tileHeight, int bombCount)
        {
            Position = position;

            TotalBombs = bombCount;
            BombsLeftUnflagged = bombCount;

            gameMap = new Tile[tileWidth, tileHeight];
            Generate();
        }


        public Texture2D GetTileset() { return Tileset; }
        public void SetTileset(Texture2D tileset) 
        { 
            Tileset = tileset;

            foreach (Tile tile in gameMap)
            {
                tile.Texture = tileset;
              
            }
        }

        void Generate()
        {
            Texture2D tileset = Globals.Content.Load<Texture2D>("Textures/Tiles");

            for(int x = 0; x < gameMap.GetLength(0); x++)
            {
                for (int y = 0; y < gameMap.GetLength(1); y++)
                {
                    gameMap[x, y] = new Tile(tileset, Position + new Vector2(x, y) * 32, new Rectangle(0, 0, 32, 32), false);
                    gameMap[x, y].TilePosition = new Point(x, y);

                    TotalTiles++;
                    ClosedTilesLeft++;
                }
            } 

            Width = gameMap.GetLength(0) * 32;
            Height = gameMap.GetLength(1) * 32;

            TileWidth = gameMap.GetLength(0);
            TileHeight = gameMap.GetLength(1);
        }

        

        public Tile[] GetTileNeighbors(Point tile, bool includeCorners = true)
        {
            List<Tile> tiles = new List<Tile>();

            if (tile.Y - 1 >= 0)
                tiles.Add(gameMap[tile.X, tile.Y - 1]);

            if (tile.X + 1 < TileWidth)
                tiles.Add(gameMap[tile.X + 1, tile.Y]);

            if (tile.Y + 1 < TileHeight)
                tiles.Add(gameMap[tile.X, tile.Y + 1]);

            if (tile.X - 1 >= 0)
                tiles.Add(gameMap[tile.X - 1, tile.Y]);


            if (includeCorners)
            {
                if (tile.Y - 1 >= 0 && tile.X + 1 < TileWidth)
                    tiles.Add(gameMap[tile.X + 1, tile.Y - 1]);

                if (tile.Y + 1 < TileHeight && tile.X + 1 < TileWidth)
                    tiles.Add(gameMap[tile.X + 1, tile.Y + 1]);

                if (tile.Y + 1 < TileHeight && tile.X - 1 >= 0)
                    tiles.Add(gameMap[tile.X - 1, tile.Y + 1]);

                if (tile.Y - 1 >= 0 && tile.X - 1 >= 0)
                    tiles.Add(gameMap[tile.X - 1, tile.Y - 1]);
            }

            return tiles.ToArray();
        }



        public void Update()
        {
            Tile hoveredTile = null;

            foreach (Tile tile in gameMap)
            {
                if (tile.GetBounds().Contains(Globals.MouseState.Position))
                {
                    hoveredTile = tile;
                    break;
                }
            }

            if (hoveredTile == null)
                return;

            if (!hoveredTile.isCovered)
            {
                if ((Globals.MouseState.LeftButton == ButtonState.Released && Globals.OldMouseState.LeftButton == ButtonState.Pressed &&
                    Globals.MouseState.RightButton == ButtonState.Released && Globals.OldMouseState.RightButton == ButtonState.Pressed) ||
                    Globals.MouseState.MiddleButton == ButtonState.Released && Globals.OldMouseState.MiddleButton == ButtonState.Pressed)
                {
                    Tile[] flaggedNeighbors = GetTileNeighbors(hoveredTile.TilePosition).Where(T => T.isFlagged).ToArray();
                    Tile[] unflaggedNeighbors = GetTileNeighbors(hoveredTile.TilePosition).Where(T => !T.isFlagged).ToArray();

                    if (hoveredTile.surroundingBombs == flaggedNeighbors.Length)
                    {
                        foreach (Tile tile in unflaggedNeighbors)
                        {
                            if (tile.isCovered)
                            {
                                if(tile.surroundingBombs > 0)
                                    tile.Open(); // Just open without logic
                                else
                                    OpenTile(tile.TilePosition); // Open with logic checking surrounding tiles and how many bombs there are around those
                            }
                                
                        }
                    }
                }

                    return;
            }


            if (Globals.MouseState.LeftButton == ButtonState.Released && Globals.OldMouseState.LeftButton == ButtonState.Pressed)
            {

                // First tile opened
                if (TilesOpened == 0)
                {
                    List<Point> positionsToExclude = new List<Point> { hoveredTile.TilePosition };
                    foreach (Tile tile in GetTileNeighbors(hoveredTile.TilePosition))
                    {
                        positionsToExclude.Add(tile.TilePosition);
                    }

                    PlaceBombs(TotalBombs, positionsToExclude);

                    /*
                    foreach (Point position in positionsToExclude)
                    {
                        if(position != hoveredTile.TilePosition)
                        {
                            if (gameMap[position.X, position.Y].surroundingBombs > 0)
                                gameMap[position.X, position.Y].Open();
                        }
                    }
                    */
                }

                OpenTile(hoveredTile.TilePosition);
            }
                

            if (Globals.MouseState.RightButton == ButtonState.Released && Globals.OldMouseState.RightButton == ButtonState.Pressed)
            {
                // If the tile is currently flagged
                if (hoveredTile.isFlagged)
                {
                    //Then we unflag it
                    TilesFlagged--;
                    
                    if(hoveredTile.isBomb)
                        BombsLeftUnflagged++;
                }
                else
                {
                    // Otherwise we flag it
                    TilesFlagged++;

                    if (hoveredTile.isBomb)
                        BombsLeftUnflagged--;
                }

                hoveredTile.ToggleFlag();
            }
        }


        public void PlaceBombs(int amount, List<Point> excludedPositions)
        {
            Tile randomTile;

            for (int i = 0; i < amount; i++)
            {
                randomTile = GetRandomTile(excludedPositions.ToArray());
                randomTile.isBomb = true;
                excludedPositions.Add(randomTile.TilePosition);

                // Notify tiles around that this tile is a bomb
                foreach (Tile tile in GetTileNeighbors(randomTile.TilePosition))
                {
                    tile.surroundingBombs++;
                }
            }
        }

        public Tile GetRandomTile(Point[] excludedPositions)
        {
            Tile selectedTile;

            do
            {
                selectedTile = gameMap[Globals.Random.Next(TileWidth - 1), Globals.Random.Next(TileHeight - 1)];
            }
            while (excludedPositions.Contains(selectedTile.TilePosition) || selectedTile.isBomb);

            return selectedTile;
        }


        public void OpenTile(Point position)
        {
            Tile currentTile = gameMap[position.X, position.Y];

            List<Tile> coveredNeighborTiles = GetTileNeighbors(currentTile.TilePosition).Where(T => T.isCovered).ToList();
            //coveredNeighborTiles.Add(currentTile);


            if (currentTile.isBomb)
                return;


            currentTile.Open();
            TilesOpened++;
            ClosedTilesLeft--;

            foreach (Tile tile in coveredNeighborTiles)
            {
                if (!tile.isBomb)
                {
                    if(tile.surroundingBombs == 0)
                    {
                        OpenTile(tile.TilePosition);

                        foreach (Tile tileAroundTile in GetTileNeighbors(tile.TilePosition).Where(T => T.surroundingBombs > 0))
                        {
                            tileAroundTile.Open();
                            TilesOpened++;
                            ClosedTilesLeft--;
                        }
                    }   
                    else
                        tile.Open();

                    TilesOpened++;
                    ClosedTilesLeft--;
                }
            }
        }


        public void Draw()
        {
            foreach(Tile tile in gameMap)
            {
                tile.Draw();
            }
        }
    }
}
