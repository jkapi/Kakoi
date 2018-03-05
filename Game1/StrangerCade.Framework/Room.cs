﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Reflection;

namespace StrangerCade.Framework
{
    /// <summary>
    /// This class is made to inhibit GameObjects. It can also be used as a standalone GameObject.
    /// </summary>
    class Room
    {
        /// <summary>
        /// ContentManager inherited from Game
        /// </summary>
        public ContentManager Content;
        /// <summary>
        /// GraphicsDevice.Graphics inherited from Game
        /// </summary>
        public GraphicsDeviceManager Graphics;
        /// <summary>
        /// GraphicsDevice inherited from Game
        /// </summary>
        public GraphicsDevice GraphicsDevice;
        /// <summary>
        /// Private spritebatch for initializing view.
        /// </summary>
        private SpriteBatch sb;
        /// <summary>
        /// The Game View used by Room and used for <see cref="GameObject">GameObjects</see>
        /// </summary>
        public View View;
        /// <summary>
        /// The keyboard handler. Passed to <see cref="GameObject">GameObjects</see>
        /// Updated in the Room.Update function
        /// </summary>
        public GMKeyboard Keyboard;
        /// <summary>
        /// The mouse handler. Passed to <see cref="GameObject">GameObjects</see>
        /// Updated in the Room.Update function
        /// </summary>
        public GMMouse Mouse;
        /// <summary>
        /// Clear color for the display. Its read by the Game object and the Game object will draw this color.
        /// </summary>
        public Color DrawClearColor = Color.SkyBlue;
        /// <summary>
        /// List of GameObjects. All the Initialize, Update and Draw functions in this list will be called by the room.
        /// </summary>
        public List<GameObject> Objects;
        /// <summary>
        /// Set to true to disable calling Update and Draw on objects that are not visible on the screen.
        /// </summary>
        /// <remarks>*Currently needs all <see cref="GameObject">GameObjects</see> in <see cref="Room.Objects">Objects</see> to have a <see cref="Sprite">Sprite</see>!*</remarks>
        public bool DeActivateOutsideWindow = false;
        /// <summary>
        /// Room background.
        /// </summary>
        public Texture2D Background;
        public GameTime GameTime;

        /// <summary>
        /// Initialize all <see cref="GameObject">GameObjects</see> in <c>Objects</c>.
        /// </summary>
        /// <remarks>
        /// This function initializes the View, Keyboard, Mouse and all Objects added to <c>List&lt;GameObject> Objects</c> in <c>public virtual void Initialize()</c>
        /// </remarks>
        /// <param name="content">ContentManager</param>
        /// <param name="graphics">The Game GraphicsDeviceManager</param>
        /// <param name="spriteBatch">Spritebatch for creation of views</param>
        public void Initialize(ContentManager content, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            Content = content;
            Graphics = graphics;
            GraphicsDevice = graphics.GraphicsDevice;
            sb = spriteBatch;
            Objects = new List<GameObject>();
            View = new View();
            View.Initialize(sb, GraphicsDevice);
            Keyboard = new GMKeyboard();
            Mouse = new GMMouse();
            Initialize();
            foreach (GameObject obj in Objects)
            {
                obj.PreInitialize(this);
                obj.Initialize();
            }
        }

        /// <summary>
        /// Room initializer to be overwritten by classes that inherit the Framework.Room class
        /// </summary>
        /// <remarks>
        /// This function is called after initialization of the view, keyboard and mouse. But befor initialization of <see cref="GameObject">GameObjects</see>
        /// This function exists for the purpose of creating <see cref="GameObject">GameObjects</see> and initializing other things to be used in the room.
        /// </remarks>
        /// <example>
        /// Example usage:\n
        /// See \ref ExampleRoom
        /// </example>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Update keyboard and mouse states. Runs Room.Update() and GameObjects.Update() after this.
        /// </summary>
        /// <remarks>
        /// Order of running:
        /// - PreUpdate()
        /// - <see cref="GMKeyboard.Update(GameTime)">Keyboard.Update()</see>
        /// - <see cref="GMMouse.Update()">Mouse.Update()</see>
        /// - Update()
        /// - For each GameObject in <see cref="Room.Objects">Objects</see>
        /// --> GameObject.Update()
        /// - PostUpdate()
        /// </remarks>
        /// <param name="gameTime">Current GameTime object</param>
        public void Update(GameTime gameTime)
        {
            GameTime = gameTime;

            PreUpdate();
            Keyboard.Update(gameTime);
            Mouse.Update();

            Update();

            int roomWidth = Graphics.PreferredBackBufferWidth;
            int roomHeight = Graphics.PreferredBackBufferHeight;

            foreach (GameObject obj in Objects)
            {
                if (DeActivateOutsideWindow)
                {
                    if (obj.Position.X < -obj.Sprite.Width | obj.Position.Y < -obj.Sprite.Height | obj.Position.X >= roomWidth | obj.Position.Y >= roomHeight)
                    {
                        obj.Activated = false;
                    }
                    else
                    {
                        obj.Activated = true;
                    }
                }

                if (obj.Activated)
                {
                    obj.Update(gameTime);
                }
            }

            PostUpdate();
        }

        /// <summary>
        /// First called code in Update. It's run before the update of the mouse and keyboard input.
        /// </summary>
        public virtual void PreUpdate()
        { }
        /// <summary>
        /// Generic room update handler.
        /// </summary>
        /// <example>
        /// Example usage:\n
        /// See \ref ExampleRoom
        /// </example>
        public virtual void Update()
        { }
        /// <summary>
        /// Update handler that is ran after the Update function but before the spritebatch Draw.
        /// </summary>
        /// <example>
        /// An example intent of this function is being able to avoid a draw call to an object by disabling it.
        /// </example>
        public virtual void PostUpdate()
        { }

        /// <summary>
        /// Game handler for draw calls
        /// </summary>
        /// <remarks>
        /// This function calls the draws in the following order:
        /// - <see cref="GMMouse.Draw()">Mouse.Draw()</see>
        /// - <see cref="Room.Draw()">Draw()</see>
        /// - For each GameObject in <see cref="Room.Objects">Objects</see>
        /// --> GameObject.Draw()
        /// - <see cref="Room.PostDraw">PostDraw()</see>
        /// - <see cref="Room.DrawGui">DrawGui()</see>
        /// 
        /// This function draws everything in the View.Viewport, except <see cref="Room.DrawGui">DrawGui()</see>
        /// </remarks>
        /// <param name="gameTime">Updated GameTime Object</param>
        public void Draw(GameTime gameTime)
        {
            GameTime = gameTime;

            Mouse.Draw();
            Viewport oldViewport = GraphicsDevice.Viewport;
            GraphicsDevice.Viewport = View.Viewport;
            Draw();
            foreach (GameObject obj in Objects)
            {
                if (obj.Activated)
                {
                    obj.Draw(View, gameTime);
                }
            }
            PostDraw();
            GraphicsDevice.Viewport = oldViewport;
            DrawGui();
        }

        /// <summary>
        /// Draw function to be overwritten by functions that inherit this class
        /// </summary>
        public virtual void Draw()
        { }

        /// <summary>
        /// Draw function called after object draws. Used to draw above objects.
        /// </summary>
        public virtual void PostDraw()
        { }

        /// <summary>
        /// Draw function for drawing and ignoring View.Viewport. This can be used to, for example draw above other viewports.
        /// </summary>
        public virtual void DrawGui()
        { }

        public static Room CurrentRoom;

        public static void GotoRoom(Type room)
        {
            var oldContent = CurrentRoom.Content;
            var oldGraphics = CurrentRoom.Graphics;
            var oldSpriteBatch = CurrentRoom.sb;
            CurrentRoom = (Room)Activator.CreateInstance(room);
            CurrentRoom.Initialize(oldContent, oldGraphics, oldSpriteBatch);
        }

        public static void LoadRoom(Type room, ContentManager content, GraphicsDeviceManager graphics, SpriteBatch spritebatch)
        {
            CurrentRoom = (Room)Activator.CreateInstance(room);
            CurrentRoom.Initialize(content, graphics, spritebatch);
        }
    }
}