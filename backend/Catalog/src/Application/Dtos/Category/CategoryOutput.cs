using DomainEntity = Domain.Entity;

namespace Application.Dtos.Category;

public class CategoryOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Is_Active { get; set; }
    public DateTime Created_At { get; set; }

    public CategoryOutput(
        Guid id,
        string name,
        string description,
        bool isActive,
        DateTime createdAt
    )
    {
        Id = id;
        Name = name;
        Description = description;
        Is_Active = isActive;
        Created_At = createdAt;
    }

    public static CategoryOutput FromCategory(DomainEntity.Category category) => new(
        category.Id,
        category.Name,
        category.Description,
        category.IsActive,
        category.CreatedAt
    );
}
