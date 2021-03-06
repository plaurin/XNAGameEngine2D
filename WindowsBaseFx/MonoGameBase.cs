using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameFramework;
using GameFramework.Inputs;
using GameFramework.Screens;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameImplementation.EngineImplementation;
using Color = Microsoft.Xna.Framework.Color;

namespace MonoGameImplementation
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MonoGameBase : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private readonly GameFramework.GameTimer gameTimer;
        private readonly IScreen screen;
        private readonly GameResourceManager gameResourceManager;

#if WINDOWS
        private readonly GameNavigatorGateway gameNavigator;
#endif

        private SpriteBatch spriteBatch;
        private Texture2D blank;
        private IDictionary<IPreDrawable, RenderTargetWrap> renderTargetDictionary;

        private bool isUpdateEnabled = true;

        public MonoGameBase(IScreen screen)
        {
            this.screen = screen;

            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            this.gameResourceManager = new XnaGameResourceManager(this.Content);

            this.gameTimer = new GameFramework.GameTimer();

#if WINDOWS
            this.gameNavigator = new GameNavigatorGateway();
#endif
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.screen.Initialize(this.GetViewPort());

#if WINDOWS
            this.gameNavigator.Launch(this.screen, this.gameResourceManager, this.Window);
#endif

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.screen.LoadContent(this.gameResourceManager);

            this.blank = new Texture2D(this.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            this.blank.SetData(new[] { Color.White });

            this.renderTargetDictionary = new Dictionary<IPreDrawable, RenderTargetWrap>();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var inputContext = new XnaInputContext();

            if (this.isUpdateEnabled)
            {
                this.gameTimer.Update(gameTime.ElapsedGameTime, gameTime.TotalGameTime);
                this.screen.Update(inputContext, this.gameTimer);
            }

            // Allows the game to exit
            if (this.screen.ShouldExit) this.Exit();

#if WINDOWS
            if (!this.gameNavigator.IsOpen && inputContext.KeyboardGetState().IsKeyDown(KeyboardKeys.F12))
                this.gameNavigator.Show();

            var navigatorMessage = this.gameNavigator.Update(this.gameTimer);
            if (navigatorMessage.ShouldExit) this.Exit();
            this.isUpdateEnabled = navigatorMessage.ShouldPlay;
#endif

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.gameTimer.DrawFrame(gameTime.ElapsedGameTime, gameTime.TotalGameTime);

            var xnaDrawContext = new XnaDrawContext(this.spriteBatch, this.blank, this.graphics.GraphicsDevice.Viewport, 
                this.renderTargetDictionary);

            var drawContext = new DrawContext(xnaDrawContext);

            this.screen.Draw(drawContext);

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        private GameFramework.Viewport GetViewPort()
        {
            return new GameFramework.Viewport(
                this.graphics.GraphicsDevice.Viewport.Width,
                this.graphics.GraphicsDevice.Viewport.Height);
        }
    }
}
