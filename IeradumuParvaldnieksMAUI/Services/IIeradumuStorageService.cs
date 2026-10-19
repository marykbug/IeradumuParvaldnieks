using IeradumuParvaldnieksKlases;
namespace IeradumuParvaldnieksMAUI.Services
{
    public interface IIeradumuStorageService
    {
        // Ielādē saglabātos ieradumus.
        Task<List<Ieradums>> IeladetAsync();
        // Saglabā ieradumus lokālajā failā.
        Task SaglabatAsync(IEnumerable<Ieradums> ieradumi);
    }
}
