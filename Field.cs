    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    namespace Mitya_plus_Kashkin_ravno_Saper;

    public class Field
    {
        private int height;
        private int width;
        private int mines;
        private Random rnd = new Random();
        public int[,] field;
        private Rectangle[,] map;
        private bool is_first_clk;
        private List<Rectangle> list_of_mines;
        public int count_flags;
        public int count_flags_to_win;
        public MainWindow game;


        public Field(int height, int width, int mines, MainWindow context)
        {
            this.mines = mines;
            this.height = height;
            this.width = width;
            this.field = new int[height, width];
            map = new Rectangle[height, width];
            is_first_clk = true;
            list_of_mines = new List<Rectangle>();
            count_flags = 0;
            game = context;


        }

        public void init_field(int fc_i, int fc_j)
        {
           
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    field[i, j] = 0;
                }
            }
            int mines_left = mines;
            while (mines_left != 0)
            {
                int place_i = rnd.Next(0, height);
                int place_j = rnd.Next(0, width);
                if ((field[place_i, place_j] != -1) && !((place_i == fc_i - 1 && place_j == fc_j - 1) 
                                                         || (place_i == fc_i - 1 && place_j == fc_j)  
                                                         || (place_i == fc_i - 1 && place_j == fc_j + 1)  
                                                         || (place_i == fc_i && place_j == fc_j + 1)  
                                                         || (place_i == fc_i + 1 && place_j == fc_j + 1)  
                                                         || (place_i == fc_i + 1 && place_j == fc_j)
                                                         || (place_i == fc_i + 1 && place_j == fc_j - 1)
                                                         || (place_i == fc_i && place_j == fc_j - 1)
                                                         || (place_i == fc_i && place_j == fc_j)))
                {
                    field[place_i, place_j] = -1;
                    mines_left--;
                    list_of_mines.Add(map[place_i, place_j]);
                }
            }

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (field[i, j] == -1)
                    {
                        if (i - 1 >= 0 && j - 1 >= 0)
                        {
                            if (field[i - 1, j - 1] != -1)
                            {
                                field[i - 1, j - 1]++;
                            }
                        }

                        if (i - 1 >= 0 && j >= 0)
                        {
                            if (field[i - 1, j] != -1)
                            {
                                field[i - 1, j]++;
                            }
                        }

                        if (i - 1 >= 0 && j + 1 <= width - 1)
                        {
                            if (field[i - 1, j + 1] != -1)
                            {
                                field[i - 1, j + 1]++;
                            }
                        }

                        if (i >= 0 && j + 1 <= width - 1)
                        {
                            if (field[i, j + 1] != -1)
                            {
                                field[i, j + 1]++;
                            }
                        }

                        if (i + 1 <= height - 1 && j + 1 <= width - 1)
                        {
                            if (field[i + 1, j + 1] != -1)
                            {
                                field[i + 1, j + 1]++;
                            }
                        }

                        if (i + 1 <= height - 1 && j >= 0)
                        {
                            if (field[i + 1, j] != -1)
                            {
                                field[i + 1, j]++;
                            }
                        }

                        if (i + 1 <= height - 1 && j - 1 >= 0)
                        {
                            if (field[i + 1, j - 1] != -1)
                            {
                                field[i + 1, j - 1]++;
                            }
                        }

                        if (i >= 0 && j - 1 >= 0)
                        {
                            if (field[i, j - 1] != -1)
                            {
                                field[i, j - 1]++;
                            }
                        }
                    }
                }
            }
        }
        public void init_grid(MainWindow context)
        {
            BitmapImage non = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\non.png", UriKind.Relative));
            for (int i = 0; i < height; i++)
            {
                context.Map.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < width; i++)
            {
                context.Map.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Rectangle cell = new Rectangle
                    {
                        Width = 35,
                        Height = 35,
                        Fill = new ImageBrush(non),
                        Stroke = Brushes.Orchid,
                        Tag = new Cell_Info(i, j, 0)


                    };
                    cell.MouseLeftButtonDown += CellOnLeftButtonDown;
                    cell.MouseRightButtonDown += CellOnRightButtonDown; 
                    context.Map.Children.Add(cell);
                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);
                    map[i, j] = cell;
                    
                    

                }
            }

            is_first_clk = true;
        }
        

        
        private void CellOnLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            Rectangle clk_cell = e.Source as Rectangle;
            Cell_Info clk_tag = (Cell_Info)clk_cell.Tag;
            if (is_first_clk)
            {
                init_field(clk_tag.cell_i, clk_tag.cell_j);
                is_first_clk = false;
            }

            if (clk_tag.cell_status != 2)
            {
                clk_tag.cell_status = 1;
                switch (field[clk_tag.cell_i, clk_tag.cell_j])
                {
                    case -1:
                        BitmapImage mine_sprite = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\mine.png",
                            UriKind.Relative));
                        clk_cell.Fill = new ImageBrush(mine_sprite);
                        foreach (var mine in list_of_mines)
                        {
                            mine.Fill = new ImageBrush(mine_sprite);
                            Cell_Info mine_tag = (Cell_Info)mine.Tag;
                            mine_tag.cell_status = 1;
                        }

                        MessageBox.Show("Поражение!");
                        game.restartgame();
                        break;
                    case 0:
                        BitmapImage empty = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\0.png",
                            UriKind.Relative));
                        clk_cell.Fill = new ImageBrush(empty);
                        Stack<Point> stack_of_empty = new Stack<Point>();
                        stack_of_empty.Push(new Point(clk_tag.cell_i, clk_tag.cell_j));
                        clk_tag.cell_status = 1;
                        while (stack_of_empty.Count > 0)
                        {
                            Point pt = stack_of_empty.Pop();
                            if (pt.i - 1 >= 0 && pt.j - 1 >= 0)
                            {
                                reveal(pt.i - 1, pt.j - 1, stack_of_empty);
                            }

                            //-------------------
                            if (pt.i - 1 >= 0 && pt.j >= 0)
                            {

                                reveal(pt.i - 1, pt.j, stack_of_empty);
                            }

                            //-------------------
                            if (pt.i - 1 >= 0 && pt.j + 1 < width)
                            {
                                reveal(pt.i - 1, pt.j + 1, stack_of_empty);
                            }

                            //-------------------
                            if (pt.i >= 0 && pt.j + 1 < width)
                            {
                                reveal(pt.i, pt.j + 1, stack_of_empty);
                            }

                            //-------------------
                            if (pt.i + 1 < height && pt.j + 1 < width)
                            {
                                reveal(pt.i + 1, pt.j + 1, stack_of_empty);
                            }

                            //-------------------
                            if (pt.i + 1 < height && pt.j >= 0)
                            {
                                reveal(pt.i + 1, pt.j, stack_of_empty);
                            }

                            //-------------------
                            if (pt.i + 1 < height && pt.j - 1 >= 0)
                            {
                                reveal(pt.i + 1, pt.j - 1, stack_of_empty);
                            }

                            //-------------------
                            if (pt.i >= 0 && pt.j - 1 >= 0)
                            {
                                reveal(pt.i, pt.j - 1, stack_of_empty);
                            }
                            //-------------------
                        }
                        break;
                    case 1:
                        BitmapImage one = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\1.png",
                            UriKind.Relative));
                        clk_cell.Fill = new ImageBrush(one);
                        break;
                    case 2:
                        BitmapImage two = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\2.png",
                            UriKind.Relative));
                        clk_cell.Fill = new ImageBrush(two);
                        break;
                    case 3:
                        BitmapImage three = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\3.png",
                            UriKind.Relative));
                        clk_cell.Fill = new ImageBrush(three);
                        break;
                    case 4:
                        BitmapImage four = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\4.png",
                            UriKind.Relative));
                        clk_cell.Fill = new ImageBrush(four);
                        break;
                    case 5:
                        BitmapImage five = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\5.png",
                            UriKind.Relative));
                        clk_cell.Fill = new ImageBrush(five);
                        break;
                    case 6:
                        BitmapImage six = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\6.png",
                            UriKind.Relative));
                        clk_cell.Fill = new ImageBrush(six);
                        break;
                    case 7:
                        BitmapImage seven = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\7.png",
                            UriKind.Relative));
                        clk_cell.Fill = new ImageBrush(seven);
                        break;
                    case 8:
                        BitmapImage eight = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\8.png",
                            UriKind.Relative));
                        clk_cell.Fill = new ImageBrush(eight);
                        break;
                }


            }

        }
        
        private void CellOnRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle clk_cell = e.Source as Rectangle;
            Cell_Info clk_tag = (Cell_Info)clk_cell.Tag;
            if (clk_tag.cell_status == 0)
            {
                clk_tag.cell_status = 2;
                BitmapImage flag = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\flag.png", UriKind.Relative));
                clk_cell.Fill = new ImageBrush(flag);
                count_flags++;
                if (field[clk_tag.cell_i, clk_tag.cell_j] == -1)
                {
                    count_flags_to_win++;
                }
                if (count_flags_to_win == mines && count_flags_to_win == count_flags)
                {
                    MessageBox.Show("Ура, победа!");
                    game.restartgame();
                }
                return;
            }
            if (clk_tag.cell_status == 2)
            {
                clk_tag.cell_status = 0;
                BitmapImage non = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\non.png", UriKind.Relative));
                clk_cell.Fill = new ImageBrush(non);
                count_flags--;
                if (field[clk_tag.cell_i, clk_tag.cell_j] == -1)
                {
                    count_flags_to_win--;
                }
                if (count_flags_to_win == mines)
                {
                    MessageBox.Show("Ура, победа!");
                }
            }

            
        }
        
        public void reveal(int i, int j, Stack<Point> stack)
        {
            Rectangle cell = map[i, j];
            Cell_Info cell_tag = (Cell_Info)cell.Tag;
            if (field[i, j] == 0 && cell_tag.cell_status == 0)
            {
                stack.Push(new Point(i, j));
            }

            cell_tag.cell_status = 1;
            switch (field[i, j])
            {
                    case 0:
                        BitmapImage empty = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\0.png",
                            UriKind.Relative));
                        cell.Fill = new ImageBrush(empty);
                    break;
                    case 1:
                        BitmapImage one = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\1.png",
                            UriKind.Relative));
                        cell.Fill = new ImageBrush(one);
                        break;
                    case 2:
                        BitmapImage two = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\2.png",
                            UriKind.Relative));
                        cell.Fill = new ImageBrush(two);
                        break;
                    case 3:
                        BitmapImage three = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\3.png",
                            UriKind.Relative));
                        cell.Fill = new ImageBrush(three);
                        break;
                    case 4:
                        BitmapImage four = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\4.png",
                            UriKind.Relative));
                        cell.Fill = new ImageBrush(four);
                        break;
                    case 5:
                        BitmapImage five = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\5.png",
                            UriKind.Relative));
                        cell.Fill = new ImageBrush(five);
                        break;
                    case 6:
                        BitmapImage six = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\6.png",
                            UriKind.Relative));
                        cell.Fill = new ImageBrush(six);
                        break;
                    case 7:
                        BitmapImage seven = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\7.png",
                            UriKind.Relative));
                        cell.Fill = new ImageBrush(seven);
                        break;
                    case 8:
                        BitmapImage eight = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Sprites\\8.png",
                            UriKind.Relative));
                        cell.Fill = new ImageBrush(eight);
                        break;
            }
        }

        
    }