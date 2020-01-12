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

namespace QuizApplication
{
    /// <summary>
    /// Interaction logic for QuizWindow.xaml
    /// </summary>
    public partial class QuizWindow : Window
    {
        QuizEntities qe = new QuizEntities();
        int no_of_available_question = 0;
        int answered_question = 0;
        int quizid = 0;
        const int default_no_of_question = 8;
        bool isAnswerRandomOrder = false;

        public QuizWindow(Window mainwindow, int QuizID, string title)
        {
            InitializeComponent();

            this.Owner = mainwindow;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.Title = title;
            this.quizid = QuizID;

            Init(QuizID);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (this.Owner as MainWindow).Init();
            this.Owner.Show();
        }

        private void Init(int QuizID)
        {
            string incorrectmessage = "";

            answered_question = 0;

            spl_question.Children.Clear();

            var question = (from n in qe.Question
                            where n.QuizID == QuizID
                            && n.Answered == false
                            select n).Take(default_no_of_question).OrderBy(x => Guid.NewGuid());

            no_of_available_question = question.ToList().Count;

            int i = 0;

            foreach (var q in question)
            {
                i++;
                StackPanel spmain = new StackPanel();
                spmain.Orientation = Orientation.Vertical;
                spmain.Margin = new Thickness(20, 0, 0, 20);

                StackPanel sp = new StackPanel();
                TextBlock tb = new TextBlock();

                TranslateTransform tt = new TranslateTransform();
                tt.X = 5;
                tt.Y = 15;

                tb.Text = i.ToString() + ".";
                tb.RenderTransform = tt;

                sp.Children.Add(tb);

                TextBlock tbQuestion = new TextBlock();
                tbQuestion.TextWrapping = TextWrapping.Wrap;
                tbQuestion.Margin = new Thickness(30, 0, 0, 10);
                tbQuestion.Text = q.Text;

                StackPanel spanswer = new StackPanel();
                spanswer.Margin = new Thickness(30, 10, 30, 10);

                List<Answer> answers;

                if (isAnswerRandomOrder)
                {
                    answers = q.Answer.OrderBy(x => Guid.NewGuid()).ToList();
                }
                else
                {
                    answers = q.Answer.ToList();
                }

                foreach (var a in q.Answer)
                {
                    RadioButton rbn = new RadioButton();
                    rbn.Margin = new Thickness(0, 0, 0, 12);
                    rbn.Content = a.Text;
                    rbn.DataContext = q;

                    if (a.IsCorrect == true)
                    {
                        rbn.Checked += rbn_Correct_Checked;

                        incorrectmessage = "Incorrect , the correct answer is " + a.Text;
                    }
                    else
                    {
                        rbn.Checked += rbn_Wrong_Checked;
                    }

                    spanswer.Children.Add(rbn);
                }

                StackPanel spMessage = new StackPanel();
                spMessage.Margin = new Thickness(30, 0, 0, 0);
                TextBlock tbCorrectMessage = new TextBlock();
                tbCorrectMessage.Text = "Correct";
                tbCorrectMessage.FontWeight = FontWeights.Bold;
                tbCorrectMessage.Foreground = new SolidColorBrush(Colors.Green);
                tbCorrectMessage.Visibility = System.Windows.Visibility.Collapsed;

                TextBlock tbIncorrectMessage = new TextBlock();
                tbIncorrectMessage.Text = incorrectmessage;
                tbIncorrectMessage.Margin = new Thickness(0, 0, 10, 0);
                tbIncorrectMessage.TextWrapping = TextWrapping.Wrap;
                tbIncorrectMessage.FontWeight = FontWeights.Bold;
                tbIncorrectMessage.Foreground = new SolidColorBrush(Colors.Red);
                tbIncorrectMessage.Visibility = System.Windows.Visibility.Collapsed;

                spMessage.Children.Add(tbCorrectMessage);
                spMessage.Children.Add(tbIncorrectMessage);
                spMessage.Visibility = System.Windows.Visibility.Hidden;


                spmain.Children.Add(sp);
                spmain.Children.Add(tbQuestion);
                spmain.Children.Add(spanswer);
                spmain.Children.Add(spMessage);

                spl_question.Children.Add(spmain);
            }

        }

        private void rbn_Correct_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rbn = sender as RadioButton;

            var q = qe.Question.Find((rbn.DataContext as Question).ID);

            q.Answered = true;
            q.Correct = true;

            qe.SaveChanges();

            StackPanel sp = rbn.Parent as StackPanel;

            StackPanel spmain = sp.Parent as StackPanel;

            (spmain.Children[3] as StackPanel).Visibility = System.Windows.Visibility.Visible;

            ((spmain.Children[3] as StackPanel).Children[0] as TextBlock).Visibility = System.Windows.Visibility.Visible;

            sp.IsEnabled = false;

            answered_question += 1;

            if (answered_question == no_of_available_question)
            {
                int not_answered_count = qe.Quiz.Find(quizid).Question.Count(x => x.Answered == false);

                if (not_answered_count != 0)
                {
                    btnNext.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    var quiz = qe.Quiz.Find(quizid);

                    quiz.Completed += 1;

                    qe.SaveChanges();

                    CreateHistoryRecord(quizid, quiz.Completed);

                    MessageBox.Show("Finished");
                }
            }
        }

        private void rbn_Wrong_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rbn = sender as RadioButton;

            var q = qe.Question.Find((rbn.DataContext as Question).ID);

            q.Answered = true;
            q.Correct = false;

            qe.SaveChanges();

            StackPanel sp = rbn.Parent as StackPanel;

            StackPanel spmain = sp.Parent as StackPanel;

            (spmain.Children[3] as StackPanel).Visibility = System.Windows.Visibility.Visible;

            ((spmain.Children[3] as StackPanel).Children[1] as TextBlock).Visibility = System.Windows.Visibility.Visible;

            sp.IsEnabled = false;

            answered_question += 1;

            if (answered_question == no_of_available_question)
            {
                int not_answered_count = qe.Quiz.Find(quizid).Question.Count(x => x.Answered == false);

                if (not_answered_count != 0)
                {

                    btnNext.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    var quiz = qe.Quiz.Find(quizid);

                    quiz.Completed += 1;

                    qe.SaveChanges();

                    CreateHistoryRecord(quizid, quiz.Completed);

                    MessageBox.Show("Finished");
                }
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Init(quizid);
            btnNext.Visibility = System.Windows.Visibility.Collapsed;

            sc_Question.ScrollToTop();
        }

        private void CreateHistoryRecord(int quizid, int completed)
        {
            var questions = qe.Quiz.Find(quizid).Question;

            int total = questions.Count;

            int correctcount = qe.Quiz.Find(quizid).Question.Count(x => x.Correct == true);

            double correctpercentage = (double)correctcount / (double)total * 100;

            correctpercentage = Math.Round(correctpercentage, 1);

            QuizHistory qh = new QuizHistory();

            qh.QuizID = quizid;
            qh.Completed = completed;
            qh.CorrectPercentage = correctpercentage;

            qe.QuizHistory.Add(qh);

            qe.SaveChanges();

            foreach (var q in questions)
            {
                QuizHistoryDetail qhd = new QuizHistoryDetail();
                qhd.QuizHistoryID = qh.ID;
                qhd.QuestionID = q.ID;
                qhd.Correct = q.Correct;

                qe.QuizHistoryDetail.Add(qhd);
            }

            qe.SaveChanges();
                           
        }
    }
}
