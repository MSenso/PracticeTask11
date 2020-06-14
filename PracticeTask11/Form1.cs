using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        const int code_coeff = 3; // Каким количеством кодируется каждый символ
        public void Read_FromFile() // Чтение из файла
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
        public string Decoding(string input, int input_case) // Расшифровка
        {
            string output = string.Empty;
            char first_symbol = '0', second_symbol = '1';
            if (input_case == 1) // Символьный ввод
            {
                first_symbol = '.';
                second_symbol = '–';
            }
            int counter = 0; // Счетчик символа в очередной трйоке чисел
            int first_symbol_count = 0; // Количество символов №1 в тройке
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == first_symbol) // Встретился символ №1
                {
                    first_symbol_count++; // Количество символов №1 увеличилось
                }
                counter++; // Счетчик увеличился
                if (counter % code_coeff == 0) // Обработано 3 символа
                {
                    if (first_symbol_count > code_coeff - first_symbol_count) output += first_symbol; // Символов №1 больше, в выходную строку добавляется символ №1
                    else output += second_symbol; // Иначе добавляется символ №2
                    first_symbol_count = 0; // Обнуление счетчика символов №1
                }
            }
            return output;
        }
        public void Input_Check(string line) // Проверка ввода
        {
            string code_sequence = line.Replace('-', '–'); // Замена дефиса на тире
            if (code_sequence.Length > 0) // Длина последовательности больше 0
            {
                if (code_sequence.Length < 1000) // Длина последовательности меньше 0
                {
                    if (code_sequence.Length % code_coeff == 0) // Длина последовательности кратна 3
                    {
                        bool is_numeric_input = code_sequence.All(ch => ch == '1' || ch == '0'); // Числовой ввод, строка только из нулей и единиц
                        bool is_symbolic_input = code_sequence.All(ch => ch == '–' || ch == '.'); // Символьный ввод, строка только из тире и точек
                        string result = string.Empty;
                        if (is_numeric_input || is_symbolic_input) // Числовой или символьный ввод
                        {
                            // Расшифровка с учетом типа ввода
                            if (is_numeric_input) result = Decoding(code_sequence, 0);
                            else result = Decoding(code_sequence, 1);
                            Print_Output(result); // Вывод
                        }
                        else MessageBox.Show("Последовательность имеет некорректные символы!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else MessageBox.Show($"Последовательность имеет длину, не кратную {code_coeff}!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else MessageBox.Show("Последовательность имеет более тысячи символов!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else MessageBox.Show("Пустая строка!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void Print_Input(int input_case, [Optional] string line) // Вывод входной строки
        {
            if (input_case == 0) // Введена из файла
            {
                if (!Length_Check(line)) return; // Проверка длины
            }
            code_input_label = new Label()
            {
                Name = "Code_Input_Label",
                Location = new Point(13, 50),
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Font = new Font(открытьФайлToolStripMenuItem.Font.FontFamily, 14, открытьФайлToolStripMenuItem.Font.Style)
            }; // Метка для вывода входной строки
            switch(input_case)
            {
                case 0: // Ввод из файла
                    {
                        code_input_label.Text = "Сообщение: " + line;
                        break;
                    }
                case 1: // Ввод вручную
                    {
                        code_input_label.Text = "Введите сообщение: ";
                        break;
                    }
            }
            Controls.Add(code_input_label);
            if (input_case == 1) // Ввод вручную
            {
                code_input_box = new TextBox() // Текстбокс для ввода
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
            else Input_Check(line); // Проверка ввода
        }
        bool Length_Check(string line)
        {
            if (line.Length > 1000) // Длина больше 1000
            {
                MessageBox.Show("Последовательность имеет более тысячи символов!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (line.Length % code_coeff != 0) // Длина не кратна 3
            {
                MessageBox.Show($"Последовательность имеет длину, не кратную {code_coeff}!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else return true;
        }
        void Print_Output(string result) // Вывод расшифрованной строки
        {
            code_output_label = new Label() // Метка для вывода расшифрованной строки
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
        void Remove_Elements() // Очистка формы
        {
            if (code_input_label != null) // Еще не удалено
            {
                Controls.Remove(code_input_label); // Удаление
                code_input_label = null;
            }
            if (code_input_box != null) // Еще не удалено
            {
                Controls.Remove(code_input_box); // Удаление
                code_input_box = null;
            }
            if (code_output_label != null) // Еще не удалено
            {
                Controls.Remove(code_output_label); // Удаление
                code_output_label = null;
            }
        }
        void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Нажат энтер
            {
                if (code_output_label != null) // Вывод не удален
                {
                    Controls.Remove(code_output_label); // Очистка результатов
                    code_output_label = null;
                }
                if (Length_Check((sender as TextBox).Text)) Input_Check((sender as TextBox).Text);
            }
        }
        private void ввестиВручнуюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Remove_Elements(); // Очистка формы
            Print_Input(1); // Ввод вручную
        }

        private void открытьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Remove_Elements(); // Очистка формы
            Read_FromFile(); // Ввод из файла
        }
    }
}
