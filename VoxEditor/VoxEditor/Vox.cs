using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace VoxEditor
{
    public class Vox//:DrawableObject
    {
        GraphicsDevice graphicsDevice;
        Matrix m_View = Matrix.Identity;
        Matrix m_Projection = Matrix.Identity;
        Matrix m_World = Matrix.Identity;
        Effect fx;
        VertexPositionColor[] verts;
        int[] indices;
        int[] wire_indices;

        VertexBuffer vbo;
        IndexBuffer ibo;

        public Vector3 Position;

        List<BoundingBox> BoundingBoxes = new List<BoundingBox>();
        //BoundingBox boundingBox;

        float rot = 0.0f;
        float HalfSize;

        public Vox()
        {

        }

        public Vox(Vector3 Pos, float size, Color colour, GraphicsDevice gd, Matrix view,Matrix proj,Effect FX)
        {
            Position = Pos;
            graphicsDevice = gd;
            fx = FX;
            HalfSize = size / 2;

            view = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphicsDevice.Viewport.AspectRatio, 1.0f, 300.0f);

            m_View = view;
            m_Projection = proj;

            verts = new VertexPositionColor[]//frblud
            {
                new VertexPositionColor(new Vector3(Pos.X-HalfSize,Pos.Y+HalfSize,Pos.Z-HalfSize), colour),
                new VertexPositionColor(new Vector3(Pos.X+HalfSize,Pos.Y+HalfSize,Pos.Z-HalfSize), colour),
                new VertexPositionColor(new Vector3(Pos.X+HalfSize,Pos.Y-HalfSize,Pos.Z-HalfSize), colour),
                new VertexPositionColor(new Vector3(Pos.X-HalfSize,Pos.Y-HalfSize,Pos.Z-HalfSize), colour),
                new VertexPositionColor(new Vector3(Pos.X-HalfSize,Pos.Y+HalfSize,Pos.Z+HalfSize), colour),
                new VertexPositionColor(new Vector3(Pos.X+HalfSize,Pos.Y+HalfSize,Pos.Z+HalfSize), colour),
                new VertexPositionColor(new Vector3(Pos.X+HalfSize,Pos.Y-HalfSize,Pos.Z+HalfSize), colour),
                new VertexPositionColor(new Vector3(Pos.X-HalfSize,Pos.Y-HalfSize,Pos.Z+HalfSize), colour)
            };            
            
            indices = new int[]
            {
                2, 1, 0, 0, 3, 2,
                6, 5, 1, 1, 2, 6,
                7, 4, 5, 5, 6, 7,
                3, 0, 4, 4, 7, 3,
                1, 5, 4, 4, 0, 1,
                7, 6, 2, 2, 3, 7
            };

            wire_indices = new int[indices.Length * 2];

            for (int i = 0; i < indices.Length; i += 2)
            {
                wire_indices[i + i] = indices[i];
                if (i < indices.Length - 1)
                    wire_indices[i + i + 1] = indices[i + 1];
                else
                    break;
            }
            wire_indices[2] = 4;
            wire_indices[3] = 5;
            wire_indices[70] = 4;
            wire_indices[71] = 0;
            vbo = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), verts.Length, BufferUsage.WriteOnly);
            ibo = new IndexBuffer(graphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.WriteOnly);

            BoundingBoxes.Add(new BoundingBox(new Vector3(Pos.X - HalfSize, Pos.Y + HalfSize, Pos.Z + HalfSize), new Vector3(Pos.X + HalfSize, Pos.Y - HalfSize, Pos.Z + HalfSize)));
            BoundingBoxes.Add(new BoundingBox(new Vector3(Pos.X - HalfSize, Pos.Y + HalfSize, Pos.Z + HalfSize), new Vector3(Pos.X - HalfSize, Pos.Y - HalfSize, Pos.Z - HalfSize)));
            BoundingBoxes.Add(new BoundingBox(new Vector3(Pos.X + HalfSize, Pos.Y + HalfSize, Pos.Z + HalfSize), new Vector3(Pos.X + HalfSize, Pos.Y - HalfSize, Pos.Z - HalfSize)));
            BoundingBoxes.Add(new BoundingBox(new Vector3(Pos.X - HalfSize, Pos.Y + HalfSize, Pos.Z + HalfSize), new Vector3(Pos.X + HalfSize, Pos.Y + HalfSize, Pos.Z - HalfSize)));
            BoundingBoxes.Add(new BoundingBox(new Vector3(Pos.X - HalfSize, Pos.Y - HalfSize, Pos.Z + HalfSize), new Vector3(Pos.X + HalfSize, Pos.Y - HalfSize, Pos.Z - HalfSize)));
            BoundingBoxes.Add(new BoundingBox(new Vector3(Pos.X - HalfSize, Pos.Y - HalfSize, Pos.Z + HalfSize), new Vector3(Pos.X + HalfSize, Pos.Y - HalfSize, Pos.Z - HalfSize)));
            BoundingBoxes.Add(new BoundingBox(new Vector3(Pos.X - HalfSize, Pos.Y + HalfSize, Pos.Z - HalfSize), new Vector3(Pos.X + HalfSize, Pos.Y - HalfSize, Pos.Z - HalfSize)));
            
        }
        public void LoadContent(ContentManager content)
        {
        }

        public Nullable<Vector3> CheckRayIntersection(Ray ray)
        {
            for (int i = 0; i < BoundingBoxes.Count; i++)
            {
                if (ray.Intersects(BoundingBoxes[i]) != null)
                {
                    float intersect = ray.Intersects(BoundingBoxes[i]).Value;
                    Nullable<Vector3> positionInWorldSpace = ray.Position + ray.Direction * intersect;
                    return positionInWorldSpace;
                }
            }
            return null;

        }

        public void Update(GameTime gametime)
        {

        }

        public void Draw(Camera camera, GameTime gametime)
        {
            RasterizerState rs = new RasterizerState();
            graphicsDevice.RasterizerState = rs;
            graphicsDevice.BlendState = BlendState.Additive;

            graphicsDevice.SetVertexBuffer(vbo);
            graphicsDevice.Indices = ibo;

            foreach (EffectTechnique et in fx.Techniques)
            {
                //rot += (float)gametime.ElapsedGameTime.TotalSeconds * 0.5f;
                //if (rot > (float)(2 * Math.PI))
                //    rot -= (float)(2 * Math.PI);
                //m_World = Matrix.CreateScale(4.0f);
                //m_World *= Matrix.CreateFromYawPitchRoll(rot, rot, rot);

                m_Projection = camera.Projection;
                m_View = camera.View;
                fx.Parameters["World"].SetValue(m_World);
                fx.Parameters["View"].SetValue(m_View);
                fx.Parameters["Projection"].SetValue(m_Projection);
                fx.CurrentTechnique = et;

                graphicsDevice.BlendState = BlendState.Opaque;
                fx.CurrentTechnique.Passes[0].Apply();
                graphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, verts, 0, verts.Length, wire_indices, 0, wire_indices.Length / 2);
                graphicsDevice.BlendState = BlendState.NonPremultiplied;
                fx.CurrentTechnique.Passes[1].Apply();
                graphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, verts, 0, verts.Length, indices, 0, indices.Length / 3);
            }
        }
    }
}
