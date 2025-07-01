using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{


  const string GetGameEndpointName = "GetGame";

  private static readonly List<GameDto> games = [

  new(
      1,
      "God of War",
      "Action",
      49.99M,
      new DateOnly(2018, 4, 20)
  ),
  new(
      2,
      "Spider-Man: Miles Morales",
      "Adventure",
      39.99M,
      new DateOnly(2020, 11, 12)
  ),
  new(
      3,
      "Horizon Zero Dawn",
      "RPG",
      29.99M,
      new DateOnly(2017, 2, 28)
  )

];



  public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
  {



    var group = app.MapGroup("games").WithParameterValidation();





    group.MapGet("/", () => games);


    group.MapGet("/{id}", (int id) =>
    {
      var game = games.Find(game => game.Id == id);
      return game is not null
        ? Results.Ok(game)
        : Results.NotFound($"Game with id {id} is not present");
    }).WithName(GetGameEndpointName);

    group.MapPost("/", (CreateGameDto newGame) =>
    {
      GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
      );
      games.Add(game);

      return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
    });


    group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
    {


      var index = games.FindIndex(game => game.Id == id);


      if (index == -1)
      {
        return Results.NotFound();
      }


      games[index] = new GameDto(

        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate

      );


      return Results.NoContent();

    });





    group.MapDelete("/{id}", (int id) =>
    {





      games.RemoveAll(game => game.Id == id);

      return Results.NoContent();

    });

    return group;

  }




}
