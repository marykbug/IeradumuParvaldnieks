using System.Net;
using System.Net.Http.Json;
using IeradumuParvaldnieksDataModels;
using IeradumuParvaldnieksKlases;

namespace IeradumuParvaldnieksMAUI.Services
{
    public class APIIeradumuService
    {
        private readonly HttpClient httpClient;

        public APIIeradumuService()
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5118/")
            };
        }

        // Iegūst visus ieradumus no API.
        public async Task<List<Ieradums>> IegutVisusAsync()
        {
            List<IeradumsDataModel>? modeli =
                await httpClient.GetFromJsonAsync<List<IeradumsDataModel>>(
                    "api/ieradumi"
                );

            List<Ieradums> ieradumi = new List<Ieradums>();

            if (modeli == null)
            {
                return ieradumi;
            }

            foreach (IeradumsDataModel modelis in modeli)
            {
                Ieradums ieradums =
                    await ParveidotParIeradumuAsync(modelis);

                ieradumi.Add(ieradums);
            }

            return ieradumi;
        }

        // Iegūst vienu ieradumu pēc ID.
        public async Task<Ieradums?> IegutPecIdAsync(Guid id)
        {
            HttpResponseMessage response =
                await httpClient.GetAsync(
                    $"api/ieradumi/{id}"
                );

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            IeradumsDataModel? modelis =
                await response.Content
                    .ReadFromJsonAsync<IeradumsDataModel>();

            if (modelis == null)
            {
                return null;
            }

            return await ParveidotParIeradumuAsync(modelis);
        }

        // Izveido jaunu ieradumu.
        public async Task<Ieradums> IzveidotAsync(
            IzveidotIeradumsDataModel modelis)
        {
            HttpResponseMessage response =
                await httpClient.PostAsJsonAsync(
                    "api/ieradumi",
                    modelis
                );

            await ParbauditAtbildi(response);

            IeradumsDataModel? izveidotaisModelis =
                await response.Content
                    .ReadFromJsonAsync<IeradumsDataModel>();

            if (izveidotaisModelis == null)
            {
                throw new InvalidOperationException(
                    "Neizdevās saņemt izveidoto ieradumu."
                );
            }

            return await ParveidotParIeradumuAsync(
                izveidotaisModelis
            );
        }

        // Labo esošu ieradumu.
        public async Task LabotAsync(
            Guid id,
            LabotIeradumuDataModel modelis)
        {
            HttpResponseMessage response =
                await httpClient.PutAsJsonAsync(
                    $"api/ieradumi/{id}",
                    modelis
                );

            await ParbauditAtbildi(response);
        }

        // Arhivē ieradumu.
        public async Task ArhivetAsync(Guid id)
        {
            HttpResponseMessage response =
                await httpClient.PutAsync(
                    $"api/ieradumi/{id}/arhivet",
                    null
                );

            await ParbauditAtbildi(response);
        }

        // Atzīmē ieradumu kā izpildītu.
        public async Task AtzimetKaIzpildituAsync(
            Guid id,
            AtzimetIzpildiDataModel modelis)
        {
            HttpResponseMessage response =
                await httpClient.PostAsJsonAsync(
                    $"api/ieradumi/{id}/izpildes",
                    modelis
                );

            await ParbauditAtbildi(response);
        }

        // Iegūst ieraduma izpildes.
        public async Task<List<IeradumaIzpildeDataModel>>
            IegutIzpildesAsync(Guid id)
        {
            List<IeradumaIzpildeDataModel>? izpildes =
                await httpClient.GetFromJsonAsync<
                    List<IeradumaIzpildeDataModel>>(
                        $"api/ieradumi/{id}/izpildes"
                    );

            return izpildes ??
                new List<IeradumaIzpildeDataModel>();
        }

        // Pārveido API datus par domēna Ieradums objektu.
        private async Task<Ieradums> ParveidotParIeradumuAsync(
            IeradumsDataModel modelis)
        {
            if (!Enum.TryParse(
                modelis.Atkartosana,
                out IeradumuAtkartosana atkartosana))
            {
                throw new InvalidOperationException(
                    "Nederīgs atkārtošanās veids no API."
                );
            }

            if (!Enum.TryParse(
                modelis.Status,
                out IeradumuStatus status))
            {
                throw new InvalidOperationException(
                    "Nederīgs ieraduma statuss no API."
                );
            }

            List<NedelasDienas> nedelasDienas =
                new List<NedelasDienas>();

            foreach (string diena in modelis.NedelasDienas)
            {
                if (Enum.TryParse(
                    diena,
                    out NedelasDienas nedelasDiena))
                {
                    nedelasDienas.Add(nedelasDiena);
                }
            }

            List<IeradumaIzpildeDataModel> izpildes =
                await IegutIzpildesAsync(modelis.Id);

            List<DateOnly> izpildesDatumi =
                new List<DateOnly>();

            foreach (IeradumaIzpildeDataModel izpilde in izpildes)
            {
                izpildesDatumi.Add(izpilde.Datums);
            }

            return new Ieradums(
                modelis.Id,
                modelis.Nosaukums,
                modelis.LietotajsID,
                modelis.Apraksts,
                atkartosana,
                nedelasDienas,
                status,
                modelis.Izveidots,
                modelis.Labots,
                izpildesDatumi
            );
        }

        // Pārbauda API atbildi un atgriež saprotamu kļūdu.
        private async Task ParbauditAtbildi(
            HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            string kluda =
                await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(kluda))
            {
                kluda = "API pieprasījums neizdevās.";
            }

            throw new InvalidOperationException(kluda);
        }
        public async Task AtjaunotNoArhivaAsync(Guid id)
        {
            HttpResponseMessage response =
                await httpClient.PutAsync(
                    $"api/ieradumi/{id}/atjaunot",
                    null
                );

            await ParbauditAtbildi(response);
        }
    }
}