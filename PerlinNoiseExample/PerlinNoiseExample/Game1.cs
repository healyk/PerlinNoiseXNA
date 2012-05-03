using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PerlinNoise;
using PerlinNoise.Transformers;
using PerlinNoise.Filters;

namespace PerlinNoiseExample {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        NoiseField<float> perlinNoise;
        Texture2D noiseTexture;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            base.Initialize();
            graphics.PreferredBackBufferWidth = 512;
            graphics.PreferredBackBufferHeight = 512;
            graphics.ApplyChanges();
        }

        public void GenerateNoiseTexture() {
            PerlinNoiseGenerator gen = new PerlinNoiseGenerator();
            gen.OctaveCount = 7;
            gen.Persistence = .55f;
            gen.Interpolation = InterpolationAlgorithms.CosineInterpolation;

            perlinNoise = gen.GeneratePerlinNoise(512, 512);
            
            CustomGradientColorFilter filter = new CustomGradientColorFilter();
            Texture2DTransformer transformer = new Texture2DTransformer(graphics.GraphicsDevice);

            filter.AddColorPoint(0.0f, 0.40f, Color.Blue);
            filter.AddColorPoint(0.4f, 0.50f, Color.Yellow);
            filter.AddColorPoint(0.50f, 0.70f, Color.Green);
            filter.AddColorPoint(0.70f, 0.90f, Color.SaddleBrown);
            filter.AddColorPoint(0.90f, 1.00f, Color.White);

            noiseTexture = transformer.Transform(filter.Filter(perlinNoise));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GenerateNoiseTexture();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        bool generated = false;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if(Keyboard.GetState().IsKeyDown(Keys.R) && !generated) {
                GenerateNoiseTexture();
                generated = true;
            } else if(Keyboard.GetState().IsKeyUp(Keys.R)) {
                generated = false;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(noiseTexture, new Vector2(0, 0), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
