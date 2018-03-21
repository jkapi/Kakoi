﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StrangerCade.Framework;
using Microsoft.Xna.Framework.Input;
using StrangerCade.Framework.UI;

namespace Game1.Minigames.DontTapWhite
{
    class Donttapwhite : Room 
    {
        //----DESIGN RELATED----//
        int[,] grid;
        public int gridDimensionLengthX { get; private set; }
        public int gridDimensionLengthY { get; private set; }
        //Field
        public Rectangle rec { get; private set; }

        //Available tiles to change to click tiles.
        List<Tile> whiteTiles = new List<Tile>();

        //Properties of the tile.
        public int widthLengthTile { get; private set; }
        public int heightLengthTile { get; private set; }

        //Spawn delay Click tile.
        public int delay { get; private set; }

        //----GAME SYSTEM RELATED----//
        public int stateOfGame { get; private set; }
        public List<Player> currentPlayerList { get; private set; }

        SpriteFont Arial;

        Button testBtn;

        //----CONSTRUCTORS+METHODS----//
        public override void Initialize()
        {
            stateOfGame = 0;
            if (stateOfGame == 0)
            {
                Arial = Content.Load<SpriteFont>("Arial");
                testBtn = new Button(new Vector2(20, 80), new Vector2(200, 30), Arial, "Start");
                testBtn.OnClick += startGame;
                Objects.Add(testBtn);
                //
                grid = getRandomGrid();
                gridDimensionLengthX = grid.GetLength(0);
                gridDimensionLengthY = grid.GetLength(1);
                rec = new Rectangle(Graphics.PreferredBackBufferWidth / 2 - Graphics.PreferredBackBufferWidth / 4, 0, Graphics.PreferredBackBufferWidth / 2, Graphics.PreferredBackBufferHeight);
                widthLengthTile = rec.Width / gridDimensionLengthX;
                heightLengthTile = rec.Height / gridDimensionLengthY;
                Tile.InitTotalTiles(grid.GetLength(0) * grid.GetLength(1));
                InitPlayground();
                delay = 20;
            }
        }
        // returns a random grid(4x4,6x6,8x8)
        public int[,] getRandomGrid()
        {
            List<int[,]> listGrids = new List<int[,]>();
            Random getListGridsNum = new Random();

            //Initialize multiple grids in listGrids
            for (int i = 4; i < 9; i += 2)
            {
                listGrids.Add(new int[i, i]);
            }

            //Get a random grid back
            return listGrids[getListGridsNum.Next(0, listGrids.Count)];

        }

        public override void Update()
        {
            if (stateOfGame == 1)
            {
                foreach (Tile aTile in Tile.totalTiles)
                {
                    if (GetMouseLeftClickedPos().X > aTile.position.X & GetMouseLeftClickedPos().X < aTile.position.X + aTile.tile.Width & GetMouseLeftClickedPos().Y > aTile.position.Y & GetMouseLeftClickedPos().Y < aTile.position.Y + aTile.tile.Height)
                    {
                        if (aTile.color == Color.Gray)
                        {
                            stateOfGame++;
                            //yet to make player life = 0;
                        }
                        if (aTile.color == Color.Black)
                        {
                            aTile.color = Color.Gray;
                            aTile.outline = true;
                        }
                    }

                }
            }
        }
        public override void Draw()
        {
            View.DrawText(Arial, "Waiting for players", new Vector2(20, 40));
            //draw the tiles on the field.
            foreach (Tile aTile in Tile.totalTiles)
            {
                View.DrawRectangle(aTile.tile, aTile.outline, aTile.color);
            }
            if (stateOfGame == 1)
            {
                //spawn the click tiles.
                if (delay < 0)
                {
                    foreach (Tile aTile in Tile.totalTiles)
                    {
                        if (aTile.color == Color.Gray)
                        {
                            whiteTiles.Add(aTile);
                        }
                    }

                    if (whiteTiles.Count > 0)
                    {
                        Tile currentTile = whiteTiles[Tile.GetRandomTilePos(0, whiteTiles.Count)];
                        foreach (Tile aTile in Tile.totalTiles)
                        {
                            if (aTile.positionTile == currentTile.positionTile)
                            {
                                aTile.outline = false;
                                aTile.color = Color.Black;
                            }
                        }
                    }
                    whiteTiles.Clear();
                    delay = 20;
                }
                else
                {
                    delay--;
                }
            }
        }

        private void startGame(object sender, EventArgs e)
        {
            stateOfGame++;
            if (stateOfGame > 2)
            {
                stateOfGame = 0;
                return;
            }
        }

        //Return mouse click position
        public Vector2 GetMouseLeftClickedPos()
        {
            if (Mouse.CheckPressed(MouseButtons.Left))
            {
                 return Mouse.Position;
            }
            return new Vector2(-1, -1);
        }
        //Init the tiles, but isnt drawn yet
        public void InitPlayground()
        {
            //Width and height of the tile.
            int widthLengthTile = rec.Width / gridDimensionLengthX;
            int heightLengthTile = rec.Height / gridDimensionLengthY;
            //Fill the field with rectangles.
            int countX = 0;
            int countY = 0;
            for (int i = 0; i < grid.GetLength(0) * grid.GetLength(1); i++)
            {
                Vector2 startPos = new Vector2((widthLengthTile * countX) + rec.X, (heightLengthTile * countY) + rec.Y);
                Rectangle tileRec = new Rectangle(Convert.ToInt32(startPos.X), Convert.ToInt32(startPos.Y), widthLengthTile, heightLengthTile);
                Tile currentTile = new Tile(tileRec, Color.Gray, true, startPos, i);

                if (countX < grid.GetLength(0) - 1)
                {
                    countX++;
                }
                else
                {
                    if (countY < grid.GetLength(1) - 1)
                    {
                        countY++;
                        countX = 0;
                    }
                    else
                    {
                        return;
                    }

                }
            }
        }
    }
}