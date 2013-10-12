using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Project1
{
    public class Camera
    {
        #region Fields

        private GraphicsDeviceManager graphics;

        /// <summary>
        /// The eye position in space
        /// </summary>
        private Vector3 eye = new Vector3(0, 30, 0);

        /// <summary>
        /// The location we are looking at in space.
        /// </summary>
        private Vector3 center = new Vector3(0, 0, 0);

        /// <summary>
        /// The up direction
        /// </summary>
        private Vector3 up = new Vector3(0, 1, 0);

        private float fov = MathHelper.ToRadians(35);
        private float znear = 10;
        private float zfar = 1000;

        private Matrix view;
        private Matrix projection;

        private bool mousePitchYaw = true;
        private bool mousePanTilt = true;

        private MouseState lastMouseState;

        private bool useChaseCamera = false;
        private Vector3 desiredEye = Vector3.Zero;
        private Vector3 desiredUp = Vector3.Zero;
        private Vector3 velocity = Vector3.Zero;
        private Vector3 velocity2 = Vector3.Zero;
        private float stiffness = 100;
        private float damping = 60;

        #endregion

        #region Properties

        public Matrix View { get { return view; } }
        public Matrix Projection { get { return projection; } }

        public bool MousePitchYaw { get { return mousePitchYaw; } set { mousePitchYaw = value; } }
        public bool MousePanTilt { get { return mousePanTilt; } set { mousePanTilt = value; } }

        public Vector3 Center { get { return center; } set { center = value; ComputeView(); } }
        public Vector3 Eye { get { return eye; } set { eye = value; ComputeView(); } }
        public Vector3 Up { get { return up; } set { up = value; ComputeView(); } }

        public bool UseChaseCamera { get { return useChaseCamera; } set { useChaseCamera = value; } }
        public Vector3 DesiredEye { get { return desiredEye; } set { desiredEye = value; } }
        public Vector3 DesiredUp { get { return desiredUp; } set { desiredUp = value; } }
        public float Stiffness { get { return stiffness; } set { stiffness = value; } }
        public float Damping { get { return damping; } set { damping = value; } }

        #endregion

        #region Construction and Initialization

        /// <summary>
        /// Constructor. Initializes the graphics field from a passed parameter.
        /// </summary>
        /// <param name="graphics">The graphics device manager for our program</param>
        public Camera(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }


        public void Initialize()
        {
            ComputeView();
            ComputeProjection();
            lastMouseState = Mouse.GetState();
        }

        #endregion

        #region Matrix Computations

        private void ComputeView()
        {
            view = Matrix.CreateLookAt(eye, center, up);
        }

        private void ComputeProjection()
        {
            projection = Matrix.CreatePerspectiveFieldOfView(fov,
                graphics.GraphicsDevice.Viewport.AspectRatio, znear, zfar);
        }

        #endregion

        /// <summary>
        /// Resets the camera class variables to default
        /// </summary>
        public void Reset()
        {
            eye = new Vector3(0, 30, 0);
            center = new Vector3(0, 0, 0);
            up = new Vector3(0, 1, 0);
            fov = MathHelper.ToRadians(35);
            znear = 10;
            zfar = 2000;
            mousePitchYaw = true;
            mousePanTilt = true;
            //useChaseCamera = false;
            desiredEye = Vector3.Zero;
            velocity = Vector3.Zero;
            stiffness = 100;
            damping = 600;
        }

        public void Update(GameTime gameTime)
        {

            if (useChaseCamera)
            {
                // Calculate the spring force
                Vector3 stretch = desiredEye - eye;
                Vector3 acceleration = stretch * stiffness - velocity * damping;

                Vector3 stretch2 = desiredUp - up;
                Vector3 accleration2 = stretch2 * stiffness - velocity2 * damping;

                // Apply acceleration
                velocity += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
                velocity2 += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Apply velocity
                eye += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                up += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}

