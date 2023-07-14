using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using StarWars.Types;

namespace StarWars;

public class StarWarsQuery : ObjectGraphType<object>
{
    public StarWarsQuery(StarWarsData data)
    {
        Name = "Query";

        Field<CharacterInterface>("hero")
            // 引数を取らない場合は省略可能
            .ResolveAsync(async context => await data.GetDroidByIdAsync("3"));

        Field<HumanType>("human")
            // 引数
            .Argument<NonNullGraphType<StringGraphType>>("id", "id of the human")
            // query実行時に呼ばれる
            .ResolveAsync(async context =>
            {
                return await data.GetHumanByIdAsync(context.GetArgument<string>("id"));
            });

        Field<DroidType>("droid")
            .Argument<NonNullGraphType<StringGraphType>>("id", "id of the droid")
            .ResolveAsync(async context => await data.GetDroidByIdAsync(context.GetArgument<string>("id")));

        // query HumansAppearingIn($episode: Episode!){
        // humans(appearsIn: $episode){
        //     id
        //     name
        //     appearsIn
        //     }
        // }
        Field<ListGraphType<HumanType>>("humans")
            .Argument<NonNullGraphType<EpisodeEnum>>("appearsIn","episode")
            .ResolveAsync(async context => await data.GetHumansAsync(context.GetArgument<Episodes>("appearsIn")));
    }
}
