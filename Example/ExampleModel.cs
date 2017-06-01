using System.Collections.Generic;
using ODataHelper;

namespace Example
{
    public class ExampleModel : ModelBase
    {
        public int Count { get; set;  }
                
        public ICollection<ExampleModel> Collection { get; set; }
    }
}