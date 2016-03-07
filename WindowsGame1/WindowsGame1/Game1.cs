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
using System.Diagnostics;
using System.IO;

namespace WindowsGame1
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {                 //объявление переменных
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D deade;
        Texture2D btoom;
        Texture2D grass;
        List<float> enemyrotate = new List<float>();        //l
        Texture2D wall;
        Texture2D grenade;
        Texture2D heroup, deadhero;
        Texture2D enemyup;
        Texture2D win1, win2, gameover1, gameover2;
        Texture2D menu0, menu1, menu2, menu3, menu4, menu5, menu6, menu7, menu8, menu;
        List<Vector2> grenaderspos = new List<Vector2>();
        Vector2 pos1 = new Vector2(1,1);
        List<Vector2> enemy = new List<Vector2>();   //l
        List<Vector2> deadenemy = new List<Vector2>();
        Vector2 speed1 = new Vector2(4, 4);
        Vector2 speed = new Vector2(2, 2);        //l
        Boolean go = true;
        Lvl levels = new Lvl();
        Vector2 grenadepos = new Vector2(-1000,-1000);
        Boolean grenadethown = false;
        int timesinceX = 0;
        int timesinceY = 0;
        Vector2 grenadespeed = new Vector2(0,0);
        Vector2 grenadespeed1 = new Vector2(0, 0);
        int grenadethrowtime = 0;
        int timebtoom = 0;
        bool isbtoom = false;
        Vector2 btoompos = new Vector2(-1000, -1000);
        enum GameState { Start, InGame, GameOver, Win };
        GameState currentGameState = GameState.Start;
        Vector2 cam = new Vector2(0, 0);
        int currentFrame = 0;
        int currentheroFrame = 0;
        List<int> currentenemyFrame = new List<int>();
        int currentgrenade = 0;
        int timeSinceLastFrame = 0;
        int HeroTime = 0;
        List<int> EnemyTime = new List<int>();
        int map;
        int menuchose = 0;
        int menutime = 0;
        SpriteFont font;
        int setuptime = 1000;
        int maps = 0;
        int mapch = 1;
        List<Grenader> grenaders = new List<Grenader>();
        List<Vector4> wallsp = new List<Vector4>();
        List<Rectangle> wallsrect = new List<Rectangle>();
        int p = 1;
        float herorotate = 0;
        //Vector2 floorcam = Vector2.Zero;
        //Vector2 floorpos = new Vector2(-180, -180);
        Texture2D white;
        Texture2D red;
        Boolean mx, my;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        
        protected override void Initialize()
        {
            base.Initialize();
            StreamReader s = new StreamReader("mapscount.txt");
            maps = Convert.ToInt32(s.ReadLine());//получение из файла количества уровней в игре
            //int ipos = 100;
            //for (int i = 0; i < 10; i++)
            //{
            //    wallspos.Add(new Vector2(ipos, 100));
            //    wallsrect.Add(new Rectangle(ipos + 10, 105, 40, 50));
            //    ipos += 50;
            //}
            //grenaders.Add(new Grenader());
            //grenaders[0].pos = new Vector2(100, 100);
            //grenaders[0].img = heroup;
        }

        
        protected override void LoadContent()
        {
            //присвоение переменным типа texture2d соответствующих изображений из ресурсов проекта
            spriteBatch = new SpriteBatch(GraphicsDevice);
            grenade = Content.Load<Texture2D>(@"Images/grenade");
            wall = Content.Load<Texture2D>(@"Images/wall");
            btoom = Content.Load<Texture2D>(@"Images/btoom");
            grass = Content.Load<Texture2D>(@"Images/floor");
            font = Content.Load<SpriteFont>(@"Fonts/SpriteFont1");
            heroup = Content.Load<Texture2D>(@"Images/Hero/HeroUp");
            deadhero = Content.Load<Texture2D>(@"Images/Hero/DeadHero");
            deade = Content.Load<Texture2D>(@"Images/Enemy/DeadEnemy");
            enemyup = Content.Load<Texture2D>(@"Images/Enemy/EnemyUp");
            win1 = Content.Load<Texture2D>(@"Images/Menues/Win1");
            win2 = Content.Load<Texture2D>(@"Images/Menues/Win2");
            gameover1 = Content.Load<Texture2D>(@"Images/Menues/GameOver1");
            gameover2 = Content.Load<Texture2D>(@"Images/Menues/GameOver2");
            menu0 = Content.Load<Texture2D>(@"Images/Menues/Main/Menu0");
            menu1 = Content.Load<Texture2D>(@"Images/Menues/Main/Menu1");
            menu2 = Content.Load<Texture2D>(@"Images/Menues/Main/Menu2");
            menu3 = Content.Load<Texture2D>(@"Images/Menues/Main/Menu3");
            menu4 = Content.Load<Texture2D>(@"Images/Menues/Main/Menu4");
            menu5 = Content.Load<Texture2D>(@"Images/Menues/Main/Menu5");
            menu6 = Content.Load<Texture2D>(@"Images/Menues/Main/Menu6");
            menu7 = Content.Load<Texture2D>(@"Images/Menues/Main/Menu7");
            menu8 = Content.Load<Texture2D>(@"Images/Menues/Main/Menu8");
            menu = Content.Load<Texture2D>(@"Images/Menues/Main/Menu5");
            white = Content.Load<Texture2D>(@"Images/White");
            red = Content.Load<Texture2D>(@"Images/Red");
        }

        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)                 //здесь весь основной код игры
        {
            KeyboardState keyboardState = Keyboard.GetState();

            //levels.Save(wallspos, wallsrect,enemy);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            switch (currentGameState)                 //оператор множественного выбора для осуществления разных состояний игры
            {
                case GameState.Start:                 //это значение стоит по умолчанию при запуске игры - это главное меню. здесь осуществляется главное меню и возможность выбора в нем
                    if (setuptime < 1000)
                    setuptime += gameTime.ElapsedGameTime.Milliseconds;
                    menutime += gameTime.ElapsedGameTime.Milliseconds;
                    if (menutime > 300)
                    {
                        if (keyboardState.IsKeyDown(Keys.W))
                            if (menuchose <= 1) { menutime = 0; menuchose = 4; }
                            else
                                if (menuchose == 5) { menutime = 0; menuchose += maps; mapch = 3; }
                                else
                                { menutime = 0; if (menuchose > 5) { if (menuchose == 5 + maps) { mapch = 3; } else if (mapch != 1) mapch--; } menuchose--;  }
                        if (keyboardState.IsKeyDown(Keys.S))
                            if (menuchose == 4) { menutime = 0; menuchose = 1; }
                            else
                                if (menuchose == 5 + maps) { menutime = 0; menuchose = 5; mapch = 1; }
                                else
                                { menutime = 0;if (menuchose >= 5) {  if (mapch != 3) mapch++; } menuchose++;  }
                    }
                    if (keyboardState.IsKeyDown(Keys.Enter))
                    {if (menutime > 200)
                        {
                        switch (menuchose)
                        { 
                            case 1:
                                
                                //levels.Read(ref wallspos, ref wallsrect, ref enemy, ref grenaderspos, map);                  //при запуске игры идет получение информации из файлов игры(.xml)
                                map = 1;
                                levels.Read(ref wallsp, ref enemy, ref grenaderspos, map);
                                foreach (var h in grenaderspos)
                                { grenaders.Add(new Grenader()); grenaders[grenaders.Count-1].pos = h; grenaders[grenaders.Count-1].rotate = 0; }
                                for (int i = 0; i < enemy.Count; i++)
                                    enemyrotate.Add(0);
                                menutime = 0;
                                currentGameState = GameState.InGame;
                                break;
                            case 2:
                                menutime = 0;
                                menuchose = 5;
                                break;
                            case 3:
                                if (setuptime >= 980)
                                {
                                    Process.Start("setup.txt");
                                    setuptime = 0;
                                }
                                break;
                            case 4:
                                Exit();
                                break;
                        }
                    }
                        if (menutime > 300)
                        {
                            if (menuchose > 4 && menuchose < 5 + maps)
                            {
                                map = menuchose - 4;
                                //levels.Read(ref wallspos, ref wallsrect, ref enemy, ref grenaderspos, map);
                                levels.Read(ref wallsp, ref enemy, ref grenaderspos, map);
                                foreach (var h in grenaderspos)
                                { grenaders.Add(new Grenader()); grenaders[grenaders.Count-1].pos = h; grenaders[grenaders.Count-1].rotate = 0; }
                                for (int i = 0; i < enemy.Count; i++)
                                    enemyrotate.Add(0);
                                menutime = 0;
                                currentGameState = GameState.InGame;
                            }
                            if (menuchose == 5+maps)
                            { menutime = 0; menuchose = 2; }
                        }
                    }
                    //if (keyboardState.IsKeyDown(Keys.Enter))
                    //{
                    //    map = 1;//////d
                    //    currentGameState = GameState.InGame;
                    //}
                    break;
                case GameState.InGame:                 //это состояние игры для - сама игра

                    //+cam - это передвижение объектов в игровом поле при хождении игрока
                    pos1 = pos1 + cam;
                    btoompos  += cam;
                    grenadepos +=cam;




                    if (p == 1)
                    {   foreach (var v in wallsp)
                        {
                            wallsrect.Add(new Rectangle((int)v.X, (int)v.Y, (int)v.Z - (int)v.X, (int)v.W - (int)v.Y));
                        }
                        for (int i = 0; i < enemy.Count; i++)
                        {
                            currentenemyFrame.Add(0);
                            EnemyTime.Add(0);
                        }
                        p = 0;
                    }
                    
                    
                    for (int i = 0; i < wallsp.Count; i++ )
                    {
                        wallsp[i] += new Vector4((int)cam.X, (int)cam.Y, (int)cam.X, (int)cam.Y);
                    }
                    //далее создание прямоугольников для объектов для определения пересечений
                    for (int i = 0; i < wallsrect.Count; i++)
                    wallsrect[i] = new Rectangle(wallsrect[i].Left + (int)cam.X, wallsrect[i].Top + (int)cam.Y, wallsrect[i].Right - wallsrect[i].Left, wallsrect[i].Bottom - wallsrect[i].Top);
                    Rectangle btoomrect = new Rectangle((int)btoompos.X + (int)cam.X + 17,
                (int)btoompos.Y + (int)cam.Y + 17, btoom.Width / 9 - 17, btoom.Height - 17);
                    Rectangle img1rect = new Rectangle((int)pos1.X+25,
                (int)pos1.Y + 30, heroup.Width / 3 - 50, heroup.Height - 70);
                    List<Rectangle> enemyrect = new List<Rectangle>();


                    foreach (var gr in grenaders)                 //перечисление всех врагов, кидающихся гранатами, и далее код для их действий
                    {
                        {
                            gr.time += gameTime.ElapsedGameTime.Milliseconds;
                            if (gr.time > 100)                            //смена кадров врагов
                            {
                                gr.time = 0;
                                gr.frame++;
                                if (gr.frame >= 3)
                                {
                                    gr.frame = 0;
                                }
                            }
                        }
                        
                        if (gr.alive)
                        {
                            if (gr.pos.X > pos1.X + 20)                 //в следующих трех условиях находится подходящее направление картинки для направления движния(если они стоят близко, то убегают; если далеко, то приближаются)
                            {
                                if (gr.pos.Y > pos1.Y + 20)
                                {
                                    if (Math.Sqrt(Math.Abs(gr.pos.Y - pos1.Y) * Math.Abs(gr.pos.Y - pos1.Y) + Math.Abs(gr.pos.X - pos1.X) * Math.Abs(gr.pos.X - pos1.X)) >= 300)
                                    gr.rotate = 5.498f;
                                    else
                                        gr.rotate = 2.356f;
                                }
                                if (gr.pos.Y < pos1.Y - 20)
                                {
                                    if (Math.Sqrt(Math.Abs(gr.pos.Y - pos1.Y) * Math.Abs(gr.pos.Y - pos1.Y) + Math.Abs(gr.pos.X - pos1.X) * Math.Abs(gr.pos.X - pos1.X)) >= 300)
                                    gr.rotate = 3.927f;
                                    else
                                        gr.rotate = 0.785f;
                                }
                                if ((gr.pos.Y >= pos1.Y - 20) && (gr.pos.Y <= pos1.Y + 20))
                                {
                                    if (Math.Sqrt(Math.Abs(gr.pos.Y - pos1.Y) * Math.Abs(gr.pos.Y - pos1.Y) + Math.Abs(gr.pos.X - pos1.X) * Math.Abs(gr.pos.X - pos1.X)) >= 300)
                                    gr.rotate = 4.712f;
                                    else
                                        gr.rotate = 1.571f;
                                }
                            }
                            if (gr.pos.X < pos1.X - 20)
                            {
                                if (gr.pos.Y > pos1.Y+20)
                                {
                                    if (Math.Sqrt(Math.Abs(gr.pos.Y - pos1.Y) * Math.Abs(gr.pos.Y - pos1.Y) + Math.Abs(gr.pos.X - pos1.X) * Math.Abs(gr.pos.X - pos1.X)) >= 300)
                                    gr.rotate = 0.785f;
                                    else
                                        gr.rotate = 3.927f;
                                }
                                if (gr.pos.Y < pos1.Y-20)
                                {
                                    if (Math.Sqrt(Math.Abs(gr.pos.Y - pos1.Y) * Math.Abs(gr.pos.Y - pos1.Y) + Math.Abs(gr.pos.X - pos1.X) * Math.Abs(gr.pos.X - pos1.X)) >= 300)
                                        gr.rotate = 2.356f;
                                    else
                                        gr.rotate = 5.498f;
                                }
                                if ((gr.pos.Y >= pos1.Y - 20) && (gr.pos.Y <= pos1.Y + 20))
                                {
                                    if (Math.Sqrt(Math.Abs(gr.pos.Y - pos1.Y) * Math.Abs(gr.pos.Y - pos1.Y) + Math.Abs(gr.pos.X - pos1.X) * Math.Abs(gr.pos.X - pos1.X)) >= 300)
                                        gr.rotate = 1.571f;
                                    else
                                        gr.rotate = 4.712f;
                                }
                            }
                            if ((gr.pos.X >= pos1.X - 20) && (gr.pos.X <= pos1.X + 20))
                            {
                                if (gr.pos.Y > pos1.Y + 20)
                                {
                                    if (Math.Sqrt(Math.Abs(gr.pos.Y - pos1.Y) * Math.Abs(gr.pos.Y - pos1.Y) + Math.Abs(gr.pos.X - pos1.X) * Math.Abs(gr.pos.X - pos1.X)) >= 300)
                                        gr.rotate = 0;
                                    else
                                        gr.rotate = 3.142f;
                                }
                                if (gr.pos.Y < pos1.Y - 20)
                                {
                                    if (Math.Sqrt(Math.Abs(gr.pos.Y - pos1.Y) * Math.Abs(gr.pos.Y - pos1.Y) + Math.Abs(gr.pos.X - pos1.X) * Math.Abs(gr.pos.X - pos1.X)) >= 300)
                                        gr.rotate = 3.142f;
                                    else
                                        gr.rotate = 0;
                                }
                            }



                            if (Math.Sqrt(Math.Abs(gr.pos.Y - pos1.Y) * Math.Abs(gr.pos.Y - pos1.Y) + Math.Abs(gr.pos.X - pos1.X) * Math.Abs(gr.pos.X - pos1.X)) <= 320)                            //здесь определяется направление полета гранаты и начало броска
                                if (gr.grenadethown == false)
                                {
                                    gr.grenadethown = true;
                                    gr.grenadepos = gr.pos + new Vector2(heroup.Width / 6 - grenade.Width / 100, heroup.Height / 2 - grenade.Height / 15);
                                    if (gr.pos.Y > pos1.Y)
                                        gr.grenadespeed = new Vector2(0, -4);
                                    else
                                        gr.grenadespeed = new Vector2(0, 4);

                                    if (gr.pos.X > pos1.X)
                                    {   gr.grenadespeed = new Vector2(-4,0);
                                        if (gr.pos.Y > pos1.Y)
                                            gr.grenadespeed = new Vector2(- 3, -3);
                                        if (gr.pos.Y < pos1.Y)
                                            gr.grenadespeed = new Vector2(-3, 3);
                                    }
                                    if (gr.pos.X < pos1.X)
                                    {
                                        gr.grenadespeed = new Vector2(4, 0);
                                        if (gr.pos.Y > pos1.Y)
                                            gr.grenadespeed = new Vector2(3, -3);
                                        if (gr.pos.Y < pos1.Y)
                                            gr.grenadespeed = new Vector2(3, 3);
                                    }
                                }




                            if (Math.Abs(gr.pos.X - pos1.X) <= 300 && Math.Sqrt(Math.Abs(gr.pos.Y - pos1.Y) * Math.Abs(gr.pos.Y - pos1.Y) + Math.Abs(gr.pos.X - pos1.X) * Math.Abs(gr.pos.X - pos1.X)) <= 300)                            //в этом условии осуществляется движение кидателей гранат и столкновение их со стенками и другими нпс
                            {
                                if (gr.pos.X > pos1.X + 20)
                                {
                                    for (int i = 0; i < wallsrect.Count; i++)
                                    {
                                        if (new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50 + (int)speed.X, enemyup.Height - 70).Intersects(wallsrect[i]))
                                        { go = false; }
                                    }
                                    //for (int i = 0; i < grenaders.Count; i++)
                                    //{
                                    //    if (gr.pos != grenaders[i].pos)
                                    //        if (new Rectangle((int)gr.pos.X + 10, (int)gr.pos.Y + 10, enemyup.Width / 3 - 10 + (int)speed.X, enemyup.Height - 5).Intersects(enemyrect[i]))
                                    //        { go = false; }
                                    //}
                                    for (int i = 0; i < grenaders.Count; i++)
                                    {
                                        if (gr.pos != grenaders[i].pos)
                                            if (new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50 + (int)speed.X, enemyup.Height - 70).Intersects(grenaders[i].rect))
                                            { go = false; }
                                    }
                                    for (int i = 0; i < enemyrect.Count; i++)
                                    {
                                        if (new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50 + (int)speed.X, enemyup.Height - 70).Intersects(enemyrect[i]))
                                        { go = false; }
                                    }
                                    if (go == true) gr.pos = new Vector2(gr.pos.X + speed.X, gr.pos.Y);
                                    go = true;
                                }
                                if (gr.pos.X < pos1.X - 20)
                                {
                                    for (int i = 0; i < wallsrect.Count; i++)
                                    {
                                        if (new Rectangle((int)gr.pos.X + 25 - (int)speed.X, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50, enemyup.Height - 70).Intersects(wallsrect[i]))
                                        { go = false; }
                                    }
                                    //for (int i = 0; i < grenaders.Count; i++)
                                    //{
                                    //    if (gr.pos != grenaders[i].pos)
                                    //        if (new Rectangle((int)gr.pos.X + 10 - (int)speed.X, (int)gr.pos.Y + 10, enemyup.Width / 3 - 10, enemyup.Height - 5).Intersects(enemyrect[i]))
                                    //        { go = false; }
                                    //}
                                    for (int i = 0; i < grenaders.Count; i++)
                                    {
                                        if ((gr.pos != grenaders[i].pos)&&grenaders[i].alive)
                                            if (new Rectangle((int)gr.pos.X + 25 - (int)speed.X, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50, enemyup.Height - 70).Intersects(grenaders[i].rect))
                                            { go = false; }
                                    }
                                    
                                    for (int i = 0; i < enemyrect.Count; i++)
                                    {
                                        if (new Rectangle((int)gr.pos.X + 25 - (int)speed.X, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50, enemyup.Height - 70).Intersects(enemyrect[i]))
                                        { go = false; }
                                    }
                                    if (go == true) gr.pos = new Vector2(gr.pos.X - speed.X, gr.pos.Y);
                                    go = true;
                                }
                            }
                            else
                            {
                                if (gr.pos.X > pos1.X + 20)
                                {
                                    for (int i = 0; i < wallsrect.Count; i++)
                                    {
                                        if (new Rectangle((int)gr.pos.X + 25 - (int)speed.X, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50, enemyup.Height - 70).Intersects(wallsrect[i]))
                                        { go = false; }
                                    }
                                    //for (int i = 0; i < grenaders.Count; i++)
                                    //{
                                    //    if (gr.pos != grenaders[i].pos)
                                    //        if (new Rectangle((int)gr.pos.X + 10 - (int)speed.X, (int)gr.pos.Y + 10, enemyup.Width / 3 - 10, enemyup.Height - 5).Intersects(enemyrect[i]))
                                    //        { go = false; }
                                    //}

                                    for (int i = 0; i < grenaders.Count; i++)
                                    {
                                        if ((gr.pos != grenaders[i].pos)&&grenaders[i].alive)
                                            if (new Rectangle((int)gr.pos.X + 25 - (int)speed.X, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50, enemyup.Height - 70).Intersects(grenaders[i].rect))
                                            { go = false; }
                                    }
                                    for (int i = 0; i < enemyrect.Count; i++)
                                    {
                                        if (new Rectangle((int)gr.pos.X + 25 - (int)speed.X, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50, enemyup.Height - 70).Intersects(enemyrect[i]))
                                        { go = false; }
                                    }
                                    if (go == true) gr.pos = new Vector2(gr.pos.X - speed.X, gr.pos.Y);
                                    go = true;
                                }
                                if (gr.pos.X < pos1.X - 20)
                                {
                                    for (int i = 0; i < wallsrect.Count; i++)
                                    {
                                        if (new Rectangle((int)gr.pos.X + 25 + (int)speed.X, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50, enemyup.Height - 70).Intersects(wallsrect[i]))
                                        { go = false; }
                                    }
                                    //for (int i = 0; i < grenaders.Count; i++)
                                    //{
                                    //    if (gr.pos != grenaders[i].pos)
                                    //        if (new Rectangle((int)gr.pos.X + 10, (int)gr.pos.Y + 10, enemyup.Width / 3 - 10 + (int)speed.X, enemyup.Height - 5).Intersects(enemyrect[i]))
                                    //        { go = false; }
                                    //}
                                    for (int i = 0; i < grenaders.Count; i++)
                                    {
                                        if ((gr.pos != grenaders[i].pos) && grenaders[i].alive)
                                            if (new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50 + (int)speed.X, enemyup.Height - 70).Intersects(grenaders[i].rect))
                                            { go = false; }
                                    }
                                    for (int i = 0; i < enemyrect.Count; i++)
                                    {
                                        if (new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50 + (int)speed.X, enemyup.Height - 70).Intersects(enemyrect[i]))
                                        { go = false; }
                                    }
                                    if (go == true) gr.pos = new Vector2(gr.pos.X + speed.X, gr.pos.Y);
                                    go = true;
                                }
                            }
                            if (Math.Abs(gr.pos.Y - pos1.Y) <= 300 && Math.Sqrt(Math.Abs(gr.pos.Y - pos1.Y) * Math.Abs(gr.pos.Y - pos1.Y) + Math.Abs(gr.pos.X - pos1.X) * Math.Abs(gr.pos.X - pos1.X)) <= 300)
                            {
                                if (gr.pos.Y > pos1.Y + 20)
                                {
                                    for (int i = 0; i < wallsrect.Count; i++)
                                    {
                                        if (new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50, enemyup.Height - 70 + (int)speed.Y).Intersects(wallsrect[i]))
                                        { go = false; }
                                    }
                                    //for (int i = 0; i < grenaders.Count; i++)
                                    //{
                                    //    if (gr.pos != grenaders[i].pos)
                                    //        if (new Rectangle((int)gr.pos.X + 10, (int)gr.pos.Y + 10, enemyup.Width / 3 - 10, enemyup.Height - 5 + (int)speed.Y).Intersects(enemyrect[i]))
                                    //        { go = false; }
                                    //}
                                    for (int i = 0; i < grenaders.Count; i++)
                                    {
                                        if ((gr.pos != grenaders[i].pos) && grenaders[i].alive)
                                            if (new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50, enemyup.Height - 70 + (int)speed.Y).Intersects(grenaders[i].rect))
                                            { go = false; }
                                    }
                                    for (int i = 0; i < enemyrect.Count; i++)
                                    {
                                        if (new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50, enemyup.Height - 70 + (int)speed.Y).Intersects(enemyrect[i]))
                                        { go = false; }
                                    }
                                    if (go == true) gr.pos = new Vector2(gr.pos.X, gr.pos.Y + speed.Y);
                                    go = true;
                                }
                                if (gr.pos.Y < pos1.Y - 20)
                                {
                                    for (int i = 0; i < wallsrect.Count; i++)
                                    {
                                        if (new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30 - (int)speed.Y, enemyup.Width / 3 - 50, enemyup.Height - 70).Intersects(wallsrect[i]))
                                        { go = false; }
                                    }
                                    //for (int i = 0; i < grenaders.Count; i++)
                                    //{
                                    //    if (gr.pos != grenaders[i].pos)
                                    //        if (new Rectangle((int)gr.pos.X + 10, (int)gr.pos.Y + 10 - (int)speed.Y, enemyup.Width / 3 - 10, enemyup.Height - 5).Intersects(enemyrect[i]))
                                    //        { go = false; }
                                    //}
                                    for (int i = 0; i < grenaders.Count; i++)
                                    {
                                        if ((gr.pos != grenaders[i].pos) && grenaders[i].alive)
                                            if (new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30 - (int)speed.Y, enemyup.Width / 3 - 50, enemyup.Height - 70).Intersects(grenaders[i].rect))
                                            { go = false; }
                                    }
                                    for (int i = 0; i < enemyrect.Count; i++)
                                    {
                                        if (new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30 - (int)speed.Y, enemyup.Width / 3 - 50, enemyup.Height - 70).Intersects(enemyrect[i]))
                                        { go = false; }
                                    }
                                    if (go == true) gr.pos = new Vector2(gr.pos.X, gr.pos.Y - speed.Y);
                                    go = true;
                                }
                            }
                            else
                            {
                                if (gr.pos.Y > pos1.Y+20)
                                {
                                    for (int i = 0; i < wallsrect.Count; i++)
                                    {
                                        if (new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30 - (int)speed.Y, enemyup.Width / 3 - 50, enemyup.Height - 70).Intersects(wallsrect[i]))
                                        { go = false; }
                                    }
                                    //for (int i = 0; i < grenaders.Count; i++)
                                    //{
                                    //    if (gr.pos != grenaders[i].pos)
                                    //        if (new Rectangle((int)gr.pos.X + 10, (int)gr.pos.Y + 10 - (int)speed.Y, enemyup.Width / 3 - 10, enemyup.Height - 5).Intersects(enemyrect[i]))
                                    //        { go = false; }
                                    //}
                                    for (int i = 0; i < grenaders.Count; i++)
                                    {
                                        if ((gr.pos != grenaders[i].pos) && grenaders[i].alive)
                                            if (new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30 - (int)speed.Y, enemyup.Width / 3 - 50, enemyup.Height - 70).Intersects(grenaders[i].rect))
                                            { go = false; }
                                    }
                                    for (int i = 0; i < enemyrect.Count; i++)
                                    {
                                        if (new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30 - (int)speed.Y, enemyup.Width / 3 - 50, enemyup.Height - 70).Intersects(enemyrect[i]))
                                        { go = false; }
                                    }
                                    if (go == true) gr.pos = new Vector2(gr.pos.X, gr.pos.Y - speed.Y);
                                    go = true;
                                }
                                if (gr.pos.Y < pos1.Y-20)
                                {
                                    for (int i = 0; i < wallsrect.Count; i++)
                                    {
                                        if (new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30 + (int)speed.Y, enemyup.Width / 3 - 50, enemyup.Height - 70).Intersects(wallsrect[i]))
                                        { go = false; }
                                    }
                                    //for (int i = 0; i < grenaders.Count; i++)
                                    //{
                                    //    if (gr.pos != grenaders[i].pos)
                                    //        if (new Rectangle((int)gr.pos.X + 10, (int)gr.pos.Y + 10, enemyup.Width / 3 - 10, enemyup.Height - 5 + (int)speed.Y).Intersects(enemyrect[i]))
                                    //        { go = false; }
                                    //}
                                    for (int i = 0; i < grenaders.Count; i++)
                                    {
                                        if ((gr.pos != grenaders[i].pos) && grenaders[i].alive)
                                            if (new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50, enemyup.Height - 70 + (int)speed.Y).Intersects(grenaders[i].rect))
                                            { go = false; }
                                    }
                                    for (int i = 0; i < enemyrect.Count; i++)
                                    {
                                        if (new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50, enemyup.Height - 70 + (int)speed.Y).Intersects(enemyrect[i]))
                                        { go = false; }
                                    }
                                    if (go == true) gr.pos = new Vector2(gr.pos.X, gr.pos.Y + speed.Y);
                                    go = true;
                                }
                            }
                        }
                        if ((new Rectangle((int)gr.pos.X + 25, (int)gr.pos.Y + 30, enemyup.Width / 3 - 50, enemyup.Height - 70)).Intersects(btoomrect))                            //здесь определяется попадение гренадеров в зону взрыва игрока
                                gr.alive = false;
                            gr.btoomrect = new Rectangle((int)gr.btoompos.X + (int)cam.X + 17, (int)gr.btoompos.Y + (int)cam.Y + 17, btoom.Width / 9 - 17, btoom.Height - 17);
                            for (int i = 0; i < enemyrect.Count; i++)                            //здесь определяется попадение обычных врагов в зону взрыва гранат кидающихся нпс
                                if (enemyrect[i].Intersects(gr.btoomrect))  //l
                                {
                                    deadenemy.Add(enemy[i]);
                                    enemy.RemoveAt(i);
                                    enemyrotate.RemoveAt(i);
                                }
                        
                            if (gr.alive)
                                foreach (var grr in grenaders)                            //определяется попадение гренадеров в зону взрывов других гранат
                                {
                                    grr.rect = new Rectangle((int)grr.pos.X + 25, (int)grr.pos.Y + 30, enemyup.Width / 3 - 50, enemyup.Height - 70);
                                    if (grr.rect.Intersects(gr.btoomrect))  //l
                                    {
                                        grr.alive = false;
                                    }
                                }
                            gr.grenadepos += cam;
                            gr.btoompos += cam;
                            if (img1rect.Intersects(gr.btoomrect))                            //определяется попадение игрока в зону взрыва
                                {
                                    menuchose = 1;
                                    menutime = 0;
                                    currentGameState = GameState.GameOver;
                                }
                            if (gr.grenadethown == true)                            //здесь описывается полет гранаты и определяется начало и место взрыва
                            {
                                gr.grenadethrowtime += gameTime.ElapsedGameTime.Milliseconds;
                                gr.currentgrenade = 0;
                                if (gr.grenadethrowtime > 200) gr.currentgrenade = 1;
                                if (gr.grenadethrowtime > 400) gr.currentgrenade = 2;
                                if (gr.grenadethrowtime > 600) gr.currentgrenade = 3;
                                if (gr.grenadethrowtime > 800) gr.currentgrenade = 4;
                                for (int i = 0; i < wallsrect.Count; i++)
                                {
                                    if (gr.grenadespeed.X > 0) if (new Rectangle((int)gr.grenadepos.X + 2, (int)gr.grenadepos.Y + 2, grenade.Width / 50 - 2 + 6, grenade.Height / 10 - 2).Intersects(wallsrect[i])) { grenadepos -= gr.grenadespeed; gr.grenadespeed.X = -gr.grenadespeed.X; }
                                    if (gr.grenadespeed.X < 0) if (new Rectangle((int)gr.grenadepos.X + 2 - 6, (int)gr.grenadepos.Y + 2, grenade.Width / 50 - 2, grenade.Height / 10 - 2).Intersects(wallsrect[i])) { grenadepos += gr.grenadespeed; gr.grenadespeed.X = -gr.grenadespeed.X; }
                                    if (gr.grenadespeed.Y > 0) if (new Rectangle((int)gr.grenadepos.X + 2, (int)gr.grenadepos.Y + 2, grenade.Width / 50 - 2, grenade.Height / 10 - 2 + 6).Intersects(wallsrect[i])) { grenadepos -= gr.grenadespeed; gr.grenadespeed.Y = -gr.grenadespeed.Y; }
                                    if (gr.grenadespeed.Y < 0) if (new Rectangle((int)gr.grenadepos.X + 2, (int)gr.grenadepos.Y + 2 - 6, grenade.Width / 50 - 2, grenade.Height / 10 - 2).Intersects(wallsrect[i])) { grenadepos += gr.grenadespeed; gr.grenadespeed.Y = -gr.grenadespeed.Y; }
                                }
                                if (gr.grenadethrowtime < 1000) { if (gr.grenadespeed.X == 0 || gr.grenadespeed.Y == 0) gr.grenadepos += 4 * (gr.grenadespeed / 3); else gr.grenadepos += gr.grenadespeed; }
                                else
                                {
                                    gr.grenadethrowtime = 0; gr.grenadethown = false; gr.btoompos = gr.grenadepos + new Vector2(grenade.Width / 100 - btoom.Width / 18, grenade.Height / 20 - btoom.Height / 2); gr.grenadepos = new Vector2(-1000, -1000); gr.isbtoom = true;
                                }
                            }
                            if (gr.isbtoom)                            //здесь описывается взрыв
                            {
                                gr.timebtoom += gameTime.ElapsedGameTime.Milliseconds;
                                if (gr.timebtoom > 800) { gr.isbtoom = false; gr.btoompos = new Vector2(-1000, -1000); gr.timebtoom = 0; gr.timeSinceLastFrame = 0; gr.currentFrame = 0; }
                                gr.timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                                if (gr.timeSinceLastFrame > 100)
                                {
                                    gr.timeSinceLastFrame = 0;
                                    gr.currentFrame++;
                                    if (gr.currentFrame >= 8)
                                    {
                                        gr.currentFrame = 0;
                                    }
                                }

                                
                            }
                        
                    }









                    for (int i = 0; i < grenaders.Count; i++)
                    {
                        grenaders[i].pos += cam;
                    }

                    for (int i = 0; i < enemy.Count; i++)                            //здесь определяется соприкосновение игрока и обычных врагов
                    {
                        enemy[i] = enemy[i] + cam;
                        enemyrect.Add(new Rectangle((int)enemy[i].X + 25, (int)enemy[i].Y + 30, enemyup.Width / 3 - 50, enemyup.Height - 70));
                        if (enemyrect[i].Intersects(img1rect))
                        {
                            menuchose = 1;
                            menutime = 0;
                            currentGameState = GameState.GameOver;
                        }


                        if (enemyrect[i].Intersects(btoomrect))  //l                            //определяется попадение обычных врагов во взрывы игрока
                        {   deadenemy.Add(enemy[i]);
                            enemy.RemoveAt(i);
                            enemyrotate.RemoveAt(i);
                        }   //l
                        

                    }                        
                    if (enemy.Count == 0)                            //определение победы
                    {
                        Boolean b = true;
                        foreach (var g in grenaders)
                            if (g.alive) b = false;

                        if (b)
                        {
                            menuchose = 1;
                            menutime = 0;
                            currentGameState = GameState.Win;
                        }
                        b = true;
                    }
                    for (int i = 0; i < deadenemy.Count; i++)                            //сдвигание мертвых нпс
                    deadenemy[i] = deadenemy[i] + cam;
                    if (img1rect.Intersects(btoomrect))                            //определение попадения игрока в свой взрыв
                    {
                        menuchose = 1;
                            menutime = 0;
                        currentGameState = GameState.GameOver;
                    }
                    if (isbtoom)                            //описывается взрыв игрока
                    {
                        timebtoom += gameTime.ElapsedGameTime.Milliseconds;
                        if (timebtoom > 800) { isbtoom = false; btoompos = new Vector2(-1000, -1000); timebtoom = 0; timeSinceLastFrame = 0; currentFrame = 0; }
                        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                        if (timeSinceLastFrame > 100)
                        {
                            timeSinceLastFrame = 0;
                            currentFrame++;
                            if (currentFrame >= 8)
                            {
                                currentFrame = 0;
                            }
                        }
                    }
                    

                    
                    

                    if (keyboardState.IsKeyUp(Keys.W) && keyboardState.IsKeyUp(Keys.A) && keyboardState.IsKeyUp(Keys.S) && keyboardState.IsKeyUp(Keys.D))     //использование картинки при остановке
                            currentheroFrame = 1;
                    if (keyboardState.IsKeyDown(Keys.A))                            //определение направления игрока и движение при нажатой клавише А и столкновение со стенами
                    {
                        if (keyboardState.IsKeyUp(Keys.W) && keyboardState.IsKeyUp(Keys.S) && keyboardState.IsKeyUp(Keys.D))
                        HeroTime += gameTime.ElapsedGameTime.Milliseconds;
                        if (HeroTime > 100)
                        {
                            HeroTime = 0;
                            currentheroFrame++;
                            if (currentheroFrame >= 3)
                            {
                                currentheroFrame = 0;
                            }
                        }
                        grenadespeed.X = -3;
                        timesinceX = 0;
                        if (timesinceY > 100) grenadespeed.Y = 0;
                        for (int i = 0; i < wallsrect.Count; i++)
                            if (new Rectangle((int)pos1.X - (int)speed1.X+20, (int)pos1.Y + 22, heroup.Width / 3 - 45, heroup.Height - 59).Intersects(wallsrect[i]))
                            { go = false; }
                        if (go == true) pos1.X -= speed1.X;
                        go = true;
                    }
                    if (keyboardState.IsKeyDown(Keys.D))                            //определение направления игрока и движение при нажатой клавише D и столкновение со стенами
                    {
                        if (keyboardState.IsKeyUp(Keys.W) && keyboardState.IsKeyUp(Keys.S))
                        HeroTime += gameTime.ElapsedGameTime.Milliseconds;
                        if (HeroTime > 100)
                        {
                            HeroTime = 0;
                            currentheroFrame++;
                            if (currentheroFrame >= 3)
                            {
                                currentheroFrame = 0;
                            }
                        }
                        grenadespeed.X = 3;
                        timesinceX = 0;
                        if (timesinceY > 100) grenadespeed.Y = 0;
                        for (int i = 0; i < wallsrect.Count; i++)
                            if (new Rectangle((int)pos1.X + (int)speed1.X + 20, (int)pos1.Y + 22, heroup.Width / 3 - 45, heroup.Height - 59).Intersects(wallsrect[i]))
                            { go = false; }
                        if (go == true) pos1.X += speed1.X;
                        go = true;
                    }
                    if (keyboardState.IsKeyDown(Keys.W))                            //определение направления игрока и движение при нажатой клавише W и столкновение со стенами
                    {
                        if (keyboardState.IsKeyUp(Keys.S))
                        HeroTime += gameTime.ElapsedGameTime.Milliseconds;
                        if (HeroTime > 100)
                        {
                            HeroTime = 0;
                            currentheroFrame++;
                            if (currentheroFrame >= 3)
                            {
                                currentheroFrame = 0;
                            }
                        }
                        grenadespeed.Y = -3;
                        timesinceY = 0;
                        if (timesinceX > 100) grenadespeed.X = 0;
                        for (int i = 0; i < wallsrect.Count; i++)
                            if (new Rectangle((int)pos1.X + 22, (int)pos1.Y + 20 - (int)speed1.Y, heroup.Width / 3 - 49, heroup.Height - 55).Intersects(wallsrect[i]))
                            { go = false; }
                        if (go == true) pos1.Y -= speed1.Y;
                        go = true;
                    }
                    if (keyboardState.IsKeyDown(Keys.S))                            //определение направления игрока и движение при нажатой клавише S и столкновение со стенами
                    {
                        HeroTime += gameTime.ElapsedGameTime.Milliseconds;
                        if (HeroTime > 100)
                        {
                            HeroTime = 0;
                            currentheroFrame++;
                            if (currentheroFrame >= 3)
                            {
                                currentheroFrame = 0;
                            }
                        }
                        grenadespeed.Y = 3;
                        timesinceY = 0;
                        if (timesinceX > 100) grenadespeed.X = 0;
                        for (int i = 0; i < wallsrect.Count; i++)
                            if (new Rectangle((int)pos1.X + 22, (int)pos1.Y + 20 + (int)speed1.Y, heroup.Width / 3 - 49, heroup.Height - 45).Intersects(wallsrect[i]))
                            { go = false; }
                        if (go == true) pos1.Y += speed1.Y;
                        go = true;
                    }
                    timesinceX += gameTime.ElapsedGameTime.Milliseconds;
                    timesinceY += gameTime.ElapsedGameTime.Milliseconds;

                    if (grenadespeed == new Vector2(3, 3)) herorotate = 2.356f;
                    if (grenadespeed == new Vector2(3, 0)) herorotate = 1.571f;
                    if (grenadespeed == new Vector2(0, 3)) herorotate = 3.142f;
                    if (grenadespeed == new Vector2(-3, -3)) herorotate = 5.498f;
                    if (grenadespeed == new Vector2(-3, 0)) herorotate = 4.712f;
                    if (grenadespeed == new Vector2(0, -3)) herorotate = 0;
                    if (grenadespeed == new Vector2(3, -3)) herorotate = 0.785f;
                    if (grenadespeed == new Vector2(-3, 3)) herorotate = 3.927f; 

                    if (menutime < 300) { menutime += gameTime.ElapsedGameTime.Milliseconds; }
                    if (menutime > 300)
                        if (keyboardState.IsKeyDown(Keys.Enter))                            //определение начала броска игрока
                        if (grenadethown == false)
                        {
                            grenadethown = true;
                            grenadepos = pos1 + new Vector2(heroup.Width / 6 - grenade.Width / 100, heroup.Height / 2 - grenade.Height / 15);
                            grenadespeed1 = grenadespeed;
                        }
                    if (grenadethown == true)                            //описание полета гранаты игрока
                    {
                        grenadethrowtime += gameTime.ElapsedGameTime.Milliseconds;
                        currentgrenade = 0;
                        if (grenadethrowtime > 200) currentgrenade = 1;
                        if (grenadethrowtime > 400) currentgrenade = 2;
                        if (grenadethrowtime > 600) currentgrenade = 3;
                        if (grenadethrowtime > 800) currentgrenade = 4;
                        for (int i = 0; i < wallsrect.Count; i++)
                        {
                            if (grenadespeed1.X > 0) if (new Rectangle((int)grenadepos.X + 2, (int)grenadepos.Y + 2, grenade.Width / 50 - 2 + 6, grenade.Height / 10 - 2).Intersects(wallsrect[i])) { grenadepos -= grenadespeed1; grenadespeed1.X = -grenadespeed1.X; }
                            if (grenadespeed1.X < 0) if (new Rectangle((int)grenadepos.X + 2 - 6, (int)grenadepos.Y + 2, grenade.Width / 50 - 2, grenade.Height / 10 - 2).Intersects(wallsrect[i])) { grenadepos += grenadespeed1; grenadespeed1.X = -grenadespeed1.X; }
                            if (grenadespeed1.Y > 0) if (new Rectangle((int)grenadepos.X + 2, (int)grenadepos.Y + 2, grenade.Width / 50 - 2, grenade.Height / 10 - 2 + 6).Intersects(wallsrect[i])) { grenadepos -= grenadespeed1; grenadespeed1.Y = -grenadespeed1.Y; }
                            if (grenadespeed1.Y < 0) if (new Rectangle((int)grenadepos.X + 2, (int)grenadepos.Y + 2 - 6, grenade.Width / 50 - 2, grenade.Height / 10 - 2).Intersects(wallsrect[i])) { grenadepos += grenadespeed1; grenadespeed1.Y = -grenadespeed1.Y; }
                        }
                        if (grenadethrowtime < 1000) { if (grenadespeed1.X == 0 || grenadespeed1.Y == 0) grenadepos += 4 * (grenadespeed1 / 3); else grenadepos += grenadespeed1; }
                        else
                        {
                            grenadethrowtime = 0; grenadethown = false; btoompos = grenadepos + new Vector2(grenade.Width / 100 - btoom.Width / 18, grenade.Height / 20 - btoom.Height / 2); grenadepos = new Vector2(-1000, -1000); isbtoom = true;
                        }
                    }

                    //l        //l 
                    for (int i2 = 0; i2 < enemy.Count; i2++)                            //определение направления обычных врагов
                    {
                        
                        
                        for (int i = 0; i < wallsrect.Count; i++)
                        {
                                if (new Rectangle((int)pos1.X + 25, (int)enemy[i2].Y + 30, (int)enemy[i2].X - (int)pos1.X - 50 + enemyup.Width/3, enemyup.Height - 70).Intersects(wallsrect[i]) || new Rectangle((int)enemy[i2].X + 25, (int)enemy[i2].Y + 30, (int)pos1.X - (int)enemy[i2].X - 50 + enemyup.Width/3, enemyup.Height - 70).Intersects(wallsrect[i]))
                                    mx = false;
                                if (new Rectangle((int)pos1.X + 25, (int)pos1.Y + 30, (int)enemy[i2].X - 50 - (int)pos1.X + enemyup.Width/3, enemyup.Height - 70).Intersects(wallsrect[i]) || new Rectangle((int)enemy[i2].X + 25, (int)pos1.Y + 30, (int)pos1.X - (int)enemy[i2].X - 50 + enemyup.Width/3, enemyup.Height - 70).Intersects(wallsrect[i]))
                                    my = false;
                                if (new Rectangle((int)enemy[i2].X + 25, (int)enemy[i2].Y + 30, enemyup.Width / 3 - 50, (int)pos1.Y - (int)enemy[i2].Y - 70 + enemyup.Height).Intersects(wallsrect[i]) || new Rectangle((int)enemy[i2].X + 25, (int)pos1.Y + 30, enemyup.Width / 3 - 50, (int)enemy[i2].Y - (int)pos1.Y - 70 + enemyup.Height).Intersects(wallsrect[i]))
                                    my = false;
                                if (new Rectangle((int)pos1.X + 25, (int)enemy[i2].Y + 30, enemyup.Width / 3 - 50, (int)pos1.Y - (int)enemy[i2].Y - 70 + enemyup.Height).Intersects(wallsrect[i]) || new Rectangle((int)pos1.X + 25, (int)pos1.Y + 30, enemyup.Width / 3 - 50, (int)enemy[i2].Y - (int)pos1.Y - 70 + enemyup.Height).Intersects(wallsrect[i]))
                                    mx = false;
                        }
                        if (mx || my)
                        {
                            if (enemy[i2].X > pos1.X)
                            {
                                if (enemy[i2].Y > pos1.Y)
                                {
                                    enemyrotate[i2] = 5.498f;
                                }
                                if (enemy[i2].Y < pos1.Y)
                                {
                                    enemyrotate[i2] = 3.927f;
                                }
                                if (enemy[i2].Y == pos1.Y)
                                {
                                    enemyrotate[i2] = 4.712f;
                                }
                            }
                            if (enemy[i2].X < pos1.X)
                            {
                                if (enemy[i2].Y > pos1.Y)
                                {
                                    enemyrotate[i2] = 0.785f;
                                }
                                if (enemy[i2].Y < pos1.Y)
                                {
                                    enemyrotate[i2] = 2.356f;
                                }
                                if (enemy[i2].Y == pos1.Y)
                                {
                                    enemyrotate[i2] = 1.571f;
                                }
                            }
                            if (enemy[i2].X == pos1.X)
                            {
                                if (enemy[i2].Y > pos1.Y)
                                {
                                    enemyrotate[i2] = 0;
                                }
                                if (enemy[i2].Y < pos1.Y)
                                {
                                    enemyrotate[i2] = 3.142f;
                                }
                            }
                        }
                        else
                        {
                            //Random ran = new Random();
                            //enemyrotate[i2] = ran.Next(6283) / 1000;
                        }
                        if (!mx && !my)
                        {
                            currentenemyFrame[i2] = 1;
                            EnemyTime[i2] = 0;
                        }
                        else
                        {
                            EnemyTime[i2] += gameTime.ElapsedGameTime.Milliseconds;
                            if (EnemyTime[i2] > 100)                            //смена кадров врагов
                            {
                                EnemyTime[i2] = 0;
                                currentenemyFrame[i2]++;
                                if (currentenemyFrame[i2] >= 3)
                                {
                                    currentenemyFrame[i2] = 0;
                                }
                            }
                        }
                        if (enemy[i2].X > pos1.X)                            //передвижение обычных врагов и столкновение со стенами
                        {
                            
                            for (int i = 0; i < wallsrect.Count; i++)
                            {
                                if (new Rectangle((int)enemy[i2].X + 25 - (int)speed.X, (int)enemy[i2].Y + 30, enemyup.Width/3 - 50, enemyup.Height - 70).Intersects(wallsrect[i]))
                                { go = false; }
                            }
                            for (int i = 0; i < enemyrect.Count; i++)
                            {
                                if (i2 != i)
                                    if (new Rectangle((int)enemy[i2].X + 25 - (int)speed.X, (int)enemy[i2].Y + 30, enemyup.Width/3 - 50, enemyup.Height - 70).Intersects(enemyrect[i]))
                                { go = false; }
                            }
                            if (mx) 
                            if (go == true) enemy[i2] = new Vector2(enemy[i2].X-speed.X,enemy[i2].Y); //enemy[i2].X -= speed.X;
                            go = true;
                            mx = true;
                        }
                        if (enemy[i2].X < pos1.X)                            //передвижение обычных врагов и столкновение со стенами
                        {
                            for (int i = 0; i < wallsrect.Count; i++)
                            {
                                if (new Rectangle((int)enemy[i2].X + 25 + (int)speed.X, (int)enemy[i2].Y + 30, enemyup.Width/3 - 50, enemyup.Height - 70).Intersects(wallsrect[i]))
                                { go = false; }
                            }
                            for (int i = 0; i < enemyrect.Count; i++)
                            {

                                if (i2 != i)
                                    if (new Rectangle((int)enemy[i2].X + 25 + (int)speed.X, (int)enemy[i2].Y + 30, enemyup.Width/3 - 50, enemyup.Height - 70).Intersects(enemyrect[i]))
                                    { go = false; }
                            }
                            if (mx)
                            if (go == true) enemy[i2] = new Vector2(enemy[i2].X + speed.X, enemy[i2].Y); //enemy[i2].X += speed.X;
                            go = true;
                            mx = true;
                        }
                        if (enemy[i2].Y > pos1.Y)                            //передвижение обычных врагов и столкновение со стенами
                        {
                            for (int i = 0; i < wallsrect.Count; i++)
                            {
                                if (new Rectangle((int)enemy[i2].X + 25, (int)enemy[i2].Y + 30 - (int)speed.Y, enemyup.Width/3 - 50, enemyup.Height - 70).Intersects(wallsrect[i]))
                                { go = false; }
                            }
                            for (int i = 0; i < enemyrect.Count; i++)
                            {

                                if (i2 != i)
                                    if (new Rectangle((int)enemy[i2].X + 25, (int)enemy[i2].Y + 30 - (int)speed.Y, enemyup.Width/3 - 50, enemyup.Height - 70).Intersects(enemyrect[i]))
                                    { go = false; }
                            }
                            if (my)
                            if (go == true) enemy[i2] = new Vector2(enemy[i2].X, enemy[i2].Y - speed.Y); //enemy[i2].Y -= speed.Y;
                            go = true;
                            my = true;
                        }
                        if (enemy[i2].Y < pos1.Y)                            //передвижение обычных врагов и столкновение со стенами
                        {
                            for (int i = 0; i < wallsrect.Count; i++)
                            {
                                if (new Rectangle((int)enemy[i2].X + 25, (int)enemy[i2].Y + 30 + (int)speed.Y, enemyup.Width/3 - 50, enemyup.Height - 70).Intersects(wallsrect[i]))
                                { go = false; }
                            }
                            for (int i = 0; i < enemyrect.Count; i++)
                            {

                                if (i2 != i)
                                    if (new Rectangle((int)enemy[i2].X + 25, (int)enemy[i2].Y + 30 - (int)speed.Y, enemyup.Width/3 - 50, enemyup.Height - 70).Intersects(enemyrect[i]))
                                    { go = false; }
                            }
                            if (my)
                            if (go == true) enemy[i2] = new Vector2(enemy[i2].X, enemy[i2].Y + speed.Y); // enemy[i2].Y += speed.Y;
                            go = true;
                             my = true;
                        }
                    }
                    //l        //l
                     cam = new Vector2(0, 0);                            //следующие 4 условия-сдвигание объектов при приближении к краю экрана
                    if (pos1.X > Window.ClientBounds.Width - heroup.Width/3-300) cam.X = cam.X - 4; //pos1.X = Window.ClientBounds.Width - img1.Width;
                    if (pos1.X < 300) cam.X = cam.X + 4; //pos1.X = 0;
                    if (pos1.Y > Window.ClientBounds.Height - heroup.Height-200) cam.Y = cam.Y - 4;//pos1.Y = Window.ClientBounds.Height - img1.Height;
                    if (pos1.Y < 200) cam.Y = cam.Y + 4;//pos1.Y = 0;

                    //floorpos += cam;
                    //floorcam += cam;
                    //if ((floorcam.X >= 60) || (floorcam.X <=-60))
                    //{ floorpos.X = -60; floorcam.X = 0; }
                    //if ((floorcam.Y >= 60) || (floorcam.Y <=-60))
                    //{ floorpos.Y = -60; floorcam.Y = 0; }
                    break;
                case GameState.GameOver:                            //состояние игры - поражение
                    if (isbtoom)
                    {
                        timebtoom += gameTime.ElapsedGameTime.Milliseconds;
                        if (timebtoom > 800) { isbtoom = false; btoompos = new Vector2(-1000, -1000); timebtoom = 0; timeSinceLastFrame = 0; currentFrame = 0; }
                        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                        if (timeSinceLastFrame > 100)
                        {
                            timeSinceLastFrame = 0;
                            currentFrame++;
                            if (currentFrame >= 8)
                            {
                                currentFrame = 0;
                            }
                        }
                    }

                         menutime += gameTime.ElapsedGameTime.Milliseconds;
                    if (menutime > 300)
                    {
                        if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.S))
                        { if (menuchose == 1) { menuchose = 2; } else { menuchose = 1; } menutime = 0; }
                    }
                    if (keyboardState.IsKeyDown(Keys.Enter))
                        if (menutime > 200) 
                            switch (menuchose)
                            {
                                case 1:
                                        //обнуление для следующего уровня
                                    herorotate = 0; p = 1;menutime = 0; wallsp.Clear(); wallsrect.Clear(); enemy.Clear(); deadenemy.Clear();enemyrotate.Clear(); pos1 = Vector2.Zero;  menuchose = 0;grenadepos = new Vector2(-1000,-1000); grenadethown = false; timesinceX = 0;  timesinceY = 0; grenadespeed = new Vector2(0,0);  grenadespeed1 = new Vector2(0, 0); grenadethrowtime = 0; timebtoom = 0; isbtoom = false; cam = new Vector2(0, 0); currentFrame = 0; currentheroFrame = 0; currentenemyFrame.Clear(); currentgrenade = 0; timeSinceLastFrame = 0; HeroTime = 0; EnemyTime.Clear(); mapch = 1; btoompos = new Vector2(-10000,-10000); btoomrect = Rectangle.Empty;grenaders.Clear();grenaderspos.Clear();
                                        //обнуление
                                    //levels.Read(ref wallspos, ref wallsrect, ref enemy, ref grenaderspos, map);
                                    levels.Read(ref wallsp, ref enemy, ref grenaderspos, map);
                                foreach (var h in grenaderspos)
                                { grenaders.Add(new Grenader()); grenaders[grenaders.Count - 1].pos = h; grenaders[grenaders.Count - 1].rotate = 0; }
                                    for (int i = 0; i < enemy.Count; i++)
                                        enemyrotate.Add(0);
                                    currentGameState = GameState.InGame;
                                    break;
                                case 2:
                                    //обнуление для след уровня
                                    herorotate = 0; p = 1;menutime = 0; wallsp.Clear(); wallsrect.Clear(); enemy.Clear(); deadenemy.Clear(); enemyrotate.Clear(); pos1 = Vector2.Zero; menuchose = 0; grenadepos = new Vector2(-1000, -1000); grenadethown = false; timesinceX = 0; timesinceY = 0; grenadespeed = new Vector2(0, 0); grenadespeed1 = new Vector2(0, 0); grenadethrowtime = 0; timebtoom = 0; isbtoom = false; cam = new Vector2(0, 0); currentFrame = 0; currentheroFrame = 0; currentenemyFrame.Clear(); currentgrenade = 0; timeSinceLastFrame = 0; HeroTime = 0; EnemyTime.Clear(); mapch = 1;btoompos = new Vector2(-10000,-10000); btoomrect = Rectangle.Empty;grenaders.Clear();grenaderspos.Clear();
                                    //обнуление
                                    currentGameState = GameState.Start;
                                    break;
                            }
                    break;
                case GameState.Win:                            //состояние игры - победа
                    if (isbtoom)
                    {
                        timebtoom += gameTime.ElapsedGameTime.Milliseconds;
                        if (timebtoom > 800) { isbtoom = false; btoompos = new Vector2(-1000, -1000); timebtoom = 0; timeSinceLastFrame = 0; currentFrame = 0; }
                        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                        if (timeSinceLastFrame > 100)
                        {
                            timeSinceLastFrame = 0;
                            currentFrame++;
                            if (currentFrame >= 8)
                            {
                                currentFrame = 0;
                            }
                        }
                    }
                    
                    menutime += gameTime.ElapsedGameTime.Milliseconds;
                    if (menutime > 300)
                    {
                        if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.S))
                        { if (menuchose == 1) { menuchose = 2; } else { menuchose = 1; } menutime = 0; }
                    }
                    if (keyboardState.IsKeyDown(Keys.Enter))
                        if (menutime > 200)
                            switch (menuchose)
                            {
                                case 1:
                                    //обнуление для след игры
                                    herorotate = 0; p = 1;menutime = 0; wallsp.Clear(); wallsrect.Clear(); enemy.Clear(); deadenemy.Clear(); enemyrotate.Clear(); pos1 = Vector2.Zero; menuchose = 0; grenadepos = new Vector2(-1000, -1000); grenadethown = false; timesinceX = 0; timesinceY = 0; grenadespeed = new Vector2(0, 0); grenadespeed1 = new Vector2(0, 0); grenadethrowtime = 0; timebtoom = 0; isbtoom = false; cam = new Vector2(0, 0); currentFrame = 0; currentheroFrame = 0; currentenemyFrame.Clear(); currentgrenade = 0; timeSinceLastFrame = 0; HeroTime = 0; EnemyTime.Clear(); mapch = 1;btoompos = new Vector2(-10000,-10000); btoomrect = Rectangle.Empty;grenaderspos.Clear();grenaders.Clear();
                                    //обнуление
                                    //levels.Read(ref wallspos, ref wallsrect, ref enemy, ref grenaderspos, map);
                                    levels.Read(ref wallsp, ref enemy, ref grenaderspos, map);
                                foreach (var h in grenaderspos)
                                { grenaders.Add(new Grenader()); grenaders[grenaders.Count - 1].pos = h; grenaders[grenaders.Count - 1].rotate = 0; }
                                    for (int i = 0; i < enemy.Count; i++)
                                        enemyrotate.Add(0);
                                    currentGameState = GameState.InGame;
                                    break;
                                case 2:
                                    //обнуление для след игры
                                    herorotate = 0; p = 1;menutime = 0; wallsp.Clear(); wallsrect.Clear(); enemy.Clear(); deadenemy.Clear(); enemyrotate.Clear(); pos1 = Vector2.Zero; menuchose = 0; grenadepos = new Vector2(-1000, -1000); grenadethown = false; timesinceX = 0; timesinceY = 0; grenadespeed = new Vector2(0, 0); grenadespeed1 = new Vector2(0, 0); grenadethrowtime = 0; timebtoom = 0; isbtoom = false; cam = new Vector2(0, 0); currentFrame = 0; currentheroFrame = 0; currentenemyFrame.Clear(); currentgrenade = 0; timeSinceLastFrame = 0; HeroTime = 0; EnemyTime.Clear(); mapch = 1;btoompos = new Vector2(-10000,-10000); btoomrect = Rectangle.Empty;grenaderspos.Clear();grenaders.Clear();
                                    //обнуление
                                    currentGameState = GameState.Start;
                                    break;
                            }
                    break;
            }
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)                            //здесь выводятся изображения
        {
            GraphicsDevice.Clear(Color.AliceBlue);                            //фон экрана
            switch (currentGameState)
            {
                case GameState.Start:                            //состояние игры - главное меню. здесь вывод изображений главного меню

                    switch (menuchose)
                        {
                            case 0:
                                {
                                    spriteBatch.Begin();
                                    spriteBatch.Draw(menu0, new Vector2(Window.ClientBounds.Width / 2 - menu0.Width / 2, Window.ClientBounds.Height / 2 - menu0.Height / 2), Color.White);
                                    spriteBatch.End();
                                }
                                break;
                            case 1:
                                {
                                    spriteBatch.Begin();
                                    spriteBatch.Draw(menu1, new Vector2(Window.ClientBounds.Width / 2 - menu1.Width / 2, Window.ClientBounds.Height / 2 - menu1.Height / 2), Color.White);
                                    spriteBatch.End();
                                }
                                break;
                            case 2:
                                {
                                    spriteBatch.Begin();
                                    spriteBatch.Draw(menu2, new Vector2(Window.ClientBounds.Width / 2 - menu2.Width / 2, Window.ClientBounds.Height / 2 - menu2.Height / 2), Color.White);
                                    spriteBatch.End();
                                }
                                break;
                            case 3:
                                {
                                    spriteBatch.Begin();
                                    spriteBatch.Draw(menu3, new Vector2(Window.ClientBounds.Width / 2 - menu3.Width / 2, Window.ClientBounds.Height / 2 - menu3.Height / 2), Color.White);
                                    spriteBatch.End();
                                }
                                break;
                            case 4:
                                {
                                    spriteBatch.Begin();
                                    spriteBatch.Draw(menu4, new Vector2(Window.ClientBounds.Width / 2 - menu4.Width / 2, Window.ClientBounds.Height / 2 - menu4.Height / 2), Color.White);
                                    spriteBatch.End();
                                }
                                break;
                        }
                            if (menuchose > 4 && menuchose < 5 + maps)
                            {
                                switch (mapch)
                                { 
                                    case 1:
                                        {
                                            spriteBatch.Begin();
                                            spriteBatch.Draw(menu5, new Vector2(Window.ClientBounds.Width / 2 - menu4.Width / 2, Window.ClientBounds.Height / 2 - menu4.Height / 2), Color.White);
                                            spriteBatch.DrawString(font, (menuchose - 4).ToString(), new Vector2(Window.ClientBounds.Width / 2 + gameover2.Width / 8, Window.ClientBounds.Height / 2 - gameover2.Height / 45*8), Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
                                            spriteBatch.DrawString(font, (menuchose + 1 - 4).ToString(), new Vector2(Window.ClientBounds.Width / 2 + gameover2.Width / 8, Window.ClientBounds.Height / 2 - gameover2.Height / 27), Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
                                            spriteBatch.DrawString(font, (menuchose + 2 - 4).ToString(), new Vector2(Window.ClientBounds.Width / 2 + gameover2.Width / 8, Window.ClientBounds.Height / 2 + gameover2.Height / 9), Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
                                            spriteBatch.End();
                                        }
                                        break;
                                    case 2:
                                        {
                                            spriteBatch.Begin();
                                            spriteBatch.Draw(menu6, new Vector2(Window.ClientBounds.Width / 2 - menu4.Width / 2, Window.ClientBounds.Height / 2 - menu4.Height / 2), Color.White);
                                            spriteBatch.DrawString(font, (menuchose - 1 - 4).ToString(), new Vector2(Window.ClientBounds.Width / 2 + gameover2.Width / 8, Window.ClientBounds.Height / 2 - gameover2.Height / 45 * 8), Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
                                            spriteBatch.DrawString(font, (menuchose - 4).ToString(), new Vector2(Window.ClientBounds.Width / 2 + gameover2.Width / 8, Window.ClientBounds.Height / 2 - gameover2.Height / 27), Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
                                            spriteBatch.DrawString(font, (menuchose + 1 - 4).ToString(), new Vector2(Window.ClientBounds.Width / 2 + gameover2.Width / 8, Window.ClientBounds.Height / 2 + gameover2.Height / 9), Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 1); 
                                            spriteBatch.End();
                                        }
                                        break;
                                    case 3:
                                        {
                                            spriteBatch.Begin();
                                            spriteBatch.Draw(menu7, new Vector2(Window.ClientBounds.Width / 2 - menu4.Width / 2, Window.ClientBounds.Height / 2 - menu4.Height / 2), Color.White);
                                            spriteBatch.DrawString(font, (menuchose - 2 - 4).ToString(), new Vector2(Window.ClientBounds.Width / 2 + gameover2.Width / 8, Window.ClientBounds.Height / 2 - gameover2.Height / 45 * 8), Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
                                            spriteBatch.DrawString(font, (menuchose - 1 - 4).ToString(), new Vector2(Window.ClientBounds.Width / 2 + gameover2.Width / 8, Window.ClientBounds.Height / 2 - gameover2.Height / 27), Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
                                            spriteBatch.DrawString(font, (menuchose - 4).ToString(), new Vector2(Window.ClientBounds.Width / 2 + gameover2.Width / 8, Window.ClientBounds.Height / 2 + gameover2.Height / 9), Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
                                            spriteBatch.End();
                                        }
                                        break;
                                }
                            }
                            if (menuchose == 5+maps)
                            {
                                {
                                    spriteBatch.Begin();
                                    spriteBatch.Draw(menu8, new Vector2(Window.ClientBounds.Width / 2 - menu8.Width / 2, Window.ClientBounds.Height / 2 - menu8.Height / 2), Color.White);
                                    spriteBatch.DrawString(font, (menuchose - 2 - 5).ToString(), new Vector2(Window.ClientBounds.Width / 2 + gameover2.Width / 8, Window.ClientBounds.Height / 2 - gameover2.Height / 45 * 8), Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
                                    spriteBatch.DrawString(font, (menuchose - 1 - 5).ToString(), new Vector2(Window.ClientBounds.Width / 2 + gameover2.Width / 8, Window.ClientBounds.Height / 2 - gameover2.Height / 27), Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
                                    spriteBatch.DrawString(font, (menuchose - 5).ToString(), new Vector2(Window.ClientBounds.Width / 2 + gameover2.Width / 8, Window.ClientBounds.Height / 2 + gameover2.Height / 9), Color.Black, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
                                    spriteBatch.End();
                                }}
                    break;
                case GameState.InGame:                            //состояние игры - сама игра(вывод персонажей, фона, гранат, взрывов, стен)
                    spriteBatch.Begin();
                    spriteBatch.Draw(grass, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
                    //spriteBatch.Draw(grass, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 0.9f, SpriteEffects.None, 1);
                    for (int i = 0; i < deadenemy.Count; i++)
                    spriteBatch.Draw(deade, deadenemy[i], null, Color.White, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0.5f);
                    for (int i = 0; i < grenaders.Count; i++)
                        if (grenaders[i].alive)
                        {
                            spriteBatch.Draw(enemyup, grenaders[i].pos + new Vector2(enemyup.Width / 6, enemyup.Height / 2), new Rectangle(grenaders[i].frame * enemyup.Width / 3, 0, enemyup.Width / 3, enemyup.Height), Color.White, grenaders[i].rotate, new Vector2(enemyup.Width / 6, enemyup.Height / 2), 0.8f, SpriteEffects.None, 0.2f);
                            //spriteBatch.Draw(red, grenaders[i].rect, null, Color.White, 0, new Vector2(heroup.Width / 6, heroup.Height / 2), SpriteEffects.None, 0.9f);
                            ////f 
                        }
                        else
                            spriteBatch.Draw(deade, grenaders[i].pos, null, Color.White, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0.5f);
                    if (currentenemyFrame.Count >0)
                        for (int i = 0; i < enemy.Count; i++)
                    {
                        
                        spriteBatch.Draw(enemyup, enemy[i] + new Vector2(enemyup.Width / 6, enemyup.Height / 2), new Rectangle(currentenemyFrame[i] * enemyup.Width / 3, 0, enemyup.Width / 3, enemyup.Height), Color.White, enemyrotate[i], new Vector2(enemyup.Width / 6, enemyup.Height / 2), 0.8f, SpriteEffects.None, 0.2f);

                        //spriteBatch.Draw(red, new Rectangle((int)enemy[i].X + 25, (int)enemy[i].Y + 30, enemyup.Width / 3 - 50, enemyup.Height - 70), null, Color.White, 0, new Vector2(heroup.Width / 6, heroup.Height / 2), SpriteEffects.None, 0.9f);
                        //                //f
                    }
                    //            spriteBatch.Draw(red, new Rectangle((int)pos1.X + 25, (int)pos1.Y + 30, heroup.Width / 3 - 50, heroup.Height - 70), null, Color.White, 0, new Vector2(heroup.Width / 6, heroup.Height / 2), SpriteEffects.None, 0.9f);
                    ////f

                    
                    //for (int i2 = 0; i2 < enemy.Count; i2++)
                    //{
                        //if (enemy[i2].X < pos1.X)
                        //spriteBatch.Draw(red, new Rectangle((int)pos1.X + 25, (int)enemy[i2].Y + 30, (int)enemy[i2].X - (int)pos1.X - 50 + enemyup.Width/3, enemyup.Height - 70), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.9f);
                        //x
                        //else
                        //spriteBatch.Draw(red, new Rectangle((int)enemy[i2].X + 25, (int)enemy[i2].Y + 30, (int)pos1.X - (int)enemy[i2].X - 50 + enemyup.Width/3, enemyup.Height - 70), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.9f);

                        //if (enemy[i2].X < pos1.X)
                        //spriteBatch.Draw(white, new Rectangle((int)pos1.X + 25, (int)pos1.Y + 30, (int)enemy[i2].X - 50 - (int)pos1.X + enemyup.Width/3, enemyup.Height - 70), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.9f);
                        //y
                        //else
                        //spriteBatch.Draw(white, new Rectangle((int)enemy[i2].X + 25, (int)pos1.Y + 30, (int)pos1.X - (int)enemy[i2].X - 50 + enemyup.Width/3, enemyup.Height - 70), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.9f);

                        //if (enemy[i2].Y < pos1.Y)
                        //spriteBatch.Draw(red, new Rectangle((int)enemy[i2].X + 25, (int)enemy[i2].Y + 30, enemyup.Width / 3 - 50, (int)pos1.Y - (int)enemy[i2].Y - 70 + enemyup.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.9f);
                        //y
                        //else
                        //spriteBatch.Draw(red, new Rectangle((int)enemy[i2].X + 25, (int)pos1.Y + 30, enemyup.Width / 3 - 50, (int)enemy[i2].Y - (int)pos1.Y - 70 + enemyup.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.9f);

                        
                        //if (enemy[i2].Y < pos1.Y)
                        //spriteBatch.Draw(white, new Rectangle((int)pos1.X + 25, (int)enemy[i2].Y + 30, enemyup.Width / 3 - 50, (int)pos1.Y - (int)enemy[i2].Y - 70 + enemyup.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.9f);
                        //x
                        //else
                        //spriteBatch.Draw(white, new Rectangle((int)pos1.X + 25, (int)pos1.Y + 30, enemyup.Width / 3 - 50, (int)enemy[i2].Y - (int)pos1.Y - 70 + enemyup.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.9f);

                    //}


                    spriteBatch.Draw(heroup, pos1 + new Vector2(heroup.Width / 6, heroup.Height / 2), new Rectangle(currentheroFrame * heroup.Width / 3, 0, heroup.Width / 3, heroup.Height), Color.White, herorotate, new Vector2(heroup.Width / 6, heroup.Height / 2), 0.8f, SpriteEffects.None, 0.3f);
                    //for (int i=0; i < wallsrect.Count;i++ )spriteBatch.Draw(red, wallsrect[i], null, Color.White, 0, new Vector2(heroup.Width / 6, heroup.Height / 2), SpriteEffects.None, 0.1f);
                    //            //f
                                foreach (var v in wallsp)
                        for (int ii = (int)v.Y; ii < (int)v.W; ii += 50)
                            for (int i = (int)v.X; i < (int)v.Z; i += 50)
                                spriteBatch.Draw(wall, new Vector2(i, ii), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.2f);
                    
                        

                    spriteBatch.Draw(btoom, btoompos,new Rectangle(currentFrame * btoom.Width / 9, 0, btoom.Width / 9, btoom.Height),Color.White, 0, Vector2.Zero,1.5f, SpriteEffects.None, 0.1f);
                    spriteBatch.Draw(grenade, grenadepos, new Rectangle(currentgrenade * grenade.Width / 5, 0, grenade.Width / 5, grenade.Height), Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0);
                    foreach (var gr in grenaders)
                    {
                        spriteBatch.Draw(grenade, gr.grenadepos, new Rectangle(gr.currentgrenade * grenade.Width / 5, 0, grenade.Width / 5, grenade.Height), Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0);
                        spriteBatch.Draw(btoom, gr.btoompos, new Rectangle(gr.currentFrame * btoom.Width / 9, 0, btoom.Width / 9, btoom.Height), Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0.1f);
                    }
                    spriteBatch.End();
            break;
                case GameState.GameOver:                            //состояние игры - поражение (вывод надписи "вы проиграли", пунктов для выбора, и последнюю картинку процесса игры)
                    spriteBatch.Begin();
                    spriteBatch.Draw(grass,new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), null,Color.White, 0, Vector2.Zero,SpriteEffects.None, 1); 
                    for (int i = 0; i < deadenemy.Count; i++)
                    spriteBatch.Draw(deade, deadenemy[i], null, Color.White, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0.5f);
                    for (int i = 0; i < grenaders.Count; i++)
                        if (grenaders[i].alive)
                            spriteBatch.Draw(enemyup, grenaders[i].pos + new Vector2(enemyup.Width / 6, enemyup.Height / 2), new Rectangle(grenaders[i].frame * enemyup.Width / 3, 0, enemyup.Width / 3, enemyup.Height), Color.White, grenaders[i].rotate, new Vector2(enemyup.Width / 6, enemyup.Height / 2), 0.8f, SpriteEffects.None, 0.2f);
                        else
                            spriteBatch.Draw(deade, grenaders[i].pos, null, Color.White, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0.5f);
                            for (int i = 0; i < enemy.Count; i++)
                        spriteBatch.Draw(enemyup, enemy[i] + new Vector2(enemyup.Width / 6, enemyup.Height / 2), new Rectangle(currentenemyFrame[i] * enemyup.Width / 3, 0, enemyup.Width / 3, enemyup.Height), Color.White, enemyrotate[i], new Vector2(enemyup.Width / 6, enemyup.Height / 2), 0.8f, SpriteEffects.None, 0.3f);
                    //spriteBatch.Draw(hero, pos1,new Rectangle(currentheroFrame * hero.Width / 3, 0, hero.Width / 3, hero.Height),Color.White, 0, Vector2.Zero,0.8f, SpriteEffects.None, 0.3f);
                    spriteBatch.Draw(deadhero, pos1, null,Color.White, 0, Vector2.Zero,0.8f, SpriteEffects.None, 0.4f);
                    foreach (var v in wallsp)
                        for (int ii = (int)v.Y; ii < (int)v.W; ii += 50)
                            for (int i = (int)v.X; i < (int)v.Z; i += 50)
                                spriteBatch.Draw(wall, new Vector2(i, ii), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
                    spriteBatch.Draw(btoom, btoompos,new Rectangle(currentFrame * btoom.Width / 9, 0, btoom.Width / 9, btoom.Height),Color.White, 0, Vector2.Zero,1.5f, SpriteEffects.None, 0.2f);
                    spriteBatch.Draw(grenade, grenadepos, new Rectangle(currentgrenade * grenade.Width / 5, 0, grenade.Width / 5, grenade.Height), Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0.1f);
                    foreach (var gr in grenaders)
                    {
                        spriteBatch.Draw(grenade, gr.grenadepos, new Rectangle(gr.currentgrenade * grenade.Width / 5, 0, grenade.Width / 5, grenade.Height), Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0.1f);
                        spriteBatch.Draw(btoom, gr.btoompos, new Rectangle(gr.currentFrame * btoom.Width / 9, 0, btoom.Width / 9, btoom.Height), Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0.2f);
                    }
                    if (menuchose == 1)
                        spriteBatch.Draw(gameover1, new Vector2(Window.ClientBounds.Width / 2 - gameover1.Width / 2, Window.ClientBounds.Height / 2 - gameover1.Height / 2), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    else
                        spriteBatch.Draw(gameover2, new Vector2(Window.ClientBounds.Width / 2 - gameover2.Width / 2, Window.ClientBounds.Height / 2 - gameover2.Height / 2), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);     
                    int f = 0; foreach (var g in grenaders) if (!g.alive) f++;
                    spriteBatch.DrawString(font,(f+deadenemy.Count).ToString(), new Vector2(Window.ClientBounds.Width / 2 + gameover2.Width / 4, Window.ClientBounds.Height / 2 - gameover2.Height / 25), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    spriteBatch.End();
            break;
                case GameState.Win:                           //состояние игры - победа (вывод надписи "вы победили", пунктов для выбора, и последнюю картинку процесса игры)
                    spriteBatch.Begin();
                    spriteBatch.Draw(grass,new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), null,Color.White, 0, Vector2.Zero,SpriteEffects.None, 1); 
                    for (int i = 0; i < deadenemy.Count; i++)
                    spriteBatch.Draw(deade, deadenemy[i], null, Color.White, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0.5f);
                    for (int i = 0; i < grenaders.Count; i++)
                            spriteBatch.Draw(deade, grenaders[i].pos, null, Color.White, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(heroup, pos1 + new Vector2(heroup.Width / 6, heroup.Height / 2), new Rectangle(currentheroFrame * heroup.Width / 3, 0, heroup.Width / 3, heroup.Height), Color.White, herorotate, new Vector2(heroup.Width / 6, heroup.Height / 2), 0.8f, SpriteEffects.None, 0.3f);
                    foreach (var v in wallsp)
                        for (int ii = (int)v.Y; ii < (int)v.W; ii += 50)
                            for (int i = (int)v.X; i < (int)v.Z; i += 50)
                                spriteBatch.Draw(wall, new Vector2(i, ii), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
                    spriteBatch.Draw(btoom, btoompos,new Rectangle(currentFrame * btoom.Width / 9, 0, btoom.Width / 9, btoom.Height),Color.White, 0, Vector2.Zero,1.5f, SpriteEffects.None, 0.2f);
                    spriteBatch.Draw(grenade, grenadepos, new Rectangle(currentgrenade * grenade.Width / 5, 0, grenade.Width / 5, grenade.Height), Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0.1f);
                    foreach (var gr in grenaders)
                    {
                        spriteBatch.Draw(grenade, gr.grenadepos, new Rectangle(gr.currentgrenade * grenade.Width / 5, 0, grenade.Width / 5, grenade.Height), Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0.1f);
                        spriteBatch.Draw(btoom, gr.btoompos, new Rectangle(gr.currentFrame * btoom.Width / 9, 0, btoom.Width / 9, btoom.Height), Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0.2f);
                    }
                    if (menuchose == 1)
                        spriteBatch.Draw(win1, new Vector2(Window.ClientBounds.Width / 2 - win1.Width / 2, Window.ClientBounds.Height / 2 - win1.Height / 2), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    else
                        spriteBatch.Draw(win2, new Vector2(Window.ClientBounds.Width / 2 - win2.Width / 2, Window.ClientBounds.Height / 2 - win2.Height / 2), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font, map.ToString(), new Vector2(Window.ClientBounds.Width / 2 + gameover2.Width / 12, Window.ClientBounds.Height / 2 - gameover2.Height / 10), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font,(grenaders.Count + deadenemy.Count).ToString(), new Vector2(Window.ClientBounds.Width / 2 + gameover2.Width / 4, Window.ClientBounds.Height / 2 - gameover2.Height / 25), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    spriteBatch.End();
            break;
            }
            // TODO: Add your drawing code here

            base.Draw(gameTime);
           

        }
    }
}
