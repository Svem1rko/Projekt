using System.Collections.Generic;

namespace Projekt2.Models.ProjectModels
{
    public class IndexViewModel
    {
        public List<Holder> Holders { get; set; }
        public List<Link> Popular { get; set; }

        public IndexViewModel(List<Holder> holders, List<Link> popular)
        {
            Holders = holders;
            Popular = popular;
        }
    }
}
