﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ProjInzynieraOprog
{
    public partial class Form1 : Form
    {
        private static int tileSize = 45;
        private  player _playerHuman = new player(1);
        readonly player _playerAi = new player(2);
        private static int mapsize = 15;
        SolidBrush allyColor = new SolidBrush(Color.LightSeaGreen);
        SolidBrush enemyColor = new SolidBrush(Color.LightCoral);
        SolidBrush neutralColor = new SolidBrush(Color.Gray);
        private int tempDefenderId;
        private int tempAttackerId;
        private int tileXposition;
        private int tileYposition;
        Bitmap bm = new Bitmap(675, 675);
        //List<tile> listoftiles = new List<tile>();
         tile [,] List_of_tiles = new tile [mapsize,mapsize];
        List <battle> battlesToDetermine = new List<battle>();
        private bool firstRun = true;
        Point selectedProvince;

        private void DrawMap()
        {
            richTextBox1.Text = _playerHuman.PointsBalance.ToString();
            int iter=0;
            //string[] lines=GetMapSize();
            string[,] tiles = GetMapSize();
            pictureBox1.Refresh();
            pictureBox1.Size = new Size(mapsize*tileSize, mapsize*tileSize);
            using (Graphics g = Graphics.FromImage(bm))
            {
                int xWidth = 0;
                int yHeight = 0;
                var shape = new PointF[4];
                
                for (int i = 0; i < mapsize; i++)
                {
                    for (int j = 0; j < mapsize; j++)
                    {
                        if (firstRun == true)
                        {
                            tile b = new tile();
                            // b.Id = new Point(i * tileSize, j * tileSize);
                            b.Id = iter;
                            iter++;
                            //listoftiles.Add(b);
                            List_of_tiles[i, j] = b;
                            //List_of_tiles[i, j].PointGain = 15;
                        
                            List_of_tiles[i, j].PointGain  = int.Parse(tiles[i,j].Substring(0,2));
                            List_of_tiles[i, j].Type  = int.Parse(tiles[i,j].Substring(3,1));
                            List_of_tiles[i, j].PlayerControllerId  = int.Parse(tiles[i,j].Substring(5,1));
                        }
                        if (List_of_tiles[i, j].PlayerControllerId == 1)
                        {
                            g.FillRectangle(allyColor, i * tileSize+1, j * tileSize+1, tileSize-1, tileSize-1);
                        }
                        else if (List_of_tiles[i, j].PlayerControllerId == 2)
                        {
                            g.FillRectangle(enemyColor, i * tileSize+1, j * tileSize+1, tileSize-1, tileSize-1);
                        }
                        else
                        {
                            g.FillRectangle(neutralColor, i * tileSize+1, j * tileSize+1, tileSize-1, tileSize-1);
                        }
                        
                        if (List_of_tiles[i, j].Type == 2)
                        {
                            List_of_tiles[i, j].PointGain = 0;
                            string fileName = "WATER.png";
                            string path = Path.Combine(Environment.CurrentDirectory, @"Resources\", fileName);
                            Image waterimg = new Bitmap(path);
                            g.DrawImage(waterimg, i * tileSize + 1, j * tileSize + 1, tileSize - 1, tileSize - 1);

                        }


                        if (List_of_tiles[i, j].Type == 1)
                        {
                            string fileName = "FOREST.png";
                            string path = Path.Combine(Environment.CurrentDirectory, @"Resources\", fileName);
                            Image villageimg = Image.FromFile(path);
                            g.DrawImage(villageimg, i * tileSize + 1, j * tileSize + 1, tileSize-1 , tileSize-1 );
                        }


                        if (List_of_tiles[i, j].Type == 0)
                        {
                            string fileName = "BUMP.png";
                            string path = Path.Combine(Environment.CurrentDirectory, @"Resources\", fileName);
                            Image wheatimg = Image.FromFile(path);

                            g.DrawImage(wheatimg, i * tileSize + 1, j * tileSize + 1, tileSize - 1, tileSize - 1);
                        }
                        
                        shape[0] = new PointF(xWidth, yHeight);
                        xWidth = xWidth + tileSize;
                        shape[1] = new PointF(xWidth, yHeight);
                        yHeight = yHeight + tileSize;
                        shape[2] = new PointF(xWidth, yHeight);
                        xWidth = xWidth - tileSize;
                        shape[3] = new PointF(xWidth, yHeight);
                        g.DrawPolygon(Pens.Black, shape);
                        yHeight = yHeight - tileSize;
                        xWidth = xWidth + tileSize;
                    }
                    xWidth = 0;
                    yHeight = yHeight + tileSize;
                }

                firstRun = false;
                pictureBox1.Image = bm;
            }
        }

        
        private void GoFullscreen(bool fullscreen)
        {
            if (fullscreen)
            {
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.None;
                Bounds = Screen.PrimaryScreen.Bounds;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.Sizable;
            }
        }


        public Form1()
        {
            CreateMaps(15,1);
            CreateMaps(10,2);
            CreateMaps(5,3);
            InitializeComponent();
            GoFullscreen(true);
            DrawMap();
        }



        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            
                int x = e.X / tileSize;
                int y = e.Y / tileSize;
                int clickX = e.X;
                int clickY = e.Y;
                tileXposition = x * tileSize;
                tileYposition = y * tileSize;
                selectedProvince.X = x;
                selectedProvince.Y = y;
                richTextBox_coordinates.Text = List_of_tiles[x,y].Id.ToString();
                richTextBox_PointGain.Text = List_of_tiles[x,y].PointGain.ToString();
                soldierTrackBar.Maximum = List_of_tiles[x, y].SoldiersOnTile;
                soldierTrackBar.Minimum = 1;
                tempAttackerId = List_of_tiles[x, y].Id;
                textBox2.Text = List_of_tiles[x, y].SoldiersOnTile.ToString();
                //disabling attack control buttons if player dont clink on tile he owns or neigbouring tiles are non existent
              
                
                if (List_of_tiles[x, y].PlayerControllerId != 1)
               {
                    buttonS.Enabled = false;
                    buttonE.Enabled = false;
                    buttonW.Enabled = false;
                    buttonN.Enabled = false;
                }
                else
               {
                    if (clickX < tileSize)
                    {
                        buttonW.Enabled = false;
                    }
                    else
                    {
                        buttonW.Enabled = true;
                    }
                    
                    if (clickY < tileSize)
                    {
                        buttonN.Enabled = false;
                    }
                    else
                    {
                        buttonN.Enabled = true;
                    }

                    if (clickX + tileSize > tileSize * mapsize)
                    {
                        buttonE.Enabled = false;
                    }
                    else
                    {
                        buttonE.Enabled = true;
                    }

                    if (clickY + tileSize > tileSize * mapsize)
                    {
                        buttonS.Enabled = false;
                    }
                    else
                    {
                        buttonS.Enabled = true;
                    }
               }

            
        }


        private void button_exit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure?", "Closing the game", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button_attack_Click(object sender, EventArgs e)
        {
            SolidBrush EnemyColor = new SolidBrush(Color.Red);
            using (var g = Graphics.FromImage(pictureBox1.Image))
            {
                g.FillRectangle(EnemyColor, tileXposition + 1, tileYposition + 1, tileSize - 1, tileSize - 1);
                pictureBox1.Refresh();
            }
        }

        void CreateMaps(int size,int map)
        {
            string name = "map" + map.ToString() + ".txt";
            string path = Path.Combine(Environment.CurrentDirectory, @"Data\", name);
            StreamWriter sw;
            if (!File.Exists(path))
            {
                sw = File.CreateText(path);
            }
            sw = new StreamWriter(path, false);
            Random r = new Random();
            string line;
            int pg; //assigning random pointgain for each tile// 
            int type; //0-clearing , 1-forest, 2-water //
            int own; //ownership //
            for (int i = 0; i < size; i++)
            {
              
                
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < size; j++)
                {
                    pg = r.Next(10, 30);
                    type = r.Next(0, 3);
                    if (i < 2 && j < 2)
                    {
                        own = 1;
                    }
                    else if (i > size - 3 && j > size - 3)
                    {
                        own = 2;
                    }
                    else
                    {
                        own = 0;
                    }
                    sb.Append(pg.ToString()+"|"+type.ToString()+"|"+own.ToString()+"#");
                }
                line = sb.ToString();
                sw.WriteLine(line);
            }
            sw.Close();
        }

        string [,] GetMapSize()
          {
              string[,] elo = new string[15,15];

            int iterator = 0;
            if (radioButton1.Checked)
              {
                  string[,] Tiles = new string[15,15]; 
                  string fileName = "map1.txt"; 
                  string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName); 
                  mapsize = 15; 
                  string[] lines = File.ReadAllLines(path, Encoding.UTF8);


               
              
                for (int i = 0; i < 15; i++) 
                  {
                    iterator = 0;
                    for (int j = 0; j < 15; j++)
                    {

                        
                        Tiles[i, j] = lines[i].Substring(iterator, 6);
                        iterator += 7;
                       
                      }

                  
                  } 
                  return Tiles;
              }
             else if (radioButton2.Checked)
             {
                 string fileName = "map2.txt";
                 string[,] Tiles = new string[10,10]; 
                 string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName); 
                 mapsize = 10; 
                 string[] lines = File.ReadAllLines(path, Encoding.UTF8); 
                 for (int i = 0; i < 10; i++) 
                 {
                    iterator = 0;
                     for (int j = 0; j < 10; j++) 
                     { 
                        
                         Tiles [i, j] = lines[i].Substring(iterator,6); 
                         iterator += 7;
                     }
                 } 
                 return Tiles;
             }
             else if (radioButton3.Checked)
             {
                 string fileName = "map3.txt";
                 string[,] Tiles = new string[5,5]; 
                 string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName); 
                 mapsize = 5; 
                 string[] lines = File.ReadAllLines(path, Encoding.UTF8); 
                 for (int i = 0; i < 5; i++) 
                 {
                    iterator = 0;
                     for (int j = 0; j < 5; j++) 
                     { 
                         
                         Tiles [i, j] = lines[i].Substring(iterator,  6); 
                         iterator += 7;
                     }
                 } 
                 return Tiles;
             }
             return elo;
          }


        private void button1_Click(object sender, EventArgs e)
        {
            firstRun = true;
            DrawMap();
        }
        

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            recrutingTrackBar.Maximum = _playerHuman.PointsBalance;
            for (int i = 0; i < mapsize; i++)
            {
                for (int j = 0; j < mapsize; j++)
                {
                    if (List_of_tiles[i, j].PlayerControllerId == 1)
                    {
                        _playerHuman.PointsBalance += Convert.ToInt32(List_of_tiles[i, j].PointGain);
                    }
                    else if (List_of_tiles[i, j].PlayerControllerId == 2)
                    {
                        _playerAi.PointsBalance += Convert.ToInt32(List_of_tiles[i, j].PointGain);
                    }

                }
            }
            //basic battle simulation

            int defenders;  
            
            for (int k = 0; k < battlesToDetermine.Count;k++)
            {
                defenders = get_soldier_num_by_id(battlesToDetermine[k].DefenderProvinceId);
                if (checkIfFriendly(tempDefenderId) == true)
                {
                    for (int i = 0; i < 0; i++)
                    {
                        for (int j = 0; j < 0; j++)
                        {
                            if (battlesToDetermine[k].AttackerProvinceId == List_of_tiles[i, j].Id)
                            {
                                for (int x = 0; x < 0; x++)
                                {
                                    for (int z = 0; z < 0; z++)
                                    {
                                        if (battlesToDetermine[k].DefenderProvinceId == List_of_tiles[x, z].Id)
                                        {
                                            List_of_tiles[x, z].SoldiersOnTile += battlesToDetermine[k].SoldierNum;
                                        }
                                    }
                                }

                                List_of_tiles[i, j].SoldiersOnTile -= battlesToDetermine[k].SoldierNum;
                            }
                        }
                    }
                }
                else
                {
                    if (defenders<battlesToDetermine[k].SoldierNum)
                    {
                        battlesToDetermine[k].SoldierNum -= defenders;
                        for (int i = 0; i < mapsize; i++)
                        {
                            for (int j = 0; j < mapsize; j++)
                            {
                                if (List_of_tiles[i, j].Id == battlesToDetermine[k].DefenderProvinceId)
                                {
                                    List_of_tiles[i, j].SoldiersOnTile = battlesToDetermine[k].SoldierNum;
                                    List_of_tiles[i, j].PlayerControllerId = 1;
                                }
                            }
                        }
                    }
                    else
                    {
                    
                        for (int i = 0; i < mapsize; i++)
                        {
                            for (int j = 0; j < mapsize; j++)
                            {
                                if (List_of_tiles[i, j].Id == battlesToDetermine[k].DefenderProvinceId)
                                {
                                    List_of_tiles[i, j].SoldiersOnTile -= defenders;
                                }
                            }
                        }
                    }
                }
                }
              
            richTextBox1.Refresh();
            battlesToDetermine.Clear();
            DrawMap();

        }
        //event handler for trackbar value change
        
        private void soldierTrackBar_ValueChanged(object sender, System.EventArgs e)
        {
            textBox1.Text = soldierTrackBar.Value.ToString();
        }

        private void buttonN_Click(object sender, EventArgs e)
        {
            tempDefenderId = tempAttackerId - 1;
            add_Battle_To_List();
        }

        private void buttonW_Click(object sender, EventArgs e)
        {
            tempDefenderId = tempAttackerId - mapsize;
            add_Battle_To_List();
        }

        private void buttonS_Click(object sender, EventArgs e)
        {
            tempDefenderId = tempAttackerId + 1;
            add_Battle_To_List();
        }

        private void buttonE_Click(object sender, EventArgs e)
        {
            tempDefenderId = tempAttackerId +mapsize;
            add_Battle_To_List();
        }

        private void add_Battle_To_List()
        {
            battle bat = new battle(tempAttackerId,tempDefenderId,soldierTrackBar.Value);
            soldierTrackBar.Maximum -= soldierTrackBar.Value;
            battlesToDetermine.Add(bat);
        }
        
        int get_soldier_num_by_id(int defid)
        {
            int solCount;
            for (int i = 0; i < mapsize; i++)
            {
                for (int j = 0; j < mapsize; j++)
                {
                    if (List_of_tiles[i, j].Id == defid)
                    {
                        solCount = List_of_tiles[i, j].SoldiersOnTile;
                        return solCount;
                    }
                }
            }
            return 0;
        }
//check if attacked tile is friendly
        bool checkIfFriendly(int id)
        {
            for (int i = 0; i < mapsize; i++)
            {
                for (int j = 0; j < mapsize; j++)
                {
                    if (id == List_of_tiles[i, j].Id)
                    {
                        if (List_of_tiles[i, j].PlayerControllerId == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List_of_tiles[selectedProvince.X, selectedProvince.Y].SoldiersOnTile += recrutingTrackBar.Value;
                    _playerHuman.PointsBalance -= recrutingTrackBar.Value;
                    recrutingTrackBar.Maximum -= recrutingTrackBar.Value;
        }
    }
}
