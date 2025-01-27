﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ProjInzynieraOprog
{
    internal class Map
    {
        internal static int tileSize = 45;
        internal player _playerHuman = new player(1);
        readonly player _playerAi = new player(2);
        private static int mapsize = 15;
        

        SolidBrush allyColor = new SolidBrush(Color.DodgerBlue);
        SolidBrush enemyColor = new SolidBrush(Color.LightCoral);
        SolidBrush neutralColor = new SolidBrush(Color.Gray);
        internal int tempDefenderId;
        internal int tempAttackerId;
        static Bitmap bm = new Bitmap(tileSize * mapsize, tileSize * mapsize);
        internal Tile [,] List_of_tiles = new Tile [mapsize,mapsize];
        internal List <battle> battlesToDetermine = new List<battle>();
        private bool firstRun = true;
        internal Point selectedProvince;
        private OpenFileDialog ofd;
        Graphics g = Graphics.FromImage(bm);
        internal int temppointsbalance;
        List<string> LogString = new List<string>();
        private int turnNumber = 0;
        internal bool attackMode;



        public int Temppointsbalance
        {
            get => temppointsbalance;
            set => temppointsbalance = value;
        }

        public static int TileSize
        {
            get => tileSize;
            set => tileSize = value;
        }

        public static int Mapsize
        {
            get => mapsize;
            set => mapsize = value;
        }

        public int TempDefenderId
        {
            get => tempDefenderId;
            set => tempDefenderId = value;
        }

        public int TempAttackerId
        {
            get => tempAttackerId;
            set => tempAttackerId = value;
        }
        public List<string> LogString1 { get => LogString; set => LogString = value; }
        public int TurnNumber { get => turnNumber; set => turnNumber = value; }

        internal void Draw_Grass(int x, int y)
        {
            string fileName = "GRASS_NEW.png";
            string path = Path.Combine(Environment.CurrentDirectory, @"Resources", fileName);
            Image wheatimg = Image.FromFile(path);

            g.DrawImage(wheatimg, x * tileSize + 1, y * tileSize + 1, tileSize - 1, tileSize - 1);
        }
        
        
        internal void Draw_Forest(int x, int y)
        {
            Draw_Grass(x, y);

            
               

                switch (List_of_tiles[x,y].Foresttype)
                {
                    case 1:

                        string fileName = "FOREST.png";
                        string path = Path.Combine(Environment.CurrentDirectory, @"Resources", fileName);
                        Image forest1 = Image.FromFile(path);
                        g.DrawImage(forest1, x * tileSize + 1, y * tileSize + 1, tileSize - 1, tileSize - 1);

                        break;
                    case 2:
                        string fileName2 = "FOREST2.png";
                        string path2 = Path.Combine(Environment.CurrentDirectory, @"Resources", fileName2);
                        Image forest2 = Image.FromFile(path2);
                        g.DrawImage(forest2, x * tileSize + 1, y * tileSize + 1, tileSize - 1, tileSize - 1);
                        break;
                    case 3:
                        string fileName3 = "FOREST3.png";
                        string path3 = Path.Combine(Environment.CurrentDirectory, @"Resources", fileName3);
                        Image forest3 = Image.FromFile(path3);
                        g.DrawImage(forest3, x * tileSize + 1, y * tileSize + 1, tileSize - 1, tileSize - 1);
                        break;
                    case 4:
                        string fileName4 = "FOREST4.png";
                        string path4 = Path.Combine(Environment.CurrentDirectory, @"Resources", fileName4);
                        Image forest4 = Image.FromFile(path4);
                        g.DrawImage(forest4, x * tileSize + 1, y * tileSize + 1, tileSize - 1, tileSize - 1);
                        break;
                    case 5:
                        string fileName5 = "FOREST5.png";
                        string path5 = Path.Combine(Environment.CurrentDirectory, @"Resources", fileName5);
                        Image forest5 = Image.FromFile(path5);
                        g.DrawImage(forest5, x * tileSize + 1, y * tileSize + 1, tileSize - 1, tileSize - 1);
                        break;

                
            }
            


           
        }

        internal void Draw_Lake(int x, int y)
        {
            Draw_Grass(x, y);
            List_of_tiles[x, y].PointGain = 0;
            string fileName = "LAKE1.png";
            string path = Path.Combine(Environment.CurrentDirectory, @"Resources", fileName);
            Image waterimg = new Bitmap(path);
            g.DrawImage(waterimg, x * tileSize + 1, y * tileSize + 1, tileSize - 1, tileSize - 1);
        }

        internal void Draw_City(int x, int y) 
        {
            Draw_Grass(x, y);
            string fileName = "CITY.png";
            string path = Path.Combine(Environment.CurrentDirectory, @"Resources", fileName);
            Image cityimg = new Bitmap(path);
            g.DrawImage(cityimg, x * tileSize + 1, y * tileSize + 1, tileSize - 1, tileSize - 1);

        }

        internal void Draw_Lumberjack(int x, int y)
        {
            Draw_Grass(x, y);
            string fileName = "LUMBERJACK.png";
            string path = Path.Combine(Environment.CurrentDirectory, @"Resources", fileName);
            Image lumberjackimg = new Bitmap(path);
            g.DrawImage(lumberjackimg, x * tileSize + 1, y * tileSize + 1, tileSize - 1, tileSize - 1);

        }

        internal void Draw_Field(int x, int y)
        {
            Draw_Grass(x, y);
            string fileName = "FIELD.png";
            string path = Path.Combine(Environment.CurrentDirectory, @"Resources", fileName);
            Image fieldimg = new Bitmap(path);
            g.DrawImage(fieldimg, x * tileSize + 1, y * tileSize + 1, tileSize - 1, tileSize - 1);

        }







        internal void soldiers_on_tile(int x, int y)
        {
            if (List_of_tiles[x, y].SoldiersOnTile > 0)
            {
                g.DrawString(List_of_tiles[x, y].SoldiersOnTile.ToString(), new Font("Arial", 8), Brushes.Black, x * tileSize + 5, y * tileSize + 5);
            }
        }
        
        bool checkIfFriendly(int iddef, int idatc)
        {
            for (int i = 0; i < mapsize; i++)
            {
                for (int j = 0; j < mapsize; j++)
                {
                    if (List_of_tiles[i, j].PlayerControllerId == iddef)
                    {
                        for(int k=0; k<mapsize; k++)
                        {
                            for(int l=0; l<mapsize; l++)
                            {
                                if(List_of_tiles[k, l].PlayerControllerId == idatc)
                                {
                                    if(List_of_tiles[k,l].PlayerControllerId==List_of_tiles[i,j].PlayerControllerId)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;

        }

        internal Image load_resource_image(string filename)
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"Resources", filename);
            return  Image.FromFile(path);
        }

        public void DrawFrameTileTaken(int x, int y)
        { g.DrawRectangle(Pens.Red, tileSize * x, tileSize * y, tileSize, tileSize);
        g.DrawRectangle(Pens.Red, tileSize * x + 1, tileSize * y + 1, tileSize - 2, tileSize - 2);
        g.DrawRectangle(Pens.Red, tileSize * x + 2, tileSize * y + 2, tileSize - 4, tileSize - 4);
        }
        internal void Draw_Frame(int x, int y)
        {
            g.DrawRectangle(Pens.MediumBlue, tileSize * x, tileSize * y, tileSize, tileSize);
            g.DrawRectangle(Pens.MediumBlue, tileSize * x + 1, tileSize * y + 1, tileSize - 2, tileSize - 2);
            g.DrawRectangle(Pens.MediumBlue, tileSize * x + 2, tileSize * y + 2, tileSize - 4, tileSize - 4);
        }
        
        internal void CreateMaps(int size,int map)
        {
            string name = "map" + map.ToString() + ".txt";
            string path = Path.Combine(Environment.CurrentDirectory, @"Data", name);
            StreamWriter sw;
            sw = new StreamWriter(path, false);
            Random r = new Random();
            string line;
            int pg; //assigning random pointgain for each tile// 
            int type;//0-clearing , 1-forest, 2-water   4- city//
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

                    if(i==1 && j==1)
                    {
                        type=4;
                    }
                    if(i==size-2 && j==size-2)
                    {
                        type=4;
                    }

                    sb.Append(pg.ToString()+"|"+type.ToString()+"|"+own.ToString()+"|0|0|0#");
                }
                line = sb.ToString();
                sw.WriteLine(line);
            }
            sw.Close();
        }
        
        internal string [,] GetMapSize(Form1 form)
        {
            int iterator = 0;
            string path = Path.Combine(Environment.CurrentDirectory, @"Data", "map1.txt");;
            mapsize = 15;
            if (form.Get_SaveFile() != null)
            {
                path = form.Get_SaveFile();
            }
            string[,] Tiles = new string[mapsize, mapsize];

            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            mapsize = lines.Length;

            for (int i = 0; i < mapsize; i++) 
            {
                iterator = 0;
                for (int j = 0; j < mapsize; j++)
                {
                    int index = lines[i].IndexOf('#',iterator);
                    Tiles[i, j] = lines[i].Substring(iterator, index-iterator+1);
                    iterator = index + 1;
                }
            } 
            return Tiles;
        }


        internal Bitmap DrawMap(Form1 form)
        {
            int iter = 0;
            string[,] tiles = GetMapSize(form);
            for (int i = 0; i < mapsize; i++)
            {
                for (int j = 0; j < mapsize; j++)
                {
                    if (firstRun == true)
                    {
                        Tile b = new Tile();
                        b.Id = iter;
                        iter++;
                        List_of_tiles[i, j] = b;
                        List_of_tiles[i, j].PointGain = int.Parse(tiles[i, j].Substring(0, 2));
                        List_of_tiles[i, j].Type = int.Parse(tiles[i, j].Substring(3, 1));
                        List_of_tiles[i, j].PlayerControllerId = int.Parse(tiles[i, j].Substring(5, 1));
                        List_of_tiles[i, j].isUpgraded1 = Convert.ToBoolean(int.Parse(tiles[i,j].Substring(7,1)));
                        List_of_tiles[i, j].Level = int.Parse(tiles[i, j].Substring(9, 1));
                        List_of_tiles[i, j].SoldiersOnTile = 
                            int.Parse(tiles[i, j].Substring(11, tiles[i, j].IndexOf('#') - 11));
                    }

                    if (List_of_tiles[i, j].PlayerControllerId == 1)
                    {
                        g.FillRectangle(allyColor, i * tileSize + 1, j * tileSize + 1, tileSize - 1, tileSize - 1);
                        
                    }
                    else if (List_of_tiles[i, j].PlayerControllerId == 2)
                    {
                        g.FillRectangle(enemyColor, i * tileSize + 1, j * tileSize + 1, tileSize - 1, tileSize - 1);
                    }
                    else
                    {
                        g.FillRectangle(neutralColor, i * tileSize + 1, j * tileSize + 1, tileSize - 1, tileSize - 1);
                    }

                    if (List_of_tiles[i, j].Type == 2)
                    {
                        Draw_Lake(i, j);
                    }
                    
                    if(List_of_tiles[i,j].Type == 4)
                    {
                        Draw_City(i, j);
                    }

                    if (List_of_tiles[i, j].Type == 1)
                    {
                        if (firstRun) 
                        {
                            Random ft = new Random();
                            List_of_tiles[i,j].Foresttype = ft.Next(1, 5);
                        }


                        if (List_of_tiles[i, j].isUpgraded1)
                        {
                            Draw_Lumberjack(i, j);
                        }
                        else 
                        {
                            Draw_Forest(i, j);

                        }

                       
                    }

                    if (List_of_tiles[i, j].Type == 0)
                    {
                        Draw_Grass(i, j);

                        if (List_of_tiles[i, j].isUpgraded1)
                        {
                            Draw_Field(i, j);
                        }
                        else
                        {
                            Draw_Grass(i, j);

                        }

                    }
                    soldiers_on_tile(i,j);
                    g.DrawRectangle(Pens.Black, tileSize * i, tileSize * j, tileSize, tileSize);
                }
            }
            firstRun = false;
            return bm;
        }

        internal void write_save_file(string path)
        {
            StreamWriter sw;
            string line;

            sw = new StreamWriter(path, false);
            for (int x = 0; x < mapsize; x++)
            {
                StringBuilder sb = new StringBuilder();
                for (int y = 0; y < mapsize; y++)
                {
                    if (List_of_tiles[x, y].PointGain == 0)
                    {
                        List_of_tiles[x, y].PointGain = 99;
                    }
                    sb.Append(List_of_tiles[x,y].PointGain.ToString()+"|"+List_of_tiles[x,y].Type.ToString()+"|"+List_of_tiles[x,y].PlayerControllerId.ToString()+"|"+Convert.ToInt32(List_of_tiles[x,y].isUpgraded1)+
                              "|"+List_of_tiles[x,y].Level.ToString()+"|"+List_of_tiles[x,y].SoldiersOnTile.ToString()+"#");
                }
                line = sb.ToString();
                sw.WriteLine(line);
            }
            sw.Close();
        }

        public void First_Run()
        {
            firstRun = true;
            
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

        public struct Flash
        {
            public int x;
            public int y;
        }
         public List<Flash> flashes = new List<Flash>();

        public void battle_simulation()
        {
            string adding = null;
            int defenderX = 0;
            int defenderY = 0;
            int attackerX = 0;
            int attackerY = 0;


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


            int attackerID = 0;
            int defenderID = 0;

            for (int k = 0; k < battlesToDetermine.Count; k++)
            {
                for (int i = 0; i < mapsize; i++)
                {
                    for (int j = 0; j < mapsize; j++)
                    {
                        if (battlesToDetermine[k].AttackerProvinceId == List_of_tiles[i, j].Id)
                        {
                            attackerID = List_of_tiles[i, j].PlayerControllerId;
                            attackerX = i;
                            attackerY = j;
                            break;
                        }
                    }
                }

                for (int i = 0; i < mapsize; i++)
                {
                    for (int j = 0; j < mapsize; j++)
                    {
                        if (battlesToDetermine[k].DefenderProvinceId == List_of_tiles[i, j].Id)
                        {
                            defenderID = List_of_tiles[i, j].PlayerControllerId;
                            defenderX = i;
                            defenderY = j;
                            break;
                        }
                    }
                }




                if (defenderID == attackerID)
                {
                    List_of_tiles[defenderX, defenderY].SoldiersOnTile += battlesToDetermine[k].SoldierNum;
                }
                else
                {
                    
                    if (attackerID == _playerHuman.PlayerId1)
                    {
                        adding = "You attacked field " + battlesToDetermine[k].AttackerProvinceId +
                                 " and lost ";
                    }
                    else if (defenderID == _playerHuman.PlayerId1)
                    {
                        adding = "You had been attacked at field" + battlesToDetermine[k].DefenderProvinceId +
                                 " and lost ";
                    }

                    if (List_of_tiles[defenderX, defenderY].SoldiersOnTile > battlesToDetermine[k].SoldierNum)
                    {
                        int temp = List_of_tiles[attackerX, attackerY].SoldiersOnTile - battlesToDetermine[k].SoldierNum;
                        List_of_tiles[attackerX, attackerY].SoldiersOnTile -= battlesToDetermine[k].SoldierNum;
                        if (attackerID == _playerHuman.PlayerId1)
                        {
                            adding += " all soldiers and LOST the battle";
                            LogString1.Add(adding);
                        }
                        else if (defenderID == _playerHuman.PlayerId1)
                        {
                            adding += temp + " soldiers, you WON the battle ";
                            LogString1.Add(adding);
                        }
                        
                    }
                    else
                    {
                        if (attackerID == _playerAi.PlayerId1)
                        {
                            Flash f = new Flash();
                            f.x = defenderX;
                            f.y = defenderY;
                            flashes.Add(f);
                        }
                        int lost_soldiers;
                        int temp;
                        //List_of_tiles[z, y].SoldiersOnTile -= battlesToDetermine[k].SoldierNum;
                        temp = battlesToDetermine[k].SoldierNum - List_of_tiles[defenderX, defenderY].SoldiersOnTile;
                        lost_soldiers = battlesToDetermine[k].SoldierNum - temp;
                        List_of_tiles[defenderX, defenderY].SoldiersOnTile = temp;
                        List_of_tiles[defenderX, defenderY].PlayerControllerId = attackerID;

                        if (attackerID == _playerHuman.PlayerId1)
                        {
                            adding += lost_soldiers + " soldiers and WON the battle";
                            LogString1.Add(adding);
                        }
                        else if (defenderID == _playerHuman.PlayerId1)
                        {
                            adding += " all soldiers, tile has been lost";
                            LogString1.Add(adding);
                        }

                    }
                }
            }

           

            
            
            battlesToDetermine.Clear();
        }

        


        public void Uprgade(int clickedX, int clickedY)
        {
            List_of_tiles[clickedX,clickedY].isUpgraded1 = true;
        }
        
        struct Provinces
        {
          public int defender_id;       
          public int attacker_id;
          public int soldiers;
        }

        void AiUpgrade()
        {
            if (_playerAi.SavedPoints > 100)
            {
                for (var i = mapsize; i-- > 0;)
                {
                    for (var j =mapsize;  j-- > 0;)
                    {
                        if (List_of_tiles[i, j].PlayerControllerId == _playerAi.PlayerId1)
                        {
                            if (List_of_tiles[i, j].PointGain * 3 < _playerAi.SavedPoints)
                            {
                                if (List_of_tiles[i, j].Type != 4 && List_of_tiles[i, j].Type != 2 && List_of_tiles[i,j].isUpgraded1==false)
                                {
                                    _playerAi.PointsBalance -= List_of_tiles[i, j].PointGain * 3;
                                    _playerAi.SavedPoints -= List_of_tiles[i, j].PointGain * 3;
                                    List_of_tiles[i, j].UpgradeTile();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        void AiUpgradeReversed()
        {
            if (_playerAi.SavedPoints > 100)
            {
                for (var j = mapsize; j-- > 0;)
                {
                    for (var i =mapsize;  i-- > 0;)
                    {
                        if (List_of_tiles[i, j].PlayerControllerId == _playerAi.PlayerId1)
                        {
                            if (List_of_tiles[i, j].PointGain * 3 < _playerAi.SavedPoints)
                            {
                                if (List_of_tiles[i, j].Type != 4 && List_of_tiles[i, j].Type != 2 && List_of_tiles[i,j].isUpgraded1==false)
                                {
                                    _playerAi.PointsBalance -= List_of_tiles[i, j].PointGain * 3;
                                    _playerAi.SavedPoints -= List_of_tiles[i, j].PointGain * 3;
                                    List_of_tiles[i, j].UpgradeTile();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        private bool a= false;
        
        
       public void playerAiController()
        {
            if (a == false)
            {
                AiUpgrade();
                a = true;
            }
            else
            {
                AiUpgradeReversed();
                a = false;
            }

            List<Provinces> provinces = new List<Provinces>();

            int iterator = 0;
            //sprawdza ile prowincji graniczy z neutralnymi/wrogimi
            
            int counter = 0;
            
            for (int i = 0; i < mapsize; i++)
            {
                for (int j = 0; j < mapsize; j++)
                {
                    if (List_of_tiles[i, j].PlayerControllerId == 2)
                    {
                        
                        if (i != 0)
                        {
                            if(List_of_tiles[i-1,j].PlayerControllerId !=2)
                            {
                                Provinces p= new Provinces();
                                p.attacker_id = List_of_tiles[i, j].Id;
                                p.defender_id = List_of_tiles[i - 1, j].Id;
                                provinces.Add(p);
                                counter++;
                            }
                        }

                        if (i != mapsize-1)
                        {
                            if(List_of_tiles[i+1,j].PlayerControllerId !=2)
                            {
                                Provinces p= new Provinces();
                                p.attacker_id = List_of_tiles[i, j].Id;
                                p.defender_id = List_of_tiles[i + 1, j].Id;
                                provinces.Add(p);
                                counter++;
                                
                            }
                        }
                        if (j != 0)
                        {
                            if(List_of_tiles[i,j-1].PlayerControllerId !=2)
                            {
                                Provinces p= new Provinces();
                                p.attacker_id = List_of_tiles[i, j].Id;
                                p.defender_id = List_of_tiles[i, j - 1].Id;
                                provinces.Add(p);
                                counter++;
                            }
                        }
                        if (j != mapsize-1)
                        {
                            if(List_of_tiles[i,j+1].PlayerControllerId !=2)
                            {
                                Provinces p= new Provinces();
                                p.attacker_id = List_of_tiles[i, j].Id;
                                p.defender_id = List_of_tiles[i, j + 1].Id;
                                provinces.Add(p);
                                counter++;
                            }
                        }
                    }
                }
            }
            
            int overallPoints = _playerAi.PointsBalance;
            int pointToSave;
            double pointsToSpend;

            pointsToSpend = overallPoints*0.7;
            int temp = (int)pointsToSpend;
            pointsToSpend/=counter;
            pointsToSpend  = Math.Floor(pointsToSpend);
            int points = Convert.ToInt32(pointsToSpend);
            pointToSave = overallPoints - temp;
            _playerAi.SavedPoints += pointToSave;
          
       
            
            
            
            //rekrutuje wojska na prowincjach graniczacych z neutralnymi/wrogimi
            
            for (int i = 0; i < mapsize; i++)
            {
                for (int j = 0; j < mapsize; j++)
                {
                    if (List_of_tiles[i, j].PlayerControllerId == 2)
                    {
                        if (i != 0)
                        {
                            if(List_of_tiles[i-1,j].PlayerControllerId !=2)
                            {
                                List_of_tiles[i,j].SoldiersOnTile=0;
                                _playerAi.PointsBalance -= points;
                                //List_of_tiles[i,j].SoldiersOnTile += points;
                            }
                        }
                        
                        if(i!=mapsize-1)
                            if(List_of_tiles[i+1,j].PlayerControllerId !=2)
                            {
                                List_of_tiles[i,j].SoldiersOnTile=0;
                                _playerAi.PointsBalance -= points;
                                //List_of_tiles[i,j].SoldiersOnTile += points;                            
                            }
                        
                        if (j != 0)
                        {
                            if(List_of_tiles[i,j-1].PlayerControllerId !=2)
                            {
                                List_of_tiles[i,j].SoldiersOnTile=0;
                                _playerAi.PointsBalance -= points;
                               // List_of_tiles[i,j].SoldiersOnTile += points;
                            }
                        }
                        if (j != mapsize-1)
                        {
                            if(List_of_tiles[i,j+1].PlayerControllerId !=2)
                            {
                                List_of_tiles[i,j].SoldiersOnTile=0;
                                _playerAi.PointsBalance -= points;
                                //List_of_tiles[i,j].SoldiersOnTile += points;
                            }
                        }
                    }
                       
                }
            }


           for(int i=0; i<provinces.Count; i++)
            {
                battle b=new battle(provinces[i].attacker_id,provinces[i].defender_id,points);
                battlesToDetermine.Add(b);
            }



        }



        public int totalPointGain_changed()
        {
            
            int totpoi = 0;
            foreach (Tile t in List_of_tiles)
            {
                if (t.PlayerControllerId == _playerHuman.PlayerId1)
                {
                    totpoi += t.PointGain;
                }
            };

            return totpoi;

        }
 
        
        
    }
}