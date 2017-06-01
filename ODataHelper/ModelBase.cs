using System.ComponentModel.DataAnnotations;

namespace ODataHelper
{
    public abstract class ModelBase
    {
        [Key]
        public int Key { get; set; }
    }
}