using System.Text.Json.Serialization;
using DomainEntity = Domain.Entity;

namespace Application.Dtos.Category;

public class CategoryOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Is_Active { get; set; }
    public DateTime Created_At { get; set; }

    [JsonConstructor]
    public CategoryOutput(
        Guid id,
        string name,
        string description,
        bool is_Active,
        DateTime created_At
    )
    {
        Id = id;
        Name = name;
        Description = description;
        Is_Active = is_Active;
        Created_At = created_At;
    }

    public static CategoryOutput FromCategory(DomainEntity.Category category) => new(
        category.Id,
        category.Name,
        category.Description,
        category.IsActive,
        category.CreatedAt
    );
}
