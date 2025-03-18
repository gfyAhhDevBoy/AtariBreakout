using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AtariBreakout;

enum GameState
{
    MainMenu,
    Playing,
    Pause,
    GameOver
}

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public static int SCREEN_HEIGHT = 768;
    public static int SCREEN_WIDTH = 640;

    Paddle paddle;
    Ball ball;
    SpriteFont scoreFont, menuFont, titleFont;

    public static int Score, Lives, Highscore;

    GameState gameState;

    //List<Block> blocks;
    List<List<Block>> blocks;

    Texture2D heart;

    List<Button> mainMenuButtons, gameOverButtons;
    Button playButton, replayButton;
    Button exitButton;

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

        gameState = GameState.MainMenu;
        mainMenuButtons = new();
        gameOverButtons = new();

        Score = 0;
        Lives = 3;

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

        scoreFont = Content.Load<SpriteFont>("Score");
        menuFont = Content.Load<SpriteFont>("MenuText");
        titleFont = Content.Load<SpriteFont>("Title");
        heart = Content.Load<Texture2D>("heart");

        playButton = new Button("Play", new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2 + 75), menuFont, Color.White, Color.Gray);
        replayButton = new Button("Replay", new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2 + 75), menuFont, Color.White, Color.Gray);
        exitButton = new Button("Exit", new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2 + 150), menuFont, Color.White, Color.IndianRed);

        mainMenuButtons.Add(playButton);
        mainMenuButtons.Add(exitButton);
        gameOverButtons.Add(exitButton);
        gameOverButtons.Add(replayButton);

        exitButton.OnClickEvent += ExitButton;
        playButton.OnClickEvent += PlayButton;
        replayButton.OnClickEvent += PlayButton;



        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        switch (gameState)
        {
            case GameState.MainMenu:
                foreach (var button in mainMenuButtons)
                {
                    button.Update();
                }
                break;
            case GameState.Playing:
                paddle.Update(gameTime);
                ball.Update(gameTime);
                foreach (var list in blocks)
                {
                    foreach (var block in list)
                    {
                        block.Update(gameTime);
                    }
                }
                if (Lives <= 0)
                    gameState = GameState.GameOver;
                break;
            case GameState.Pause:
                break;
            case GameState.GameOver:
                foreach (var button in gameOverButtons)
                {
                    button.Update();
                }
                break;
        }

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        switch (gameState)
        {
            case GameState.MainMenu:
                foreach (var button in mainMenuButtons)
                {
                    button.Draw(_spriteBatch);
                }
                _spriteBatch.DrawString(titleFont, "BREAKOUT!", new Vector2(SCREEN_WIDTH / 2 - titleFont.MeasureString("BREAKOUT").X / 2, SCREEN_HEIGHT / 2 - titleFont.MeasureString("BREAKOUT").Y / 2 - 100), Color.White);
                break;
            case GameState.Playing:
                foreach (var list in blocks)
                {
                    foreach (var block in list)
                    {
                        block.Draw(_spriteBatch);
                    }
                }
                ball.Draw(_spriteBatch);
                paddle.Draw(_spriteBatch);
                _spriteBatch.DrawString(scoreFont, Score.ToString(), new Vector2(5, SCREEN_HEIGHT - scoreFont.MeasureString(Score.ToString()).Y), Color.White);
                _spriteBatch.DrawString(scoreFont, Lives.ToString(), new Vector2(SCREEN_WIDTH - scoreFont.MeasureString(Lives.ToString()).Y, SCREEN_HEIGHT - scoreFont.MeasureString(Score.ToString()).Y), Color.White);
                break;
            case GameState.Pause:
                break;
            case GameState.GameOver:
                foreach (var button in gameOverButtons)
                {
                    button.Draw(_spriteBatch);
                }
                _spriteBatch.DrawString(titleFont, "Game Over!", new Vector2(SCREEN_WIDTH / 2 - titleFont.MeasureString("Game Over").X / 2, (SCREEN_HEIGHT / 2) - 100 - titleFont.MeasureString("Game Over").Y / 2), Color.IndianRed);
                break;
        }



        _spriteBatch.End();

        base.Draw(gameTime);
    }

    void ExitButton(object sender, EventArgs e)
    {
        Environment.Exit(0);
    }

    void PlayButton(object sender, EventArgs e)
    {
        gameState = GameState.Playing;
    }
}
