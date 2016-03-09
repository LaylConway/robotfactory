using System;
using UnityEngine;

namespace RobotFactory.Model
{
    public class Factory
    {
        private readonly Tile[,] _tiles;
        private readonly int _width, _height;

        public Factory(int width, int height)
        {
            _width = width;
            _height = height;
            _tiles = new Tile[_width, _height];

            Debug.Log(string.Format("Generating [{0}, {1}] world...", _width, _height));

            // Generate the map
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    _tiles[x, y] = new Tile
                    {
                        Type = TileType.Grass
                    };
                }
            }

            Debug.Log("Finished world generation");
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public event EventHandler<TileChangedEventArgs> TileChanged;

        public Tile GetTileAt(Vector2I pos)
        {
            return _tiles[pos.x, pos.y];
        }

        public void SetTileAt(Vector2I pos, Tile tile)
        {
            if (pos.x > _width || pos.y > _height)
            {
                throw new IndexOutOfRangeException();
            }

            // Actually perform the set
            _tiles[pos.x, pos.y] = tile;

            // Notify listeners of the change
            TileChanged.SafeInvoke(this, new TileChangedEventArgs(pos));
        }
    }

    public class TileChangedEventArgs : EventArgs
    {
        public Vector2I Position { get; private set; }

        public TileChangedEventArgs(Vector2I pos)
        {
            Position = pos;
        }
    }
}