﻿namespace FC.CodeFlix.Catalog.Api.Common;

public abstract class SearchPayload<TPayload>
    where TPayload : class
{
    public int CurrentPage { get; set; }

    public int PerPage { get; set; }

    public int Total { get; set; }

    public IReadOnlyList<TPayload> Items { get; set; } = null!;
}
