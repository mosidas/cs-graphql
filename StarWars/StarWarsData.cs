using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars.Types;

namespace StarWars;

public class StarWarsData
{
    private readonly List<Human> _humans = new List<Human>();
    private readonly List<Droid> _droids = new List<Droid>();

    public StarWarsData()
    {
        _humans.Add(new Human
        {
            Id = "1",
            Name = "Luke",
            Friends = new[] { "3", "4" },
            AppearsIn = new[] { Episodes.NEWHOPE, Episodes.EMPIRE, Episodes.JEDI },
            HomePlanet = "Tatooine"
        });
        _humans.Add(new Human
        {
            Id = "2",
            Name = "Vader",
            AppearsIn = new[] { Episodes.NEWHOPE, Episodes.EMPIRE, Episodes.JEDI },
            HomePlanet = "Tatooine"
        });

        _droids.Add(new Droid
        {
            Id = "3",
            Name = "R2-D2",
            Friends = new[] { "1", "4" },
            AppearsIn = new[] { Episodes.NEWHOPE, Episodes.EMPIRE, Episodes.JEDI },
            PrimaryFunction = "Astromech"
        });
        _droids.Add(new Droid
        {
            Id = "4",
            Name = "C-3PO",
            AppearsIn = new[] { Episodes.NEWHOPE, Episodes.EMPIRE, Episodes.JEDI },
            PrimaryFunction = "Protocol"
        });
    }

    public string GetMessage(string id)
    {
        return _humans.FirstOrDefault(h => h.Id == id).Messages.FirstOrDefault();
    }

    public IEnumerable<StarWarsCharacter> GetFriends(StarWarsCharacter character)
    {
        if (character == null)
        {
            return null;
        }

        var friends = new List<StarWarsCharacter>();
        var lookup = character.Friends;
        if (lookup != null)
        {
            foreach (var h in _humans.Where(h => lookup.Contains(h.Id)))
                friends.Add(h);
            foreach (var d in _droids.Where(d => lookup.Contains(d.Id)))
                friends.Add(d);
        }
        return friends;
    }

    public Task<Human> GetHumanByIdAsync(string id)
    {
        return Task.FromResult(_humans.FirstOrDefault(h => h.Id == id));
    }

    public Task<Droid> GetDroidByIdAsync(string id)
    {
        return Task.FromResult(_droids.FirstOrDefault(h => h.Id == id));
    }

    public Human AddHuman(Human human)
    {
        human.Id = Guid.NewGuid().ToString();
        _humans.Add(human);
        return human;
    }

    internal Task<object> GetHumansAsync(Episodes episode)
    {
        var humans = new List<Human>();
        foreach (var h in _humans.Where(h => h.AppearsIn.Contains(episode)))
        {
            humans.Add(h);
        }

        return Task.FromResult<object>(humans);
    }
}
