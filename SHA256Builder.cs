using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace ProjectName
{
    public partial class FormAddUsers : Form
    {
        // Модель для взаимодействия с БД
        ModelEF model = new ModelEF();

        public FormAddUsers()
        {
            InitializeComponent();
        }

        // Метод загрузки данных в DataGridView
        void StartLoad()
        {
            dataGridView1.DataSource = model.UsersHash.ToList();
        }

        private void FormAddUsers_Load(object sender, EventArgs e)
        {
            StartLoad();
        }

        // Событие добавления данных в таблицу UsersHash
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (loginTextBox.Text == "" ||
                passwordTextBox.Text == "" ||
                firstNameTextBox.Text == "")
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            // Создание объекта пользователя
            UsersHash usersHash = new UsersHash();
            usersHash.Login = loginTextBox.Text;
            usersHash.Password = ConvertToHash(passwordTextBox.Text);
            usersHash.FirstName = firstNameTextBox.Text;

            try
            {
                // Добавление объекта в список и сохранение
                model.UsersHash.Add(usersHash);
                model.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                // Обновление данных
                StartLoad();
            }

            loginTextBox.Text = "";
            passwordTextBox.Text = "";
            firstNameTextBox.Text = "";

            MessageBox.Show("Данные добавлены");
        }

        // Метод хэширования пароля (SHA-256)
        string ConvertToHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));

                return builder.ToString();
            }
        }

        private void buttonAuthorization_Click(object sender, EventArgs e)
        {
            // Открытие формы авторизации
            FormAuthorization form = new FormAuthorization();
            form.ShowDialog();
        }
    }
}
