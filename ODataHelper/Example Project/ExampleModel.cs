using System.Collections.Generic;

namespace ODataHelper.Example_Project
{
    public class ExampleModel : ModelBase
    {
        public int Count { get; set;  }
                
        public ICollection<ExampleModel> Collection { get; set; }
    }
}