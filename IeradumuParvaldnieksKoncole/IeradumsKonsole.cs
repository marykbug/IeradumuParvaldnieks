using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksTestData;

List<Ieradums> ieradumi = IeradumsTestData.IzveidotIeradumus();

Console.WriteLine($"Kopējais ieradumu skaits: {ieradumi.Count}");
List<string> lietotaji = new List<string>();

foreach (Ieradums ieradums in ieradumi)
{
    if (!lietotaji.Contains(ieradums.LietotajsID))
    {
        lietotaji.Add(ieradums.LietotajsID);
    }
}

Console.WriteLine("Lietotāji:");

foreach (string lietotajs in lietotaji)
{
    Console.WriteLine(lietotajs);
}

Console.WriteLine();

foreach (string lietotajs in lietotaji)
{
    Console.WriteLine("Lietotājs: " + lietotajs);

    foreach (Ieradums ieradums in ieradumi)
    {
        if (ieradums.LietotajsID == lietotajs)
        {
            Console.WriteLine("Nosaukums: " + ieradums.Nosaukums);
            Console.WriteLine("Statuss: " + ieradums.Status);
            Console.WriteLine("Atkārtošanās: " + ieradums.Atkartosana);

            if (ieradums.IrIzpilditsSodien())
            {
                Console.WriteLine("Šodien izpildīts: Jā");
            }
            else
            {
                Console.WriteLine("Šodien izpildīts: Nē");
            }

            Console.WriteLine();
        }
    }
}