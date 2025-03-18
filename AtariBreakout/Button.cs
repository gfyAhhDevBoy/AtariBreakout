using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AtariBreakout;

class Button
{
    public event EventHandler OnClickEvent;

    public Vector2 Position;
    public string Text;
    private SpriteFont font;

    Color color;
    Color hoverColor;

    MouseState prevMouseState;

    bool hovered;

    public Rectangle Rect
    {
        get
        {
            return new Rectangle((int)Position.X - (int)(font.MeasureString(Text).X / 2), (int)Position.Y - (int)(font.MeasureString(Text).Y / 2), (int)font.MeasureString(Text).X, (int)font.MeasureString(Text).Y);
        }
    }

    private Rectangle mouseRect;

    public Button(string text, Vector2 position, SpriteFont font, Color color, Color hoverColor)
    {
        Text = text;
        Position = position;
        this.font = font;
        this.color = color;
        this.hoverColor = hoverColor;
        hovered = false;

        prevMouseState = Mouse.GetState();
    }

    public void Update()
    {
        mouseRect = new Rectangle((int)Mouse.GetState().Position.X, (int)Mouse.GetState().Position.Y, 1, 1);
        MouseState currentMouseState = Mouse.GetState();

        if (mouseRect.Intersects(Rect))
        {
            hovered = true;
            if (currentMouseState.LeftButton == ButtonState.Pressed && !(prevMouseState.LeftButton == ButtonState.Pressed))
            {
                OnClickEvent?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            hovered = false;
        }

        prevMouseState = currentMouseState;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(font, Text, new Vector2(Position.X - font.MeasureString("Text").X / 2, Position.Y - font.MeasureString("Text").Y / 2), hovered ? hoverColor : color);
    }
}