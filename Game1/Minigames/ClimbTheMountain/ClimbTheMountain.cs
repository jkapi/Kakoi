﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StrangerCade.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Minigames.ClimbTheMountain
{
    class ClimbTheMountain : Room
    {
        //--DESIGN--//
        Texture2D bg;
        Texture2D wolk1;
        Texture2D wolk2;
        Texture2D wolk3;
        public List<Wolk> listWolkjes { get; private set; }
        public Rectangle playField { get; private set; }
        public Rectangle letterField { get; private set; }
        public int widthLengthBlock { get; private set; }
        public int heightLengthBlock { get; private set; }

        //--GAME--//
        //public List<string> queueOfLetters { get; private set; }
        public int score { get; private set; }

        public override void Initialize()
        {
            //queueOfLetters = new List<string>();
            bg = Content.Load<Texture2D>("minigame/climbthemountain/bgmountainNew");
            wolk1 = Content.Load<Texture2D>("minigame/climbthemountain/Wolk1");
            wolk2 = Content.Load<Texture2D>("minigame/climbthemountain/Wolk2");
            wolk3 = Content.Load<Texture2D>("minigame/climbthemountain/Wolk3");
            

            Objects.Add(new Wolk(wolk1, new Vector2(Graphics.PreferredBackBufferHeight / 4, 0), new Vector2(250, 100),new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight)));
            Objects.Add(new Wolk(wolk2, new Vector2(Graphics.PreferredBackBufferHeight / 4, 150), new Vector2(250, 100), new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight)));
            Objects.Add(new Wolk(wolk3, new Vector2(Graphics.PreferredBackBufferHeight / 4, 300), new Vector2(250, 100), new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight)));

            Block.Arial = Content.Load<SpriteFont>("arial16"); 

            Vector2 field = new Vector2(Graphics.PreferredBackBufferWidth / 4, Graphics.PreferredBackBufferHeight);
            playField = new Rectangle((Graphics.PreferredBackBufferWidth / 2) - ((int)field.X / 2), 0, (int)field.X, (int)field.Y);

            Vector2 field2 = new Vector2(field.X/2, field.Y);
            letterField = new Rectangle(Graphics.PreferredBackBufferWidth / 2 - ((int)field2.X / 2), 0, (int)field2.X, (int)field2.Y);
            widthLengthBlock = letterField.Width / 2;
            heightLengthBlock = letterField.Height / 10;

            InitPlayground();
        }
        public override void Update()
        {
            //if (queueOfLetters.Count <= 0 | queueOfLetters.Count < 21)
            //{
            //    queueOfLetters.Add(GetNextRandomLetter());
            //}
            //if(Block.totalBlocks.Count > 0)
            //{
            //    foreach (Block block in Block.totalBlocks)
            //    {
            //        block
            //    }
            //}
            //List<Block> updatedTotalBlocks = new List<Block>();

            //foreach (Block block in Block.totalBlocks)
            //{
            //    if (block.letter != " ")
            //    {
            //        updatedTotalBlocks.Add(block);
            //    }
            //}

            //foreach (Keys key in Keyboard.PressedKeys)
            //{
            //    if (Keyboard.Check(key))
            //    {
            //        if (Block.totalBlocks[0].letter == key.ToString())
            //        {
            //            Block.totalBlocks[0].letter = "";
            //            if (Block.totalBlocks[0].letter == "")
            //            {
            //                for (int prevPos = 1; prevPos < Block.totalBlocks.Count(); prevPos++)
            //                {
            //                    Block.totalBlocks[prevPos - 1].letter = Block.totalBlocks[prevPos].letter;
            //                }
            //                //***This line of code can be a problem***
            //                Block.totalBlocks[Block.totalBlocks.Length-1].letter = Block.listOfLetters[Block.randomNum.Next(0, Block.listOfLetters.Count - 1)];
            //            }
            //        }
            //    }
            //}
            if (Keyboard.String.Length > 0)
            {
                if (Keyboard.String[0] == Convert.ToChar(Block.totalBlocks[0].letter))
                {
                    Block.totalBlocks[0].letter = "";
                    if (Block.totalBlocks[0].letter == "")
                    {
                        for (int prevPos = 1; prevPos < Block.totalBlocks.Count(); prevPos++)
                        {
                            Block.totalBlocks[prevPos - 1].letter = Block.totalBlocks[prevPos].letter;
                        }
                        //***This line of code can be a problem***
                        Block.totalBlocks[Block.totalBlocks.Length - 1].letter = Block.listOfLetters[Block.randomNum.Next(0, Block.listOfLetters.Count - 1)];
                        Keyboard.String = "";
                        score++;
                    }
                }
                else
                {
                    Keyboard.String = "";
                }
            }
               
            //foreach (Block block in Block.totalBlocks)
            // {
            //     if(Keyboard.PressedKeys)
            //     block.letter
            // }
        }
        public override void Draw()
        {
            //--design--
            View.DrawTextureStretched(bg, new Vector2(0, 0), new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight));
            // View.DrawTexture(bg, new Vector2(0, 0), new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight));
            // View.DrawTexture(bg, new Vector2(0, 0), new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight));
            //-wolkjes-
            //--end design--

            View.DrawText(Block.Arial, "Score: " + score, new Vector2(20, 40));
            View.DrawRectangle(playField, true, Color.Black);
            View.DrawRectangle(letterField, true, Color.Black);
            //foreach (Block block in Block.totalBlocks)
            //{
            //    View.DrawRectangle(block.rectangle, true, Color.Yellow);
            //    View.DrawText(Block.Arial, block.letter, new Vector2(block.rectangle.X+block.rectangle.Width/2, block.rectangle.Y+ block.rectangle.Height/2));

            //   // Block.Arial2.DrawString(Arial, "A", new Vector2(block.rectangle.X + block.rectangle.Width / 2, block.rectangle.Y + block.rectangle.Height / 2), Color.Black);
            //}
            for (int i = 0; i < Block.totalBlocks.Length; i++)
            {
                View.DrawRectangle(Block.totalBlocks[i].rectangle, true, Color.Yellow);
                View.DrawText(Block.Arial, Block.totalBlocks[i].letter, new Vector2(Block.totalBlocks[i].rectangle.X + Block.totalBlocks[i].rectangle.Width / 2, Block.totalBlocks[i].rectangle.Y + Block.totalBlocks[i].rectangle.Height / 2));
            }
            //View.DrawText(Block.Arial,Keyboard.String, new Vector2(100,500))
        }

        public void InitPlayground()
        {
            int arrayPos = 0;
            int InitializeY_value(int value)
            {
                int even = letterField.Y;
                int oneven = letterField.Height / 10;

                //return (value == letterField.X) ? even : oneven;
                if (value == letterField.X)
                {
                    return even;
                }
                else
                {
                    arrayPos = 1;
                    return oneven;
                }
            }

            for (int X = letterField.X; X < (letterField.X + letterField.Width); X = X + letterField.Width / 2)
            {
                for (int Y = InitializeY_value(X); Y < (letterField.Y + letterField.Height); Y = Y + (letterField.Height / 10) * 2)
                {
                    Vector2 startPos = new Vector2(X, Y);
                    Rectangle blockRec = new Rectangle(X, Y, widthLengthBlock, heightLengthBlock);
                    Block currentBlock = new Block(blockRec);

                    if (arrayPos <= 9)
                    {
                        Block.totalBlocks[arrayPos] = currentBlock;
                        arrayPos += 2;
                    }
                }
            }

            Block.ReverseArray();
        }

        public void changeValue(ref int reference, int value)
        {
            reference = value;
        }
    }
}
