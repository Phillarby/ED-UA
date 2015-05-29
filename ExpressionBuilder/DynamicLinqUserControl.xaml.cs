using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChabadOnCampus.DynamicLinq
{
    /// <summary>
    /// Interaction logic for DynamicLinqUserControl.xaml
    /// </summary>
    public partial class DynamicLinqUserControl : UserControl
    {
        public DynamicLinqUserControl()
        {
            InitializeComponent();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var vm = DataContext as ExpressionList;
            var param = e.Parameter as ExpressionVM;
            if (vm != null && param != null) vm.Remove(param);
        }
    }
}
