using System;

namespace filmstudion.api.Models
{
  public interface IFilmCopy
  {
    public string FilmCopyId { get; set; }
    public string FilmId { get; set; }
    public bool RentedOut { get; set; }
    public string FilmStudioId { get; set; }
  }
}