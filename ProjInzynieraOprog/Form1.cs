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
        Map M = new Map();
        internal player _playerHuman = new player(1);
        readonly player _playerAi = new player(2);
        private int clickedX;
        private int clickedY;
        
        
        void UprgadeTile()
        {
            if (_playerHuman.PointsBalance >= 300)
            {
                M.Uprgade(clickedX , clickedY);
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
            
            M.CreateMaps(15, 1);
            M.CreateMaps(10, 2);
            M.CreateMaps(5, 3);
            InitializeComponent();
            
            GoFullscreen(true);
            DrawMap_OnPictrurebox();
            this.BackgroundImage = M.load_resource_image("bg_texture.jpg");
            panel1.BackColor = Color.Transparent;

            string fileName = "NEW_TURN.png";
            string path = Path.Combine(Environment.CurrentDirectory, @"Resources", fileName);
            Image new_turn = Image.FromFile(path);
            newTurnButton.Image = new_turn;
        }

        private void
            DrawMap_OnPictrurebox() //tu trzeba użyć lokalnej funkcji do rysowania, w której zawarta jest ta z klasy map XDDD
        {
            pictureBox1.Image = null;
            pictureBox1.Size = new Size(Map.Mapsize * Map.TileSize, Map.Mapsize * Map.TileSize);
            pictureBox1.Image = M.DrawMap(this);
            pictureBox1.Refresh();
        }

        internal void Refresh_Picturebox()
        {
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e) //najcięższa robota
        {
            
            
            check_if_soldiers_sufficient();
            int TS = Map.tileSize;
            int MS = Map.Mapsize;
           
            int x = e.X / TS;
            int y = e.Y / TS;
            
            if(M.attackMode == true && ((x == clickedX && (y >= clickedY-1 || y <= clickedY+1)) || (y == clickedY && (y >= clickedX-1 || y <= clickedX+1))))
            {
                M.TempAttackerId = M.List_of_tiles[clickedX, clickedY].Id;
                M.TempDefenderId = M.List_of_tiles[x, y].Id;
                add_Battle_To_List();
                soldierTrackBar.Maximum -= soldierTrackBar.Value;
            }
            
            clickedX = x;
            clickedY = y;
            int clickX = e.X;
            int clickY = e.Y;


            tileIDtextbox.Text = M.List_of_tiles[x, y].Id.ToString();
            textBox1.Text = M.List_of_tiles[x, y].SoldiersOnTile.ToString();
            textBox2.Text = M.List_of_tiles[x, y].PointGain.ToString();
            M.temppointsbalance = M._playerHuman.PointsBalance;


            DrawMap_OnPictrurebox();
            M.Draw_Frame(x, y);
            pictureBox1.Refresh();

            if (M.List_of_tiles[x, y].PlayerControllerId == 1)
            {
                Enable_if_Friendly();
                soldierTrackBar.Maximum = M.List_of_tiles[x, y].SoldiersOnTile;
            }
            else
            {
                soldierTrackBar.Maximum = 0;
                Disable_if_enemy();
            }

            M.selectedProvince.X = x;
            M.selectedProvince.Y = y;

            M.tempAttackerId = M.List_of_tiles[x, y].Id;

           

            switch (M.List_of_tiles[x, y].Type)
            {
                case 0:
                    pictureBox2.Image = M.load_resource_image("plains-background.png");
                    break;
                case 1:
                    pictureBox2.Image = M.load_resource_image("forest-background.png");
                    break;
                case 2:
                    pictureBox2.Image = M.load_resource_image("lake-background.png");
                    break;
            }



            //disabling attack control buttons if player dont clink on tile he owns or neigbouring tiles are non existent

            if (M.List_of_tiles[x, y].PlayerControllerId != 1)
            {
                buttonS.Enabled = false;
                buttonE.Enabled = false;
                buttonW.Enabled = false;
                buttonN.Enabled = false;
                buttonx1.Visible = false;
                buttonx10.Visible = false;
                buttonx100.Visible = false;
                buttonx1000.Visible = false;
                textBox3.Visible = false;
            }
            else
            {
                if (clickX < TS)
                {
                    buttonW.Enabled = false;
                }
                else
                {
                    buttonW.Enabled = true;
                }

                if (clickY < TS)
                {
                    buttonN.Enabled = false;
                }
                else
                {
                    buttonN.Enabled = true;
                }

                if (clickX + TS > TS * MS)
                {
                    buttonE.Enabled = false;
                }
                else
                {
                    buttonE.Enabled = true;
                }

                if (clickY + TS > TS * MS)
                {
                    buttonS.Enabled = false;
                }
                else
                {
                    buttonS.Enabled = true;
                }
                buttonx1.Visible = true;
                buttonx10.Visible = true;
                buttonx100.Visible = true;
                buttonx1000.Visible = true;
                textBox3.Visible = true;
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

        private void button1_Click(object sender, EventArgs e) // LOAD GAME
        {
            M.First_Run();
            pictureBox1.Refresh();
            DrawMap_OnPictrurebox();
            pictureBox1.Refresh();
            DrawMap_OnPictrurebox();
        }

        private void newTurnButton_Click(object sender, EventArgs e)
        {
            LogBook.Text = "";
            M.playerAiController();
            M.battle_simulation();
            DrawMap_OnPictrurebox();
            AllPointsTextBox.Text = M._playerHuman.PointsBalance.ToString();
            int tmp = M.TurnNumber + 1;
           string temp = "Turn: " + tmp.ToString() + "\n";
            M.LogString1.Add(temp);

            foreach (string s in M.LogString1)
            {
                LogBook.Text += s + "\n";
            }
            M.TurnNumber += 1;


        }
        //event handler for trackbar value change

        private void soldierTrackBar_ValueChanged(object sender, System.EventArgs e)
        {
            textBox1.Text = soldierTrackBar.Value.ToString();
        }

        private void buttonN_Click(object sender, EventArgs e)
        {
            M.TempDefenderId = M.TempAttackerId - 1;
            add_Battle_To_List();
            soldierTrackBar.Maximum -= soldierTrackBar.Value;
        }

        private void buttonW_Click(object sender, EventArgs e)
        {
            M.TempDefenderId = M.TempAttackerId - Map.Mapsize;
            add_Battle_To_List();
            soldierTrackBar.Maximum -= soldierTrackBar.Value;
        }

        private void buttonS_Click(object sender, EventArgs e)
        {
            M.TempDefenderId = M.TempAttackerId + 1;
            add_Battle_To_List();
            soldierTrackBar.Maximum -= soldierTrackBar.Value;
        }

        private void buttonE_Click(object sender, EventArgs e)
        {
            M.TempDefenderId = M.TempAttackerId + Map.Mapsize;
            add_Battle_To_List();
            soldierTrackBar.Maximum -= soldierTrackBar.Value;
        }

        private void add_Battle_To_List()
        {
            battle bat = new battle(M.TempAttackerId, M.TempDefenderId, soldierTrackBar.Value);
            M.battlesToDetermine.Add(bat);
        }

        void populate_listboxSave()
        {
            ListBox_SaveNew.Items.Clear();
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, @"Data\"));
            FileInfo[] Files = dir.GetFiles();
            foreach (FileInfo file in Files)
            {
                ListBox_SaveNew.Items.Add(file.Name);
            }
        }

        private void button_options_Click(object sender, EventArgs e)
        {
            populate_listboxSave();
            buttonE.Visible = false;
            buttonN.Visible = false;
            buttonW.Visible = false;
            buttonS.Visible = false;
            newTurnButton.Visible = false;
            button2.Visible = false;
            soldierTrackBar.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            button_SaveGame.Visible = true;
            button_back.Visible = true;
            button1.Visible = true;
            ListBox_SaveNew.Visible = true;
            textBox_SaveFileName.Visible = true;
            button_NewSave.Visible = true;
            buttonx1.Visible = false;
            buttonx10.Visible = false;
            buttonx100.Visible = false;
            buttonx1000.Visible = false;
            textBox3.Visible = false;
            label1.Visible = false;
            label2.Visible = false; 


            //populate_listboxSave();
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            buttonE.Visible = true;
            buttonN.Visible = true;
            buttonW.Visible = true;
            buttonS.Visible = true;
            newTurnButton.Visible = true;
            button2.Visible = true;
            soldierTrackBar.Visible = true;
            textBox1.Visible = true;
            textBox2.Visible =true;
            label1.Visible=true;
            label2.Visible=true;
            button_back.Visible = false;
            button1.Visible = false;
            button_SaveGame.Visible = false;
            ListBox_SaveNew.Visible = false;
            textBox_SaveFileName.Visible = false;
            button_NewSave.Visible = false;
            buttonx1.Visible = true;
            buttonx10.Visible = true;
            buttonx100.Visible = true;
            buttonx1000.Visible = true;
            textBox3.Visible = true;
        }

        private void button_SaveGame_Click(object sender, EventArgs e)
        {
            if (ListBox_SaveNew.SelectedItem != null)
            {
                string path = Path.Combine(Environment.CurrentDirectory, @"Data\",
                    ListBox_SaveNew.SelectedItem.ToString());
                M.write_save_file(path);
                ListBox_SaveNew.SelectedItem = null;
            }
        }

        private void button_NewSave_Click(object sender, EventArgs e)
        {
            ListBox_SaveNew.Items.Clear();
            string fileName = textBox_SaveFileName.Text + ".txt";
            string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);
            M.write_save_file(path);
            populate_listboxSave();
        }

        void check_if_soldiers_sufficient()
        {
            if (M.Temppointsbalance > 999)
            {
                buttonx1000.Enabled = true;
                buttonx100.Enabled = true;
                buttonx10.Enabled = true;
                buttonx1.Enabled = true;
            }
            else if (M.Temppointsbalance > 99)
            {
                buttonx1000.Enabled = false;
                buttonx100.Enabled = true;
                buttonx10.Enabled = true;
                buttonx1.Enabled = true;
            }
            else if (M.Temppointsbalance > 9)
            {
                buttonx1000.Enabled = false;
                buttonx100.Enabled = false;
                buttonx10.Enabled = true;
                buttonx1.Enabled = true;
            }
            else if (M.Temppointsbalance > 0)
            {
                buttonx1000.Enabled = false;
                buttonx100.Enabled = false;
                buttonx10.Enabled = false;
                buttonx1.Enabled = true;
            }
            else
            {
                buttonx1000.Enabled = false;
                buttonx100.Enabled = false;
                buttonx10.Enabled = false;
                buttonx1.Enabled = false;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int selectedRecruiting = Convert.ToInt32(textBox3.Text);
            M.List_of_tiles[M.selectedProvince.X, M.selectedProvince.Y].SoldiersOnTile += selectedRecruiting;
            M._playerHuman.PointsBalance -= selectedRecruiting;
            textBox3.Text = "0";
            soldierTrackBar.Maximum = M.List_of_tiles[M.selectedProvince.X, M.selectedProvince.Y].SoldiersOnTile;
            textBox1.Refresh();
            string dodanie = "You recruited " + selectedRecruiting.ToString() + " soldiers to the tile";
            M.LogString1.Add(dodanie);
            LogBook.Clear();
            foreach (string s in M.LogString1) 
            {
                LogBook.Text += s + "\n";
            }
            DrawMap_OnPictrurebox();
        }

        private void buttonx1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                int textToInt = Convert.ToInt32(textBox3.Text);
                M.Temppointsbalance -= textToInt;
                textToInt += 1;
                textBox3.Text = textToInt.ToString();
            }
            else
            {
                int textToInt = 0;
                M.Temppointsbalance -= textToInt;
                textToInt += 1;
                textBox3.Text = textToInt.ToString();
            }

            check_if_soldiers_sufficient();
        }

        private void buttonx10_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                int textToInt = Convert.ToInt32(textBox3.Text);
                M.Temppointsbalance -= textToInt;
                textToInt += 10;
                textBox3.Text = textToInt.ToString();
            }
            else
            {
                int textToInt = 0;
                M.Temppointsbalance -= textToInt;
                textToInt += 10;
                textBox3.Text = textToInt.ToString();
            }

            check_if_soldiers_sufficient();
        }

        private void buttonx100_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                int textToInt = Convert.ToInt32(textBox3.Text);
                M.Temppointsbalance -= textToInt;
                textToInt += 10;
                textBox3.Text = textToInt.ToString();
            }
            else
            {
                int textToInt = 0;
                M.Temppointsbalance -= textToInt;
                textToInt += 100;
                textBox3.Text = textToInt.ToString();
            }

            check_if_soldiers_sufficient();
        }

        private void buttonx1000_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                int textToInt = Convert.ToInt32(textBox3.Text);
                M.Temppointsbalance -= textToInt;
                textToInt += 10;
                textBox3.Text = textToInt.ToString();
            }
            else
            {
                int textToInt = 0;
                M.Temppointsbalance -= textToInt;
                textToInt += 1000;
                textBox3.Text = textToInt.ToString();
            }

            check_if_soldiers_sufficient();
        }

         public  string Get_SaveFile()
        {
            if (ListBox_SaveNew.SelectedItem != null)
            {
                return Path.Combine(Environment.CurrentDirectory, @"Data\", ListBox_SaveNew.SelectedItem.ToString());
            }
            else
            {
                string default_path = Path.Combine(Environment.CurrentDirectory, @"Data\", "map1.txt");
                return default_path;
            }
        }

        int Get_SoldierTrackBar()
        {
            return soldierTrackBar.Value;
        }

        private void Disable_if_enemy()
        {
            buttonE.Visible = false;
            buttonN.Visible = false;
            buttonS.Visible = false;
            buttonW.Visible = false;
            button2.Visible = false;
            soldierTrackBar.Visible = false;
        }
        

        void Enable_if_Friendly()
        {
            buttonE.Visible = true;
            buttonN.Visible = true;
            buttonS.Visible = true;
            buttonW.Visible = true;
            button2.Visible = true;
            soldierTrackBar.Visible = true;
        }

        



        //funkcja do ulepszania pól, na click jakiegos buttona;


        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (M.List_of_tiles[clickedX, clickedY].PlayerControllerId == 1)
            {
                DrawMap_OnPictrurebox();

                M.Draw_Frame(clickedX, clickedY + 1);
                M.Draw_Frame(clickedX, clickedY - 1);
                M.Draw_Frame(clickedX - 1, clickedY);
                M.Draw_Frame(clickedX + 1, clickedY);

                pictureBox1.Refresh();

                M.attackMode = true;
            }
        }
    }




}
