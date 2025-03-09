using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RpgApi.Models;
using RpgApi.Models.Enums;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonagensExercicioController : ControllerBase
    {
        private static List<Personagem> personagens = new List<Personagem>()
        {
            new Personagem() { Id = 1, Nome = "Frodo", PontosVida=100, Forca=17, Defesa=23, Inteligencia=33, Classe=ClasseEnum.Cavaleiro},
            new Personagem() { Id = 2, Nome = "Sam", PontosVida=100, Forca=15, Defesa=25, Inteligencia=30, Classe=ClasseEnum.Cavaleiro},
            new Personagem() { Id = 3, Nome = "Galadriel", PontosVida=100, Forca=18, Defesa=21, Inteligencia=35, Classe=ClasseEnum.Clerigo },
            new Personagem() { Id = 4, Nome = "Gandalf", PontosVida=100, Forca=18, Defesa=18, Inteligencia=37, Classe=ClasseEnum.Mago },
            new Personagem() { Id = 5, Nome = "Hobbit", PontosVida=100, Forca=20, Defesa=17, Inteligencia=31, Classe=ClasseEnum.Cavaleiro },
            new Personagem() { Id = 6, Nome = "Celeborn", PontosVida=100, Forca=21, Defesa=13, Inteligencia=34, Classe=ClasseEnum.Clerigo },
            new Personagem() { Id = 7, Nome = "Radagast", PontosVida=100, Forca=25, Defesa=11, Inteligencia=35, Classe=ClasseEnum.Mago }
        };

        [HttpGet("GetByNome/{nome}")]
        public IActionResult GetByNome(string nome)
        {
            var personagem = personagens.FirstOrDefault(p => p.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
            if (personagem == null)
                return NotFound("Personagem não encontrado.");
            return Ok(personagem);
        }

        [HttpGet("GetClerigoMago")]
        public IActionResult GetClerigoMago()
        {
            var listaFiltrada = personagens
            .Where(p => p.Classe != ClasseEnum.Cavaleiro)
            .OrderByDescending(p => p.PontosVida)
            .ToList();

            if (listaFiltrada.Count == 0)
            return NotFound("Nenhum Clérigo ou Mago encontrado.");

            return Ok(listaFiltrada);
        }

        [HttpGet("GetEstatisticas")]
        public IActionResult GetEstatisticas()
        {
            int quantidade = personagens.Count;
            int somaInteligencia = personagens.Sum(p => p.Inteligencia);
            return Ok(new { Quantidade = quantidade, SomaInteligencia = somaInteligencia });
        }

        [HttpPost("PostValidacao")]
        public IActionResult PostValidacao([FromBody] Personagem novoPersonagem)
        {
            if (novoPersonagem.Defesa < 10 || novoPersonagem.Inteligencia > 30)
                return BadRequest("Personagem inválido: Defesa deve ser maior ou igual a 10 e Inteligência menor ou igual a 30.");
            
            personagens.Add(novoPersonagem);
            return Ok(novoPersonagem);
        }

        [HttpPost("PostValidacaoMago")]
        public IActionResult PostValidacaoMago([FromBody] Personagem novoPersonagem)
        {
            if (novoPersonagem.Classe == ClasseEnum.Mago && novoPersonagem.Inteligencia < 35)
                return BadRequest("Magos devem ter Inteligência maior ou igual a 35.");
            
            personagens.Add(novoPersonagem);
            return Ok(novoPersonagem);
        }

        [HttpGet("GetByClasse/{idClasse}")]
        public IActionResult GetByClasse(int idClasse)
        {
            var listaFiltrada = personagens.Where(p => (int)p.Classe == idClasse).ToList();
            return Ok(listaFiltrada);
        }
    }
}
