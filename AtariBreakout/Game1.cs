using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AtariBreakout;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public static int SCREEN_HEIGHT = 768;
    public static int SCREEN_WIDTH = 640;

    Paddle paddle;
    Ball ball;
    SpriteFont font;

    public static int Score, Lives, Highscore;


    //List<Block> blocks;
    List<List<Block>> blocks;

    Texture2D heart;
    bool gameOver;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
        _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
        _graphics.ApplyChanges();

        Score = 0;
        Lives = 3;
        gameOver = false;

        blocks = [new List<Block>(), new List<Block>(), new List<Block>(), new List<Block>()];

        for (int i = 0; i < 4; i++)
        {
            Color color;
            switch (i)
            {
                case 0:
                    color = Color.Blue;
                    break;
                case 1:
                    color = Color.Red;
                    break;
                case 2:
                    color = Color.Yellow;
                    break;
                case 3:
                    color = Color.Green;
                    break;
                default:
                    color = Color.White;
                    break;
            }
            for (int j = 0; j < 8; j++)
            {


                blocks[i].Add(new Block(new Vector2(j * Block.Width * 1.1f, i * Block.Height * 1.25f), _graphics.GraphicsDevice, color));
            }
        }

        paddle = new(new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT - 150), 15, 150, _graphics.GraphicsDevice);
        ball = new(new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT - 170), 450f, 10, 10, paddle, _graphics.GraphicsDevice, blocks);



        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        font = Content.Load<SpriteFont>("Score");
        heart = Content.Load<Texture2D>("heart");

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (!gameOver)
        {
            paddle.Update(gameTime);
            ball.Update(gameTime);

            foreach (var list in blocks)
            {
                foreach (var block in list)
                {
                    block.Update(gameTime);

                }
            }
        }

        if (Lives <= 0)
        {
            gameOver = true;
        }

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        foreach (var list in blocks)
        {
            foreach (var block in list)
            {
                block.Draw(_spriteBatch);
            }
        }

        ball.Draw(_spriteBatch);
        paddle.Draw(_spriteBatch);

        for (int i = 0; i < Lives; i++)
        {
            //_spriteBatch.Draw(heart, new Rectangle(SCREEN_WIDTH - heart.Width - i * heart.Width + 15, SCREEN_HEIGHT - heart.Height, (int)(heart.Width * 0.25f), (int)(heart.Height * 0.25f)), Color.White);
            //_spriteBatch.Draw(heart, new Vector2(SCREEN_WIDTH - heart.Width - i * heart.Width + 15, SCREEN_HEIGHT - heart.Height), new Rectangle(), Color.White, 0f, Vector2.Zero, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
        }

        _spriteBatch.DrawString(font, Score.ToString(), new Vector2(5, SCREEN_HEIGHT - font.MeasureString(Score.ToString()).Y), Color.White);
        _spriteBatch.DrawString(font, Lives.ToString(), new Vector2(SCREEN_WIDTH - font.MeasureString(Lives.ToString()).Y, SCREEN_HEIGHT - font.MeasureString(Score.ToString()).Y), Color.White);

        if (gameOver)
            _spriteBatch.DrawString(font, "Game Over!", new Vector2(SCREEN_WIDTH / 2 - font.MeasureString("Game Over!").X / 2, SCREEN_HEIGHT / 2 - font.MeasureString("Game Over!").Y / 2), Color.IndianRed);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
