using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Jumper.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for AgentAddWindow.xaml
    /// </summary>
    public partial class AgentAddWindow : Window
    {
        private string _LogoPath = @"\agents\picture.png";
        public AgentAddWindow()
        {
            InitializeComponent();
            ComboBoxAddType.ItemsSource = new JumperContext().AgentTypes.ToList();
        }

        private void ButtonReady_Click(object sender, RoutedEventArgs e)
        {
            var dbContext = new JumperContext();
            try
            {
                dbContext.Agents.Add(
                    new Agent()
                    {
                        Id = 0,
                        Title = TextBoxAddTitle.Text,
                        AgentTypeId = dbContext.AgentTypes.First(AgentType => AgentType.Title == ((AgentType)ComboBoxAddType.SelectedItem).Title).Id,
                        Address = TextBoxAddAddres.Text,
                        Inn = TextBoxAddInn.Text,
                        Kpp = TextBoxAddKpp.Text,
                        DirectorName = TextBoxAddDirector.Text,
                        Phone = TextBoxAddPhone.Text,
                        Email = TextBoxAddMail.Text,
                        Logo = _LogoPath,
                        Priority = Convert.ToInt32(TextBoxAddPriority.Text)
                    });
                dbContext.SaveChanges();

                MessageBox.Show("Агент успешно добавлен");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ButtonAddLogo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (open.ShowDialog() == true)
                _LogoPath = String.Concat(@"\agents\", open.SafeFileName);

        }
    }
}
