namespace GameStore.Api.Dtos;

public record class GameSummrayDto
(
      int Id,
      string Name,
      string Genre,
      decimal Price,
      DateOnly ReleaseDate


);

