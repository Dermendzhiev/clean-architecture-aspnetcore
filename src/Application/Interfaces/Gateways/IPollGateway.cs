namespace CleanArchitecture.Application.Interfaces.Gateways
{
    using System.Threading.Tasks;
    using CleanArchitecture.Domain.Entities;

    public interface IPollGateway
    {
        Task<Poll> GetAsync(int id);

        Task CreateAsync(Poll poll);

        Task UpdateAsync(Poll poll);

        Task DeleteAsync(Poll poll);
    }
}
