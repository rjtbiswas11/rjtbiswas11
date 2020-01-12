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
using System.Windows.Shapes;
using System.Data.Entity;

namespace QuizApplication
{
    /// <summary>
    /// Interaction logic for QuizHistoryWindow.xaml
    /// </summary>
    public partial class QuizHistoryWindow : Window
    {
        int QuizID = 0;
        QuizEntities qe = new QuizEntities();

        public QuizHistoryWindow(Window owner, int QuizID)
        {
            InitializeComponent();

            this.Owner = owner;
            this.QuizID = QuizID;

            var q = qe.Quiz.Find(QuizID);

            this.Title = q.Title;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource quizHistoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("quizHistoryViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            qe.QuizHistory.Load();

            var qh = qe.QuizHistory.Local.Where(x => x.QuizID == QuizID);
            
            quizHistoryViewSource.Source = qh;
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            QuizHistory qh = (sender as Button).DataContext as QuizHistory;

            QuizHistoryDetailWindow qhdw = new QuizHistoryDetailWindow(this, qh.ID);

            qhdw.ShowDialog();
        }
    }
}
