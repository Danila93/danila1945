using System;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Check;

namespace TestsEditor
{
    public partial class HelloForm : Form
    {
        XmlTextWriter testWriter; //XmlTextWriter для записи *.xml-файла с тестом

        public HelloForm()
        {
            InitializeComponent();

            if (!Checking.DataChecking())
                MessageBox.Show("Некоторые файлы, необходимые для стабильной работы приложения, не были обнаружены!!!\nНастоятельно рекомендуем переустановить программу!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            DirectoryInfo TestsDir = new DirectoryInfo("Tests"); //Создаем DiryctoryInfo соответствующее папке Tests c тестами

            if (!TestsDir.Exists) //Если папки Tests нет
                TestsDir.Create(); //Создаем её

            comboBox1.Items.AddRange(TestsDir.GetDirectories()); //Заполняем comboBox1 поддерикториями папки Tests
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && ThemeBox.Text != "" && NameBox.Text != "")
            {
                try
                {
                    testWriter = new XmlTextWriter("Tests\\" + comboBox1.Text + "\\" + NameBox.Text + ".xml", Encoding.UTF8); //Пытаемся создать файл теста
                }
                catch (DirectoryNotFoundException) //Если поддериктории (предмета тестирования) нет
                {
                    Directory.CreateDirectory("Tests\\" + comboBox1.Text); //Создаем директорию
                    testWriter = new XmlTextWriter("Tests\\" + comboBox1.Text + "\\" + NameBox.Text + ".xml", Encoding.UTF8); //Создаем файл теста
                }

                testWriter.Formatting = Formatting.Indented; //Обеспечиваем читабельность файла

                //Начинаем документ
                testWriter.WriteStartDocument(); //Открываем документ
                testWriter.WriteStartElement("test"); //Открываем основной тэг - test

                //Записываем тему
                testWriter.WriteStartElement("Theme"); //Открываем тэг темы
                testWriter.WriteString(ThemeBox.Text); //Записываем тему
                testWriter.WriteEndElement(); //Закрывем тэг темы

                //Записываем вопросы
                testWriter.WriteStartElement("qw"); //Открываем тэг вопросов
                testWriter.WriteStartAttribute("numbers"); //Записываем атрибут - количество вопросов
                testWriter.WriteString(NumQwBox.Value.ToString());
                testWriter.WriteEndAttribute();

                for (int i = 1; i <= NumQwBox.Value; i++)
                {
                    QwestForm QF = new QwestForm(i, testWriter);
                    QF.ShowDialog();
                }

                //Завершаем написание документа
                testWriter.WriteEndElement(); //Закрывем тэг вопросов
                testWriter.WriteEndElement(); //Закрывем основной тэг - test
                testWriter.WriteEndDocument(); //Закрываем документ

                testWriter.Close();

                MessageBox.Show("Все вопросы успешно созданы!", "Выход");
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!");
            }
        }
    }
}
