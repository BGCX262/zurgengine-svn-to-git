using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Major;
using Microsoft.Xna.Framework.Graphics;

namespace Major
{
    public class User : GameComponent
    {
        //public Camera camera;

        public Random RandomClass = new Random();
        public float timer = 0.0f;
        public float interval = 250.0f;
        public bool allowedToClick = true;
        bool mPressed = false;
        bool isTower = true;
        bool isTile = false;
        public KeyboardState newState;
        public KeyboardState oldState;
        public int count = 0;//for making random tiles
        //public Tile tile;
        //public Creep cr;
        public int creepMoveCount = 0;
        public int seconds = 3;
        public int backWidth = 16;// same as graphics.PreferredBackBufferWidth
        public int backHeight = 12;// same as graphics.PreferredBackBufferHeight

        public int spawnSeconds = 5;
        public Vector2 vec;

       
        public User(Game1 game)
            : base(game)
        {
            //this.camera = camera;
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                // Do Something
                allowedToClick = true;
                timer = 0f;
            }
            //camera.Update(dt);
            HandleInput(dt,gameTime);
            base.Update(gameTime);
        }

        /*
        //making a random path at the start of the game for creeps to run down
        public void drawRandomTiles(Game1 game)
        {
            
            int var = RandomNumber(0, backWidth);//same as backbufferedwidth
            //System.Diagnostics.Debug.WriteLine(Game1.SCALE);
            Vector2 tempPos;
            //Creep cr;
            for (int y = 0; y < backHeight; y++)
            {
                tempPos = new Vector2(var*Game1.SCALE ,y*Game1.SCALE);
                //game.Path.Add(tempPos);
                this.tile = new Tile(tempPos, game.texTile, game);
                game.Tiles.Add(this.tile);

                //cr = new Creep(tempPos, game.texCreep, game);
                //game.Creeps.Add(cr);
                //System.Diagnostics.Debug.WriteLine("start");
                //System.Diagnostics.Debug.WriteLine(var);
                //System.Diagnostics.Debug.WriteLine(y);
            }

        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public void moveCreep(GameTime gameTime, Game1 game)
        {
            if (creepMoveCount < backHeight)
            {
                if (gameTime.TotalGameTime.TotalSeconds > seconds)
                {
                    //Getting Vector2 of path
                    int x = (int)game.Path.ElementAt(creepMoveCount).X;
                    int y = (int)game.Path.ElementAt(creepMoveCount).Y;
                    creepMoveCount++;//Kinda of an iterator to know where abouts in the path we are
                    Vector2 temp;
                    seconds = (int)(gameTime.TotalGameTime.TotalSeconds + game.Creeps[0].Speed);
                    temp = new Vector2(x, y);
                    for (int z = 0; z < game.Creeps.Count; z++)
                    {
                        game.Creeps[z].setPos(temp);
                    }
                }
            }
        }

        public void findClosestCreep(GameTime gameTime, Tower tower, Game1 game)
        {
            //search the path and choose the closest creep to the towers position
            int xDiff = 1000;
            int yDiff = 1000;
            int tempX = 0;
            int tempY = 0;
            int crPos = 0;
            if (gameTime.TotalGameTime.TotalSeconds > tower.seconds)
            {

                for (int x = 0; x < game.Creeps.Count; x++)
                {
                    tempX = Math.Abs((int)(tower.pos.X - game.Creeps.ElementAt(x).pos.X));
                    tempY = Math.Abs((int)(tower.pos.Y - game.Creeps.ElementAt(x).pos.Y));
                    if ((tempX <= xDiff) && (tempY <= yDiff))
                    {
                        xDiff = tempX;
                        yDiff = tempY;
                        crPos = x;
                        System.Diagnostics.Debug.WriteLine("JONO!");
                        System.Diagnostics.Debug.WriteLine(tempX);
                        System.Diagnostics.Debug.WriteLine(tempY);
                    }

                }
                tower.seconds = (int)(gameTime.TotalGameTime.TotalSeconds + tower.Speed);
                Shoot(game.Creeps.ElementAt(crPos), game, tower);
                
            }
            
    }
        /*
        public void Shoot(Creep cr,Game1 game,Tower tower)
        {
            System.Diagnostics.Debug.WriteLine("HERE!");
            Projectile p = new Projectile(tower.pos, game.texProj, game);
            //p.Damage = this.Damage;
            //p.Speed = this.Speed;
            p.Damage = 5;
            p.Speed = 2;
            p.setCreep(cr);
            game.Projs.Add(p);
        }

        public void spawnCreep(GameTime gameTime, Vector2 vec, Game1 game)
        {
            if (gameTime.TotalGameTime.TotalSeconds > spawnSeconds)
            {
                Creep cr = new Creep(vec, game.texCreep, game);
                game.Creeps.Add(cr);
                spawnSeconds = (int)(gameTime.TotalGameTime.TotalSeconds + 5);
            }
        }
        */
        public void HandleInput(float dt, GameTime gameTime)
        {

            Game1 game = Game as Game1;
            /*
            if (game.Creeps.Count > 0)
            {
                moveCreep(gameTime, game);
                for (int x=0;x<game.Towers.Count;x++)
                {
                    findClosestCreep(gameTime,game.Towers.ElementAt(x), game);
                }
            }
            if (this.count < 1)
            {
                drawRandomTiles(game);
                this.count++;
                //create practuse creep at start of path
                

            }
            vec = new Vector2(game.Path.ElementAt(0).X, game.Path.ElementAt(0).Y);
            spawnCreep(gameTime, vec, game);
            */
            KeyboardState keyboardState = game.keyboardState;
            MouseState mouseState = game.mouseState;

            float xDifference = mouseState.X - 200;
            float yDifference = mouseState.Y - 200;


            
            //Mouse.SetPosition(200, 200);

            #region 1-5 key binds

            //Ball Selected
            if (keyboardState.IsKeyDown(Keys.D1))
            {
                game.towerType = 1;
            }
            //Handgun Selected
            if (keyboardState.IsKeyDown(Keys.D2))
            {
                game.towerType = 2;
            }

            if (mouseState.X < 50)// && mouseState.Y
            {
                game.camPos = new Vector3(game.camPos.X - 10, game.camPos.Y, game.camPos.Z);
                //Game.camLook = new Vector3(0, 0, 0);
                //Console.WriteLine("shit!");

            }
            //Console.WriteLine(mouseState.X);

            #endregion 


            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (allowedToClick)
                {
                    mPressed = true;
                }

            }

            newState = Keyboard.GetState();
            //pressed = KeypressTest(Keys.LeftAlt);
            //if left alt key pressed
             if (newState.IsKeyUp(Keys.LeftAlt) && oldState.IsKeyDown(Keys.LeftAlt))
             {
                 if (isTower)
                 {
                     isTower = false;
                     isTile = true;
                 }
                 else if (isTile)
                 {
                     isTile = false;
                     isTower = true;
                 }
             }
             oldState = newState;
            
            Vector2 tempMouse;
            Tower tower;
            //Tile tile ;
            bool valid = false;
            int count = 0;
            if (mouseState.LeftButton == ButtonState.Released)
            {
                if (mPressed == true)
                {
                    if (isTower || isTile)
                    {
                        //Box toAdd = new Box(camera.position, 1, 1, 1, 1);
                        mPressed = false;
                        tempMouse = new Vector2(mouseState.X - mouseState.X % Game1.SCALE, mouseState.Y - mouseState.Y % Game1.SCALE);

                        foreach (Tower t in game.Towers)
                        {
                            if (t.pos.Equals(tempMouse))
                                valid = true;
                            count++;

                        }
                        /*
                        if (!valid)
                        {
                            foreach (Tile tt in game.Tiles)
                            {
                                if (tt.pos.Equals(tempMouse))
                                    valid = true;
                                count++;

                            }
                        }*/
                        //game.test = new Tower(new Vector2(mouseState.X, mouseState.Y), game.tex, game);

                        //TODO
                        //check if tower not already in same place / if NOT on path
                        if (!valid)
                        {
                            if (isTower)
                            {
                                tower = new Tower(new Vector3 (tempMouse.X,35,tempMouse.Y), game.texTower, game);
                                if (game.money >= 50)
                                {
                                    game.Towers.Add(tower);
                                    game.money -= 50;
                                }
                            }
                            /*
                            if (isTile)
                            {
                                this.tile = new Tile(tempMouse, game.texTile, game);
                                if (game.money >= 50)
                                {
                                    game.Tiles.Add(this.tile);
                                    game.money -= 50;
                                }
                            }*/
                            
                        }
                        allowedToClick = false;
                        timer = 0f;
                    }

                }
            }


                //Scoot the camera around depending on what keys are pressed.

                /*
                if (keyboardState.IsKeyDown(Keys.W))
                    camera.MoveForward(dt);
                if (keyboardState.IsKeyDown(Keys.S))
                    camera.MoveForward(-dt);
                if (keyboardState.IsKeyDown(Keys.D))
                    camera.MoveRight(dt);
                if (keyboardState.IsKeyDown(Keys.A))
                    camera.MoveRight(-dt);
                if (keyboardState.IsKeyDown(Keys.Space))
                    camera.MoveUp(dt);
                if (keyboardState.IsKeyDown(Keys.C))
                    camera.MoveUp(-dt);
                */

            if (keyboardState.IsKeyDown(Keys.A))
                game.camPos = new Vector3(game.camPos.X - 10, game.camPos.Y, game.camPos.Z);

            
        }

    }
}

