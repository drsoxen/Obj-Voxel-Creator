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

namespace VoxEditor
{
    /// <summary>
    /// basic Free look camera
    /// controlled from the outside
    /// </summary>
    public class Camera
    {
        protected Matrix view;
        protected Matrix projection;

        public Vector3 position;
        protected Vector3 target;

        protected float farPlane = 1000f;
        protected float nearPlane = 10f;

        protected float yaw = 0f;
        protected float pitch = 0f;

        protected Ray mouseRay;        

        protected GraphicsDevice device;

        public Camera(Vector3 position, Vector3 target, float near, float far , Game1 game , Matrix View,Matrix Proj)
        {
            if (position == target) target.Z += 10f;

            this.position = position;
            this.target = target;

            this.device = game.GraphicsDevice;
            this.nearPlane = near;
            this.farPlane = far;

            // If the camera's looking straight down it has to be fixed

            CalculateYawPitch();

            while (Math.Abs(pitch) >= MathHelper.ToRadians(80))
            {
                this.position.Z += 10;
                CalculateYawPitch();
            }

            projection = Proj;
            view = View;
            //projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, nearPlane, farPlane);
            //view = Matrix.CreateLookAt(this.position, target, this.Up);
        }

        protected virtual void CalculateYawPitch()
        {
            Vector3 dir =  target - position;            
            dir.Normalize();
            Vector3 m = dir; m.Y = position.Y;

            yaw = (float)Math.Atan2(dir.X,dir.Z);

            float len = (new Vector2(m.X, m.Z)).Length();
            pitch = (float)Math.Atan2(dir.Y,len);
        }

        public Ray GetMouseRay(Vector2 mousePosition, Viewport viewport)
        {
            Vector3 near = new Vector3(mousePosition, 0);
            Vector3 far = new Vector3(mousePosition, 1);

            near = viewport.Unproject(near, projection, view, Matrix.Identity);
            far = viewport.Unproject(far, projection, view, Matrix.Identity);

            return new Ray(near, Vector3.Normalize(far - near));
        }

        public virtual void UpdateMouseRay(Vector2 mousePos, Viewport viewport)
        {
            mouseRay = this.GetMouseRay(mousePos, viewport);
        }

        public virtual void Update()
        {            
            view = Matrix.CreateLookAt(position, target, this.Up);
        }

        public virtual void MoveForward(float amount)
        {
            Vector3 temp = position;
            position += amount * this.Direction;
            target += amount * this.Direction;
            this.Update();
        }

        public virtual void Starfe(float amount, bool Target)
        {
            position += amount*this.Right;
            if(Target)
            target += amount * this.Right;
            this.Update();
        }

        public virtual void AddYaw(float angle)
        {
            yaw += angle;
            Vector3 dir = this.Direction;
            dir = Vector3.Transform(dir, Matrix.CreateFromAxisAngle(this.Up, angle));

            target =position + Vector3.Distance(target, position) * dir;
            CalculateYawPitch();
            this.Update();
        }

        public virtual void AddPitch(float angle)
        {
            if (Math.Abs(pitch + angle) >= MathHelper.ToRadians(80)) return;
            pitch += angle;
            Vector3 dir = this.Direction;
            dir = Vector3.Transform(dir, Matrix.CreateFromAxisAngle(this.Right, angle));

            target = position + Vector3.Distance(target, position) * dir;
            CalculateYawPitch();
            this.Update();
        }

        public virtual void Levitate(float amount)
        {
            position.Y += amount;
            target.Y += amount;
            this.Update();
        }


        #region Properties
        public BoundingFrustum GetFrustum
        {
            get
            {
                return new BoundingFrustum(View * Projection);
            }
        }

        public Matrix View
        {
            get
            {
                return view;
            }
        }

        public Matrix Projection
        {
            get
            {
                return projection;
            }
        }

        public Vector3 Direction
        {
            get
            {
                return Vector3.Normalize(target-position);
            }
        }

        public Vector3 Up
        {
            get
            {
                return Vector3.Up;
            }
        }

        public Vector3 Right
        {
            get
            {
                return Vector3.Normalize(Vector3.Cross(this.Direction,this.Up));
            }
        }

        public Ray MouseRay
        {
            get
            {
                return mouseRay;
            }
        }

        #endregion
    }
}
