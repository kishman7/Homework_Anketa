using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using System.Data.SqlClient;

namespace Anketa
{
    public partial class Anketa : Form
    {
        public Anketa()
        {
            InitializeComponent();
            var country = new [] { "Ukraine", "USA", "England" };
            comboBox1.Items.AddRange(country);

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        

        string name; // імя
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            name = textBox1.Text;
        }

        string surname; //призвіще
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            surname = textBox2.Text;
        }

       
        int age; // кількість років
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            age = DateTime.Now.Year - dateTimePicker1.Value.Year;
            labelCountAge.Text = ($"{ age }");
        }


        string gender; // стать
        private void radioButtonMale_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile("../../../ImageProject/Male.png");
            gender = radioButtonMale.Text;
        }

        private void radioButtonFemale_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile("../../../ImageProject/Female.png");
            gender = radioButtonFemale.Text;
        }
        string country; // країна
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //country = comboBox1.Items;
        }

        string hobby; // хоббі
        private void checkBoxPrograming_CheckedChanged(object sender, EventArgs e)
        {
            hobby = checkBoxPrograming.Text;
        }

        private void checkBoxSport_CheckedChanged(object sender, EventArgs e)
        {
            hobby = checkBoxSport.Text;
        }

        private void textBoxOtherNobby_TextChanged(object sender, EventArgs e)
        {
            //hobby = textBoxOtherNobby.Text;
        }

        private void checkBoxOther_CheckedChanged(object sender, EventArgs e)
        {
            textBoxOtherNobby.Visible = checkBoxOther.Checked; //коли галочка, тоді зявляється поле для заповнення
        }
        
        private void buttonSave_Click(object sender, EventArgs e)
        {
            
            ///////////
            ///////////

            // серіалізація мтодом json
            var json = JsonConvert.SerializeObject(GetPerson(), Newtonsoft.Json.Formatting.Indented); // потрібно встановити через Newget пакети Newtonsoft.json
            File.WriteAllText("person.json", json); 

        }

        Person GetPerson()
        {
            var person = new Person();
            person.Age = dateTimePicker1.Value;
            person.Name = name;
            person.Surname = surname;
            person.Gender = gender;
            person.OtherHobby = textBoxOtherNobby.Text;
            person.Country = comboBox1.SelectedIndex; // вибір країни. обраний елемент колекцыъ по індексу
            // запис хобі в колецію
            foreach (var item in this.Controls)
            {
                if (item is CheckBox)
                {
                    if ((item as CheckBox).Checked)
                    {
                        person.hobby.Add((item as CheckBox).Text);
                    }
                }
            }
            return person;
        }


        private void buttonReadFromFile_Click(object sender, EventArgs e)
        {
            var json = File.ReadAllText("person.json");
            var person1 = JsonConvert.DeserializeObject<Person>(json);
            Debug.WriteLine(person1.Name);

            /////////
            textBox1.Text = person1.Name;
            textBox2.Text = person1.Surname;
            dateTimePicker1.Value = person1.Age;
            //comboBox1.Text = person1.Age;
            textBoxOtherNobby.Text = person1.OtherHobby;
            (((this.Controls.Find("radioButton" + person1.Gender, false)).First()) as RadioButton).Checked = true; // підгружається стать разои з ссилкою на картинку
            comboBox1.SelectedIndex = person1.Country;
            foreach (var item in person1.hobby)
            {
                foreach (var item1 in this.Controls)
                {
                    if (item1 is CheckBox)
                    {
                        if ((item1 as CheckBox).Text == item )
                        {
                            (item1 as CheckBox).Checked = true;
                        }
                    }
                }
                
            }
        }
        // Підключаємось до сервера та закидаємо туди дані
        private void button1_Click(object sender, EventArgs e)
        {

            string connectionString = @"Data Source=DESKTOP-48R2R1B\SQLEXPRESS;User ID=DodUser;Password=123;Initial Catalog=Persons;Integrated Security=False";

            // Создание подключения
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                // Открываем подключение
                connection.Open();
                //connection.ChangeDatabase("Persons"); //міняю на потрібну мені БД
                var per = GetPerson(); // передаємо обєкт класау в новий обєкт
                //Console.WriteLine("Подключение открыто");
                string hobby_other = ""; // при умові вибору кількох варіантів хоббі, ми в цю змінну записуємо всі вибрані варіанти
                foreach (string item in per.hobby)
                {
                    hobby_other += item + ",";
                }
                string sqlExpression = $"INSERT INTO Person  VALUES ('{per.Name}', '{per.Surname}', '{per.Age}', '{per.Country}', '{per.Gender}', '{per.OtherHobby}', '{hobby_other}')"; // записуємо дані в таблицю
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // закрываем подключение
                connection.Close();
                //Console.WriteLine("Подключение закрыто...");
            }

        }
    }
}
