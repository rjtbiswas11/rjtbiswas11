using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace QuizApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (lstview_Quiz.SelectedItem != null)
            {
                int quizid = (lstview_Quiz.SelectedItem as QuizInfo).QuizID;

                string title = (lstview_Quiz.SelectedItem as QuizInfo).QuizTitle;

                if ((lstview_Quiz.SelectedItem as QuizInfo).FinishedPercentageText == "Completed")
                {
                    QuizEntities qe = new QuizEntities();

                    var quiz = qe.Quiz.Find(quizid);

                    foreach (var q in quiz.Question)
                    {
                        q.Answered = false;
                        q.Correct = false;
                    }

                    qe.SaveChanges();
                }

                QuizWindow qw = new QuizWindow(this, quizid, title);

                qw.Show();

                this.Hide();
            }
        }

        public void Init()
        {
            QuizEntities qe = new QuizEntities();

            List<QuizInfo> qi = new List<QuizInfo>();

            var quiz = from q in qe.Quiz
                       select q;

            foreach (var q in quiz)
            {
                int answeredquestion = q.Question.Count(n => n.Answered == true);

                int totalquestion = q.Question.Count;

                double finishedpercentage = (double)answeredquestion / (double)totalquestion * 100;

                int answered_and_correct_question = q.Question.Count(n => n.Answered == true && n.Correct == true);

                double correctpercentage = 0;

                if (answeredquestion == 0 && answered_and_correct_question == 0)
                {
                    correctpercentage = 100;
                }
                else
                {
                    correctpercentage = (double)answered_and_correct_question / (double)answeredquestion * 100;
                }

                qi.Add(new QuizInfo(q.ID, q.Title, q.Question.Count, finishedpercentage, correctpercentage, q.Completed));
            }

            lstview_Quiz.ItemsSource = qi;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void MenuItemHistoryView_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)e.Source;
            ContextMenu menu = (ContextMenu)menuItem.Parent;
            ListViewItem item = (ListViewItem)menu.PlacementTarget;
            QuizInfo qi = (QuizInfo)item.DataContext;

            QuizHistoryWindow qhw = new QuizHistoryWindow(this, qi.QuizID);
            qhw.ShowDialog();
        }
    }

    class QuizInfo
    {
        public int QuizID { get; set; }
        public string QuizTitle { get; set; }
        public int NoOfQuestion { get; set; }
        public double FinishedPercentage { get; set; }
        public string CorrectPercentageText { get; set; }
        public int NoOfCompleted { get; set; }
        public string FinishedPercentageText { get; set; }

        public QuizInfo(int quizid, string quiztitle, int noofquestion, double finishedpercentage, double correctpercentage, int noofcompleted)
        {
            this.QuizID = quizid;
            this.QuizTitle = quiztitle;
            this.NoOfQuestion = noofquestion;
            this.FinishedPercentage = finishedpercentage;
            this.NoOfCompleted = noofcompleted;

            double n = Math.Round(finishedpercentage, 1);

            if (finishedpercentage == 100)
            {
                this.FinishedPercentageText = "Completed";
            }
            else
            {
                this.FinishedPercentageText = n.ToString() + " %";
            }

            n = Math.Round(correctpercentage, 1);


            this.CorrectPercentageText = n.ToString() + " %";
        }
    }

}
