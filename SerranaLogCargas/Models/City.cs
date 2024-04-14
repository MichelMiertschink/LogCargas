﻿using System.ComponentModel.DataAnnotations;

namespace SerranaLogCargas.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Cidade")]
        [Required (ErrorMessage = "Informe o nome da cidade")]
        public string Name { get; set; }
        [Required (ErrorMessage = "Informe a unidade federativa")]
        public State State { get; set; }
        public int StateId { get; set; }

        // Colocar campo para o código IBGE
        
        public City()
        {
        }

        public City(int id, string name, State state)
        {
            Id = id;
            Name = name;
            State = state;
        }
    }
}
