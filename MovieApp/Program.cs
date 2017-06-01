using System;


namespace MovieApp
{
    class Program
    {
        //get an antire entity set
        static void ListAllMovies(Default.Container container)
        {
            foreach(var m in container.Movies){
                Console.WriteLine("{0} {1} {2} {3}", m.Title, m.Description, m.Rating, m.Year);
            }
        }

        static void AddMovie(Default.Container container, MovieService1.Models.Movie movie)
        {
            container.AddToMovies(movie);
            var serviceResponse = container.SaveChanges();
            foreach (var operationResponse in serviceResponse)
            {
                Console.WriteLine("Response: {0}", operationResponse.StatusCode);
            }
        }




        static void Main(string[] args)
        {
            // TODO: Replace with your local URI.
            string serviceUri = "http://localhost/2047469/odata";
            var container = new Default.Container(new Uri(serviceUri));

            var product = new MovieService1.Models.Movie()
            {
                Name = "Yo-yo",
                Category = "Toys",
                Price = 4.95M
            };

            AddMovie(container, product);
            ListAllMovies(container);

        }
    }
}
