using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for AgentEditWindow.xaml
    /// </summary>
    public partial class AgentEditWindow : Window
    {
        private string _LogoPath;
        private Agent _Agent;

        public AgentEditWindow(int id)
        {
            InitializeComponent();

            ComboBoxEditType.ItemsSource = new JumperContext().AgentTypes.ToList();
            _Agent = new JumperContext().Agents.First(Agent => Agent.Id == id);
            SetTextField();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            var dbContext = new JumperContext();
            try
            {
                if (_Agent == null)
                {
                    MessageBox.Show("Агент не найден");
                    this.Close();
                    return;
                }

                _Agent.Title = TextBoxEditTitle.Text;
                _Agent.AgentTypeId = dbContext.AgentTypes.First(AgentType => AgentType.Title == ((AgentType)ComboBoxEditType.SelectedItem).Title).Id;
                _Agent.Address = TextBoxEditAddres.Text;
                _Agent.Inn = TextBoxEditInn.Text;
                _Agent.Kpp = TextBoxEditKpp.Text;
                _Agent.DirectorName = TextBoxEditDirector.Text;
                _Agent.Phone = TextBoxEditPhone.Text;
                _Agent.Email = TextBoxEditMail.Text;
                _Agent.Logo = _LogoPath;
                _Agent.Priority = Convert.ToInt32(TextBoxEditPriority.Text);

                dbContext.Agents.Update(_Agent);
                dbContext.SaveChanges();

                MessageBox.Show("Агент успешно добавлен");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ButtonEditLogo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (open.ShowDialog() == true)
                _LogoPath = "\\" + $"{ open.SafeFileName} ";
        }

        private void SetTextField()
        {
            TextBoxEditTitle.Text = _Agent.Title;
            TextBoxEditAddres.Text = _Agent.Address;
            TextBoxEditInn.Text = _Agent.Inn;
            TextBoxEditKpp.Text = _Agent.Kpp;
            TextBoxEditDirector.Text = _Agent.DirectorName;
            TextBoxEditPhone.Text = _Agent.Phone;
            TextBoxEditMail.Text = _Agent.Email;
            TextBoxEditPriority.Text = _Agent.Priority.ToString();
        }
    }
}
