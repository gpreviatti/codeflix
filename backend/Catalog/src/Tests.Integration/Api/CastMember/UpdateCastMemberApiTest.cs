using Application.Dtos.CastMember;
using Application.Messages;
using System.Net;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Api.CastMember;
public class UpdateCastMemberApiTest : CastMembersApiTestFixture
{
    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Api", "CastMember - Update")]
    public async Task Update()
    {
        var input = new CreateCastMemberInput(
            CastMemberGenerator.GetCastMamemberName(),
            CastMemberGenerator.GetRandomCastMemberType()
        );

        var (_, outputCreate) = await apiClient
            .Post<BaseResponse<CastMemberOutput>>(RESOURCE_URL, input);

        var inputUpdate = new UpdateCastMemberInput(
            outputCreate.Data.Id,
            CastMemberGenerator.GetCastMamemberName(),
            CastMemberGenerator.GetRandomCastMemberType()
        );

        var (response, output) = await apiClient
            .Put<BaseResponse<CastMemberOutput>>(RESOURCE_URL, inputUpdate);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<BaseResponse<CastMemberOutput>>().And.NotBeNull();
        output!.Data.Id.Should().Be(outputCreate!.Data.Id);
        output!.Data.Name.Should().Be(inputUpdate.Name);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Integration/Api", "CastMember - Update")]
    public async Task UpdateOnlyName()
    {
        var inputCreate = new CreateCastMemberInput(
            CastMemberGenerator.GetCastMamemberName(),
            CastMemberGenerator.GetRandomCastMemberType()
        );

        var (_, outputCreate) = await apiClient
            .Post<BaseResponse<CastMemberOutput>>(RESOURCE_URL, inputCreate);

        var inputUpdate = new UpdateCastMemberInput(
            outputCreate.Data.Id,
            CastMemberGenerator.GetCastMamemberName(),
            default
        );

        var (response, output) = await apiClient
            .Put<BaseResponse<CastMemberOutput>>(RESOURCE_URL, inputUpdate);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<BaseResponse<CastMemberOutput>>().And.NotBeNull();
        output!.Data.Id.Should().Be(outputCreate!.Data.Id);
        output!.Data.Name.Should().Be(inputUpdate.Name);
        output!.Data.Type.Should().Be(inputCreate.Type);
    }
}
