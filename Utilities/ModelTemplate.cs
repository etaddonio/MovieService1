using System.ComponentModel.DataAnnotations;

namespace Utilities
{
    public abstract class ModelTemplate
    {
        [Key]
        public int Key { get; set; }
    }
}