using DemoEvent.Evenement.Models;
using System.Runtime.ExceptionServices;
using System.Text.Json;


public static class GestionnaireConsole
{
    // Méthode pour le POST (Création)
    public static async Task CreerEvenement()
    {
        Console.WriteLine("\n--- AJOUT D'UN ÉVÉNEMENT ---");
        //Evenement evt = new Evenement();
        var evt = new Evenement();

        evt.Id = SaisirChamp("ID : ", true);
        evt.Titre = SaisirChamp("Titre : ", true);

        // Validation spécifique pour la date
        string dateStr = SaisirChamp("Date (AAAA-MM-JJ) : ", true);
        DateTime dateValide;
        while (!DateTime.TryParse(dateStr, out dateValide))
        {
            Console.WriteLine("Format invalide ! Veuillez utiliser AAAA-MM-JJ.");
            dateStr = SaisirChamp("Date : ", true);
        }
        evt.Date = dateValide;

        evt.Lieu = SaisirChamp("Lieu : ", true);
        evt.Artiste = SaisirChamp("Artiste : ", true);
        evt.Prix = SaisirChamp("Prix (ex: 45$) : ", true);

        bool success = await EvenementApiHelper.PostEvenementAsync(evt);
        Console.WriteLine(success ? "Événement ajouté avec succès !" : "Erreur lors de l'envoi à l'API.");
    }

    // Méthode pour le PUT (Modification)
    public static async Task ModifierEvenement()
    {
        Console.WriteLine("\n--- MODIFICATION D'UN ÉVÉNEMENT ---");
        Console.Write("Entrez l'ID de l'événement à modifier : ");
        string id = Console.ReadLine();

        // 1. On récupère d'abord les données actuelles
        var existant = await EvenementApiHelper.GetByIdAsync(id);

        if (existant == null)
        {
            Console.WriteLine("Événement introuvable sur le serveur.");
            return;
        }

        Console.WriteLine("\n(Laissez vide pour conserver la valeur actuelle)");

        // 2. On propose de modifier chaque champ
        string nTitre = SaisirChamp($"Titre [{existant.Titre}] : ", false);
        if (!string.IsNullOrEmpty(nTitre)) existant.Titre = nTitre;

        string nDate = SaisirChamp($"Date [{existant.Date:yyyy-MM-dd}] : ", false);
        if (!string.IsNullOrEmpty(nDate) && DateTime.TryParse(nDate, out DateTime d))
            existant.Date = d;

        string nLieu = SaisirChamp($"Lieu [{existant.Lieu}] : ", false);
        if (!string.IsNullOrEmpty(nLieu)) existant.Lieu = nLieu;

        string nArtiste = SaisirChamp($"Artiste [{existant.Artiste}] : ", false);
        if (!string.IsNullOrEmpty(nArtiste)) existant.Artiste = nArtiste;

        string nPrix = SaisirChamp($"Prix [{existant.Prix}] : ", false);
        if (!string.IsNullOrEmpty(nPrix)) existant.Prix = nPrix;

        // 3. On renvoie l'objet complet mis à jour
        bool success = await EvenementApiHelper.PutEvenementAsync(id, existant);
        Console.WriteLine(success ? "Modification enregistrée !" : "Erreur lors de la mise à jour.");
    }

    // Méthode pour le PATCH (Modification)
    public static async Task ModifierEvenementPartiel()
    {
        Console.WriteLine("\n--- MODIFICATION D'UN ÉVÉNEMENT ---");
        Console.Write("Entrez l'ID de l'événement à modifier : ");
        string id = Console.ReadLine();

        // 1. On récupère d'abord les données actuelles
        Evenement? existant = await EvenementApiHelper.GetByIdAsync(id);

        if (existant == null)
        {
            Console.WriteLine("Événement introuvable sur le serveur.");
            return;
        }

        Console.WriteLine("\n(Choisissez quelle propriété modifier de l'évènement.)");
        Console.WriteLine("Saisir l'identifiant:");

        //Pour chaque champ dans un événement
        foreach (var p in typeof(Evenement).GetProperties())
        {
            int idx = 0;
            var name = p.Name;
            var value = p.GetValue(existant);

            if (idx != 0)
            Console.WriteLine($"[{idx+1}] {name} = {value}");
            idx++;
        }

        string? saisieTxt;
        do
        {
            Console.WriteLine("Choix:");
            saisieTxt = Console.ReadLine();

        } while (string.IsNullOrWhiteSpace(saisieTxt) || int.TryParse(saisieTxt, out int s));
        int choix = int.Parse(saisieTxt) -1 ;

        bool valid = false;
        switch (choix)
        {
            case 0:
                string nTitre = SaisirChamp($"Titre [{existant.Titre}] : ", false);
                if (!string.IsNullOrEmpty(nTitre)) existant.Titre = nTitre;
                valid = true;
                break;
            case 1:
                string nDate = SaisirChamp($"Date [{existant.Date:yyyy-MM-dd}] : ", false);
                if (!string.IsNullOrEmpty(nDate) && DateTime.TryParse(nDate, out DateTime d))
                    existant.Date = d;
                valid = true;
                break;
            case 2:
                string nLieu = SaisirChamp($"Lieu [{existant.Lieu}] : ", false);
                if (!string.IsNullOrEmpty(nLieu)) existant.Lieu = nLieu;
                valid=true;
                break;
            case 3: 
                string nArtiste = SaisirChamp($"Artiste [{existant.Artiste}] : ", false);
                if (!string.IsNullOrEmpty(nArtiste)) existant.Artiste = nArtiste;
                valid = true;
                break;
            case 4:
                string nPrix = SaisirChamp($"Prix [{existant.Prix}] : ", false);
                if (!string.IsNullOrEmpty(nPrix)) existant.Prix = nPrix;
                valid = true;
                break;
            default:
                break;
        }
        
        if (valid)
        {
            bool success = await EvenementApiHelper.PatchAsync(id, existant);
            Console.WriteLine(success ? "Modification enregistrée !" : "Erreur lors de la mise à jour.");
        }
    }

    // Outil pratique pour valider la saisie
    private static string SaisirChamp(string message, bool obligatoire)
    {
        string saisie;
        do
        {
            Console.Write(message);
            saisie = Console.ReadLine()?.Trim();
            if (obligatoire && string.IsNullOrEmpty(saisie))
            {
                Console.WriteLine("Erreur : Ce champ ne peut pas être vide.");
            }
        } while (obligatoire && string.IsNullOrEmpty(saisie));

        return saisie;
    }
}