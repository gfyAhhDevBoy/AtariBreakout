using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AtariBreakout;

class Paddle
{
    public Vector2 Position;
    public int Height, Width;
    public Texture2D Texture;
    public float MoveSpeed = 500.0f;

    public Rectangle Rect
    {
        get { return new Rectangle((int)Position.X - Width / 2, (int)Position.Y - Height / 2, Width, Height); }
    }

    public Paddle(Vector2 position, int height, int width, GraphicsDevice graphicsDevice)
    {
        Position = position;
        Height = height;
        Width = width;

        Texture = new Texture2D(graphicsDevice, 1, 1);
        Texture.SetData([Color.CornflowerBlue]);
    }

    public void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Left) && Position.X - Width / 2 >= 0)
        {
            Position.X -= MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Right) && Position.X + Width / 2 <= Game1.SCREEN_WIDTH)
        {
            Position.X += MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Rect, Color.White);
    }
}