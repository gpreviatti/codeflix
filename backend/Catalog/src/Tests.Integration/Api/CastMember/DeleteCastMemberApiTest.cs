using Application.Dtos.CastMember;
using Application.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tests.Common.Generators.Entities;

namespace Tests.Integration.Api.CastMember;
public class DeleteCastMemberApiTest : CastMembersApiTestFixture
{
    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Api", "CastMember - Delete")]
    public async Task Delete()
    {
        var inputCreate = new CreateCastMemberInput(
            CastMemberGenerator.GetCastMamemberName(),
            CastMemberGenerator.GetRandomCastMemberType()
        );
        var (_, outputCreate) = await apiClient
            .Post<BaseResponse<CastMemberOutput>>(RESOURCE_URL, inputCreate);


        var responseDelete = await apiClient.Delete(RESOURCE_URL + "/" + outputCreate!.Data.Id);

        var (responseGet, outputGet) = await apiClient
            .Get<ProblemDetails>(RESOURCE_URL + "/" + outputCreate!.Data.Id);


        responseDelete!.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGet!.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        outputGet!.Detail.Should().Be($"CastMember '{outputCreate!.Data.Id}' not found.");
    }

    [Fact(DisplayName = nameof(ErrorCastMemberNotFound))]
    [Trait("Integration/Api", "CastMember - Delete")]
    public async Task ErrorCastMemberNotFound()
    {
        var id = Guid.NewGuid();


        var response = await apiClient.Delete(RESOURCE_URL + "/" + id);


        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
