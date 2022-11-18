
Projeto para testar o uso de response cache no .net 7.


O cache pode nos ajudar a entregar respostas mais rápidas, sendo configuradas conforme a necessidade. Existem várias formas e estratégias para utilizar cache em api’s, uma delas é um cache com o responseCache. Neste artigo veremos como implementar e quais os benefícios podemos ter ao utilizá-lo.

Primeiro passo é criar um projeto ASP.NET Core Empty, que pode ser verificado no https://github.com/Viniciusdevti/response-cache

Adicionamos o controller e service, para realizar uma simulação.


Adicionamos na classe Program a injeção de dependência para o service e as demais dependências do controller e do cache.

using ResponseCache.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<INumberService, NumberService>();
builder.Services.AddControllers();
builder.Services.AddControllers();
builder.Services.AddResponseCaching();

var app = builder.Build();
app.MapControllers();
app.UseResponseCaching();
app.Run();
O service é bem simples, temos um GetRandomNumber que retorna retorna um número aleatório depois de 2 segundos.

namespace ResponseCache.Service
{
    public class NumberService : INumberService
    {
        public int GetRandomNumber()
        {
            TimeSpan interval = new TimeSpan(0, 0, 2);
            Thread.Sleep(interval);
            Random rnd = new Random();
            int number = rnd.Next();

            return number;
        }
    }
}
No controller, temos a injeção para o service, que com o .net 7, não será necessário adicionar o [FromServices]. Foram criados 3 endpoints que nos mostrarão como podemos melhorar nosso desempenho com o responseCache.

using Microsoft.AspNetCore.Mvc;
using ResponseCache.Service;

namespace ResponseCache.Controllers
{
    public class NumberController : Controller
    {
        [HttpGet("number")]
        public IActionResult Get(NumberService numberService)
        {
            var result = numberService.GetRandomNumber();
            return Ok(result);
        }

        [HttpGet("number/cache/")]
        [ResponseCache(Duration = 5)]
        public IActionResult GetCache(NumberService numberService)
        {
            var result = numberService.GetRandomNumber();
            return Ok(result);
        }

        [HttpGet("number/cache/{userid}")]
        [ResponseCache(Duration = 20, VaryByQueryKeys = new string[] { "userId" })]
        public IActionResult GetCacheWithId(NumberService numberService, int id)
        {
            var result = numberService.GetRandomNumber();
            return Ok(result);
        }
    }
}
1- /number

Ao realizar a request na rota /number sempre é gerado um novo número aleatorio, pois não definimos cache.


/number
2- /number/cache

Ao realizar a request na rota /number/cache só é gerado um novo número aleatório depois de 5 segundos, que foi executada a última request, que foi o tempo definido no duration desse exemplo.

Então se realizamos uma request dentro do tempo de 5 segundos, será retornado o mesmo número que foi gerado antes. Posterior aos 5 segundos, será gerado um novo número aleatório.


/number/cache
3- /number/{userid}

Ao realizar a request na rota /number/{userid} só é gerado um novo número aleatório depois de 20 segundos, que foi executada a última request, para aquela rota e aquele determinado id, que foi o tempo definido no duration desse exemplo.

Então se realizamos uma request dentro no tempo de 20 segundos, para a rota /number/1, será definido um cache de 20 segundos, se realizarmos o acesso na rota /number/2, será gerado um novo cache de 20 segundos e assim em diante.


/number/{userid}
Com isso podemos ganhar um melhor desempenho conforme a necessidade do nosso negócio. Para uma resposta mais rápida.

