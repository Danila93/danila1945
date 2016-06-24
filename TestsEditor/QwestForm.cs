using System;
using System.Windows.Forms;
using System.Xml;

namespace TestsEditor
{
    public partial class QwestForm : Form
    {
        XmlTextWriter testWriter; //XmlTextWriter для записи *.xml-файла с тестом
        int count; //Целочисленное значение количества вопросов

        public QwestForm(int k, XmlTextWriter Writer)
        {
            testWriter = Writer;
            count = k;
            InitializeComponent();
            this.Text = "Редактирование вопроса №" + count;
        }

        private void QwestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (QwestBox.Text != "" && answ1.Text != "" && answ2.Text != "" && answ3.Text != "" && answ4.Text != "" && RightAnswerBox.Text != "")
            {
                if (RightAnswerBox.Text == answ1.Text || RightAnswerBox.Text == answ2.Text || RightAnswerBox.Text == answ3.Text || RightAnswerBox.Text == answ4.Text)
                {
                    //Записываем вопрос
                    testWriter.WriteStartElement("q" + count); //Открываем тэг вопроса - qN (где N - номер вопроса)

                    //Записываем текст вопроса в атрибут text
                    testWriter.WriteStartAttribute("text"); //Открываем атрибут text
                    testWriter.WriteString(QwestBox.Text); //Записываем атрибут text
                    testWriter.WriteEndAttribute(); //Закрываем атрибут text

                    //Записываем правильный ответ в атрибут right
                    testWriter.WriteStartAttribute("right"); //Открываем атрибут right
                    testWriter.WriteString(RightAnswerBox.Text); //Записываем атрибут right
                    testWriter.WriteEndAttribute(); //Закрываем атрибут right

                    //Записываем тэг с ответами
                    testWriter.WriteStartElement("answers"); //Открываем тэг answers
                    testWriter.WriteString(answ1.Text + "|" + answ2.Text + "|" + answ3.Text + "|" + answ4.Text); //Записываем ответы в тэг разделяя знаком '|'
                    testWriter.WriteEndElement(); //Закрываем тэг answers

                    testWriter.WriteEndElement(); //Закрываем тэг вопроса qN

                    this.Dispose(); //Закрываем форму
                }
                else
                {
                    MessageBox.Show("Правильный ответ не совпадает ни к одним из вариантов ответа!\n\nПодсказка: Скопируйте правильный вариант в поле правильного ответа!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!");
            }
        }
    }
}
