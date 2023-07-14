using GraphQL.Types;

namespace StarWars.Types;

public class HumanInputType : InputObjectGraphType<Human>
{
    public HumanInputType()
    {
        Name = "HumanInput";
        Field(x => x.Name).Description("The name of the human.");
        Field(x => x.HomePlanet, nullable: true).Description("The home planet of the human.");
    }
}
