using GraphQL.Instrumentation;
using GraphQL.Types;

namespace StarWars;

public class StarWarsSchema : Schema
{
    public StarWarsSchema(StarWarsData data, IChat chat)
    {
        Query = new StarWarsQuery(data);
        Mutation = new StarWarsMutation(data);
        Subscription = new MessageSubscription(chat);

        FieldMiddleware.Use(new InstrumentFieldsMiddleware());
    }
}