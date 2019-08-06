using System;
using System.Collections.Generic;
using System.Text;
using Cyriller.Model;

namespace Cyriller.Desktop.Models
{
    public class AdjectiveDeclineResultRowModel : DeclineResultRowModel
    {
        public string SingularMasculineAnimate { get; set; }
        public string SingularFeminineAnimate { get; set; }
        public string SingularNeuterAnimate { get; set; }

        public string SingularMasculineInanimate { get; set; }
        public string SingularFeminineInanimate { get; set; }
        public string SingularNeuterInanimate { get; set; }

        public string PluralAnimate { get; set; }
        public string PluralInanimate { get; set; }
    }
}
