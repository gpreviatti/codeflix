using Application.Dtos.CastMember;
using Application.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Api.CastMember;
public class GetCastMemberApiTest : CastMembersApiTestFixture
{
    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Api", "CastMember - Get")]
    public async Task Get()
    {
        var inputCreate = new CreateCastMemberInput(
            CastMemberGenerator.GetCastMamemberName(),
            CastMemberGenerator.GetRandomCastMemberType()
        );

        var (_, outputCreate) = await apiClient
            .Post<BaseResponse<CastMemberOutput>>(RESOURCE_URL, inputCreate);

        
        var (response, output) = await apiClient
            .Get<BaseResponse<CastMemberOutput>>(RESOURCE_URL + "/" + outputCreate!.Data.Id);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        output.Should().NotBeNull();
        output!.GetType().Should().Be<BaseResponse<CastMemberOutput>>().And.NotBeNull();
        output!.Data.Id.Should().Be(outputCreate.Data.Id);
        output!.Data.Name.Should().Be(inputCreate.Name);
    }

    [Fact(DisplayName = nameof(ErrorCastMemberNotFound))]
    [Trait("Integration/Api", "CastMember - Get")]
    public async Task ErrorCastMemberNotFound()
    {
        var id = Guid.NewGuid();
        var (response, output) = await apiClient
            .Get<ProblemDetails>(RESOURCE_URL + "/" + id);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        output.Should().NotBeNull();
        output!.Title.Should().Be("An unexpected error ocurred");
        output!.Type.Should().Be("UnexpectedError");
        output!.Status.Should().Be((int)HttpStatusCode.InternalServerError);
        output!.Detail.Should().Be($"CastMember '{id}' not found.");
    }
}
