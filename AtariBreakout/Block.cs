using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtariBreakout;

class Block
{
    public static readonly int Width = 75;
    public static readonly int Height = 20;
    public Vector2 Position;
    public Texture2D texture;
    public Rectangle Rect
    {
        get
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }
    }

    public Block(Vector2 position, GraphicsDevice graphicsDevice, Color color)
    {
        Position = position;

        texture = new(graphicsDevice, 1, 1);
        texture.SetData([color]);

    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, Rect, Color.White);
    }
}