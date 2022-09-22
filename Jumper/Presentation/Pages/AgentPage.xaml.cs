using Jumper.Presentation.Windows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Jumper.Presentation.Pages
{
    /// <summary>
    /// Interaction logic for AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        private List<Agent> _ListAgent;

        public AgentPage()
        {
            InitializeComponent();
            LoadAgent();
        }

        private void LoadAgent()
        {
            const string PATH = @"D:\Пользователи\Рабочий стол\Попрыженок - агенты";
            _ListAgent = new JumperContext().Agents.ToList();

            foreach (var agent in _ListAgent)
                agent.Logo = String.Concat(PATH, agent.Logo);

            ListView.ItemsSource = _ListAgent;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTxtBox.Text == String.Empty)
            {
                ListView.ItemsSource = _ListAgent;
                return;
            }

            ListView.ItemsSource = _ListAgent.FindAll(agent
                => $"{agent.Title} {agent.Email} {agent.Phone}".Contains(SearchTxtBox.Text));
        }

        private void ButtonAddClick(object sender, RoutedEventArgs e)
        {
            new AgentAddWindow().ShowDialog();
            LoadAgent();
        }

        private void ButtonEditClick(object sender, RoutedEventArgs e)
        {
            if (ListView.SelectedIndex != -1)
            {
                new AgentEditWindow(((Agent)ListView.SelectedItem).Id).ShowDialog();
                LoadAgent();
            }
        }

        private void ButtonDeleteClick(object sender, RoutedEventArgs e)
        {
            if (ListView.SelectedIndex != -1)
            {
                var dbContext = new JumperContext();
                var SelectedAgent = dbContext.Agents.FirstOrDefault(Agent => Agent.Id == ((Agent)ListView.SelectedItem).Id);
                dbContext.Remove(SelectedAgent);
                dbContext.SaveChanges();
                LoadAgent();
            }
        }

        private void ComboBoxFilters_DropDownClosed(object sender, EventArgs e)
        {
            if (ComboBoxSortType.SelectedIndex == 0)
                ListView.ItemsSource = _ListAgent.OrderBy(agent => agent.AgentType.Title);

            else
                ListView.ItemsSource = _ListAgent.Where(Agent => Agent.AgentType.Title == ComboBoxFilters.Text);
        }

        private void ComboBoxSortType_DropDownClosed(object sender, EventArgs e)
        {
            if (ComboBoxSortType.SelectedIndex == 0)
                ListView.ItemsSource = _ListAgent.OrderBy(agent => agent.Title);

            if (ComboBoxSortType.SelectedIndex == 1)
                ListView.ItemsSource = _ListAgent.OrderByDescending(agent => agent.Title);
        }
    }
}

