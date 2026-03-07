using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

public class HomeController : Controller
{
    private readonly NbaDataService _data;

    public HomeController(NbaDataService data) => _data = data;

    public async Task<IActionResult> Index(string name, string team, int page = 1)
    {
        int pageSize = 20;
        var filter = Builders<BsonDocument>.Filter.Empty;

        if (!string.IsNullOrEmpty(name))
        {
            var nameFilter = Builders<BsonDocument>.Filter.Or(
                Builders<BsonDocument>.Filter.Regex("player_name", new BsonRegularExpression(name, "i")),
                Builders<BsonDocument>.Filter.Regex("name", new BsonRegularExpression(name, "i")),
                Builders<BsonDocument>.Filter.Regex("full_name", new BsonRegularExpression(name, "i")),
                Builders<BsonDocument>.Filter.Regex("first_name", new BsonRegularExpression(name, "i")),
                Builders<BsonDocument>.Filter.Regex("last_name", new BsonRegularExpression(name, "i")),
                Builders<BsonDocument>.Filter.Regex("display_first_last", new BsonRegularExpression(name, "i"))
            );
            filter &= nameFilter;
        }

        if (!string.IsNullOrEmpty(team))
            filter &= Builders<BsonDocument>.Filter.Regex("team_abbreviation", new BsonRegularExpression(team, "i"));

        // DELETE debug code after this works
        var total = await _data.Players.CountDocumentsAsync(filter);
        var players = await _data.Players.Find(filter).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

        ViewBag.NameFilter = name;
        ViewBag.TeamFilter = team;
        ViewBag.Page = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)total / pageSize);
        return View(players);
    }


}
