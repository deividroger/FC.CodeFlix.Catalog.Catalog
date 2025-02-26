using FC.CodeFlix.Catalog.Domain.Enums;
using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using FC.CodeFlix.Catalog.Infra.ES.Models;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
namespace FC.CodeFlix.Catalog.Tests.Shared;

public class CastMemberDataGenerator : DataGeneratorBase
{
    public DomainEntity.CastMember GetValidCastMember()
        => new(
            Guid.NewGuid(),
            GetValidName(),
            GetRandomCastMemberType()
            );

    public string GetValidName()
        => Faker.Name.FullName();

    public CastMemberType GetRandomCastMemberType()
        => (CastMemberType)new Random().Next(1, 2);

    public IList<CastMemberModel> GetCastMemberModelList(int count)
        => Enumerable.Range(0, count)
                            .Select(_ =>
                            {
                                Task.Delay(5).GetAwaiter().GetResult();
                                return CastMemberModel.FromEntity(GetValidCastMember());
                            })
                            .ToList();

    public IList<CastMemberModel> GetCastMemberModelList(List<string> castMemberName)
         => castMemberName.Select(name =>
         {
             Task.Delay(5).GetAwaiter().GetResult();
             var castMember = CastMemberModel.FromEntity(GetValidCastMember());
             castMember.Name = name;
             return castMember;
         }).ToList();

    public IList<CastMemberModel> CloneCastMemberListOrdered(
    IList<CastMemberModel> castMemberList,
    string orderBy,
    SearchOrder direction)
    {
        var listClone = new List<CastMemberModel>(castMemberList);
        var orderedEnumerable = (orderBy.ToLower(), direction) switch
        {
            ("name", SearchOrder.ASC) => listClone.OrderBy(x => x.Name)
                .ThenBy(x => x.Id),
            ("name", SearchOrder.DESC) => listClone.OrderByDescending(x => x.Name)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.ASC) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.DESC) => listClone.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.ASC) => listClone.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.DESC) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
        };
        return orderedEnumerable.ToList();
    }
}