﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Asteroid_Belt_Assault
{
    class PlayerManager
    {
        public Sprite playerSprite;
        private float playerSpeed = 160.0f;
        private Rectangle playerAreaLimit;
        private bool ReggaeSharkMode = false;

        public long PlayerScore = 0;
        public int LivesRemaining = 3;
        public bool Destroyed = false;
        public bool isSongPlaying = false;

        private Vector2 gunOffset = new Vector2(25, 10);
        private float shotTimer = 0.0f;
        private float minShotTimer = 0.2f;
        private int playerRadius = 15;
       
        public ShotManager PlayerShotManager;

        private Texture2D texture;

        KeyboardState oldks;

        public PlayerManager(
            Texture2D texture,  
            Rectangle initialFrame,
            int frameCount,
            Rectangle screenBounds)
        {
            this.texture = texture;

            playerSprite = new Sprite(
                new Vector2(500, 500),
                texture,
                initialFrame,
                Vector2.Zero);

            PlayerShotManager = new ShotManager(
                texture,
                new Rectangle(0, 300, 5, 5),
                4,
                2,
                250f,
                screenBounds);
           
            playerAreaLimit =
                new Rectangle(
                    0,
                    screenBounds.Height / 2,
                    screenBounds.Width,
                    screenBounds.Height / 2);

            for (int x = 1; x < frameCount; x++)
            {
                playerSprite.AddFrame(
                    new Rectangle(
                        initialFrame.X + (initialFrame.Width * x),
                        initialFrame.Y,
                        initialFrame.Width,
                        initialFrame.Height));
            }
            playerSprite.CollisionRadius = playerRadius;

            
        }

        private void FireShot()
        {
            if (shotTimer >= minShotTimer)
            {
                Vector2 target = new Vector2((float)Math.Sin((double)playerSprite.Rotation), -(float)Math.Cos((double)playerSprite.Rotation));

                PlayerShotManager.FireShot(
                    playerSprite.Location + gunOffset,
                    target,
                    true);
                shotTimer = 0.0f;
            }
        }

        private void HandleKeyboardInput(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
            {
                playerSprite.Velocity += new Vector2(0, -1);
            }

            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
            {
                playerSprite.Velocity += new Vector2(0, 1);
            }

            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
            {
                playerSprite.Velocity += new Vector2(-1, 0);
            }

            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
            {
                playerSprite.Velocity += new Vector2(1, 0);
            }

            MouseState mx = Mouse.GetState();
            if (mx.LeftButton == ButtonState.Pressed)
            {
                FireShot();
            }

            if (!oldks.IsKeyDown(Keys.R) && keyState.IsKeyDown(Keys.R))
            {
                if (!isSongPlaying)
                {
                    ReggaeSharkMode = true;
                    isSongPlaying = true;
                    MediaPlayer.Play(SoundManager.Reggae);
                    playerSprite.CurrentList = 1;
                    playerSprite.AddFrame(new Rectangle(146, 149, 50, 50));
                }



                
            }

            if (!oldks.IsKeyDown(Keys.T) && keyState.IsKeyDown(Keys.T))
            {
                ReggaeSharkMode = false;
                isSongPlaying = false;
                MediaPlayer.Stop();
                playerSprite.CurrentList = 0;
            }
              
        }

        private void HandleGamepadInput(GamePadState gamePadState)
        {
            playerSprite.Velocity +=
                new Vector2(
                    gamePadState.ThumbSticks.Left.X,
                    -gamePadState.ThumbSticks.Left.Y);

            if (gamePadState.Buttons.A == ButtonState.Pressed)
            {
                FireShot();
            }
        }

        private void imposeMovementLimits()
        {
            Vector2 location = playerSprite.Location;

            if (location.X < playerAreaLimit.X)
                location.X = playerAreaLimit.X;

            if (location.X >
                (playerAreaLimit.Right - playerSprite.Source.Width))
                location.X =
                    (playerAreaLimit.Right - playerSprite.Source.Width);

            if (location.Y < playerAreaLimit.Y)
                location.Y = playerAreaLimit.Y;

            if (location.Y >
                (playerAreaLimit.Bottom - playerSprite.Source.Height))
                location.Y =
                    (playerAreaLimit.Bottom - playerSprite.Source.Height);

            playerSprite.Location = location;
        }

        public void Update(GameTime gameTime)
        {
            PlayerShotManager.Update(gameTime);

            if (!Destroyed)
            {
                playerSprite.Velocity = Vector2.Zero;

                shotTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                HandleKeyboardInput(Keyboard.GetState());
                HandleGamepadInput(GamePad.GetState(PlayerIndex.One));

                playerSprite.Velocity.Normalize();
                playerSprite.Velocity *= playerSpeed;

                playerSprite.Update(gameTime);
                imposeMovementLimits();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerShotManager.Draw(spriteBatch);

            if (!Destroyed)
            {
                playerSprite.Draw(spriteBatch);
            }
        }

    }
}
