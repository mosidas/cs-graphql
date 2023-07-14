using GraphQL.Resolvers;
using GraphQL.Types;

namespace StarWars.Types;

public class CharacterInterface : InterfaceGraphType<StarWarsCharacter>
{
    public CharacterInterface()
    {
        Name = "Character";

        Field(d => d.Id).Description("The id of the character.");
        Field(d => d.Name, nullable: true).Description("The name of the character.");
        Field(d => d.Friends, nullable: true).Description("The friends of the character.");
        Field(d => d.AppearsIn, nullable: true).Description("Which movie they appear in.");
    }
}
