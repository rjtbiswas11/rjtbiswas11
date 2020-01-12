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
    /// Interaction logic for QuizHistoryDetailWindow.xaml
    /// </summary>
    public partial class QuizHistoryDetailWindow : Window
    {
        const int no_of_question_per_page = 8;

        int QuizHistoryID = 0;

        int total_no_of_correct_page = 0;
        int total_no_of_wrong_page = 0;

        int current_correct_page = 0;
        int current_wrong_page = 0;

        QuizEntities qe = new QuizEntities();


        public QuizHistoryDetailWindow(Window owner, int QuizHistoryID)
        {
            InitializeComponent();

            this.Owner = owner;

            var q = qe.QuizHistory.Find(QuizHistoryID);

            this.Title = q.Quiz.Title;
            this.QuizHistoryID = QuizHistoryID;

            init();
            
        }

        private void init()
        {
            int total_no_of_correct_question = 0;
            int total_no_of_wrong_question = 0;

            current_correct_page = 1;
            current_wrong_page = 1;

            total_no_of_correct_question = qe.QuizHistory.Find(QuizHistoryID).QuizHistoryDetail
                                            .Count(x => x.Correct == true);

            total_no_of_wrong_question = qe.QuizHistory.Find(QuizHistoryID).QuizHistoryDetail
                                            .Count(x => x.Correct == false);

            double m = 0;

            if (total_no_of_correct_question == 0)
                total_no_of_correct_page = 0;
            else
            {
                m = (double)total_no_of_correct_question / (double)no_of_question_per_page;
                m = Math.Ceiling(m);

                total_no_of_correct_page = (int)m;
            }

            if (total_no_of_wrong_question == 0)
                total_no_of_wrong_page = 0;
            else
            {
                m = (double)total_no_of_wrong_question / (double)no_of_question_per_page;
                m = Math.Ceiling(m);

                total_no_of_wrong_page = (int)m;
            }

            var correct_questions = (from n in qe.QuizHistory.Find(QuizHistoryID).QuizHistoryDetail
                                    where n.Correct == true
                                    orderby n.QuestionID
                                    select n.Question).Take(no_of_question_per_page);

            SetupQuestion(correct_questions.ToList(), true);

            if (total_no_of_correct_question > no_of_question_per_page)
            {
                btnCorrect_Next.Visibility = System.Windows.Visibility.Visible;
                btnCorrect_Prev.Visibility = System.Windows.Visibility.Visible;

                btnCorrect_Next.IsEnabled = true;
                btnCorrect_Prev.IsEnabled = false;
            }

            var wrong_questions = (from n in qe.QuizHistory.Find(QuizHistoryID).QuizHistoryDetail
                                   where n.Correct == false
                                   orderby n.QuestionID
                                   select n.Question).Take(no_of_question_per_page);

            if (total_no_of_wrong_question > no_of_question_per_page)
            {
                btnWrong_Next.Visibility = System.Windows.Visibility.Visible;
                btnWrong_Prev.Visibility = System.Windows.Visibility.Visible;

                btnWrong_Next.IsEnabled = true;
                btnWrong_Prev.IsEnabled = false;
            }

            SetupQuestion(wrong_questions.ToList(), false);

        }

        private void SetupQuestion(List<Question> lstquestion, bool isCorrect)
        {
            int i = 0;

            foreach (Question q in lstquestion)
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

                answers = q.Answer.ToList();

                foreach (var a in q.Answer)
                {
                    RadioButton rbn = new RadioButton();
                    rbn.Margin = new Thickness(0, 0, 0, 12);
                    rbn.Content = a.Text;

                    if (a.IsCorrect == true)
                    {
                        rbn.IsChecked = true;
                    }

                    spanswer.Children.Add(rbn);
                }

                spanswer.IsEnabled = false;

                spmain.Children.Add(sp);
                spmain.Children.Add(tbQuestion);
                spmain.Children.Add(spanswer);

                if (isCorrect)
                {
                    spl_Correct.Children.Add(spmain);
                }
                else
                {
                    spl_Wrong.Children.Add(spmain);
                }
            }
        }

        private void btnCorrect_Prev_Click(object sender, RoutedEventArgs e)
        {
            current_correct_page -= 1;

            int skiprecord = (current_correct_page -1) * no_of_question_per_page;

            if (current_correct_page == 1)
            {
                btnCorrect_Prev.IsEnabled = false;
                btnCorrect_Next.IsEnabled = true;
            }
            else
            {
                btnCorrect_Prev.IsEnabled = true;
                btnCorrect_Next.IsEnabled = true;
            }

            spl_Correct.Children.Clear();

            var correct_questions = (from n in qe.QuizHistory.Find(QuizHistoryID).QuizHistoryDetail
                                     where n.Correct == true
                                     orderby n.QuestionID
                                     select n.Question).Skip(skiprecord).Take(no_of_question_per_page);

            SetupQuestion(correct_questions.ToList(), true);

            sc_Correct.ScrollToTop();
        }

        private void btnCorrect_Next_Click(object sender, RoutedEventArgs e)
        {
            int skiprecord = current_correct_page * no_of_question_per_page;

            current_correct_page += 1;

            if (current_correct_page == total_no_of_correct_page)
            {
                btnCorrect_Next.IsEnabled = false;
                btnCorrect_Prev.IsEnabled = true;
            }
            else
            {
                btnCorrect_Next.IsEnabled = true;
                btnCorrect_Prev.IsEnabled = true;
            }

            spl_Correct.Children.Clear();

            var correct_questions = (from n in qe.QuizHistory.Find(QuizHistoryID).QuizHistoryDetail
                                     where n.Correct == true
                                     orderby n.QuestionID
                                     select n.Question).Skip(skiprecord).Take(no_of_question_per_page);

            SetupQuestion(correct_questions.ToList(), true);

            sc_Correct.ScrollToTop();
        }

        private void btnWrong_Prev_Click(object sender, RoutedEventArgs e)
        {
            current_wrong_page -= 1;

            int skiprecord = (current_wrong_page -1) * no_of_question_per_page;

            if (current_wrong_page == 1)
            {
                btnWrong_Prev.IsEnabled = false;
                btnWrong_Next.IsEnabled = true;
            }
            else
            {
                btnWrong_Prev.IsEnabled = true;
                btnWrong_Next.IsEnabled = true;
            }

            spl_Wrong.Children.Clear();

            var wrong_questions = (from n in qe.QuizHistory.Find(QuizHistoryID).QuizHistoryDetail
                                   where n.Correct == false
                                   orderby n.QuestionID
                                   select n.Question).Skip(skiprecord).Take(no_of_question_per_page);

            SetupQuestion(wrong_questions.ToList(), false);

            sc_Wrong.ScrollToTop();
        }

        private void btnWrong_Next_Click(object sender, RoutedEventArgs e)
        {
            int skiprecord = current_wrong_page * no_of_question_per_page;

            current_wrong_page += 1;

            if (current_wrong_page == total_no_of_wrong_page)
            {
                btnWrong_Next.IsEnabled = false;
                btnWrong_Prev.IsEnabled = true;
            }
            else
            {
                btnWrong_Next.IsEnabled = true;
                btnWrong_Prev.IsEnabled = true;
            }

            spl_Wrong.Children.Clear();

            var wrong_questions = (from n in qe.QuizHistory.Find(QuizHistoryID).QuizHistoryDetail
                                   where n.Correct == false
                                   orderby n.QuestionID
                                   select n.Question).Skip(skiprecord).Take(no_of_question_per_page);

            SetupQuestion(wrong_questions.ToList(), false);

            sc_Wrong.ScrollToTop();
        }
    }
}
