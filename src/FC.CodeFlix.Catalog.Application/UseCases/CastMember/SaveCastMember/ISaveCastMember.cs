using FC.CodeFlix.Catalog.Application.UseCases.CastMember.Common;
using MediatR;

namespace FC.CodeFlix.Catalog.Application.UseCases.CastMember.SaveCastMember;

public interface ISaveCastMember : IRequestHandler<SaveCastMemberInput, CastMemberModelOutput>
{
}
