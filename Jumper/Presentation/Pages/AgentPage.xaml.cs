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
        private string _SearchParameter = String.Empty;
        private int _SortParameter = 0;
        private int _FilterParameter = 0;
        private const string _PATH = @"D:\Пользователи\Рабочий стол\Попрыженок - агенты";

        public AgentPage()
        {
            InitializeComponent();
            LoadAgent();
        }

        private void LoadAgent()
        {
            _ListAgent = new JumperContext().Agents.ToList();

            foreach (var agent in _ListAgent)
                agent.Logo = String.Concat(_PATH, agent.Logo);

            AgentListView.ItemsSource = _ListAgent;
        }

        private void ButtonAddClick(object sender, RoutedEventArgs e)
        {
            new AgentAddWindow().ShowDialog();
            LoadAgent();
        }

        private void ButtonEditClick(object sender, RoutedEventArgs e)
        {
            if (AgentListView.SelectedIndex != -1)
            {
                new AgentEditWindow(((Agent)AgentListView.SelectedItem).Id).ShowDialog();
                LoadAgent();
            }
        }

        private void ButtonDeleteClick(object sender, RoutedEventArgs e)
        {
            if (AgentListView.SelectedIndex != -1)
            {
                var dbContext = new JumperContext();
                var SelectedAgent = dbContext.Agents.FirstOrDefault(Agent => Agent.Id == ((Agent)AgentListView.SelectedItem).Id);
                dbContext.Remove(SelectedAgent);
                dbContext.SaveChanges();
                LoadAgent();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _SearchParameter = SearchTxtBox.Text;
            SetFilterAndSortType();
        }

        private void ComboBoxFilters_DropDownClosed(object sender, EventArgs e)
        {
            _FilterParameter = ComboBoxFilters.SelectedIndex;
            SetFilterAndSortType();
        }

        private void ComboBoxSortType_DropDownClosed(object sender, EventArgs e)
        {
            _SortParameter = ComboBoxSortType.SelectedIndex;
            SetFilterAndSortType();
        }

        private void SetFilterAndSortType()
        {
            if (_SortParameter == 0 && _FilterParameter == 0)
                AgentListView.ItemsSource = _ListAgent
                    .Where(agent => $"{agent.Title} {agent.Email} {agent.Phone}".Contains(_SearchParameter))
                    .OrderBy(agent => agent.Title);

            if (_SortParameter == 1 && _FilterParameter == 0)
                AgentListView.ItemsSource = _ListAgent
                    .Where(agent => $"{agent.Title} {agent.Email} {agent.Phone}".Contains(_SearchParameter))
                    .OrderByDescending(agent => agent.Title);

            if (_SortParameter == 0 && _FilterParameter != 0)
                AgentListView.ItemsSource = _ListAgent
                    .Where(agent => $"{agent.Title} {agent.Email} {agent.Phone}".Contains(_SearchParameter) && agent.AgentType.Title == ComboBoxFilters.Text)
                    .OrderBy(agent => agent.Title);

            if (_SortParameter == 1 && _FilterParameter != 0)
                AgentListView.ItemsSource = _ListAgent
                    .Where(agent => $"{agent.Title} {agent.Email} {agent.Phone}".Contains(_SearchParameter) && agent.AgentType.Title == ComboBoxFilters.Text)
                    .OrderByDescending(agent => agent.Title);
        }
    }
}

