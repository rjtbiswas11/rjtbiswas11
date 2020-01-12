//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuizApplication
{
    using System;
    using System.Collections.Generic;
    
    public partial class Question
    {
        public Question()
        {
            this.Answer = new HashSet<Answer>();
            this.QuizHistoryDetail = new HashSet<QuizHistoryDetail>();
        }
    
        public int ID { get; set; }
        public Nullable<int> QuizID { get; set; }
        public string Text { get; set; }
        public Nullable<bool> Answered { get; set; }
        public Nullable<bool> Correct { get; set; }
    
        public virtual ICollection<Answer> Answer { get; set; }
        public virtual Quiz Quiz { get; set; }
        public virtual ICollection<QuizHistoryDetail> QuizHistoryDetail { get; set; }
    }
}