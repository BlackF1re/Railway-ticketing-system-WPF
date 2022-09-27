using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Railway_ticketing_system_WPF_
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        DataBase dataBase = new DataBase();//!!!!!
        public RegistrationWindow()
        {
            InitializeComponent();
            RegistrationLoginEntry.MaxLength = 20;
            RegistrationPwdEntry.MaxLength = 20;
            RegistrationPwdEntry_repeated.MaxLength = 20;
            FnameEntry.MaxLength = 20;
            LnameEntry.MaxLength = 20;
        }

        private void RegistrationCancellingButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void RegistrationDataClearButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationLoginEntry.Clear();
            RegistrationPwdEntry.Clear();
            RegistrationPwdEntry_repeated.Clear();
            FnameEntry.Clear();
            LnameEntry.Clear();

        }

        private void MakeRegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            var registrationLogin = RegistrationLoginEntry.Text;
            var registrationPwd = Convert.ToString(RegistrationPwdEntry.Password);
            var registrationPwdRepeated = Convert.ToString(RegistrationPwdEntry_repeated);//необходимо для проверки пароля. Значение в таблицу не заносится
            var fname = FnameEntry.Text;
            var lname = LnameEntry.Text;

            if (
                RegistrationLoginEntry.Text.Length > 0 &&
                RegistrationPwdEntry.Password.Length > 0 &&
                FnameEntry.Text.Length > 0 &&
                LnameEntry.Text.Length > 0)
            {
                if (registrationPwd == registrationPwdRepeated)
                {
                    string sqlQueryString = $"INSERT INTO LoginUsers (userLogin, userPassword, userFname, userLname) values (N'{registrationLogin}', N'{registrationPwd}', N'{fname}', N'{lname}')";
                    SqlCommand sqlCommand = new SqlCommand(sqlQueryString, dataBase.GetConnection());
                    dataBase.OpenConnection();

                    if (sqlCommand.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Вы зарегистрированы", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Аккаунт не создан", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    dataBase.CloseConnection();
                }
                else
                {
                    MessageBox.Show("Введенные пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    RegistrationPwdEntry.Clear();
                    RegistrationPwdEntry_repeated.Clear();
                }


            }

            else
            {
                MessageBox.Show("Необходимо заполнить все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private Boolean CheckRepeatingUser()
        {
            var registrationLogin = RegistrationLoginEntry.Text;
            var registrationPwd = Convert.ToString(RegistrationPwdEntry.Password);

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            string sqlQueryString = $"SELECT userID, userLogin, userPassword, userFname, userLname FROM LoginUsers WHERE userLogin = '{registrationLogin}' AND userPassword = '{registrationPwd}'";
            
            SqlCommand sqlCommand = new SqlCommand(sqlQueryString, dataBase.GetConnection());
            sqlDataAdapter.SelectCommand = sqlCommand;
            sqlDataAdapter.Fill(dataTable);
            if(dataTable.Rows.Count > 0)
            {
                MessageBox.Show("Такой пользователь уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else
            {
                return false;
            }

        }



    }
}
