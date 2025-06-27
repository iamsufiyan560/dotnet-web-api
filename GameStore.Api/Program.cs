using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


const string GetGameEndpointName = "GetGame";

List<GameDto> games = [

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




app.MapGet("/", () => "Hello World!");


app.MapGet("/games", () => games);


app.MapGet("/games/{id}", (int id) =>
{
  var game = games.Find(game => game.Id == id);
  return game is not null
    ? Results.Ok(game)
    : Results.NotFound($"Game with id {id} is not present");
}).WithName(GetGameEndpointName);

app.MapPost("/games", (CreateGameDto newGame) =>
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


app.MapPut("/game/{id}", (int id, UpdateGameDto updatedGame) =>
{


  var index = games.FindIndex(game => game.Id == id);



  games[index] = new GameDto(

    id,
    updatedGame.Name,
    updatedGame.Genre,
    updatedGame.Price,
    updatedGame.ReleaseDate

  );


  return Results.NoContent();

});


app.Run();
