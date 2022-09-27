using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Railway_ticketing_system_WPF_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataBase dataBase = new DataBase();

        public MainWindow()
        {

            InitializeComponent();
            AuthorizationLoginBox.MaxLength = 20;
            AuthorizationPwdBox.MaxLength = 20;
        }

        private void AuthorizationGO_Click(object sender, RoutedEventArgs e)
        {

            string authorizationLogin = Convert.ToString(AuthorizationLoginBox.Text);
            string authorizationPwd = Convert.ToString(AuthorizationPwdBox.Password);
            if (authorizationLogin.Length > 0 || authorizationPwd.Length > 0)
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable dataTable = new DataTable();
                string sqlQueryString = $"SELECT userID , userLogin , userPassword FROM LoginUsers where userLogin = N'{authorizationLogin}' AND userPassword = N'{authorizationPwd}'";
                SqlCommand sqlCommand = new SqlCommand(sqlQueryString, dataBase.GetConnection());

                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Проверьте введенные данные и повторите попытку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    //AuthorizationLoginBox.Clear();
                    AuthorizationPwdBox.Clear();
                }
                else
                {
                    UserAccount userAccount = new UserAccount();
                    userAccount.Show();
                    this.Close();
                }
            }

            else
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        private void RegistrationHyperlink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }
    }
}
