using GraphQL.Types;

namespace StarWars.Types;

public class DroidType : ObjectGraphType<Droid>
{
    public DroidType()
    {
        Name = "Droid";
        Description = "A mechanical creature in the Star Wars universe.";

        Field(d => d.Id).Description("The id of the droid.");
        Field(d => d.Name, nullable: true).Description("The name of the droid.");
        Field(d => d.Friends, nullable: true).Description("The friends of the character.");
        Field(d => d.AppearsIn, nullable: true).Description("Which movie they appear in.");
        Field(d => d.PrimaryFunction, nullable: true).Description("The primary function of the droid.");

        Interface<CharacterInterface>();
    }
}
