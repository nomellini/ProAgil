namespace ProAgil.Domain
{
  public class RedeSocial
  {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string URL { get; set; }
    public int? EventoId { get; set; }
    public Evento Evento { get; }
    public int? PalestranteId { get; set; }
    public Palestrante Palestrante { get; }

  }
}



/*

{
    "Id": 4,
    "Local": "SBC",
    "DataEvento": "2019-08-31T00:00:00",
    "Tema": "Liderança 2",
    "QtdePessoas": 10,
    "ImagemUrl": "img2.jpg",
    "Telefone": "123",
    "Email": "fernando@nomellini.net",
    "Lote": "1o ",
    "Lotes": [
        {
            "Id": 3,
            "Nome": "1o Lode",
            "Preco": 50.5,
            "DataInicio": null,
            "DataFim": null,
            "Quantidade": 125,
            "EventoId": 4,
            "Evento": null
        },
        {
            "Id": 4,
            "Nome": "2o Lode",
            "Preco": 60.5,
            "DataInicio": null,
            "DataFim": null,
            "Quantidade": 125,
            "EventoId": 4,
            "Evento": null
        }
    ],
    "RedesSociais": [
        {
            "Id": 1,
            "Nome": "Twitter",
            "URL": "http://blablabla",
            "EventoId": 4,
            "Evento": null,
            "PalestranteId": null,
            "Palestrante": null
        }
    ],
    "PalestranteEventos": []
}

 */
