using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using WTMFirst.Model;


namespace WTMFirst.ViewModel.MyUserVMs
{
    public partial class MyUserSearcher : BaseSearcher
    {
        [Display(Name = "Account")]
        public String ITCode { get; set; }
        [Display(Name = "Name")]
        public String Name { get; set; }

        protected override void InitVM()
        {
        }

    }
}
