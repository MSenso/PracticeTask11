using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PracticeTask11
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Label code_input_label, code_output_label;
        TextBox code_input_box;
        const int code_coeff = 3;
        void Read_FromFile()
        {
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Открытие текстового файла";
            openFileDialog1.Filter = "Текстовые файлы|*.txt";
            openFileDialog1.InitialDirectory = "";
            string[] filelines = null;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                filelines = File.ReadAllLines(filename);
            }
            if (filelines != null)
            {
                if (filelines.Length == 1)
                {
                    Print_Input(0, filelines[0]);
                }
                else MessageBox.Show($"В файле содержится больше одной строки!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        string Decoding(string input, int input_case)
        {
            string output = string.Empty;
            char first_symbol = '0', second_symbol = '1';
            switch (input_case)
            {
                case 0:
                    {
                        first_symbol = '0';
                        second_symbol = '1';
                        break;
                    }
                case 1:
                    {
                        first_symbol = '.';
                        second_symbol = '–';
                        break;
                    }
            }
            int counter = 0;
            int first_symbol_count = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == first_symbol)
                {
                    first_symbol_count++;
                }
                counter++;
                if (counter % code_coeff == 0)
                {
                    if (first_symbol_count > code_coeff - first_symbol_count) output += first_symbol;
                    else output += second_symbol;
                    counter = 0;
                    first_symbol_count = 0;
                }
            }
            return output;
        }
        void Input_Check(string line)
        {
            string code_sequence = line.Replace('-', '–');
            if (code_sequence.Length > 0)
            {
                if (code_sequence.Length <= 1000)
                {
                    if (code_sequence.Length % code_coeff == 0)
                    {
                        bool is_numeric_input = code_sequence.All(ch => ch == '1' || ch == '0');
                        bool is_symbolic_input = code_sequence.All(ch => ch == '–' || ch == '.');
                        string result = string.Empty;
                        if (is_numeric_input || is_symbolic_input)
                        {
                            if (is_numeric_input) result = Decoding(code_sequence, 0);
                            else result = Decoding(code_sequence, 1);
                            Print_Output(result);
                        }
                        else MessageBox.Show("Последовательность имеет некорректные символы!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else MessageBox.Show($"Последовательность имеет длину, не кратную {code_coeff}!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else MessageBox.Show("Последовательность имеет более тысячи символов!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else MessageBox.Show("Пустая строка!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        void Print_Input(int input_case, [Optional] string line)
        {
            if (input_case == 0)
            {
                if (!Length_Check(line)) return;
            }
            code_input_label = new Label()
            {
                Name = "Code_Input_Label",
                Location = new Point(13, 50),
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Font = new Font(открытьФайлToolStripMenuItem.Font.FontFamily, 14, открытьФайлToolStripMenuItem.Font.Style)
            };
            switch(input_case)
            {
                case 0:
                    {
                        code_input_label.Text = "Сообщение: " + line;
                        break;
                    }
                case 1:
                    {
                        code_input_label.Text = "Введите сообщение: ";
                        break;
                    }
            }
            Controls.Add(code_input_label);
            if (input_case == 1)
            {
                code_input_box = new TextBox()
                {
                    Name = "Code_Input_TextBox",
                    Size = new Size(415, 41),
                    Location = new Point(code_input_label.Location.X, code_input_label.Location.Y + code_input_label.Height + 20),
                    BackColor = Color.FromArgb(255, 245, 248),
                    ForeColor = Color.Black,
                    Font = new Font(открытьФайлToolStripMenuItem.Font.FontFamily, 14, открытьФайлToolStripMenuItem.Font.Style)
                };
                code_input_box.KeyDown += new KeyEventHandler(Input_KeyDown);
                Controls.Add(code_input_box);
                code_input_box.Focus();
            }
            else Input_Check(line);
        }
        bool Length_Check(string line)
        {
            if (line.Length == 0)
            {
                MessageBox.Show("Пустая строка!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (line.Length > 1000)
            {
                MessageBox.Show("Последовательность имеет более тысячи символов!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (line.Length % code_coeff != 0)
            {
                MessageBox.Show($"Последовательность имеет длину, не кратную {code_coeff}!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else return true;
        }
        void Print_Output(string result)
        {
            code_output_label = new Label()
            {
                Name = "Code_Output_label",
                Text = "Расшифровка: " + result,
                AutoSize = true,
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                Font = new Font(открытьФайлToolStripMenuItem.Font.FontFamily, 14, открытьФайлToolStripMenuItem.Font.Style)
            };
            if (code_input_box == null)
            {
                code_output_label.Location = new Point(code_input_label.Location.X, code_input_label.Location.Y + code_input_label.Height + 20);
            }
            else
            {
                code_output_label.Location = new Point(code_input_box.Location.X, code_input_box.Location.Y + code_input_box.Height + 20);
            }
            Controls.Add(code_output_label);
        }
        void Remove_Elements()
        {
            if (code_input_label != null)
            {
                Controls.Remove(code_input_label);
                code_input_label = null;
            }
            if (code_input_box != null)
            {
                Controls.Remove(code_input_box);
                code_input_box = null;
            }
            if (code_output_label != null)
            {
                Controls.Remove(code_output_label);
                code_output_label = null;
            }
        }
        void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (code_output_label != null)
                {
                    Controls.Remove(code_output_label);
                    code_output_label = null;
                }
                if (Length_Check((sender as TextBox).Text)) Input_Check((sender as TextBox).Text);
            }
        }

        private void ввестиВручнуюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Remove_Elements();
            Print_Input(1);
        }

        private void открытьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Remove_Elements();
            Read_FromFile();
        }
    }
}
