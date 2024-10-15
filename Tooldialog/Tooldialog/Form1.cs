using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tooldialog
{
    public partial class Form1 : Form
    {
        private Button[,] pixelGrid;
        private Color selectedColor = Color.Black; 
        private int gridSize = 16; 

        public Form1()
        {
            InitializeComponent();
            CreateGrid(gridSize);
            AddControlButtons(); 
        }

   
        private void CreateGrid(int size)
        {
            pixelGrid = new Button[size, size];
            int buttonSize = 40; 
            this.ClientSize = new Size(size * buttonSize + 160, size * buttonSize); 

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Button pixelButton = new Button();
                    pixelButton.Size = new Size(buttonSize, buttonSize);
                    pixelButton.Location = new Point(x * buttonSize, y * buttonSize);
                    pixelButton.BackColor = Color.White;
                    pixelButton.MouseDown += PixelButton_MouseDown;
                    pixelGrid[x, y] = pixelButton;
                    this.Controls.Add(pixelButton);
                }
            }
        }

        private void AddControlButtons()
        {
            // Botón para guardar como texto
            Button btnSaveText = new Button();
            btnSaveText.Text = "Guardar Dibujo (TXT)";
            btnSaveText.Size = new Size(140, 40);
            btnSaveText.Location = new Point(gridSize * 40 + 10, 20); 
            btnSaveText.Click += BtnSave_Click; 
            this.Controls.Add(btnSaveText); 


            Button btnSaveImage = new Button();
            btnSaveImage.Text = "Guardar Dibujo (PNG/BMP)";
            btnSaveImage.Size = new Size(140, 40); 
            btnSaveImage.Location = new Point(gridSize * 40 + 10, 70);
            btnSaveImage.Click += BtnSaveImage_Click; 
            this.Controls.Add(btnSaveImage); 


            Button btnClear = new Button();
            btnClear.Text = "Borrar Todo";
            btnClear.Size = new Size(140, 40); 
            btnClear.Location = new Point(gridSize * 40 + 10, 120); 
            btnClear.Click += BtnClear_Click; 
            this.Controls.Add(btnClear); 

      
            Button btnLoad = new Button();
            btnLoad.Text = "Cargar Dibujo (TXT)";
            btnLoad.Size = new Size(140, 40); 
            btnLoad.Location = new Point(gridSize * 40 + 10, 170);
            btnLoad.Click += BtnLoad_Click;
            this.Controls.Add(btnLoad); 

  
            Button btnLoadImage = new Button();
            btnLoadImage.Text = "Cargar Imagen (PNG/BMP)";
            btnLoadImage.Size = new Size(140, 40); 
            btnLoadImage.Location = new Point(gridSize * 40 + 10, 220); 
            btnLoadImage.Click += BtnLoadImage_Click;
            this.Controls.Add(btnLoadImage); 
        }

        private void PixelButton_MouseDown(object sender, MouseEventArgs e)
        {
            Button pixelButton = sender as Button;
            if (e.Button == MouseButtons.Left)
            {
                pixelButton.BackColor = selectedColor; 
            }
            else if (e.Button == MouseButtons.Right)
            {
                pixelButton.BackColor = Color.White; 
            }
        }



        private void SelectColor(Color color)
        {
            selectedColor = color;
        }


        private void btnSelectColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    SelectColor(colorDialog.Color);
                }
            }
        }


        private void BtnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text files (*.txt)|*.txt"; 
                saveFileDialog.Title = "Guardar Dibujo ASCII";
                saveFileDialog.FileName = "dibujo"; 

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveAsciiArt(saveFileDialog.FileName); 
                }
            }
        }

        private void SaveAsciiArt(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int y = 0; y < gridSize; y++)
                {
                    for (int x = 0; x < gridSize; x++)
                    {
                        Button pixelButton = pixelGrid[x, y];
                        if (pixelButton.BackColor == Color.Black)
                        {
                            writer.Write("*");
                        }
                        else
                        {
                            writer.Write(" ");
                        }
                    }
                    writer.WriteLine();
                }
            }
            MessageBox.Show("Dibujo guardado exitosamente!", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSaveImage_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG Image (*.png)|*.png|Bitmap Image (*.bmp)|*.bmp"; 
                saveFileDialog.Title = "Guardar Dibujo como Imagen";
                saveFileDialog.FileName = "dibujo"; 

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveImage(saveFileDialog.FileName); 
                }
            }
        }

        private void SaveImage(string filePath)
        {
            int buttonSize = 40;
            using (Bitmap bitmap = new Bitmap(gridSize * buttonSize, gridSize * buttonSize))
            {
                for (int x = 0; x < gridSize; x++)
                {
                    for (int y = 0; y < gridSize; y++)
                    {
                        Color color = pixelGrid[x, y].BackColor;
                        for (int i = 0; i < buttonSize; i++)
                        {
                            for (int j = 0; j < buttonSize; j++)
                            {
                                bitmap.SetPixel(x * buttonSize + i, y * buttonSize + j, color);
                            }
                        }
                    }
                }
                bitmap.Save(filePath); 
            }
            MessageBox.Show("Dibujo guardado como imagen exitosamente!", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            foreach (var button in pixelGrid)
            {
                button.BackColor = Color.White;
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt"; 
                openFileDialog.Title = "Cargar Dibujo ASCII";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadAsciiArt(openFileDialog.FileName); 
                }
            }
        }

        private void LoadAsciiArt(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            for (int y = 0; y < gridSize; y++)
            {
                if (y < lines.Length)
                {
                    for (int x = 0; x < gridSize; x++)
                    {
                        if (x < lines[y].Length)
                        {

                            pixelGrid[x, y].BackColor = (lines[y][x] == '*') ? Color.Black : Color.White;
                        }
                    }
                }
            }
            MessageBox.Show("Dibujo cargado exitosamente!", "Cargado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.png;*.bmp)|*.png;*.bmp"; 
                openFileDialog.Title = "Cargar Imagen";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadImage(openFileDialog.FileName); 
                }
            }
        }


        private void LoadImage(string filePath)
        {
            using (Bitmap bitmap = new Bitmap(filePath))
            {
                if (bitmap.Width != gridSize * 40 || bitmap.Height != gridSize * 40)
                {
                    MessageBox.Show("La imagen debe ser de tamaño " + (gridSize * 40) + "x" + (gridSize * 40), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                for (int y = 0; y < gridSize; y++)
                {
                    for (int x = 0; x < gridSize; x++)
                    {
                        Color pixelColor = bitmap.GetPixel(x * 40, y * 40);
                        if (pixelColor.ToArgb() == Color.Black.ToArgb())
                        {
                            pixelGrid[x, y].BackColor = Color.Black; 
                        }
                        else
                        {
                            pixelGrid[x, y].BackColor = Color.White; 
                        }
                    }
                }

                this.Refresh();
            }

            MessageBox.Show("Imagen cargada exitosamente!", "Cargado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
