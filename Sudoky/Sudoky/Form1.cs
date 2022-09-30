using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoky
{
    public partial class Form1 : Form
    {

        const int n = 3;
        const int sizeButton = 50;
        public int[,] map = new int[n * n, n * n]; //массив с размером карты для игры в судоку
        public Button[,] buttons = new Button[n * n, n * n];

        public Form1()
        {
            InitializeComponent();
            textBox1.AcceptsReturn = textBox1.ReadOnly;
            GenerateMap();
        }

        public void GenerateMap() //функция генерация карты, здесь генерируем поле игры
        {
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    map[i, j] = (i * n + i / n + j) % (n * n) + i;
                }
            }
            //рандомно выбираем способ расстановки цифр
            Random r = new Random();
            for(int i = 0; i < 10; i++)
            {
                SuffleMap(r.Next(0, 5));
                
            }
            CreateMap();
            HideCells();
            
        }
        
        public void SuffleMap(int i)
        {
            /*функция для выбора алгоритма по которому пудут расставляться значения 
             * в поле игры. Их всего 5.
             */
            switch (i)
            {
                case 0:
                    MatrixTransponation();
                    break;
                case 1:
                    SwapRowsInBlock();
                    break;
                case 2:
                    SwapColumsInBlock();
                    break;
                case 3:
                    SwapBlocksInRow();
                    break;
                case 4:
                    SwapBlocksInColumm();
                    break;
                default:
                    MatrixTransponation();
                    break;
            }
        }

        public void HideCells()//функция для того, чтобы скрыть ячейки сгенерированные
        {
            int N = 40;
            Random r = new Random();
            while (N > 0)
            {
                for (int i = 0; i < n * n; i++)
                {
                    for (int j = 0; j < n * n; j++)
                    {
                        if (!string.IsNullOrEmpty(buttons[i, j].Text))
                        {
                            int a = r.Next(0, 3);
                            buttons[i, j].Text = a == 0 ? "" : buttons[i, j].Text;
                            buttons[i, j].Enabled = a == 0 ? true : false;
                            if (a == 0)
                                N--;
                            if (N <= 0)
                                break;
                        }
                        if (N <= 0)
                            break;
                    }
                }
            }
            

        }

        public void SwapBlocksInColumm()
        {
            Random r = new Random();
            var block1 = r.Next(0, n);
            var block2 = r.Next(0, n);
            while (block2 == block1)
            {
                block2 = r.Next(0, n);
            }
            block1 *= n;
            block2 *= n;
            for (int i = 0; i < n * n; i++)
            {
                var k = block2;
                for (int j = block1; j < block1 + n; j++)
                {
                    var temp = map[i, j];
                    map[i, j] = map[i, k];
                    map[i, k] = temp;
                    k++;
                }

            }
        }

        public void SwapBlocksInRow()
        {
            Random r = new Random();
            var block1 = r.Next(0, n);
            var block2 = r.Next(0, n);
            while (block1 == block2)
            {
                block2 = r.Next(0, n);
            }
            block1 *= n;
            block2 *= n;
            for (int i = 0; i < n * n; i++)
            {
                var k = block2;
                for(int j = block1; j < block1 +n; j++)
                {
                    var temp = map[j, i];
                    map[j, i] = map[k, i];
                    map[k, i] = temp;
                    k++;
                }
                
            }
        }

        public void SwapRowsInBlock()
        {
            Random r = new Random();
            var block = r.Next(0, n);
            var row1 = r.Next(0, n);
            var line1 = block * n + row1;
            var row2 = r.Next(0, n);
            while (row2 == row1)
            {
                row2 = r.Next(0, n);
            }
            var line2 = block * n + row2;
            for(int i = 0; i < n*n; i++)
            {
                var temp = map[line1, i];
                map[line1, i] = map[line2, i];
                map[line2, i] = temp;
            }
        }

        public void SwapColumsInBlock()
        {
            Random r = new Random();
            var block = r.Next(0, n);
            var row1 = r.Next(0, n);
            var line1 = block * n + row1;
            var row2 = r.Next(0, n);
            while (row2 == row1)
            {
                row2 = r.Next(0, n);
            }
            var line2 = block * n + row2;
            for (int i = 0; i < n * n; i++)
            {
                var temp = map[i, line1];
                map[i,line1] = map[i, line2];
                map[i,line2] = temp;
            }
        }

        public void MatrixTransponation()
        {
            int[,] tMap = new int[n * n, n * n];
            for(int i = 0; i < n*n; i++)
            {
                for(int j = 0; j < n*n; j++)
                {
                    tMap[i, j] = map[j, i];
                }
            }
            map = tMap;
        }
        public void CreateMap() //функция создания карты
        /*В этой функции создаем "кнопки" и заполняем их цифрами, которые лежат в массиве map
         * Таким образом у нас получится поле для игры в судоку
         */
        {
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    Button button = new Button();
                    buttons[i, j] = button;
                    button.Size = new Size(sizeButton, sizeButton);
                    button.Text = map[i, j].ToString();
                    button.Click += OnCellPressed; 
                    button.Location = new Point(j * sizeButton, i * sizeButton);
                    this.Controls.Add(button);
                   
                }
            }
            MessageBox.Show("Введите пожалуйста имя игрока в поле в правом верхнем углу!");
        }

        public void OnCellPressed(object sender, EventArgs e)
        {
            /*функция для генерации цифр
             */
            Button pressedButton = sender as Button;
            string buttonText = pressedButton.Text;
            if(string.IsNullOrEmpty(buttonText))
            {
                pressedButton.Text = "1";
            }
            else
            {
                int num = int.Parse(buttonText);
                num++;
                if(num == 10)
                {
                    num = num - 1;
                }
                pressedButton.Text = num.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)//функция проверки
        /*Необходимо проверить правильность ввода пользователем значений
         * Для этого сравним то что ввел пользователь с тем, что лежит в массиве map
         * Если пользователь ввел ошибочное значение, то выводи сообщение "Неверно!"
         * Если пользователь ввел все правильно, то сообщение "Верно!"
         * После победы завершаем игру и генерируем поле сначала
         */
        {
            for(int i = 0; i < n*n; i++)
            {
                for(int j = 0; j < n*n; j++)
                {
                    var btnText = buttons[i, j].Text;
                    if(btnText != map[i,j].ToString())
                    {
                        MessageBox.Show("Неверно!");
                        return;
                    }
                }
            }
            MessageBox.Show("Верно!");
            for(int i = 0; i < n*n; i++)
            {
                for(int j = 0; j < n*n; j++)
                {
                    this.Controls.Remove(buttons[i, j]);
                }
            }
            GenerateMap();
        }

        private void button2_Click(object sender, EventArgs e)//функция для завершения игры
        {
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    this.Controls.Remove(buttons[i, j]);
                }
            }
            GenerateMap();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
