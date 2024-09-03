using gp_backend.Core.Models;

namespace gp_backend.EF.MySql.Repositories.Interfaces
{
    public interface IFeedBackRepo : IGenericRepo<FeedBack>
    {
        Task<List<FeedBack>> GetAllAsync();
    }
}
