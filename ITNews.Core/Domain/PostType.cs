using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ITNews.Core.Domain
{
    public enum PostType 
    {
        [Display(Name = "Computer Arithmetic")]
        ComputerArithmetic,
        [Display(Name = "Theory of Algorithms")]
        TheoryOfAlgorithms,
        [Display(Name = "Computer Architecture")]
        ComputerArchitecture,
        [Display(Name = "Data Processing")]
        DataProcessing,
        [Display(Name = "Data Analysis")]
        DataAnalysis
    }
}
