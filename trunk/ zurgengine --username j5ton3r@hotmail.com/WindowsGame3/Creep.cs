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
    public class Creep : baseClass
    {
        private bool isDead;
        private int creepMoveCount;

        //public creep(Game game)
            //: base(game)
        public Creep()
        {
            // TODO: Construct any child components here
            this.isDead = false;
            this.creepMoveCount = 0;
        }

        public bool ISDEAD
        {
            get
            {
                return isDead;
            }
            set
            {
                isDead = value;
            }
        }

        public int MOVE
        {
            get
            {
                return creepMoveCount;
            }
            set
            {
                creepMoveCount = value;
            }
        }

        public bool moveCreep()
        {
            return false;
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
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
    }
}