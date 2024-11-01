﻿using FC.CodeFlix.Catalog.Domain.Repositories.DTOs;
using FluentAssertions;
using NSubstitute;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;
using UseCase = FC.CodeFlix.Catalog.Application.UseCases.Category.SearchCategory;

namespace FC.CodeFlix.Catalog.UnitTests.Application.UseCases.Categories.SearchCategory;

[Collection(nameof(SearchCategoryTestFixture))]
public class SearchCategoryTest
{
    private readonly SearchCategoryTestFixture _fixture;

    public SearchCategoryTest(SearchCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ReturnsSearchResult))]
    [Trait("Application", "[UseCase] SearchCategory")]
    public async Task ReturnsSearchResult()
    {
        var repository = _fixture.GetMockRepository();

        var categories = _fixture.GetCategoryList();

        var input = _fixture.GetSearchInput();

        var expectedQueryResult = new SearchOutput<DomainEntity.Category>(
           input.Page, input.PerPage, input.PerPage, categories);

        repository.SearchAsync(Arg.Any<SearchInput>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(expectedQueryResult));

        var useCase = new UseCase.SearchCategory(repository);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();

        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);

        output.Total.Should().Be(expectedQueryResult.Total);

        output.Items.Should().BeEquivalentTo(categories);

        await repository.Received(1).SearchAsync(
            Arg.Is<SearchInput>(search =>
            search.Page == input.Page &&
            search.Search == input.Search &&
            search.PerPage == input.PerPage &&
            search.Order == input.Order &&
            search.OrderBy == input.OrderBy),
            Arg.Any<CancellationToken>());
    }

}
