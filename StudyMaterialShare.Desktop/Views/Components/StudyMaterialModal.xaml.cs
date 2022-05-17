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
    /// Interaction logic for StudyMaterialModal.xaml
    /// </summary>
    public partial class StudyMaterialModal : UserControl
    {


        public bool RemoveRatings
        {
            get { return (bool)GetValue(RemoveRatingsProperty); }
            set { SetValue(RemoveRatingsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RemoveRatings.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemoveRatingsProperty =
            DependencyProperty.Register("RemoveRatings", typeof(bool), typeof(StudyMaterialModal), new PropertyMetadata(false));



        public bool IsResetDownloads
        {
            get { return (bool)GetValue(IsResetDownloadsProperty); }
            set { SetValue(IsResetDownloadsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsResetDownload.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsResetDownloadsProperty =
            DependencyProperty.Register("IsResetDownloads", typeof(bool), typeof(StudyMaterialModal), new PropertyMetadata(false));



        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SaveCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SaveCommandProperty =
            DependencyProperty.Register("SaveCommand", typeof(ICommand), typeof(StudyMaterialModal), new PropertyMetadata(null));

        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CloseCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(StudyMaterialModal), new PropertyMetadata(null));

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(StudyMaterialModal), new PropertyMetadata(false));

        public StudyMaterialModal()
        {
            InitializeComponent();
        }
    }
}
