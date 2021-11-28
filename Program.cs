using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;


namespace moment3_1
{
    class Program
    {
        private const string fileName = @"Book.json";
        private static int index = 0;
        
        private static Book[] bookArray;
        static void Main(string[] args)
        {
            bool showMainMenu = true;
            // Loop som visar menyn tills användaren väljer att stänga ner
            while (showMainMenu)
            {
                showMainMenu = MainMenu();
            }
        } 

        public static bool MainMenu()
        {
            // Meny som visar olika alternativ att välja
            index=0;
            Console.Clear();
            Console.WriteLine("ASHRAFS |GÄSTBOK");
            Console.WriteLine("----------------\r\n");
            Console.WriteLine("Välj ett alternativ:");
            Console.WriteLine("1) Skriv i gästboken");
            Console.WriteLine("2) Ta bort inlägg");
            Console.WriteLine("3) Avsluta\r\n");
            // Läser in funktionen för att visa alla inlägg
            readGuestBookPosts();

            // Switch som läser in olika menyval för att därefter kalla på olika funktioner
            switch (Console.ReadLine())
            {
                case "1":
                    addGuestBookPosts();
                    return true;
                case "2":
                    deleteGuestBookPosts();
                    return true;
                case "3":
                    return false;
                default:
                    return true;
            }
        }
       
        private static void readGuestBookPosts(){
            // Kollar om filen inte existerar, då skrivs nedanstående meddelande ut
            if (!File.Exists(fileName)){
                System.Console.WriteLine("Inga poster ännu");
            }
            else
            {
                // Om filen finns, då deserialiseras innehållet till array
                string jsonString = File.ReadAllText(fileName);
                bookArray = JsonSerializer.Deserialize<Book[]>(jsonString);

                // Loop skapar objekt för varje index i arrayen
                for(int i =0;i<bookArray.Length;i++){
                    Book newBookEntry = new Book();
                    newBookEntry.Name = bookArray[i].Name;
                    newBookEntry.Content = bookArray[i].Content;
                    // Skriver ut innehållet till användaren
                    System.Console.WriteLine($"[{index}] {newBookEntry.Name} - {newBookEntry.Content}");
                    // Vi ökar index med ett för varje omgång för att hålla koll på hur många inlägg som skrivits ut
                    index++;
                }
            }
        } 

        private static void addGuestBookPosts(){
            // Användaren uppmanas att skriva ett inlägg vid start av funktionen
            Console.Write("Namn på författare: ");
            string name = Console.ReadLine();
            
            Console.Write("Vad vill du skriva:  ");
            string content = Console.ReadLine();

            bool stop = true;
            
            while(stop!=false){
                // Felhantering som kollar ifall användaren inte matat in tom sträng eller endast space
                if((!string.IsNullOrEmpty(name)&&!string.IsNullOrEmpty(content))&&
                    name!=" " && content!= " "){
                    Book newEntry=new Book()
                    {
                    Name = name,
                    Content = content
                    };
                    // Om filen redan finns, läses innehållet in och deserialiseras till array och sedan till en lista
                    if (File.Exists(fileName)){
                        string jsonString = File.ReadAllText(fileName);
                        bookArray = JsonSerializer.Deserialize<Book[]>(jsonString);
                        List<Book> bookLista = bookArray.ToList();
                        // Vi lägger till nytt objekt i listan
                        bookLista.Add(newEntry);

                        // Serialiserar alla objekt och skriver till fil
                        jsonString = JsonSerializer.Serialize(bookLista);
                        File.WriteAllText(fileName, jsonString);

                        //byte[] jsonUtf8Bytes =JsonSerializer.SerializeToUtf8Bytes(weatherForecast);
                    }

                    else{
                        File.Create(fileName).Dispose();
                        List<Book> newBookEntry = new List<Book>();
                        newBookEntry.Add(newEntry);
                        string jsonString = JsonSerializer.Serialize(newBookEntry);
                        File.WriteAllText(fileName, jsonString);
                    }
                    stop = false;      
            }
           
            else{
                // Skulle användaren mata in tom sträng visas nedanstående och de uppmanas ange på nytt
                Console.WriteLine("Varning! Författare och innehåll får inte vara tomt, försök igen!");
                Console.Write("Namn på författare: ");
                name = Console.ReadLine();
            
                Console.Write("Vad vill du skriva:  ");
                content = Console.ReadLine();     
            }        
        }
        }

        private static void deleteGuestBookPosts(){
            // Om filen excisterar, läs in innehåll, deseralisera, skriv till lista
            if(File.Exists(fileName)){
            string jsonString = File.ReadAllText(fileName);
            bookArray = JsonSerializer.Deserialize<Book[]>(jsonString);
            List<Book> bookLista = bookArray.ToList();
            
            // Frågar användaren efter index som skall raderas
            Console.Write("Ange index på det inlägg du vill radera?");
            int deleteIndex = int.Parse(Console.ReadLine());
            bool stop = true;

            while(stop!=false){
                // Om index är mindre än tillgängliga poster, raderas inlägget från listan för att sedan serialiseras på nytt och skrivas till fil
                if(deleteIndex>=0 && deleteIndex<index){
                bookLista.RemoveAt(deleteIndex);
                // Serialiserar alla objekt och skriver till fil
                jsonString = JsonSerializer.Serialize(bookLista);
                File.WriteAllText(fileName, jsonString);
                stop = false;
            }
            else{
                // Om inte index finns, skrivs nedanstående och användaren får försöka igen
                Console.WriteLine("Kunde inte hitta angivet index!");
                Console.Write("Ange index på det inlägg du vill radera?");
                deleteIndex = int.Parse(Console.ReadLine());     
            }
        }
            }
            
        } 
}
}
