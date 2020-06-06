using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateIndex();
        }

        public static void CreateIndex()
        {
            var node = new Uri("http://localhost:9200/");
            var settings = new ConnectionSettings(node);

            settings.DefaultIndex("defaultindex")
                .DefaultMappingFor<Post>(m => m.IndexName("posts")
                );

            var post = new Post()
            {
                PostDate = DateTime.Now,
                PostTest = "Volvo",
                UserID = 1
            };
            var post2 = new Post()
            {
                PostDate = DateTime.Now,
                PostTest = "Wolkswagen",
                UserID = 2
            };


            var client = new ElasticClient(settings);

            client.Index<Post>(post, idx => idx.Index("car"));
            client.Index<Post>(post2, idx => idx.Index("car"));


            var response = client.Search<Post>(s => s
                 .Index("car")
                 .Query(q => 
                 //q.Term(f=>f.UserID , 2) "id'si 2 olanları getiriyor"
                  q.MatchPhrasePrefix(mq => mq.Field(f=>f.PostTest).Query("Volvo"))
                 )
                 );

            int sayi = 0;
            foreach (var item in response.Documents)
            {
                sayi += 1;
                Console.WriteLine(sayi + " - " + "\n" + "Post Date :" + item.PostDate + "\n" + "Post Test :" + item.PostTest + "\n" +
                  "Post Id :" + item.UserID + "\n" + "-------------------------------------------");

            }
            Console.ReadLine();
        }
    }
}
