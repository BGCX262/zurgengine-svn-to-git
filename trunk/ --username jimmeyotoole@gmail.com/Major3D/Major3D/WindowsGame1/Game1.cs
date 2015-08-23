using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Major
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public struct VertexPositionNormalColored
        {
            public Vector3 Position;
            public Color Color;
            public Vector3 Normal;

            public static int SizeInBytes = 7 * 4;
            public static VertexElement[] VertexElements = new VertexElement[]
              {
                  new VertexElement( 0, 0, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Position, 0 ),
                  new VertexElement( 0, sizeof(float) * 3, VertexElementFormat.Color, VertexElementMethod.Default, VertexElementUsage.Color, 0 ),
                  new VertexElement( 0, sizeof(float) * 4, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Normal, 0 ),
              };
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        Effect effect;

        VertexPositionNormalColored[] vertices;
        VertexBuffer myVertexBuffer;

        VertexDeclaration myVertexDeclaration;
        int[] indices;
        IndexBuffer myIndexBuffer;

        public Matrix viewMatrix;
        public Matrix projectionMatrix;

        private int terrainWidth;
        private int terrainHeight;
        private float[,] heightData;

        public User user;

        float angle = 0;

        public const int SCALE = 64;

        public Texture2D grass;

        public Tower test;//(new Vector3(-10,35,5), Texture2D t);

        public List<Tower> Towers = new List<Tower>();

        public Model lilGuy;



        public KeyboardState keyboardState;
        public KeyboardState newState;
        public KeyboardState oldState;
        public MouseState mouseState;

        public int towerType = 1;
        public float money;// = 500;
        public float globalTime = 0.0f;

        public Texture2D texTower;
        public Texture2D texTile;
        public Texture2D texCreep;
        public Texture2D texProj;

        //viewMatrix = Matrix.CreateLookAt(new Vector3(0, 100, 100), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

        public Vector3 camPos;
        public Vector3 camLook;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Major 3D";

            this.IsMouseVisible = true;
            //test=new Tower(new Vector3(-10,35,5), lilGuy,Game1);
            Towers.Add(test);

            camPos = new Vector3(0, 100, 100);
            camLook = new Vector3(0, 0, 0);
               

            Components.Add(user);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            device = graphics.GraphicsDevice;
            spriteBatch = new SpriteBatch(GraphicsDevice);

            effect = Content.Load<Effect>("effects");

            Texture2D heightMap = Content.Load<Texture2D>("heightmap"); LoadHeightData(heightMap);
            grass = Content.Load<Texture2D>("grass");
            lilGuy = Content.Load<Model>("lilguy");

            SetUpIndices();
            SetUpVertices();
            SetUpCamera();
            CalculateNormals();

            CopyToBuffers();
        }

        private void SetUpVertices()
        {
            float minHeight = float.MaxValue;
            float maxHeight = float.MinValue;
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    if (heightData[x, y] < minHeight)
                        minHeight = heightData[x, y];
                    if (heightData[x, y] > maxHeight)
                        maxHeight = heightData[x, y];
                }
            }

            vertices = new VertexPositionNormalColored[terrainWidth * terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    vertices[x + y * terrainWidth].Position = new Vector3(x, heightData[x, y], -y);

                    if (heightData[x, y] < minHeight + (maxHeight - minHeight) / 4)
                        vertices[x + y * terrainWidth].Color = Color.Blue;
                    else if (heightData[x, y] < minHeight + (maxHeight - minHeight) * 2 / 4)
                        vertices[x + y * terrainWidth].Color = Color.Green;
                    else if (heightData[x, y] < minHeight + (maxHeight - minHeight) * 3 / 4)
                        vertices[x + y * terrainWidth].Color = Color.Brown;
                    else
                        vertices[x + y * terrainWidth].Color = Color.White;
                }
            }

            myVertexDeclaration = new VertexDeclaration(device, VertexPositionNormalColored.VertexElements);
        }

        private void SetUpIndices()
        {
            indices = new int[(terrainWidth - 1) * (terrainHeight - 1) * 6];
            int counter = 0;
            for (int y = 0; y < terrainHeight - 1; y++)
            {
                for (int x = 0; x < terrainWidth - 1; x++)
                {
                    int lowerLeft = x + y * terrainWidth;
                    int lowerRight = (x + 1) + y * terrainWidth;
                    int topLeft = x + (y + 1) * terrainWidth;
                    int topRight = (x + 1) + (y + 1) * terrainWidth;

                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;

                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = lowerRight;
                }
            }
        }

        private void CalculateNormals()
        {
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];

                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;
            }

            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal.Normalize();
        }

        private void CopyToBuffers()
        {
            myVertexBuffer = new VertexBuffer(device, vertices.Length * VertexPositionNormalColored.SizeInBytes, BufferUsage.WriteOnly);
            myVertexBuffer.SetData(vertices);

            myIndexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
            myIndexBuffer.SetData(indices);
        }

        private void SetUpCamera()
        {
            viewMatrix = Matrix.CreateLookAt(camPos, camLook, new Vector3(0, 1, 0));
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 1.0f, 300.0f);
        }

        private void LoadHeightData(Texture2D heightMap)
        {
            terrainWidth = heightMap.Width;
            terrainHeight = heightMap.Height;

            Color[] heightMapColors = new Color[terrainWidth * terrainHeight];
            heightMap.GetData(heightMapColors);

            heightData = new float[terrainWidth, terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainHeight; y++)
                    heightData[x, y] = heightMapColors[x + y * terrainWidth].R / 5.0f;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Delete))
                angle += 0.05f;
            if (keyState.IsKeyDown(Keys.PageDown))
                angle -= 0.05f;

            SetUpCamera();

            base.Update(gameTime);
        }

        public void DrawModel(Model model, Matrix worldMatrix, Texture2D texture, bool applyDiffuse, bool enableFog)
        {
            Matrix[] boneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransforms[mesh.ParentBone.Index] * worldMatrix;
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;

                    effect.EnableDefaultLighting();

                    //if (applyDiffuse)
                    //    effect.DiffuseColor = Weather.settings.ambientColor;

                    // Set the specular lighting to match the sky color.
                    //effect.SpecularColor = Weather.settings.lightColor;
                    effect.SpecularPower = 8;

                    // Set the fog to match the black background color
                    //effect.FogEnabled = enableFog;
                    //effect.FogColor = Weather.settings.lightColor * 2;
                    //effect.FogStart = Weather.settings.fogStart;
                    //effect.FogEnd = Weather.settings.fogEnd;

                    if (texture != null)
                    {
                        effect.Texture = texture;
                        effect.TextureEnabled = true;
                    }
                }

                mesh.Draw();
            }
        }

        public void DrawT(Tower x)
        {
                        
        }

        

        protected override void Draw(GameTime gameTime)
        {
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            device.RenderState.CullMode = CullMode.None;

            Matrix worldMatrix = Matrix.CreateTranslation(-terrainWidth / 2.0f, 0, terrainHeight / 2.0f) * Matrix.CreateRotationY(angle);
            effect.CurrentTechnique = effect.Techniques["Colored"];
            effect.Parameters["xTexture"].SetValue(grass);
            effect.Parameters["xView"].SetValue(viewMatrix);
            effect.Parameters["xProjection"].SetValue(projectionMatrix);
            effect.Parameters["xWorld"].SetValue(worldMatrix);

            effect.Parameters["xEnableLighting"].SetValue(true);
            Vector3 lightDirection = new Vector3(1.0f, -1.0f, -1.0f);
            lightDirection.Normalize();
            effect.Parameters["xLightDirection"].SetValue(lightDirection);
            effect.Parameters["xAmbient"].SetValue(0.2f);
            

            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                
                device.VertexDeclaration = myVertexDeclaration;
                device.Indices = myIndexBuffer;
                device.Vertices[0].SetSource(myVertexBuffer, 0, VertexPositionNormalColored.SizeInBytes);
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertices.Length, 0, indices.Length / 3);

                pass.End();
            }
            /*
            foreach (BasicEffect x in mesh.Effects) 
            {
                x.TextureEnabled = true;
                x.Texture = grass;*/
            
            effect.End();
            // DrawModel(Model model, Matrix worldMatrix, Texture2D texture, bool applyDiffuse, bool enableFog)

            
            DrawModel(lilGuy, Matrix.CreateWorld(new Vector3(-10,35,5), Vector3.Forward, Vector3.Up), grass, true, true);

            base.Draw(gameTime);
        }
    }
}