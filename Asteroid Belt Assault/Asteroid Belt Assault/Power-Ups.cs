using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Asteroid_Belt_Assault
{
    public class Power_Ups : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Random rand = new Random((int)DateTime.UtcNow.Ticks);

        
        
        Texture2D PlasmabuttonSheet;



        Sprite Plasmabutton;

        public Power_Ups()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            PlasmabuttonSheet = Content.Load<Texture2D>("Textures/PlasmaButton");

            Plasmabutton = new Sprite(new Vector2(-50 + rand.Next(-500, 0), rand.Next(50, 700)),
                                  PlasmabuttonSheet,
                                  new Rectangle(0, 0, 115, 104),
                                  new Vector2(0, -20));
        }


            protected override void Update(GameTime gameTime)
            {
                Plasmabutton.Update(gameTime);

                base.Update(gameTime);
            }

            protected override void  Draw(GameTime gameTime)
            {
                spriteBatch.Begin();

                Plasmabutton.Draw(spriteBatch);

                spriteBatch.End();
                base.Draw(gameTime);
            }


        }

    }

