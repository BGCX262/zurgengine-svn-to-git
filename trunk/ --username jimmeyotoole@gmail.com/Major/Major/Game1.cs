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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont comicSansMS;
        GraphicsDevice device;
        Effect effect;
 

        public User user;

        public Terrain;

        public Tower test;
        public Texture2D texTower;
        public Texture2D texTile;
        public Texture2D texCreep;
        public Texture2D texProj;

        public List<Tower> Towers = new List<Tower>();
        public List<Tile> Tiles = new List<Tile>();
        public List<Creep> Creeps = new List<Creep>();
        public List<Projectile> Projs = new List<Projectile>();
        public List<Vector2> Path = new List<Vector2>();

        public KeyboardState keyboardState;
        public KeyboardState newState;
        public KeyboardState oldState;
        public MouseState mouseState;

        public int towerType = 1;
        public const int SCALE = 64;
        public float money;// = 500;
        public float globalTime = 0.0f;
        

        public bool pressed = false;

        //FPS Counter
        int frameRate = 0;
        int frameCount = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;
        //Vector2 spritePosition = new Vector2(385, 300); 
        int secondsThreshold = 5;//how many seconds user gets money
        int moneyToUser = 1000; //how much money they get ever secondsThreshold
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = SCALE * 16; // (32 * 32  = 1024)
            graphics.PreferredBackBufferHeight = SCALE * 12; // 24 * 32 = 768

            
            Content.RootDirectory = "Content";
        }

        

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            money = 1500;
            comicSansMS = Content.Load<SpriteFont>("SpriteFont1");
            //test.pos = new Vector2(50, 50);
            this.IsMouseVisible = true;
            base.Initialize();
            //test = new Tower(new Vector2(200, 100), tex, this);
            user = new User(this);

            //Towers.Add(test);
            //user.drawRandomTiles();

            Components.Add(user);
            Components.Add(terrain);

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texTower = Content.Load<Texture2D>("circle");
            texTile = Content.Load<Texture2D>("tile");
            texCreep = Content.Load<Texture2D>("creepz");
            texProj = Content.Load<Texture2D>("projectile");

            effect = Content.Load<Effect>("effects");
            Texture2D heightMap = Content.Load<Texture2D>("heightmap");         terrain.LoadHeightData(heightMap);



            // TODO: use this.Content to load your game content here
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            //float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            globalTime += ((float)gameTime.ElapsedGameTime.TotalMilliseconds/1000.0f);
            //System.Diagnostics.Debug.WriteLine(globalTime);

            //FPS Counter Update
            elapsedTime += gameTime.ElapsedGameTime;
            
            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCount;
                frameCount = 0;
            }
            

            newState = Keyboard.GetState();

            base.Update(gameTime);

            oldState = newState;

            giveUserMoney(gameTime);
        }

        public void giveUserMoney(GameTime gameTime)
        {
            //giving +1000 every 5 seconds
            if (gameTime.TotalGameTime.TotalSeconds > secondsThreshold)
            {
                money += moneyToUser;
                secondsThreshold = (int)(gameTime.TotalGameTime.TotalSeconds + secondsThreshold);
            }
        }

        private bool KeypressTest(Keys theKey)
        {
                //if (newState.IsKeyUp(theKey) && oldState.IsKeyDown(theKey))
                if (oldState.IsKeyDown(theKey))
                    return true;
                return false;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 

        void drawTower(Tower x)
        {
              spriteBatch.Draw(x.texture, x.pos, Color.White);
        }
        void drawTile(Tile x)
        {
            spriteBatch.Draw(x.texture, x.pos, Color.White);
        }

        void drawCreeps(Creep x)
        {
            if (x.isDead)
                Creeps.Remove(x);
            spriteBatch.Draw(x.texture, x.pos, Color.White);
            /*System.Diagnostics.Debug.WriteLine("Creeop");
            System.Diagnostics.Debug.WriteLine(x.pos.X);
            System.Diagnostics.Debug.WriteLine(x.pos.Y);*/
        }

        void drawProj(Projectile x)
        {
            x.damageCreep();
            UpdateProj(x);
            spriteBatch.Draw(x.texture, x.pos, Color.White);
            /*System.Diagnostics.Debug.WriteLine("PROJECTILE");
            System.Diagnostics.Debug.WriteLine(x.pos.X);
            System.Diagnostics.Debug.WriteLine(x.pos.Y);*/
        }
        public void UpdateProj(Projectile p)
        {
            if ((p.pos.X - p.creep.pos.X) > 0)
                p.pos.X -= p.Speed;
            else if ((p.pos.X - p.creep.pos.X) < 0)
                p.pos.X += p.Speed;
            if ((p.pos.Y - p.creep.pos.Y) > 0)
                p.pos.Y -= p.Speed;
            else if ((p.pos.Y - p.creep.pos.Y) < 0)
                p.pos.Y += p.Speed;

            if ((p.pos.Y == p.creep.pos.Y) && (p.pos.X == p.creep.pos.Y))
            {
                Projs.Remove(p);
            }

            if (p.creep.isDead)
            {
                Projs.Remove(p);
            }

            /*System.Diagnostics.Debug.WriteLine("CreeopProjectile");
            System.Diagnostics.Debug.WriteLine(p.creep.pos.X);
            System.Diagnostics.Debug.WriteLine(p.creep.pos.Y);*/

        }

        

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Draw The Terrain

            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            device.RenderState.CullMode = CullMode.None;

            Matrix worldMatrix = Matrix.CreateTranslation(-terrain.terrainWidth / 2.0f, 0, terrain.terrainHeight / 2.0f) * Matrix.CreateRotationY(terrain.angle);
            effect.CurrentTechnique = effect.Techniques["Colored"];
            effect.Parameters["xView"].SetValue(terrain.viewMatrix);
            effect.Parameters["xProjection"].SetValue(terrain.projectionMatrix);
            effect.Parameters["xWorld"].SetValue(worldMatrix);

            effect.Parameters["xEnableLighting"].SetValue(true);
            Vector3 lightDirection = new Vector3(1.0f, -1.0f, -1.0f);
            lightDirection.Normalize();
            effect.Parameters["xLightDirection"].SetValue(lightDirection);
            effect.Parameters["xAmbient"].SetValue(0.1f);


            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                device.VertexDeclaration = terrain.myVertexDeclaration;
                device.Indices = terrain.myIndexBuffer;
                device.Vertices[0].SetSource(terrain.myVertexBuffer, 0, Major.Terrain.VertexPositionNormalColored.SizeInBytes);
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, terrain.vertices.Length, 0, terrain.indices.Length / 3);

                pass.End();
            }
            effect.End();

            base.Draw(gameTime);

            // TODO: Add your drawing code here
            //test.Draw(gameTime);

            string strMoney = string.Format("Money - {0}", money);
            string strTime = string.Format("Time - {0}", (int)globalTime);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            Towers.ForEach(drawTower);
            //Towers.ForEach(findClosestCreep);
            Tiles.ForEach(drawTile);
            Creeps.ForEach(drawCreeps);
            Projs.ForEach(drawProj);
            
            

            spriteBatch.DrawString(comicSansMS,strMoney, new Vector2(3, 13), Color.Black);

            spriteBatch.DrawString(comicSansMS, strTime, new Vector2(3, 33), Color.Black);
            
            spriteBatch.End();

            //FPS + Crosshair

            frameCount++;

            string fps = string.Format("fps: {0}", frameRate);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);


            spriteBatch.DrawString(comicSansMS, fps, new Vector2(3, 103), Color.Black);
            spriteBatch.DrawString(comicSansMS, fps, new Vector2(2, 102), Color.White);

            spriteBatch.End(); 

            base.Draw(gameTime);
        }
    }
}
