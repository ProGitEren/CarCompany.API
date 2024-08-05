using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Params
{
    public class VehicleParams
    {
        //Page size
        public int maxpagesize { get; set; } = 50;
        
        private int pagesize = 13;
        public int Pagesize
        {
            get => pagesize;
            set => pagesize = value > maxpagesize ? maxpagesize : value;
        }
        public int PageNumber { get; set; } = 1;

        //Filter By Model
        public int? ModelId { get; set; }
       
        //Sorting
        public string Sorting { get; set; }
        //search 

        private string _search;

        public string Search
        {
            get { return _search; }
            set { _search = value.ToLower(); }
        }
        //Asp.Net Core 8 Web API :https://www.youtube.com/watch?v=UqegTYn2aKE&list=PLazvcyckcBwitbcbYveMdXlw8mqoBDbTT&index=1



    }
}
