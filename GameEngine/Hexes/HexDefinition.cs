using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Hexes
{
    public class HexDefinition
    {
        public HexDefinition(HexSheet sheet, string name, Rectangle rectangle)
        {
            this.Sheet = sheet;
            this.Name = name;
            this.Rectangle = rectangle;
        }

        public HexSheet Sheet { get; private set; }

        public string SheetName
        {
            get { return this.Sheet.Name; }
        }
        
        public string Name { get; private set; }

        public Rectangle Rectangle { get; private set; }

        public virtual void Draw(SpriteBatch spriteBatch, Rectangle destination)
        {
            this.Sheet.Draw(spriteBatch, this, destination);
        }

        public object GetXml()
        {
            return new XElement("HexDefinition",
                new XAttribute("name", this.Name),
                new XAttribute("rectangle", this.Rectangle));
        }
    }
}