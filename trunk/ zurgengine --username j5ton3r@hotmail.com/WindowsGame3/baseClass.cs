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


namespace WindowsGame3
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
   // public class baseClass : DrawableGameComponent
    public class baseClass
    {
        public Vector2 pos;
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private int speed;
        private int seconds;
        private int damage;
        private int hp;


        //public baseClass(Game game)
          //  : base(game)
        public baseClass()
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public virtual void Initialize()
        {
            // TODO: Add your initialization code here

            //base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            //base.Update(gameTime);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.texture, this.pos, Color.White);
        }

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            this.texture = theContentManager.Load<Texture2D>(theAssetName);
        }


       /* public Vector2 POS
        {
            get
            {
                return currentPos;
            }
            set
            {
                currentPos = value;
            }
        }*/

        public SpriteBatch SB
        {
            get
            {
                return spriteBatch;
            }
            set
            {
                spriteBatch = value;
            }
        }

        public Texture2D TEXTURE
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
            }
        }

        public int SPEED
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }

        }

        public int SECONDS
        {
            get
            {
                return seconds;
            }
            set
            {
                seconds = value;
            }

        }
        public int DAMAGE
        {
            get
            {
                return damage;
            }
            set
            {
                damage = value;
            }

        }
        public int HP
        {
            get
            {
                return hp;
            }
            set
            {
                hp = value;
            }

        }

    }
}