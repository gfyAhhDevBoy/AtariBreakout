using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace AtariBreakout;

class Ball
{
    private Vector2 position;
    private Vector2 originalPosition;
    private Vector2 movement;
    private float originalSpeed;
    private Texture2D texture;
    private int width, height;
    Paddle paddle;

    bool started;

    List<List<Block>> blocks;

    public Rectangle Rect
    {
        get
        {
            return new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width, height);
        }
    }

    KeyboardState prevkb;
    Random random;

    public Ball(Vector2 pos, float speed, int width, int height, Paddle paddle, GraphicsDevice graphicsDevice, List<List<Block>> blocks)
    {
        position = pos;
        originalPosition = pos;
        this.width = width;
        this.height = height;
        this.paddle = paddle;
        movement.Y = -speed;
        originalSpeed = speed;
        this.blocks = new();
        this.blocks = blocks;
        started = false;

        random = new Random();
        int rand = random.Next(1, 11);
        if (rand <= 5)
        {
            movement.X = -speed;
        }
        else
        {
            movement.X = speed;
        }

        texture = new Texture2D(graphicsDevice, 1, 1);
        texture.SetData([Color.White]);

        prevkb = Keyboard.GetState();
    }

    public void Update(GameTime gameTime)
    {
        if (!started && Keyboard.GetState().IsKeyDown(Keys.Space) && !prevkb.IsKeyDown(Keys.Space))
        {
            started = true;
            movement = -Vector2.UnitY * originalSpeed;
        }


        if (started)
        {
            position += movement * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (position.X < 0)
            {
                position.X = 0;
                movement.X *= -1;
            }
            if (position.X > Game1.SCREEN_WIDTH)
            {
                position.X = Game1.SCREEN_WIDTH;
                movement.X *= -1;
            }
            if (position.Y < 0)
            {
                position.Y = 0;
                movement.Y *= -1;
            }
            if (position.Y > Game1.SCREEN_HEIGHT)
            {
                Reset();
                Game1.Lives--;
            }

            if (Rect.Intersects(paddle.Rect))
            {
                movement.Y *= -1;
                movement.X = (position.X - paddle.Position.X) / (paddle.Width / 2) * -movement.Y;
            }

            List<Block> killList = new();
            foreach (var list in blocks)
            {
                foreach (var block in list)
                {
                    if (Rect.Intersects(block.Rect))
                    {
                        movement.Y *= -1.02f;
                        killList.Add(block);
                        Game1.Score++;
                    }
                }
            }

            foreach (var block in killList)
            {
                foreach (var list in blocks)
                {
                    if (list.Contains(block))
                    {
                        list.Remove(block);
                    }
                }
            }
        }
        else
        {
            position.X = paddle.Position.X;
        }

    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, Rect, Color.White);
    }

    public void Reset()
    {
        started = false;
        int rand = random.Next(1, 11);
        if (rand <= 5)
        {
            movement.X = -originalSpeed;
        }
        else
        {
            movement.X = originalSpeed;
        }
        movement.Y = -originalSpeed;
        position = originalPosition;
    }
}