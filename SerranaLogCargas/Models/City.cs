using System.ComponentModel.DataAnnotations;

namespace LogCargas.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Cidade")]
        [Required (ErrorMessage = "Informe o nome da cidade")]
        public string Name { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Informe o estado")]
        public State State { get; set; }
        
        [Display(Name = "Estado")]
        public int StateId { get; set; }
        [Display(Name = "Código IBGE")]
        public string? CodIbge { get; set; }
        
        public City()
        {
        }

        public City(int id, string name, State state, string? codIbge)
        {
            Id = id;
            Name = name;
            State = state;
            CodIbge = codIbge;
        }
    }
}
