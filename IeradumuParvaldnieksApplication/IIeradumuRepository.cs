using IeradumuParvaldnieksKlases;

namespace IeradumuParvaldnieksApplication
{
    public interface IIeradumuRepository
    {
        // Iegūst visus ieradumus.
        Task<List<Ieradums>> IegutVisusAsync();
        // Iegūst vienu ieradumu pēc identifikatora.
        Task<Ieradums?> IegutPecIdAsync(Guid id);
        // Pievieno jaunu ieradumu.
        Task PievienotAsync(Ieradums ieradums);
        // Saglabā izmaiņas esošā ieradumā.
        Task SaglabatAsync(Ieradums ieradums);
    }
}
