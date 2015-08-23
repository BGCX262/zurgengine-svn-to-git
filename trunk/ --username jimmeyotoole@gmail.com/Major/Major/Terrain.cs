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
using Major;


namespace Major
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    /// 

    public class Terrain : DrawableGameComponent
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

         public Terrain(Game game)
            : base(game)
        {

        }


         public VertexPositionNormalColored[] vertices;
         public VertexBuffer myVertexBuffer;

         public VertexDeclaration myVertexDeclaration;
         public int[] indices;
         public IndexBuffer myIndexBuffer;

         public Matrix viewMatrix;
         public Matrix projectionMatrix;
 
         public int terrainWidth;
         public int terrainHeight;
         public float[,] heightData;

         public float angle = 0;

        //Vars used by terrain
        
        
 
         protected override void LoadContent()
         {

            SetUpIndices();
            SetUpVertices();
            SetUpCamera();
            CalculateNormals();

            CopyToBuffers();
         }
 
         public void SetUpVertices()
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
 
             myVertexDeclaration = new VertexDeclaration(game.device, VertexPositionNormalColored.VertexElements);
         }

         public void SetUpIndices()
         {
             indices = new int[(terrainWidth - 1) * (terrainHeight - 1) * 6];
             int counter = 0;
             for (int y = 0; y < terrainHeight - 1; y++)
             {
                 for (int x = 0; x < terrainWidth - 1; x++)
                 {
                     int lowerLeft = x + y*terrainWidth;
                     int lowerRight = (x + 1) + y*terrainWidth;
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

         public void CalculateNormals()
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

         public void CopyToBuffers()
         {
             myVertexBuffer = new VertexBuffer(device, vertices.Length * VertexPositionNormalColored.SizeInBytes, BufferUsage.WriteOnly);
             myVertexBuffer.SetData(vertices);
 
             myIndexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
             myIndexBuffer.SetData(indices);
         }

         public void SetUpCamera()
         {
             viewMatrix = Matrix.CreateLookAt(new Vector3(0, 100, 100), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
             projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 1.0f, 300.0f);
         }
 
         public void LoadHeightData(Texture2D heightMap)
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

         

         public override void Update(GameTime gameTime)
         {
             // TODO: Add your update code here
             base.Update(gameTime);
         }
 
       
     }
 }