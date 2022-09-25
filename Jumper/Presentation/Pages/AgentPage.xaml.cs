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
using System.Windows.Controls.Primitives;
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
        private List<Agent> _ListAgent = new JumperContext().Agents.OrderBy(agent => agent.Title).ToList();
        private int _TotalPages = (int)Math.Ceiling(((double)new JumperContext().Agents.ToList().Count / 10d));
        private int _ItemsPerPage = 10;
        private int _PageNumber = 1;
        public int PageNumber
        {
            get => _PageNumber;
            set
            {
                if (value >= 1 && value <= _TotalPages)
                {
                    _PageNumber = value;
                    TextBoxtPageNumber.Text = PageNumber.ToString();
                    AgentListView.ItemsSource = GetListAgentPerPage(value);

                    Decorator border = VisualTreeHelper.GetChild(AgentListView, 0) as Decorator;
                    ScrollViewer scroll = border.Child as ScrollViewer;
                    scroll.ScrollToTop();
                }
            }
        }

        public AgentPage()
        {
            InitializeComponent();
            AgentListView.ItemsSource = GetListAgentPerPage(_PageNumber);
        }

        private List<Agent> GetListAgentPerPage(int PageNumber)
            => _ListAgent.Skip((PageNumber - 1) * _ItemsPerPage).Take(_ItemsPerPage).ToList();

        private void UpdateList()
        {
            _ListAgent = new JumperContext().Agents.OrderBy(agent => agent.Title).ToList();
            AgentListView.ItemsSource = _ListAgent;
        }
 
        private void ButtonAddClick(object sender, RoutedEventArgs e)
        {
            new AgentAddWindow().ShowDialog();
            UpdateList();
        }

        private void ButtonEditClick(object sender, RoutedEventArgs e)
        {
            if (AgentListView.SelectedIndex != -1)
            {
                new AgentEditWindow(((Agent)AgentListView.SelectedItem).Id).ShowDialog();
                UpdateList();
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

                UpdateList();
                MessageBox.Show("Агент удален");
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetFilterAndSortType();
        }

        private void ComboBoxFilters_DropDownClosed(object sender, EventArgs e)
        {
            SetFilterAndSortType();
        }

        private void ComboBoxSortType_DropDownClosed(object sender, EventArgs e)
        {
            SetFilterAndSortType();
        }

        private void SetFilterAndSortType()
        {
            PageNumber = 1;
            TextBoxtPageNumber.Text = "1";

            var filteredList = GetListAgentPerPage(PageNumber)
                    .Where(agent => $"{agent.Title} {agent.Email} {agent.Phone}".Contains(SearchTxtBox.Text))
                    .Where(agent => ComboBoxFilters.SelectedIndex == 0 || agent.AgentType.Title == ComboBoxFilters.Text);

            AgentListView.ItemsSource = ComboBoxSortType.SelectedIndex == 0 ? filteredList.OrderBy(agent => agent.Title) : filteredList.OrderByDescending(agent => agent.Title);
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            PageNumber--;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            PageNumber++;
        }
    }
}

