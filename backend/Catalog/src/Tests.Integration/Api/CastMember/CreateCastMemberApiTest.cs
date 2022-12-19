using Application.Dtos.CastMember;
using Application.Messages;
using System.Net;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Api.CastMember;
public class CreateCastMemberApiTest : CastMembersApiTestFixture
{
    [Fact(DisplayName = nameof(Create))]
    [Trait("Integration/Api", "CastMember - Create")]
    public async Task Create()
    {
        var input = new CreateCastMemberInput(
            CastMemberGenerator.GetCastMamemberName(),
            CastMemberGenerator.GetRandomCastMemberType()
        );

        var (response, output) = await apiClient
            .Post<BaseResponse<CastMemberOutput>>(RESOURCE_URL, input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.Created);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<BaseResponse<CastMemberOutput>>().And.NotBeNull();
        output!.Data.Id.Should().NotBeEmpty();
        output!.Data.Name.Should().Be(input.Name);
    }
}
