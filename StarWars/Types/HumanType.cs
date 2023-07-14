using GraphQL.Types;

namespace StarWars.Types;

public class HumanType : ObjectGraphType<Human>
{
    public HumanType()
    {
        Name = "Human";

        Field(h => h.Id).Description("The id of the human.");
        Field(h => h.Name, nullable: true).Description("The name of the human.");
        Field(d => d.Friends, nullable: true).Description("The friends of the character.");
        Field(d => d.AppearsIn, nullable: true).Description("Which movie they appear in.");
        Field(h => h.HomePlanet, nullable: true).Description("The home planet of the human.");
        Field(h => h.Messages, nullable: true).Description("The messages of the human.");

        Interface<CharacterInterface>();
    }
}