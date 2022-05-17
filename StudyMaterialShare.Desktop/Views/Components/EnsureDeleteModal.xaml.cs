using System;
using System.Collections.Generic;
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

namespace StudyMaterialShare.Desktop.Views.Components
{
    /// <summary>
    /// Interaction logic for EnsureDeleteModal.xaml
    /// </summary>
    public partial class EnsureDeleteModal : UserControl
    {
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(EnsureDeleteModal), new PropertyMetadata(false));


        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CloseCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(EnsureDeleteModal), new PropertyMetadata(null));


        public ICommand EnsureCommand
        {
            get { return (ICommand)GetValue(EnsureCommandProperty); }
            set { SetValue(EnsureCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnsureCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnsureCommandProperty =
            DependencyProperty.Register("EnsureCommand", typeof(ICommand), typeof(EnsureDeleteModal), new PropertyMetadata(null));


        public EnsureDeleteModal()
        {
            InitializeComponent();
        }
    }
}
