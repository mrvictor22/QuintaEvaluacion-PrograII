using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuintaEvaluacion.Data
{
    public class PhotoModel {

        [DisplayName("Select a Photo")]
        [Required(ErrorMessage = "Please select file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Only Image files allowed.")]
        public HttpPostedFileBase Image { get; set; }

        [Required(ErrorMessage = "Please add a title.")]
        public string Title { get; set; }

        public string Description { get; set; }
    }
}