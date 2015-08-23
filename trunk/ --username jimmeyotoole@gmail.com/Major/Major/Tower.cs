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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Major
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Tower : DrawableGameComponent
    {
        public Vector2 pos;
        public SpriteBatch spriteBatch;
        public Texture2D texture;
        public int Speed;
        public int Damage;
        public int seconds;
        

        public Tower(Vector2 p, Texture2D t, Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            //public static Vector2 pos;
            Vector2 truncated = new Vector2(p.X - p.X% Game1.SCALE, p.Y - p.Y % Game1.SCALE);
            this.pos = truncated;
            this.texture = t;
            this.Speed = 2;
            this.Damage = 10;
            this.seconds = 0;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();

            //Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            base.Update(gameTime);
        }
        /*
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            spriteBatch.Draw(texture, pos, Color.White);

            spriteBatch.End();

            //Draw();
            base.Draw(gameTime);

        }*/
    }
}