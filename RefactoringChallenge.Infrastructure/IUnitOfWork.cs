namespace RefactoringChallenge.Infrastructure;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}