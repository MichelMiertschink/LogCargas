using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBase.Models
{
    public class ModelBase
    {
        [Key]
        public int Id { get; set; }

        // user ID form AspNetUser table.
        public string? OwnerID { get; set; }

        [DisplayName("Criação")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Alteração")]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; }

        public ModelBase()
        {
            if (CreatedAt == new DateTime())
            {
                CreatedAt = DateTime.Now;
            }
                
        }

        [NotMapped]
        public object Key
        {
            get { return Id; }
            set { }
        }

        public bool Equals(object obj)
        {
            if (obj is null || GetType() != obj.GetType())
            {
                return false;
            }

            return this.GetHashCode() == obj.GetHashCode();
        }

        public ModelBase(int id,string ownerId , DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            OwnerID = ownerId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

    }
}