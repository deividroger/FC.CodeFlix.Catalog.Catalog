﻿namespace FC.CodeFlix.Catalog.Domain.Exceptions;

public abstract class BusinessRuleException : Exception
{
    protected BusinessRuleException(string? message) : base(message)
    {
    }
}
