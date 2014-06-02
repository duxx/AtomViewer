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

namespace AtomViewer
{
    public enum Type 
    {
        Proton,
        Neutron,
        ElectronRing1,
        ElectronRing2
    };

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Model sphereModel;
        float aspectRatio;
        List<Sphere> spheres = new List<Sphere>();
        Matrix projection;
        Matrix view;
        float modelRotation;

        SpriteFont systemFont;

        Vector3 cameraPosition = new Vector3( 0.0f, 50.0f, 5.0f );

        public Game1()
        {
            graphics = new GraphicsDeviceManager( this );
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
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch( GraphicsDevice );

            sphereModel = Content.Load<Model>( @"Models\sphere" );
            systemFont = Content.Load<SpriteFont>( @"Fonts\contentFont" );
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;

            //Neutrons
            spheres.Add( new Sphere( new Vector3( 0, 1, 0 ), Color.White.ToVector3(), Type.Neutron ) );
            spheres.Add( new Sphere( new Vector3( -2, -1, 2 ), Color.White.ToVector3(), Type.Neutron ) );
            spheres.Add( new Sphere( new Vector3( 2, -1, 2 ), Color.White.ToVector3(), Type.Neutron ) );
            spheres.Add( new Sphere( new Vector3( 0, -1, -2 ), Color.White.ToVector3(), Type.Neutron ) );
            spheres.Add( new Sphere( new Vector3( 0.5f, -2, 0.7f ), Color.White.ToVector3(), Type.Neutron ) );
            spheres.Add( new Sphere( new Vector3( -0.5f, -2, -0.7f ), Color.White.ToVector3(), Type.Neutron ) );

            //Protons
            spheres.Add( new Sphere( new Vector3( 0, 0, -1 ), Color.Red.ToVector3(), Type.Proton ) );
            spheres.Add( new Sphere( new Vector3( -1, 0, 1 ), Color.Red.ToVector3(), Type.Proton ) );
            spheres.Add( new Sphere( new Vector3( 1, 0, 1 ), Color.Red.ToVector3(), Type.Proton ) );
            spheres.Add( new Sphere( new Vector3( 0, -1, 2 ), Color.Red.ToVector3(), Type.Proton ) );
            spheres.Add( new Sphere( new Vector3( -1, -1, 0 ), Color.Red.ToVector3(), Type.Proton ) );
            spheres.Add( new Sphere( new Vector3( 1, -1, 0 ), Color.Red.ToVector3(), Type.Proton ) );

            //Electrons Ring I
            spheres.Add( new Sphere( new Vector3( 5f, 0, 0 ), Color.Blue.ToVector3(), Type.ElectronRing1 ) );
            spheres.Add( new Sphere( new Vector3( -5f, 0, 0 ), Color.Blue.ToVector3(), Type.ElectronRing1 ) );

            //Electrons Ring II
            spheres.Add( new Sphere( new Vector3( 10f, 0, 0 ), Color.Blue.ToVector3(), Type.ElectronRing2 ) );
            spheres.Add( new Sphere( new Vector3( -10f, 0, 0 ), Color.Blue.ToVector3(), Type.ElectronRing2 ) );
            spheres.Add( new Sphere( new Vector3( 0f, 10, 0 ), Color.Blue.ToVector3(), Type.ElectronRing2 ) );
            spheres.Add( new Sphere( new Vector3( 0f, -10, 0 ), Color.Blue.ToVector3(), Type.ElectronRing2 ) );

            projection = Matrix.CreatePerspectiveFieldOfView(
                            MathHelper.ToRadians( 45.0f ), aspectRatio,
                            1.0f, 10000.0f );
            view = Matrix.CreateLookAt( cameraPosition,
                            Vector3.Zero, Vector3.Up );
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
        protected override void Update( GameTime gameTime )
        {
            // Allows the game to exit
            if ( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed )
                this.Exit();

            if ( Keyboard.GetState().IsKeyDown( Keys.Escape ) ) this.Exit();

            modelRotation += ( float ) gameTime.ElapsedGameTime.TotalMilliseconds *
                                MathHelper.ToRadians( 0.1f );

            base.Update( gameTime );
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw( GameTime gameTime )
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[sphereModel.Bones.Count];
            sphereModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach ( Sphere sphere in spheres )
            {
                // Draw the model. A model can have multiple meshes, so loop.
                foreach ( ModelMesh mesh in sphereModel.Meshes )
                {
                    // This is where the mesh orientation is set, as well 
                    // as our camera and projection.
                    foreach ( BasicEffect effect in mesh.Effects )
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        effect.DiffuseColor = sphere.Color;
                        if ( sphere.Type == Type.ElectronRing1 )
                        {
                            effect.World = transforms[mesh.ParentBone.Index] *
                                Matrix.CreateTranslation( sphere.Position )
                                * Matrix.CreateRotationZ( -modelRotation )
                                * Matrix.CreateRotationY( 180 )
                                //* Matrix.CreateRotationX( -60 )
                                ;
                        }
                        else if ( sphere.Type == Type.ElectronRing2 )
                        {
                            effect.World = transforms[mesh.ParentBone.Index] *
                                Matrix.CreateTranslation( sphere.Position )
                                * Matrix.CreateRotationZ( modelRotation )
                                * Matrix.CreateRotationY( -180 )
                                //* Matrix.CreateRotationX( 60 )
                                ;
                        }
                        else
                        {
                            effect.World = transforms[mesh.ParentBone.Index] *
                                Matrix.CreateTranslation( sphere.Position )
                                //* Matrix.CreateRotationX( modelRotation )
                                //* Matrix.CreateRotationZ( modelRotation )
                                //* Matrix.CreateRotationY( modelRotation )
                                ;
                        }
                        //effect.World *= Matrix.CreateRotationX( modelRotation / 10 );
                        effect.View = view;
                        effect.Projection = projection;
                    }
                    // Draw the mesh, using the effects set above.
                    mesh.Draw();
                }
            }

            //Draw debug info
            spriteBatch.Begin();
            spriteBatch.DrawString( systemFont, "Carbon atom", new Vector2( 10, 10 ), Color.Gold );
            spriteBatch.DrawString( systemFont, "Atomic number: 6", new Vector2( 10, 40 ), Color.Green );
            spriteBatch.DrawString( systemFont, "Atomic Mass Avg: 12.011", new Vector2( 10, 70 ), Color.Green );
            spriteBatch.DrawString( systemFont, "Protons: 6", new Vector2( 10, 140 ), Color.White );
            spriteBatch.DrawString( systemFont, "Neutrons: 6", new Vector2( 10, 170 ), Color.Red );
            spriteBatch.DrawString( systemFont, "Electrons (1st ring): 2", new Vector2( 10, 200 ), Color.Blue );
            spriteBatch.DrawString( systemFont, "Electrons (2nd ring): 4", new Vector2( 10, 230 ), Color.Blue );
            //Protons:\nNeutrons:\nElectrons (1st ring):\nElectrons (2nd ring):
            spriteBatch.End();

            //Reset depth stencil after spritebatch
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
