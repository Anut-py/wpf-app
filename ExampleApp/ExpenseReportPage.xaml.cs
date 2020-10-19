using System.Windows.Controls;

namespace ExampleApp
{
    public partial class ExpenseReportPage : Page
    {
        public ExpenseReportPage()
        {
            InitializeComponent();
        }

        public ExpenseReportPage(object data) : this()
        {
            // Bind to expense report data.
            DataContext = data;
        }
    }
}