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
    
    public partial class QuizHistoryDetail
    {
        public int ID { get; set; }
        public Nullable<int> QuizHistoryID { get; set; }
        public Nullable<int> QuestionID { get; set; }
        public Nullable<bool> Correct { get; set; }
    
        public virtual Question Question { get; set; }
        public virtual QuizHistory QuizHistory { get; set; }
    }
}
