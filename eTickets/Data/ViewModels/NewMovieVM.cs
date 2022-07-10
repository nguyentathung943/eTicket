using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eTickets.Data.Base;
using eTickets.Data.Enums;

namespace eTickets.Data.ViewModels
{
    public class NewMovieVM
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [Display(Description = "Movie name")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Price is required")]
        [Display(Description = "Movie Price")]
        public double Price { get; set; }
        
        [Required(ErrorMessage = "Movie Poster Url is required")]
        [Display(Description = "Movie Poster Url")]
        public string ImageURL { get; set; }
        
        [Required(ErrorMessage = "Description is required")]
        [Display(Description = "Movie Description")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Start Date is required")]
        [Display(Description = "Movie Start Date")]
        public DateTime StartDate { get; set; }
        
        [Required(ErrorMessage = "End Date is required")]
        [Display(Description = "Movie End Date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Movie Category is required")]
        [Display(Description = "Select a category")]
        public MovieCategory MovieCategory { get; set; }
        
        // Relationships
        [Required(ErrorMessage = "Select actor(s) is required")]
        [Display(Description = "Select actor(s)")]
        public List<int> ActorIds { get; set; }
        
        // Cinema
        [Required(ErrorMessage = "Select a cinema is required")]
        [Display(Description = "Select a cinema")]
        public int CinemaId { get; set; }

        // Producer
        [Required(ErrorMessage = "Select a producer is required")]
        [Display(Description = "Select a producer")]
        public int ProducerId { get; set; }
    }
}

