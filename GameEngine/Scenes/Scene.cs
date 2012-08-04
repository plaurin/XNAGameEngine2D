using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Cameras;
using WindowsGame1.Maps;

namespace WindowsGame1.Scenes
{
    public class Scene
    {
        private readonly List<MapBase> maps;

        public Scene()
        {
            this.maps = new List<MapBase>();
        }

        public IEnumerable<MapBase> Maps
        {
            get
            {
                return this.maps;
            }
        }

        public void AddMap(MapBase map)
        {
            this.maps.Add(map);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (var map in this.maps)
            {
                map.Draw(spriteBatch, camera);
            }
        }
    }
}