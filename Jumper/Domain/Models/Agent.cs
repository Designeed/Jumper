using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Jumper
{
    public partial class Agent
    {
        private const string _PATH = @"D:\Пользователи\Рабочий стол\Попрыженок - агенты";

        public Agent()
        {
            AgentPriorityHistories = new HashSet<AgentPriorityHistory>();
            ProductSales = new HashSet<ProductSale>();
            Shops = new HashSet<Shop>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int AgentTypeId { get; set; }
        public string? Address { get; set; }
        public string Inn { get; set; } = null!;
        public string? Kpp { get; set; }
        public string? DirectorName { get; set; }
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }

        private string? _Logo = null!;
        public string? Logo 
        { 
            get => String.Concat(_PATH, _Logo); 
            set { _Logo = value; } 
        }
        public int Priority { get; set; }
        public string Discount => GetDiscount();
        public Color BackgroundColor => Discount.Contains("20") ? Colors.LightGreen : Colors.Transparent;
        public decimal SumSale => ProductSales.Sum(sum => sum.ProductCount * sum.Product.MinCostForAgent);
        public int ProductCount => ProductSales.Select(sale => sale.ProductCount).Sum();

        public virtual AgentType AgentType { get; set; } = null!;
        public virtual ICollection<AgentPriorityHistory> AgentPriorityHistories { get; set; }
        public virtual ICollection<ProductSale> ProductSales { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }

        private string GetDiscount()
        {
            if (SumSale >= 0 && SumSale <= 10000)
                return "0%";

            if (SumSale >= 10000 && SumSale <= 50000)
                return "5%";

            if (SumSale >= 50000 && SumSale <= 150000)
                return "10%";

            if (SumSale >= 150000 && SumSale <= 500000)
                return "20%";

            if (SumSale >= 500000)
                return "25%";

            else
                return "0%";
        }
    }
}
