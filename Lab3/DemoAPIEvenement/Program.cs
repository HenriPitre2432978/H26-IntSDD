using DemoEvent.Evenement.Models;


namespace DemoEvent
{
    class Program
    {
        static async Task Main(string[] args)
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                Console.WriteLine("=== GESTION DES ÉVÉNEMENTS (Salsa Saguenay) ===");
                Console.WriteLine("1. Afficher tous les événements (GET)");
                Console.WriteLine("2. Rechercher un événement par ID (GET)");
                Console.WriteLine("3. Ajouter un nouvel événement (POST)");
                Console.WriteLine("4. Modifier un événement existant (PUT)");
                Console.WriteLine("5. Modifier un événement existant (PATCH)");
                Console.WriteLine("6. Supprimer un événement (DELETE)");
                Console.WriteLine("q. Quitter");
                Console.Write("\nVotre choix : ");

                string choix = Console.ReadLine()?.ToLower();

                switch (choix)
                {
                    case "1":
                        await AfficherTous();
                        break;
                    case "2":
                        await AfficherParId();
                        break;
                    case "3":
                        await GestionnaireConsole.CreerEvenement();
                        break;
                    case "4":
                        await GestionnaireConsole.ModifierEvenement();
                        break;
                    case "5":
                        await GestionnaireConsole.ModifierEvenementPartiel();
                        break;
                    case "6":
                        await Supprimer();
                        break;
                    case "q":
                        continuer = false;
                        break;
                    default:
                        Console.WriteLine("Option invalide. Appuyez sur une touche...");
                        Console.ReadKey();
                        break;
                }

                if (continuer)
                {
                    Console.WriteLine("\nAction terminée. Appuyez sur une touche pour revenir au menu.");
                    Console.ReadKey();
                }
            }
        }

        // --- Méthodes de support pour le menu ---

        static async Task AfficherTous()
        {
            var list = await EvenementApiHelper.GetAllAsync();
            if (list != null)
            {
                foreach (var e in list)
                    Console.WriteLine($"[{e.Id}] {e.Titre} - {e.Artiste} ({e.Prix})");
            }
            else Console.WriteLine("Erreur ou liste vide.");
        }

        static async Task AfficherParId()
        {
            Console.Write("ID à rechercher : ");
            string id = Console.ReadLine();
            var e = await EvenementApiHelper.GetByIdAsync(id);
            if (e != null)
                Console.WriteLine($"\nTrouvé : {e.Titre} à {e.Lieu} le {e.Date:f}");
            else
                Console.WriteLine("Événement introuvable.");
        }

        static async Task Supprimer()
        {
            Console.Write("ID à supprimer : ");
            string id = Console.ReadLine();
            Console.Write($"Êtes-vous sûr de vouloir supprimer {id} ? (o/n) : ");
            if (Console.ReadLine()?.ToLower() == "o")
            {
                bool ok = await EvenementApiHelper.DeleteEvenementAsync(id);
                Console.WriteLine(ok ? "Supprimé avec succès." : "Erreur lors de la suppression.");
            }
        }
    }
}
