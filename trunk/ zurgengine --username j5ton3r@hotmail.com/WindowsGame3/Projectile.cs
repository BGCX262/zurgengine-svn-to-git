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
    public class Projectile : baseClass
    {
        private Creep c;

       // public projectile(Game game)
         //   : base(game)
        public Projectile()
        {
            // TODO: Construct any child components here
        }

        public Creep CREEP
        {
            get
            {
                return c;
            }
            set
            {
                c = value;
            }
        }
        //This function damages the creep if the projectiles current position is close to that of the creeps
        public bool damageCreep()
        {
            /*
            //System.Diagnostics.Debug.WriteLine("WTF!!!");
            //System.Diagnostics.Debug.WriteLine(this.pos);
            //System.Diagnostics.Debug.WriteLine(creep.pos);
            //if (this.pos.Equals(creep.pos))
            if ((Math.Abs(this.POS.X - c.POS.X) <= 4) && (Math.Abs(this.POS.Y - c.POS.Y) <= 4))
            {
                c.HP -= this.DAMAGE;
                if (c.HP <= 0)
                {
                    c.ISDEAD = true;
                    System.Diagnostics.Debug.WriteLine("DEAD!!!");
                }
                System.Diagnostics.Debug.WriteLine("HURT!!!");
                System.Diagnostics.Debug.WriteLine(c.HP);
            }*/

            return false;
        }

        //This function moves the projectile towards the creeps current position
        public bool moveProj()
        {
           /* if ((this.POS.X - c.POS.X) > 0)
                this.POS.X -= this.SPEED;
            else if ((this.POS.X - c.POS.X) < 0)
                this.POS.X += this.SPEED;
            if ((this.POS.Y - c.POS.Y) > 0)
                this.POS.Y -= this.SPEED;
            else if ((this.POS.Y - c.POS.Y) < 0)
                this.POS.Y += this.SPEED;

            if ((this.POS.Y == c.POS.Y) && (this.POS.X == c.POS.X))
            {
                //Projs.Remove(p);
            }

            if (c.ISDEAD)
            {
                //Projs.Remove(p);
            }*/


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